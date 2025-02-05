using UnityEngine;

public class LockMouse : MonoBehaviour
{
#if UNITY_EDITOR
    private bool _cursorLocked;
    private void Update()
    {
        if (PlayerControl.Instance && PlayerControl.Instance.platform == Platform.PC)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Hide_ShowMouseCursor();
            }
            if (Input.GetMouseButtonDown(1))
            {
                Hide_ShowMouseCursor();
            }
        }
    }

    private void Hide_ShowMouseCursor()
    {
        if (!_cursorLocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            _cursorLocked = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            _cursorLocked = false;
        }
    }
#endif
}