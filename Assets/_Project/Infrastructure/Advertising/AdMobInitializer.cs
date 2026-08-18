using System;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using UnityEngine;
using Zenject;

namespace Game.Infrastructure.Advertising
{
    public sealed class AdMobInitializer : IInitializable, IDisposable
    {
        public event Action Initialized;

        private bool _isDisposed;

        public bool IsInitialized { get; private set; }

        public void Initialize()
        {
            MobileAds.Initialize(OnMobileAdsInitialized);
        }

        public void Dispose()
        {
            _isDisposed = true;
            Initialized = null;
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
                    Debug.LogError("Google Mobile Ads initialization failed");
                    return;
                }

                IsInitialized = true;

                Debug.Log("Google Mobile Ads initialized");
                Initialized?.Invoke();
            });
        }
    }
}
