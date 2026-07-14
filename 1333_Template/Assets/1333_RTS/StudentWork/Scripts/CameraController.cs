using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public bool isPaused = true;

    [SerializeField] private CinemachineCamera _cam;
    [SerializeField] private GameObject _camTarget;
    private Vector3 input;
    [SerializeField] private float _panSpeed = 10f;
    [SerializeField] private bool _keyboardPanning;
    private float zoom;
    [SerializeField] private float _zoomSpeed = 50f;
    [SerializeField] private float _maxZoom = 20f;
    [SerializeField] private float _minZoom = 50f;

    private void Update()
    {
        if (!isPaused)
        {
            MoveCamera();
            ZoomCamera();
        }
        
    }
    private void MoveCamera()
    {
        _camTarget.transform.Translate(input * Time.deltaTime * _panSpeed, Space.World);
    }
    private void ZoomCamera()
    {
        _cam.Lens.FieldOfView = _cam.Lens.FieldOfView + (zoom * Time.deltaTime * _zoomSpeed);
        if (_cam.Lens.FieldOfView <= _maxZoom) { _cam.Lens.FieldOfView = _maxZoom; }
        if (_cam.Lens.FieldOfView >= _minZoom) { _cam.Lens.FieldOfView = _minZoom; }
    }
    public void WASD(InputAction.CallbackContext context)
    {
        if (isPaused) return;
        if (context.performed) { _keyboardPanning = true; }
        else { _keyboardPanning = false; }

        if (!isPaused)
        {
            Debug.Log($"{context.ReadValue<Vector2>()}");
            input.x = context.ReadValue<Vector2>().x * 2;
            input.z = context.ReadValue<Vector2>().y * 2;
        }
    }
    public void MousePan(InputAction.CallbackContext context)
    {
        if (_keyboardPanning) return;
        if (!isPaused)
        {
            Debug.Log($"{context.ReadValue<Vector2>()}");
            if (context.ReadValue<Vector2>().x > (Screen.width * 0.9f)) { input.x = 1f; }
            else if (context.ReadValue<Vector2>().x < (Screen.width * 0.1f)) { input.x = -1f; }
            else { input.x = 0f; }

            if (context.ReadValue<Vector2>().y > (Screen.height * 0.9f)) { input.z = 1f; }
            else if (context.ReadValue<Vector2>().y < (Screen.height * 0.1f)) { input.z = -1f; }
            else { input.z = 0f; }
        }
    }
    public void Scroll(InputAction.CallbackContext context)
    {
        if (!isPaused)
        {

            if (context.ReadValue<Vector2>().y >= 1) { zoom = -2; }
            else if (context.ReadValue<Vector2>().y <= -1) { zoom = 2; }
            else { zoom = 0; }
        }
    }
}
