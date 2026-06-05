using UnityEngine;

namespace UIFramework
{
    /// <summary>
    /// 挂在 UI Prefab 根节点上的桥接组件：连接 View 与 Context。
    /// </summary>
    [DisallowMultipleComponent]
    public class UIView : MonoBehaviour
    {
        private RectTransform _rectTransform;

        /// <summary>所属 Context，由 UIManager 在 Open 流程中注入。</summary>
        public BaseContext Context { get; internal set; }

        /// <summary>RectTransform 便捷访问。</summary>
        public RectTransform RectTransform
        {
            get
            {
                if (_rectTransform == null)
                {
                    _rectTransform = GetComponent<RectTransform>();
                }

                return _rectTransform;
            }
        }

    }
}
