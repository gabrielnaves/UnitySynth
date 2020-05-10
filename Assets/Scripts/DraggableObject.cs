using UnityEngine;

public class DraggableObject : MonoBehaviour, IDraggable {

    private Vector3 initialMouseWorldPos;
    private Vector3 initialObjectWorldPos;

    void IDraggable.OnDragStart(Vector2 mouseScreenPosition, Vector3 mouseWorldPosition) {
        initialMouseWorldPos = mouseWorldPosition;
        initialObjectWorldPos = transform.position;
    }

    void IDraggable.OnDragUpdate(Vector2 mouseScreenPosition, Vector3 mouseWorldPosition) {
        DragObject(mouseWorldPosition);
    }

    void IDraggable.OnDragEnd(Vector2 mouseScreenPosition, Vector3 mouseWorldPosition) {
        DragObject(mouseWorldPosition);
    }

    void DragObject(Vector3 mouseWorldPosition) {
        Vector3 worldOffset = mouseWorldPosition - initialMouseWorldPos;
        worldOffset.z = 0;
        transform.position = initialObjectWorldPos + worldOffset;
    }
}
