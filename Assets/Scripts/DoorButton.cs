using UnityEngine;
public class DoorButton : MonoBehaviour
{
    [Header("The Animator on this door's pivot")]
    [SerializeField] private Animator doorAnimator;
    private const string OpenParam = "IsOpen";
    public void ToggleDoor()
    {
        if (doorAnimator == null) return;
        bool isOpen = doorAnimator.GetBool(OpenParam);
        doorAnimator.SetBool(OpenParam, !isOpen);
    }
    public void CloseDoor()
    {
        if (doorAnimator != null)
            doorAnimator.SetBool(OpenParam, false);
    }
}