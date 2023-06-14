using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : Hazard
{
    public void OnCollisionEnter2D(Collision2D other)
    {
        GameplayInterfaceManager.Instance.AddCoin(1);
    }
}
