using UnityEngine;

public interface IDraggable {
    void OnDragStart(Vector2 mouseScreenPosition, Vector3 mouseWorldPosition);
    void OnDragUpdate(Vector2 mouseScreenPosition, Vector3 mouseWorldPosition);
    void OnDragEnd(Vector2 mouseScreenPosition, Vector3 mouseWorldPosition);
}
