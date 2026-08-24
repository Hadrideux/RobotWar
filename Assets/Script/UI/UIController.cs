using UnityEngine;

public class UIController : MonoBehaviour
{

    public void BuildSelected(ABuildClass prefab)
    {
        PlayerInteraction.Instance.ChangePlayerState(EPlayerState.CONSTURCTION);

        BuilderSystemManager.Instance.SelectedBuild(prefab);
    }
}
