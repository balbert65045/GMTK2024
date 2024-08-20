using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebTail : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        GetComponent<Rigidbody2D>().gravityScale = 0;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

}