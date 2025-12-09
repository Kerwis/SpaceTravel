using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SpaceTravel.Game.UI
{
    public class ShipTileView : MonoBehaviour, IPointerClickHandler
    {
        [Tooltip("Background image for the tile.")]
        public Image Background;

        [Tooltip("Image used to show module icon on this tile.")]
        public Image ModuleIcon;

        [Tooltip("Optional debug label.")]
        public TMP_Text DebugLabel;

        [HideInInspector] public int X;
        [HideInInspector] public int Y;
        [HideInInspector] public TileType TileType;

        private ShipViewController _owner;

        public void Init(ShipViewController owner, int x, int y, TileType tileType)
        {
            _owner = owner;
            X = x;
            Y = y;
            TileType = tileType;

            if (tileType == TileType.Empty)
            {
                Background.enabled = false;
                return;
            }

            if (DebugLabel != null)
                DebugLabel.text = X + "," + Y + "\n" + TileType;
        }

        public void SetModule(ModuleDefinition def)
        {
            if (ModuleIcon == null)
                return;

            if (def == null || def.Icon == null)
            {
                ModuleIcon.enabled = false;
                ModuleIcon.sprite = null;
            }
            else
            {
                ModuleIcon.enabled = true;
                ModuleIcon.sprite = def.Icon;
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _owner?.OnTileClicked(this);
        }
    }
}