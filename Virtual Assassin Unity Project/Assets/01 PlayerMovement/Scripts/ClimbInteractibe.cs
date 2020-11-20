//author: Tim Bouwman
//Github: https://github.com/TimBouwman
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    protected virtual void Grab(XRBaseInteractor interactor)
    {
        
    }

    protected virtual void Drop(XRBaseInteractor interactor)
    {

    }
}