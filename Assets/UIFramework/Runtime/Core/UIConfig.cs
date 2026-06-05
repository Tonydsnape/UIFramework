using System;

namespace UIFramework
{
    /// <summary>
    /// UI 注册配置（Config 驱动）。
    /// </summary>
    [Serializable]
    public class UIConfig
    {
        /// <summary>唯一 ID，例如：HelloPage。</summary>
        public string Id;

        /// <summary>资源地址键（交由 IResourceLoader 解析）。</summary>
        public string PrefabKey;

        /// <summary>目标分层。</summary>
        public UILayer Layer;

        /// <summary>
        /// 关闭后是否缓存。
        /// P1 仅保留字段；缓存策略在后续阶段增强。
        /// </summary>
        public bool CacheOnClose;

        /// <summary>
        /// 是否全屏遮挡下层。
        /// P1 仅保留字段；导航栈阶段接入。
        /// </summary>
        public bool FullScreen;
    }
}
