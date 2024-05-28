using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Fruit : MonoBehaviour
{
    public GameObject[] fallingObjects; // Düşecek nesnelerin prefabları
    public float spawnInterval = 3f; // Nesnelerin düşme aralığı
    
    private void Start()
    {
        InvokeRepeating("SpawnObject", spawnInterval, spawnInterval);
   
    }

    void SpawnObject()
    {
        // Ekranın genişliğine göre rastgele bir pozisyon seç
        float randomX = Random.Range(-8f, 8f);
        Vector3 spawnPosition = new Vector3(randomX, 6f, 0);

        // Rastgele bir nesne prefabı seç
        if (fallingObjects.Length > 0)
        {
            int randomIndex = Random.Range(0, fallingObjects.Length);
          
            Instantiate(fallingObjects[randomIndex], spawnPosition, Quaternion.identity);
        }
    }
    // Karakter öldüğünde bu metot çağrılır
    

}