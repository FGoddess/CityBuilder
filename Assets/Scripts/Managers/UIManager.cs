using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Action OnRoadPlacement;
    public Action OnHousePlacement;
    public Action OnSpecialPlacement;
    public Action OnBigStructurePlacement;

    public Button placeRoadButton;
    public Button placeHouseButton;
    public Button placeSpecialButton;
    public Button placeBigStructureButton;

    [SerializeField] private Color outlineColor;
    private List<Button> buttonsList;


    private static UIManager _instance;
    public static UIManager Instance => _instance;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else if (_instance != this)
            Destroy(this.gameObject);
    }


    private void Start()
    {
        buttonsList = new List<Button> { placeHouseButton, placeRoadButton, placeSpecialButton, placeBigStructureButton };

        placeRoadButton.onClick.AddListener(() =>
        {
            ResetButtonColor();
            ModifyOutline(placeRoadButton);
            OnRoadPlacement?.Invoke();
        });
        placeHouseButton.onClick.AddListener(() =>
        {
            ResetButtonColor();
            ModifyOutline(placeHouseButton);
            OnHousePlacement?.Invoke();
        });
        placeSpecialButton.onClick.AddListener(() =>
        {
            ResetButtonColor();
            ModifyOutline(placeSpecialButton);
            OnSpecialPlacement?.Invoke();
        });
        placeBigStructureButton.onClick.AddListener(() =>
        {
            ResetButtonColor();
            ModifyOutline(placeBigStructureButton);
            OnBigStructurePlacement?.Invoke();
        });
    }

    private void ModifyOutline(Button bttn)
    {
        var outline = bttn.GetComponent<Outline>();
        outline.effectColor = outlineColor;
        outline.enabled = true;
    }

    public void ResetButtonColor()
    {
        foreach(Button bttn in buttonsList)
        {
            bttn.GetComponent<Outline>().enabled = false;
        }
    }
}
