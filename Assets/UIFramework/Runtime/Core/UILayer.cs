namespace UIFramework
{
    /// <summary>
    /// UI 分层定义：数值用于 Canvas sortingOrder 与层级排序。
    /// </summary>
    public enum UILayer
    {
        /// <summary>场景贴附层（例如 3D 世界锚点 UI）。</summary>
        Scene = 0,

        /// <summary>底层背景与常驻底图。</summary>
        Bottom = 100,

        /// <summary>普通全屏页面层（后续导航栈主要工作层）。</summary>
        Normal = 200,

        /// <summary>常驻 HUD 层（例如状态条、固定按钮）。</summary>
        Fixed = 300,

        /// <summary>弹窗层（可叠加）。</summary>
        Popup = 400,

        /// <summary>引导层（遮罩、高亮、引导手势）。</summary>
        Guide = 500,

        /// <summary>全局顶层（飘字、全局提示）。</summary>
        Top = 600,

        /// <summary>系统最高优先级层（Loading、断线重连等）。</summary>
        System = 700,
    }
}
