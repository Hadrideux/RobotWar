using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform = null;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
      cameraTransform = CharacterManager.Instance.CharacterController.transform;   
    }

    // Update is called once per frame
    void Update()
    {
        FocusEffectUIToCamera();
    }

    public void FocusEffectUIToCamera()
    {
        transform.LookAt(transform.position + Camera.main.transform.forward);
    }
}
