using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class WebShooter : MonoBehaviour
{
    [SerializeField] GameObject WebProjectilePrefab;
    public  float webShotSpeed = 10f;
    GameObject currentProjectile;

    public LineRenderer ropeRenderer;
    public DistanceJoint2D ropeJoint;

    public bool ropeAttached = false;
    private Vector2 ropeAnchor;
    Vector2 WebDir;

    [SerializeField] float swingSpeed = 1f;
    [SerializeField] float MaxSwingVel = 15f;
    [SerializeField] float LaunchAddY = 1.5f;
    [SerializeField] float LaunchAddX = 1.5f;

    private Vector2 _swingInput;


    public void Swing(Vector2 moveInput)
    {
        _swingInput = moveInput;
    }
    //
    void DoSwing()
    {
        if (GetComponent<Rigidbody2D>().velocity.x < 0 && _swingInput.x < 0)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Clamp(GetComponent<Rigidbody2D>().velocity.x - (swingSpeed * Time.deltaTime), -1 * MaxSwingVel, MaxSwingVel), GetComponent<Rigidbody2D>().velocity.y);
        }
        else if (GetComponent<Rigidbody2D>().velocity.x > 0 && _swingInput.x > 0)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Clamp(GetComponent<Rigidbody2D>().velocity.x + (swingSpeed * Time.deltaTime), - 1 * MaxSwingVel, MaxSwingVel), GetComponent<Rigidbody2D>().velocity.y);
        }
    }
    public void ReleaseWeb()
    {
        ResetRope();
    }

    public void FireWeb()
    {
        WebResourceController.Instance.DecrementWebCount(1);
        currentProjectile = Instantiate(WebProjectilePrefab, transform.position + .1f * Vector3.up, Quaternion.identity);
        currentProjectile.GetComponent<Rigidbody2D>().velocity = WebDir * webShotSpeed;
    }
    
    public void ConnectWeb(Vector3 pos)
    {
        currentProjectile = null;
        ConnectRope(pos);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 WorldMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        WebDir =  Vector3.Normalize(WorldMousePos - transform.position);
        ropeRenderer.SetPosition(0, transform.position);
        if (currentProjectile != null)
        {
            ropeRenderer.SetPosition(1, ropeAnchor);
        }
    }

    bool locked = false;
    //
    private void FixedUpdate()
    {
        if (ropeJoint.enabled)
        {
            DoSwing();
        }
        if (ropeAttached && transform.position.y > ropeAnchor.y && !locked)
        {
            if (ropeJoint.enabled)
            {
                ropeJoint.enabled = false;
                GetComponent<PlayerMovement>().EnableMoveInputs();
            }
        }
        else if (ropeAttached && GetComponent<Rigidbody2D>().velocity.y < -.1f && !ropeJoint.enabled)
        {
            ropeJoint.enabled = true;
            ropeJoint.connectedAnchor = ropeAnchor;
            ropeJoint.distance = Vector2.Distance(transform.position, ropeAnchor);
            GetComponent<PlayerMovement>().DisableMoveInputs();
            locked = true;
        }
        else if (ropeAttached && (GetComponent<PlayerMovement>().touchingGround || GetComponent<PlayerMovement>().touchingWall) && ropeJoint.enabled)
        {
            locked = false;
            ropeJoint.enabled = false;
            GetComponent<PlayerMovement>().EnableMoveInputs();
        }
    }
 

    private void ConnectRope(Vector2 pos)
    {
        ropeRenderer.enabled = true;
        ropeRenderer.positionCount = 2;
        ropeRenderer.SetPosition(0, transform.position);
        ropeRenderer.SetPosition(1, pos);
        ropeAttached = true;
        ropeAnchor = pos;
    }

    private void ResetRope()
    {
        if (currentProjectile != null && currentProjectile.GetComponent<WebProjectile>().attachedToSpider) { currentProjectile.GetComponent<WebProjectile>().Detach(); }
        GetComponent<PlayerMovement>().EnableMoveInputs();
        ropeJoint.enabled = false;
        ropeAttached = false;
        ropeRenderer.enabled = false;
        if (!GetComponent<PlayerMovement>().touchingGround)
        {
            GetComponent<PlayerMovement>().ReleaseFromSwing();
            float y = GetComponent<Rigidbody2D>().velocity.y;
            float x = GetComponent<Rigidbody2D>().velocity.x;
            if (GetComponent<Rigidbody2D>().velocity.y > 0)
            {
                y += LaunchAddY;
            }
            if (GetComponent<Rigidbody2D>().velocity.x > 1)
            {
                x += LaunchAddX;
            }
            else if (GetComponent<Rigidbody2D>().velocity.x < -1)
            {
                x -= LaunchAddX;
            }
            GetComponent<Rigidbody2D>().velocity = new Vector2(x, y);
        }
        //GetComponent<PlayerMovement>().DoJump();
    }

}
