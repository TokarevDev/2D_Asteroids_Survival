using System;
using Game.Core;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using UnityEngine;
using Zenject;

namespace Game.Infrastructure
{
    public sealed class AdMobAdvertisementService : IAdvertisementService, IInitializable, IDisposable
    {
#if UNITY_ANDROID
        private const string BannerAdUnitId =
            "ca-app-pub-3940256099942544/6300978111";
#elif UNITY_IOS
        private const string BannerAdUnitId =
            "ca-app-pub-3940256099942544/2934735716";
#else
        private const string BannerAdUnitId = "unused";
#endif

        private BannerView _bannerView;

        private bool _isInitialized;
        private bool _isBannerRequested;
        private bool _isDisposed;

        public void Initialize()
        {
            MobileAds.Initialize(OnMobileAdsInitialized);
        }

        public void Dispose()
        {
            _isDisposed = true;
            DestroyBanner();
        }

        public void ShowBanner()
        {
            _isBannerRequested = true;
            if (!_isInitialized || _isDisposed)
            {
                return;
            }

            CreateBannerIfRequired();
            _bannerView.Show();
        }

        public void HideBanner()
        {
            _isBannerRequested = false;
            DestroyBanner();
        }

        private void OnMobileAdsInitialized(InitializationStatus status)
        {
            MobileAdsEventExecutor.ExecuteInUpdate(() =>
            {
                if (_isDisposed)
                {
                    return;
                }

                if (status == null)
                {
                    Debug.LogError("Google Mobile Ads Initialization failed");
                    return;
                }

                _isInitialized = true;
                Debug.Log("Google Mobile Ads Initialized");
                if (_isBannerRequested)
                {
                    ShowBanner();
                }
            });
        }

        private void CreateBannerIfRequired()
        {
            if (_bannerView != null)
            {
                return;
            }

            _bannerView = new BannerView(BannerAdUnitId, AdSize.Banner, AdPosition.Bottom);

            _bannerView.OnBannerAdLoaded += OnBannerAdLoaded;
            _bannerView.OnBannerAdLoadFailed += OnBannerAdLoadFailed;

            _bannerView.LoadAd(new AdRequest());
        }

        private void OnBannerAdLoaded()
        {
            MobileAdsEventExecutor.ExecuteInUpdate(() =>
            {
                if (_isDisposed || _bannerView == null)
                {
                    return;
                }

                if (_isBannerRequested)
                {
                    _bannerView.Show();
                }
                else
                {
                    _bannerView.Hide();
                }

                Debug.Log("AdMob test banner loaded");
            });
        }

        private void OnBannerAdLoadFailed(LoadAdError error)
        {
            Debug.LogError($"AdMob banner loading failed: {error}");
        }

        private void DestroyBanner()
        {
            if (_bannerView == null)
            {
                return;
            }

            _bannerView.OnBannerAdLoaded -= OnBannerAdLoaded;
            _bannerView.OnBannerAdLoadFailed -= OnBannerAdLoadFailed;

            _bannerView.Destroy();
            _bannerView = null;
        }
    }
}
