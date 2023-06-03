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
        validMaterial = validMat;
        invalidMaterial = invalidMat;
        selectedMaterial = selectedMat;
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
