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
    public void SdkVersionIs1350()
    {
        var version = global::Google.MobileAds.MobileAds.SharedInstance.VersionNumber;
        Assert.That((version.MajorVersion, version.MinorVersion, version.PatchVersion),
            Is.EqualTo(((nint)13, (nint)5, (nint)0)));
    }

    [Test]
    public void SkipUninitializedAdaptersRoundTrips()
    {
        using var request = Request.GetDefaultRequest();
        Assert.That(request.ShouldSkipUninitializedAdapters, Is.False);
        request.ShouldSkipUninitializedAdapters = true;
        Assert.That(request.ShouldSkipUninitializedAdapters, Is.True);
        request.ShouldSkipUninitializedAdapters = false;
        Assert.That(request.ShouldSkipUninitializedAdapters, Is.False);
    }

    [TestCase(AgeRestrictedTreatment.Unspecified)]
    [TestCase(AgeRestrictedTreatment.Child)]
    [TestCase(AgeRestrictedTreatment.Teen)]
    public void AgeRestrictedTreatmentRoundTrips(AgeRestrictedTreatment treatment)
    {
        var configuration = global::Google.MobileAds.MobileAds.SharedInstance.RequestConfiguration;
        var original = configuration.AgeRestrictedTreatment;
        try
        {
            configuration.AgeRestrictedTreatment = treatment;
            Assert.That(configuration.AgeRestrictedTreatment, Is.EqualTo(treatment));
        }
        finally
        {
            configuration.AgeRestrictedTreatment = original;
        }
    }

    [TestCase(false)]
    [TestCase(true)]
    public void PreloadConfigurationRoundTrips(bool explicitRequest)
    {
        const string adUnitId = "ca-app-pub-3940256099942544/4411468910";
        using var request = Request.GetDefaultRequest();
        request.NeighboringContentUrlStrings = ["https://example.com/preload"];
        using var configuration = explicitRequest
            ? new PreloadConfigurationV2(adUnitId, request)
            : new PreloadConfigurationV2(adUnitId);
        Assert.That(configuration.AdUnitID, Is.EqualTo(adUnitId));
        Assert.That(configuration.Request, Is.Not.Null);
        if (explicitRequest)
        {
            Assert.That(configuration.Request.NeighboringContentUrlStrings,
                Is.EqualTo(request.NeighboringContentUrlStrings));
        }
        configuration.BufferSize = 2;
        Assert.That(configuration.BufferSize, Is.EqualTo((nuint)2));
    }

    [Test]
    public void MediationInitializationConfigurationRoundTrips()
    {
        using var configuration = new MediationInitializationConfiguration();
        using var format = NSNumber.FromInt64((long)AdFormat.Interstitial);
        using var formats = new NSSet<NSNumber>(format);
        using var adUnit = new NSString("ca-app-pub-3940256099942544/4411468910");
        using var adUnits = new NSSet<NSString>(adUnit);
        using var adapter = new NSString("ExampleAdapter");
        using var adapters = new NSSet<NSString>(adapter);
        configuration.IncludedAdFormats = formats;
        configuration.IncludedAdUnitIDs = adUnits;
        configuration.IncludedAdapterClasses = adapters;
        configuration.ExcludedAdapterClasses = adapters;
        configuration.InitializationTimeout = 5;
        Assert.That(configuration.IncludedAdFormats.Contains(format), Is.True);
        Assert.That(configuration.IncludedAdUnitIDs.Contains(adUnit), Is.True);
        Assert.That(configuration.IncludedAdapterClasses.Contains(adapter), Is.True);
        Assert.That(configuration.ExcludedAdapterClasses.Contains(adapter), Is.True);
        Assert.That(configuration.InitializationTimeout, Is.EqualTo(5));
        Assert.That(global::Google.MobileAds.MobileAds.SharedInstance.RespondsToSelector(
            new ObjCRuntime.Selector("startWithMediationInitializationConfiguration:completionHandler:")), Is.True);
    }

    [TestCase("AppOpen")]
    [TestCase("Interstitial")]
    [TestCase("Rewarded")]
    [TestCase("RewardedInterstitial")]
    public void PreloaderHasNoAdsForUnknownId(string format)
    {
        var id = Guid.NewGuid().ToString();
        switch (format)
        {
            case "AppOpen":
                var appOpen = AppOpenAdPreloader.SharedInstance;
                AssertEmptyPreloader(appOpen, appOpen.IsAdAvailable(id), appOpen.GetNumberOfAdsAvailable(id),
                    appOpen.GetAd(id), appOpen.GetAdResponseInfo(id), appOpen.GetConfiguration(id), appOpen.Configurations.ContainsKey(new NSString(id)));
                break;
            case "Interstitial":
                var interstitial = InterstitialAdPreloader.SharedInstance;
                AssertEmptyPreloader(interstitial, interstitial.IsAdAvailable(id), interstitial.GetNumberOfAdsAvailable(id),
                    interstitial.GetAd(id), interstitial.GetAdResponseInfo(id), interstitial.GetConfiguration(id), interstitial.Configurations.ContainsKey(new NSString(id)));
                break;
            case "Rewarded":
                var rewarded = RewardedAdPreloader.SharedInstance;
                AssertEmptyPreloader(rewarded, rewarded.IsAdAvailable(id), rewarded.GetNumberOfAdsAvailable(id),
                    rewarded.GetAd(id), rewarded.GetAdResponseInfo(id), rewarded.GetConfiguration(id), rewarded.Configurations.ContainsKey(new NSString(id)));
                break;
            case "RewardedInterstitial":
                var rewardedInterstitial = RewardedInterstitialAdPreloader.SharedInstance;
                AssertEmptyPreloader(rewardedInterstitial, rewardedInterstitial.IsAdAvailable(id), rewardedInterstitial.GetNumberOfAdsAvailable(id),
                    rewardedInterstitial.GetAd(id), rewardedInterstitial.GetAdResponseInfo(id), rewardedInterstitial.GetConfiguration(id), rewardedInterstitial.Configurations.ContainsKey(new NSString(id)));
                break;
        }
    }

    private static void AssertEmptyPreloader(NSObject preloader, bool available, nuint count,
        NSObject? ad, ResponseInfo? responseInfo, PreloadConfigurationV2? configuration, bool configured)
    {
        Assert.That(available, Is.False);
        Assert.That(count, Is.EqualTo((nuint)0));
        Assert.That(ad, Is.Null);
        Assert.That(responseInfo, Is.Null);
        Assert.That(configuration, Is.Null);
        Assert.That(configured, Is.False);
        foreach (var selector in new[] { "preloadForPreloadID:configuration:delegate:",
                     "stopPreloadingAndRemoveAdsForPreloadID:", "stopPreloadingAndRemoveAllAds" })
        {
            Assert.That(preloader.RespondsToSelector(new ObjCRuntime.Selector(selector)), Is.True, selector);
        }
    }

    [Test]
    public void SwipeableInterstitialOptionsRoundTrip()
    {
        using var options = new SwipeableInterstitialAdOptions();
        Assert.That(options.MaxScreenHoldDuration, Is.EqualTo(0));
        Assert.That(options.CustomClickSwipeGestureTapsAllowed, Is.True);
        options.MaxScreenHoldDuration = 5;
        options.AdSize = new CoreGraphics.CGSize(320, 480);
        Assert.That(options.MaxScreenHoldDuration, Is.EqualTo(5));
        Assert.That(options.AdSize, Is.EqualTo(new CoreGraphics.CGSize(320, 480)));
        options.EnableCustomClickSwipeGesture(UIKit.UISwipeGestureRecognizerDirection.Up, false);
        Assert.That(options.CustomClickSwipeGestureDirection, Is.EqualTo(UIKit.UISwipeGestureRecognizerDirection.Up));
        Assert.That(options.CustomClickSwipeGestureTapsAllowed, Is.False);
        options.EnableCustomClickSwipeGesture(UIKit.UISwipeGestureRecognizerDirection.Down, true);
        Assert.That(options.CustomClickSwipeGestureDirection, Is.EqualTo(UIKit.UISwipeGestureRecognizerDirection.Down));
        Assert.That(options.CustomClickSwipeGestureTapsAllowed, Is.True);
        Assert.That(ObjCRuntime.Class.GetHandle("GADSwipeableInterstitialAd"), Is.Not.EqualTo(IntPtr.Zero));
    }

    [Test]
    public void UmpSdkVersionIs310()
    {
        Assert.That(global::Google.UserMessagingPlatform.ConsentInformation.UMPVersionString.ToString(),
            Is.EqualTo("3.1.0"));
    }

    [Test]
    public void ConsentSyncIdRoundTrips()
    {
        using var parameters = new global::Google.UserMessagingPlatform.RequestParameters();
        Assert.That(parameters.ConsentSyncId, Is.Null);
        var id = Guid.NewGuid().ToString();
        parameters.ConsentSyncId = id;
        Assert.That(parameters.ConsentSyncId, Is.EqualTo(id));
        parameters.ConsentSyncId = null;
        Assert.That(parameters.ConsentSyncId, Is.Null);
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
