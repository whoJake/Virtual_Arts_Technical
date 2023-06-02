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

    protected abstract void UpdateValidity();
    public abstract void PlaceOnSurface(Vector3 point, Vector3 normal, bool ignoreValidity);


    private void Update() {
        if (isSelected) {
            //Show scale editing controls
        }
    }

    public void Place(Vector3 point, bool ignoreValidity) {
        transform.position = point;
        if (!ignoreValidity) UpdateValidity();
    }

    public void Scale(Vector3 newScale, bool ignoreValidity) {
        transform.localScale = newScale;
        if (!ignoreValidity) UpdateValidity();
    }

    protected void UpdateMaterial() {
        Material activeMaterial;
        if (isMoving) {
            activeMaterial = isValid ? validMaterial : invalidMaterial;
        } else {
            activeMaterial = isSelected ? selectedMaterial : validMaterial;
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

    public void SetIsMoving(bool _isMoving) {
        isMoving = _isMoving;
        UpdateMaterial();
    }

    public void SetMaterials(Material validMat, Material invalidMat, Material selectedMat) {
        validMaterial = validMat;
        invalidMaterial = invalidMat;
        selectedMaterial = selectedMat;
    }
}
