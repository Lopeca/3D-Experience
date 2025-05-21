using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public void OnTargeted();
    public void OnTargetLost();
    public void Interact();
}
