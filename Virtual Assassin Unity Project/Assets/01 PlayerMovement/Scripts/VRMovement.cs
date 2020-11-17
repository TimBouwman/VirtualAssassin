//author: Tim Bouwman
//Github: https://github.com/TimBouwman
using System.ComponentModel;
using UnityEngine;

/// <summary>
/// This class is resposeble for moving and rotating the player object.
/// it also makes sure the collider gets smaller when the player ducks down and keeps the collider underneath the plays head.
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class VRMovement : MonoBehaviour
{
    #region Variables
    [Header("Movement Value's")]
    [Tooltip("The speed with which the player moves")]
    [SerializeField] private float speed = 2;
    [ReadOnly][SerializeField]
    private Vector2 moveInput = Vector2.zero;
    [Tooltip("The amount the player rotates when using the snap rotation")]
    [SerializeField] private int snapIncrement = 45;
    [SerializeField] private float minimumTurnInput = 0.5f;
    [ReadOnly][SerializeField]
    private Vector2 turnInput = Vector2.zero;

    /// <summary> Velocity is the speed that gets build up while the plays is falling. if the player is grounded it get set to 0.0f, -2.0f, 0.0f. </summary>
    [ReadOnly][SerializeField]
    private Vector3 velocity;
    public readonly float test;

    [Header("Objects")]
    [SerializeField] private Transform head = null;
    public Transform Head { get { return this.head; } }
    private Transform lookIndex;
    private CharacterController characterController;
    #endregion

    #region Unity Methods
    private void Start()
    {
        Setup();
    }
    private void Update()
    {
        BodyHandler();
        MovementHandler();
        GravityHandler();
        SnapRotation();
    }
    #endregion

    #region Custom Methods
    private void Setup()
    {
        characterController = this.GetComponent<CharacterController>();

        //create look index
        lookIndex = new GameObject("Look Index").transform;
        lookIndex.position = head.position;
        lookIndex.rotation = head.rotation;
        lookIndex.parent = head.parent;
    }

    /// <summary>
    /// This method is resposeble for keeping the collider underneath the players head
    /// and making it smaller when the player ducks down.
    /// </summary>
    private void BodyHandler()
    {
        //Sets the hight of the collider to the hight of the players head with a clamp for 1 to 2
        float headHight = Mathf.Clamp(head.localPosition.y, 1, 2);
        characterController.height = headHight;

        //resets the center
        Vector3 center = Vector3.zero;
        center.y = characterController.height / 2;
        center.y += characterController.skinWidth;

        //keeps the collider underneath the players head
        center.x = head.localPosition.x;
        center.z = head.localPosition.z;

        characterController.center = center;
    }
    /// <summary>
    /// This method handels the movement of the player it does this reletive to the rotation of the head.
    /// </summary>
    private void MovementHandler()
    {
        Quaternion rotation = head.localRotation;
        rotation.x = 0;
        rotation.z = 0;
        lookIndex.localRotation = rotation;

        if (moveInput != Vector2.zero)
        {
            Vector3 move = head.right * moveInput.x + lookIndex.forward * moveInput.y;
            characterController.Move(move * speed * Time.deltaTime);
        }
    }
    /// <summary>
    /// Appleys gravity to the player object it uses this formula Δy = ½g·t² to calculate the increasing velocity when falling.
    /// </summary>
    private void GravityHandler()
    {
        //this if statment makes sure you stop exelorating when you on the ground
        if (characterController.isGrounded && velocity.y < 0f)
            velocity.y = -2f;
        else //increasing velocity while faling
            velocity += Physics.gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }
    /// <summary>
    /// Turns the player a certain amount to the left or to the right.
    /// when the if statement is true the player object rotates around the head object so that the player collider
    /// stays in the same position and does not clip through another object
    /// </summary>
    private void SnapRotation()
    {
        if (turnInput.x < -minimumTurnInput)
            this.transform.RotateAround(head.position, Vector3.up, -Mathf.Abs(snapIncrement));
        else if (turnInput.x > minimumTurnInput)
            this.transform.RotateAround(head.position, Vector3.up, Mathf.Abs(snapIncrement));
    }
    #endregion
}