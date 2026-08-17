using UnityEngine;
using Zenject;
using UnityApplication = UnityEngine.Application;

namespace Game.Infrastructure.Performance
{
    public sealed class MobileFrameRateService : IInitializable
    {
        private const int FallbackFrameRate = 60;
        private const int MaximumFrameRate = 120;

        public void Initialize()
        {
            if (!UnityApplication.isMobilePlatform)
            {
                return;
            }

            double displayRefreshRate = Screen.currentResolution.refreshRateRatio.value;

            int targetFrameRate = displayRefreshRate > 0d
                ? Mathf.RoundToInt((float)displayRefreshRate)
                : FallbackFrameRate;

            UnityApplication.targetFrameRate = Mathf.Min(targetFrameRate, MaximumFrameRate);
        }
    }
}
