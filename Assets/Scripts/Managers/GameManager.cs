using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private CameraMovement cameraMovement;

    private static GameManager _instance;
    public static GameManager Instance
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

    private void Start()
    {
        UIManager.Instance.OnRoadPlacement += RoadPlacementHandler;
        UIManager.Instance.OnHousePlacement += HousePlacementHandler;
        UIManager.Instance.OnSpecialPlacement += SpecialPlacementHandler;
        UIManager.Instance.OnBigStructurePlacement += BigStructureHandler;
        InputManager.Instance.OnEscape += HandleEscape;
    }

    private void HandleEscape()
    {
        ClearInputActions();
        UIManager.Instance.ResetButtonColor();
        PathVisualizer.Instance.ResetPath();
        InputManager.Instance.OnMouseClick += TrySelectAgent;
    }

    private void TrySelectAgent(Ray ray)
    {
        GameObject hitObj = ObjectDetector.Instance.RaycastAll(ray);
        if(hitObj != null)
        {
            var tempAgent = hitObj.GetComponent<AIAgent>();
            tempAgent?.ShowPath();
        }

    }

    private void BigStructureHandler()
    {
        ClearInputActions();
        InputManager.Instance.OnMouseClick += (pos) =>
        {
            ProcessInputAndCall(StructureManager.Instance.PlaceBigStructure, pos);
        };
        InputManager.Instance.OnEscape += HandleEscape;
    }

    private void HousePlacementHandler()
    {
        ClearInputActions();
        InputManager.Instance.OnMouseClick += (pos) =>
        {
            ProcessInputAndCall(StructureManager.Instance.PlaceHouse, pos);
        };
        InputManager.Instance.OnEscape += HandleEscape;
    }

    private void SpecialPlacementHandler()
    {
        ClearInputActions();
        InputManager.Instance.OnMouseClick += (pos) =>
        {
            ProcessInputAndCall(StructureManager.Instance.PlaceSpecial, pos);
        };
        InputManager.Instance.OnEscape += HandleEscape;
    }

    private void RoadPlacementHandler()
    {
        ClearInputActions();
        InputManager.Instance.OnMouseClick += (pos) =>
        {
            ProcessInputAndCall(RoadManager.Instance.PlaceRoad, pos);
        };
        InputManager.Instance.OnMouseUp += RoadManager.Instance.FinishPlacingRoad;
        InputManager.Instance.OnMouseHold += (pos) =>
        {
            ProcessInputAndCall(RoadManager.Instance.PlaceRoad, pos);
        };
        InputManager.Instance.OnEscape += HandleEscape;
    }

    private void ProcessInputAndCall(Action<Vector3Int> callback, Ray ray)
    {
        Vector3Int? result = ObjectDetector.Instance.RaycastGround(ray);
        if (result.HasValue)
            callback.Invoke(result.Value);
    }

    private void Update()
    {
        cameraMovement.MoveCamera(new Vector3(InputManager.Instance.CameraMovementVector.x, 0, InputManager.Instance.CameraMovementVector.y));
    }

    private void ClearInputActions()
    {
        InputManager.Instance.ClearEvents();
    }
}
