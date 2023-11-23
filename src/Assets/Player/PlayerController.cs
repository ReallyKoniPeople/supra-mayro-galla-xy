using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float moveSpeed = 10f, rotateSpeed = 10f;
    [SerializeField] private Transform playerModel;
    [SerializeField] private CustomGravity _customGravity;
    [SerializeField] private float jumpForce;

    private Vector3 playerInput;
    private bool readyToJump;

    public AudioSource jumpAudioSource;
    public AudioSource walkAudioSource;
    public AudioSource deathAudioSource;

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
        transform.position += moveSpeed * playerInput.z * Time.deltaTime * playerModel.forward;
        if (!walkAudioSource.isPlaying && playerInput.z != 0f)
        {
            walkAudioSource.PlayOneShot(walkAudioSource.clip, 1f);
        }
        else if (walkAudioSource.isPlaying && playerInput.z == 0f)
        {
            walkAudioSource.Stop();
        }
    }

    private void RotateCharacter()
    {
        playerModel.Rotate(Vector3.up, playerInput.x * rotateSpeed * Time.deltaTime);
    }

    public void JumpWithoutSound()
    {
        rb.velocity = Vector3.zero;
        rb.AddForce(_customGravity.currentNormal * jumpForce, ForceMode.VelocityChange);
    }
    private void Jump()
    {
        JumpWithoutSound();
        jumpAudioSource.PlayOneShot(jumpAudioSource.clip, 1f);
    }

    public void PlayerDeath()
    {
        if (!deathAudioSource.isPlaying)
        {
            DontDestroyOnLoad(deathAudioSource);
            deathAudioSource.PlayOneShot(deathAudioSource.clip, 1f);
        }
        SceneManager.LoadScene(1);
    }
}
