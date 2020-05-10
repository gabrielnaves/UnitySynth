using UnityEngine;

public class DraggableObject : MonoBehaviour
{
    public LayerMask obstacleMask;
    public MinMax raycastDepth = new MinMax(-5, 5);

    private Camera cam;
    private Vector2 initialScreenPos;
    private Vector3 initialWorldPos;
    private bool draggingObject;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        if (!draggingObject)
            CheckForDragStart();
        else
            UpdateDrag();
    }

    private void CheckForDragStart()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 worldMousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            var hit = Physics2D.Raycast(worldMousePos, Vector2.zero, 0f, obstacleMask, raycastDepth.min, raycastDepth.max);
            if (hit && hit.collider.gameObject == gameObject)
            {
                draggingObject = true;
                initialScreenPos = Input.mousePosition;
                initialWorldPos = transform.position;
            }
        }
    }

    private void UpdateDrag()
    {
        Vector3 worldOffset = cam.ScreenToWorldPoint(Input.mousePosition) - cam.ScreenToWorldPoint(initialScreenPos);
        worldOffset.z = 0;
        transform.position = initialWorldPos + worldOffset;
        if (Input.GetMouseButtonUp(0))
            draggingObject = false;
    }
}
