using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float gravity = -9.81f;
    public SFXPlayer sfxPlayer;
    private bool isWalkingSoundPlaying = false;
    private float verticalVelocity = 0f;
    private CharacterController controller;
    private Camera cam;

    [SerializeField] private Transform spriteTransform;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        cam = Camera.main;
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 inputDir = new Vector3(h, 0, v);
        inputDir.Normalize();

        // Dirección de la cámara
        Vector3 camForward = cam.transform.forward;
        camForward.y = 0;
        camForward.Normalize();

        Vector3 camRight = cam.transform.right;
        camRight.y = 0;
        camRight.Normalize();

        Vector3 move = inputDir.z * camForward + inputDir.x * camRight;
        move *= moveSpeed;

        // Gravedad
        if (controller.isGrounded)
        {
            verticalVelocity = -1f; // Mantiene el personaje pegado al suelo
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }

        move.y = verticalVelocity;

        controller.Move(move * Time.deltaTime);

        //Logica para el sonido de pasos
        bool isMoving = inputDir.magnitude > 0.1f;

        if (isMoving && !isWalkingSoundPlaying && controller.isGrounded)
        {
            sfxPlayer.PlayLooping(sfxPlayer.stepsClip, 0.2f, 0.40f);
            isWalkingSoundPlaying = true;
        }
        else if (!isMoving && isWalkingSoundPlaying)
        {
            sfxPlayer.StopLooping();
            isWalkingSoundPlaying = false;
        }
        // Flip del sprite
        if (h > 0.1f)
            spriteTransform.localScale = new Vector3(1, 1, 1);
        else if (h < -0.1f)
        {
            spriteTransform.localScale = new Vector3(-1, 1, 1);
        }
        //spriteTransform.localScale = new Vector3(-1, 1, 1);

        

    }
}
