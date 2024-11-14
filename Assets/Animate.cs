using UnityEngine;

public class Animate : MonoBehaviour
{
    public Animator animator;

    void Start()
    {
        // Get the Animator component attached to the same GameObject or child
        //animator = GetComponent<Animator>();
    }

    // This method will be called every time the tower attacks
    public void TriggerAnimation()
    {
        // Set the trigger to start the pulse animation
        animator.SetTrigger("PulseEffect");
    }
}
