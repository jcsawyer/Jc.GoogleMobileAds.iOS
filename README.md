# Jc.GoogleMobileAds.iOS

Google Mobile Ads SDK iOS Bindings

# Package Showcase

- [marius-bughiu](https://github.com/marius-bughiu) - [Plugin.AdMob](https://github.com/marius-bughiu/Plugin.AdMob) - AdMob plugin for .NET MAUI
- [jcsawyer](https://github.com/jcsawyer) - [Jc.AdMob.Avalonia](https://github.com/jcsawyer/Jc.AdMob.Avalonia) - AdMob plugin for Avalonia mobile projects

# Breaking Changes

Each major version contains breaking changes according to the Google AdMob SDK. Please refer to the [Google AdMob SDK](https://developers.google.com/admob/ios/release-notes) for more information.
The headline changes relevant will be listed here.

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
