using UnityEngine;

public class UIController : MonoBehaviour
{

    public void BuildSelected(ABuildClass prefab)
    {

        BuilderSystemManager.Instance.SelectedBuild(prefab);
    }
}
