using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonMushroom : Mushroom
{
    public System.Action onPoisoningPlayer;
    public override void Eaten()
    {
        base.Eaten();
        this.onPoisoningPlayer();
    }
}
