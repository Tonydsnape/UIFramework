using System.Threading.Tasks;
using UnityEngine;
using System.Collections.Generic;

namespace UIFramework
{
    /// <summary>
    /// 示例专用加载器：使用代码构造占位“Prefab”，避免提交二进制资源。
    /// 实际业务请替换为 ResourcesLoader / AddressablesLoader。
    /// </summary>
    public sealed class CodeViewLoader : IResourceLoader
    {
        private readonly Dictionary<string, GameObject> _prefabCache = new Dictionary<string, GameObject>();

        public Task<GameObject> LoadPrefabAsync(string key)
        {
            if (_prefabCache.TryGetValue(key, out var prefab) && prefab != null)
            {
                return Task.FromResult(prefab);
            }

            var viewRoot = new GameObject($"CodeView_{key}", typeof(RectTransform), typeof(UIView));
            var rect = viewRoot.GetComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
            viewRoot.SetActive(false);
            _prefabCache[key] = viewRoot;
            return Task.FromResult(viewRoot);
        }

        public void Release(string key, GameObject instance)
        {
            if (instance != null)
            {
                Object.Destroy(instance);
            }
        }
    }
}
