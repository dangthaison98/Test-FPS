using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopControl : SingletonSingleScene<LoopControl>
{
    public Action somethingUpdate;
    
    private void Update()
    {
        somethingUpdate?.Invoke();
    }
}
