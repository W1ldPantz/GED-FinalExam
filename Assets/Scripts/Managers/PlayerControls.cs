using Characters;
using DrawingSystem;
using UnityEngine;

namespace Managers
{
    public static class PlayerControls
    {
        private static readonly Controls Controls = new Controls();
        private static Player _player;
        private static CanvasController _canvas;

        public static void InitPlayer(Player player)
        {
            if (!_player)
            {
                Controls.Player.Move.performed += context =>
                    _player.PlayerMovementComponent.Move(context.ReadValue<Vector2>());
                Controls.Player.Look.performed += context =>
                    _player.PlayerMovementComponent.Look(context.ReadValue<Vector2>());
                Controls.Player.Jump.performed += _ => _player.PlayerMovementComponent.Jump();

                Controls.Player.Interact.performed += context =>
                    _player.PlayerInteractionComponent.SetInteractionState(context.ReadValueAsButton());
            }

            _player = player;

        }

        public static void DrawingScreen(CanvasController canvas)
        {
            if (!_canvas)
            {
                Controls.Drawing.Primary.started += ctx => _canvas!.BeginPrimary(ctx.ReadValue<Vector2>());
                Controls.Drawing.Primary.performed += ctx => _canvas!.UpdatePrimary(ctx.ReadValue<Vector2>());
                Controls.Drawing.Primary.canceled += ctx => _canvas!.EndPrimary(ctx.ReadValue<Vector2>());
                
                Controls.Drawing.Secondary.started += ctx => _canvas!.BeginSecondary(ctx.ReadValue<Vector2>());
                Controls.Drawing.Secondary.performed += ctx => _canvas!.UpdateSecondary(ctx.ReadValue<Vector2>());
                Controls.Drawing.Secondary.canceled += ctx => _canvas!.EndSecondary(ctx.ReadValue<Vector2>());
                
                Controls.Drawing.Undo.performed += _ => _canvas!.Undo();
                Controls.Drawing.Redo.performed += _ => _canvas!.Redo();
                Controls.Drawing.Save.performed += _ => _canvas!.Save();
            }

            _canvas = canvas;
        }
        




        public static void EnableGameControls()
        {
            Controls.Player.Enable();
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        public static void EnterDrawing()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            Controls.Player.Disable();
            Controls.Drawing.Enable();
        }
    }
}