using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EditablePrimitive : MonoBehaviour
{
    protected bool isValid = true;
    public bool IsValid { get { return isValid; } }

    private int enteredColliderCount;

    private bool isSelected;
    public bool isMoving;

    protected Material validMaterial;
    protected Material invalidMaterial;
    protected Material selectedMaterial;

    protected Transform lastPlaceState;

    public abstract void PlaceOnSurface(Vector3 point, Vector3 normal);
    public abstract void Scale(Vector3 newScale, Axis changedAxis, bool dir);

    public void SetColor(Color color) {
        validMaterial.SetColor("_ObjectColor", color);
        invalidMaterial.SetColor("_ObjectColor", color);
        selectedMaterial.SetColor("_ObjectColor", color);
    }

    protected void UpdateMaterial() {
        Material activeMaterial;
        if (isSelected) {
            activeMaterial = isValid ? selectedMaterial : invalidMaterial;
        } else {
            activeMaterial = isValid ? validMaterial : invalidMaterial;
        }

        GetComponent<MeshRenderer>().sharedMaterial = activeMaterial;
    }

    public void Select() {
        isSelected = true;
        GetComponent<Collider>().isTrigger = true;
        UpdateMaterial();
    }

    public void Deselect() {
        isSelected = false;
        GetComponent<Collider>().isTrigger = false;
        UpdateMaterial();
    }

    public void SetMaterials(Material validMat, Material invalidMat, Material selectedMat) {
        validMaterial = new Material(validMat);
        invalidMaterial = new Material(invalidMat);
        selectedMaterial = new Material(selectedMat);
    }

    private void OnTriggerEnter(Collider other) {
        if (!isSelected) return;
        if (other.CompareTag("Ignore Validity"))
            return;

        enteredColliderCount++;
        isValid = false;
        UpdateMaterial();
    }

    private void OnTriggerExit(Collider other) {
        if (!isSelected) return;

        if (other.CompareTag("Ignore Validity"))
            return;

        enteredColliderCount--;
        if(enteredColliderCount == 0) {
            isValid = true;
        }
        UpdateMaterial();
    }
}
