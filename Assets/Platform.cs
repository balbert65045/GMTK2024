using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] float slipperiness = 0f;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.GetComponent<PlayerMovement>() != null)
        {
            //Apply New friction
            collision.transform.GetComponent<PlayerMovement>().SetNewSliperiness(slipperiness);
            Debug.Log("applying new friction");
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.GetComponent<PlayerMovement>() != null)
        {
            //Unnapply New friction
            collision.transform.GetComponent<PlayerMovement>().SetNewSliperiness(0);

            Debug.Log("unapplying new friction");

        }
    }
}
