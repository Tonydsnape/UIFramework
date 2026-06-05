using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace UIFramework
{
    /// <summary>
    /// UI 框架核心调度器：负责注册、打开、关闭与生命周期驱动。
    /// </summary>
    public sealed class UIManager
    {
        private static readonly UIManager _instance = new UIManager();

        private readonly Dictionary<Type, UIConfig> _configRegistry = new Dictionary<Type, UIConfig>();
        private readonly Dictionary<Type, BaseContext> _activeContexts = new Dictionary<Type, BaseContext>();
        private readonly Dictionary<BaseContext, string> _contextPrefabKeys = new Dictionary<BaseContext, string>();

        private IResourceLoader _resourceLoader;
        private UILayerManager _layerManager;
        private UIRoot _uiRoot;

        private UIManager() { }

        public static UIManager Instance => _instance;

        /// <summary>
        /// 依赖注入初始化：由外部传入资源加载实现。
        /// </summary>
        public void Init(IResourceLoader loader)
        {
            _resourceLoader = loader ?? throw new ArgumentNullException(nameof(loader));
            _uiRoot = UIRoot.Instance;
            _layerManager = new UILayerManager(_uiRoot);

            // TODO(P2): 接入页面导航栈（UINavigator）管理 Page Push/Pop。
            // TODO(P4): 接入对象池（IUIPool）替代直接实例化/销毁。
            // TODO(P5): 接入消息中心（IMessageCenter）以支持解耦通信。
        }

        /// <summary>
        /// 注册 Context 配置。
        /// </summary>
        public void Register<T>(UIConfig config) where T : BaseContext
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            _configRegistry[typeof(T)] = config;
        }

        /// <summary>
        /// 打开（或刷新）指定 Context。
        /// </summary>
        public async Task<T> OpenAsync<T>(object args = null) where T : BaseContext
        {
            EnsureInitialized();
            var type = typeof(T);
            var config = GetConfig(type);

            if (_activeContexts.TryGetValue(type, out var existedContext))
            {
                BringToFront(existedContext, config.Layer);
                if (existedContext.ViewObject != null)
                {
                    existedContext.ViewObject.SetActive(true);
                }

                existedContext.OnShow(args);
                return (T)existedContext;
            }

            var prefab = await _resourceLoader.LoadPrefabAsync(config.PrefabKey);
            if (prefab == null)
            {
                throw new InvalidOperationException($"无法加载 UI Prefab，Key: {config.PrefabKey}");
            }

            var instance = UnityEngine.Object.Instantiate(prefab);
            instance.name = string.IsNullOrWhiteSpace(config.Id) ? type.Name : config.Id;

            var view = instance.GetComponent<UIView>() ?? instance.AddComponent<UIView>();
            var context = Activator.CreateInstance<T>();
            context.State = UIContextState.Loading;
            var resolvedId = string.IsNullOrWhiteSpace(config.Id) ? type.Name : config.Id;
            context.BindRuntime(resolvedId, config.Layer, view, instance);
            view.Context = context;

            BringToFront(context, context.Layer);
            context.OnInit();

            instance.SetActive(true);
            context.OnShow(args);

            _activeContexts[type] = context;
            _contextPrefabKeys[context] = config.PrefabKey;
            return context;
        }

        /// <summary>
        /// 关闭指定类型 Context。
        /// </summary>
        public Task CloseAsync<T>() where T : BaseContext
        {
            var type = typeof(T);
            if (!_activeContexts.TryGetValue(type, out var ctx))
            {
                return Task.CompletedTask;
            }

            return CloseAsync(ctx);
        }

        /// <summary>
        /// 关闭指定 Context 实例。
        /// </summary>
        public Task CloseAsync(BaseContext ctx)
        {
            if (ctx == null)
            {
                return Task.CompletedTask;
            }

            var type = ctx.GetType();
            var config = GetConfig(type);

            ctx.OnHide();
            ctx.OnClose();

            if (config.CacheOnClose)
            {
                if (ctx.ViewObject != null)
                {
                    ctx.ViewObject.SetActive(false);
                }

                return Task.CompletedTask;
            }

            _activeContexts.Remove(type);
            var hasPrefabKey = _contextPrefabKeys.TryGetValue(ctx, out var prefabKey);
            _contextPrefabKeys.Remove(ctx);

            ctx.OnDestroy();
            if (hasPrefabKey)
            {
                _resourceLoader.Release(prefabKey, ctx.ViewObject);
            }
            else
            {
                _resourceLoader.Release(config.PrefabKey, ctx.ViewObject);
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// 获取已打开的 Context；不存在返回 null。
        /// </summary>
        public T Get<T>() where T : BaseContext
        {
            if (_activeContexts.TryGetValue(typeof(T), out var ctx))
            {
                return (T)ctx;
            }

            return null;
        }

        /// <summary>
        /// 是否已处于显示状态。
        /// </summary>
        public bool IsOpen<T>() where T : BaseContext
        {
            var ctx = Get<T>();
            return ctx != null && ctx.State == UIContextState.Shown;
        }

        private UIConfig GetConfig(Type type)
        {
            if (_configRegistry.TryGetValue(type, out var config))
            {
                return config;
            }

            throw new InvalidOperationException($"UI 类型未注册: {type.Name}");
        }

        private void BringToFront(BaseContext context, UILayer layer)
        {
            if (context?.View?.RectTransform == null)
            {
                return;
            }

            _layerManager.AddToLayer(layer, context.View.RectTransform);
        }

        private void EnsureInitialized()
        {
            if (_resourceLoader == null || _layerManager == null || _uiRoot == null)
            {
                throw new InvalidOperationException("UIManager 未初始化，请先调用 Init(IResourceLoader)。");
            }
        }
    }
}
