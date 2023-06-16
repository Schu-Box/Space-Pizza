using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDisplay : MonoBehaviour
{
    public float duration = 3f;

    public SpriteRenderer spriteRenderer;

    private void Awake()
    {
        StartCoroutine(DamageCoroutine());
    }

    private IEnumerator DamageCoroutine()
    {
        float timer = 0f;
        WaitForFixedUpdate waiter = new WaitForFixedUpdate();
        while (timer <= duration)
        {
            timer += Time.deltaTime;

            spriteRenderer.color = Color.Lerp(Color.white, Color.clear, timer / duration);
            
            yield return waiter;
        }

        Destroy(gameObject);
    }
}
