using System.Collections.Generic;
using UnityEngine;

namespace SpaceTravel.Game.UI
{
    public class WindowManager : MonoBehaviour
    {
        [Tooltip("List of window instances managed by this controller.")]
        public List<GameWindow> Windows = new();

        private readonly Dictionary<WindowId, GameWindow> _windowsById = new();

        private void Awake()
        {
            _windowsById.Clear();

            if (Windows != null)
            {
                foreach (var window in Windows)
                {
                    if (window == null)
                        continue;

                    if (window.Id == WindowId.None)
                    {
#if UNITY_EDITOR
                        Debug.LogWarning($"[WindowManager] Window '{window.name}' has WindowId.None");
#endif
                        continue;
                    }

                    if (_windowsById.ContainsKey(window.Id))
                    {
#if UNITY_EDITOR
                        Debug.LogWarning($"[WindowManager] Duplicate WindowId '{window.Id}' on '{window.name}'");
#endif
                        continue;
                    }

                    _windowsById[window.Id] = window;

                    // Ensure windows start hidden unless you want some visible by default
                    if (window.Root != null)
                        window.Root.SetActive(false);
                }
            }
        }

        private void Update()
        {
            float dt = Time.deltaTime;

            foreach (var kvp in _windowsById)
            {
                var window = kvp.Value;
                if (window != null && window.IsVisible)
                    window.Tick(dt);
            }
        }

        public void Show(WindowId id)
        {
            if (_windowsById.TryGetValue(id, out var window) && window != null)
                window.Show();
        }

        public void Hide(WindowId id)
        {
            if (_windowsById.TryGetValue(id, out var window) && window != null)
                window.Hide();
        }

        public void Toggle(WindowId id)
        {
            if (_windowsById.TryGetValue(id, out var window) && window != null)
            {
                if (window.IsVisible)
                    window.Hide();
                else
                    window.Show();
            }
        }

        public void HideAll()
        {
            foreach (var window in _windowsById.Values)
            {
                if (window != null && window.IsVisible)
                    window.Hide();
            }
        }

        public T GetWindow<T>(WindowId id) where T : GameWindow
        {
            if (_windowsById.TryGetValue(id, out var window))
                return window as T;
            return null;
        }
    }
}
