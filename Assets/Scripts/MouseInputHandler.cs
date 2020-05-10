using UnityEngine;

public class MouseInputHandler : MonoBehaviour {

    static public MouseInputHandler instance { get; private set; }

    public float holdDistance = 0.1f;
    public float doubleClickTime = 0.2f;
    public bool debugging;

    public bool singleClick { get; private set; }
    public bool doubleClick { get; private set; }
    public bool mouseDrag { get; private set; }
    public bool mouseDragStart { get; private set; }
    public bool mouseDragEnd { get; private set; }
    public Vector2 startingDragScreenPos { get; private set; }
    public Vector2 startingDragWorldPos { get; private set; }

    public Vector2 mouseScreenPosition => Input.mousePosition;
    public Vector2 mouseWorldPosition => mainCamera.ScreenToWorldPoint(Input.mousePosition);
    public bool mouseButtonDown => Input.GetMouseButtonDown(0);
    public bool mouseButtonUp => Input.GetMouseButtonUp(0);

    Camera mainCamera;
    bool pressed;
    bool waitingForSecondClick;
    float elapsedTime;

    void Awake() {
        instance = (MouseInputHandler)Singleton.Setup(this, instance);
    }

    void Start() {
        mainCamera = Camera.main;
    }

    void Update() {
        ResetSingleFrameFlags();
        UpdateDoubleClickTimer();

        if (mouseButtonDown) {
            startingDragScreenPos = mouseScreenPosition;
            startingDragWorldPos = mouseWorldPosition;
            pressed = true;
        }

        if (mouseButtonUp && pressed) {
            if (!mouseDrag) {
                if (!waitingForSecondClick) {
                    waitingForSecondClick = true;
                    singleClick = true;
                }
                else {
                    waitingForSecondClick = false;
                    elapsedTime = 0f;
                    doubleClick = true;
                }
            }
            else
                mouseDragEnd = true;
            mouseDrag = false;
            pressed = false;
        }

        if (pressed && !mouseDrag && Vector2.Distance(mouseScreenPosition, startingDragScreenPos) > holdDistance) {
            mouseDrag = mouseDragStart = true;
        }

        if (debugging)
            ShowDebuggingFlags();
    }

    void ResetSingleFrameFlags() {
        singleClick = false;
        doubleClick = false;
        mouseDragStart = false;
        mouseDragEnd = false;
    }

    void UpdateDoubleClickTimer() {
        if (waitingForSecondClick) {
            elapsedTime += Time.deltaTime;
            if (elapsedTime > doubleClickTime) {
                waitingForSecondClick = false;
                elapsedTime = 0f;
            }
        }
    }

    void ShowDebuggingFlags() {
        if (singleClick)
            Debug.Log("single click");
        if (doubleClick)
            Debug.Log("double click");
        if (mouseDragStart)
            Debug.Log("mouse drag start");
        if (mouseDragEnd)
            Debug.Log("mouse drag end");
        if (mouseDrag)
            Debug.Log("mouse drag");
    }
}
