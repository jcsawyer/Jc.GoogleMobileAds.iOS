using NUnit.Framework;

namespace Tests.Google.MobileAds.Host;

[TestFixture]
public sealed class SimulatorTests
{
    [Test]
    public async Task PackagedBindingsPassOnIosSimulator()
    {
        if (!OperatingSystem.IsMacOS())
        {
            Assert.Ignore("The iOS simulator requires macOS and Xcode.");
        }

        var repo = new DirectoryInfo(TestContext.CurrentContext.TestDirectory);
        while (repo != null && !File.Exists(Path.Combine(repo.FullName, "src", "jc-google-mobile-ads-ios.sln")))
            repo = repo.Parent;
        Assert.That(repo, Is.Not.Null, "Cannot locate the binding solution.");

        using var timeout = new CancellationTokenSource(TimeSpan.FromMinutes(20));
        try
        {
            await new SimulatorSmokeRun(repo!.FullName, timeout.Token).RunAsync();
        }
        finally
        {
            var reports = Path.Combine(repo!.FullName, "artifacts", "simulator");
            if (Directory.Exists(reports))
            {
                foreach (var report in Directory.GetFiles(reports, "*", SearchOption.AllDirectories))
                {
                    TestContext.AddTestAttachment(report);
                }
            }
        }
    }
}