using UnityEngine;

public class HealthBar : MonoBehaviour
{
    private Camera mainCamera;
    
    private void Start()
    {
        mainCamera = Camera.main;
        LoopControl.Instance.somethingUpdate += RotateBar;
    }

    private void RotateBar()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - mainCamera.transform.position);
    }
}
