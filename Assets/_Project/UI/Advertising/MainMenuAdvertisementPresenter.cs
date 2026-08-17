using System;
using Game.Core.Advertising;
using UnityEngine;
using Zenject;

namespace Game.UI.Advertising
{
    public sealed class MainMenuAdvertisementPresenter : MonoBehaviour
    {
        private IAdvertisementService _advertisementService;

        [Inject]
        private void Construct(IAdvertisementService advertisementService)
        {
            _advertisementService = advertisementService
                ?? throw new ArgumentNullException(nameof(advertisementService));
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
