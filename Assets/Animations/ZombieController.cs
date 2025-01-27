using UnityEngine;

public class ZombieController : MonoBehaviour
{
    private Animator animator;
    private bool isVaulting = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the zombie collided with a vaultable object
        Debug.Log("Collided with: " + other.name);
        if (other.CompareTag("Vaultable") && !isVaulting)
        {
            StartVaulting();
        }
    }

    void StartVaulting()
    {
        isVaulting = true;
        animator.SetBool("IsVaulting", true);
        Debug.Log("Set IsVaulting to true");

        float animationLength = GetAnimationLength("Vault");
        Debug.Log("Vault Animation Length: " + animationLength);

        // Fallback to a fixed duration if the length is 0
        Invoke(nameof(StopVaulting), animationLength > 0 ? animationLength : 1.5f);
    }


    void StopVaulting()
    {
        isVaulting = false;
        animator.SetBool("IsVaulting", false);
    }



    float GetAnimationLength(string animationName)
    {
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            if (clip.name == animationName)
            {
                return clip.length;
            }
        }
        return 0f; // Default if animation not found
    }

    
}
