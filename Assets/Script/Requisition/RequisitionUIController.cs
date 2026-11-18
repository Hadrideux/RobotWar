using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class RequisitionUIController : MonoBehaviour
{
    #region ATTRIBUTS
    [Header("Manager")]
    [SerializeField] private RequisitionManager requisitionManageur = null;
    [Space(5)]
    [Header("HUD")]
    [SerializeField] private UIDocument HUDInterface = null;
    #endregion ATTRIBUTS

    #region METHODE
    #region MONO
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HUDInterface.rootVisualElement.Q<Label>("RequisitionQuantity").text = RequisitionManager.Instance.GetRequisitionData(EFactionType.ALLY).RequisitionStock.ToString();

        requisitionManageur = RequisitionManager.Instance;
        requisitionManageur.GetRequisitionData(EFactionType.ALLY).OnUpdateRequisition += UpdateRequisitionLoad;
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnDestroy()
    {
        requisitionManageur.GetRequisitionData(EFactionType.ALLY).OnUpdateRequisition -= UpdateRequisitionLoad;
    }
    private void OnApplicationQuit()
    {
        requisitionManageur.GetRequisitionData(EFactionType.ALLY).OnUpdateRequisition -= UpdateRequisitionLoad;
    }
    #endregion
    private void UpdateRequisitionLoad(int amount)
    {
        HUDInterface.rootVisualElement.Q<Label>("RequisitionQuantity").text = amount.ToString();
    }
    #endregion



}
