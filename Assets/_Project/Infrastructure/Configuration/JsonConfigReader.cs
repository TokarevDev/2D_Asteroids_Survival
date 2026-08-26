using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine.Networking;
using UnityApplication = UnityEngine.Application;

namespace Game.Infrastructure.Configuration
{
    public sealed class JsonConfigReader
    {
        private static readonly JsonSerializerSettings _serializerSettings =
            new()
            {
                MissingMemberHandling = MissingMemberHandling.Error
            };

        private static readonly JsonLoadSettings _loadSettings =
            new()
            {
                DuplicatePropertyNameHandling =
                    DuplicatePropertyNameHandling.Error
            };

        public async UniTask<T> ReadAsync<T>(string relativePath, CancellationToken cancellationToken)
            where T : class
        {
            if (string.IsNullOrWhiteSpace(relativePath))
            {
                throw new ArgumentException("JSON path cannot be empty", nameof(relativePath));
            }

            string path = $"{UnityApplication.streamingAssetsPath}/{relativePath}";

            string requestPath = path.Contains("://") ? path : new Uri(path).AbsoluteUri;

            using UnityWebRequest request = UnityWebRequest.Get(requestPath);

            await request.SendWebRequest().ToUniTask(cancellationToken: cancellationToken);

            if (request.result != UnityWebRequest.Result.Success)
            {
                throw new InvalidOperationException($"Failed to load JSON '{relativePath}' : {request.error}");
            }

            string json = request.downloadHandler.text.TrimStart('\uFEFF');

            JToken root = JToken.Parse(json, _loadSettings);

            T result = root.ToObject<T>(JsonSerializer.Create(_serializerSettings));

            return result ?? throw new InvalidOperationException($"JSON '{relativePath}' contains no configuration");
        }
    }
}
