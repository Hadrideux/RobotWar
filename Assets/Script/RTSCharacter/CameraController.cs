using UnityEngine;

public class CameraController : MonoBehaviour
{
    //[SerializeField] private CharacterManager characterManager = null;

    [SerializeField] private float speedMovement = 0f;
    [SerializeField] private float speedRotation = 0f;
    [SerializeField] private float zoomSpeed = 0f;

    [SerializeField] private float maxHeight = 15f;
    [SerializeField] private float minHeight = 5f;

    [SerializeField] private Vector3 rotationTarget = Vector3.zero;
    [Range(0f, 1f)]
    [SerializeField] private float speedAlpha = 0f;


    private void Start()
    {
        //characterManager = CharacterManager.Instance;
        //characterManager.CharacterController = this.gameObject;
    }
    // Update is called once per frame
    void Update()
    {
        CameraMovement();

        if (Input.GetKey(KeyCode.LeftShift))
        {
            CameraRotation();
        }
    }
    private void CameraMovement()
    {
        Vector3 movementCamera = transform.position;

        Vector3 forwardMove = Input.GetAxis("Forward") * transform.forward * speedMovement * Time.deltaTime;
        Vector3 rightMove = Input.GetAxis("Right") * transform.right * speedMovement * Time.deltaTime;

        movementCamera.z += forwardMove.z + rightMove.z;
        movementCamera.x += forwardMove.x + rightMove.x;
        movementCamera.y += Input.GetAxis("ScrollWheel") * zoomSpeed * Time.deltaTime;

        Vector3 yAxisCamera = movementCamera;
        yAxisCamera.y = Mathf.Clamp(yAxisCamera.y, minHeight, maxHeight);
        transform.position = yAxisCamera;

        //characterManager.OnCharacterMove();
    }

    private void CameraRotation()
    {
        rotationTarget = new Vector3(0, Input.GetAxis("RotateCamera") * speedRotation * Time.deltaTime, 0);

        Vector3 eulerAngles = new Vector3(0, transform.eulerAngles.y + rotationTarget.y, 0);
        Vector3 lerp = Vector3.Lerp(transform.eulerAngles, eulerAngles, speedAlpha);

        transform.rotation = Quaternion.Euler(lerp);
    }
}
