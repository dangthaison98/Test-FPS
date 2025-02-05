using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopControl : MonoBehaviour
{
    public static LoopControl instance;
    
    public Action somethingUpdate;

    private void Awake()
    {
        instance = this;
    }
    
    private void Update()
    {
        somethingUpdate?.Invoke();
    }
}
