using UnityEngine;
using UnityEngine.UIElements;

public class BuildingUIController : MonoBehaviour
{
    #region ATTRIBUTS

    [Header("Manager")]
    [SerializeField] private SelectableManager selectableManager = null;
    [Space(5)]
    [Header("HUD")]
    [SerializeField] private UIDocument HUDInterface = null;
    #endregion ATTRIBUTS

    #region MONO
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        selectableManager = SelectableManager.Instance;

        CloseBuildingUI();

        selectableManager.OnBuildingSelected += OpenBuildingUI;
        selectableManager.OnSelectionCleared += CloseBuildingUI;
        selectableManager.OnUnitSelected += CloseBuildingUI;
    }

    private void OnDestroy()
    {
        selectableManager.OnBuildingSelected -= OpenBuildingUI;
        selectableManager.OnSelectionCleared -= CloseBuildingUI;
        selectableManager.OnUnitSelected -= CloseBuildingUI;
    }
    private void OnApplicationQuit()
    {
        selectableManager.OnBuildingSelected -= OpenBuildingUI;
        selectableManager.OnSelectionCleared -= CloseBuildingUI;
        selectableManager.OnUnitSelected -= CloseBuildingUI;
    }
    #endregion MONO

    private void OpenBuildingUI(ABuildClass build)
    { 
        switch (build.BuildType)
        {
            case EBuildType.ASSEMBLY:
                HUDInterface.rootVisualElement.Q("BuildingControlUI").style.display = DisplayStyle.Flex;
                DisplayFactoryUI(build);
                break;
            case EBuildType.SUPPLY:
                CloseBuildingUI();
                break;
            default:
                CloseBuildingUI();
                break;
        }

    }
    private void CloseBuildingUI()
    {
        HUDInterface.rootVisualElement.Q("BuildingControlUI").style.display = DisplayStyle.None;
    }

    public void AssemblyProductionSelect(AUnitClass unit)
    {
        foreach (ISelectable selectable in selectableManager.SelectableObj)
        {
            Assembly assembly = selectable as Assembly;
            
            if(assembly != null)
            {
                assembly.UnitAssembled = unit;
                DisplayFactoryUI(assembly);
            }
        }
    }

    private void DisplayFactoryUI(ABuildClass build)
    {
        Assembly assembly = build as Assembly;
        VisualElement root = HUDInterface.rootVisualElement.Q<VisualElement>("ControlPanel");

        root.Q<Label>("BuildName").text = assembly.BuildData.BuildName;
        root.Q<Label>("BuildDurability").text = "Durability" + assembly.CurrentDurability.ToString();
        root.Q<Label>("BuildArmor").text = "Armor" + assembly.BuildData.Armor.ToString();

        if (assembly.UnitAssembled != null)
        {
            root.Q<Image>("UnitProduct").sprite =assembly.UnitAssembled.UnitData.UnitIcon;
            root.Q<Label>("ProductTime").text = "Duration" + assembly.UnitAssembled.UnitData.ProductionTime.ToString();
            root.Q<Label>("ProductCost").text = "Cost" + assembly.UnitAssembled.UnitData.RequisitionCost.ToString();
        }
        //root.Q<ProgressBar>("TimeRemaining");
    }
}
