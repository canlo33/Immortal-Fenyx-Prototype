using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    public GameObject hitParticle;
    public Transform particleSpawnPoint;
    public int firstComboDamage;
    public int secondComboDamage;
    public int thirdComboDamage;
    private Animator anim;
    private GameObject enemy;
    private void Start()
    {
        anim = transform.GetComponentInParent<Animator>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        enemy = collision.gameObject;
        HealthSystem healthSystem = enemy.GetComponent<HealthSystem>();
        if (enemy.CompareTag("Enemy"))
        {
            Instantiate(hitParticle);
            hitParticle.transform.position = particleSpawnPoint.position;
            switch (anim.GetInteger("comboCount"))
            {
                case 0:
                    healthSystem.Damage(firstComboDamage);
                    break;
                case 2:
                    healthSystem.Damage(secondComboDamage);
                    break;
                case 3:
                    healthSystem.Damage(thirdComboDamage);
                    break;
            }

        }
    }

}
