using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

namespace DapperDino.BuildingBlocks
{
    public class RotateObject : MonoBehaviour
    {
        [SerializeField] private float speed = 0f;
        [SerializeField] private bool xAxis = true;
        [SerializeField] private bool yAxis = true;

        private void Awake()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        public void Rotate(CallbackContext ctx)
        {
            if (!ctx.performed) { return; }

            var aimInput = ctx.ReadValue<Vector2>();

            var rotation = new Vector3(
                xAxis ? -aimInput.y : 0f,
                yAxis ? aimInput.x : 0f,
                0f);

            rotation *= speed * Time.deltaTime;

            transform.Rotate(rotation);
        }
    }
}
