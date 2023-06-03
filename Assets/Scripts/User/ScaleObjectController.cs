using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ScaleObjectController : MonoBehaviour {
    [SerializeField]
    EditablePrimitive controlling;

    [SerializeField]
    float dragSensitivity;

    Transform xAxis, yAxis, zAxis;
    Transform posXDrag, posYDrag, posZDrag;
    Transform negXDrag, negYDrag, negZDrag;

    public bool changeHeld;
    Axis axisHeld;
    float dir;
    Transform heldPoint;


    public void Awake() {
        xAxis = transform.Find("X");
        yAxis = transform.Find("Y");
        zAxis = transform.Find("Z");

        posXDrag = transform.Find("PosX");
        posYDrag = transform.Find("PosY");
        posZDrag = transform.Find("PosZ");

        negXDrag = transform.Find("NegX");
        negYDrag = transform.Find("NegY");
        negZDrag = transform.Find("NegZ");
    }

    public void SelectObjectToControl(EditablePrimitive obj) {
        controlling = obj;
    }

    public enum Axis {
        X,
        Y,
        Z,
        None
    }

    public void UpdateAxisVisual(Axis axis, float scale) {
        Transform axisBar;
        Transform axisPos;
        Transform axisNeg;

        float barLength;
        Vector3 direction;

        switch (axis) {
            case Axis.X:
                axisBar = xAxis;
                axisPos = posXDrag;
                axisNeg = negXDrag;

                barLength = controlling.transform.localScale.x;
                direction = controlling.transform.right;
                break;
            case Axis.Y:
                axisBar = yAxis;
                axisPos = posYDrag;
                axisNeg = negYDrag;

                barLength = controlling.transform.localScale.y;
                direction = controlling.transform.up;
                break;
            case Axis.Z:
                axisBar = zAxis;
                axisPos = posZDrag;
                axisNeg = negZDrag;

                barLength = controlling.transform.localScale.z;
                direction = controlling.transform.forward;
                break;
            default:
                throw new System.Exception("Lol");
        }

        axisBar.position = controlling.transform.position;
        axisBar.localScale = new Vector3(scale / 3f, barLength, scale / 3f);
        axisBar.up = direction;

        axisPos.position = axisBar.position + (direction * barLength / 2f);
        axisPos.localScale = Vector3.one * scale;
        axisNeg.position = axisBar.position - (direction * barLength / 2f);
        axisNeg.localScale = Vector3.one * scale;
    }

    void LateUpdate()
    {
        bool active;
        if(controlling == null) {
            active = false;
        } else {
            active = true;
        }
        foreach(Transform child in transform) {
            child.gameObject.SetActive(active);
        }
        if (!active) return;
        if (Cursor.lockState != CursorLockMode.Locked) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (changeHeld) {
            Vector3 start = controlling.transform.position;
            Vector3 end = heldPoint.position;
            Vector3 startEnd = end - start;

            float distanceToCheck = Mathf.Min(Vector3.Distance(Camera.main.transform.position, end), 75f);
            Vector3 heldEndOfRay = ray.origin + ray.direction * distanceToCheck;

            float scalar = Vector3.Dot(heldEndOfRay - start, startEnd) / Vector3.Dot(startEnd, startEnd);

            float distanceDragged;
            Vector3 scale = controlling.transform.localScale;

            switch (axisHeld) {
                case Axis.X:
                    distanceDragged = scale.x - (scale.x * scalar);
                    scale.x *= scalar;
                    controlling.transform.position += (controlling.transform.right * distanceDragged * dir) / 2f;
                    break;
                case Axis.Y:
                    distanceDragged = scale.y - (scale.y * scalar);
                    scale.y *= scalar;
                   controlling.transform.position += (controlling.transform.up * distanceDragged * dir) / 2f;

                    break;
                case Axis.Z:
                    distanceDragged = scale.z - (scale.z * scalar);
                    scale.z *= scalar;
                    controlling.transform.position += (controlling.transform.forward * distanceDragged * dir) / 2f;
                    break;
            }
            controlling.transform.localScale = scale;
            controlling.UpdateValidity();

            if (Input.GetMouseButtonUp(0)) {
                 changeHeld = false;
            }
        }
        else if (Physics.Raycast(ray, out RaycastHit hit, 50f, LayerMask.GetMask("Scaler"))) {
            if (Input.GetMouseButtonDown(0)) {
                Enum.TryParse(hit.transform.name[^1].ToString(), out axisHeld);
                dir = hit.transform.name[..3].Equals("Pos") ? -1 : 1;
                changeHeld = true;
                heldPoint = hit.transform;
            }
        }

        float axisScale = Mathf.Clamp(Mathf.Min(controlling.transform.localScale.x, controlling.transform.localScale.y, controlling.transform.localScale.z), 1f, 3f) / 10f;
        UpdateAxisVisual(Axis.X, axisScale);
        UpdateAxisVisual(Axis.Y, axisScale);
        UpdateAxisVisual(Axis.Z, axisScale);
    }
}
