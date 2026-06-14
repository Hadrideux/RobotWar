using TMPro;
using UnityEngine;

public class BuildingUIController : MonoBehaviour
{
    [SerializeField] private SelectableManager selectableManager = null;

    [Header("Factory UI")]
    [SerializeField] private Canvas warFactoryUI = null;
    [SerializeField] private TMP_Text buildNameText = null;
    [SerializeField] private TMP_Text buildDurabilityText = null;
    [SerializeField] private TMP_Text buildArmorText = null;


    [Header("Ressource UI")]
    [SerializeField] private Canvas SupplyUI = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        selectableManager = SelectableManager.Instance;

        selectableManager.OnBuildingSelected += OpenBuildingUI;
        selectableManager.OnSelectionCleared += CloseBuildingUI;
        selectableManager.OnUnitSelected += CloseBuildingUI;
    }

    private void OpenBuildingUI(ABuildClass build)
    {
        switch (build.BuildType)
        {
            case EBuildType.ASSEMBLY:
                warFactoryUI.gameObject.SetActive(true);
                buildNameText.text = build.BuildData.BuildName;
                buildDurabilityText.text = "Durability: " + build.CurrentDurability.ToString();
                buildArmorText.text = "Armor: " + build.BuildData.Armor.ToString();
                break;
            case EBuildType.SUPPLY:
                break;
            default:
                break;
        }
    }
    private void CloseBuildingUI()
    {
        warFactoryUI.gameObject.SetActive(false);

    }
}
