using System.Collections;
using UnityEngine;

public class Miscelaneous : MonoBehaviour
{
    public bool endpoint;
    private bool checkered;
    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
        checkered = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" && !checkered)
        {
            StartCoroutine(action());
        }
    }

    IEnumerator action()
    {
        animator.Play("Triggered");
        checkered = true;
        yield return new WaitForSeconds(1f);
        if(endpoint) UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
    }
}
