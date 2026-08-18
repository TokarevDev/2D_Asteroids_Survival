using System;
using Game.Core.Advertising;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using UnityEngine;
using Zenject;

namespace Game.Infrastructure.Advertising
{
    public sealed class BannerAdvertisementService : IAdvertisementService, IInitializable, IDisposable
    {
        private readonly AdMobConfiguration _configuration;
        private readonly AdMobInitializer _initializer;

        private BannerView _bannerView;

        private bool _isBannerRequested;
        private bool _isDisposed;

        public BannerAdvertisementService(AdMobConfiguration configuration, AdMobInitializer initializer)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

            _initializer = initializer ?? throw new ArgumentNullException(nameof(initializer));
        }

        public void Initialize()
        {
            _initializer.Initialized += OnAdMobInitialized;

            if (_initializer.IsInitialized)
            {
                OnAdMobInitialized();
            }
        }

        public void Dispose()
        {
            _isDisposed = true;
            _initializer.Initialized -= OnAdMobInitialized;

            DestroyBanner();
        }

        public void ShowBanner()
        {
            _isBannerRequested = true;

            if (!_initializer.IsInitialized || _isDisposed)
            {
                return;
            }

            CreateBannerIfRequired();

            if (_bannerView != null)
            {
                _bannerView.Show();
            }
        }

        public void HideBanner()
        {
            _isBannerRequested = false;
            DestroyBanner();
        }

        private void OnAdMobInitialized()
        {
            if (_isDisposed || !_isBannerRequested)
            {
                return;
            }

            ShowBanner();
        }

        private void CreateBannerIfRequired()
        {
            if (_bannerView != null)
            {
                return;
            }

            string bannerAdUnitId = _configuration.BannerAdUnitId;
            if (string.IsNullOrWhiteSpace(bannerAdUnitId))
            {
                Debug.LogWarning("AdMob banner unit ID is not configured for the current platform");
                return;
            }

            _bannerView = new BannerView(bannerAdUnitId, AdSize.Banner, AdPosition.Bottom);

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
