using UnityEngine;

public class CameraControls : MonoBehaviour
{
    public LayerMask obstacleMask;
    public MinMax raycastDepth = new MinMax(-5, 5);

    private Camera cam;
    private Vector2 initialScreenPos;
    private Vector3 initialWorldPos;
    private bool movingCamera;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    private void Update()
    {
        if (!movingCamera)
            CheckForMovementStart();
        else
            UpdateCameraMovement();
    }

    private void CheckForMovementStart()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 worldMousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            var hit = Physics2D.Raycast(worldMousePos, Vector2.zero, 0f, obstacleMask, raycastDepth.min, raycastDepth.max);
            if (!hit)
            {
                movingCamera = true;
                initialScreenPos = Input.mousePosition;
                initialWorldPos = transform.position;
            }
        }
    }

    private void UpdateCameraMovement()
    {
        Vector3 worldOffset = cam.ScreenToWorldPoint(initialScreenPos) - cam.ScreenToWorldPoint(Input.mousePosition);
        worldOffset.z = 0;
        transform.position = initialWorldPos + worldOffset;
        if (Input.GetMouseButtonUp(0))
            movingCamera = false;
    }
}
