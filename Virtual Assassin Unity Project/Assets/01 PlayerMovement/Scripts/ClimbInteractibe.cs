//author: Tim Bouwman
//Github: https://github.com/TimBouwman
using UnityEngine.XR.Interaction.Toolkit;

public class ClimbInteractibe : XRBaseInteractable
{
    protected override void Awake()
    {
        base.Awake();
        onSelectEntered.AddListener(Grab);
        onSelectExited.AddListener(Drop);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        onSelectEntered.RemoveListener(Grab);
        onSelectExited.RemoveListener(Drop);
    }

    private void Grab(XRBaseInteractor interactor)
    {
        VRParkour.climbingHand = interactor.GetComponent<XRController>();
    }

    private void Drop(XRBaseInteractor interactor)
    {
        if (VRParkour.climbingHand && VRParkour.climbingHand.name == interactor.name)
        {
            VRParkour.climbingHand = null;
        }
    }
}