using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EditablePrimative : MonoBehaviour
{
    protected bool isSelected;
    public bool IsSelected { get { return isSelected; } }

    protected bool isValid;
    public bool IsValid { get { return isValid; } }

    public abstract void PlaceOnSurface(Vector3 point, Vector3 normal);
    public abstract void Place();
    public abstract void Scale();
    public abstract void Delete();
    public abstract void SetMaterial(Material material);
}
