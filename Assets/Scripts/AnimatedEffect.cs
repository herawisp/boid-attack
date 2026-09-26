using UnityEngine;

public class AnimatedEffect : MonoBehaviour {

    public Animator Animator;
    public string AnimationTrigger = "Play";

    void Start() {
        if (!string.IsNullOrEmpty(AnimationTrigger))
            Animator.SetTrigger(AnimationTrigger);
        Destroy(gameObject, GetClipLength());
    }

    float GetClipLength() {
        if (Animator == null || Animator.runtimeAnimatorController == null) return 1f;
        return Animator.runtimeAnimatorController.animationClips.Length > 0
            ? Animator.runtimeAnimatorController.animationClips[0].length
            : 1f;
    }
}