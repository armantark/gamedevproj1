using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    #region gameObj_variables
    [SerializeField]
    [Tooltip("HealthPack")]
    public GameObject healthPack;
    #endregion

    #region helper_functions
    IEnumerator DeleteChest()
    {
        yield return new WaitForSeconds(0.3f);
        Instantiate(healthPack, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    public void Interact()
    {
        StartCoroutine(DeleteChest());
    }
    #endregion
}
