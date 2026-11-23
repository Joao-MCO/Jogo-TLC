using System.Collections;
using UnityEngine;

// Os nomes devem bater com os estados do Animator.
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
    private Animator animator;
    private bool isCollected;

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.Play(collectable.ToString());
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (isCollected || !collision.gameObject.CompareTag("Player")) return;
        if (!collision.gameObject.TryGetComponent(out Player player)) return;

        isCollected = true;
        GetComponent<Collider2D>().enabled = false;
        player.collectables += 1;
        StartCoroutine(Collected());
    }

    IEnumerator Collected()
    {
        animator.Play("Collected");
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
