namespace UIFramework
{
    /// <summary>
    /// 全屏页面 Context（后续阶段将接入页面导航栈）。
    /// </summary>
    public abstract class BasePageContext : BaseContext
    {
        public override UILayer DefaultLayer => UILayer.Normal;

        /// <summary>
        /// 预留：是否全屏页面（导航栈阶段用于遮挡策略）。
        /// </summary>
        public virtual bool IsFullScreen => true;
    }
}
