using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceTravel.Game.UI
{
    /// <summary>
    /// Visual representation of a single resource:
    /// icon + name + amount.
    /// Can be fed directly with ResourceDefinition + amount
    /// or with ResourceAmount (recipe entries).
    /// </summary>
    public class ResourceItemView : MonoBehaviour
    {
        [Tooltip("Image used to display resource icon.")]
        public Image Icon;

        [Tooltip("Text used to display resource name / label.")]
        public TMP_Text NameLabel;

        [Tooltip("Text used to display resource amount.")]
        public TMP_Text AmountLabel;

        private ResourceDefinition _resource;

        /// <summary>
        /// Setup view from definition and amount.
        /// </summary>
        public void Setup(ResourceDefinition resource, float amount)
        {
            _resource = resource;

            if (_resource != null)
            {
                if (Icon != null)
                    Icon.sprite = _resource.Icon;

                if (NameLabel != null)
                    NameLabel.text = _resource.DisplayName;
            }

            SetAmount(amount);
        }

        /// <summary>
        /// Setup view from ResourceAmount (e.g. recipe input/output).
        /// </summary>
        public void Setup(ResourceAmount resourceAmount)
        {
            if (resourceAmount == null || resourceAmount.Resource == null)
            {
                _resource = null;
                if (NameLabel != null) NameLabel.text = string.Empty;
                if (AmountLabel != null) AmountLabel.text = string.Empty;
                if (Icon != null) Icon.sprite = null;
                return;
            }

            Setup(resourceAmount.Resource, resourceAmount.Amount);
        }

        /// <summary>
        /// Update only visual amount (keeping icon/name as is).
        /// </summary>
        public void SetAmount(float amount)
        {
            if (AmountLabel != null)
                AmountLabel.text = amount.ToString("0.##");
        }

        public ResourceDefinition Resource => _resource;
    }
}
