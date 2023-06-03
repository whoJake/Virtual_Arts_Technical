using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EditablePrimitive : MonoBehaviour
{
    protected bool isValid;
    public bool IsValid { get { return isValid; } }

    private bool isSelected;
    public bool isMoving;

    protected Material validMaterial;
    protected Material invalidMaterial;
    protected Material selectedMaterial;

    protected Transform lastPlaceState;

    public abstract void UpdateValidity();
    public abstract void PlaceOnSurface(Vector3 point, Vector3 normal, bool ignoreValidity);

    public void Place(Vector3 point, bool ignoreValidity) {
        transform.position = point;
        if (!ignoreValidity) UpdateValidity();
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
        UpdateMaterial();
    }

    public void Deselect() {
        isSelected = false;
        UpdateMaterial();
    }

    public void SetMaterials(Material validMat, Material invalidMat, Material selectedMat) {
        validMaterial = validMat;
        invalidMaterial = invalidMat;
        selectedMaterial = selectedMat;
    }
}
