using UnityEngine;
using UnityEngine.InputSystem;

namespace SpaceTravel.Game
{
    public class DebugResetController : MonoBehaviour
    {
        public GameLoopController GameLoop;

        private void Update()
        {
            if (GameLoop == null)
                return;

            var keyboard = Keyboard.current;
            if (keyboard == null)
                return;

            if (keyboard.rKey.wasPressedThisFrame)
            {
                GameLoop.ResetGame();
            }
        }
    }
}