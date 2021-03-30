using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventController : MonoBehaviour
{
    private Animator anim;
    private PlayerController playerController;
    public TimeEffect attackEffect1;
    public TimeEffect attackEffect2;
    public TimeEffect attackEffect3;
    void Start()
    {
        anim = GetComponent<Animator>();
        playerController = GetComponentInParent<PlayerController>();
    }
    void StartCombo(int amount)
    {
        if(playerController.comboTimerOn)
        {
            anim.SetInteger("comboCount", amount);
            playerController.comboTimerOn = false;
        }
    }
    void ResetCombo()
    {
        playerController.comboTimerOn = false;
        playerController.comboCount = 0;
        playerController.canMove = true;
        playerController.isMeleeAttacking = false;
        anim.SetInteger("comboCount", 0);       
    }
    void ActivateMeleeComboEffect(int comboCount)
    {
        switch (comboCount)
        {
            case 1:
                attackEffect1.Activate();
                break;
            case 2:
                attackEffect2.Activate();
                break;
            case 3:
                attackEffect3.Activate();
                break;
        }
    }
}
