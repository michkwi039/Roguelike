using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovingObject : MonoBehaviour {
    public float moveTime = 0.1f;          
    public LayerMask blockingLayer;         


    private BoxCollider2D boxCollider;      
    private Rigidbody2D rb2D;               
    private float inverseMoveTime;          
        // Use this for initialization
    protected virtual void Start() {//pobieranie komponetnów z których składa się postać
        boxCollider = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
        inverseMoveTime = 1f / moveTime;
    }

    protected bool Move(int xDir, int yDir, out RaycastHit2D hit)
    {
        Vector2 start = transform.position;//pozycja z której zaczyna się ruch
        Vector2 end = start + new Vector2(xDir, yDir);//pozycja na której kończy się ruch
        boxCollider.enabled = false;
        hit = Physics2D.Linecast(start, end, blockingLayer);//sprawdzanie czy nic nie stoi na przeszkodzie
        boxCollider.enabled = true;

        if (hit.transform == null)//jesli nic nie stoi na przeszkodzie wykonaj ruch
        {
            StartCoroutine(SmoothMovement(end));
            return true;
        }

        return false;
    }

    protected IEnumerator SmoothMovement(Vector3 end)//metoda dzięki której postać płynnie się przemieszcza
    {
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;
        while (sqrRemainingDistance > float.Epsilon)//wykonuj tak długo dopóki odległość do celu nie jest bliska zeru
        {
            
            Vector3 newPostion = Vector3.MoveTowards(rb2D.position, end, inverseMoveTime * Time.deltaTime);

            rb2D.MovePosition(newPostion);

            sqrRemainingDistance = (transform.position - end).sqrMagnitude;//modyfikacja pozostałej odległości

            yield return null;
        }
    }
    protected virtual void AttemptMove<T>(int xDir,int yDir)
        where T : Component
    {
        RaycastHit2D hit;
        bool canMove = Move(xDir, yDir, out hit);//sprawdzanie czy można się poruszyć

        if (hit.transform ==null)
        {
            return;
        }
        T hitComponent = hit.transform.GetComponent<T>();
        if (!canMove && hitComponent != null)
            OnCantMove(hitComponent);
    }
    protected abstract void OnCantMove<T>(T component)
        where T : Component;
}
