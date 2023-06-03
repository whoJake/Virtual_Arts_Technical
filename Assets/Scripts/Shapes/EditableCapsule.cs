using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditableCapsule : EditablePrimitive {
    public override void PlaceOnSurface(Vector3 point, Vector3 normal) {
        CapsuleCollider collider = GetComponent<CapsuleCollider>();
        float realHeight = Mathf.Max(collider.height * transform.localScale.y, collider.radius * transform.localScale.x * 2);

        Vector3 placePosition = point + (normal * (realHeight / 2)) + (normal * 0.0001f);
        transform.position = placePosition;
        transform.up = normal;
    }

    public override void Scale(Vector3 newScale, Axis changedAxis, bool dir) {
        float dirMultiplier = dir ? -1 : 1;
        float change;
        switch (changedAxis) {
            case Axis.X:
                change = transform.localScale.x - newScale.x;
                transform.position += (change * dirMultiplier / 2f) * transform.right;
                newScale.z = newScale.x;
                break;
            case Axis.Y:
                change = transform.localScale.y - newScale.y;
                transform.position += (change * dirMultiplier / 2f) * transform.up;
                break;
            case Axis.Z:
                change = transform.localScale.z - newScale.z;
                transform.position += (change * dirMultiplier / 2f) * transform.forward;
                newScale.x = newScale.z;
                break;
            default:
                return;
        }

        //Clamp size to avoid negative scales :3
        newScale = Vector3.Max(Vector3.one * 0.2f, newScale);

        transform.localScale = newScale;
    }

}

