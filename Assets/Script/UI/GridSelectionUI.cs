using System;
using UnityEngine;
using UnityEngine.UIElements;

public class GridSelectionUI : MonoBehaviour
{
    #region ATTRBITUS
    [Header("Manager")] 
    [SerializeField] private SelectableManager selectableManager = null;
    [SerializeField] private BuildingUIController buildingUIController = null;
    [Space(5)]
    [Header("HUD")]
    [SerializeField] private UIDocument HUDInterface = null;
    [SerializeField] private VisualTreeAsset buildSlot = null;
    private ScrollView buildGrid = null;
    [Space(5)]
    [SerializeField] private ABuildClass[] prefabArray = null;
    #endregion ATTRBITUS

    #region METHODES
    #region MONO
    private void Start()
    {
        buildGrid = HUDInterface.rootVisualElement.Q<ScrollView>("BuildGrid");

        selectableManager = SelectableManager.Instance;

        selectableManager.OnBuildingSelected += OnBuildingSelected;
        selectableManager.OnSelectionCleared += ShowBuildings;
        selectableManager.OnUnitSelected += ShowBuildings;

        ShowBuildings();
    }

    private void OnDestroy()
    {
        selectableManager.OnBuildingSelected -= OnBuildingSelected;
        selectableManager.OnSelectionCleared -= ShowBuildings;
        selectableManager.OnUnitSelected -= ShowBuildings;

    }
    private void OnApplicationQuit()
    {
        selectableManager.OnBuildingSelected -= OnBuildingSelected;
        selectableManager.OnSelectionCleared -= ShowBuildings;
        selectableManager.OnUnitSelected -= ShowBuildings;

    }
    #endregion MONO
    #endregion METHODES

    private void OnBuildingSelected(ABuildClass build)
    {
        if (build is Assembly factory) ShowUnits(factory);
        else ShowBuildings();
    }

    private void ShowBuildings()
    {
        buildGrid.Clear();
        foreach (ABuildClass prefab in prefabArray)
            AddSlot(prefab.BuildData.BuildingIcon, prefab.BuildData.BuildName, () => BuildSelected(prefab));
    }

    private void ShowUnits(Assembly factory)
    {
        buildGrid.Clear();
        foreach (AUnitClass unit in factory.UnitProductibleList)
            AddSlot(unit.UnitData.UnitIcon, unit.UnitData.UnitName, () => buildingUIController.AssemblyProductionSelect(unit));
    }

    private void AddSlot(Sprite icon, string label, Action onClick)
    {
        VisualElement slot = buildSlot.Instantiate();
        slot.Q<Image>("PrefabIcon").sprite = icon;
        slot.Q<Label>("PrefabName").text = label;
        slot.Q<Button>("PrefabButton").clicked += onClick;
        buildGrid.Add(slot);
    }

    private void BuildSelected(ABuildClass prefab)
    {
        PlayerInteraction.Instance.ChangePlayerState(EPlayerState.CONSTURCTION);
        BuilderSystemManager.Instance.SelectedBuild(prefab);
    }
}
