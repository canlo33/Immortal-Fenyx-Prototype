using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public GameObject hitParticle;
    public int damageAmount;
    private PlayerController playerController;
    private GameObject enemy;
    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void OnCollisionEnter(Collision collider)
    {
        enemy = collider.gameObject;
        if (enemy.transform.CompareTag("Enemy") || enemy.transform.CompareTag("Ground"))
        {
            enemy = collider.gameObject;
            GameObject instatiatedParticle = Instantiate(hitParticle);
            instatiatedParticle.transform.position = transform.position;
            instatiatedParticle.transform.LookAt(playerController.transform.position);
            playerController.OnArrowLanded();
            if (enemy.GetComponent<HealthSystem>() != null)
                enemy.GetComponent<HealthSystem>().Damage(damageAmount);
        }
    }


}
