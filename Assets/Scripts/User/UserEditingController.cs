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

    public SelectPrimitive hotbarSelection;
    private EditablePrimative currentSelection;
    private EditablePrimative currentPreview;

    void PlacePreview() {
        if (currentPreview.IsValid || ignoreValidity) {
            currentPreview.gameObject.layer = LayerMask.NameToLayer("Default");
            currentPreview.GetComponent<Collider>().enabled = true;
            currentPreview = null;
        } else {
            Debug.Log("Place position is invalid");
        }
    }

    void Update()
    {
        if (Cursor.lockState != CursorLockMode.Locked) return;
        bool leftClicked = Input.GetMouseButtonDown(0);
        bool rightClicked = Input.GetMouseButtonDown(1);

        if(Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 25f, ~LayerMask.GetMask("Ignore Raycast"))) {
            //Need to create a preview
            if(!currentPreview && !currentSelection) {
                switch (hotbarSelection) {
                    case SelectPrimitive.None:
                        break;
                    case SelectPrimitive.Cube:
                        Debug.Log("made new preview");
                        GameObject temp = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        temp.transform.parent = creationParent;
                        temp.layer = LayerMask.NameToLayer("Ignore Raycast");
                        temp.GetComponent<Collider>().enabled = false;
                        currentPreview = temp.AddComponent<EditableCube>();
                        currentPreview.SetMaterials(objectValidMaterial, objectInvalidMaterial);
                        break;
                    case SelectPrimitive.Sphere:
                        break;
                }
            }

            //Preview is present
            if (currentPreview) {
                currentPreview.PlaceOnSurface(hit.point, hit.normal, ignoreValidity);

                if (rightClicked) {
                    PlacePreview();
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
