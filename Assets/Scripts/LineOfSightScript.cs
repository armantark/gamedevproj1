using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineOfSightScript : MonoBehaviour
{
    //called when something enters the trigger collider
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GetComponentInParent<Enemy>().player = collision.transform;
            Debug.Log("See player run at player");
        }
    }
}
