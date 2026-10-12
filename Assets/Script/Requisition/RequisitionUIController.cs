using TMPro;
using UnityEngine;

public class RequisitionUIController : MonoBehaviour
{
    #region ATTRIBUTS
    [SerializeField] private TMP_Text requisitionText = null;
    #endregion ATTRIBUTS

    #region METHODE
    #region MONO
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        requisitionText.text = RequisitionManager.Instance.GetRequisitionData(EFactionType.ALLY).RequisitionStock.ToString();

        RequisitionManager.Instance.GetRequisitionData(EFactionType.ALLY).OnUpdateRequisition += UpdateRequisitionLoad;
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnDestroy()
    {
        RequisitionManager.Instance.GetRequisitionData(EFactionType.ALLY).OnUpdateRequisition -= UpdateRequisitionLoad;
    }
    private void OnApplicationQuit()
    {
        RequisitionManager.Instance.GetRequisitionData(EFactionType.ALLY).OnUpdateRequisition -= UpdateRequisitionLoad;
    }
    #endregion
    #endregion


    private void UpdateRequisitionLoad(int amount)
    {
        requisitionText.text = amount.ToString();
    }

}
