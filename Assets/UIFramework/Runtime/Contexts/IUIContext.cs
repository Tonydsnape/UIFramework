namespace UIFramework
{
    /// <summary>
    /// 统一 UI 生命周期接口。
    /// </summary>
    public interface IUIContext
    {
        /// <summary>实例化后仅一次：绑定组件、注册事件。</summary>
        void OnInit();

        /// <summary>每次打开：传参、刷新、入场。</summary>
        void OnShow(object args);

        /// <summary>每次关闭：出场、暂停。</summary>
        void OnHide();

        /// <summary>关闭收尾：解绑事件。</summary>
        void OnClose();

        /// <summary>销毁/入池前最终清理。</summary>
        void OnDestroy();
    }
}
