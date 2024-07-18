using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LVL4_GuardLookAt : MonoBehaviour
{
    Animator animator;
    public bool lookAt = false;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnAnimatorIK() {
        if (animator) {
            if (lookAt) {
                animator.SetLookAtWeight(1);
                animator.SetLookAtPosition(Camera.main.transform.position);
            }
            else {
                animator.SetLookAtWeight(0);
            }
        }
    }
}
