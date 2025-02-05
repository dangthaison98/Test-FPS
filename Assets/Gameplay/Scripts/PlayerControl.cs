using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerControl : MonoBehaviour
{
        public static PlayerControl Instance;

#if UNITY_EDITOR
    [EnumToggleButtons]
    public Platform platform;
#else
    private Platform platform = Platform.Android;
#endif

    public float groundSpeed;

    public float TouchSensitivityX;
    public float TouchSensitivityY;
    public float mouseSensitivity;

    private const string modelRotateTag = "ModelRotateUI";
    private const float PickRange = 5;
    public GameObject CinemachineCameraTarget;
    public CharacterController _controller;

    public Transform handPos;

    public FloatingJoystick joystick;

    [HideInInspector] public Vector2 movement;
    private float sensitivity;

    private void Awake()
    {
        Instance = this;
        Application.targetFrameRate = 60;
    }



    private void Start()
    {
        jointOriginalPos = handPos.localPosition;
    }
    private void LateUpdate()
    {
        JoystickInput();
        Movement();

        if (platform == Platform.Android)
        {
            RotateCamera();
        }
        else
        {
            RotateCameraPC();
        }

        HeadBob();
    }

    #region Control

    private void JoystickInput()
    {
        if (platform == Platform.Android)
        {
            movement.x = joystick.Horizontal;
            movement.y = joystick.Vertical;
        }
        else
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
        }
    }
    private float yaw;
    [HideInInspector] public float pitch;

    private void RotateCamera()
    {
        touchDelta = GetTouchXDelta();
        pitch -= TouchSensitivityY * touchDelta.y * Time.deltaTime * sensitivity;
        yaw = touchDelta.x * TouchSensitivityX * Time.deltaTime * sensitivity;

        pitch = Mathf.Clamp(pitch, -70f, 70f);

        CinemachineCameraTarget.transform.localRotation = Quaternion.Euler(pitch, 0.0f, 0.0f);

        transform.Rotate(Vector3.up * yaw);
    }

    private void RotateCameraPC()
    {
        pitch -= mouseSensitivity * Input.GetAxis("Mouse Y");
        yaw = Input.GetAxis("Mouse X") * mouseSensitivity;

        pitch = Mathf.Clamp(pitch, -70f, 70f);

        CinemachineCameraTarget.transform.localRotation = Quaternion.Euler(pitch, 0.0f, 0.0f);

        transform.Rotate(Vector3.up * yaw);
    }
    #endregion

    #region Movement
    private float _speed;

    private void Movement()
    {
        float targetSpeed = groundSpeed;
        Vector3 inputDirection = new Vector3(movement.x, 0.0f, movement.y).normalized;

        if (movement == Vector2.zero)
        {
            // if (isWalking)
            //     SoundManager.Instance.SoundFootStepPlayer.Active(false);
            isWalking = false;
            targetSpeed = 0.0f;
        }
        else
        {
            // if (!isWalking)
            //     SoundManager.Instance.SoundFootStepPlayer.Active(true);
            isWalking = true;
            inputDirection = transform.right * movement.x + transform.forward * movement.y;
        }

        _speed = targetSpeed;

        if (_controller.enabled)
            _controller.Move(inputDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, -1, 0.0f) * Time.deltaTime);
    }
    [Title("Head Bob")]
    public float bobSpeed = 10f;
    private bool isWalking;
    public Vector2 bobAmount = new(.15f, .05f);

    private Vector3 jointOriginalPos;
    private float timer;
    private void HeadBob()
    {
        if (isWalking)
        {
            timer += Time.deltaTime * bobSpeed;
            handPos.localPosition = new Vector3(jointOriginalPos.x + Mathf.Sin(timer) * bobAmount.x, jointOriginalPos.y + Mathf.Sin(timer * 2) * bobAmount.y, jointOriginalPos.z);
        }
        else
        {
            timer += Time.deltaTime * 2;
            handPos.localPosition = new Vector3(Mathf.Lerp(handPos.localPosition.x, jointOriginalPos.x, Time.deltaTime * bobSpeed), Mathf.Lerp(handPos.localPosition.y, jointOriginalPos.y, Time.deltaTime * bobSpeed), jointOriginalPos.z + Mathf.Sin(timer) * 0.05f);
        }
    }
    #endregion
    
    #region Caculate
    private Camera mainCam;

    private bool canRotate;
    private Vector2 lastMousePositionX;
    private Vector2 mouseMovementDelta;
    [HideInInspector] public Vector2 touchDelta;
    private bool isIn;
    private Vector2 GetTouchXDelta()
    {
        int order = GetModelRotateTouchOrder();
        if (order > -1)
        {
            Touch touch = Input.GetTouch(order);

            if (!isIn)
            {
                lastMousePositionX = touch.position;
                isIn = true;
            }

            if (touch.phase == UnityEngine.TouchPhase.Began)
            {
                lastMousePositionX = touch.position;
                canRotate = true;
            }

            if (touch.phase == UnityEngine.TouchPhase.Ended)
            {
                mouseMovementDelta = Vector2.zero;
                isIn = false;
                canRotate = false;
            }

            if (touch.phase == UnityEngine.TouchPhase.Moved && canRotate)
            {
                mouseMovementDelta = touch.position - lastMousePositionX;
                lastMousePositionX = touch.position;
                return new Vector2(mouseMovementDelta.x, mouseMovementDelta.y);
            }
        }
        else
        {
            isIn = false;
        }

        return Vector2.zero;
    }
    private int GetModelRotateTouchOrder()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            if (IsPointerOverModelRotateUI(Input.GetTouch(i).position))
            {
                return i;
            }
        }
        return -1;
    }
    private bool IsPointerOverModelRotateUI(Vector3 pos)
    {
        return UIUtils.IsPointerOverUI(UIUtils.GetEventSystemRaycastResults(pos), modelRotateTag);
    }
    #endregion
}


public static class UIUtils
{
    public static List<RaycastResult> GetEventSystemRaycastResults()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raycastResults);
        return raycastResults;
    }

    public static List<RaycastResult> GetEventSystemRaycastResults(Vector3 pos)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current)
        {
            position = pos
        };
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raycastResults);
        return raycastResults;
    }

    public static bool IsPointerOverUI(List<RaycastResult> eventSystemRaycastResults, string UITag)
    {
        if (eventSystemRaycastResults.Count == 0) return false;
        return eventSystemRaycastResults[0].gameObject.tag.Equals(UITag);
    }
}
public enum Platform
{
    PC, Android
}