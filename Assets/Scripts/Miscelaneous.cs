using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Miscelaneous : MonoBehaviour
{
    public bool endpoint;
    private bool isTriggered;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isTriggered)
        {
            StartCoroutine(Trigger());
        }
    }

    IEnumerator Trigger()
    {
        animator.Play("Triggered");
        isTriggered = true;
        yield return new WaitForSeconds(1f);
        if (endpoint) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
