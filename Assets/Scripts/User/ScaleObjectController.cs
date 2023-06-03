using UnityEngine;

public class ScaleObjectController : MonoBehaviour 
{
    [SerializeField]    EditablePrimitive target;

    Transform xAxis, yAxis, zAxis;
    Transform posXDrag, posYDrag, posZDrag;
    Transform negXDrag, negYDrag, negZDrag;

    bool isChangeHeld;
    Axis axisHeld;
    Transform heldPoint;
    bool dirHeld;

    public void Awake() {
        xAxis = transform.Find("X");
        yAxis = transform.Find("Y");
        zAxis = transform.Find("Z");

        posXDrag = transform.Find("PosX");
        posYDrag = transform.Find("PosY");
        posZDrag = transform.Find("PosZ");

        negXDrag = transform.Find("NegX");
        negYDrag = transform.Find("NegY");
        negZDrag = transform.Find("NegZ");
    }

    public void SelectObjectToControl(EditablePrimitive obj) {
        target = obj;
    }

    public void UpdateAxisVisual(Axis axis, float scale) {
        Transform axisBar;
        Transform axisPos;
        Transform axisNeg;

        float barLength;
        Vector3 direction;

        switch (axis) {
            case Axis.X:
                axisBar = xAxis;
                axisPos = posXDrag;
                axisNeg = negXDrag;

                barLength = target.transform.localScale.x;
                direction = target.transform.right;
                break;
            case Axis.Y:
                axisBar = yAxis;
                axisPos = posYDrag;
                axisNeg = negYDrag;

                barLength = target.transform.localScale.y;
                direction = target.transform.up;
                break;
            case Axis.Z:
                axisBar = zAxis;
                axisPos = posZDrag;
                axisNeg = negZDrag;

                barLength = target.transform.localScale.z;
                direction = target.transform.forward;
                break;
            default:
                throw new System.Exception("Why isn't there an axis set?");
        }

        axisBar.position = target.transform.position;
        axisBar.localScale = new Vector3(scale / 3f, barLength, scale / 3f);
        axisBar.up = direction;

        axisPos.position = axisBar.position + (direction * barLength / 2f);
        axisPos.localScale = Vector3.one * scale;
        axisNeg.position = axisBar.position - (direction * barLength / 2f);
        axisNeg.localScale = Vector3.one * scale;
    }

    void LateUpdate()
    {
        bool active = target != null;

        foreach(Transform child in transform) 
            child.gameObject.SetActive(active);
        

        if (!active) 
            return;
        if (Cursor.lockState != CursorLockMode.Locked) 
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (isChangeHeld) {
            Vector3 targetPos = target.transform.position;
            Vector3 heldPos = heldPoint.position;
            Vector3 vectorDifference = heldPos - targetPos;

            float distanceToCheck = Mathf.Min(Vector3.Distance(Camera.main.transform.position, heldPos), 75f);
            Vector3 heldEndOfRay = ray.origin + ray.direction * distanceToCheck;

            //Scalar returns scalar of cursor worldPosition projected onto the line between targetPos and heldPos
            float scalar = Vector3.Dot(heldEndOfRay - targetPos, vectorDifference) / Vector3.Dot(vectorDifference, vectorDifference);

            Vector3 newScale = target.transform.localScale;
            switch (axisHeld) {
                case Axis.X:
                    newScale.x *= scalar;
                    break;
                case Axis.Y:
                    newScale.y *= scalar;
                    break;
                case Axis.Z:
                    newScale.z *= scalar;
                    break;
            }
            target.Scale(newScale, axisHeld, dirHeld);

            if (Input.GetMouseButtonUp(0)) 
                isChangeHeld = false;
            
        }
        //See if user has grabbed onto an axis handle
        else if (Physics.Raycast(ray, out RaycastHit hit, 50f, LayerMask.GetMask("Scaler"))) {
            if (Input.GetMouseButtonDown(0)) {
                System.Enum.TryParse(hit.transform.name[^1].ToString(), out axisHeld);
                dirHeld = hit.transform.name[..3].Equals("Pos");
                isChangeHeld = true;
                heldPoint = hit.transform;
            }
        }

        float axisScale = Mathf.Clamp(Mathf.Min(target.transform.localScale.x, target.transform.localScale.y, target.transform.localScale.z), 1f, 3f) / 10f;
        UpdateAxisVisual(Axis.X, axisScale);
        UpdateAxisVisual(Axis.Y, axisScale);
        UpdateAxisVisual(Axis.Z, axisScale);
    }
}

public enum Axis {
    X,
    Y,
    Z,
    None
}
