using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float _speedMovement = 0f;
    [SerializeField] private float _speedRotation = 0f;
    [SerializeField] private float _zoomSpeed = 0f;

    [SerializeField] private float _maxHeight = 15f;
    [SerializeField] private float _minHeight = 5f;

    [SerializeField] private Vector3 _rotationTarget = Vector3.zero;
    [Range(0f, 1f)]
    [SerializeField] private float _speedAlpha = 0f;


    private void Start()
    {
        
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
            
        Vector3 forwardMove = Input.GetAxis("Forward") * transform.forward * _speedMovement * Time.deltaTime;
        Vector3 rightMove = Input.GetAxis("Right") * transform.right * _speedMovement * Time.deltaTime;

        movementCamera.z += forwardMove.z + rightMove.z;
        movementCamera.x += forwardMove.x + rightMove.x;
        movementCamera.y -= Input.GetAxis("ScrollWheel") * _zoomSpeed * Time.deltaTime;     

        Vector3 yAxisCamera = movementCamera;
        yAxisCamera.y = Mathf.Clamp(yAxisCamera.y, _minHeight, _maxHeight);
        transform.position = yAxisCamera;
    }

    private void CameraRotation()
    {
        _rotationTarget = new Vector3(0, Input.GetAxis("RotateCamera") * _speedRotation * Time.deltaTime, 0);
    
        Vector3 eulerAngles = new Vector3(0, transform.eulerAngles.y + _rotationTarget.y, 0);
        Vector3 lerp = Vector3.Lerp(transform.eulerAngles, eulerAngles, _speedAlpha);

        transform.rotation = Quaternion.Euler(lerp);
    }
}
