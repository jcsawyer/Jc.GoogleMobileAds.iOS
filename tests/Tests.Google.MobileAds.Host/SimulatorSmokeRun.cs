using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Xml.Linq;
using NUnit.Framework;

namespace Tests.Google.MobileAds.Host;

internal sealed class SimulatorSmokeRun(string repository, CancellationToken cancellationToken)
{
    private const string BundleId = "com.jcsawyer.gma.bindingsmoketests";
    private string ProjectDirectory => Path.Combine(repository, "tests", "Tests.Google.MobileAds");
    private string ResultsDirectory => Path.Combine(repository, "artifacts", "simulator");
    private string ResultsFile => Path.Combine(ResultsDirectory, "test-results.xml");

    public async Task RunAsync()
    {
        if (Directory.Exists(ResultsDirectory))
        {
            Directory.Delete(ResultsDirectory, true);
        }

        Directory.CreateDirectory(ResultsDirectory);

        var feed = Path.Combine(repository, "artifacts", "smoke-packages");
        foreach (var package in new[] { "Jc.GMA.iOS.13.0.0.nupkg", "Jc.UMP.iOS.2.7.5.nupkg" })
        {
            Assert.That(File.Exists(Path.Combine(feed, package)), Is.True, $"Pack {package} into {feed} first.");
        }

        var simulator = await FindSimulatorAsync();
        var app = await BuildAppAsync();
        await RunAppAsync(app, simulator);
        AssertPassingResults();
    }

    private async Task<string> BuildAppAsync()
    {
        var rid = RuntimeInformation.ProcessArchitecture switch
        {
            Architecture.Arm64 => "iossimulator-arm64",
            Architecture.X64 => "iossimulator-x64",
            var architecture => throw new PlatformNotSupportedException($"Unsupported simulator host: {architecture}.")
        };
        var project = Path.Combine(ProjectDirectory, "Tests.Google.MobileAds.csproj");
        var cache = Directory.CreateTempSubdirectory("gma-smoke-nuget-");
        try
        {
            await CommandAsync("dotnet", [
                "restore", project, "-r", rid, "--force", "--no-http-cache",
                "--packages", cache.FullName, "--configfile", Path.Combine(ProjectDirectory, "NuGet.config")
            ], 600);
            await CommandAsync("dotnet", [
                "build", project, "-c", "Release", "-r", rid, "--no-restore", "-t:Rebuild",
                $"-p:RestorePackagesPath={cache.FullName}"
            ], 600);
            return Path.Combine(ProjectDirectory, "bin", "Release", "net10.0-ios", rid, "Tests.Google.MobileAds.app");
        }
        finally
        {
            cache.Delete(true);
        }
    }

    private async Task RunAppAsync(string app, Simulator simulator)
    {
        try
        {
            if (!simulator.WasBooted)
            {
                await CommandAsync("xcrun", ["simctl", "boot", simulator.Udid]);
            }

            await CommandAsync("xcrun", ["simctl", "bootstatus", simulator.Udid, "-b"], 180);
            await CommandAsync("xcrun", ["simctl", "install", simulator.Udid, app]);

            var container =
                (await CommandAsync("xcrun", ["simctl", "get_app_container", simulator.Udid, BundleId, "data"])).Trim();
            var report = Path.Combine(container, "Documents", "test-results.xml");
            File.Delete(report); // A previous installation must never supply this run's results.
            try
            {
                await CommandAsync("xcrun",
                    ["simctl", "launch", "--console", "--terminate-running-process", simulator.Udid, BundleId]);
            }
            finally
            {
                if (File.Exists(report))
                {
                    File.Copy(report, ResultsFile, true);
                }
            }
        }
        finally
        {
            await CleanupAsync(["simctl", "uninstall", simulator.Udid, BundleId]);
            if (!simulator.WasBooted)
            {
                await CleanupAsync(["simctl", "shutdown", simulator.Udid]);
            }
        }
    }

    private void AssertPassingResults()
    {
        Assert.That(File.Exists(ResultsFile), Is.True, "The simulator app did not produce NUnit results.");
        var report = XDocument.Load(ResultsFile).Root!;
        Assert.That(report.Name.LocalName, Is.EqualTo("test-run"));
        Assert.That((string?)report.Attribute("result"), Is.EqualTo("Passed"),
            "See the attached simulator NUnit results.");
        Assert.That((int?)report.Attribute("passed"), Is.GreaterThan(0), "No tests passed inside the simulator.");
    }

    private async Task<Simulator> FindSimulatorAsync()
    {
        using var inventory =
            JsonDocument.Parse(await CommandAsync("xcrun", ["simctl", "list", "devices", "available", "--json"]));
        var simulators = new List<Simulator>();
        foreach (var runtime in inventory.RootElement.GetProperty("devices").EnumerateObject())
        {
            var parts = runtime.Name.Split(".iOS-", 2);
            if (parts.Length != 2 || !Version.TryParse(parts[1].Replace('-', '.'), out var version) ||
                version.Major < 18)
            {
                continue;
            }

            foreach (var device in runtime.Value.EnumerateArray())
            {
                if (device.GetProperty("isAvailable").GetBoolean() &&
                    device.GetProperty("name").GetString()!.Contains("iPhone"))
                {
                    simulators.Add(new Simulator(device.GetProperty("udid").GetString()!,
                        device.GetProperty("state").GetString() == "Booted"));
                }
            }
        }

        return simulators.FirstOrDefault(device => device.WasBooted) ?? simulators.FirstOrDefault()
            ?? throw new InvalidOperationException(
                "No available iPhone simulator with iOS 18 or later. Install a runtime in Xcode.");
    }

    private async Task CleanupAsync(string[] arguments)
    {
        try
        {
            await CommandAsync("xcrun", arguments, 30, CancellationToken.None);
        }
        catch (Exception error)
        {
            await TestContext.Out.WriteLineAsync($"Simulator cleanup failed: {error.Message}");
        }
    }

    private async Task<string> CommandAsync(string executable, string[] arguments, int timeoutSeconds = 120,
        CancellationToken? cancellation = null)
    {
        await TestContext.Progress.WriteLineAsync($"+ {executable} {string.Join(" ", arguments)}");
        var start = new ProcessStartInfo(executable)
        {
            WorkingDirectory = repository,
            RedirectStandardOutput = true,
            RedirectStandardError = true
        };
        foreach (var argument in arguments)
        {
            start.ArgumentList.Add(argument);
        }

        using var process = Process.Start(start) ??
                            throw new InvalidOperationException($"Could not start {executable}.");
        var output = process.StandardOutput.ReadToEndAsync();
        var errors = process.StandardError.ReadToEndAsync();
        using var timeout = CancellationTokenSource.CreateLinkedTokenSource(cancellation ?? cancellationToken);
        timeout.CancelAfter(TimeSpan.FromSeconds(timeoutSeconds));
        try
        {
            await process.WaitForExitAsync(timeout.Token);
        }
        catch (OperationCanceledException error)
        {
            if (!process.HasExited)
            {
                process.Kill(true);
            }

            await process.WaitForExitAsync();
            throw new TimeoutException($"{executable} timed out or the smoke run was cancelled.", error);
        }
        finally
        {
            await TestContext.Out.WriteLineAsync(await output);
            await TestContext.Out.WriteLineAsync(await errors);
        }

        if (process.ExitCode != 0)
        {
            throw new InvalidOperationException(
                $"{executable} {string.Join(" ", arguments)} exited with code {process.ExitCode}.");
        }

        return await output;
    }

    private sealed record Simulator(string Udid, bool WasBooted);
}