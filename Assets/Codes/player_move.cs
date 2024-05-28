using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class player_move : MonoBehaviour
{
    Rigidbody2D rb;
    float horizontal;
    public float speed;
    public float jumpForce;
    private bool isGrounded;
    public Transform groundCheck;
    public LayerMask whatIsGround;
    public float checkRadius;
    private int extraJump;
    bool lookright = true;
    public int maxExtraJumps = 2;
    Vector3 scale;
    Animator ani;
    private int puan = 0; // Puan değişkeni
    public TextMeshProUGUI puanText; // TMPro TextMeshProUGUI bileşeni

    public float deadAnimationDuration = 1f; // Dead animasyonu süresi


    public int playerLives = 3; // Oyuncunun başlangıç canı
    public int score = 0; // Oyuncunun başlangıç puanı


    void Start()
    {

        rb = gameObject.GetComponent<Rigidbody2D>();
        ani = gameObject.GetComponent<Animator>();
        whatIsGround = LayerMask.GetMask("Ground");
        extraJump = maxExtraJumps;
        puanText = GameObject.Find("PuanText").GetComponent<TextMeshProUGUI>(); // TMPro TextMeshProUGUI bileşenini bul ve referansla
    }

    private void Update()
    { if (puan < 120)
        {
            if (playerLives > 0)
            {
                puanText.text = "Puan: " + puan; // Puanı metin olarak güncelle
                jumping();
            }
        }
  

        dead();

    }

    void FixedUpdate()
    {

        horizontal = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);


        if (horizontal > 0 && !lookright)
        {
            turn();
        }
        else if (horizontal < 0 && lookright)
        {
            turn();
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            extraJump = maxExtraJumps;
        }
        else if (collision.gameObject.CompareTag("Block"))
        {
            // Engelleyici bloğa çarptığında kaydırıcı kuvvet uygulama
            rb.velocity = new Vector2(rb.velocity.x, -jumpForce * 0.5f);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("healthy"))
        {
            // Sağlıklı prefab'a dokunulduğunda yapılacak işlemler
            puan += 10; // Puan ekleme işlemi
            Debug.Log("Healthy prefab collided with player. Score: " + score);
            Destroy(collision.gameObject); // Nesneyi yok et
        }
        else if (collision.CompareTag("unhealthy"))
        {
            // Sağlıksız prefab'a dokunulduğunda yapılacak işlemler
            playerLives -= 1; // Can eksiltme işlemi
            Debug.Log("Unhealthy prefab collided with player. Lives left: " + playerLives);
            Destroy(collision.gameObject); // Nesneyi yok et
            kalp_yoket();
           
        }
        

    }
    void kalp_yoket()
    { 
        if (playerLives == 2)
        {
            GameObject nesne = GameObject.Find("can1");
            if (nesne != null)
            {
                nesne.SetActive(false); // Nesneyi devre dışı bırak
            }
        }
        else if (playerLives == 1)
        {
            GameObject nesne = GameObject.Find("can2");
            if (nesne != null)
            {
                nesne.SetActive(false); // Nesneyi devre dışı bırak
            }
        }
        else if (playerLives == 0)
        {
            GameObject nesne = GameObject.Find("can3");
            if (nesne != null)
            {
                nesne.SetActive(false); // Nesneyi devre dışı bırak
            }
        }

        if (playerLives <= 0)
        {
            Debug.Log("Game Over!");
            //SceneManager.LoadScene("EsmaScene");
        }

    }




    private void jumping()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
       

        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)))
        {
            if (isGrounded || extraJump > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                
                if (!isGrounded)
                {
                    extraJump--;
                }
            }
            

        }
        
    }

    void turn()
    {
        lookright = !lookright;
        scale = gameObject.transform.localScale;
        scale.x *= -1;
        gameObject.transform.localScale = scale;
    }
    void dead()
    { if (playerLives <= 0) { 
        ani.SetBool("isDead", true);
        rb.isKinematic = true; // Yerçekimi etkisi aktif
        rb.velocity = Vector2.zero; // Hızı sıfırla
        StartCoroutine(HandleDeath());
    } }
    private IEnumerator HandleDeath()
    {
        // Dead animasyonu sırasında karakterin hareketini durdur
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;


        // Dead animasyonunun tamamlanması için bekle
        yield return new WaitForSeconds(deadAnimationDuration);

        // Dead animasyonu tamamlandıktan sonra yapılacak işlemler
        // Örneğin, sahneyi yeniden başlat veya başka bir ekran göster
        // Buraya gerekli işlemleri ekleyebilirsiniz
        Debug.Log("Dead animasyon tamamlandı. Yeni ekran veya işlemler yapılabilir.");
        RestartGame();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}