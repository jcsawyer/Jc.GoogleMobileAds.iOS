# Jc.GoogleMobileAds.iOS

Google Mobile Ads SDK iOS Bindings

# Smoke Tests

The smoke tests verify the locally built NuGet packages inside an **iOS simulator**. The iOS test app references exactly GMA 13.0.0 and UMP 2.7.5 from the local package feed. An empty temporary NuGet cache ensures the run uses those packages.

There are two projects:

- `tests/Tests.Google.MobileAds`: an iOS app that runs the binding tests using NUnitLite on UIKit's main thread.
- `tests/Tests.Google.MobileAds.Host`: a desktop NUnit project that provides the `dotnet test` entry point. It builds the app, installs and launches it with Apple's `simctl`, collects the results, and checks that the run passed. The binding tests themselves run in the simulator.

The nine iOS cases check the native SDK version, all three new large adaptive banner sizing functions, all three deprecated sizing functions, neighboring-content URL array marshalling, and removal of the native-ad registration methods. The app uses Google's sample application ID and makes no live ad requests. An additional intentional failure probe is skipped during normal runs.

## Run locally

Requires macOS, .NET 10 with the iOS workloads needed by the .NET 9/10 binding targets, Xcode, and an available iPhone simulator running iOS 18 or later. The binding packages still support iOS 15+.

Run from the repository root:

```sh
dotnet workload restore src/jc-google-mobile-ads-ios.sln
dotnet build src/jc-google-mobile-ads-ios.sln -c Release
dotnet pack src/jc-google-mobile-ads-ios.sln -c Release --no-build -o artifacts/smoke-packages
dotnet test tests/Tests.Google.MobileAds.Host/Tests.Google.MobileAds.Host.csproj -c Release --logger trx --results-directory artifacts/dotnet-test
```

Clean the binding projects before rebuilding if you replaced the native frameworks locally.

The flow is: **pack bindings → build iOS test app → launch in simulator → collect and validate NUnit results**. Build errors, crashes, timeouts, missing results, and failed assertions fail the run. Afterward, the host uninstalls the app and shuts down the simulator only if it started it.

## Results and CI

- `artifacts/dotnet-test`: desktop TRX results containing one aggregate test for the complete simulator run.
- `artifacts/simulator/test-results.xml`: NUnit results containing the individual iOS binding cases, also attached to the desktop test result.

CI runs the smoke tests after packaging and uploads both result directories. The release workflow publishes the same packages only after the smoke tests pass.

See the [smoke-test README](tests/Tests.Google.MobileAds/README.md) for diagnostic options.

# Package Showcase

- [marius-bughiu](https://github.com/marius-bughiu) - [Plugin.AdMob](https://github.com/marius-bughiu/Plugin.AdMob) - AdMob plugin for .NET MAUI
- [jcsawyer](https://github.com/jcsawyer) - [Jc.AdMob.Avalonia](https://github.com/jcsawyer/Jc.AdMob.Avalonia) - AdMob plugin for Avalonia mobile projects

# Breaking Changes

Each major version contains breaking changes according to the Google AdMob SDK. Please refer to the [Google AdMob SDK](https://developers.google.com/admob/ios/release-notes) for more information.
The headline changes relevant will be listed here.

## v13.0.0

This package binds **Google Mobile Ads SDK 13.0.0**. It continues to support .NET 9/10 and requires iOS 15 or later. Jc.UMP.iOS remains at 2.7.5.

- `NativeAd.RegisterAdView` (both overloads) and `NativeAd.UnregisterAdView` have been removed with the native SDK. Use `NativeAdView` to render native ads.
- The existing anchored adaptive banner sizing functions are obsolete but retain their original behavior. Use these replacements on the **main thread**:

  | Existing `AdSizeCons` method | Replacement |
  |---|---|
  | `GetCurrentOrientationAnchoredAdaptiveBannerAdSize` | `GetLargeAnchoredAdaptiveBannerAdSize` |
  | `GetPortraitAnchoredAdaptiveBannerAdSize` | `GetLargePortraitAnchoredAdaptiveBannerAdSize` |
  | `GetLandscapeAnchoredAdaptiveBannerAdSize` | `GetLargeLandscapeAnchoredAdaptiveBannerAdSize` |

- Large anchored adaptive banners can be taller; size the banner container from the returned `AdSize`.
- `Request.NeighboringContentUrlStrings` remains a `string[]`. The native SDK now raises an exception for non-string array elements; avoid null elements or passing other object types through native interop.

See Google's [v12 to v13 migration guide](https://developers.google.com/admob/ios/migration#migrate_from_sdk_version_v12_to_v13).

## v12.13.0

Both `NativeAd.RegisterAdView` overloads and `NativeAd.UnregisterAdView` are obsolete, matching the native SDK deprecations. Use `NativeAdView` to render native ads instead. These methods remain available but are no longer supported by Google and will be removed in a future SDK version.

## v12.2.2

`AdSizeCons` constants were changed to be hardcoded while I try to find a better approach to access the true values.
This means that values will be the same for both iPhone and iPad.

`SmartBannerPortrait`, `SmartBannerLandscape`, and `Fluid` have been removed until a workaround is discovered as these are entirely dependent on the framework or the calling framework (MAUI/Avalonia/etc.)

## v9.x.x to v11.x.x

- The minimum supported version of iOS is now 13.
  - Note: SDK 11.0.0 still builds on iOS 12 devices, but ads will not serve.
- armv7 support has been removed from the SDK

From `v11.13.0` and beyond, you must add this target to your csproj file to support linking with Swift system libraries

```xml
<Target Name="LinkWithSwift" DependsOnTargets="_ParseBundlerArguments;_DetectSdkLocations" BeforeTargets="_LinkNativeExecutable">
    <PropertyGroup>
        <_SwiftPlatform Condition="$(RuntimeIdentifier.StartsWith('iossimulator-'))">iphonesimulator</_SwiftPlatform>
        <_SwiftPlatform Condition="$(RuntimeIdentifier.StartsWith('ios-'))">iphoneos</_SwiftPlatform>
    </PropertyGroup>
    <ItemGroup>
        <_CustomLinkFlags Include="-L" />
        <_CustomLinkFlags Include="/usr/lib/swift" />
        <_CustomLinkFlags Include="-L" />
        <_CustomLinkFlags Include="$(_SdkDevPath)/Toolchains/XcodeDefault.xctoolchain/usr/lib/swift/$(_SwiftPlatform)" />
        <_CustomLinkFlags Include="-Wl,-rpath" />
        <_CustomLinkFlags Include="-Wl,/usr/lib/swift" />
    </ItemGroup>
</Target>
```

More info can be found [here](https://developers.google.com/admob/ios/migration#migrate_from_v9_to_v10)

### Usage

To use Jc.GoogleMobileAds.iOS, you can add the required packages to your project:

```
dotnet add package Jc.GMA.iOS
dotnet add package Jc.UMP.iOS
```

Ensure that the `SupportedOSPlatformVersion` attribute in your project file is set to `15.0` or higher:

```xml
<SupportedOSPlatformVersion>15.0</SupportedOSPlatformVersion>
```

along with setting it in the `Info.plist` file:

```xml
<key>MinimumOSVersion</key>
<string>15.0</string>
```

Failing to set the minimum version will result in the compiler warning:

```
Jc.*.iOS requires a minimum SupportedOSPlatformVersion of 15.0. Update your project to target iOS 15 or later.
```
