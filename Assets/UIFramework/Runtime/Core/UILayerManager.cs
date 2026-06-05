using System.Collections.Generic;
using UnityEngine;

namespace UIFramework
{
    /// <summary>
    /// 管理 UILayer 根节点与同层内排序分配。
    /// </summary>
    public sealed class UILayerManager
    {
        private const int SortingStep = 10;
        private readonly Dictionary<UILayer, RectTransform> _layerRoots = new Dictionary<UILayer, RectTransform>();
        private readonly Dictionary<UILayer, int> _nextSortingOrder = new Dictionary<UILayer, int>();

        public UILayerManager(UIRoot uiRoot)
        {
            foreach (UILayer layer in System.Enum.GetValues(typeof(UILayer)))
            {
                _layerRoots[layer] = uiRoot.GetLayerRoot(layer);
                _nextSortingOrder[layer] = (int)layer;
            }
        }

        /// <summary>
        /// 获取层根节点。
        /// </summary>
        public RectTransform GetLayer(UILayer layer)
        {
            _layerRoots.TryGetValue(layer, out var layerRoot);
            return layerRoot;
        }

        /// <summary>
        /// 将视图加入目标层并置顶。
        /// </summary>
        public void AddToLayer(UILayer layer, RectTransform view)
        {
            var layerRoot = GetLayer(layer);
            if (layerRoot == null || view == null)
            {
                return;
            }

            view.SetParent(layerRoot, false);
            view.SetAsLastSibling();

            var canvas = view.GetComponent<Canvas>();
            if (canvas != null)
            {
                canvas.overrideSorting = true;
                canvas.sortingOrder = AllocSortingOrder(layer);
            }
        }

        /// <summary>
        /// 为同层界面分配 sortingOrder（步进 10）。
        /// </summary>
        public int AllocSortingOrder(UILayer layer)
        {
            if (!_nextSortingOrder.TryGetValue(layer, out var order))
            {
                order = (int)layer;
            }

            order += SortingStep;
            _nextSortingOrder[layer] = order;
            return order;
        }
    }
}
