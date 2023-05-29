using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerHealth : MonoBehaviour
{

    [SerializeField] private int maxHeatlh = 100;
    [SerializeField] private int curHealth;
    private Rigidbody2D rb;
    private Animator animC;
    public Healthbar healthBar;


    // Start is called before the first frame update
    void Start()
    {
        healthBar = GameObject.Find("Healthbar").GetComponent<Healthbar>();
        curHealth = maxHeatlh;
        rb = gameObject.GetComponent<Rigidbody2D>();
        animC = gameObject.GetComponent<Animator>();
        healthBar.SetMaxHealth(100);
    }

    // Update is called once per frame
    void Update()
    {
        if(curHealth <= 0)
        {
            curHealth = 0;
        }

        if(curHealth == 0)
        {
            animC.SetBool("Dead", true);
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
           // gameObject.GetComponent<Player>().enabled = false;
            Invoke("Respawn", .7f);
        }


    }

    public void HitPlayer(int damage)
    {
        curHealth -= damage;
        animC.SetTrigger("IsHit");
        FindObjectOfType<AudioManager>().Play("PlayerHit");
        healthBar.SetHealth(curHealth);
    }

    public void HealPlayer(int healAmount)
    {
        curHealth += healAmount;

        if(curHealth >= 100)
        {
            curHealth = 100;
            healthBar.SetHealth(curHealth);
        }
        healthBar.SetHealth(curHealth);
    }


    private void Respawn()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == ("HealthPickup") && curHealth < maxHeatlh)
        {
            FindObjectOfType<AudioManager>().Play("Pickup");
            HealPlayer(50);
            Destroy(collision.gameObject);
        }
    }
}
