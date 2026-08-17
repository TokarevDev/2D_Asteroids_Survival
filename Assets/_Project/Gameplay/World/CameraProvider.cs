using System;
using UnityEngine;

namespace Game.Gameplay.World
{
    public sealed class CameraProvider
    {
        public Camera Camera { get; }

        public CameraProvider(Camera camera)
        {
            if (camera == null)
            {
                throw new ArgumentNullException(nameof(camera));
            }

            Camera = camera;
        }
    }
}
