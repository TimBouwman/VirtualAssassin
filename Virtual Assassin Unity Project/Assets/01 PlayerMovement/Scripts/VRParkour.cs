//author: Tim Bouwman
//Github: https://github.com/TimBouwman
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// 
/// </summary>
public class VRParkour : MonoBehaviour
{
    #region Variables
    /// <summary> Velocity is the speed that gets build up while the plays is falling. if the player is grounded it get set to 0.0f, -2.0f, 0.0f. </summary>
    [ReadOnly]
    [SerializeField]
    private Vector3 velocity;
    private CharacterController characterController;
    public static XRController climbingHand;
    private VRLocomotion locomotion;

    #endregion

    #region Unity Methods
    private void Start()
    {
        characterController = this.GetComponent<CharacterController>();
        locomotion = this.GetComponent<VRLocomotion>();
    }

    private void FixedUpdate()
    {
        if (climbingHand)
        {
            Climbing();
            locomotion.enabled = false;
        }
        else
        {
            Gravity();
            locomotion.enabled = true;
        }
    }
    #endregion

    #region Custom Methods
    /// <summary>
    /// Appleys gravity to the player object it uses this formula Δy = ½g·t² to calculate the increasing velocity when falling.
    /// </summary>
    private void Gravity()
    {
        //this if statment makes sure you stop exelorating when you on the ground
        if(characterController.isGrounded && velocity.y < 0f)
            velocity.y = -2f;
        else //increasing velocity while faling
            velocity += UnityEngine.Physics.gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }
    private void Climbing()
    {
        InputDevices.GetDeviceAtXRNode(climbingHand.controllerNode).TryGetFeatureValue(CommonUsages.deviceVelocity, out Vector3 velocity);
        characterController.Move(-velocity * Time.fixedDeltaTime);
    }
    #endregion
}