using TMPro;
using Utils.CustomComponents;

namespace UI
{
    public class UIMoney: MyMonoBehaviour
    {
        public TextMeshProUGUI moneyText;

        void Update()
        {
            RequireGameManager();

            if (moneyText)
            {
                int money = GameManager.GameState?.GetMoney() ?? 0;
                moneyText.SetText(money.ToString());
            }
        }
    }
}
