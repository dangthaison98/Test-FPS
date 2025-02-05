using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Vector3 offset;
    
    private void Start()
    {
        LoopControl.instance.somethingUpdate += Follow;
    }

    private void Follow()
    {
        
    }
}
