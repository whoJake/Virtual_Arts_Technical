using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UserEditingController : MonoBehaviour
{
    [SerializeField]
    private Transform creationParent;

    [SerializeField]
    private Material objectValidMaterial;

    [SerializeField]
    private Material objectInvalidMaterial;

    void Update()
    {
        if (Cursor.lockState != CursorLockMode.Locked) return;

        bool leftClicked = Input.GetMouseButtonDown(0);
        bool rightClicked = Input.GetMouseButtonDown(1);

        Ray targetRay = new Ray(transform.position, transform.forward);
        if (leftClicked) {

            EditablePrimative creation;
            if(Physics.Raycast(targetRay, out RaycastHit hit, 50f)) {
                GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                obj.transform.parent = creationParent;
                creation = obj.AddComponent<EditableCube>();
                creation.PlaceOnSurface(hit.point, hit.normal);
                creation.SetMaterial(creation.IsValid ? objectValidMaterial : objectInvalidMaterial);
            }

            Debug.DrawRay(targetRay.origin, targetRay.direction * 50, Color.red, 2f);
        }

    }
}
