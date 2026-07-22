using Game.Core;
using UnityEngine;
using Zenject;

namespace Game.UI
{
    public sealed class MainMenuAdvertisementPresenter : MonoBehaviour
    {
        private IAdvertisementService _advertisementService;

        [Inject]
        private void Construct(IAdvertisementService advertisementService)
        {
            _advertisementService = advertisementService;
        }

        private void Start()
        {
            _advertisementService.ShowBanner();
        }

        private void OnDestroy()
        {
            _advertisementService?.HideBanner();
        }
    }
}
