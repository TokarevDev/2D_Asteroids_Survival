using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace Game.Infrastructure.Configuration
{
    public sealed class JsonConfigReader
    {
        private static readonly JsonSerializerSettings _serializerSettings =
            new()
            {
                MissingMemberHandling = MissingMemberHandling.Error
            };

        public async UniTask<T> ReadAsync<T>(
            string relativePath, CancellationToken cancellationToken)
            where T : class
        {
            if (string.IsNullOrWhiteSpace(relativePath))
            {
                throw new ArgumentException("JSON path cannot be empty", nameof(relativePath));
            }

            string path = $"{Application.streamingAssetsPath}/{relativePath}";

            string requestPath = path.Contains("://") ? path : new Uri(path).AbsoluteUri;

            using UnityWebRequest request = UnityWebRequest.Get(requestPath);

            await request.SendWebRequest().ToUniTask(cancellationToken: cancellationToken);

            if (request.result != UnityWebRequest.Result.Success)
            {
                throw new InvalidOperationException($"Failed to load JSON '{relativePath}' : {request.error}");
            }

            string json = request.downloadHandler.text.TrimStart('\uFEFF');

            T result = JsonConvert.DeserializeObject<T>(json, _serializerSettings);

            return result ?? throw new InvalidOperationException($"JSON '{relativePath}' contains no configuration");
        }
    }
}
