using System.Collections.Generic;
using UI.DataStructures;
using UnityEngine;

namespace UI
{
    public class UIUpgradePath : MonoBehaviour
    {
        public Transform upgradesRoot;
        public UIUpgrade upgradePrefab;

        private readonly List<UIUpgrade> _upgrades = new();

        public void SetUpgrades(UIUpgradeDescription[] upgrades, int nextUpgrade, bool upgradePathLocked)
        {
            for (int i = _upgrades.Count; i < upgrades.Length; i++)
            {
                UIUpgrade upgrade = Instantiate(upgradePrefab, upgradesRoot);
                _upgrades.Add(upgrade);
            }

            for (int i = 0; i < _upgrades.Count; i++)
            {
                _upgrades[i].SetParams(upgrades[i], i == nextUpgrade, i >= nextUpgrade && upgradePathLocked);
                _upgrades[i].gameObject.SetActive(i < upgrades.Length);
            }
            
            upgradePrefab.gameObject.SetActive(false);
        }
    }
}
