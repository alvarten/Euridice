using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private CharacterController controller;
    private Camera cam;

    [SerializeField] private Transform spriteTransform; // Asigna aquí el objeto hijo con el sprite

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

        // Dirección de la cámara
        Vector3 camForward = cam.transform.forward;
        camForward.y = 0;
        camForward.Normalize();

        Vector3 camRight = cam.transform.right;
        camRight.y = 0;
        camRight.Normalize();

        Vector3 move = inputDir.z * camForward + inputDir.x * camRight;

        controller.SimpleMove(move * moveSpeed);

        // Flip del sprite en X según la dirección horizontal del input
        if (h > 0.1f)
            spriteTransform.localScale = new Vector3(1, 1, 1);
        else if (h < -0.1f)
            spriteTransform.localScale = new Vector3(-1, 1, 1);
    }
}
