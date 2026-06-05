using UnityEngine;

namespace UIFramework
{
    /// <summary>
    /// 所有 UI Context 的抽象基类，统一维护生命周期与状态流转。
    /// </summary>
    public abstract class BaseContext : IUIContext
    {
        private bool _inited;

        /// <summary>唯一 ID（来自注册配置）。</summary>
        public string Id { get; internal set; }

        /// <summary>所属层（来自注册配置或默认层）。</summary>
        public UILayer Layer { get; internal set; }

        /// <summary>当前生命周期状态。</summary>
        public UIContextState State { get; internal set; } = UIContextState.None;

        /// <summary>当前绑定的 UIView 组件。</summary>
        public UIView View { get; internal set; }

        /// <summary>当前实例化出来的 View 根对象。</summary>
        public GameObject ViewObject { get; internal set; }

        /// <summary>子类默认层定义。</summary>
        public abstract UILayer DefaultLayer { get; }

        /// <summary>
        /// 由 UIManager 在实例创建时注入运行时信息。
        /// </summary>
        internal void BindRuntime(string id, UILayer layer, UIView view, GameObject viewObject)
        {
            Id = id;
            Layer = layer;
            View = view;
            ViewObject = viewObject;
        }

        public void OnInit()
        {
            if (_inited)
            {
                return;
            }

            HandleInit();
            _inited = true;
            State = UIContextState.Hidden;
        }

        public void OnShow(object args)
        {
            HandleShow(args);
            State = UIContextState.Shown;
        }

        public void OnHide()
        {
            HandleHide();
            State = UIContextState.Hidden;
        }

        public void OnClose()
        {
            HandleClose();
            State = UIContextState.Closed;
        }

        public void OnDestroy()
        {
            HandleDestroy();
            State = UIContextState.Destroyed;
        }

        /// <summary>一次性初始化逻辑（可重写）。</summary>
        protected virtual void HandleInit() { }

        /// <summary>显示逻辑（可重写）。</summary>
        protected virtual void HandleShow(object args) { }

        /// <summary>隐藏逻辑（可重写）。</summary>
        protected virtual void HandleHide() { }

        /// <summary>关闭收尾逻辑（可重写）。</summary>
        protected virtual void HandleClose() { }

        /// <summary>销毁前清理逻辑（可重写）。</summary>
        protected virtual void HandleDestroy() { }
    }
}
