using TMPro;
using UnityEngine;

public class RequisitionUIController : MonoBehaviour
{
    #region ATTRIBUTS

    [SerializeField] private RequisitionManager requisitionManager = null;

    [SerializeField] private TMP_Text requisitionAmount = null;
    [SerializeField] private int totalRequisitionAmount = 0;
    #endregion ATTRIBUTS

    #region METHODE
    #region MONO
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        requisitionAmount.text = string.Empty;

        requisitionManager = RequisitionManager.Instance;
        requisitionManager.OnUpdateRequisition += UpdateRequisitionLoad;
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnDestroy()
    {
        requisitionManager.OnUpdateRequisition -= UpdateRequisitionLoad;
    }
    private void OnApplicationQuit()
    {
        requisitionManager.OnUpdateRequisition -= UpdateRequisitionLoad;
    }
    #endregion
    #endregion


    private void UpdateRequisitionLoad(int amount)
    {
        totalRequisitionAmount += amount;
        requisitionAmount.text = totalRequisitionAmount.ToString();

    }

}
