using UnityEngine;
using UnityEngine.UI;

namespace UIFramework
{
    /// <summary>
    /// 最小可运行示例 Page：使用代码构建简单面板。
    /// </summary>
    public sealed class SampleHelloPage : BasePageContext
    {
        private Text _messageText;
        private Button _closeButton;

        protected override void HandleInit()
        {
            if (ViewObject == null)
            {
                return;
            }

            var panel = CreateUIObject("Panel", ViewObject.transform);
            var panelRect = panel.GetComponent<RectTransform>();
            panelRect.anchorMin = new Vector2(0.5f, 0.5f);
            panelRect.anchorMax = new Vector2(0.5f, 0.5f);
            panelRect.sizeDelta = new Vector2(520f, 300f);
            panelRect.anchoredPosition = Vector2.zero;

            var panelImage = panel.AddComponent<Image>();
            panelImage.color = new Color(0f, 0f, 0f, 0.75f);

            var textGo = CreateUIObject("Message", panel.transform);
            var textRect = textGo.GetComponent<RectTransform>();
            textRect.anchorMin = new Vector2(0.1f, 0.55f);
            textRect.anchorMax = new Vector2(0.9f, 0.9f);
            textRect.offsetMin = Vector2.zero;
            textRect.offsetMax = Vector2.zero;

            _messageText = textGo.AddComponent<Text>();
            _messageText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            _messageText.alignment = TextAnchor.MiddleCenter;
            _messageText.color = Color.white;
            _messageText.horizontalOverflow = HorizontalWrapMode.Wrap;
            _messageText.verticalOverflow = VerticalWrapMode.Overflow;

            var buttonGo = CreateUIObject("CloseButton", panel.transform);
            var buttonRect = buttonGo.GetComponent<RectTransform>();
            buttonRect.anchorMin = new Vector2(0.3f, 0.12f);
            buttonRect.anchorMax = new Vector2(0.7f, 0.32f);
            buttonRect.offsetMin = Vector2.zero;
            buttonRect.offsetMax = Vector2.zero;

            var buttonImage = buttonGo.AddComponent<Image>();
            buttonImage.color = new Color(0.2f, 0.6f, 1f, 1f);

            _closeButton = buttonGo.AddComponent<Button>();
            _closeButton.targetGraphic = buttonImage;
            _closeButton.onClick.AddListener(OnCloseButtonClicked);

            var btnTextGo = CreateUIObject("Text", buttonGo.transform);
            var btnTextRect = btnTextGo.GetComponent<RectTransform>();
            btnTextRect.anchorMin = Vector2.zero;
            btnTextRect.anchorMax = Vector2.one;
            btnTextRect.offsetMin = Vector2.zero;
            btnTextRect.offsetMax = Vector2.zero;

            var btnText = btnTextGo.AddComponent<Text>();
            btnText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            btnText.alignment = TextAnchor.MiddleCenter;
            btnText.color = Color.white;
            btnText.text = "Close";
        }

        protected override void HandleShow(object args)
        {
            var message = args?.ToString() ?? "Hello from SampleHelloPage";
            if (_messageText != null)
            {
                _messageText.text = message;
            }

            Debug.Log($"[SampleHelloPage] OnShow args: {message}");
        }

        protected override void HandleDestroy()
        {
            if (_closeButton != null)
            {
                _closeButton.onClick.RemoveListener(OnCloseButtonClicked);
            }
        }

        private void OnCloseButtonClicked()
        {
            _ = UIManager.Instance.CloseAsync(this);
        }

        private static GameObject CreateUIObject(string name, Transform parent)
        {
            var go = new GameObject(name, typeof(RectTransform));
            var rect = go.GetComponent<RectTransform>();
            rect.SetParent(parent, false);
            return go;
        }
    }
}
