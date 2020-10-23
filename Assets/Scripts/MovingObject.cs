using System.Collections;
using UnityEngine;

public abstract class MovingObject : MonoBehaviour
{
    protected BoxCollider2D boxCollider;
    protected Rigidbody2D rb2D;

    protected Vector2 movementDirection;
    [SerializeField]protected float movementSpeed = 10;


    // Start is called before the first frame update
    protected virtual void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
    }

    protected void Move()
    {
        rb2D.velocity = movementDirection * movementSpeed;
    }

    //protected bool Move(int xDir, int yDir, out RaycastHit2D hit)
    //{
    //    Vector2 start = transform.position;
    //    Vector2 end = start + new Vector2(xDir, yDir);

    //    boxCollider.enabled = false;
    //    hit = Physics2D.Linecast(start, end, blockingLayer);
    //    boxCollider.enabled = true;

    //    if (hit.transform == null && !isMoving)
    //    {
    //        StartCoroutine(SmoothMovement(end));
    //        return true;
    //    }
    //    return false;
    //}

    //protected IEnumerator SmoothMovement(Vector3 end)
    //{
    //    isMoving = true;
    //    float sqRemainingDistance = (transform.position - end).sqrMagnitude;
    //    while (sqRemainingDistance > float.Epsilon)
    //    {
    //        Vector3 newPosition = Vector3.MoveTowards(rb2D.position, end, inverseMoveTime * Time.deltaTime);
    //        rb2D.MovePosition(newPosition);
    //        sqRemainingDistance = (transform.position - end).sqrMagnitude;
    //        yield return null;
    //    }
    //    rb2D.MovePosition(end);
    //    isMoving = false;
    //}

    //protected virtual void AttemptMove<T>(int xDir , int yDir)
    //     where T : Component
    //{
    //    RaycastHit2D hit;
    //    bool canMove = Move(xDir, yDir, out hit);
    //    if (hit.transform == null)
    //        return;

    //    T hitComponent = hit.transform.GetComponent<T>();

    //    if (!canMove && hitComponent != null)
    //    {
    //        OnCantMove(hitComponent);
    //    }
    //}

    //protected abstract void OnCantMove<T>(T component)
    //   where T : Component;


}
