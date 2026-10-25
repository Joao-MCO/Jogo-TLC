using UnityEngine;
using System.Collections;

public class MovingPlataform : MonoBehaviour
{
    public float speed = 2f;
    public float waitSeconds = 1f;
    public Transform[] points;

    private int i;
    void Start()
    {
        transform.position = points[0].position;
    }
    void Update()
    {
        if(Vector2.Distance(transform.position, points[i].position) < 0.01f)
        {
            StartCoroutine(StandCoroutine());
            i++;
            if(i == points.Length)
            {
                i = 0;
            }
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, points[i].position, speed * Time.deltaTime);
        }
    }

    private IEnumerator StandCoroutine()
    {
        transform.position = points[i].position;
        yield return new WaitForSeconds(waitSeconds);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.transform.SetParent(transform);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.transform.SetParent(null);
        }
    }
}
