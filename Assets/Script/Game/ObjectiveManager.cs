using UnityEngine;

public class ObjectiveManager : Singleton<ObjectiveManager>
{
    #region ATTRIBUTS

    #endregion ATTRIBUTS

    #region METHODES
    #region MONO

    #endregion MONO
    public void HQDestroyed(EFactionType faction)
    {
        switch (faction)
        {
            case EFactionType.ALLY:
                GameManager.Instance.HandleGameState(EGameState.DEFEAT);
                break;
            case EFactionType.IA:
                GameManager.Instance.HandleGameState(EGameState.VICTORY);
                break;
            default:
                break;
        }
    }
    #endregion METHODES


}
