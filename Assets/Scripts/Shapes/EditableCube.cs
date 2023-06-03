using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditableCube : EditablePrimitive {

    public override void PlaceOnSurface(Vector3 point, Vector3 normal) {
        Vector3 placePosition = point + (normal * (transform.localScale.y / 2)) + (normal * 0.0001f);
        transform.position = placePosition;
        transform.up = normal;
    }

}
