using Game.Core.Application;
using UnityEngine;

namespace Game.Infrastructure
{
    public sealed class ApplicationQuitService : IApplicationQuitService
    {
        public void Quit()
        {
            Application.Quit();
        }
    }
}
