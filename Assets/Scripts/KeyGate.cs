﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyGate : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.name == "Player" && GameVariables.keyCount>0)
        {
            GameVariables.keyCount --;
            Destroy (gameObject);
        }
    }
}
