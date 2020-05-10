using UnityEngine;
using UnityEngine.Animations;

public class SetMainCameraAsParent : MonoBehaviour {

    public bool useParentConstraintComponent;

    void Start() {
        if (useParentConstraintComponent)
            SetupParentConstraint();
        else
            transform.SetParent(Camera.main.transform);
    }

    void SetupParentConstraint() {
        Vector3 translationOffset = transform.position - Camera.main.transform.position;
        ParentConstraint constraint = gameObject.AddComponent<ParentConstraint>();
        constraint.AddSource(MakeConstraintSourceFromMainCamera());
        constraint.SetTranslationOffset(0, translationOffset);
        constraint.constraintActive = true;
    }

    ConstraintSource MakeConstraintSourceFromMainCamera() {
        return new ConstraintSource() {
            sourceTransform = Camera.main.transform,
            weight = 1
        };
    }
}
