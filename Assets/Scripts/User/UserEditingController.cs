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
    private ScaleObjectController scaleObjectController;

    private bool movingSelection;
    private bool isDraggingOutObject;

    public SelectPrimitive hotbarSelection;

    [SerializeField]
    private EditablePrimitive currentSelection;

    public void DragOutObject(string type) {
        SelectPrimitive hotbar = SelectPrimitive.None;
        switch (type) {
            case "Cube":
                hotbar = SelectPrimitive.Cube;
                break;
            case "Sphere":
                hotbar = SelectPrimitive.Sphere;
                break;
        }

        EditablePrimitive obj = CreateNewObject();
        SelectObject(obj);
        hotbarSelection = hotbar;
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
    }

    void DeselectSelection() {
        Debug.Log("Tried to deselect");
        Debug.Log("is valid : " + currentSelection.IsValid + " collider enabled: " + currentSelection.GetComponent<Collider>().enabled + " is moving: " + currentSelection.isMoving);
        if (currentSelection) {
            if (!currentSelection.IsValid) return;
            Debug.Log("Deselecting");

            currentSelection.Deselect();
            currentSelection = null;
            scaleObjectController.SelectObjectToControl(null);
        }
    }

    void MoveSelection(RaycastHit hit) {
        movingSelection = true;
        currentSelection.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        currentSelection.PlaceOnSurface(hit.point, hit.normal, ignoreValidity);
    }

    void FinishMove() {
        if (currentSelection.IsValid || ignoreValidity) {
            currentSelection.gameObject.layer = LayerMask.NameToLayer("Default");
            movingSelection = false;
        }
    }

    EditablePrimitive CreateNewObject() {
        if (currentSelection) FinishMove();
        switch (hotbarSelection) {
            case SelectPrimitive.None:
                return null;
            case SelectPrimitive.Cube:
                GameObject temp = GameObject.CreatePrimitive(PrimitiveType.Cube);
                temp.transform.parent = creationParent;
                temp.tag = "EditableObject";
                EditableCube script = temp.AddComponent<EditableCube>();
                script.SetMaterials(objectValidMaterial, objectInvalidMaterial, objectSelectedMaterial);
                return script;
            case SelectPrimitive.Sphere:
                return null;
            default:
                return null;
        }
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

        if (Input.GetKeyDown(KeyCode.Delete) && cursorLocked) {
            if (currentSelection) {
                Destroy(currentSelection.gameObject);
                currentSelection = null;
                scaleObjectController.SelectObjectToControl(null);
            }
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
        Sphere
    }

}
