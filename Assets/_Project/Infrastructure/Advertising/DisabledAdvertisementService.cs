using Game.Core.Advertising;

namespace Game.Infrastructure.Advertising
{
    public sealed class DisabledAdvertisementService : IAdvertisementService
    {
        public void ShowBanner()
        {
        }

        public void HideBanner()
        {
        }
    }
}
