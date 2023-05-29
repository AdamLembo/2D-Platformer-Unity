using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private float curHealth;
    public bool isHit = true;
    Animator animC;

    public GameObject healthBarUI;
    public Slider healthBar;

    private GameManager GM;


    // Start is called before the first frame update
    void Start()
    {
        animC = gameObject.GetComponent<Animator>();
        curHealth = maxHealth;
        healthBar.value = CalculateHealth();
        healthBarUI.SetActive(false);
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
       healthBar.value = CalculateHealth();
        if(curHealth < maxHealth)
        {
           healthBarUI.SetActive(true);
        }

        if (curHealth <= 0)
        {
            StartCoroutine(DestroyEnemy());
            animC.SetTrigger("Death");
        }
    }

    public void AdjustCurrentHealth(int adj)
    {
        animC.SetTrigger("Hit");
        FindObjectOfType<AudioManager>().Play("EnemyHit");
        curHealth += adj;
        isHit = true;

        if(curHealth < 0)
        {
            curHealth = 0;
        }

        if(curHealth > maxHealth)
        {
            curHealth = maxHealth;
        }
    }

    float CalculateHealth()
    {
        return curHealth / maxHealth;
    }

    IEnumerator DestroyEnemy()
    {
        yield return new WaitForSeconds(0.8f);
        GM.scoreCounter += 10;
        Destroy(this.transform.gameObject);
    }
}
