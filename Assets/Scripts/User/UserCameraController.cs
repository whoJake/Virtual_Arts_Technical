using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserCameraController : MonoBehaviour
{
    private Transform m_Camera;
    private Vector2 m_LookDirection;

    [SerializeField]
    private Vector2 mouseSensitivity;
    [SerializeField]
    private bool invertYAxis;

    [SerializeField]
    private Vector2 minMaxLookHeight;

    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        m_Camera = transform;
    }

    private void LateUpdate() {
        if (Cursor.lockState != CursorLockMode.Locked) return;

        Vector2 mouseMovement = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        m_LookDirection.x = (m_LookDirection.x + mouseMovement.x * mouseSensitivity.x) % 360;
        m_LookDirection.y = Mathf.Clamp((m_LookDirection.y + mouseMovement.y * mouseSensitivity.y), minMaxLookHeight.x - 90f, minMaxLookHeight.y - 90f);

        Vector3 eulerLookRotation = new Vector3(m_LookDirection.y, m_LookDirection.x, 0);
        eulerLookRotation.x *= invertYAxis ? 1 : -1;

        m_Camera.localRotation = Quaternion.Euler(eulerLookRotation);
    }

}
