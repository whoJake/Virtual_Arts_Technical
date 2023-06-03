using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditableSphere : EditablePrimitive {
    public override void PlaceOnSurface(Vector3 point, Vector3 normal, bool keepRotation) {
        Vector3 placePosition = point + (normal * (transform.localScale.y / 2)) + (normal * 0.0001f);
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
                newScale.y = newScale.x;
                newScale.z = newScale.x;
                break;
            case Axis.Y:
                change = transform.localScale.y - newScale.y;
                transform.position += (change * dirMultiplier / 2f) * transform.up;
                newScale.x = newScale.y;
                newScale.z = newScale.y;
                break;
            case Axis.Z:
                change = transform.localScale.z - newScale.z;
                transform.position += (change * dirMultiplier / 2f) * transform.forward;
                newScale.x = newScale.z;
                newScale.y = newScale.z;
                break;
            default:
                return;
        }

        //Clamp size to avoid negative scales :3
        newScale = Vector3.Max(Vector3.one * 0.2f, newScale);

        transform.localScale = newScale;
    }

}
