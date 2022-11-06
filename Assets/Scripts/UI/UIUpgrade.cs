using Managers;
using TMPro;
using UI.DataStructures;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class UIUpgrade : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        public Sprite pathLockedImage;

        [Space(10)]
        public Image image;
        public Button button;
        public TextMeshProUGUI nameText;
        public TextMeshProUGUI costText;

        [Space(10)]
        public Transform tooltip;
        public TextMeshProUGUI tooltipNameText;
        public TextMeshProUGUI tooltipCostText;
        public TextMeshProUGUI tooltipDescriptionText;

        [Space(10)]
        public UIUpgradeDescription upgrade;

        public void SetParams(UIUpgradeDescription upgrade, bool isNextUpgrade, bool displayLock)
        {
            this.upgrade = upgrade;

            if (nameText)
            {
                nameText.SetText(upgrade.name);
            }

            if (tooltipNameText)
            {
                tooltipNameText.SetText(upgrade.name);
            }

            if (costText)
            {
                costText.SetText(upgrade.cost.ToString());
            }

            if (tooltipCostText)
            {
                tooltipCostText.SetText(upgrade.cost.ToString());
            }

            if (tooltipDescriptionText)
            {
                tooltipDescriptionText.SetText(upgrade.description);
            }

            button.interactable = isNextUpgrade && !displayLock;
            image.sprite = displayLock ? pathLockedImage : upgrade.sprite;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            tooltip.gameObject.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            tooltip.gameObject.SetActive(false);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (upgrade.isTowerUpgrade)
            {
                GameManager.Instance.Tower.BuyUpgrade(upgrade.towerId, upgrade.upgradePath);
            }
        }
    }
}
