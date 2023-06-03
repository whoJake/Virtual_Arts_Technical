using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
public class EditableSphere : EditablePrimitive {

    public override void PlaceOnSurface(Vector3 point, Vector3 normal, bool igoreValidity) {
        Vector3 placePosition = point + (normal * (transform.localScale.y / 2)) + (normal * 0.0001f);
        transform.position = placePosition;
        transform.up = normal;

        if (!igoreValidity) UpdateValidity();
    }
    
    public override void UpdateValidity() {
        GetComponent<Collider>().enabled = false;
        isValid = !Physics.CheckSphere(transform.position, transform.localScale / 2f, transform.rotation, ~LayerMask.GetMask("IgnoreValidityCheck", "Overlay", "Scaler"));
        GetComponent<Collider>().enabled = true;
        UpdateMaterial();
    }
    


}
*/
