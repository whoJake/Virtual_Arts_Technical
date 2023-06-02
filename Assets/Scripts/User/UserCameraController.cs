using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserCameraController : MonoBehaviour
{
    private Transform affectedTransform;
    private Vector2 lookDirection;

    [SerializeField]
    private Vector2 mouseSensitivity;
    [SerializeField]
    private bool invertYAxis;

    [SerializeField]
    private Vector2 minMaxLookHeight;

    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        affectedTransform = transform;
    }

    private void LateUpdate() {
        if (Cursor.lockState != CursorLockMode.Locked) return;

        Vector2 mouseMovement = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        lookDirection.x = (lookDirection.x + mouseMovement.x * mouseSensitivity.x) % 360;
        lookDirection.y = Mathf.Clamp((lookDirection.y + mouseMovement.y * mouseSensitivity.y), minMaxLookHeight.x - 90f, minMaxLookHeight.y - 90f);

        Vector3 eulerLookRotation = new Vector3(lookDirection.y, lookDirection.x, 0);
        eulerLookRotation.x *= invertYAxis ? 1 : -1;

        affectedTransform.localRotation = Quaternion.Euler(eulerLookRotation);
    }

}
