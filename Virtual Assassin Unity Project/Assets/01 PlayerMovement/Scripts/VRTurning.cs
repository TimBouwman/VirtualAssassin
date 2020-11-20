//author: Tim Bouwman
//Github: https://github.com/TimBouwman
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class VRTurning : MonoBehaviour
{
    #region Variables
    [SerializeField] private Transform head = null;
    
    [SerializeField]
    [Tooltip("The speed the player rotates with when using smooth turn")]
    private TurnType turnType = TurnType.None;
    private enum TurnType { Snap,Smooth,None };

    [ReadOnly]
    [SerializeField]
    [Tooltip("The input must be set by a external inputhandler")]
    private Vector2 turnInput = Vector2.zero;
    public Vector2 TurnInput { set { this.turnInput = value; } }

    [Header("Snap Turn")]
    [SerializeField]
    [Tooltip("The amount the player rotates when using snap turn")]
    private int snapIncrement = 45;

    [SerializeField]
    [Tooltip("The minimum amount if input needed for the snap turn to activate")]
    private float minimumTurnInput = 0.5f;
    
    [Header("Smooth Turn")]
    [SerializeField]
    [Tooltip("The speed the player rotates with when using smooth turn")]
    private float smoothTurnSpeed = 5f;
    #endregion

    #region Unity Methods
    private void Update()
    {
        switch (turnType)
        {
            case TurnType.Snap:
                SnapTurn();
                break;
            case TurnType.Smooth:
                SmoothTurn();
                break;
            case TurnType.None:
                break;
            default:
                SnapTurn();
                break;
        }
    }
    #endregion

    #region Custom Methods
    /// <summary>
    /// Turns the player a certain amount to the left or to the right.
    /// when the if statement is true the player object rotates around the head object so that the player collider
    /// stays in the same position and does not clip through another object
    /// </summary>
    private void SnapTurn()
    {
        if(turnInput.x < -minimumTurnInput)
            this.transform.RotateAround(head.position, Vector3.up, -Mathf.Abs(snapIncrement));
        else if(turnInput.x > minimumTurnInput)
            this.transform.RotateAround(head.position, Vector3.up, Mathf.Abs(snapIncrement));
    }
    private void SmoothTurn()
    {

    }
    #endregion
}