
//  Author: Alejandro Benítez López
//  Date: 01/03/2019
//  benitezdev@gmail.com

using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class MovementController : MonoBehaviour
{

    CharacterController controller;

    [Header("Playe Speeds")]
    [SerializeField] float playerRotationSpeed = 100;
    [SerializeField] float playerMovementSpeed = 10;

    [Space(5)]
    [SerializeField] float gravity;                     // Absolute value of Gravity
    float velocityY;                                    // Velocity of the character in Y

    [SerializeField] float jumpStrength;

    [Space(5)]
    [SerializeField] float debugRayLenght = 5;          // Lenght of debug ray


    private void Awake() { controller = GetComponent<CharacterController>(); }

    private void Update()
    {
        // Get the resultant direction
        Vector3 direction = transform.forward * Input.GetAxis("Vertical") +
                            transform.right   * Input.GetAxis("Horizontal");

        #region Debugs DrawRay
        // Draw foreward ray
        Debug.DrawRay(transform.position, transform.forward * debugRayLenght * Input.GetAxis("Vertical"), Color.blue);

        // Draw right/left ray
        Debug.DrawRay(transform.position, transform.right * debugRayLenght * Input.GetAxis("Horizontal"), Color.red);

        // Draw resultant direction
        Debug.DrawRay(transform.position, direction * debugRayLenght, Color.cyan);
        #endregion


        #region Calculate rotation
        // Filter if it is moving in any axis
        if (direction != Vector3.zero && direction.normalized != -transform.forward)
        {
            // Calculate the rotation according to the direction
            Quaternion rotation = Quaternion.LookRotation(direction);

            // Rotate the character progressively
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, Time.deltaTime * playerRotationSpeed);
        }
        #endregion

        // This Vector3 contain the direction foreward or backwards of the character(0,0,Z)
        Vector3 directorVector = Input.GetAxis("Vertical") * transform.forward * playerMovementSpeed;

        // Vector3 that represent the force in Y of the character (used for gravity and jump)
        Vector3 axisY = new Vector3(0, velocityY, 0);

        // Character Jump
        if (Input.GetButtonDown("Jump") && controller.isGrounded)
            velocityY = jumpStrength;

        // Character is in air
        if (!controller.isGrounded)
            velocityY -= gravity * Time.deltaTime; 

        // Resultant vector of the direction and Y axis
        directorVector += axisY;

        // Apply the movement to the character
        controller.Move(directorVector * Time.deltaTime);
      
        // Draw the movement direction
        Debug.DrawRay(transform.position, directorVector * debugRayLenght, Color.magenta);
    }
}
