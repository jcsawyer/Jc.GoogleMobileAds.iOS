using NUnit.Common;
using NUnitLite;

namespace Tests.Google.MobileAds;

internal static class Program
{
    private static void Main(string[] args)
    {
        UIApplication.Main(args, null, typeof(AppDelegate));
    }
}

[Register("AppDelegate")]
public sealed class AppDelegate : UIApplicationDelegate
{
    public override UIWindow? Window { get; set; }

    public override bool FinishedLaunching(UIApplication application, NSDictionary? launchOptions)
    {
        Window = new UIWindow(UIScreen.MainScreen.Bounds)
        {
            RootViewController = new UIViewController()
        };
        Window.MakeKeyAndVisible();
        BeginInvokeOnMainThread(RunTests);
        return true;
    }

    private static void RunTests()
    {
        try
        {
            var results = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "test-results.xml");
            var options = new List<string> { "--mainthread", $"--result={results};format=nunit3" };
            options.AddRange(NSProcessInfo.ProcessInfo.Arguments.Skip(1));
            // The default overload reads Console.In, which is unsupported on iOS.
            var exitCode = new AutoRun(typeof(BindingTests).Assembly).Execute(
                options.ToArray(), new ExtendedTextWrapper(Console.Out), TextReader.Null);
            Console.Out.Flush();
            Environment.Exit(exitCode);
        }
        catch (Exception error)
        {
            Console.WriteLine(error);
            Console.Out.Flush();
            Environment.Exit(1);
        }
    }
}