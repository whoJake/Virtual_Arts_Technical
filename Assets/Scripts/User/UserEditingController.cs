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

    private Vector3 currentScaleSelection = Vector3.one;

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
            currentSelection.Deselect();
        }
        obj.Select();
        currentSelection = obj;
    }

    void DeselectSelection() {
        if (currentSelection)
            currentSelection.Deselect();
        currentSelection = null;
    }

    void MoveSelection(RaycastHit hit) {
        currentSelection.SetIsMoving(true);
        movingSelection = true;
        currentSelection.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        currentSelection.GetComponent<Collider>().enabled = false;
        currentSelection.PlaceOnSurface(hit.point, hit.normal, ignoreValidity);
    }

    void FinishMove() {
        if (currentSelection.IsValid || ignoreValidity) {
            currentSelection.gameObject.layer = LayerMask.NameToLayer("Default");
            currentSelection.GetComponent<Collider>().enabled = true;
            currentSelection.SetIsMoving(false);
            movingSelection = false;
        } else {
            Debug.Log("Place position is invalid");
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

    void Update()
    {
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

        bool scaleUp = Input.GetKey(KeyCode.E);
        bool scaleDown = Input.GetKey(KeyCode.Q);

        if (isDraggingOutObject) {
            if (!leftHeld) {
                FinishMove();
                isDraggingOutObject = false;
                if (!currentSelection.IsValid) {
                    Destroy(currentSelection.gameObject);
                    currentSelection = null;
                } else {
                    DeselectSelection();
                }
            } else {
                Ray dragRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                if(Physics.Raycast(dragRay, out RaycastHit draghit, 25f, ~LayerMask.GetMask("Ignore Raycast"))) {
                    MoveSelection(draghit);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Delete) && cursorLocked) {
            if (currentSelection) {
                Destroy(currentSelection.gameObject);
                currentSelection = null;
            }
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit hit, 25f, ~LayerMask.GetMask("Ignore Raycast"))) {
            if(leftClicked && cursorLocked) {
                if (movingSelection) { }
                else if (hit.transform.CompareTag("EditableObject")) {
                    EditablePrimitive hitObject = hit.transform.GetComponent<EditablePrimitive>();
                    if (hitObject == currentSelection) {
                        DeselectSelection();
                    } else {
                        SelectObject(hitObject);
                        currentScaleSelection = currentSelection.transform.localScale;
                    }
                } else {
                    DeselectSelection();
                }
            }

            if (Input.GetKeyDown(KeyCode.F)) {
                EditablePrimitive obj = CreateNewObject();
                SelectObject(obj);
                MoveSelection(hit);
                FinishMove();
            }

            //Preview is present
            if (currentSelection) {
                if (scaleUp) {
                    currentScaleSelection += Vector3.one * Time.deltaTime;
                }
                if (scaleDown) {
                    currentScaleSelection -= Vector3.one * Time.deltaTime;
                }
                currentSelection.transform.localScale = currentScaleSelection;
                //currentSelection.Place(currentSelection.transform.position, ignoreValidity);

                if (rightHeld && cursorLocked) {
                    MoveSelection(hit);
                } else if(!rightHeld && cursorLocked) {
                    FinishMove();
                }
                

            }

        }

    }

    public enum SelectPrimitive {
        None,
        Cube,
        Sphere
    }

}
