using System.Collections;
using UnityEngine;

public enum CollectableType
{
    Apple,
    Banana,
    Cherry,
    Kiwi,
    Melon,
    Orange,
    Pineapple,
    Strawberry
}

public class Collectable : MonoBehaviour
{
    public CollectableType collectable;
    private string[] collectableName = new string[] { "Apple", "Banana", "Cherry", "Kiwi", "Melon", "Orange", "Pineapple", "Strawberry" };
    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.Play(collectableName[(int) collectable]);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Player player = collision.gameObject.GetComponent<Player>();
            player.collectables += 1;
            StartCoroutine(collected());
        }
    }

    IEnumerator collected()
    {
        animator.Play("Collected");
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
