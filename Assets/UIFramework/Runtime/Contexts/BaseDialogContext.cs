namespace UIFramework
{
    /// <summary>
    /// 弹窗 Context（默认位于 Popup 层）。
    /// </summary>
    public abstract class BaseDialogContext : BaseContext
    {
        public override UILayer DefaultLayer => UILayer.Popup;

        /// <summary>
        /// 预留：后续阶段接入统一遮罩控制。
        /// </summary>
        public virtual bool UseMask => true;

        /// <summary>
        /// 预留：后续阶段接入遮罩可点击关闭策略。
        /// </summary>
        public virtual bool CloseOnMaskClick => false;
    }
}
