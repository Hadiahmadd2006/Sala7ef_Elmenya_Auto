using UnityEngine;

public class DoorPart : MonoBehaviour, IInteractablePart
{
    [SerializeField] private Animator animator;

    private static readonly int IsOpenHash = Animator.StringToHash("IsOpen");

    public void OnTap()
    {
        bool current = animator.GetBool(IsOpenHash);
        animator.SetBool(IsOpenHash, !current);
    }

    // Called by world-space buttons too
    public void Toggle() => OnTap();
}