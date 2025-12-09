using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceTravel.Game.UI
{
    public class BuildWindowModuleItem : MonoBehaviour
    {
        public Image Icon;
        public TMP_Text NameLabel;
        public TMP_Text CostLabel;
        public Button Button;

        private BuildWindow _owner;
        private ModuleDefinition _def;

        public void Setup(BuildWindow owner, ModuleDefinition def)
        {
            _owner = owner;
            _def = def;

            if (Icon != null)
                Icon.sprite = def.Icon;

            if (NameLabel != null)
                NameLabel.text = def.DisplayName;

            if (CostLabel != null)
                CostLabel.text = BuildCostLabel(def);

            if (Button != null)
            {
                Button.onClick.RemoveAllListeners();
                Button.onClick.AddListener(OnClick);
            }
        }

        string BuildCostLabel(ModuleDefinition def)
        {
            var cost = def.BuildCost;
            if (cost == null || cost.Length == 0)
                return string.Empty;

            if (cost.Length == 1)
                return cost[0].Amount.ToString("0.##") + " " + cost[0].Resource.DisplayName;

            return cost.Length + " resources";
        }

        void OnClick()
        {
            _owner?.OnModuleClicked(_def);
        }
    }
}