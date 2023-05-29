using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] Transform atkPoint;
    [SerializeField] float atkCooldown;
    [SerializeField] float atkCooldownNormal;
    [SerializeField] float atkCooldownHeavy;
    [SerializeField] float range = 0.5f;

    public Player player;
    [SerializeField] LayerMask enemyLayer;

    private Animator animC;
    private GameManager GM;


    // Start is called before the first frame update
    void Start()
    {
        atkCooldown = 0;
        animC = gameObject.GetComponent<Animator>();
        player = GameObject.Find("Player").GetComponent<Player>();
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0) && atkCooldown <= 0 && GM.isPaused == false)
        {
            Attack();
        }
        if(atkCooldown > 0)
        {
            atkCooldown = atkCooldown - Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Mouse1) && atkCooldown <= 0 && GM.isPaused == false)
        {
            HeavyAttack();
        }
        if (atkCooldown > 0)
        {
            atkCooldown = atkCooldown - Time.deltaTime;
        }


    }

    public void DamageEnemyNormal()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(atkPoint.position, range, enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            EnemyHealth enemyHealth = enemy.gameObject.GetComponent<EnemyHealth>();
            enemyHealth.AdjustCurrentHealth(-10);
        }
    }

    public void DamageEnemyHeavy()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(atkPoint.position, range, enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            EnemyHealth enemyHealth = enemy.gameObject.GetComponent<EnemyHealth>();
            enemyHealth.AdjustCurrentHealth(-20);
        }
    }

    private void Attack()
    {
        FindObjectOfType<AudioManager>().Play("Attack");
        animC.SetTrigger("Attack");
        atkCooldown = atkCooldownNormal;
    }

    private void HeavyAttack()
    {
        FindObjectOfType<AudioManager>().Play("HeavyAttack");
        animC.SetTrigger("HeavyAttack");
        atkCooldown = atkCooldownHeavy;
    }

    private void OnDrawGizmosSelected()
    {
        if(atkPoint == null)
        {
            return;
        }

        Gizmos.DrawWireSphere(atkPoint.position, range);
    }
}
