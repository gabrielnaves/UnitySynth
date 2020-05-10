using UnityEngine;

public class CameraControls : MonoBehaviour, IDraggable {

    static public CameraControls instance { get; private set; }

    private Vector2 initialScreenPos;
    private Vector3 initialWorldPos;
    private Camera cam;

    private void Awake() {
        instance = (CameraControls)Singleton.Setup(this, instance);
        cam = Camera.main;
    }

    void IDraggable.OnDragStart(Vector2 mouseScreenPosition, Vector3 mouseWorldPosition) {
        initialScreenPos = mouseScreenPosition;
        initialWorldPos = transform.position;
    }

    void IDraggable.OnDragUpdate(Vector2 mouseScreenPosition, Vector3 mouseWorldPosition) {
        UpdateCameraPosition(mouseWorldPosition);
    }

    void IDraggable.OnDragEnd(Vector2 mouseScreenPosition, Vector3 mouseWorldPosition) {
        UpdateCameraPosition(mouseWorldPosition);
    }

    void UpdateCameraPosition(Vector3 mouseWorldPosition) {
        Vector3 worldOffset = cam.ScreenToWorldPoint(initialScreenPos) - mouseWorldPosition;
        worldOffset.z = 0;
        transform.position = initialWorldPos + worldOffset;
    }
}
