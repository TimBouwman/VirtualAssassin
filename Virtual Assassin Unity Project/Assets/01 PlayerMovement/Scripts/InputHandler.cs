//author: Tim Bouwman
//Github: https://github.com/TimBouwman
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

/// <summary>
/// 
/// </summary>
public class InputHandler : MonoBehaviour
{
    #region Variables
    [SerializeField] private VRLocomotion movement = null;
    [SerializeField] private VRTurning turning = null;
    [SerializeField] private VRParkour parkour = null;
    [SerializeField] private InputDeviceCharacteristics leftController = InputDeviceCharacteristics.None, rightController = InputDeviceCharacteristics.None;
    private InputDevice leftInputDevice, rightInputDevice;
    #endregion

    #region Unity Methods
    private void Update()
    {
        if(!leftInputDevice.isValid && !rightInputDevice.isValid)
            TrySetup(); 
        else
            GetInput();
    }
    #endregion

    #region Custom Methods
    private void TrySetup()
    {
        List<InputDevice> leftControllers = new List<InputDevice>(), rightControllers = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(leftController | InputDeviceCharacteristics.Controller, leftControllers);
        InputDevices.GetDevicesWithCharacteristics(rightController | InputDeviceCharacteristics.Controller, rightControllers);

        if (leftControllers.Count > 0)
            leftInputDevice = leftControllers[0];
       if (rightControllers.Count > 0)
            rightInputDevice = rightControllers[0];
    }

    private void GetInput()
    {
        leftInputDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 moveInput);
        movement.MoveInput = moveInput;
        parkour.MoveInput = moveInput;
        rightInputDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 turnInput);
        turning.TurnInput = turnInput;
    }
    #endregion
}