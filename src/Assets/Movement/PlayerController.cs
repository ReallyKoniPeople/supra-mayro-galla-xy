using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float moveSpeed = 10f, rotateSpeed= 10f;
    [SerializeField] private Transform playerModel;
    [SerializeField] private CustomGravity _customGravity;
    [SerializeField] private float jumpForce;

    private Vector3 playerInput;
    private bool jump;
    
    private void CheckInput()
    {
        playerInput.x = Input.GetAxisRaw("Horizontal");
        playerInput.z = Input.GetAxisRaw("Vertical");
        if(!jump)
        jump = Input.GetKeyDown(KeyCode.Space);
    }

    private void Update()
    {
        CheckInput();
    }

    private void FixedUpdate()
    {
        RotateCharacter();
        Move();
        if (jump)
        {
            jump = false;
            Jump();
        }
    }

    private void Move()
    {
        transform.position += playerModel.forward *playerInput.z* moveSpeed * Time.deltaTime;
    }

    private void RotateCharacter()
    {
        playerModel.Rotate(Vector3.up,playerInput.x*rotateSpeed*Time.deltaTime);
    }

    private void Jump()
    {
        rb.AddForce(_customGravity.currentNormal*jumpForce,ForceMode.VelocityChange);
    }
}
