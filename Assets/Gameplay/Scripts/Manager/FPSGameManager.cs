using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSGameManager : MonoBehaviour
{
    public static FPSGameManager instance;

    private void Awake()
    {
        instance = this;
    }
}
