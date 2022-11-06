using Managers;
using TMPro;
using UI.DataStructures;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class UIUpgrade : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public Sprite pathLockedImage;

        [Space(10)]
        public Image image;
        public Button button;
        public TextMeshProUGUI nameText;
        public TextMeshProUGUI costText;

        [Space(10)]
        public RectTransform tooltip;
        public TextMeshProUGUI tooltipNameText;
        public TextMeshProUGUI tooltipCostText;
        public TextMeshProUGUI tooltipDescriptionText;

        [Space(10)]
        public UIUpgradeDescription upgrade;
        public bool bought;
        public bool isNextUpgrade;
        public bool displayLock;

        private bool _tooltipVisible;

        void Awake()
        {
            if (tooltip)
            {
                tooltip.gameObject.SetActive(false);
            }

            if (button)
            {
                button.onClick.AddListener(OnClick);
            }
        }

        void Update()
        {
            if (_tooltipVisible)
            {
                tooltip.anchoredPosition = Input.mousePosition;
            }
        }
        
        public void SetParams(UIUpgradeDescription upgrade, bool bought, bool isNextUpgrade, bool displayLock)
        {
            this.upgrade = upgrade;
            this.bought = bought;
            this.isNextUpgrade = isNextUpgrade;
            this.displayLock = displayLock;
            
            if (nameText)
            {
                nameText.SetText(upgrade.name);
            }
            
            if (costText)
            {
                costText.SetText(upgrade.cost.ToString());
            }

            if (button)
            {
                button.interactable = isNextUpgrade && !displayLock;
            }

            if (image)
            {
                image.color = bought ? Color.green : Color.white;

                Sprite sprite = displayLock ? pathLockedImage : upgrade.sprite;
                if (sprite)
                {
                    image.sprite = sprite;
                }
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (tooltip)
            {
                tooltip.gameObject.SetActive(true);
                
                if (tooltipNameText)
                {
                    tooltipNameText.SetText(upgrade.name);
                }

                if (tooltipCostText)
                {
                    tooltipCostText.SetText(upgrade.cost.ToString());
                }

                if (tooltipDescriptionText)
                {
                    tooltipDescriptionText.SetText(upgrade.description);
                }
                
                _tooltipVisible = true;
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (tooltip)
            {
                tooltip.gameObject.SetActive(false);
            }
            
            _tooltipVisible = false;
        }

        public void OnClick()
        {
            if (upgrade.isTowerUpgrade)
            {
                GameManager.Instance.Tower.BuyUpgrade(upgrade.towerId, upgrade.upgradePath);
            }
        }
    }
}
