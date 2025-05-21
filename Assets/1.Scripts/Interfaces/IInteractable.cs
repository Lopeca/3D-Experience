using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public string Prompt { get; }
    public void OnTargeted();
    public void OnTargetLost();
    public void Interact();
}
