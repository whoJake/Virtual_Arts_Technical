using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserMovementController : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed;

    [SerializeField]
    public float verticalSpeed;

    void Update()
    {
        Vector2 inputDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector3 movementDirection = transform.right * inputDirection.x + transform.forward * inputDirection.y;
        movementDirection.y = 0;
        movementDirection = movementDirection.normalized * movementSpeed;

        float upDownVelocity = Input.GetAxisRaw("VerticalControl") * verticalSpeed;
        movementDirection.y = upDownVelocity;

        transform.position += movementDirection * Time.deltaTime;
    }
}
