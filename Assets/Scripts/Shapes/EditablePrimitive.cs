using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EditablePrimative : MonoBehaviour
{
    protected bool isValid;
    public bool IsValid { get { return isValid; } }

    protected Material validMaterial;
    protected Material invalidMaterial;

    protected Transform lastPlaceState;

    public abstract void PlaceOnSurface(Vector3 point, Vector3 normal, bool ignoreValidity);
    public void Select() {
        lastPlaceState = transform;
    }
    public void Place() {
        lastPlaceState = transform;
    }
    public abstract void Scale();
    public abstract void Delete();

    public void SetMaterials(Material validMat, Material invalidMat) {
        validMaterial = validMat;
        invalidMaterial = invalidMat;
    }
}
