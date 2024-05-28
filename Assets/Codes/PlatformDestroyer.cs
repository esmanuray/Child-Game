using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlatformDestroyer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {

        // Sağlıklı veya sağlıksız fark etmeksizin platforma dokunan tüm nesneleri yok et
        if (collision.CompareTag("healthy") || collision.CompareTag("unhealthy"))
        {
            Destroy(collision.gameObject);
        }
        else
        {
        }
    }
}
