using UnityEditor.Toolbars;
using UnityEngine;

public class MainMenueUIController : MonoBehaviour
{
    #region ATTRIBTUS
    [Header("Manager")]
    [SerializeField] private GameManager gameManager = null;

    [Header("UI")]
    [SerializeField] private GameObject mainMenuUI = null;
    #endregion ATTRIBTUS

    #region METHODES
    private void Start()
    {
        GameManager.Instance.OnGameStateChanged += GameStateAction;
    }
    private void OnDestroy()
    {
        GameManager.Instance.OnGameStateChanged -= GameStateAction;        
    }
    private void OnApplicationQuit()
    {
        GameManager.Instance.OnGameStateChanged -= GameStateAction;                
    }
    private void GameStateAction(EGameState state)
    {
        switch (state)
        {
            case EGameState.INMENU:
                break;
            case EGameState.PLAY:
                HandleMainMenuUI(false);

                break;
            case EGameState.PAUSE:
                HandleMainMenuUI(true);

                break;
        }
    }

    private void HandleMainMenuUI(bool isActive)
    {
        mainMenuUI.SetActive(true);
    }

    public void PlayGame()
    {
        GameManager.Instance.OnGameState(EGameState.PLAY);
    }
    public void QuitGame()
    {
        GameManager.Instance.OnGameState(EGameState.QUIT);
    }
    #endregion METHODES

}
