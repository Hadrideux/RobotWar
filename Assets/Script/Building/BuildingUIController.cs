using UnityEngine;
using UnityEngine.UIElements;

public class BuildingUIController : MonoBehaviour
{
    #region ATTRIBUTS

    [Header("Manager")]
    [SerializeField] private SelectableManager selectableManager = null;
    [Space(5)]
    [Header("Factory UI")]
    [SerializeField] private UIDocument buildingControlUI = null;
    #endregion ATTRIBUTS

    #region MONO
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        selectableManager = SelectableManager.Instance;

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
                buildingControlUI.enabled = true;
                DisplayFactoryUI(build);
                break;
            case EBuildType.SUPPLY:
                break;
            default:
                break;
        }

    }
    private void CloseBuildingUI()
    {
        buildingControlUI.enabled = false;
    }

    public void AssemblyProductionSelect(AUnitClass unit)
    {
        foreach (ISelectable selectable in selectableManager.SelectableObj)
        {
            Assembly assembly = selectable as Assembly;
            
            if(assembly != null)
            {
                assembly.UnitAssembled = unit;
            }
        }

    }

    private void DisplayFactoryUI(ABuildClass build)
    {
        Assembly assembly = build as Assembly;
        VisualElement root = buildingControlUI.rootVisualElement.Q<VisualElement>("ControlPanel");

        root.Q<Label>("BuildName").text = assembly.BuildData.BuildName;
        root.Q<Label>("BuildDurability").text = assembly.CurrentDurability.ToString();
        root.Q<Label>("BuildArmor").text = assembly.BuildData.Armor.ToString();

        if (assembly.UnitAssembled != null)
        {
            root.Q<Image>("UnitProduct").sprite = assembly.UnitAssembled.UnitData.UnitIcon;
            root.Q<Label>("ProductTime").text = assembly.UnitAssembled.UnitData.ProductionTime.ToString();
            root.Q<Label>("ProductCost").text = assembly.UnitAssembled.UnitData.RequisitionCost.ToString();
        }
        //root.Q<ProgressBar>("TimeRemaining");
    }
}
