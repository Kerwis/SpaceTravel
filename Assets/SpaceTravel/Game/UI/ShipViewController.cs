using UnityEngine;
using UnityEngine.UI;

namespace SpaceTravel.Game.UI
{
    public class ShipViewController : MonoBehaviour
    {
        [Header("Visual roots")]
        [Tooltip("Background image under the ship (planet/outpost/scrap field).")]
        public Image LocationBackground;

        [Tooltip("Image used to display the ship sprite.")]
        public Image ShipImage;

        [Tooltip("RectTransform covering the 6x6 grid area on the ship sprite.")]
        public RectTransform GridRoot;

        [Tooltip("Prefab for a single tile view (must have ShipTileView).")]
        public ShipTileView TilePrefab;

        [Tooltip("Optional GridLayoutGroup for automatic tile layout.")]
        public GridLayoutGroup GridLayout;

        [Header("Windows")]
        [Tooltip("Main window manager.")]
        public WindowManager WindowManager;

        private GameState _state;
        private IGameDefinitions _defs;
        private ShipTileView[,] _tiles;
        private bool _initialized;

        public void OnTileClicked(ShipTileView tile)
        {
            if (!_initialized || tile == null || _state == null || _state.Ship == null)
                return;

            bool hasModule = HasModuleAt(tile.X, tile.Y);
            if (hasModule)
            {
#if UNITY_EDITOR
                Debug.Log("[ShipView] Clicked tile with module at (" + tile.X + "," + tile.Y + ")");
#endif
                return;
            }

            if (WindowManager == null)
                return;

            var buildWindow = WindowManager.GetWindow<BuildWindow>(WindowId.Build);
            if (buildWindow == null)
            {
#if UNITY_EDITOR
                Debug.LogWarning("[ShipView] BuildWindow not found in WindowManager.");
#endif
                return;
            }

            buildWindow.OpenForTile(tile.X, tile.Y, tile.TileType);
        }

        public void RefreshModules()
        {
            if (!_initialized || _tiles == null || _state == null || _defs == null)
                return;

            int w = _tiles.GetLength(0);
            int h = _tiles.GetLength(1);

            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    var tile = _tiles[x, y];
                    if (tile != null)
                        tile.SetModule(null);
                }
            }

            var modules = _state.Ship.Modules;
            if (modules == null)
                return;

            for (int i = 0; i < modules.Count; i++)
            {
                var m = modules[i];
                var def = _defs.GetModuleDefinition(m.Id);
                if (def == null)
                    continue;

                int tx = m.TilePos.x;
                int ty = m.TilePos.y;

                if (tx < 0 || ty < 0 || tx >= w || ty >= h)
                    continue;

                var tile = _tiles[tx, ty];
                if (tile != null)
                    tile.SetModule(def);
            }
        }

        private void OnEnable()
        {
            GameLoopController.Initialized += OnGameInitialized;
        }

        private void OnDisable()
        {
            GameLoopController.Initialized -= OnGameInitialized;
        }

        private void Start()
        {
            TryInitialize();
        }

        private void OnGameInitialized()
        {
            TryInitialize();
        }

        private void TryInitialize()
        {
            var loop = GameLoopController.Instance;
            if (loop == null || loop.State == null || loop.Definitions == null)
                return;

            _state = loop.State;
            _defs = loop.Definitions;

            if (!_initialized)
            {
                if (_state.Ship == null)
                    return;

                var shipDef = _defs.GetShipDefinition(_state.Ship.ShipDefinitionId);
                if (shipDef == null || shipDef.Layout == null)
                    return;

                BuildGrid(shipDef);
                _initialized = true;
            }

            RefreshModules();
        }

        private void BuildGrid(ShipDefinition shipDef)
        {
            if (GridRoot == null || TilePrefab == null)
                return;

            ClearGrid();

            int w = shipDef.Layout.Width;
            int h = shipDef.Layout.Height;

            _tiles = new ShipTileView[w, h];

            if (GridLayout != null)
            {
                GridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
                GridLayout.constraintCount = w;
            }

            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    var tileType = shipDef.Layout.GetTile(x, y);

                    var instance = Instantiate(TilePrefab, GridRoot);
                    instance.Init(this, x, y, tileType);

                    _tiles[x, y] = instance;
                }
            }
        }

        private void ClearGrid()
        {
            if (GridRoot == null)
                return;

            for (int i = GridRoot.childCount - 1; i >= 0; i--)
                Destroy(GridRoot.GetChild(i).gameObject);

            _tiles = null;
        }

        private bool HasModuleAt(int x, int y)
        {
            var modules = _state.Ship.Modules;
            if (modules == null)
                return false;

            for (int i = 0; i < modules.Count; i++)
            {
                var m = modules[i];
                if (m.TilePos.x == x && m.TilePos.y == y)
                    return true;
            }

            return false;
        }
    }
}
