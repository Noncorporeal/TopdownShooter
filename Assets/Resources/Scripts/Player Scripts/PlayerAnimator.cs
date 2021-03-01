using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;
    private PlayerController controller;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        /*controller = GetComponent<PlayerController>();
        if (controller.isMoving)
            animator.SetBool("isMoving", true);
        else
            animator.SetBool("isMoving", false);*/
    }
}
