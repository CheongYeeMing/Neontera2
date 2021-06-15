using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Animation
{
    public void Start();
    public void ChangeAnimationState(string newState);
}
