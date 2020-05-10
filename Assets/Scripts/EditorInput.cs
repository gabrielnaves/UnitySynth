using UnityEngine;

public class EditorInput : MonoBehaviour {

    public LayerMask obstacleMask;
    public MinMax raycastDepth = new MinMax(-5, 5);

    MouseInputHandler input;
    IDraggable currentDraggable;

    void Start() {
        input = MouseInputHandler.instance;
    }

    void Update() {
        if (input.singleClick)
            ClickCast();
        if (input.doubleClick)
            DoubleClickCast();
        if (input.mouseDragStart)
            DragCast();
        if (input.mouseDrag)
            currentDraggable?.OnDragUpdate(input.mouseScreenPosition, input.mouseWorldPosition);
        if (input.mouseDragEnd) {
            currentDraggable?.OnDragEnd(input.mouseScreenPosition, input.mouseWorldPosition);
            currentDraggable = null;
        }
    }

    void ClickCast() {
        var hit = RaycastAt(input.mouseWorldPosition);
        if (hit) {
            IClickable clickable = hit.collider.GetComponent<IClickable>();
            clickable?.OnClick();
        }
    }

    void DoubleClickCast() {
        var hit = RaycastAt(input.mouseWorldPosition);
        if (hit) {
            IClickable clickable = hit.collider.GetComponent<IClickable>();
            clickable?.OnDoubleClick();
        }
    }

    void DragCast() {
        var hit = RaycastAt(input.startingDragWorldPos);
        if (hit)
            currentDraggable = hit.collider.GetComponent<IDraggable>();
        else
            currentDraggable = CameraControls.instance;
        currentDraggable?.OnDragStart(input.startingDragScreenPos, input.startingDragWorldPos);
    }

    RaycastHit2D RaycastAt(Vector2 position) {
        return Physics2D.Raycast(position, Vector2.zero, 0f, obstacleMask, raycastDepth.min, raycastDepth.max);
    }
}
