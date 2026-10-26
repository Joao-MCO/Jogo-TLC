using UnityEngine;

public class MiscelaneousScript : MonoBehaviour
{
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
            animator.Play("Triggered");
            checkered = true;
        }
    }
}
