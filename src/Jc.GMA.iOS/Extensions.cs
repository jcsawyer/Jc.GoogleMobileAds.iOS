using System;
using System.Runtime.InteropServices;

using ObjCRuntime;
using Foundation;
using CoreGraphics;
using UIKit;

namespace Google.MobileAds {
	public partial class Request {
		public static readonly string GADGoogleAdMobNetworkName = "GoogleAdMobAds";
	}

	[Preserve (AllMembers = true)]
	public partial class AdSizeCons {
		// GAD_EXTERN GADAdSize GADPortraitInlineAdaptiveBannerAdSizeWithWidth(CGFloat width);
		[DllImport ("__Internal", EntryPoint = "GADPortraitInlineAdaptiveBannerAdSizeWithWidth")]
		public static extern AdSize GetPortraitInlineAdaptiveBannerAdSize (nfloat width);

		// GAD_EXTERN GADAdSize GADLandscapeInlineAdaptiveBannerAdSizeWithWidth(CGFloat width);
		[DllImport ("__Internal", EntryPoint = "GADLandscapeInlineAdaptiveBannerAdSizeWithWidth")]
		public static extern AdSize GetLandscapeInlineAdaptiveBannerAdSize (nfloat width);

		// GAD_EXTERN GADAdSize GADCurrentOrientationInlineAdaptiveBannerAdSizeWithWidth(CGFloat width);
		[DllImport ("__Internal", EntryPoint = "GADCurrentOrientationInlineAdaptiveBannerAdSizeWithWidth")]
		public static extern AdSize GetCurrentOrientationInlineAdaptiveBannerAdSizeh (nfloat width);

		// GAD_EXTERN GADAdSize GADInlineAdaptiveBannerAdSizeWithWidthAndMaxHeight(CGFloat width, CGFloat maxHeight);
		[DllImport ("__Internal", EntryPoint = "GADInlineAdaptiveBannerAdSizeWithWidthAndMaxHeight")]
		public static extern AdSize GetInlineAdaptiveBannerAdSizeWithMaxHeight (nfloat width, nfloat maxHeight);

		//GAD_EXTERN GADAdSize GADPortraitAnchoredAdaptiveBannerAdSizeWithWidth (CGFloat width);
		[DllImport ("__Internal", EntryPoint = "GADPortraitAnchoredAdaptiveBannerAdSizeWithWidth")]
		public static extern AdSize GetPortraitAnchoredAdaptiveBannerAdSize (nfloat width);

		//GAD_EXTERN GADAdSize GADLandscapeAnchoredAdaptiveBannerAdSizeWithWidth (CGFloat width);
		[DllImport ("__Internal", EntryPoint = "GADLandscapeAnchoredAdaptiveBannerAdSizeWithWidth")]
		public static extern AdSize GetLandscapeAnchoredAdaptiveBannerAdSize (nfloat width);

		//GAD_EXTERN GADAdSize GADCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth (CGFloat width);
		[DllImport ("__Internal", EntryPoint = "GADCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth")]
		public static extern AdSize GetCurrentOrientationAnchoredAdaptiveBannerAdSize (nfloat width);

		// GADAdSize GADAdSizeFromCGSize(CGSize size);
		[DllImport ("__Internal", EntryPoint = "GADAdSizeFromCGSize")]
		public static extern AdSize GetFromCGSize (CGSize size);

		// GADAdSize GADAdSizeFullWidthPortraitWithHeight(CGFloat height);
		[DllImport ("__Internal", EntryPoint = "GADAdSizeFullWidthPortraitWithHeight")]
		public static extern AdSize GetFullWidthPortrait (nfloat height);

		// GADAdSize GADAdSizeFullWidthLandscapeWithHeight (CGFloat height);
		[DllImport ("__Internal", EntryPoint = "GADAdSizeFullWidthLandscapeWithHeight")]
		public static extern AdSize GetFullWidthLandscape (nfloat height);

		// BOOL GADAdSizeEqualToSize(GADAdSize size1, GADAdSize size2);
		[DllImport ("__Internal", EntryPoint = "GADAdSizeEqualToSize")]
		public static extern bool Equals (AdSize size, AdSize toSize);

		// CGSize CGSizeFromGADAdSize(GADAdSize size);
		[DllImport ("__Internal", EntryPoint = "CGSizeFromGADAdSize")]
		public static extern CGSize GetCGSize (AdSize size);

		// BOOL IsGADAdSizeValid(GADAdSize size);
		[DllImport ("__Internal", EntryPoint = "IsGADAdSizeValid")]
		public static extern bool IsAdSizeValid (AdSize size);

		// GAD_EXTERN BOOL GADAdSizeIsFluid(GADAdSize size);
		[DllImport ("__Internal", EntryPoint = "GADAdSizeIsFluid")]
		public static extern bool AdSizeIsFluid (AdSize size);

		// NSString *NSStringFromGADAdSize(GADAdSize size);
		[DllImport ("__Internal", EntryPoint = "NSStringFromGADAdSize")]
		static extern IntPtr _GetNSString (AdSize size);

		// NSValue *NSValueFromGADAdSize(GADAdSize size);
		[DllImport ("__Internal", EntryPoint = "NSValueFromGADAdSize")]
		static extern IntPtr _GetNSValue (AdSize size);

		// GADAdSize GADAdSizeFromNSValue(NSValue *value);
		[DllImport ("__Internal", EntryPoint = "GADAdSizeFromNSValue")]
		public static extern AdSize _GetFromNSValue (IntPtr value);
		
		public static NSString GetNSString (AdSize size)
		{
			return Runtime.GetNSObject<NSString> (_GetNSString (size));
		}

		public static NSValue GetNSValue (AdSize size)
		{
			return Runtime.GetNSObject<NSValue> (_GetNSValue (size));
		}

		public static AdSize GetFromNSValue (NSValue value)
		{
			return _GetFromNSValue (value.Handle);
		}

		public static AdSize Banner => new AdSize { Size = new CGSize (320, 50) };

		public static AdSize LargeBanner => new AdSize { Size = new CGSize (320, 100) };

		public static AdSize MediumRectangle => new AdSize { Size = new CGSize (300, 250) };

		public static AdSize FullBanner => new AdSize { Size = new CGSize (486, 60) };

		public static AdSize Leaderboard => new AdSize { Size = new CGSize (728, 90) };

		public static AdSize Skyscraper => new AdSize { Size = new CGSize (160, 600) };

		public static AdSize Invalid => new AdSize { Size = new CGSize (0, 0) };

		public static string GetString (AdSize size)
		{
			return GetNSString (size);
		}
	}
}