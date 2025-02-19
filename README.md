# Jc.GoogleMobileAds.iOS

Google Mobile Ads SDK iOS Bindings

# Breaking Changes

Each major version contains breaking changes according to the Google AdMob SDK. Please refer to the [Google AdMob SDK](https://developers.google.com/admob/ios/release-notes) for more information.
The headline changes relevant will be listed here.

## v9.x.x to v11.x.x

- The minimum supported version of iOS is now 13.
  - Note: SDK 11.0.0 still builds on iOS 12 devices, but ads will not serve.
- armv7 support has been removed from the SDK

More info can be found [here](https://developers.google.com/admob/ios/migration#migrate_from_v9_to_v10)

### Usage

To use Jc.GoogleMobileAds.iOS, you can add the required packages to your project:

```
dotnet add package Jc.GMA.iOS
dotnet add package Jc.UMP.iOS
```

To use the latest `v11.13.0`, you must add this target to your csproj file to support linking with Swift system libraries

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
