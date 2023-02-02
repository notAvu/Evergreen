using System.Collections;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public static GameObject activePlatform;
    [SerializeField]
    private float lifeSpan = 8f;
    [SerializeField]
    private GameObject origin;

    [SerializeField]
    private float growthDuration = 2f;

    private void Start()
    {

        if (activePlatform != null)
        {
            Destroy(activePlatform);
        }
        activePlatform = gameObject;
        StartCoroutine(nameof(DestructionTimer));
        StartCoroutine(Grow());
    }
    /// <summary>
    /// Corutina que hace que el tamaño de la plataforma cambie en 
    /// </summary>
    /// <returns></returns>
    private IEnumerator Grow()
    {
        Vector3 offset = transform.position - origin.transform.position;
        Vector2 originalScale = new Vector2(0.01f, transform.localScale.y);
        Vector2 destinationScale = new Vector2(1.0f , transform.localScale.y);

        float currentTime = 0.0f;
        transform.position -= offset;

        do
        {
            transform.localScale = Vector2.Lerp(originalScale, destinationScale, currentTime / growthDuration);
            currentTime += Time.deltaTime;
            yield return null;
        } while (currentTime <= growthDuration);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Vines"))
        {
            Destroy(gameObject);
        }
    }
    /// <summary>
    /// Empieza un contador que tras <see cref="lifeSpan"/> segundos destruye la plataforma
    /// </summary>
    /// <returns></returns>
    private IEnumerator DestructionTimer()
    {
        yield return new WaitForSeconds(lifeSpan);
        Destroy(gameObject);
    }
}
