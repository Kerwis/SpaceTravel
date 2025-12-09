using UnityEngine;
using UnityEngine.InputSystem; // NEW

namespace SpaceTravel.Game.UI
{
    public class WindowInputController : MonoBehaviour
    {
        public WindowManager WindowManager;

        private void Update()
        {
            if (WindowManager == null)
                return;

            var keyboard = Keyboard.current;
            if (keyboard == null)
                return;

            if (keyboard.iKey.wasPressedThisFrame)
            {
                WindowManager.Toggle(WindowId.Storage);
            }
        }
    }
}