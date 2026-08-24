using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class BuildingUIController : MonoBehaviour
{
    [SerializeField] private SelectableManager selectableManager = null;

    [Header("Factory UI")]
    [SerializeField] private GameObject assemblyUI = null;
    [SerializeField] private TMP_Text nameText = null;
    [SerializeField] private TMP_Text durabilityText = null;
    [SerializeField] private TMP_Text armorText = null;
    [SerializeField] private TMP_Text productionTimeText = null;
    [SerializeField] private TMP_Text productionCostText = null;
    [SerializeField] private Image assemblyProgress = null;
    [SerializeField] private Image unitAssembled = null;


    [Header("Ressource UI")]
    [SerializeField] private Canvas SupplyUI = null;

    #region MONO
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        selectableManager = SelectableManager.Instance;

        selectableManager.OnBuildingSelected += OpenBuildingUI;
        selectableManager.OnSelectionCleared += CloseBuildingUI;
        selectableManager.OnUnitSelected += CloseBuildingUI;

        assemblyUI.SetActive(false);
    }
    private void Update()
    {
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
                Assembly assembly = build as Assembly;
                assemblyUI.gameObject.SetActive(true);

                nameText.text = assembly.BuildData.BuildName;
                durabilityText.text = "Durability: " + assembly.CurrentDurability.ToString();
                armorText.text = "Armor: " + assembly.BuildData.Armor.ToString();

                if(assembly.UnitAssembled != null)
                {
                    unitAssembled.sprite = assembly.UnitAssembled.UnitData.UnitIcon;
                }

                break;
            case EBuildType.SUPPLY:
                break;
            default:
                break;
        }
    }
    private void CloseBuildingUI()
    {
        assemblyUI.gameObject.SetActive(false);

    }

    public void AssemblyProductionSelect(AUnitClass unit)
    {
        foreach (ISelectable selectable in selectableManager.SelectableObj)
        {
            Assembly assembly = selectable as Assembly;

            assembly.UnitAssembled = unit;
            unitAssembled.sprite = assembly.UnitAssembled.UnitData.UnitIcon;
        }
    }
}
