using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed = 2f;

    private Vector2 target;
    private bool alive = true;

    void Start()
    {
        if (pointA == null || pointB == null)
        {
            pointA = new GameObject("A").transform;
            pointB = new GameObject("B").transform;
            pointA.position = transform.position + Vector3.left * 2f;
            pointB.position = transform.position + Vector3.right * 2f;
        }
        target = pointB.position;
    }

    void Update()
    {
        if (!alive) return;

        var newPos = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
        transform.position = newPos;
        if (Vector2.Distance(transform.position, target) < 0.05f)
        {
            target = (target == (Vector2)pointA.position) ? (Vector2)pointB.position : (Vector2)pointA.position;
        }
    }

    public void OnStomped()
    {
        alive = false;
        var rend = GetComponent<SpriteRenderer>();
        if (rend != null) rend.enabled = false;
        var col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;
        Destroy(gameObject, 0.8f);
    }
    public void InitializePatrol(Vector2 a, Vector2 b)
    {
        if (pointA == null) pointA = new GameObject(name + "_A").transform;
        if (pointB == null) pointB = new GameObject(name + "_B").transform;

        pointA.position = a;
        pointB.position = b;
        target = pointB.position;
    }
}
