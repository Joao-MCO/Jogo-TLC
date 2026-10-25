using System.Collections;
using UnityEngine;

public abstract class BaseBehaviour : MonoBehaviour
{
    public IEnumerator WaitCoroutine(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }
}