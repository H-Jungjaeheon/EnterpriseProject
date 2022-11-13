using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public int multiplication;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PerfectZone"))
        {
            multiplication = 3;
        }
        else if (collision.gameObject.CompareTag("SuccessZone"))
        {
            multiplication = 2;
        }
        else
        {
            multiplication = -1;
        }
    }
}
