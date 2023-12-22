using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [Header("Controller")]
    [SerializeField] private CameraController _cameraController =null;
    [SerializeField] private PlayerInteraction _playerInteraction = null;


    private event Action _onCharacterMovement = null;
    public event Action OnCharacterMovement
    {
        add 
        {
            _onCharacterMovement -= value;
            _onCharacterMovement += value;        
        }
        remove 
        { 
            _onCharacterMovement -= value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _cameraController = GetComponent<CameraController>();
        _playerInteraction = GetComponent<PlayerInteraction>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
