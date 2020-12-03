//author: Tim Bouwman
//Github: https://github.com/TimBouwman
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// This class is resposeble for moving the VR rig and it also makes sure the collider gets smaller when the player ducks down and
/// keeps the collider underneath the plays head.
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class VRLocomotion : MonoBehaviour
{
    #region Variables
    [SerializeField]
    [Tooltip("The speed with which the player moves")]
    private float speed = 2;

    [SerializeField]
    [ReadOnly]
    [Tooltip("The input must be set by a external inputhandler")]
    private Vector2 moveInput = Vector2.zero;
    public Vector2 MoveInput { set { this.moveInput = value; } }

    [Header("Objects")]
    [SerializeField] private Transform head = null;
    [SerializeField] private Transform forwardMovement = null;
    private Transform lookIndex;
    public Transform LookIndex { get { return this.lookIndex; } }
    private CharacterController characterController;

    [Header("Vignette")]
    [SerializeField] private bool comfort = false;
    [SerializeField] private Volume comfortVignette = null;
    #endregion

    #region Unity Methods
    private void Start()
    {
        Setup();
    }
    private void Update()
    {
        Move();
        BodyHandler();
        if (comfort) Comfort();
    }
    #endregion

    #region Custom Methods
    private void Setup()
    {
        characterController = this.GetComponent<CharacterController>();

        if (!forwardMovement)
            forwardMovement = head;

        //create look index
        lookIndex = new GameObject("Look Index").transform;
        lookIndex.position = forwardMovement.position;
        lookIndex.rotation = forwardMovement.rotation;
        lookIndex.parent = forwardMovement.parent;
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
    private void Move()
    {
        Quaternion rotation = forwardMovement.localRotation;
        rotation.x = 0;
        rotation.z = 0;
        lookIndex.localRotation = rotation;

        if (moveInput != Vector2.zero)
        {
            Vector3 move = forwardMovement.right * moveInput.x + lookIndex.forward * moveInput.y;
            characterController.Move(move * speed * Time.deltaTime);
        }
    }
    private void Comfort()
    {
        if(moveInput != Vector2.zero)
            comfortVignette.weight = Mathf.Abs(moveInput.x) + Mathf.Abs(moveInput.y);
    }
    #endregion
}