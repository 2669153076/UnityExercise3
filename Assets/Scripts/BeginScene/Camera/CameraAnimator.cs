using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraAnimator : MonoBehaviour
{
    private Animator animator;

    private UnityAction action;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TurnLeft(UnityAction action)
    {
        animator.SetTrigger("Left");
        this.action = action;
    }
    public void TurnRight(UnityAction action)
    {
        animator.SetTrigger("Right");
        this.action = action;
    }

    public void AnimationPlayOver()
    {
        action?.Invoke();
        action = null;
    }
}
