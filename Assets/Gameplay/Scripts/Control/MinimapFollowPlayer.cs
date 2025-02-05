using UnityEngine;

public class MinimapFollowPlayer : MonoBehaviour
{
    public int height;
    
    private void Start()
    {
        LoopControl.instance.somethingUpdate += Follow;
    }

    private Vector3 target;
    private void Follow()
    {
        target = PlayerControl.Instance.transform.position;
        target.y = height;
        transform.position = target;
    }
}
