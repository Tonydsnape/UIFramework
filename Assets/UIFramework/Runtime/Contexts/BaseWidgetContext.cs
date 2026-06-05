namespace UIFramework
{
    /// <summary>
    /// 常驻挂件 Context（不参与页面栈）。
    /// </summary>
    public abstract class BaseWidgetContext : BaseContext
    {
        public override UILayer DefaultLayer => UILayer.Fixed;
    }
}
