using System.Collections.Generic;
using PrimeTween;
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
    public GameObject CinemachineCameraTarget;
    public CharacterController _controller;

    public Transform handPos;

    public FloatingJoystick joystick;

    [HideInInspector] public Vector2 movement;

    [Title("Weapon")]
    public WeaponControl weapon;

    private float countTimeAttack;
    
    private void Awake()
    {
        Instance = this;
        Application.targetFrameRate = 60;
    }



    private void Start()
    {
        mainCam = Camera.main;
        jointOriginalPos = handPos.localPosition;
        checkLayer = LayerMask.GetMask("Enemy", "Default");
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
        if (weapon)
        {
            if (countTimeAttack > 0)
            {
                countTimeAttack -= Time.deltaTime;
            }
            else
            {
                Shoot();
            }
        }

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
#endif
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
        pitch -= TouchSensitivityY * touchDelta.y * Time.deltaTime;
        yaw = touchDelta.x * TouchSensitivityX * Time.deltaTime;

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
    private Vector3 heightControl;
    [Title("Jump")]
    public float jumpHeight;
    private const float gravityValue = -9.81f;
    private bool _isGrounded;
    private void Movement()
    {
        float targetSpeed = groundSpeed;
        Vector3 inputDirection = new Vector3(movement.x, 0.0f, movement.y).normalized;

        if (movement == Vector2.zero)
        {
            isWalking = false;
            targetSpeed = 0.0f;
        }
        else
        {
            isWalking = true;
            inputDirection = transform.right * movement.x + transform.forward * movement.y;
        }
        
        _isGrounded = _controller.isGrounded;

        if (!_isGrounded)
        {
            heightControl.y += gravityValue * Time.deltaTime;
        }
        
        _speed = targetSpeed;

        if (_controller.enabled)
            _controller.Move(inputDirection.normalized * (_speed * Time.deltaTime) + heightControl * Time.deltaTime);
    }

    public void Jump()
    {
        if (_isGrounded)
        {
            heightControl.y = Mathf.Sqrt(jumpHeight * -2.0f * gravityValue);
        }
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

    #region Shoot

    private RaycastHit objectHit;
    private LayerMask checkLayer;
    private void Shoot()
    {
        if (Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out objectHit, weapon.attackRange,checkLayer))
        {
            if (objectHit.collider.gameObject.layer == DataManager.EnemyLayer)
            {
                //Shoot
                weapon.Attack(true);
                countTimeAttack = 1f / weapon.attackSpeed;
                CrosshairTrigger();
            }
        }
    }

    private void CrosshairTrigger()
    {
        Tween.StopAll(onTarget: UIManager.instance.Crosshair.transform);
        Tween.Scale(UIManager.instance.Crosshair.transform, startValue: 1f, endValue: 1.5f, duration: 0.1f, cycles: 2, cycleMode: CycleMode.Yoyo);
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

            if (touch.phase == TouchPhase.Began)
            {
                lastMousePositionX = touch.position;
                canRotate = true;
            }

            if (touch.phase == TouchPhase.Ended)
            {
                mouseMovementDelta = Vector2.zero;
                isIn = false;
                canRotate = false;
            }

            if (touch.phase == TouchPhase.Moved && canRotate)
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