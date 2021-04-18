using UnityEngine;

public class GameManager : MonoBehaviour
{
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

    [SerializeField] private CameraMovement cameraMovement;

    private void Start()
    {
        InputManager.Instance.OnMouseClick += HandleMouseClick;
    }

    private void HandleMouseClick(Vector3Int pos)
    {
        Debug.Log(pos);
        RoadManager.Instance.PlaceRoad(pos);
    }

    private void Update()
    {
        cameraMovement.MoveCamera(new Vector3(InputManager.Instance.CameraMovementVector.x, 0, InputManager.Instance.CameraMovementVector.y));
    }
}
