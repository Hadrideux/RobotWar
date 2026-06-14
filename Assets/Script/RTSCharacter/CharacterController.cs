using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [Header("Controller")]
    [SerializeField] private CharacterManager characterManager = null;




    // Start is called before the first frame update
    void Start()
    {
        characterManager = CharacterManager.Instance;

        characterManager.OnCharacterMovement += CharacterMove;
    }

    // Update is called once per frame
    void Update()
    {


    }

    private void OnDestroy()
    {
        characterManager.OnCharacterMovement -= CharacterMove;
    }
    private void OnApplicationQuit()
    {
        characterManager.OnCharacterMovement -= CharacterMove;
    }

    public void CharacterMove()
    {
        characterManager.CharacterController = this.gameObject;
    }
}
