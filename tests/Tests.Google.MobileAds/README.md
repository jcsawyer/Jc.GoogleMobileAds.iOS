# iOS binding smoke tests

See [Smoke Tests](../../README.md#smoke-tests) for the toolchain and commands.

The C# host restores the iOS test app into an empty temporary NuGet cache, builds it, and uses Apple's `simctl` to
install and launch it. GMA 13.6.1 and UMP 3.1.1 are exact-version package references mapped exclusively to the local
feed. Clean the binding projects before packing after replacing native frameworks locally.

Inside the app, NUnitLite runs twenty-three cases on UIKit's main thread: GMA and UMP SDK versions, consent sync ID
marshalling, skipping uninitialized adapters, age treatment, beta preload and mediation configuration, empty preloader and response-info queries, swipeable interstitial options, three new and three deprecated banner sizing functions, neighboring-content URL marshalling, and removal
of native-ad registration methods. It uses Google's
sample application ID and makes no ad requests. The explicit `HarnessFailureProbe` is skipped normally; the app accepts
NUnitLite command-line options such as `--test=Tests.Google.MobileAds.BindingTests.HarnessFailureProbe` for diagnostics.

The host copies the app's NUnit XML into `artifacts/simulator`, requires a passing report with executed tests, and
attaches it to the desktop test result. Missing results, build errors, failed assertions, crashes, and timeouts fail the
run. The app is uninstalled afterward; a simulator that was already running stays running. No simulator data reset is
requested.

`dotnet test` reports one aggregate desktop test; the attached XML contains the individual iOS cases. Desktop TRX output
is in `artifacts/dotnet-test`. CI uploads both result directories and publishes the same NuGet files only after the test
passes. No Python, XHarness, launcher shim, or Simulator GUI is required.
