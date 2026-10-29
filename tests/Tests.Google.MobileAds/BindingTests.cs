using Google.MobileAds;
using NUnit.Framework;

namespace Tests.Google.MobileAds;

[TestFixture]
[NonParallelizable]
public sealed class BindingTests
{
    [SetUp]
    public void RequireMainThread()
    {
        Assert.That(NSThread.IsMain, Is.True);
    }

    [Test]
    [Explicit("Used only to verify that a failing simulator test fails the host command.")]
    public void HarnessFailureProbe()
    {
        Assert.Fail("Intentional harness failure probe.");
    }

    [Test]
    public void SdkVersionIs1310()
    {
        var version = global::Google.MobileAds.MobileAds.SharedInstance.VersionNumber;
        Assert.That((version.MajorVersion, version.MinorVersion, version.PatchVersion),
            Is.EqualTo(((nint)13, (nint)1, (nint)0)));
    }

    [TestCase("Current")]
    [TestCase("Portrait")]
    [TestCase("Landscape")]
    public void LargeAdaptiveBannerSizeIsValid(string orientation)
    {
        var size = orientation switch
        {
            "Portrait" => AdSizeCons.GetLargePortraitAnchoredAdaptiveBannerAdSize(320),
            "Landscape" => AdSizeCons.GetLargeLandscapeAnchoredAdaptiveBannerAdSize(320),
            _ => AdSizeCons.GetLargeAnchoredAdaptiveBannerAdSize(320)
        };
        AssertValidSize(size);
    }

    [TestCase("Current")]
    [TestCase("Portrait")]
    [TestCase("Landscape")]
    public void DeprecatedAdaptiveBannerSizeStillWorks(string orientation)
    {
#pragma warning disable CS0618 // Exercise the deprecated native entry points.
        var size = orientation switch
        {
            "Portrait" => AdSizeCons.GetPortraitAnchoredAdaptiveBannerAdSize(320),
            "Landscape" => AdSizeCons.GetLandscapeAnchoredAdaptiveBannerAdSize(320),
            _ => AdSizeCons.GetCurrentOrientationAnchoredAdaptiveBannerAdSize(320)
        };
#pragma warning restore CS0618
        AssertValidSize(size);
    }

    [Test]
    public void NeighboringContentUrlsRoundTrip()
    {
        using var request = Request.GetDefaultRequest();
        string[] urls = ["https://example.com/one", "https://example.com/two"];
        request.NeighboringContentUrlStrings = urls;
        Assert.That(request.NeighboringContentUrlStrings, Is.EqualTo(urls));
    }

    [Test]
    public void RemovedNativeAdRegistrationMethodsAreAbsent()
    {
        Assert.That(typeof(NativeAd).GetMethods().Select(method => method.Name),
            Has.None.EqualTo("RegisterAdView").And.None.EqualTo("UnregisterAdView"));
    }

    private static void AssertValidSize(AdSize size)
    {
        Assert.That(AdSizeCons.IsAdSizeValid(size), Is.True);
        Assert.That((double)size.Size.Width, Is.EqualTo(320));
        Assert.That((double)size.Size.Height, Is.GreaterThan(0));
    }
}