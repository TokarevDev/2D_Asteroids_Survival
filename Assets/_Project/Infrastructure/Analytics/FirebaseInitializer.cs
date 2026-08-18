using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Firebase;
using UnityEngine;

namespace Game.Infrastructure.Analytics
{
    public sealed class FirebaseInitializer
    {
        public bool IsInitialized { get; private set; }

        public async UniTask InitializeAsync(CancellationToken cancellationToken)
        {
            if (IsInitialized)
            {
                return;
            }

            DependencyStatus dependencyStatus =
                await FirebaseApp.CheckAndFixDependenciesAsync()
                    .AsUniTask()
                    .AttachExternalCancellation(cancellationToken);

            if (dependencyStatus != DependencyStatus.Available)
            {
                throw new InvalidOperationException($"Firebase dependencies are unavailable: {dependencyStatus}");
            }

            IsInitialized = true;
            Debug.Log("Firebase initialized");
        }
    }
}
