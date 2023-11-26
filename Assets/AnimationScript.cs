using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationScript : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 lastPos;
    private Animator animator;
    void Start()
    {
        lastPos = transform.position;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (CharMoved())
        {
            animator.SetBool("walk", true);
        }
        else
        {
            animator.SetBool("walk", false);
        }
    }

    bool CharMoved()
    {
        Vector3 displacement = transform.position - lastPos;
        lastPos = transform.position;
        Debug.Log("Moving");
        return displacement.magnitude > 0.001; // return true if char moved 1mm
    }
}
