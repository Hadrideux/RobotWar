using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{

    public void BuildSelected(ABuildClass prefab)
    {

        BuilderManager.Instance.SelectedBuild(prefab);
    }
}
