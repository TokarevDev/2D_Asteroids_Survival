using Game.Core.Application;
using UnityEngine;
using UnityApplication = UnityEngine.Application;

namespace Game.Infrastructure.Application
{
    public sealed class ApplicationQuitService : IApplicationQuitService
    {
        public void Quit()
        {
            UnityApplication.Quit();
        }
    }
}
