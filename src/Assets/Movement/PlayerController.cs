using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float moveSpeed = 10f, rotateSpeed = 10f;
    [SerializeField] private Transform playerModel;
    [SerializeField] private CustomGravity _customGravity;
    [SerializeField] private float jumpForce;

    private Vector3 playerInput;
    private bool readyToJump;

    private void CheckInput()
    {
        playerInput.x = Input.GetAxisRaw("Horizontal");
        playerInput.z = Input.GetAxisRaw("Vertical");
        if (Input.GetKeyDown(KeyCode.Space) && readyToJump)
        {
            readyToJump = false;
            Jump();
        }
    }

    private void Update()
    {
        CheckInput();
    }

    private void FixedUpdate()
    {
        RotateCharacter();
        Move();
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Check if player collided with Object containing the "whatIsGround"-Layer
        readyToJump = true;
    }

    private void Move()
    {
        transform.position += playerModel.forward * playerInput.z * moveSpeed * Time.deltaTime;
    }

    private void RotateCharacter()
    {
        playerModel.Rotate(Vector3.up, playerInput.x * rotateSpeed * Time.deltaTime);
    }

    private void Jump()
    {
        rb.AddForce(_customGravity.currentNormal * jumpForce, ForceMode.VelocityChange);
    }
}
