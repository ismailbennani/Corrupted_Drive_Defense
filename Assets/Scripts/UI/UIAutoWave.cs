using UnityEngine.Assertions;
using UnityEngine.UI;
using Utils.CustomComponents;

namespace UI
{
    public class UIAutoWave : MyMonoBehaviour
    {
        public Toggle toggle;

        void Start()
        {
            Assert.IsNotNull(toggle);

            RequireGameManager();
        }

        void Update()
        {
            if (GameManager.EnemyWave == null)
            {
                return;
            }

            bool autoWave = GameManager.EnemyWave.GetAutoWave();
            toggle.SetIsOnWithoutNotify(autoWave);
        }
    }
}
