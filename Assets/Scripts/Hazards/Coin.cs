using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : Hazard
{
    public void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("HIT COIN!");

        InterfaceManager.Instance.AddCoin(1);
    }
}
