using UnityEngine;
using UnityApplication = UnityEngine.Application;

namespace Game.Infrastructure.Advertising
{
    [CreateAssetMenu(fileName = "AdMobConfiguration", menuName = "Game/Advertising/AdMob Configuration")]
    public sealed class AdMobConfiguration : ScriptableObject
    {
        [SerializeField] private string _androidBannerAdUnitId = "ca-app-pub-3940256099942544/6300978111";
        [SerializeField] private string _iosBannerAdUnitId = "ca-app-pub-3940256099942544/2934735716";

        public string BannerAdUnitId
        {
            get
            {
                switch (UnityApplication.platform)
                {
                    case RuntimePlatform.Android:
                        return _androidBannerAdUnitId;

                    case RuntimePlatform.IPhonePlayer:
                        return _iosBannerAdUnitId;

                    default:
                        return UnityApplication.isEditor
                            ? _androidBannerAdUnitId
                            : string.Empty;
                }
            }
        }
    }
}
