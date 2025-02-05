using UnityEngine;

public class MinimapFollowPlayer : MonoBehaviour
{
    public int height;

    public RectTransform directionIcon;
    
    private void Start()
    {
        LoopControl.Instance.somethingUpdate += Follow;
    }

    private Vector3 target;
    private void Follow()
    {
        target = PlayerControl.Instance.transform.position;
        target.y = height;
        transform.position = target;
        target = Vector3.zero;
        target.z = -PlayerControl.Instance.transform.eulerAngles.y - 135;
        directionIcon.eulerAngles = target;
    }
}
