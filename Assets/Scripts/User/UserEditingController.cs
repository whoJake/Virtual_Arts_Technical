using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UserEditingController : MonoBehaviour
{
    [SerializeField]
    private Transform creationParent;

    [SerializeField]
    private bool ignoreValidity;

    [SerializeField]
    private Material objectValidMaterial;

    [SerializeField]
    private Material objectInvalidMaterial;

    [SerializeField]
    private Material objectSelectedMaterial;

    [SerializeField]
    private Material objectFresnelSelectedMaterial;

    [SerializeField]
    private Material objectFresnelInvalidMaterial;

    [SerializeField]
    private ScaleObjectController scaleObjectController;

    [SerializeField]
    private ColorPickerHandler colorPicker;

    private bool isDraggingOutObject;

    private SelectPrimitive hotbarSelection;

    [SerializeField]
    private Color currentColor = Color.white;

    [SerializeField]
    private EditablePrimitive currentSelection;

    public void ChangeColor(Color color) {
        if (currentSelection) {
            currentSelection.SetColor(color);
        }

        currentColor = color;
    }

    public void DeleteSelected() {
        if (currentSelection) {
            Destroy(currentSelection.gameObject);
            currentSelection = null;
            scaleObjectController.SelectObjectToControl(null);
        }
    }

    public void DragOutObject(string type) {
        SelectPrimitive hotbar;
        switch (type) {
            case "Cube":
                hotbar = SelectPrimitive.Cube;
                break;
            case "Sphere":
                hotbar = SelectPrimitive.Sphere;
                break;
            case "Capsule":
                hotbar = SelectPrimitive.Capsule;
                break;
            default:
                hotbar = SelectPrimitive.None;
                break;
        }
        hotbarSelection = hotbar;

        EditablePrimitive obj = CreateNewObject();
        SelectObject(obj);
        isDraggingOutObject = true;
    }

    void SelectObject(EditablePrimitive obj) {
        if (currentSelection) {
            if (!currentSelection.IsValid) return;

            currentSelection.Deselect();
        }

        obj.Select();
        currentSelection = obj;
        scaleObjectController.SelectObjectToControl(currentSelection);
        colorPicker.SetColor(currentSelection.curColor);
    }

    void DeselectSelection() {
        if (currentSelection) {
            if (!currentSelection.IsValid) return;

            currentSelection.Deselect();
            currentSelection = null;
            scaleObjectController.SelectObjectToControl(null);
        }
    }

    void MoveSelection(RaycastHit hit) {
        currentSelection.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        currentSelection.PlaceOnSurface(hit.point, hit.normal);
    }

    void FinishMove() {
        if (currentSelection.IsValid || ignoreValidity) {
            currentSelection.gameObject.layer = LayerMask.NameToLayer("Default");
        }
    }

    EditablePrimitive CreateNewObject() {
        if (currentSelection) FinishMove();

        GameObject tempShape; ;
        EditablePrimitive result;

        switch (hotbarSelection) {
            case SelectPrimitive.None:
                return null;
            case SelectPrimitive.Cube:
                tempShape = GameObject.CreatePrimitive(PrimitiveType.Cube);
                result = tempShape.AddComponent<EditableCube>();
                result.SetMaterials(objectValidMaterial, objectInvalidMaterial, objectSelectedMaterial);
                break;
            case SelectPrimitive.Sphere:
                tempShape = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                result = tempShape.AddComponent<EditableSphere>();
                result.SetMaterials(objectValidMaterial, objectFresnelInvalidMaterial, objectFresnelSelectedMaterial);
                break;
            case SelectPrimitive.Capsule:
                tempShape = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                result = tempShape.AddComponent<EditableCapsule>();
                result.SetMaterials(objectValidMaterial, objectFresnelInvalidMaterial, objectFresnelSelectedMaterial);
                break;
            default:
                return null;
        }

        result.SetColor(currentColor);
        tempShape.transform.parent = creationParent;
        tempShape.tag = "EditableObject";
        Rigidbody detector = tempShape.AddComponent<Rigidbody>();
        detector.isKinematic = true;
        detector.useGravity = false;
        return result;
    }

    void Update() {
        if (Input.GetKey(KeyCode.R) || isDraggingOutObject) {
            Cursor.lockState = CursorLockMode.None;
        } else {
            Cursor.lockState = CursorLockMode.Locked;
        }

        bool cursorLocked = Cursor.lockState == CursorLockMode.Locked;

        bool leftClicked = Input.GetMouseButtonDown(0);
        bool rightClicked = Input.GetMouseButtonDown(1);

        bool leftHeld = Input.GetMouseButton(0);
        bool rightHeld = Input.GetMouseButton(1);

        if (isDraggingOutObject) {
            if (!leftHeld) {
                FinishMove();
                isDraggingOutObject = false;
                if (!currentSelection.IsValid) {
                    Destroy(currentSelection.gameObject);
                    currentSelection = null;
                    scaleObjectController.SelectObjectToControl(null);
                } else {
                    DeselectSelection();
                }
            } else {
                Ray dragRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(dragRay, out RaycastHit draghit, 25f, ~LayerMask.GetMask("Ignore Raycast", "Overlay", "Scaler"))) {
                    MoveSelection(draghit);
                }
            }
        }
        //Already accounted for dragging out action so can leave from here
        if (!cursorLocked) return;

        if (Input.GetKeyDown(KeyCode.Delete)) {
            DeleteSelected();
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        bool hitActiveObject = Physics.Raycast(ray, out RaycastHit activeHit, 50f, ~LayerMask.GetMask("Ignore Raycast", "Overlay", "Scaler"));
        bool hitScalarAxis = Physics.Raycast(ray, out _, 75f, LayerMask.GetMask("Scaler"));

        EditablePrimitive hitEditable = null;
        bool hasHitEditable = hitActiveObject ? activeHit.transform.TryGetComponent(out hitEditable) : false;

        //When an item is selected
        if (currentSelection) {
            if (rightHeld && hitActiveObject) {
                MoveSelection(activeHit);
            } else if (!rightHeld) {
                FinishMove();
            }

            if (leftClicked && !hitScalarAxis) {
                if (hitEditable == currentSelection) {
                    DeselectSelection();
                }else if (hasHitEditable) {
                    SelectObject(hitEditable);
                } else {
                    DeselectSelection();
                }
            }
        } else {
            if (leftClicked && hasHitEditable) {
                SelectObject(hitEditable);
            }
        }
        
    }

    public enum SelectPrimitive {
        None,
        Cube,
        Sphere,
        Capsule
    }

}
