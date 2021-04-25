using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    private static InputManager _instance;
    public static InputManager Instance
    {
        get
        {
            if (_instance == null)
                Instantiate(_instance);
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }

    public event Action<Ray> OnMouseClick;
    public event Action<Ray> OnMouseHold;
    public event Action OnMouseUp;
    public event Action OnEscape;

    [SerializeField] private Camera mainCamera;

    private Vector2 _cameraMovementVector;
    public Vector2 CameraMovementVector
    {
        get { return _cameraMovementVector; }
    }

    private void Update()
    {
        CheckClickDownEvent();
        CheckClickUpEvent();
        CheckClickHoldEvent();
        CheckArrowInput();
        CheckEscClick();
    }

    private void CheckClickDownEvent()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) //if not under UI
        {
            OnMouseClick?.Invoke(mainCamera.ScreenPointToRay(Input.mousePosition));
        }
    }
    private void CheckClickUpEvent()
    {
        if (Input.GetMouseButtonUp(0) && !EventSystem.current.IsPointerOverGameObject()) //if not under UI
        {
            OnMouseUp?.Invoke();
        }
    }
    private void CheckClickHoldEvent()
    {
        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject()) //if not under UI
        {
            OnMouseHold?.Invoke(mainCamera.ScreenPointToRay(Input.mousePosition));
        }
    }
    private void CheckArrowInput()
    {
        _cameraMovementVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }

    private void CheckEscClick()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            OnEscape.Invoke();
        }
    }

    public void ClearEvents()
    {
        OnMouseClick = null;
        OnMouseHold = null;
        OnEscape = null;
        OnMouseUp = null;
    }
}
