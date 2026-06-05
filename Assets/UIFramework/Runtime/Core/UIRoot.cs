using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UIFramework
{
    /// <summary>
    /// UI 根节点：负责初始化全局 Canvas、EventSystem 以及各 UILayer 子 Canvas。
    /// </summary>
    public sealed class UIRoot : MonoBehaviour
    {
        private static UIRoot _instance;
        private readonly Dictionary<UILayer, RectTransform> _layerRoots = new Dictionary<UILayer, RectTransform>();
        private bool _built;

        /// <summary>
        /// 全局唯一 UIRoot。
        /// </summary>
        public static UIRoot Instance
        {
            get
            {
                if (_instance != null)
                {
                    return _instance;
                }

                _instance = FindObjectOfType<UIRoot>();
                if (_instance != null)
                {
                    return _instance;
                }

                var rootGo = new GameObject("UIRoot");
                _instance = rootGo.AddComponent<UIRoot>();
                return _instance;
            }
        }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);
            BuildIfNeeded();
        }

        /// <summary>
        /// 获取指定层的根节点。
        /// </summary>
        public RectTransform GetLayerRoot(UILayer layer)
        {
            BuildIfNeeded();
            _layerRoots.TryGetValue(layer, out var root);
            return root;
        }

        private void BuildIfNeeded()
        {
            if (_built)
            {
                return;
            }

            EnsureRootCanvasComponents();
            EnsureEventSystem();
            BuildLayerCanvases();
            _built = true;
        }

        private void EnsureRootCanvasComponents()
        {
            var rootRect = gameObject.GetComponent<RectTransform>();
            if (rootRect == null)
            {
                rootRect = gameObject.AddComponent<RectTransform>();
            }

            rootRect.anchorMin = Vector2.zero;
            rootRect.anchorMax = Vector2.one;
            rootRect.offsetMin = Vector2.zero;
            rootRect.offsetMax = Vector2.zero;

            var canvas = gameObject.GetComponent<Canvas>() ?? gameObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 0;

            var scaler = gameObject.GetComponent<CanvasScaler>() ?? gameObject.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.matchWidthOrHeight = 0.5f;

            _ = gameObject.GetComponent<GraphicRaycaster>() ?? gameObject.AddComponent<GraphicRaycaster>();
        }

        private void EnsureEventSystem()
        {
            var eventSystem = FindObjectOfType<EventSystem>();
            if (eventSystem != null)
            {
                return;
            }

            var eventSystemGo = new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
            DontDestroyOnLoad(eventSystemGo);
        }

        private void BuildLayerCanvases()
        {
            foreach (UILayer layer in Enum.GetValues(typeof(UILayer)))
            {
                if (_layerRoots.ContainsKey(layer))
                {
                    continue;
                }

                var layerGo = new GameObject($"Layer_{layer}", typeof(RectTransform), typeof(Canvas), typeof(GraphicRaycaster));
                var rect = layerGo.GetComponent<RectTransform>();
                rect.SetParent(transform, false);
                rect.anchorMin = Vector2.zero;
                rect.anchorMax = Vector2.one;
                rect.offsetMin = Vector2.zero;
                rect.offsetMax = Vector2.zero;

                var canvas = layerGo.GetComponent<Canvas>();
                canvas.overrideSorting = true;
                canvas.sortingOrder = (int)layer;

                _layerRoots[layer] = rect;
            }
        }
    }
}
