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
    }

    private void BigStructureHandler()
    {
        ClearInputActions();
        InputManager.Instance.OnMouseClick += StructureManager.Instance.PlaceBigStructure;
    }

    private void HousePlacementHandler()
    {
        ClearInputActions();
        InputManager.Instance.OnMouseClick += StructureManager.Instance.PlaceHouse;
    }

    private void SpecialPlacementHandler()
    {
        ClearInputActions();
        InputManager.Instance.OnMouseClick += StructureManager.Instance.PlaceSpecial;
    }

    private void RoadPlacementHandler()
    {
        ClearInputActions();
        InputManager.Instance.OnMouseClick += RoadManager.Instance.PlaceRoad;
        InputManager.Instance.OnMouseHold += RoadManager.Instance.PlaceRoad;
        InputManager.Instance.OnMouseUp += RoadManager.Instance.FinishPlacingRoad;
    }

    /*private void OnDisable()
    {
        InputManager.Instance.OnMouseClick -= RoadManager.Instance.PlaceRoad;
        InputManager.Instance.OnMouseHold -= RoadManager.Instance.PlaceRoad;
        InputManager.Instance.OnMouseUp -= RoadManager.Instance.FinishPlacingRoad;
    }*/

    private void Update()
    {
        cameraMovement.MoveCamera(new Vector3(InputManager.Instance.CameraMovementVector.x, 0, InputManager.Instance.CameraMovementVector.y));
    }

    
    
    private void ClearInputActions()
    {
        InputManager.Instance.OnMouseClick = null;
        InputManager.Instance.OnMouseHold = null;
        InputManager.Instance.OnMouseUp = null;
    }
}
