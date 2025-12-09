using UnityEngine;

namespace SpaceTravel.Game.UI
{
    public abstract class GameWindow : MonoBehaviour
    {
        [Tooltip("Unique window identifier.")]
        public WindowId Id;

        [Tooltip("Root GameObject used to show/hide this window.")]
        public GameObject Root;

        protected bool _isVisible;

        protected virtual void Awake()
        {
            if (Root == null)
                Root = gameObject;
        }

        public virtual void Show()
        {
            _isVisible = true;
            if (Root != null)
                Root.SetActive(true);
            OnShow();
        }

        public virtual void Hide()
        {
            _isVisible = false;
            if (Root != null)
                Root.SetActive(false);
            OnHide();
        }

        public bool IsVisible => _isVisible;

        protected virtual void OnShow() { }

        protected virtual void OnHide() { }

        /// <summary>
        /// Called every frame by the window manager while the window is visible.
        /// </summary>
        public virtual void Tick(float deltaTime) { }
    }
}