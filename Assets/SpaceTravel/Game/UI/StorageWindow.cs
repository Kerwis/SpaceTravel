using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace SpaceTravel.Game.UI
{
    public class StorageWindow : GameWindow
    {
        [Tooltip("Parent transform for resource item views (e.g. a VerticalLayoutGroup).")]
        public Transform ItemsParent;

        [Tooltip("Prefab for a single resource item (must have ResourceItemView).")]
        public ResourceItemView ItemPrefab;

        private readonly List<Entry> _entries = new();
        private GameState _state;
        private IGameDefinitions _defs;
        private bool _initialized;

        private class Entry
        {
            public string ResourceId;
            public ResourceItemView View;
        }

        private void OnEnable()
        {
            GameLoopController.Initialized += OnInitialized;
        }

        private void OnDisable()
        {
            GameLoopController.Initialized -= OnInitialized;
        }

        protected override void OnShow()
        {
            base.OnShow();
            TryInit();
            UpdateItems();
        }

        public override void Tick(float dt)
        {
            if (!_isVisible || !_initialized)
                return;

            UpdateItems();
        }

        private void OnInitialized()
        {
            TryInit();
        }

        private void TryInit()
        {
            var loop = GameLoopController.Instance;
            if (loop == null || loop.State == null || loop.Definitions == null)
                return;

            _state = loop.State;
            _defs = loop.Definitions;

            if (!_initialized)
            {
                BuildItems();
                _initialized = true;
            }
        }


        private void BuildItems()
        {
            if (ItemsParent == null || ItemPrefab == null)
                return;

            for (int i = ItemsParent.childCount - 1; i >= 0; i--)
                Destroy(ItemsParent.GetChild(i).gameObject);

            _entries.Clear();

            var resources = _defs.GetAllResources();
            if (resources == null)
                return;

            for (int i = 0; i < resources.Count; i++)
            {
                var res = resources[i];
                if (res == null)
                    continue;

                var go = Instantiate(ItemPrefab, ItemsParent);
                float amt = _state.Storage.GetAmount(res.Id);
                go.Setup(res, amt);

                _entries.Add(new Entry
                {
                    ResourceId = res.Id,
                    View = go
                });
            }
        }

        private void UpdateItems()
        {
            if (!_initialized)
                return;

            for (int i = 0; i < _entries.Count; i++)
            {
                var e = _entries[i];
                float amt = _state.Storage.GetAmount(e.ResourceId);
                e.View.SetAmount(amt);
            }
        }
    }
}