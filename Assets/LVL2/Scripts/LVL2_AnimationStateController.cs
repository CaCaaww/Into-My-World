using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LVL2_AnimationStateController : MonoBehaviour
{
    


   //Fetch the Animator
    Animator animator;

    StarterAssetsInputs inputs;

    void Start()
    {
        animator = gameObject.GetComponent<Animator>();

        inputs = gameObject.GetComponent <StarterAssetsInputs>();
    }

    void Update()
    {
        //checks if moving, if not it plays the idle animation
        if (inputs.move.y != 0 || inputs.move.x != 0)
            animator.SetBool("isMoving", true);

        else animator.SetBool("isMoving", false); 

    }
}

