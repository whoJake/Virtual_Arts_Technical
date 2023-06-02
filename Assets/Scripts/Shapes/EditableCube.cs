using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditableCube : EditablePrimative {

    public override void PlaceOnSurface(Vector3 point, Vector3 normal, bool igoreValidity) {
        Vector3 placePosition = point + (normal * (transform.localScale.y / 2)) + (normal * 0.0001f);
        transform.position = placePosition;
        transform.up = normal;

        if(!igoreValidity) UpdateValidity();
    }

    protected override void UpdateValidity() {
        isValid = !Physics.CheckBox(transform.position, transform.localScale / 2f, transform.rotation, ~LayerMask.GetMask("IgnoreValidityCheck"));
        GetComponent<MeshRenderer>().sharedMaterial = isValid ? validMaterial : invalidMaterial;
    }

    public override void Delete() {
        throw new System.NotImplementedException();
    }
}
