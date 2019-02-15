using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPotion : MonoBehaviour
{
    #region speedpack_variables
    [SerializeField]
    [Tooltip("Assign the speeding value of the speed potion")]
    private float speedAmount;
    [SerializeField]
    [Tooltip("Assign the boost timer value of the speed potion")]
    private float time;
    #endregion

    #region functions
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            collision.transform.GetComponent<PlayerController>().Speed(speedAmount, time);
            Destroy(this.gameObject);
        }
    }
    #endregion
}
