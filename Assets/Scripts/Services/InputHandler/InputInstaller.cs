using UnityEngine.InputSystem;

namespace Service.InputHandler
{
    public class InputInstaller
    {
        private readonly Controls inputActions;
        public InputAction Click => inputActions.Gameplay.LeftClick;

        public InputInstaller()
        {
            inputActions = new();
            inputActions.Gameplay.Enable();
        }
    }
}