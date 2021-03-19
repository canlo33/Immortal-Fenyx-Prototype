using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float HorizontalMove { get; private set; }
    public float VerticalMove { get; private set; }
    public float HorizontalMoveRaw { get; private set; }
    public float VerticalMoveRaw { get; private set; }
    public bool DashInput { get; private set; }
    public bool JumpInput { get; private set; }
    public bool AttackInput { get; private set; }
    private void Update()
    {
        HorizontalMove = Input.GetAxis("Horizontal");
        HorizontalMoveRaw = Input.GetAxisRaw("Horizontal");
        VerticalMove = Input.GetAxis("Vertical");
        VerticalMoveRaw = Input.GetAxisRaw("Vertical");
        JumpInput = Input.GetKeyDown(KeyCode.Space);
        DashInput = Input.GetKeyDown(KeyCode.LeftShift);
        AttackInput = Input.GetMouseButtonDown(0);
    }
}
