using System.Threading.Tasks;
using UnityEngine;

namespace UIFramework
{
    /// <summary>
    /// 场景引导脚本：初始化框架并打开一个示例页面。
    /// </summary>
    public sealed class HelloUIBootstrap : MonoBehaviour
    {
        private async void Start()
        {
            await BootstrapAsync();
        }

        private static async Task BootstrapAsync()
        {
            var uiManager = UIManager.Instance;
            uiManager.Init(new CodeViewLoader());
            uiManager.Register<SampleHelloPage>(new UIConfig
            {
                Id = "HelloPage",
                PrefabKey = "SampleHelloPage",
                Layer = UILayer.Normal,
                CacheOnClose = false,
                FullScreen = true,
            });

            await uiManager.OpenAsync<SampleHelloPage>("Hello UIFramework!");
        }
    }
}
