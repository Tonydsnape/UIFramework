using System.Threading.Tasks;
using UnityEngine;

namespace UIFramework
{
    /// <summary>
    /// 基于 Unity Resources 的默认加载器实现。
    /// 后续可新增 AddressablesLoader，实现同一接口无缝替换。
    /// </summary>
    public sealed class ResourcesLoader : IResourceLoader
    {
        public async Task<GameObject> LoadPrefabAsync(string key)
        {
            var request = Resources.LoadAsync<GameObject>(key);
            if (request.isDone)
            {
                return request.asset as GameObject;
            }

            var completion = new TaskCompletionSource<bool>();
            request.completed += _ => completion.TrySetResult(true);
            await completion.Task;

            return request.asset as GameObject;
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
