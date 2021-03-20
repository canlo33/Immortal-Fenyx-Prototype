using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventController : MonoBehaviour
{
    private Animator anim;
    private PlayerController playerController;
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
        anim.SetInteger("comboCount", 0);
        
        
    }
}
