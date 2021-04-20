using System;
using System.Collections;
using System.Collections.Generic;
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
                //Instantiate(_instance);
                Debug.LogError("INPUT MANAGER IS NULL");
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }


    public Action<Vector3Int> OnMouseClick;
    public Action<Vector3Int> OnMouseHold;
    public Action OnMouseUp;

    private Vector2 _cameraMovementVector;

    [SerializeField] Camera mainCamera;

    [SerializeField] LayerMask _groundMask;

    public Vector2 CameraMovementVector
    {
        get { return _cameraMovementVector; }
        set { _cameraMovementVector = value; }
    }

    private void Update()
    {
        CheckClickDownEvent();
        CheckClickUpEvent();
        CheckClickHoldEvent();
        CheckArrowInput();
    }

    private Vector3Int? RaycastGround()
    {
        if(Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
        {
            Vector3Int position = Vector3Int.RoundToInt(hit.point);
            return position;
        }
        return null;
    }

    private void CheckClickDownEvent()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) //if not under UI
        {
            var position = RaycastGround();
            if (position != null)
            {
                OnMouseClick?.Invoke(position.Value);
            }
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
        if(Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject()) //if not under UI
        {
            var position = RaycastGround();
            if(position != null)
            {
                OnMouseHold?.Invoke(position.Value);
            }
        }
    }
    private void CheckArrowInput()
    {
        _cameraMovementVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }
}
