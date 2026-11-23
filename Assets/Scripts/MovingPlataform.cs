using UnityEngine;

public class MovingPlataform : MonoBehaviour
{
    public float speed = 2f;
    public float waitSeconds = 1f;
    public Transform[] points;

    private int i;
    private float waitUntil;

    void Start()
    {
        transform.position = points[0].position;
    }

    void Update()
    {
        if (Time.time < waitUntil) return;

        if (Vector2.Distance(transform.position, points[i].position) < 0.01f)
        {
            transform.position = points[i].position;
            waitUntil = Time.time + waitSeconds;
            i = (i + 1) % points.Length;
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, points[i].position, speed * Time.deltaTime);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}
