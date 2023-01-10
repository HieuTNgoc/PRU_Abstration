using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTri : Enemy
{
    public override void TakeDamage(int damage)
    {
        Debug.Log("Tam giắc dính đạn");
    }

}
