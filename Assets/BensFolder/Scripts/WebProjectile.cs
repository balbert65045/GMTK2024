using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WebProjectile : MonoBehaviour
{
    [SerializeField] GameObject WebEnd;
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] float maxWebDistance = 10f;
    WebShooter webShooter;
    public bool attachedToSpider = true;
    GameObject webEnd;
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (attachedToSpider)
        {
            webShooter.ConnectWeb(collision.contacts[0].point);
            if (webEnd != null) { Destroy(webEnd.gameObject); }
            Destroy(this.gameObject);
            return;
        }
        GetComponent<Rigidbody2D>().gravityScale = 0;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        StartCoroutine("DestroyAfterDelay");
        //Destroy(this.gameObject);
        //if (webEnd != null) { Destroy(webEnd.gameObject); }
    }

    IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(1f);
        if (webEnd != null ) { Destroy(webEnd.gameObject); }
        Destroy(this.gameObject);
    }
    

    private void Start()
    {
        webShooter = FindObjectOfType<WebShooter>();
    }

    public void Detach()
    {
        attachedToSpider = false;
        webEnd = Instantiate(WebEnd, webShooter.transform.position, Quaternion.identity);
        Vector2 dir = Vector3.Normalize(transform.position - webShooter.transform.position);
        webEnd.GetComponent<Rigidbody2D>().velocity = dir * GetComponent<Rigidbody2D>().velocity.magnitude;
    }


    private void Update()
    {
        if (attachedToSpider)
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, webShooter.transform.position);
            float distance = Vector3.Magnitude(transform.position - webShooter.transform.position);
            if (distance > maxWebDistance) { 
                Detach();
            }
        }
        else
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, webEnd.transform.position);
        }
    }
}