using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace SpaceTravel.Game.UI
{
    public class BuildWindow : GameWindow
    {
        [Tooltip("Header label with tile info.")]
        public TMP_Text HeaderLabel;

        [Tooltip("Parent for module buttons.")]
        public Transform ModuleListParent;

        [Tooltip("Prefab for a single module button.")]
        public BuildWindowModuleItem ModuleItemPrefab;

        private GameState _state;
        private IGameDefinitions _defs;
        private bool _initialized;
        private int _tileX;
        private int _tileY;
        private TileType _tileType;

        private readonly List<BuildWindowModuleItem> _items = new();

        public void OpenForTile(int x, int y, TileType tileType)
        {
            _tileX = x;
            _tileY = y;
            _tileType = tileType;

            if (HeaderLabel != null)
                HeaderLabel.text = "Tile (" + x + "," + y + ") - " + tileType;

            Show();
        }

        public void OnModuleClicked(ModuleDefinition def)
        {
            if (def == null)
                return;

            var loop = GameLoopController.Instance;
            if (loop == null || loop.State == null || loop.Definitions == null)
                return;

            _state = loop.State;
            _defs = loop.Definitions;
            _initialized = true;

            bool ok = ShipBuildService.TryBuildModuleAt(
                _state,
                _defs,
                def.Id,
                _tileX,
                _tileY,
                _tileType);

#if UNITY_EDITOR
            Debug.Log("[BuildWindow] Build " + def.Id + " at (" + _tileX + "," + _tileY + ") result=" + ok);
#endif

            if (!ok)
                return;

            var shipView = FindObjectOfType<ShipViewController>();
            if (shipView != null)
                shipView.RefreshModules();

            Hide();
        }

        public void OnCloseButtonClicked()
        {
            Hide();
        }

        private void OnEnable()
        {
            GameLoopController.Initialized += OnGameInitialized;
        }

        private void OnDisable()
        {
            GameLoopController.Initialized -= OnGameInitialized;
        }

        private void OnGameInitialized()
        {
            TryInit();
        }

        protected override void OnShow()
        {
            base.OnShow();
            TryInit();
            RebuildModuleList();
        }

        private void TryInit()
        {
            var loop = GameLoopController.Instance;
            if (loop == null || loop.State == null || loop.Definitions == null)
                return;

            _state = loop.State;
            _defs = loop.Definitions;
            _initialized = true;
        }

        private void RebuildModuleList()
        {
            if (!_initialized || ModuleListParent == null || ModuleItemPrefab == null)
                return;

            for (int i = 0; i < _items.Count; i++)
            {
                if (_items[i] != null)
                    Destroy(_items[i].gameObject);
            }
            _items.Clear();

            var modules = _defs.GetAllModules();
            if (modules == null)
                return;

            for (int i = 0; i < modules.Count; i++)
            {
                var def = modules[i];
                if (def == null)
                    continue;

                if (!IsModuleAllowedOnTile(def, _tileType))
                    continue;

                var item = Object.Instantiate(ModuleItemPrefab, ModuleListParent);
                item.Setup(this, def);
                _items.Add(item);
            }
        }

        private bool IsModuleAllowedOnTile(ModuleDefinition def, TileType tileType)
        {
            if (def.AllowedTileTypes == null || def.AllowedTileTypes.Length == 0)
                return true;

            for (int i = 0; i < def.AllowedTileTypes.Length; i++)
            {
                if (def.AllowedTileTypes[i] == tileType)
                    return true;
            }

            return false;
        }
    }
}
