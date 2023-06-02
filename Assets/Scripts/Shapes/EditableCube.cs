using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditableCube : EditablePrimative {

    public override void PlaceOnSurface(Vector3 point, Vector3 normal) {
        Vector3 placePosition = point + (normal * (transform.localScale.y / 2)) + (normal * 0.001f);
        transform.position = placePosition;
        transform.up = normal;

        UpdateValidity();
    }

    private void UpdateValidity() {
        isValid = !Physics.CheckBox(transform.position, transform.localScale / 2f, transform.rotation, ~LayerMask.GetMask("IgnoreValidityCheck"));
        Debug.Log(isValid);
    }

    public override void Delete() {
        throw new System.NotImplementedException();
    }

    public override void Place() {
        throw new System.NotImplementedException();
    }

    public override void SetMaterial(Material material) {
        GetComponent<MeshRenderer>().sharedMaterial = material;
    }
     
    public override void Scale() {
        throw new System.NotImplementedException();
    }
}
