using System;
using System.Collections.Generic;

using CoreGraphics;
using Foundation;
using ObjCRuntime;
using StoreKit;
using UIKit;
using WebKit;

namespace Google.MobileAds
{
    #region CustomLib
    // This is a custom class created by me and is not part of Google Admob lib
    // But it is necesary for this binding to work
    [Static]
    interface AdSizeCons
    {
        
    }
    #endregion

    // typedef void (^GADInitializationCompletionHandler)(GADInitializationStatus * _Nonnull);
    delegate void InitializationCompletionHandler(InitializationStatus status);
    // typedef void (^GADAdInspectorCompletionHandler)(NSError *_Nullable error);
    delegate void AdInspectorCompletionHandler(NSError error);

    // @interface GADMobileAds : NSObject
    [DisableDefaultCtor]
    [BaseType(typeof(NSObject), Name = "GADMobileAds")]
    interface MobileAds
    {
        // + (GADMobileAds *)sharedInstance;
        [Static]
        [Export("sharedInstance")]
        MobileAds SharedInstance { get; }

        // @property(nonatomic, nonnull, readonly) NSString *sdkVersion;
        [Obsolete("Use VersionNumber property instead.")]
        [Export("sdkVersion")]
        string SdkVersion { get; }
        
        // @property (nonatomic, readonly) GADVersionNumber versionNumber;
        [Export("versionNumber")]
        VersionNumber VersionNumber { get; }

        // @property(nonatomic, assign) float applicationVolume;
        [Export("applicationVolume", ArgumentSemantic.Assign)]
        float ApplicationVolume { get; set; }

        // @property(nonatomic, assign) BOOL applicationMuted;
        [Export("applicationMuted", ArgumentSemantic.Assign)]
        bool ApplicationMuted { get; set; }

        // @property(nonatomic, readonly, strong) GADAudioVideoManager *audioVideoManager;
        [Export("audioVideoManager", ArgumentSemantic.Strong)]
        AudioVideoManager AudioVideoManager { get; }

        // @property(nonatomic, readonly, strong) GADRequestConfiguration *requestConfiguration;
        [Export("requestConfiguration", ArgumentSemantic.Strong)]
        RequestConfiguration RequestConfiguration { get; }

        // @property (readonly, nonatomic) GADInitializationStatus * _Nonnull initializationStatus;
        [Export("initializationStatus")]
        InitializationStatus InitializationStatus { get; }

        // - (BOOL)isSDKVersionAtLeastMajor:(NSInteger)major minor:(NSInteger)minor patch:(NSInteger)patch;
        [Export("isSDKVersionAtLeastMajor:minor:patch:")]
        bool IsSdkVersionAtLeast(nint major, nint minor, nint patch);

        // -(void)startWithCompletionHandler:(GADInitializationCompletionHandler _Nullable)completionHandler;
        [Async]
        [Export("startWithCompletionHandler:")]
        void Start([NullAllowed] InitializationCompletionHandler completionHandler);

        // -(void)disableSDKCrashReporting;
        [Export("disableSDKCrashReporting")]
        void DisableSdkCrashReporting();

        // -(void)disableMediationInitialization;
        [Export("disableMediationInitialization")]
        void DisableMediationInitialization();
        
        // - (void)presentAdInspectorFromViewController: (nullable UIViewController *)viewController completionHandler: (nullable GADAdInspectorCompletionHandler)completionHandler;
        [Export("presentAdInspectorFromViewController:completionHandler:")]
        void PresentAdInspectorFromViewController([NullAllowed] UIViewController viewController, [NullAllowed] AdInspectorCompletionHandler completionHandler);
        
        // - (void)registerWebView:(nonnull WKWebView *)webView;
        [Export("registerWebView:")]
        void RegisterWebView(WKWebView webView);
    }

    // @interface GADMultipleAdsAdLoaderOptions : GADAdLoaderOptions
    [BaseType(typeof(AdLoaderOptions), Name = "GADMultipleAdsAdLoaderOptions")]
    interface MultipleAdsAdLoaderOptions
    {
        // @property(nonatomic) NSInteger numberOfAds;
        [Export("numberOfAds")]
        nint NumberOfAds { get; set; }
    }

    interface IAdNetworkExtras
    {

    }

    [Model]
    [Protocol]
    [BaseType(typeof(NSObject), Name = "GADAdNetworkExtras")]
    interface AdNetworkExtras
    {

    }

    // @interface GADAdReward : NSObject
    [DisableDefaultCtor]
    [BaseType(typeof(NSObject), Name = "GADAdReward")]
    interface AdReward
    {
        // @property(nonatomic, readonly, nonnull) NSString *type;
        [Export("type")]
        string Type { get; }

        // @property(nonatomic, readonly, nonnull) NSDecimalNumber *amount;
        [Export("amount")]
        NSDecimalNumber Amount { get; }

        // -(instancetype)initWithRewardType:(NSString *)rewardType rewardAmount:(NSDecimalNumber *)rewardAmount __attribute__((objc_designated_initializer));
        [DesignatedInitializer]
        [Export("initWithRewardType:rewardAmount:")]
        NativeHandle Constructor(string rewardType, NSDecimalNumber rewardAmount);
    }

    [BaseType(typeof(UIView),
        Name = "GADBannerView",
        Delegates = new string[] { "Delegate", "AdSizeDelegate" },
        Events = new Type[] { typeof(BannerViewDelegate), typeof(AdSizeDelegate) })]
    interface BannerView
    {
        [Export("initWithAdSize:origin:")]
        NativeHandle Constructor(AdSize size, CGPoint origin);

        [Export("initWithAdSize:")]
        NativeHandle Constructor(AdSize size);

        [NullAllowed]
        [Export("adUnitID", ArgumentSemantic.Copy)]
        string AdUnitId { get; set; }

        [NullAllowed]
        [Export("rootViewController", ArgumentSemantic.Weak)]
        UIViewController RootViewController { get; set; }

        [Export("adSize", ArgumentSemantic.Assign)]
        AdSize AdSize { get; set; }

        [NullAllowed]
        [Export("delegate", ArgumentSemantic.Weak)]
        IBannerViewDelegate Delegate { get; set; }

        // @property(nonatomic, weak, GAD_NULLABLE) IBOutlet id<GADAdSizeDelegate> adSizeDelegate;
        [NullAllowed]
        [Export("adSizeDelegate", ArgumentSemantic.Weak)]
        IAdSizeDelegate AdSizeDelegate { get; set; }

        [Export("loadRequest:")]
        void LoadRequest([NullAllowed] Request request);

        [Export("autoloadEnabled", ArgumentSemantic.Assign)]
        bool AutoloadEnabled { [Bind("isAutoloadEnabled")] get; set; }

        // @property (readonly, nonatomic) GADResponseInfo * _Nullable responseInfo;
        [NullAllowed]
        [Export("responseInfo")]
        ResponseInfo ResponseInfo { get; }

        // @property (copy, nonatomic) GADPaidEventHandler _Nullable paidEventHandler;
        [NullAllowed]
        [Export("paidEventHandler", ArgumentSemantic.Copy)]
        PaidEventHandler PaidEventHandler { get; set; }
        
        // @property (nonatomic, readonly) BOOL isCollapsible;
        [Export("isCollapsible")]
        bool IsCollapsible { get; }
    }

    interface IBannerViewDelegate
    {

    }

    [Model]
    [Protocol]
    [BaseType(typeof(NSObject), Name = "GADBannerViewDelegate")]
    interface BannerViewDelegate
    {

        [EventArgs("BannerViewE")]
        [EventName("AdReceived")]
        [Export("bannerViewDidReceiveAd:")]
        void DidReceiveAd(BannerView view);

        [EventArgs("BannerViewError")]
        [EventName("ReceiveAdFailed")]
        [Export("bannerView:didFailToReceiveAdWithError:")]
        void DidFailToReceiveAd(BannerView view, NSError error);

        [EventArgs("BannerViewE")]
        [EventName("ImpressionRecorded")]
        [Export("bannerViewDidRecordImpression:")]
        void DidRecordImpression(BannerView view);

        [EventArgs("BannerViewE")]
        [EventName("ClickRecorded")]
        [Export("bannerViewDidRecordClick:")]
        void DidRecordClick(BannerView view);

        [EventArgs("BannerViewE")]
        [Export("bannerViewWillPresentScreen:")]
        void WillPresentScreen(BannerView adView);

        [EventArgs("BannerViewE")]
        [Export("bannerViewWillDismissScreen:")]
        void WillDismissScreen(BannerView adView);

        [EventArgs("BannerViewE")]
        [EventName("ScreenDismissed")]
        [Export("bannerViewDidDismissScreen:")]
        void DidDismissScreen(BannerView adView);
    }

    [BaseType(typeof(NSObject), Name = "GADExtras")]
    interface Extras : AdNetworkExtras
    {

        [NullAllowed]
        [Export("additionalParameters", ArgumentSemantic.Copy)]
        NSDictionary AdditionalParameters { get; set; }
    }

    // typedef void (^GADInterstitialAdLoadCompletionHandler)(GADInterstitialAd * _Nullable, NSError * _Nullable);
    delegate void InterstitialAdLoadCompletionHandler([NullAllowed] InterstitialAd interstitialAd, [NullAllowed] NSError error);
    // typedef void (^GADRewardedInterstitialAdLoadCompletionHandler)(GADRewardedInterstitialAd * _Nullable, NSError * _Nullable);
    delegate void RewardedInterstitialAdLoadCompletionHandler([NullAllowed] RewardedInterstitialAd rewardedInterstitialAd, [NullAllowed] NSError error);

    [DisableDefaultCtor]
    [BaseType(typeof(FullScreenPresentingAd), Name = "GADInterstitialAd")]
    interface InterstitialAd
    {
        // + (void)loadWithAdUnitID:(nonnull NSString *)adUnitID request:(nullable GADRequest *)request completionHandler:(nonnull GADInterstitialAdLoadCompletionHandler)completionHandler;
        [Async]
        [Static]
        [Export("loadWithAdUnitID:request:completionHandler:")]
        void Load(string adUnitId, [NullAllowed] Request request, InterstitialAdLoadCompletionHandler completionHandler);

        // @property(nonatomic, readonly, nonnull) NSString *adUnitID;
        [Export("adUnitID")]
        string AdUnitId { get; }

        // @property (readonly, nonatomic) GADResponseInfo * _Nullable responseInfo;
        [NullAllowed]
        [Export("responseInfo")]
        ResponseInfo ResponseInfo { get; }

        // @property(nonatomic, nullable, copy) GADPaidEventHandler paidEventHandler;
        [NullAllowed]
        [Export("paidEventHandler", ArgumentSemantic.Copy)]
        PaidEventHandler PaidEventHandler { get; set; }

        // -(BOOL)canPresentFromRootViewController:(UIViewController * _Nonnull)rootViewController error:(NSError * _Nullable * _Nullable)error;
        [Export("canPresentFromRootViewController:error:")]
        bool CanPresent([NullAllowed] UIViewController rootViewController, [NullAllowed] out NSError error);

        // - (void)presentFromRootViewController:(nonnull UIViewController *)rootViewController;
        [Export("presentFromRootViewController:")]
        void Present([NullAllowed] UIViewController rootViewController);
    }

    // @interface GADRewardedInterstitialAd : NSObject <GADAdMetadataProvider, GADFullScreenPresentingAd>
    [BaseType(typeof(FullScreenPresentingAd), Name = "GADRewardedInterstitialAd")]
    interface RewardedInterstitialAd : AdMetadataProvider
    {
        // + (void)loadWithAdUnitID:(nonnull NSString *)adUnitID request:(nullable GADRequest *)request completionHandler:(nonnull GADRewardedInterstitialAdLoadCompletionHandler)completionHandler;
        [Async]
        [Static]
        [Export("loadWithAdUnitID:request:completionHandler:")]
        void Load(string adUnitId, [NullAllowed] Request request, RewardedInterstitialAdLoadCompletionHandler completionHandler);

        // @property (readonly, nonatomic) NSString * _Nonnull adUnitID;
        [Export("adUnitID")]
        string AdUnitId { get; }

        // @property (readonly, nonatomic) GADResponseInfo * _Nullable responseInfo;
        [NullAllowed]
        [Export("responseInfo")]
        ResponseInfo ResponseInfo { get; }

        // @property (nonatomic, readonly, nonnull) GADAdReward *adReward;
        [Export("adReward")]
        AdReward Reward { get; }

        // @property (nonatomic, weak, nullable) id<GADFullScreenContentDelegate> fullScreenContentDelegate;
        [Export("fullScreenContentDelegate")]
        FullScreenContentDelegate FullScreenContentDelegate { get; set; }

        // @property (nonatomic, nullable) GADServerSideVerificationOptions *serverSideVerificationOptions;
        [NullAllowed]
        [Export("serverSideVerificationOptions")]
        ServerSideVerificationOptions ServerSideVerificationOptions { get; set; }

        // @property (copy, nonatomic) GADPaidEventHandler _Nullable paidEventHandler;
        [NullAllowed]
        [Export("paidEventHandler", ArgumentSemantic.Copy)]
        PaidEventHandler PaidEventHandler { get; set; }

        // -(BOOL)canPresentFromRootViewController:(UIViewController * _Nonnull)rootViewController error:(NSError * _Nullable * _Nullable)error;
        [Export("canPresentFromRootViewController:error:")]
        bool CanPresent([NullAllowed] UIViewController rootViewController, [NullAllowed] out NSError error);

        // - (void)presentFromRootViewController:(nullable UIViewController *)viewController userDidEarnRewardHandler:(nonnull GADUserDidEarnRewardHandler)userDidEarnRewardHandler;
        [Export("presentFromRootViewController:userDidEarnRewardHandler:")]
        void Present([NullAllowed] UIViewController viewController, GADUserDidEarnRewardHandler @delegate);

        [Export("adMetadata")]
        NSDictionary<NSString, NSObject> AdMetadata { get; }
    }

    // @interface GADMediaContent : NSObject
    [BaseType(typeof(NSObject), Name = "GADMediaContent")]
    interface MediaContent
    {
        // @property (readonly, nonatomic) GADVideoController * _Nonnull videoController;
        [Export("videoController")]
        VideoController VideoController { get; }

        // @property (readonly, nonatomic) BOOL hasVideoContent;
        [Export("hasVideoContent")]
        bool HasVideoContent { get; }

        // @property(nonatomic, readonly) CGFloat aspectRatio;
        [Export("aspectRatio")]
        nfloat AspectRatio { get; }

        // @property (readonly, nonatomic) NSTimeInterval duration;
        [Export("duration")]
        double Duration { get; }

        // @property (readonly, nonatomic) NSTimeInterval currentTime;
        [Export("currentTime")]
        double CurrentTime { get; }

        /// 
        /// From GADMediaContent (NativeAd) category
        /// 

        // @property (nonatomic) UIImage * _Nullable mainImage;
        [NullAllowed]
        [Export("mainImage", ArgumentSemantic.Assign)]
        UIImage MainImage { get; set; }
    }

    [DisableDefaultCtor]
    [BaseType(typeof(NSObject), Name = "GADRequest")]
    interface Request : INSCopying
    {
        [Static]
        [Export("request")]
        Request GetDefaultRequest();

        [Export("registerAdNetworkExtras:")]
        void RegisterAdNetworkExtras(IAdNetworkExtras extras);

        [Export("adNetworkExtrasFor:")]
        IAdNetworkExtras AdNetworkExtrasFor([NullAllowed] Class aClass);

        [Export("removeAdNetworkExtrasFor:")]
        void RemoveAdNetworkExtrasFor(Class aClass);
        
        [NullAllowed]
        [Export("scene", ArgumentSemantic.Weak)]
        UIWindowScene Scene { get; set; }

        [Export("setLocationWithLatitude:longitude:accuracy:")]
        void SetLocation(nfloat latitude, nfloat longitude, nfloat accuracyInMeters);

        [NullAllowed]
        [Export("keywords", ArgumentSemantic.Copy)]
        string[] Keywords { get; set; }

        [NullAllowed]
        [Export("contentURL", ArgumentSemantic.Copy)]
        string ContentUrl { get; set; }

        [NullAllowed]
        [Export("neighboringContentURLStrings", ArgumentSemantic.Copy)]
        string[] NeighboringContentUrlStrings { get; set; }

        [NullAllowed]
        [Export("requestAgent", ArgumentSemantic.Copy)]
        string RequestAgent { get; set; }
        
        [NullAllowed]
        [Export("customTargeting", ArgumentSemantic.Copy)]
        NSDictionary<NSString, NSObject> CustomTargeting { get; set; }
    }

    [Static]
    interface MaxAdContentRatingConstants
    {
        // GAD_EXTERN GADMaxAdContentRating _Nonnull const GADMaxAdContentRatingGeneral;
        [Field("GADMaxAdContentRatingGeneral", "__Internal")]
        NSString General { get; }

        // GAD_EXTERN GADMaxAdContentRating _Nonnull const GADMaxAdContentRatingParentalGuidance;
        [Field("GADMaxAdContentRatingParentalGuidance", "__Internal")]
        NSString ParentalGuidance { get; }

        // GAD_EXTERN GADMaxAdContentRating _Nonnull const GADMaxAdContentRatingTeen;
        [Field("GADMaxAdContentRatingTeen", "__Internal")]
        NSString Teen { get; }

        // GAD_EXTERN GADMaxAdContentRating _Nonnull const GADMaxAdContentRatingMatureAudience;
        [Field("GADMaxAdContentRatingMatureAudience", "__Internal")]
        NSString MatureAudience { get; }
    }

    // @interface GADRequestConfiguration : NSObject
    [BaseType(typeof(NSObject), Name = "GADRequestConfiguration")]
    interface RequestConfiguration
    {
        // @property(nonatomic, strong, nullable) GADMaxAdContentRating maxAdContentRating;
        [NullAllowed]
        [Export("maxAdContentRating", ArgumentSemantic.Strong)]
        string MaxAdContentRating { get; set; }

        // @property (copy, nonatomic) NSArray<NSString *> * _Nullable testDeviceIdentifiers;
        [NullAllowed, Export("testDeviceIdentifiers", ArgumentSemantic.Copy)]
        string[] TestDeviceIdentifiers { get; set; }

        // @property(nonatomic, nullable, copy) NSNumber *tagForUnderAgeOfConsent;
        /// <summary>
        /// <para>
        /// If you set this property with <c>1</c>, a TFUA parameter will be included in all ad requests, and
        /// you are indicating that you want ad requests to be handled in a manner suitable for users under
        /// the age of consent. This parameter disables personalized advertising, including remarketing, for
        /// all ad requests. It also disables requests to third-party ad vendors, such as ad measurement
        /// pixels and third-party ad servers.
        ///</para>
        /// <para>
        /// If you set this property with <c>0</c>, you are indicating that you don't want ad requests to be
        /// handled in a manner suitable for users under the age of consent.
        /// </para>
        /// <para>
        /// If you leave or reset this property as <c>null</c>, ad requests will include no indication
        /// of how you would like your ad requests to be handled in a manner suitable for users under the
        /// age of consent.
        /// </para>
        /// </summary>
        [NullAllowed]
        [Export("tagForUnderAgeOfConsent", ArgumentSemantic.Copy)]
        NSNumber TagForUnderAgeOfConsent { get; set; }
        
        // @property(nonatomic, nullable, copy) NSNumber *tagForChildDirectedTreatment;
        /// <summary>
        /// <para>
        /// [Optional] This property indicates whether you would like your app to be treated as
        /// child-directed for purposes of the Children’s Online Privacy Protection Act (COPPA),
        /// https://www.ftc.gov/business-guidance/privacy-security/childrens-privacy.
        /// </para>
        ///<para>
        /// If you set this property with <c>1</c>, you are indicating that your app should be treated as
        /// child-directed for purposes of the Children’s Online Privacy Protection Act (COPPA).
        /// </para>
        ///<para>
        /// If you set this property with <c>0</c>, you are indicating that your app should not be treated as
        /// child-directed for purposes of the Children’s Online Privacy Protection Act (COPPA).
        /// </para>
        ///<para>
        /// If you leave or reset this property as nil or unknown, ad requests will include no indication of
        /// how you would like your app treated with respect to COPPA.
        /// </para>
        /// <para>
        /// By setting this property, you certify that this notification is accurate and you are authorized
        /// to act on behalf of the owner of the app. You understand that abuse of this setting may result
        /// in termination of your Google account.
        /// </para>
        /// </summary>
        [NullAllowed]
        [Export("tagForChildDirectedTreatment", ArgumentSemantic.Copy)]
        NSNumber TagForChildDirectedTreatment { get; set; }

        // -(void)tagForUnderAgeOfConsent:(BOOL)underAgeOfConsent;
        [Obsolete("This method is deprecated. Use the TagForUnderAgeOfConsent property instead.")]
        [Export("tagForUnderAgeOfConsent:")]
        void UnderAgeofConsent(bool underAgeOfConsent);

        // -(void)tagForChildDirectedTreatment:(BOOL)childDirectedTreatment;
        [Obsolete("This method is deprecated. Use the TagForChildDirectedTreatment property instead.")]
        [Export("tagForChildDirectedTreatment:")]
        void ChildDirectedTreatment(bool childDirectedTreatment);

        // - (void)setSameAppKeyEnabled:(BOOL)enabled;
        [Obsolete("This method is deprecated. Use the SetPublisherFirstPartyIDEnabled: method instead.")]
        [Export("setSameAppKeyEnabled:")]
        void SameAppKeyEnabled(bool enabled);
        
        // @property(nonatomic) GADPublisherPrivacyPersonalizationState publisherPrivacyPersonalizationState;
        [Export("publisherPrivacyPersonalizationState", ArgumentSemantic.Assign)]
        PublisherPrivacyPersonalizationState PublisherPrivacyPersonalizationState { get; set; }
        
        // - (void)setPublisherFirstPartyIDEnabled:(BOOL)enabled;
        [Export("setPublisherFirstPartyIDEnabled:")]
        void SetPublisherFirstPartyIDEnabled(bool enabled);
    }

    // @interface GADAdNetworkResponseInfo : NSObject
    [BaseType(typeof(NSObject), Name = "GADAdNetworkResponseInfo")]
    interface AdNetworkResponseInfo
    {
        // @property(nonatomic, readonly, nonnull) NSString *adNetworkClassName;        
        [Export("responseIdentifier")]
        string AdNetworkClassName { get; }

        // @property(nonatomic, readonly, nonnull) NSDictionary<NSString *, id> *adUnitMapping;       
        [Export("adUnitMapping")]
        NSDictionary<NSString, NSObject> AdUnitMapping { get; }

        // @property(nonatomic, readonly, nullable) NSError *error;
        [NullAllowed]
        [Export("error")]
        NSError Error { get; }

        // @property(nonatomic, readonly) NSTimeInterval latency;       
        [Export("latency")]
        double Latency { get; }

        // @property(nonatomic, readonly, nonnull) NSDictionary<NSString *, id> *dictionaryRepresentation;
        [Export("dictionaryRepresentation")]
        NSDictionary<NSString, NSObject> DictionaryRepresentation { get; }
    }

    // @interface GADResponseInfo : NSObject
    [BaseType(typeof(NSObject), Name = "GADResponseInfo")]
    interface ResponseInfo
    {
        // extern NSString *const _Nonnull GADGoogleAdNetworkClassName;
        [Field("GADGoogleAdNetworkClassName", "__Internal")]
        NSString GoogleAdNetworkClassName { get; }

        // extern NSString *const _Nonnull GADCustomEventAdNetworkClassName;
        //[Field("GADCustomEventAdNetworkClassName", "__Internal")]
        //NSString CustomEventAdNetworkClassName { get; }

        // extern NSString * _Nonnull GADErrorUserInfoKeyResponseInfo;
        //[Field("GADErrorUserInfoKeyResponseInfo", "__Internal")]
        //NSString ErrorUserInfoKey { get; }

        // @property (readonly, nonatomic) NSString * _Nullable responseIdentifier;
        [NullAllowed]
        [Export("responseIdentifier")]
        string ResponseIdentifier { get; }

        // @property (nonatomic, readonly, nullable) NSString *adNetworkClassName;
        [NullAllowed]
        [Export("adNetworkClassName")]
        string AdNetworkClassName { get; }

        // @property(nonatomic, readonly, nonnull) NSArray<GADAdNetworkResponseInfo *> *adNetworkInfoArray;     
        [Export("adNetworkInfoArray")]
        AdNetworkResponseInfo[] AdNetworkInfo { get; }

        // @property(nonatomic, readonly, nonnull) NSDictionary<NSString *, id> *dictionaryRepresentation;
        [Export("dictionaryRepresentation")]
        NSDictionary<NSString, NSObject> DictionaryRepresentation { get; }
    }

    // typedef void (^GADRewardedAdLoadCompletionHandler)(GADRewardedAd *_Nullable rewardedAd, NSError *_Nullable error);
    delegate void RewardedAdLoadCompletionHandler([NullAllowed] RewardedAd rewardedAd, [NullAllowed] NSError error);

    // typedef void (^GADUserDidEarnRewardHandler)(void)
    delegate void UserDidEarnRewardHandler();

    // @interface GADRewardedAd : NSObject <GADAdMetadataProvider, GADFullScreenPresentingAd>
    [BaseType(typeof(FullScreenPresentingAd), Name = "GADRewardedAd")]
    interface RewardedAd : AdMetadataProvider
    {
        // + (void)loadWithAdUnitID:(nonnull NSString *)adUnitID request:(nullable GADRequest *)request completionHandler:(nonnull GADRewardedAdLoadCompletionHandler)completionHandler;
        [Async]
        [Static]
        [Export("loadWithAdUnitID:request:completionHandler:")]
        void Load(string adUnitId, [NullAllowed] Request request, RewardedAdLoadCompletionHandler completionHandler);

        // @property (readonly, nonatomic) NSString * _Nonnull adUnitID;
        [Export("adUnitID")]
        string AdUnitId { get; }

        // @property (readonly, nonatomic) GADResponseInfo * _Nullable responseInfo;
        [NullAllowed]
        [Export("responseInfo")]
        ResponseInfo ResponseInfo { get; }

        // @property (readonly, nonatomic, nonnull) GADAdReward *adReward;
        [Export("adReward")]
        AdReward AdReward { get; }

        // @property (nonatomic, nullable) GADServerSideVerificationOptions *serverSideVerificationOptions;
        [NullAllowed]
        [Export("serverSideVerificationOptions")]
        ServerSideVerificationOptions ServerSideVerificationOptions { get; set; }

        // @property (copy, nonatomic) GADPaidEventHandler _Nullable paidEventHandler;
        [NullAllowed]
        [Export("paidEventHandler", ArgumentSemantic.Copy)]
        PaidEventHandler PaidEventHandler { get; set; }

        // - (BOOL)canPresentFromRootViewController:(nullable UIViewController *)rootViewController error:(NSError *_Nullable __autoreleasing *_Nullable)error NS_SWIFT_NAME(canPresent(from:)) NS_SWIFT_UI_ACTOR;
        [Export("canPresentFromRootViewController:error:")]
        bool CanPresent([NullAllowed] UIViewController rootViewController, [NullAllowed] out NSError error);

        // - (void)presentFromRootViewController:(nullable UIViewController *)rootViewController userDidEarnRewardHandler:(nonnull GADUserDidEarnRewardHandler)userDidEarnRewardHandler NS_SWIFT_NAME(present(from:userDidEarnRewardHandler:)) NS_SWIFT_UI_ACTOR;
        [Export("presentFromRootViewController:userDidEarnRewardHandler:")]
        void Present([NullAllowed] UIViewController viewController, UserDidEarnRewardHandler userDidEarnRewardHandler);

        [Export("adMetadata")]
        NSDictionary<NSString, NSObject> AdMetadata { get; }
    }

    interface IRewardedAdDelegate { }

    // @protocol GADRewardedAdDelegate <NSObject>
    [Model]
    [Protocol]
    [BaseType(typeof(NSObject), Name = "GADRewardedAdDelegate")]
    interface RewardedAdDelegate
    {
        // @required -(void)rewardedAd:(GADRewardedAd * _Nonnull)rewardedAd userDidEarnReward:(GADAdReward * _Nonnull)reward;
        [Abstract]
        [Export("rewardedAd:userDidEarnReward:")]
        void UserDidEarnReward(RewardedAd rewardedAd, AdReward reward);

        // @optional -(void)rewardedAd:(GADRewardedAd * _Nonnull)rewardedAd didFailToPresentWithError:(NSError * _Nonnull)error;
        [Export("rewardedAd:didFailToPresentWithError:")]
        void DidFailToPresent(RewardedAd rewardedAd, NSError error);

        // @optional -(void)rewardedAdDidPresent:(GADRewardedAd * _Nonnull)rewardedAd;
        [Export("rewardedAdDidPresent:")]
        void DidPresent(RewardedAd rewardedAd);

        // @optional -(void)rewardedAdDidDismiss:(GADRewardedAd * _Nonnull)rewardedAd;
        [Export("rewardedAdDidDismiss:")]
        void DidDismiss(RewardedAd rewardedAd);
    }

    interface IAdSizeDelegate
    {

    }

    [Model]
    [Protocol]
    [BaseType(typeof(NSObject), Name = "GADAdSizeDelegate")]
    interface AdSizeDelegate
    {
        [Abstract]
        [EventArgs("AdSizeDelegateSize")]
        [Export("adView:willChangeAdSizeTo:")]
        void WillChangeAdSizeTo(BannerView view, AdSize size);
    }

    // typedef void (^GADPaidEventHandler)(GADAdValue * _Nonnull);
    delegate void PaidEventHandler(AdValue value);

    // typedef void (^GADUserDidEarnRewardHandler)(void)
    delegate void GADUserDidEarnRewardHandler();

    // @interface GADAdValue : NSObject <NSCopying>
    [BaseType(typeof(NSObject), Name = "GADAdValue")]
    interface AdValue : INSCopying
    {
        // @property (readonly, nonatomic) GADAdValuePrecision precision;
        [Export("precision")]
        AdValuePrecision Precision { get; }

        // @property (readonly, nonatomic) NSDecimalNumber * _Nonnull value;
        [Export("value")]
        NSDecimalNumber Value { get; }

        // @property (readonly, nonatomic) NSString * _Nonnull currencyCode;
        [Export("currencyCode")]
        string CurrencyCode { get; }
    }

    interface IAppEventDelegate
    {

    }

    [Model]
    [Protocol]
    [BaseType(typeof(NSObject), Name = "GADAppEventDelegate")]
    interface AppEventDelegate
    {
        [Export("adView:didReceiveAppEvent:withInfo:")]
        void AdViewDidReceiveAppEvent(BannerView banner, string name, [NullAllowed] string info);

        [Export("interstitial:didReceiveAppEvent:withInfo:")]
        void InterstitialDidReceiveAppEvent(InterstitialAd interstitial, string name, [NullAllowed] string info);
    }

    // typedef void (^GADAppOpenAdLoadCompletionHandler)(GADAppOpenAd * _Nullable, NSError * _Nullable);
    delegate void AppOpenAdLoadCompletionHandler([NullAllowed] AppOpenAd appOpenAd, [NullAllowed] NSError error);

    interface IFullScreenContentDelegate
    {
    }

    [Model]
    [Protocol]
    [BaseType(typeof(NSObject), Name = "GADFullScreenContentDelegate")]
    interface FullScreenContentDelegate
    {
        // - (void)adDidRecordImpression:(nonnull id<GADFullScreenPresentingAd>)ad;
        [EventArgs("FullScreenPresentingAd")]
        [EventName("RecordedImpression")]
        [Export("adDidRecordImpression:")]
        void DidRecordImpression(FullScreenPresentingAd ad);

        // - (void)adDidRecordClick:(nonnull id<GADFullScreenPresentingAd>)ad;
        [EventArgs("FullScreenPresentingAd")]
        [EventName("RecordedClick")]
        [Export("adDidRecordClick:")]
        void DidRecordClick(FullScreenPresentingAd ad);

        // - (void)ad:(nonnull id<GADFullScreenPresentingAd>)ad
        [EventArgs("FullScreenPresentingAdWithError")]
        [EventName("FailedToPresentContent")]
        [Export("ad:didFailToPresentFullScreenContentWithError:")]
        void DidFailToPresentFullScreenContent(FullScreenPresentingAd ad, NSError error);

        // - (void)adWillPresentFullScreenContent:(nonnull id<GADFullScreenPresentingAd>)ad;
        [EventArgs("FullScreenPresentingAd")]
        [EventName("PresentedContent")]
        [Export("adWillPresentFullScreenContent:")]
        void WillPresentFullScreenContent(FullScreenPresentingAd ad);

        // - (void)adWillDismissFullScreenContent:(nonnull id<GADFullScreenPresentingAd>)ad;
        [EventArgs("FullScreenPresentingAd")]
        [EventName("DismissingContent")]
        [Export("adWillDismissFullScreenContent:")]
        void WillDismissFullScreenContent(FullScreenPresentingAd ad);

        // - (void)adDidDismissFullScreenContent:(nonnull id<GADFullScreenPresentingAd>)ad;
        [Export("adDidDismissFullScreenContent:")]
        [EventArgs("FullScreenPresentingAd")]
        [EventName("DismissedContent")]
        void DidDismissFullScreenContent(FullScreenPresentingAd ad);
    }

    [Protocol]
    [BaseType(typeof(NSObject),
            Name = "GADFullScreenPresentingAd",
            Delegates = new string[] { "Delegate" },
            Events = new Type[] { typeof(FullScreenContentDelegate) })]
    interface FullScreenPresentingAd
    {
        [NullAllowed]
        [Export("fullScreenContentDelegate", ArgumentSemantic.Weak)]
        IFullScreenContentDelegate Delegate { get; set; }
    }

    // @interface GADAppOpenAd : GADFullScreenPresentingAd
    [DisableDefaultCtor]
    [BaseType(typeof(FullScreenPresentingAd), Name = "GADAppOpenAd")]
    interface AppOpenAd
    {
        // + (void)loadWithAdUnitID:(nonnull NSString *)adUnitID request:(nullable GADRequest *)request orientation:(UIInterfaceOrientation)orientation completionHandler:(nonnull GADAppOpenAdLoadCompletionHandler)completionHandler;
        [Async]
        [Static]
        [Export("loadWithAdUnitID:request:orientation:completionHandler:")]
        [Obsolete("Use +loadWithAdUnitID:request:completionHandler:")]
        void Load(string adUnitId, [NullAllowed] Request request, UIInterfaceOrientation orientation, AppOpenAdLoadCompletionHandler completionHandler);
        
        // + (void)loadWithAdUnitID:(nonnull NSString *)adUnitID request:(nullable GADRequest *)request completionHandler: (nonnull GADAppOpenAdLoadCompletionHandler)completionHandler;
        [Async]
        [Static]
        [Export("loadWithAdUnitID:request:completionHandler:")]
        void Load(string adUnitId, [NullAllowed] Request request, AppOpenAdLoadCompletionHandler completionHandler);

        // @property (nonatomic, readonly, nonnull) GADResponseInfo* responseInfo;
        [Export("responseInfo")]
        ResponseInfo ResponseInfo { get; }

        // @property (nonatomic, nullable, copy) GADPaidEventHandler paidEventHandler;
        [NullAllowed]
        [Export("paidEventHandler", ArgumentSemantic.Copy)]
        PaidEventHandler PaidEventHandler { get; set; }

        // - (BOOL) canPresentFromRootViewController:(nonnull UIViewController *)rootViewController	error:(NSError* _Nullable __autoreleasing *_Nullable)error;
        [Export("canPresentFromRootViewController:error:")]
        bool CanPresent([NullAllowed] UIViewController rootViewController, [NullAllowed] out NSError error);

        // - (void) presentFromRootViewController:(nonnull UIViewController *)rootViewController;
        [Export("presentFromRootViewController:")]
        void Present([NullAllowed] UIViewController rootViewController);
        
        // @property (nonatomic, readonly, nonnull) NSString *adUnitID;
        [Export("adUnitID")]
        string AdUnitId { get; }
    }

    interface ISwipeableBannerViewDelegate
    {

    }

    [Model]
    [Protocol]
    [BaseType(typeof(NSObject), Name = "GADSwipeableBannerViewDelegate")]
    interface SwipeableBannerViewDelegate
    {
        [Export("adViewDidActivateAd:"), EventArgs("SwipeableBannerViewDelegateInfo")]
        void DidActivateAd(BannerView banner);

        [Export("adViewDidDeactivateAd:"), EventArgs("SwipeableBannerViewDelegateInfo")]
        void DidDeactivateAd(BannerView banner);
    }

    // @interface GADAudioVideoManager : NSObject
    [BaseType(typeof(NSObject),
           Name = "GADAudioVideoManager",
           Delegates = new string[] { "Delegate" },
           Events = new Type[] { typeof(AudioVideoManagerDelegate) })]
    interface AudioVideoManager
    {
        // @property(nonatomic, weak, nullable) id<GADAudioVideoManagerDelegate> delegate;
        [NullAllowed]
        [Export("delegate", ArgumentSemantic.Weak)]
        IAudioVideoManagerDelegate Delegate { get; set; }

        // @property(nonatomic, assign) BOOL audioSessionIsApplicationManaged;
        [Export("audioSessionIsApplicationManaged")]
        bool AudioSessionIsApplicationManaged { get; set; }
    }

    interface IAudioVideoManagerDelegate
    {
    }

    [Model]
    [Protocol]
    [BaseType(typeof(NSObject), Name = "GADAudioVideoManagerDelegate")]
    interface AudioVideoManagerDelegate
    {
        // - (void)audioVideoManagerWillPlayVideo:(GADAudioVideoManager *)audioVideoManager;
        [EventArgs("AudioVideoManagerWillPlayVideo")]
        [Export("audioVideoManagerWillPlayVideo:")]
        void WillPlayVideo(AudioVideoManager audioVideoManager);

        // - (void)audioVideoManagerDidPauseAllVideo:(GADAudioVideoManager *)audioVideoManager;
        [EventArgs("AudioVideoManagerAllVideoPaused")]
        [EventName("AllVideoPaused")]
        [Export("audioVideoManagerDidPauseAllVideo:")]
        void DidPauseAllVideo(AudioVideoManager audioVideoManager);

        // - (void)audioVideoManagerWillPlayAudio:(GADAudioVideoManager *)audioVideoManager;
        [EventArgs("AudioVideoManagerWillPlayAudio")]
        [Export("audioVideoManagerWillPlayAudio:")]
        void WillPlayAudio(AudioVideoManager audioVideoManager);

        // - (void)audioVideoManagerDidStopPlayingAudio:(GADAudioVideoManager *)audioVideoManager;
        [EventArgs("AudioVideoManagerPlayingAudioStopped")]
        [EventName("PlayingAudioStopped")]
        [Export("audioVideoManagerDidStopPlayingAudio:")]
        void DidStopPlayingAudio(AudioVideoManager audioVideoManager);
    }

    #region Search

    // @interface GADServerSideVerificationOptions : NSObject <NSCopying>
    [BaseType(typeof(NSObject), Name = "GADServerSideVerificationOptions")]
    interface ServerSideVerificationOptions : INSCopying
    {
        // @property (copy, nonatomic) NSString * _Nullable userIdentifier;
        [NullAllowed]
        [Export("userIdentifier")]
        string UserIdentifier { get; set; }

        // @property (copy, nonatomic) NSString * _Nullable customRewardString;
        [NullAllowed]
        [Export("customRewardString")]
        string CustomRewardString { get; set; }
    }

    // @interface GADNativeAd : NSObject
    [BaseType(typeof(NSObject),
           Name = "GADNativeAd",
           Delegates = new[] { "Delegate", "UnconfirmedClickDelegate" },
           Events = new[] { typeof(NativeAdDelegate), typeof(NativeAdUnconfirmedClickDelegate) })]
    interface NativeAd
    {
        // @property (readonly, copy, nonatomic) NSString * _Nullable headline;
        [NullAllowed]
        [Export("headline")]
        string Headline { get; }

        // @property (readonly, copy, nonatomic) NSString * _Nullable callToAction;
        [NullAllowed]
        [Export("callToAction")]
        string CallToAction { get; }

        // @property (readonly, nonatomic, strong) GADNativeAdImage * _Nullable icon;
        [NullAllowed]
        [Export("icon", ArgumentSemantic.Strong)]
        NativeAdImage Icon { get; }

        // @property (readonly, copy, nonatomic) NSString * _Nullable body;
        [NullAllowed]
        [Export("body")]
        string Body { get; }

        // @property (readonly, nonatomic, strong) NSArray<GADNativeAdImage *> * _Nullable images;
        [NullAllowed]
        [Export("images", ArgumentSemantic.Strong)]
        NativeAdImage[] Images { get; }

        // @property (readonly, copy, nonatomic) NSDecimalNumber * _Nullable starRating;
        [NullAllowed]
        [Export("starRating", ArgumentSemantic.Copy)]
        NSDecimalNumber StarRating { get; }

        // @property (readonly, copy, nonatomic) NSString * _Nullable store;
        [NullAllowed]
        [Export("store")]
        string Store { get; }

        // @property (readonly, copy, nonatomic) NSString * _Nullable price;
        [NullAllowed]
        [Export("price")]
        string Price { get; }

        // @property (readonly, copy, nonatomic) NSString * _Nullable advertiser;
        [NullAllowed]
        [Export("advertiser")]
        string Advertiser { get; }

        // @property(nonatomic, readonly, nonnull) GADMediaContent *mediaContent;
        [Export("mediaContent")]
        MediaContent MediaContent { get; }

        // @property (nonatomic, weak) id<GADCustomNativeAdDelegate> _Nullable delegate;
        [NullAllowed]
        [Export("delegate", ArgumentSemantic.Weak)]
        INativeAdDelegate Delegate { get; set; }

        // @property (nonatomic, weak) UIViewController * _Nullable rootViewController;
        [NullAllowed]
        [Export("rootViewController", ArgumentSemantic.Weak)]
        UIViewController RootViewController { get; set; }

        // @property (readonly, copy, nonatomic) NSDictionary<NSString *,id> * _Nullable extraAssets;
        [NullAllowed]
        [Export("extraAssets", ArgumentSemantic.Copy)]
        NSDictionary<NSString, NSObject> ExtraAssets { get; }

        // @property (readonly, nonatomic) GADResponseInfo * _Nonnull responseInfo;
        [Export("responseInfo")]
        ResponseInfo ResponseInfo { get; }

        // @property (copy, nonatomic) GADPaidEventHandler _Nullable paidEventHandler;
        [NullAllowed]
        [Export("paidEventHandler", ArgumentSemantic.Copy)]
        PaidEventHandler PaidEventHandler { get; set; }

        // @property(nonatomic, readonly, getter=isCustomMuteThisAdAvailable) BOOL customMuteThisAdAvailable;
        [Export("isCustomMuteThisAdAvailable")]
        bool IsCustomMuteThisAdAvailable { get; }

        // @property(nonatomic, readonly, nullable) NSArray<GADMuteThisAdReason *> *muteThisAdReasons;
        [NullAllowed]
        [Export("muteThisAdReasons")]
        MuteThisAdReason[] MuteThisAdReasons { get; }

        // -(void)registerAdView:(UIView * _Nonnull)adView clickableAssetViews:(NSDictionary<GADNativeAssetIdentifier,UIView *> * _Nonnull)clickableAssetViews nonclickableAssetViews:(NSDictionary<GADNativeAssetIdentifier,UIView *> * _Nonnull)nonclickableAssetViews;
        [Export("registerAdView:clickableAssetViews:nonclickableAssetViews:")]
        void RegisterAdView(UIView adView, NSDictionary<NSString, UIView> nsClickableAssetViews, NSDictionary<NSString, UIView> nsNonclickableAssetViews);

        [Wrap("RegisterAdView (adView, NSDictionary<NSString, UIView>.FromObjectsAndKeys (System.Linq.Enumerable.ToArray (clickableAssetViews.Values), System.Linq.Enumerable.ToArray (clickableAssetViews.Keys), clickableAssetViews.Keys.Count), NSDictionary<NSString, UIView>.FromObjectsAndKeys (System.Linq.Enumerable.ToArray (nonclickableAssetViews.Values), System.Linq.Enumerable.ToArray (nonclickableAssetViews.Keys), nonclickableAssetViews.Keys.Count))")]
        void RegisterAdView(UIView adView, Dictionary<string, UIView> clickableAssetViews, Dictionary<string, UIView> nonclickableAssetViews);

        // -(void)unregisterAdView;
        [Export("unregisterAdView")]
        void UnregisterAdView();

        // - (void)muteThisAdWithReason:(nullable GADMuteThisAdReason *)reason;
        [Export("muteThisAdWithReason:")]
        void MuteThisAd(MuteThisAdReason reason);

        ///
        /// From NativeAd_ConfirmationClick Category
        ///

        // @property (nonatomic, weak) id<GADNativeAdUnconfirmedClickDelegate> _Nullable unconfirmedClickDelegate;
        [NullAllowed]
        [Export("unconfirmedClickDelegate", ArgumentSemantic.Weak)]
        INativeAdUnconfirmedClickDelegate UnconfirmedClickDelegate { get; set; }

        // -(void)registerClickConfirmingView:(UIView * _Nullable)view;
        [Export("registerClickConfirmingView:")]
        void RegisterClickConfirmingView([NullAllowed] UIView view);

        // -(void)cancelUnconfirmedClick;
        [Export("cancelUnconfirmedClick")]
        void CancelUnconfirmedClick();

        ///
        /// From NativeAd_CustomClickGesture Category
        ///

        // - (void)enableCustomClickGestures;
        [Export("enableCustomClickGestures")]
        void EnableCustomClickGestures();

        // - (void)recordCustomClickGesture;
        [Export("recordCustomClickGesture")]
        void RecordCustomClickGesture();
    }

    interface IUnifiedNativeAdLoaderDelegate { }

    // CHECK
    // @protocol GADUnifiedNativeAdLoaderDelegate <GADAdLoaderDelegate>
    [Model]
    [Protocol]
    [BaseType(typeof(NSObject), Name = "GADUnifiedNativeAdLoaderDelegate")]
    interface UnifiedNativeAdLoaderDelegate : AdLoaderDelegate
    {
        // @required -(void)adLoader:(GADAdLoader * _Nonnull)adLoader didReceiveUnifiedNativeAd:(GADNativeAd * _Nonnull)nativeAd;
        [Abstract]
        [Export("adLoader:didReceiveUnifiedNativeAd:")]
        void DidReceiveUnifiedNativeAd(AdLoader adLoader, NativeAd nativeAd);
    }

    interface INativeAdLoaderDelegate { }

    // CHECK
    // @protocol GADNativeAdLoaderDelegate <GADAdLoaderDelegate>
    [Model]
    [Protocol]
    [BaseType(typeof(NSObject), Name = "GADNativeAdLoaderDelegate")]
    interface NativeAdLoaderDelegate : AdLoaderDelegate
    {
        // @required -(void)adLoader:(GADAdLoader * _Nonnull)adLoader didReceiveNativeAd:(GADNativeAd * _Nonnull)nativeAd;
        [Abstract]
        [Export("adLoader:didReceiveNativeAd:")]
        void DidReceiveNativeAd(AdLoader adLoader, NativeAd nativeAd);
    }

    // @interface GADNativeAdView : UIView
    [BaseType(typeof(UIView), Name = "GADNativeAdView")]
    interface NativeAdView
    {
        [Export("initWithFrame:")]
        NativeHandle Constructor(CGRect frame);

        // @property (nonatomic, strong) GADNativeAd * _Nullable nativeAd;
        [NullAllowed]
        [Export("nativeAd", ArgumentSemantic.Strong)]
        NativeAd NativeAd { get; set; }

        // @property (nonatomic, weak) UIView * _Nullable headlineView __attribute__((iboutlet));
        [NullAllowed]
        [Export("headlineView", ArgumentSemantic.Weak)]
        UIView HeadlineView { get; set; }

        // @property (nonatomic, weak) UIView * _Nullable callToActionView __attribute__((iboutlet));
        [NullAllowed]
        [Export("callToActionView", ArgumentSemantic.Weak)]
        UIView CallToActionView { get; set; }

        // @property (nonatomic, weak) UIView * _Nullable iconView __attribute__((iboutlet));
        [NullAllowed]
        [Export("iconView", ArgumentSemantic.Weak)]
        UIView IconView { get; set; }

        // @property (nonatomic, weak) UIView * _Nullable bodyView __attribute__((iboutlet));
        [NullAllowed]
        [Export("bodyView", ArgumentSemantic.Weak)]
        UIView BodyView { get; set; }

        // @property (nonatomic, weak) UIView * _Nullable storeView __attribute__((iboutlet));
        [NullAllowed]
        [Export("storeView", ArgumentSemantic.Weak)]
        UIView StoreView { get; set; }

        // @property (nonatomic, weak) UIView * _Nullable priceView __attribute__((iboutlet));
        [NullAllowed]
        [Export("priceView", ArgumentSemantic.Weak)]
        UIView PriceView { get; set; }

        // @property (nonatomic, weak) UIView * _Nullable imageView __attribute__((iboutlet));
        [NullAllowed]
        [Export("imageView", ArgumentSemantic.Weak)]
        UIView ImageView { get; set; }

        // @property (nonatomic, weak) UIView * _Nullable starRatingView __attribute__((iboutlet));
        [NullAllowed]
        [Export("starRatingView", ArgumentSemantic.Weak)]
        UIView StarRatingView { get; set; }

        // @property (nonatomic, weak) UIView * _Nullable advertiserView __attribute__((iboutlet));
        [NullAllowed]
        [Export("advertiserView", ArgumentSemantic.Weak)]
        UIView AdvertiserView { get; set; }

        // @property (nonatomic, weak) GADMediaView * _Nullable mediaView __attribute__((iboutlet));
        [NullAllowed]
        [Export("mediaView", ArgumentSemantic.Weak)]
        MediaView MediaView { get; set; }

        // @property (nonatomic, weak) GADAdChoicesView * _Nullable adChoicesView __attribute__((iboutlet));
        [NullAllowed]
        [Export("adChoicesView", ArgumentSemantic.Weak)]
        AdChoicesView AdChoicesView { get; set; }
    }

    [Static]
    interface NativeAdAssetIdentifiers
    {
        // extern const GADNativeAssetIdentifier _Nonnull GADNativeHeadlineAsset __attribute__((visibility("default")));
        [Field("GADNativeHeadlineAsset", "__Internal")]
        NSString HeadlineAsset { get; }

        // extern const GADNativeAssetIdentifier _Nonnull GADNativeCallToActionAsset __attribute__((visibility("default")));
        [Field("GADNativeCallToActionAsset", "__Internal")]
        NSString CallToActionAsset { get; }

        // extern const GADNativeAssetIdentifier _Nonnull GADNativeIconAsset __attribute__((visibility("default")));
        [Field("GADNativeIconAsset", "__Internal")]
        NSString IconAsset { get; }

        // extern const GADNativeAssetIdentifier _Nonnull GADNativeBodyAsset __attribute__((visibility("default")));
        [Field("GADNativeBodyAsset", "__Internal")]
        NSString BodyAsset { get; }

        // extern const GADNativeAssetIdentifier _Nonnull GADNativeStoreAsset __attribute__((visibility("default")));
        [Field("GADNativeStoreAsset", "__Internal")]
        NSString StoreAsset { get; }

        // extern const GADNativeAssetIdentifier _Nonnull GADNativePriceAsset __attribute__((visibility("default")));
        [Field("GADNativePriceAsset", "__Internal")]
        NSString PriceAsset { get; }

        // extern const GADNativeAssetIdentifier _Nonnull GADNativeImageAsset __attribute__((visibility("default")));
        [Field("GADNativeImageAsset", "__Internal")]
        NSString ImageAsset { get; }

        // extern const GADNativeAssetIdentifier _Nonnull GADNativeStarRatingAsset __attribute__((visibility("default")));
        [Field("GADNativeStarRatingAsset", "__Internal")]
        NSString StarRatingAsset { get; }

        // extern const GADNativeAssetIdentifier _Nonnull GADNativeAdvertiserAsset __attribute__((visibility("default")));
        [Field("GADNativeAdvertiserAsset", "__Internal")]
        NSString AdvertiserAsset { get; }

        // extern const GADNativeAssetIdentifier _Nonnull GADNativeMediaViewAsset __attribute__((visibility("default")));
        [Field("GADNativeMediaViewAsset", "__Internal")]
        NSString MediaViewAsset { get; }

        // extern const GADNativeAssetIdentifier _Nonnull GADNativeAdChoicesViewAsset __attribute__((visibility("default")));
        [Field("GADNativeAdChoicesViewAsset", "__Internal")]
        NSString AdChoicesViewAsset { get; }
    }

    // typedef void (^GADNativeAdCustomClickHandler)(NSString* assetID);
    delegate void NativeAdCustomClickHandle(string assetId);

    // @interface GADCustomNativeAd : UIView
    [BaseType(typeof(UIView), Name = "GADCustomNativeAd")]
    interface CustomNativeAd
    {
        // @property(nonatomic, readonly, nonnull) NSString *formatID;
        [Export("formatID")]
        string FormatID { get; }

        // @property(nonatomic, readonly, nonnull) NSArray<NSString *> *availableAssetKeys;
        [Export("availableAssetKeys")]
        string[] AvailableAssetKeys { get; }

        // @property(atomic, copy, nullable) GADNativeAdCustomClickHandler customClickHandler;
        [NullAllowed]
        [Export("customClickHandler", ArgumentSemantic.Copy)]
        NativeAdCustomClickHandle CustomClickHandler { get; }

        // @property(nonatomic, readonly, nullable) GADDisplayAdMeasurement *displayAdMeasurement;
        [NullAllowed]
        [Export("displayAdMeasurement")]
        DisplayAdMeasurement DisplayAdMeasurement { get; }

        // @property(nonatomic, readonly, nonnull) GADMediaContent *mediaContent;
        [NullAllowed]
        [Export("mediaContent")]
        MediaContent MediaContent { get; }

        // @property(nonatomic, weak, nullable) id<GADCustomNativeAdDelegate> delegate;
        [NullAllowed]
        [Export("delegate", ArgumentSemantic.Weak)]
        ICustomNativeAdDelegate Delegate { get; set; }

        // @property(nonatomic, weak, nullable) UIViewController *rootViewController;
        [NullAllowed]
        [Export("rootViewController", ArgumentSemantic.Weak)]
        UIViewController RootViewController { get; set; }

        // @property(nonatomic, readonly, nonnull) GADResponseInfo *responseInfo;
        [Export("responseInfo")]
        ResponseInfo ResponseInfo { get; }

        // - (nullable GADNativeAdImage *)imageForKey:(nonnull NSString *)key;
        [return: NullAllowed]
        [Export("imageForKey:")]
        NativeAdImage ImageForKey(NSString key);

        // - (nullable NSString *)stringForKey:(nonnull NSString *)key;		
        [return: NullAllowed]
        [Export("stringForKey:")]
        NSString StringForKey(NSString key);

        // - (void)performClickOnAssetWithKey:(nonnull NSString *)assetKey;
        [return: NullAllowed]
        [Export("performClickOnAssetWithKey:")]
        void RecordImpression(NSString assetKey);

        // - (void)recordImpression;
        [Export("recordImpression")]
        void RecordImpression();
    }


    interface ICustomNativeAdDelegate
    {
    }

    // @protocol GADCustomNativeAdDelegate <NSObject>
    [Model]
    [Protocol]
    [BaseType(typeof(NSObject), Name = "GADCustomNativeAdDelegate")]
    interface CustomNativeAdDelegate
    {
        // - (void)customNativeAdDidRecordImpression:(nonnull GADCustomNativeAd *)nativeAd;
        [EventArgs("NativeAd")]
        [EventName("ImpressionRecorded")]
        [Export("customNativeAdDidRecordImpression:")]
        void DidRecordImpression(NativeAd nativeAd);

        // - (void)customNativeAdDidRecordClick:(nonnull GADCustomNativeAd *)nativeAd;
        [EventArgs("NativeAd")]
        [EventName("ClickRecorded")]
        [Export("customNativeAdDidRecordClick:")]
        void DidRecordClick(NativeAd nativeAd);

        // - (void)customNativeAdWillPresentScreen:(nonnull GADCustomNativeAd *)nativeAd;
        [EventArgs("NativeAd")]
        [EventName("ScreenPresenting")]
        [Export("customNativeAdWillPresentScreen:")]
        void WillPresentScreen(NativeAd nativeAd);

        // - (void)customNativeAdWillDismissScreen:(nonnull GADCustomNativeAd *)nativeAd;
        [EventArgs("NativeAd")]
        [EventName("ScreenDismissing")]
        [Export("customNativeAdWillDismissScreen:")]
        void WillDismissScreen(NativeAd nativeAd);

        // - (void)customNativeAdDidDismissScreen:(nonnull GADCustomNativeAd *)nativeAd;
        [EventArgs("NativeAd")]
        [EventName("ScreenDismissed")]
        [Export("customNativeAdDidDismissScreen:")]
        void DidDismissScreen(NativeAd nativeAd);
    }

    interface INativeAdUnconfirmedClickDelegate { }

    // @protocol GADNativeAdUnconfirmedClickDelegate <NSObject>
    [Model]
    [Protocol]
    [BaseType(typeof(NSObject), Name = "GADNativeAdUnconfirmedClickDelegate")]
    interface NativeAdUnconfirmedClickDelegate
    {
        // @required -(void)nativeAd:(GADUNativeAd * _Nonnull)nativeAd didReceiveUnconfirmedClickOnAssetID:(GADNativeAssetIdentifier _Nonnull)assetID;
        [EventArgs("NativeAdUnconfirmedClickReceived")]
        [EventName("UnconfirmedClickReceived")]
        [Abstract]
        [Export("nativeAd:didReceiveUnconfirmedClickOnAssetID:")]
        void DidReceiveUnconfirmedClick(NativeAd nativeAd, string assetId);

        // @required -(void)nativeAdDidCancelUnconfirmedClick:(GADNativeAd * _Nonnull)nativeAd;
        [EventArgs("NativeAdUnconfirmedClickCancelled")]
        [EventName("UnconfirmedClickCancelled")]
        [Abstract]
        [Export("nativeAdDidCancelUnconfirmedClick:")]
        void DidCancelUnconfirmedClick(NativeAd nativeAd);
    }

    // @interface GADVideoController : NSObject
    [BaseType(typeof(NSObject),
           Name = "GADVideoController",
           Delegates = new string[] { "Delegate" },
           Events = new Type[] { typeof(VideoControllerDelegate) })]
    interface VideoController
    {
        // @property (nonatomic, weak, GAD_NULLABLE) id<GADVideoControllerDelegate> delegate;
        [NullAllowed]
        [Export("delegate", ArgumentSemantic.Weak)]
        IVideoControllerDelegate Delegate { get; set; }

        // - (void)setMute:(BOOL)mute;
        [Export("setMute:")]
        void SetMute(bool mute);

        // - (void)play;
        [Export("play")]
        void Play();

        // - (void)pause;
        [Export("pause")]
        void Pause();

        // - (void) stop;
        [Export("stop")]
        void Stop();

        // - (BOOL)customControlsEnabled;
        [Export("customControlsEnabled")]
        bool IsCustomControlsEnabled { get; }

        // - (BOOL)clickToExpandEnabled;
        [Export("clickToExpandEnabled")]
        bool IsClickToExpandEnabled { get; }
        
        // @property(nonatomic, getter=isMuted) BOOL muted;
        [Export("muted")]
        bool IsMuted { get; }
    }

    interface IVideoControllerDelegate
    {
    }

    // @protocol GADVideoControllerDelegate<NSObject>
    [Model]
    [Protocol]
    [BaseType(typeof(NSObject), Name = "GADVideoControllerDelegate")]
    interface VideoControllerDelegate
    {
        // - (void)videoControllerDidPlayVideo:(GADVideoController *)videoController;
        [EventArgs("VideoControllerVideoPlayed")]
        [EventName("VideoPlayed")]
        [Export("videoControllerDidPlayVideo:")]
        void DidPlayVideo(VideoController videoController);

        // - (void)videoControllerDidPauseVideo:(GADVideoController *)videoController;
        [EventArgs("VideoControllerVideoPaused")]
        [EventName("VideoPaused")]
        [Export("videoControllerDidPauseVideo:")]
        void DidPauseVideo(VideoController videoController);

        // - (void)videoControllerDidEndVideoPlayback:(GADVideoController*)videoController;
        [EventArgs("VideoControllerVideoPlaybackEnded")]
        [EventName("VideoPlaybackEnded")]
        [Export("videoControllerDidEndVideoPlayback:")]
        void DidEndVideoPlayback(VideoController videoController);

        // - (void)videoControllerDidMuteVideo:(GADVideoController *)videoController;
        [EventArgs("VideoControllerVideoMuted")]
        [EventName("VideoMuted")]
        [Export("videoControllerDidMuteVideo:")]
        void DidMuteVideo(VideoController videoController);

        // - (void)videoControllerDidUnmuteVideo:(GADVideoController *)videoController;
        [EventArgs("VideoControllerVideoUnuted")]
        [EventName("VideoUnuted")]
        [Export("videoControllerDidUnmuteVideo:")]
        void DidUnmuteVideo(VideoController videoController);
    }

    // @interface GADVideoOptions : GADAdLoaderOptions
    [BaseType(typeof(AdLoaderOptions), Name = "GADVideoOptions")]
    interface VideoOptions
    {
        // @property(nonatomic, assign) BOOL startMuted;
        [Export("startMuted", ArgumentSemantic.Assign)]
        bool StartMuted { get; set; }

        // @property(nonatomic, assign) BOOL customControlsRequested;
        [Export("customControlsRequested", ArgumentSemantic.Assign)]
        bool CustomControlsRequested { get; set; }

        //@property(nonatomic, assign) BOOL clickToExpandRequested;
        [Export("clickToExpandRequested", ArgumentSemantic.Assign)]
        bool ClickToExpandRequested { get; set; }
    }

    #endregion

    #region Loading

    // @interface GADAdChoicesView : UIView
    [BaseType(typeof(UIView), Name = "GADAdChoicesView")]
    interface AdChoicesView
    {
    }

    // @interface GADAdLoaderOptions : NSObject
    [BaseType(typeof(NSObject), Name = "GADAdLoaderOptions")]
    interface AdLoaderOptions
    {
    }

    [Static]
    interface AdLoadAdTypeConstants
    {
        // extern GADAdLoaderAdType  _Nonnull const GADAdLoaderAdTypeCustomNative;
        [Field ("GADAdLoaderAdTypeCustomNative", "__Internal")]
        NSString CustomNative { get; }

        // extern NS_SWIFT_NAME(adManagerBanner) GADAdLoaderAdType GADAdLoaderAdTypeGAMBanner __attribute__((swift_name("adManagerBanner")));
        [Field ("GADAdLoaderAdTypeGAMBanner", "__Internal")]
        NSString GamBanner { get; }

        // extern GADAdLoaderAdType  _Nonnull const GADAdLoaderAdTypeNative;
        [Field ("GADAdLoaderAdTypeNative", "__Internal")]
        NSString Native { get; }
    }

    // @interface GADAdLoader : NSObject
    [DisableDefaultCtor]
    [BaseType(typeof(NSObject), Name = "GADAdLoader")]
    interface AdLoader
    {
        // @property (nonatomic, weak) id<GADAdLoaderDelegate> __nullable delegate;
        [NullAllowed]
        [Export("delegate", ArgumentSemantic.Weak)]
        IAdLoaderDelegate Delegate { get; set; }

        // @property(nonatomic, readonly) NSString *adUnitID;
        [Export("adUnitID")]
        string AdUnitId { get; }

        // @property(nonatomic, getter=isLoading, readonly) BOOL loading;
        [Export("isLoading")]
        bool IsLoading { get; }

        // -(instancetype)initWithAdUnitID:(NSString *)adUnitID rootViewController:(UIViewController *)rootViewController adTypes:(NSArray *)adTypes options:(NSArray *)options;
        [Export("initWithAdUnitID:rootViewController:adTypes:options:")]
        NativeHandle Constructor(string adUnitID, [NullAllowed] UIViewController rootViewController, NSString[] adTypes, [NullAllowed] AdLoaderOptions[] options);

        // -(void)loadRequest:(GADRequest *)request;
        [Export("loadRequest:")]
        void LoadRequest([NullAllowed] Request request);
    }

    // @protocol GADAdMetadataProvider <NSObject>
    [Model]
    [Protocol]
    [BaseType(typeof(NSObject), Name = "GADAdMetadataProvider")]
    interface AdMetadataProvider
    {
        // @property(nonatomic, readonly, nullable) NSDictionary<GADAdMetadataKey, id> *adMetadata;
        [Export("adMetadata")]
        NSDictionary<NSString, NSObject> AdMetadata { get; }

        // @property(nonatomic, weak, nullable) id<GADAdMetadataDelegate> adMetadataDelegate;
        [NullAllowed]
        [Export("adMetadataDelegate", ArgumentSemantic.Weak)]
        IAdMetadataDelegate AdMetadataDelegate { get; set; }
    }

    interface IAdMetadataDelegate
    {
    }

    // @protocol GADAdMetadataDelegate <NSObject>
    [Model]
    [Protocol]
    [BaseType(typeof(NSObject), Name = "GADAdMetadataDelegate")]
    interface AdMetadataDelegate
    {
        // - (void)adMetadataDidChange:(nonnull id<GADAdMetadataProvider>)ad;
        [Abstract]
        [Export("adMetadataDidChange:")]
        void AdMetadataDidChange(AdMetadataProvider ad);
    }

    interface IAdLoaderDelegate
    {
    }

    // @protocol GADAdLoaderDelegate <NSObject>
    [Model]
    [Protocol]
    [BaseType(typeof(NSObject), Name = "GADAdLoaderDelegate")]
    interface AdLoaderDelegate
    {
        // @required -(void)adLoader:(GADAdLoader *)adLoader didFailToReceiveAdWithError:(NSError *)error;
        [Abstract]
        [Export("adLoader:didFailToReceiveAdWithError:")]
        void DidFailToReceiveAd(AdLoader adLoader, NSError error);

        // @optional - (void)adLoaderDidFinishLoading:(GADAdLoader *)adLoader;
        [Export("adLoaderDidFinishLoading:")]
        void DidFinishLoading(AdLoader adLoader);
    }

    #region Loading.Formats

    interface INativeAdDelegate
    {
    }

    // @protocol GADNativeAdDelegate <NSObject>
    [Model]
    [Protocol]
    [BaseType(typeof(NSObject), Name = "GADNativeAdDelegate")]
    interface NativeAdDelegate
    {

        // @optional -(void)nativeAdDidRecordImpression:(GADNativeAd *)nativeAd;
        [EventArgs("NativeAd")]
        [EventName("ImpressionRecorded")]
        [Export("nativeAdDidRecordImpression:")]
        void DidRecordImpression(NativeAd nativeAd);

        // @optional -(void)nativeAdDidRecordClick:(GADNativeAd *)nativeAd;
        [EventArgs("NativeAd")]
        [EventName("ClickRecorded")]
        [Export("nativeAdDidRecordClick:")]
        void DidRecordClick(NativeAd nativeAd);

        // @optional -(void)nativeAdWillPresentScreen:(GADNativeAd *)nativeAd;
        [EventArgs("NativeAd")]
        [Export("nativeAdWillPresentScreen:")]
        void WillPresentScreen(NativeAd nativeAd);

        // @optional -(void)nativeAdWillDismissScreen:(GADNativeAd *)nativeAd;
        [EventArgs("NativeAd")]
        [Export("nativeAdWillDismissScreen:")]
        void WillDismissScreen(NativeAd nativeAd);

        // @optional -(void)nativeAdDidDismissScreen:(GADNativeAd *)nativeAd;
        [EventArgs("NativeAd")]
        [EventName("ScreenDismissed")]
        [Export("nativeAdDidDismissScreen:")]
        void DidDismissScreen(NativeAd nativeAd);

        // @optional -(void)nativeAdWillLeaveApplication:(GADNativeAd *)nativeAd;
        [EventArgs("NativeAd")]
        [Export("nativeAdWillLeaveApplication:")]
        void WillLeaveApplication(NativeAd nativeAd);
    }

    // @interface GADNativeAdImage : NSObject
    [DisableDefaultCtor]
    [BaseType(typeof(NSObject), Name = "GADNativeAdImage")]
    interface NativeAdImage
    {
        // -(instancetype)initWithImage:(UIImage *)image;
        [Export("initWithImage:")]
        NativeHandle Constructor(UIImage image);

        // -(instancetype)initWithURL:(NSURL *)URL scale:(CGFloat)scale;
        [Export("initWithURL:scale:")]
        NativeHandle Constructor(NSUrl url, nfloat scale);

        // @property (readonly, nonatomic, strong) UIImage * image;
        [NullAllowed]
        [Export("image", ArgumentSemantic.Strong)]
        UIImage Image { get; }

        // @property (readonly, nonatomic, strong) NSURL * imageURL;
        [NullAllowed]
        [Export("imageURL", ArgumentSemantic.Copy)]
        NSUrl ImageUrl { get; }

        // @property (readonly, assign, nonatomic) CGFloat scale;
        [Export("scale")]
        nfloat Scale { get; }
    }

    [BaseType(typeof(AdLoaderOptions), Name = "GADNativeAdViewAdOptions")]
    interface NativeAdViewAdOptions
    {
        // @property(nonatomic, assign) GADAdChoicesPosition preferredAdChoicesPosition;
        [Export("preferredAdChoicesPosition", ArgumentSemantic.Assign)]
        AdChoicesPosition PreferredAdChoicesPosition { get; set; }
    }

    #endregion

    #region Loading.Options

    // @interface GADNativeAdImageAdLoaderOptions : GADAdLoaderOptions
    [BaseType(typeof(AdLoaderOptions), Name = "GADNativeAdImageAdLoaderOptions")]
    interface NativeAdImageAdLoaderOptions
    {
        // @property (assign, nonatomic) BOOL disableImageLoading;
        [Export("disableImageLoading")]
        bool DisableImageLoading { get; set; }

        // @property (assign, nonatomic) BOOL shouldRequestMultipleImages;
        [Export("shouldRequestMultipleImages")]
        bool ShouldRequestMultipleImages { get; set; }
    }

    // @interface GADNativeAdMediaAdLoaderOptions : GADAdLoaderOptions
    [BaseType(typeof(AdLoaderOptions), Name = "GADNativeAdMediaAdLoaderOptions")]
    interface NativeAdMediaAdLoaderOptions
    {
        // @property (assign, nonatomic) GADMediaAspectRatio mediaAspectRatio;
        [Export("mediaAspectRatio", ArgumentSemantic.Assign)]
        MediaAspectRatio MediaAspectRatio { get; set; }
    }

    #endregion

    #endregion

    #region Mediation
    
    // typedef void (^GADMediationAdapterSetUpCompletionBlock)(NSError *_Nullable)
    delegate void MediationAdapterSetUpCompletionBlock([NullAllowed] NSError error);
    
    // @protocol GADMediationAdEventDelegate <NSObject>
    [Model]
    [Protocol]
    [BaseType(typeof(NSObject), Name = "GADMediationAdEventDelegate")]
    interface MediationAdEventDelegate
    {
    }
    
    
    // @interface GADMediationServerConfiguration : NSObject
    [BaseType(typeof(NSObject), Name = "GADMediationServerConfiguration")]
    interface MediationServerConfiguration
    {
        // @property (nonatomic, readonly, nonnull) NSArray<GADMediationCredentials *> *credentials;
        [Export("credentials")]
        MediationCredentials[] Credentials { get; }
    }

    // @interface GADMediationCredentials : NSObject
    [BaseType(typeof(NSObject), Name = "GADMediationCredentials")]
    interface MediationCredentials
    {
        // @property (nonatomic, readonly, nonnull) NSDictionary<NSString *, id> *settings;
        [Export("settings")]
        NSDictionary<NSString, NSObject> Settings { get; }
        
        // @property (nonatomic, readonly) GADAdFormat format;
        [Export("format")]
        AdFormat Format { get; }
    }
    
    // @interface GADMediationAdConfiguration : NSObject
    [BaseType(typeof(NSObject), Name = "GADMediationAdConfiguration")]
    interface MediationAdConfiguration
    {
        // @property (nonatomic, readonly, nullable) NSString *bidResponse;
        [NullAllowed]  
        [Export("bidResponse")]
        string BidResponse { get; }
        
        // @property (nonatomic, readonly, nullable) UIViewController *topViewController;
        [NullAllowed]
        [Export("topViewController")]
        UIViewController TopViewController { get; }
        
        // @property (nonatomic, readonly, nonnull) GADMediationCredentials *credentials;
        [Export("credentials")]
        MediationCredentials Credentials { get; }
        
        // @property (nonatomic, readonly, nullable) NSData *watermark;
        [NullAllowed]
        [Export("watermark")]
        NSData Watermark { get; }
        
        // @property (nonatomic, readonly, nullable) id<GADAdNetworkExtras> extras;
        [NullAllowed]
        [Export("extras")]
        AdNetworkExtras Extras { get; }
        
        // @property (nonatomic, readonly) BOOL isTestRequest;
        [Export("isTestRequest")]
        bool IsTestRequest { get; }
    }
    
    interface IGADMediationAdapter
    {
    }

    [Protocol(Name = "GADMediationAdapter")]
    interface MediationAdapter
    {
        // + (GADVersionNumber)adapterVersion;
        [Static]
        [Abstract]
        [Export("adapterVersion")]
        VersionNumber AdapterVersion { get; }
        
        // + (GADVersionNumber)adSDKVersion;
        [Static]
        [Abstract]
        [Export("adSDKVersion")]
        VersionNumber AdSDKVersion { get; }
        
        // + (nullable Class<GADAdNetworkExtras>)networkExtrasClass;
        [Static]
        [Abstract]
        [return: NullAllowed]
        [Export("networkExtrasClass")]
        AdNetworkExtras NetworkExtrasClass { get; }
        
        // + (void)setUpWithConfiguration: (nonnull GADMediationServerConfiguration *)configuration completionHandler:(nonnull GADMediationAdapterSetUpCompletionBlock) completionHandler;
        [Static]
        [Export("setUpWithConfiguration:completionHandler:")]
        void SetUpWithConfiguration(MediationServerConfiguration configuration, MediationAdapterSetUpCompletionBlock completionHandler);
        
        // - (void)loadBannerForAdConfiguration: (nonnull GADMediationBannerAdConfiguration *)adConfiguration completionHandler: (nonnull GADMediationBannerLoadCompletionHandler) completionHandler;
        [Abstract]
        [Export("loadBannerForAdConfiguration:completionHandler:")]
        void LoadBannerForAdConfiguration(MediationBannerAdConfiguration adConfiguration, MediationBannerLoadCompletionHandler completionHandler);
        
        // - (void)loadInterstitialForAdConfiguration: (nonnull GADMediationInterstitialAdConfiguration *)adConfiguration completionHandler: (nonnull GADMediationInterstitialLoadCompletionHandler)completionHandler;
        [Abstract]
        [Export("loadInterstitialForAdConfiguration:completionHandler:")]
        void LoadInterstitialForAdConfiguration(MediationInterstitialAdConfiguration adConfiguration, MediationInterstitialLoadCompletionHandler completionHandler);
        
        // - (void)loadNativeAdForAdConfiguration: (nonnull GADMediationNativeAdConfiguration *)adConfiguration completionHandler: (nonnull GADMediationNativeLoadCompletionHandler) completionHandler;
        [Abstract]
        [Export("loadNativeAdForAdConfiguration:completionHandler:")]
        void LoadNativeAdForAdConfiguration(MediationNativeAdConfiguration adConfiguration, MediationNativeLoadCompletionHandler completionHandler);
        
        // - (void)loadRewardedAdForAdConfiguration: (nonnull GADMediationRewardedAdConfiguration *)adConfiguration completionHandler: (nonnull GADMediationRewardedLoadCompletionHandler) completionHandler;
        [Abstract]
        [Export("loadRewardedAdForAdConfiguration:completionHandler:")]
        void LoadRewardedAdForAdConfiguration(MediationRewardedAdConfiguration adConfiguration, MediationRewardedLoadCompletionHandler completionHandler);
        
        // - (void)loadAppOpenAdForAdConfiguration: (nonnull GADMediationAppOpenAdConfiguration *)adConfiguration completionHandler: (nonnull GADMediationAppOpenLoadCompletionHandler) completionHandler;
        [Abstract]
        [Export("loadAppOpenAdForAdConfiguration:completionHandler:")]
        void LoadAppOpenAdForAdConfiguration(MediationAppOpenAdConfiguration adConfiguration, MediationAppOpenLoadCompletionHandler completionHandler);
    }
    
    // @protocol GADMediationAd <NSObject>
    [BaseType(typeof(NSObject), Name = "GADMediationAd")]
    [Protocol(Name = "GADMediationAd")]
    interface MediationAd
    {
    }
    
    // typedef id<GADMediationBannerAdEventDelegate> _Nullable (^GADMediationBannerLoadCompletionHandler)(id<GADMediationBannerAd> _Nullable, NSError *_Nullable)
    delegate MediationBannerAdEventDelegate MediationBannerLoadCompletionHandler([NullAllowed] MediationBannerAd ad, [NullAllowed] NSError error);
    
    // @protocol GADMediationBannerAd <GADMediationAd>
    [BaseType(typeof(NSObject), Name = "GADMediationBannerAd")]
    [Protocol(Name = "GADMediationBannerAd")]
    interface MediationBannerAd : MediationAd
    {
        // @property (nonatomic, readonly) UIView * _Nonnull view;
        [Export("view")]
        UIView View { get; }
        
        // - (void)changeAdSizeTo:(GADAdSize)adSize;
        [Abstract]
        [Export("changeAdSizeTo:")]
        void ChangeAdSizeTo(AdSize adSize);
    }
    
    [Model]
    [Protocol]
    [BaseType(typeof(MediationAdEventDelegate), Name = "GADMediationBannerAdEventDelegate")]
    interface MediationBannerAdEventDelegate
    {
        // - (void)reportImpression;
        [Abstract]
        [Export("reportImpression")]
        void ReportImpression();
        
        // - (void)reportClick;
        [Abstract]
        [Export("reportClick")]
        void ReportClick();
        
        // - (void)willPresentFullScreenView;
        [Abstract]
        [Export("willPresentFullScreenView")]
        void WillPresentFullScreenView();
        
        // - (void)didFailToPresentWithError:(nonnull NSError *)error;
        [Abstract]
        [Export("didFailToPresentWithError:")]
        void DidFailToPresent(NSError error);
        
        // - (void)willDismissFullScreenView;
        [Abstract]
        [Export("willDismissFullScreenView")]
        void WillDismissFullScreenView();
        
        // - (void)didDismissFullScreenView;
        [Abstract]
        [Export("didDismissFullScreenView")]
        void DidDismissFullScreenView();
    }
    
    
    // @interface GADMediationBannerAdConfiguration : GADMediationAdConfiguration
    [BaseType(typeof(MediationAdConfiguration), Name = "GADMediationBannerAdConfiguration")]
    interface MediationBannerAdConfiguration
    {
        // @property (nonatomic, readonly) GADAdSize adSize;
        [Export("adSize")]
        AdSize AdSize { get; }
    }
    
    // @interface GADMediationInterstitialAdConfiguration : GADMediationAdConfiguration
    [BaseType(typeof(MediationAdConfiguration), Name = "GADMediationInterstitialAdConfiguration")]
    interface MediationInterstitialAdConfiguration
    {
    }
    
    // typedef id<GADMediationInterstitialAdEventDelegate> _Nullable (^GADMediationInterstitialLoadCompletionHandler)(id<GADMediationInterstitialAd> _Nullable, NSError *_Nullable)
    delegate MediationInterstitialAdEventDelegate MediationInterstitialLoadCompletionHandler([NullAllowed] MediationInterstitialAd ad, [NullAllowed] NSError error);
    
    // @protocol GADMediationInterstitialAd <GADMediationAd>
    [BaseType(typeof(NSObject), Name = "GADMediationInterstitialAd")]
    [Protocol(Name = "GADMediationInterstitialAd")]
    interface MediationInterstitialAd : MediationAd
    {
        // - (void)presentFromViewController:(nonnull UIViewController *)viewController;
        [Abstract]
        [Export("presentFromViewController:")]
        void PresentFromViewController(UIViewController viewController);
    }
    
    [Model]
    [Protocol]
    [BaseType(typeof(MediationAdEventDelegate), Name = "GADMediationInterstitialAdEventDelegate")]
    interface MediationInterstitialAdEventDelegate
    {
    }
    
    // @interface GADMediationNativeAdConfiguration : GADMediationAdConfiguration
    [BaseType(typeof(MediationAdConfiguration), Name = "GADMediationNativeAdConfiguration")]
    interface MediationNativeAdConfiguration
    {
    }
    
    // typedef id<GADMediationNativeAdEventDelegate> _Nullable (^GADMediationNativeLoadCompletionHandler)(id<GADMediationNativeAd> _Nullable, NSError *_Nullable)
    delegate MediationNativeAdEventDelegate MediationNativeLoadCompletionHandler([NullAllowed] MediationNativeAd ad, [NullAllowed] NSError error);
    
    // @protocol GADMediationNativeAdEventDelegate <GADMediationAdEventDelegate>
    [Model]
    [Protocol]
    [BaseType(typeof(MediationAdEventDelegate), Name = "GADMediationNativeAdEventDelegate")]
    interface MediationNativeAdEventDelegate
    {
        // - (void)didPlayVideo;
        [Abstract]
        [Export("didPlayVideo")]
        void DidPlayVideo();
        
        // - (void)didPauseVideo;
        [Abstract]
        [Export("didPauseVideo")]
        void DidPauseVideo();
        
        // - (void)didEndVideo;
        [Abstract]
        [Export("didEndVideo")]
        void DidEndVideo();
        
        // - (void)didMuteVideo;
        [Abstract]
        [Export("didMuteVideo")]
        void DidMuteVideo();
        
        // - (void)didUnmuteVideo;
        [Abstract]
        [Export("didUnmuteVideo")]
        void DidUnmuteVideo();
    }

    // @protocol GADMediationNativeAd <GADMediationAd, GADMediatedUnifiedNativeAd>
    [BaseType(typeof(NSObject), Name = "GADMediationNativeAd")]
    [Protocol]
    [Model]
    interface MediationNativeAd : MediationAd, Mediation.MediatedUnifiedNativeAd
    {
        // - (BOOL)handlesUserClicks;
        [Abstract]
        [Export("handlesUserClicks")]
        bool HandlesUserClicks();

        // - (BOOL)handlesUserImpressions;
        [Abstract]
        [Export("handlesUserImpressions")]
        bool HandlesUserImpressions();
    }
    
    // - (void)loadRewardedAdForAdConfiguration: (nonnull GADMediationRewardedAdConfiguration *)adConfiguration completionHandler: (nonnull GADMediationRewardedLoadCompletionHandler) completionHandler;
    [BaseType(typeof(MediationAdConfiguration), Name = "GADMediationRewardedAdConfiguration")]
    interface MediationRewardedAdConfiguration
    {
    }
    
    // typedef id<GADMediationRewardedAdEventDelegate> _Nullable (^GADMediationRewardedLoadCompletionHandler)(id<GADMediationRewardedAd> _Nullable, NSError *_Nullable)
    delegate MediationRewardedAdEventDelegate MediationRewardedLoadCompletionHandler([NullAllowed] MediationRewardedAd ad, [NullAllowed] NSError error);
    
    // @protocol GADMediationRewardedAdEventDelegate <GADMediationAdEventDelegate>
    [Model]
    [Protocol]
    [BaseType(typeof(MediationAdEventDelegate), Name = "GADMediationRewardedAdEventDelegate")]
    interface MediationRewardedAdEventDelegate
    {
        // - (void)didRewardUser;
        [Abstract]
        [Export("didRewardUser")]
        void DidRewardUser();
        
        // - (void)didStartVideo;
        [Abstract]
        [Export("didStartVideo")]
        void DidStartVideo();
        
        // - (void)didEndVideo;
        [Abstract]
        [Export("didEndVideo")]
        void DidEndVideo();
    }
    
    // @protocol GADMediationRewardedAd <GADMediationAd>
    [BaseType(typeof(NSObject), Name = "GADMediationRewardedAd")]
    [Protocol]
    [Model]
    interface MediationRewardedAd : MediationAd
    {
        // - (void)presentFromViewController:(nonnull UIViewController *)viewController;
        [Abstract]
        [Export("presentFromViewController:")]
        void PresentFromViewController(UIViewController viewController);
    }
    
    // @interface GADMediationAppOpenAdConfiguration : GADMediationAdConfiguration
    [BaseType(typeof(MediationAdConfiguration), Name = "GADMediationAppOpenAdConfiguration")]
    interface MediationAppOpenAdConfiguration
    {
    }
    
    // typedef id<GADMediationAppOpenAdEventDelegate> _Nullable (^GADMediationAppOpenLoadCompletionHandler)(id<GADMediationAppOpenAd> _Nullable, NSError *_Nullable)
    delegate MediationAppOpenAdEventDelegate MediationAppOpenLoadCompletionHandler([NullAllowed] MediationAppOpenAd ad, [NullAllowed] NSError error);
    
    // @protocol GADMediationAppOpenAdEventDelegate <GADMediationAdEventDelegate>
    [Model]
    [Protocol]
    [BaseType(typeof(MediationAdEventDelegate), Name = "GADMediationAppOpenAdEventDelegate")]
    interface MediationAppOpenAdEventDelegate
    {
    }
    
    // @protocol GADMediationAppOpenAd <GADMediationAd>
    [BaseType(typeof(NSObject), Name = "GADMediationAppOpenAd")]
    [Protocol] 
    [Model]
    interface MediationAppOpenAd : MediationAd
    {
        // - (void)presentFromViewController:(nonnull UIViewController *)viewController;
        [Abstract]
        [Export("presentFromViewController:")]
        void PresentFromViewController(UIViewController viewController);
    }
    
    
    [Obsolete("Use MediationAdapter instead.")]
    interface ICustomEventBanner
    {

    }

    [Obsolete("Use MediationAdapter instead.")]
    [Protocol(Name = "GADCustomEventBanner")]
    interface CustomEventBanner
    {
        [Abstract]
        [Export("requestBannerAd:parameter:label:request:")]
        [Obsolete("Use MediationAdapter instead.")]
        void RequestBannerAd(AdSize adSize, [NullAllowed] string serverParameter, [NullAllowed] string serverLabel, CustomEventRequest request);

        [Abstract]
        [return: NullAllowed]
        [Export("delegate")]
        ICustomEventBannerDelegate GetDelegate();

        [Abstract]
        [Export("setDelegate:")]
        void SetDelegate([NullAllowed] ICustomEventBannerDelegate aDelegate);
    }

    [Obsolete("Use MediationAdapter instead.")]
    interface ICustomEventBannerDelegate
    {
    }

    [Model]
    [Protocol]
    [BaseType(typeof(NSObject), Name = "GADCustomEventBannerDelegate")]
    [Obsolete("Use MediationAdapter instead.")]
    interface CustomEventBannerDelegate
    {
        [Abstract]
        [Export("customEventBanner:didReceiveAd:")]
        void DidReceiveAd(ICustomEventBanner customEvent, UIView view);

        [Abstract]
        [Export("customEventBanner:didFailAd:")]
        void DidFailAd(ICustomEventBanner customEvent, [NullAllowed] NSError error);

        [Abstract]
        [Export("customEventBannerWasClicked:")]
        void DidClickInAd(ICustomEventBanner customEvent);

        [Abstract]
        [Export("viewControllerForPresentingModalView")]
        UIViewController ViewControllerForPresentingModalView();

        [Abstract]
        [Export("customEventBannerWillPresentModal:")]
        void WillPresentModal(ICustomEventBanner customEvent);

        [Abstract]
        [Export("customEventBannerWillDismissModal:")]
        void WillDismissModal(ICustomEventBanner customEvent);

        [Abstract]
        [Export("customEventBannerDidDismissModal:")]
        void DidDismissModal(ICustomEventBanner customEvent);

        [Obsolete("Deprecated. No replacement.")]
        [Abstract]
        [Export("customEventBannerWillLeaveApplication:")]
        void WillLeaveApplication(ICustomEventBanner customEvent);
    }

    [BaseType(typeof(NSObject), Name = "GADCustomEventExtras")]
    [Obsolete("Use MediationAdapter instead.")]
    interface CustomEventExtras : AdNetworkExtras
    {

        [Export("setExtras:forLabel:")]
        [PostGet("AllExtras")]
        void SetExtras([NullAllowed] NSDictionary extras, string label);

        [return: NullAllowed]
        [Export("extrasForLabel:")]
        NSDictionary ExtrasForLabel(string label);

        [Export("removeAllExtras")]
        [PostGet("AllExtras")]
        void RemoveAllExtras();

        [Export("allExtras")]
        NSDictionary AllExtras { get; }
    }

    [Obsolete("Use MediationAdapter instead.")]
    interface ICustomEventInterstitial
    {

    }

    [Obsolete("Use MediationAdapter instead.")]
    [Protocol(Name = "GADCustomEventInterstitial")]
    interface CustomEventInterstitial
    {

        [Abstract]
        [return: NullAllowed]
        [Export("delegate")]
        ICustomEventInterstitialDelegate GetDelegate();

        [Abstract]
        [Export("setDelegate:")]
        void SetDelegate([NullAllowed] ICustomEventInterstitialDelegate aDelegate);

        [Abstract]
        [Export("requestInterstitialAdWithParameter:label:request:")]
        [Obsolete("Use MediationAdapter instead.")]
        void RequestInterstitialAd([NullAllowed] string serverParameter, [NullAllowed] string serverLabel, CustomEventRequest request);

        [Abstract]
        [Export("presentFromRootViewController:")]
        [Obsolete("Use MediationAdapter instead.")]
        void PresentFromRootViewController([NullAllowed] UIViewController rootViewController);
    }

    [Obsolete("Use MediationAdapter instead.")]
    interface ICustomEventInterstitialDelegate
    {
    }

    [Obsolete("Use MediationAdapter instead.")]
    [Model]
    [Protocol]
    [BaseType(typeof(NSObject), Name = "GADCustomEventInterstitialDelegate")]
    interface CustomEventInterstitialDelegate
    {
        [Export("customEventInterstitialDidReceiveAd:")]
        void DidReceiveAd(ICustomEventInterstitial customEvent);

        [Export("customEventInterstitial:didFailAd:")]
        void DidFailAd(ICustomEventInterstitial customEvent, [NullAllowed] NSError error);

        [Export("customEventInterstitialWasClicked:")]
        void DidClickAd(ICustomEventInterstitial customEvent);

        [Export("customEventInterstitialWillPresent:")]
        void WillPresent(ICustomEventInterstitial customEvent);

        [Export("customEventInterstitialWillDismiss:")]
        void WillDismiss(ICustomEventInterstitial customEvent);

        [Export("customEventInterstitialDidDismiss:")]
        void DidDismiss(ICustomEventInterstitial customEvent);

        [Obsolete("Deprecated. No replacement.")]
        [Export("customEventInterstitialWillLeaveApplication:")]
        void WillLeaveApplication(ICustomEventInterstitial customEvent);
    }

    [Obsolete("Use MediationAdapter instead.")]
    interface ICustomEventNativeAd
    {
    }

    // @protocol GADCustomEventNativeAd <NSObject>
    [Obsolete("Use MediationAdapter instead.")]
    [Model]
    [Protocol]
    [BaseType(typeof(NSObject), Name = "GADCustomEventNativeAd")]
    interface CustomEventNativeAd
    {
        // @required -(void)requestNativeAdWithParameter:(NSString *)serverParameter request:(GADCustomEventRequest *)request adTypes:(NSArray *)adTypes options:(NSArray *)options rootViewController:(UIViewController *)rootViewController;
        [Abstract]
        [Export("requestNativeAdWithParameter:request:adTypes:options:rootViewController:")]
        [Obsolete("Use MediationAdapter instead.")]
        void Request(string serverParameter, CustomEventRequest request, NSString[] adTypes, NSNumber[] options, UIViewController rootViewController);

        // - (BOOL)handlesUserClicks;
        [Obsolete("Use MediationAdapter instead.")]
        [Abstract]
        [Export("handlesUserClicks")]
        bool HandlesUserClicks();

        // - (BOOL)handlesUserImpressions;
        [Obsolete("Use MediationAdapter instead.")]
        [Abstract]
        [Export("handlesUserImpressions")]
        bool HandlesUserImpressions();

        // @required @property (nonatomic, weak) id<GADCustomEventNativeAdDelegate> _Nullable delegate;
        [Abstract]
        [return: NullAllowed]
        [Export("delegate")]
        ICustomEventNativeAdDelegate GetDelegate();

        [Abstract]
        [Export("setDelegate:")]
        void SetDelegate([NullAllowed] ICustomEventNativeAdDelegate aDelegate);
    }

    [Obsolete("Use MediationAdapter instead.")]
    interface ICustomEventNativeAdDelegate
    {
    }

    [Obsolete("Use MediationAdapter instead.")]
    [Model]
    [Protocol]
    [BaseType(typeof(NSObject), Name = "GADCustomEventNativeAdDelegate")]
    interface CustomEventNativeAdDelegate
    {
        // @required -(void)customEventNativeAd:(id<GADCustomEventNativeAd>)customEventNativeAd didFailToLoadWithError:(NSError *)error;
        [Abstract]
        [Export("customEventNativeAd:didFailToLoadWithError:")]
        void DidFailToLoad(ICustomEventNativeAd customEventNativeAd, NSError error);

        // - (void)customEventNativeAd:(id<GADCustomEventNativeAd>)customEventNativeAd didReceiveMediatedUnifiedNativeAd:(id<GADMediatedUnifiedNativeAd>) mediatedUnifiedNativeAd;
        [Abstract]
        [Export("customEventNativeAd:didReceiveMediatedUnifiedNativeAd:")]
        void DidReceiveMediatedUnifiedNativeAd(ICustomEventNativeAd customEventNativeAd, Mediation.IMediatedUnifiedNativeAd mediatedUnifiedNativeAd);
    }

    [Obsolete("Use MediationAdapter instead.")]
    [BaseType(typeof(NSObject), Name = "GADCustomEventRequest")]
    interface CustomEventRequest
    {
        [NullAllowed]
        [Export("userKeywords", ArgumentSemantic.Copy)]
        NSObject[] UserKeywords { get; }

        [NullAllowed]
        [Export("additionalParameters", ArgumentSemantic.Copy)]
        NSDictionary AdditionalParameters { get; }

        [Export("isTesting", ArgumentSemantic.Assign)]
        bool IsTesting { get; }
    }

    interface IDebugOptionsViewControllerDelegate
    {
    }

    // @protocol GADDebugOptionsViewControllerDelegate<NSObject>
    [Model]
    [Protocol]
    [BaseType(typeof(NSObject), Name = "GADDebugOptionsViewControllerDelegate")]
    interface DebugOptionsViewControllerDelegate
    {
        // - (void)debugOptionsViewControllerDidDismiss:(GADDebugOptionsViewController*)controller;
        [Abstract]
        [EventArgs("DebugOptionsViewControllerDismissed")]
        [EventName("Dismissed")]
        [Export("debugOptionsViewControllerDidDismiss:")]
        void DidDismiss(DebugOptionsViewController controller);
    }

    // @interface GADDebugOptionsViewController : UIViewController
    [DisableDefaultCtor]
    [BaseType(typeof(UIViewController), Name = "GADDebugOptionsViewController")]
    interface DebugOptionsViewController
    {
        // + (instancetype)debugOptionsViewControllerWithAdUnitID:(NSString*)adUnitID;
        [Static]
        [Export("debugOptionsViewControllerWithAdUnitID:")]
        DebugOptionsViewController GetInstance(string adUnitId);

        // @property(nonatomic, weak, GAD_NULLABLE) IBOutlet id<GADDebugOptionsViewControllerDelegate> delegate;
        [NullAllowed]
        [Export("delegate", ArgumentSemantic.Weak)]
        IDebugOptionsViewControllerDelegate Delegate { get; set; }
    }

    // @interface GADDisplayAdMeasurement : NSObject
    [BaseType(typeof(NSObject), Name = "GADDisplayAdMeasurement")]
    interface DisplayAdMeasurement
    {
        // @property (nonatomic, weak) UIView * _Nullable view;
        [NullAllowed]
        [Export("view", ArgumentSemantic.Weak)]
        UIView View { get; set; }

        // -(BOOL)startWithError:(NSError * _Nullable * _Nullable)error;
        [Export("startWithError:")]
        bool Start([NullAllowed] out NSError error);
    }

    // @interface GADAdapterStatus : NSObject <NSCopying>
    [BaseType(typeof(NSObject), Name = "GADAdapterStatus")]
    interface AdapterStatus : INSCopying
    {
        // @property (readonly, nonatomic) GADAdapterInitializationState state;
        [Export("state")]
        AdapterInitializationState State { get; }

        // @property (readonly, nonatomic) NSString * _Nonnull description;
        [Export("description")]
        string Description { get; }

        // @property (readonly, nonatomic) NSTimeInterval latency;
        [Export("latency")]
        double Latency { get; }
    }

    // @interface GADInitializationStatus : NSObject <NSCopying>
    [BaseType(typeof(NSObject), Name = "GADInitializationStatus")]
    interface InitializationStatus : INSCopying
    {
        // @property (readonly, nonatomic) NSDictionary<NSString *,GADAdapterStatus *> * _Nonnull adapterStatusesByClassName;
        [Export("adapterStatusesByClassName")]
        NSDictionary<NSString, AdapterStatus> AdapterStatusesByClassName { get; }
    }

    // @interface GADMuteThisAdReason : NSObject
    [BaseType(typeof(NSObject), Name = "GADMuteThisAdReason")]
    interface MuteThisAdReason
    {
        // @property(nonatomic, readonly, nonnull) NSString *reasonDescription;
        [Export("reasonDescription")]
        string ReasonDescription { get; }
    }

    #endregion

    // @interface GADMediaView : UIView
    [BaseType(typeof(UIView), Name = "GADMediaView")]
    interface MediaView
    {
        // @property (nonatomic, nullable) GADMediaContent* mediaContent;
        [NullAllowed]
        [Export("mediaContent")]
        MediaContent MediaContent { get; set; }
    }

    // @interface GADNativeMuteThisAdLoaderOptions : GADAdLoaderOptions
    [BaseType(typeof(AdLoaderOptions), Name = "GADNativeMuteThisAdLoaderOptions")]
    interface NativeMuteThisAdLoaderOptions
    {
        // @property(nonatomic) BOOL customMuteThisAdRequested;
        [Export("customMuteThisAdRequested", ArgumentSemantic.Assign)]
        bool CustomMuteThisAdRequested { get; set; }
    }
}

namespace Google.MobileAds.DoubleClick
{
    #region DoubleClick

    interface IBannerAdLoaderDelegate { }

    // @protocol GAMBannerAdLoaderDelegate<GADAdLoaderDelegate>
    [Model]
    [Protocol]
    [BaseType(typeof(NSObject), Name = "GAMBannerAdLoaderDelegate")]
    interface BannerAdLoaderDelegate : Google.MobileAds.AdLoaderDelegate
    {
        // - (NSArray<NSValue *> *)validBannerSizesForAdLoader:(GADAdLoader *)adLoader;
        [Abstract]
        [Export("validBannerSizesForAdLoader:")]
        NSValue[] ValidBannerSizes(Google.MobileAds.AdLoader adLoader);

        // - (void)adLoader:(GADAdLoader *)adLoader didReceiveGAMBannerView:(GAMBannerView *)bannerView;
        [Abstract]
        [Export("adLoader:didReceiveGAMBannerView:")]
        void DidReceiveBannerView(Google.MobileAds.AdLoader adLoader, BannerView bannerView);
    }

    interface IAdSizeDelegate
    {
    }

    [Model]
    [Protocol]
    [BaseType(typeof(NSObject), Name = "GADAdSizeDelegate")]
    interface AdSizeDelegate
    {
        [Abstract]
        [EventArgs("AdSizeDelegateSize")]
        [Export("adView:willChangeAdSizeTo:")]
        void WillChangeAdSizeTo(BannerView view, AdSize size);
    }

    [BaseType(typeof(Google.MobileAds.BannerView), Name = "GAMBannerView")]
    interface BannerView
    {
        [Export("initWithFrame:")]
        NativeHandle Constructor(CGRect frame);

        [Export("initWithAdSize:origin:")]
        NativeHandle Constructor(AdSize size, CGPoint origin);

        [Export("initWithAdSize:")]
        NativeHandle Constructor(AdSize size);

        [New]
        [NullAllowed]
        [Export("adUnitID", ArgumentSemantic.Copy)]
        string AdUnitID { get; set; }

        [NullAllowed]
        [Export("appEventDelegate", ArgumentSemantic.Weak)]
        IAppEventDelegate AppEventDelegate { get; set; }

        [New]
        [NullAllowed]
        [Export("adSizeDelegate", ArgumentSemantic.Weak)]
        IAdSizeDelegate AdSizeDelegate { get; set; }

        [NullAllowed]
        [Export("validAdSizes", ArgumentSemantic.Copy)]
        NSValue[] ValidAdSizes { get; set; }

        [Export("enableManualImpressions")]
        bool EnableManualImpressions { get; set; }

        // @property(nonatomic, readonly, nonnull) GADVideoController *videoController;
        [Export("videoController")]
        Google.MobileAds.VideoController VideoController { get; }

        [Export("recordImpression")]
        void RecordImpression();

        [Export("resize:")]
        void Resize(AdSize size);

        // - (void)setAdOptions:(NSArray *)adOptions;
        [Export("setAdOptions:")]
        void SetAdOptions(AdLoaderOptions[] adOptions);

        [Internal]
        [Export("setValidAdSizesWithSizes:", IsVariadic = true)]
        void SetValidAdSizes(AdSize firstSize, IntPtr sizesPtr);
    }

    // @interface GAMBannerViewOptions : GADAdLoaderOptions
    [BaseType(typeof(AdLoaderOptions), Name = "GAMBannerViewOptions")]
    interface BannerViewOptions
    {
        // @property(nonatomic, weak, GAD_NULLABLE) id<GADAppEventDelegate> appEventDelegate;
        [NullAllowed]
        [Export("appEventDelegate", ArgumentSemantic.Weak)]
        Google.MobileAds.IAppEventDelegate AppEventDelegate { get; set; }

        // @property(nonatomic, weak, GAD_NULLABLE) id<GADAdSizeDelegate> adSizeDelegate;
        [NullAllowed]
        [Export("adSizeDelegate", ArgumentSemantic.Weak)]
        Google.MobileAds.IAdSizeDelegate AdSizeDelegate { get; set; }

        // @property(nonatomic, assign, getter=isManualImpressionEnabled) BOOL manualImpressionEnabled;
        [Export("manualImpressionEnabled", ArgumentSemantic.Assign)]
        bool EnableManualImpressions { get; set; }
    }

    [DisableDefaultCtor]
    [BaseType(typeof(Google.MobileAds.InterstitialAd),
        Name = "GAMInterstitialAd")]
    interface InterstitialAd
    {
        // + (void)loadWithAdManagerAdUnitID:(nonnull NSString *)adUnitID request:(nullable GAMRequest *)request completionHandler:(nonnull GAMInterstitialAdLoadCompletionHandler)completionHandler;
        [Async]
        [Static]
        [Export("loadWithAdManagerAdUnitID:request:completionHandler:")]
        void LoadWithAdManagerAdUnitID(string adUnitId, [NullAllowed] Request request, InterstitialAdLoadCompletionHandler completionHandler);

        // + (void)loadWithAdUnitID:(nonnull NSString *)adUnitID request:(nullable GADRequest *)request completionHandler:(nonnull GADInterstitialAdLoadCompletionHandler)completionHandler;
        [Async]
        [Static]
        [Export("loadWithAdUnitID:request:completionHandler:")]
        void Load(string adUnitId, [NullAllowed] Request request, InterstitialAdLoadCompletionHandler completionHandler);

        [NullAllowed]
        [Export("appEventDelegate", ArgumentSemantic.Weak)]
        IAppEventDelegate AppEventDelegate { get; set; }
    }

    [BaseType(typeof(Google.MobileAds.Request), Name = "GAMRequest")]
    interface Request
    {
        [New]
        [Static]
        [Export ("request")]
        Request GetDefaultRequest ();
        
        [NullAllowed]
        [Export("publisherProvidedID", ArgumentSemantic.Copy)]
        string PublisherProvidedID { get; set; }

        [NullAllowed]
        [Export("categoryExclusions", ArgumentSemantic.Copy)]
        string[] CategoryExclusions { get; set; }
    }

    #endregion
}

namespace Google.MobileAds.Mediation
{
    interface IMediatedUnifiedNativeAd { }

    // @protocol GADMediatedUnifiedNativeAd <NSObject>
    [Model]
    [Protocol]
    [BaseType(typeof(NSObject), Name = "GADMediatedUnifiedNativeAd")]
    interface MediatedUnifiedNativeAd
    {
        // @required @property (readonly, copy, nonatomic) NSString * _Nullable headline;
        [Abstract]
        [NullAllowed]
        [Export("headline")]
        string Headline { get; }

        // @required @property (readonly, nonatomic) NSArray<GADNativeAdImage *> * _Nullable images;
        [Abstract]
        [NullAllowed]
        [Export("images")]
        NativeAdImage[] Images { get; }

        // @required @property (readonly, copy, nonatomic) NSString * _Nullable body;
        [Abstract]
        [NullAllowed]
        [Export("body")]
        string Body { get; }

        // @required @property (readonly, nonatomic) GADNativeAdImage * _Nullable icon;
        [Abstract]
        [NullAllowed]
        [Export("icon")]
        NativeAdImage Icon { get; }

        // @required @property (readonly, copy, nonatomic) NSString * _Nullable callToAction;
        [Abstract]
        [NullAllowed]
        [Export("callToAction")]
        string CallToAction { get; }

        // @required @property (readonly, copy, nonatomic) NSDecimalNumber * _Nullable starRating;
        [Abstract]
        [NullAllowed]
        [Export("starRating", ArgumentSemantic.Copy)]
        NSDecimalNumber StarRating { get; }

        // @required @property (readonly, copy, nonatomic) NSString * _Nullable store;
        [Abstract]
        [NullAllowed]
        [Export("store")]
        string Store { get; }

        // @required @property (readonly, copy, nonatomic) NSString * _Nullable price;
        [Abstract]
        [NullAllowed]
        [Export("price")]
        string Price { get; }

        // @required @property (readonly, copy, nonatomic) NSString * _Nullable advertiser;
        [Abstract]
        [NullAllowed]
        [Export("advertiser")]
        string Advertiser { get; }

        // @required @property (readonly, copy, nonatomic) NSDictionary<NSString *,id> * _Nullable extraAssets;
        [Abstract]
        [NullAllowed]
        [Export("extraAssets", ArgumentSemantic.Copy)]
        NSDictionary<NSString, NSObject> ExtraAssets { get; }

        // @optional @property (readonly, nonatomic) UIView * _Nullable adChoicesView;
        [return: NullAllowed]
        [Export("adChoicesView")]
        UIView GetAdChoicesView();

        // @optional @property (readonly, nonatomic) UIView * _Nullable mediaView;
        [return: NullAllowed]
        [Export("mediaView")]
        UIView GetMediaView();

        // @optional @property (readonly, assign, nonatomic) BOOL hasVideoContent;
        [Export("hasVideoContent")]
        bool HasVideoContent();

        // @optional @property (readonly, nonatomic) CGFloat mediaContentAspectRatio;
        [Export("mediaContentAspectRatio")]
        nfloat GetMediaContentAspectRatio();

        // @optional @property (readonly, nonatomic) NSTimeInterval duration;
        [Export("duration")]
        double GetDuration();

        // @optional @property (readonly, nonatomic) NSTimeInterval currentTime;
        [Export("currentTime")]
        double GetCurrentTime();

        // @optional -(void)didRenderInView:(UIView * _Nonnull)view clickableAssetViews:(NSDictionary<GADNativeAssetIdentifier,UIView *> * _Nonnull)clickableAssetViews nonclickableAssetViews:(NSDictionary<GADNativeAssetIdentifier,UIView *> * _Nonnull)nonclickableAssetViews viewController:(UIViewController * _Nonnull)viewController;
        [Export("didRenderInView:clickableAssetViews:nonclickableAssetViews:viewController:")]
        void DidRenderInView(UIView view, NSDictionary<NSString, UIView> clickableAssetViews, NSDictionary<NSString, UIView> nonclickableAssetViews, UIViewController viewController);

        // @optional -(void)didRecordImpression;
        [Export("didRecordImpression")]
        void DidRecordImpression();

        // @optional -(void)didRecordClickOnAssetWithName:(GADNativeAssetIdentifier _Nonnull)assetName view:(UIView * _Nonnull)view viewController:(UIViewController * _Nonnull)viewController;
        [Export("didRecordClickOnAssetWithName:view:viewController:")]
        void DidRecordClick(string assetName, UIView view, UIViewController viewController);

        // @optional -(void)didUntrackView:(UIView * _Nullable)view;
        [Export("didUntrackView:")]
        void DidUntrackView([NullAllowed] UIView view);
    }
}
