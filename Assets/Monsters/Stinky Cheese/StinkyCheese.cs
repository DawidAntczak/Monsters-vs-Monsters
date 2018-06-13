using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StinkyCheese : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Movable attacker = collision.GetComponent<Movable>();
        if(!attacker)
            { return; }

        attacker.ChangeToNeighborLane();
    }
}
