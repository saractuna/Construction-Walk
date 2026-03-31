using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    //Components
    Rigidbody2D rb;
    SpriteRenderer sr;
    CapsuleCollider2D cc;
    BoxCollider2D bc;

    [Header("Movement")]
    [SerializeField] float enemySpeed = 1f;
    [SerializeField] bool movingRight = true;
    [SerializeField] LayerMask groundLayerMask;

    [Header("Debug Spheres")]
    [SerializeField] float sphereRadius = 0.2f;
    [SerializeField] float faceXadjustment = 0.2f;
    [SerializeField] float platformXadjustment = 0.2f;
    [SerializeField] float platformYadjustment = 0.2f;
    [SerializeField] float faceYadjustment = 0.2f;
    [SerializeField] Color leftPlatformColor, rightPlatformColor, leftFaceColor, rightFaceColor = Color.white;
    Vector3 leftPlatformCheck, rightPlatformCheck, leftFaceCheck, rightFaceCheck;

    [SerializeField] float collisionTimer = 0.2f;
    float tempCollisionTimer = 1f;



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        cc = GetComponent<CapsuleCollider2D>();
        bc = GetComponent<BoxCollider2D>();

        TurnAround();

        tempCollisionTimer = collisionTimer;
    }


    void Update()
    {       
        DefineCollisionCheckCoords();

        tempCollisionTimer -= Time.deltaTime;

        if (tempCollisionTimer < 0)
        {
            MakeCollisionChecks();
            tempCollisionTimer = collisionTimer;
        }
    }

    void SetEnemySpeed()
    {
        if (!movingRight)
        {
            enemySpeed = Mathf.Abs(enemySpeed);
        }
        else
        {
            enemySpeed = -Mathf.Abs(enemySpeed);
        }
    }

    void DefineCollisionCheckCoords()
    {
        // Platform check positions at the bottom-left and bottom-right of the collider
        Vector3 baseYPosition = new Vector3(0, -bc.size.y / 2 + platformYadjustment); // Adjustable Y position for platform checks
        Vector3 platformXOffset = new Vector3(platformXadjustment, 0); // Adjustable X offset for platform checks

        leftPlatformCheck = transform.position + baseYPosition - new Vector3(bc.size.x / 2, 0) - platformXOffset;
        rightPlatformCheck = transform.position + baseYPosition + new Vector3(bc.size.x / 2, 0) + platformXOffset;

        // Face check positions at the middle left and middle right of the collider
        Vector3 middleYPosition = new Vector3(0, bc.offset.y + faceYadjustment); // Adjustable Y position for face checks
        Vector3 faceXOffset = new Vector3(faceXadjustment, 0); // Adjustable X offset for face checks

        leftFaceCheck = transform.position + middleYPosition - new Vector3(bc.size.x / 2, 0) - faceXOffset;
        rightFaceCheck = transform.position + middleYPosition + new Vector3(bc.size.x / 2, 0) + faceXOffset;
    }

    bool CheckWall(Vector3 leftSide, Vector3 rightSide, LayerMask mask)
    {
        if (Physics2D.OverlapCircle(leftSide, sphereRadius, mask) || (Physics2D.OverlapCircle(rightSide, sphereRadius, mask)))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    bool CheckPlatform(Vector3 leftSide, Vector3 rightSide, LayerMask mask)
    {
        if (Physics2D.OverlapCircle(leftSide, sphereRadius, mask) && (Physics2D.OverlapCircle(rightSide, sphereRadius, mask)))
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    void TurnAround()
    {
        movingRight = !movingRight;

        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;

        SetEnemySpeed();
    }

    void MakeCollisionChecks()
    {
        if (CheckWall(leftFaceCheck, rightFaceCheck, groundLayerMask) || CheckPlatform(leftPlatformCheck, rightPlatformCheck, groundLayerMask))
        {
            TurnAround();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = leftPlatformColor;
        Gizmos.DrawWireSphere(leftPlatformCheck, sphereRadius);

        Gizmos.color = rightPlatformColor;
        Gizmos.DrawWireSphere(rightPlatformCheck, sphereRadius);

        Gizmos.color = leftFaceColor;
        Gizmos.DrawWireSphere(leftFaceCheck, sphereRadius);

        Gizmos.color = rightFaceColor;
        Gizmos.DrawWireSphere(rightFaceCheck, sphereRadius);
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector3(enemySpeed, rb.velocity.y, 0);
    }
}
