using System.Threading.Tasks;
using UnityEngine;

namespace UIFramework
{
    /// <summary>
    /// 资源加载接口：用于隔离具体资源系统（Resources/Addressables）。
    /// </summary>
    public interface IResourceLoader
    {
        Task<GameObject> LoadPrefabAsync(string key);
        void Release(string key, GameObject instance);
    }
}
