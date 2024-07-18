using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LVL4_GuardLookAt : MonoBehaviour
{
    private Animator animator;
    private float lookAtWeight = 0.0f;
    public bool lookAt = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnAnimatorIK()
    {
        if (animator)
        {
            animator.SetLookAtWeight(lookAtWeight);
            animator.SetLookAtPosition(Camera.main.transform.position);

            if (lookAt)
            {
                lookAtWeight = Mathf.Lerp(lookAtWeight, 1f, Time.deltaTime * 2);
            }
            else
            {
                lookAtWeight = Mathf.Lerp(lookAtWeight, 0f, Time.deltaTime * 2);
            }
        }
    }
}
