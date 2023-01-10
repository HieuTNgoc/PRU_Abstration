using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySqu : Enemy
{
    public override void TakeDamage(int damage)
    {
        Debug.Log("Square ăn đạn");
    }

}
