using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCone : MonoBehaviour
{
    public Turret turret;

    public bool isLeft = false;
    private void Awake()
    {
        turret = gameObject.GetComponentInParent<Turret>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Player"))
        {
            if (isLeft)
            {
                turret.Attack(false);
            }
            else
            {
                turret.Attack(true);
            }
        }   
    }
}
