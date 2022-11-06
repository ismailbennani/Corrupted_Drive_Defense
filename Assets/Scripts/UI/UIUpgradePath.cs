using System.Collections.Generic;
using TMPro;
using UI.DataStructures;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIUpgradePath : MonoBehaviour
    {
        public Transform upgradesRoot;
        public UIUpgrade upgradePrefab;

        [Space(10)]
        public TextMeshProUGUI titleText;
        public Image background;
        
        private readonly List<UIUpgrade> _upgrades = new();

        public void SetUpgrades(int path, UIUpgradeDescription[] upgrades, int nextUpgrade, bool upgradePathLocked)
        {
            if (titleText)
            {
                titleText.SetText($"Path {path}");
            }

            if (background)
            {
                background.color = upgradePathLocked ? Color.gray.WithAlpha(0.2f) : Color.white.WithAlpha(0.2f);
            }

            for (int i = _upgrades.Count; i < upgrades.Length; i++)
            {
                UIUpgrade upgrade = Instantiate(upgradePrefab, upgradesRoot);
                _upgrades.Add(upgrade);
            }

            for (int i = 0; i < _upgrades.Count; i++)
            {
                _upgrades[i].SetParams(upgrades[i], i < nextUpgrade, i == nextUpgrade, i >= nextUpgrade && upgradePathLocked);
                _upgrades[i].gameObject.SetActive(i < upgrades.Length);
            }
            
            upgradePrefab.gameObject.SetActive(false);
        }
    }
}
