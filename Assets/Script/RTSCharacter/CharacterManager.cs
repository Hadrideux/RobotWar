using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterManager : Singleton<CharacterManager>
{
    [SerializeField] private GameObject chracterController = null;


    public GameObject CharacterController
    {
        get => chracterController;
        set => chracterController = value;
    }


    private event Action onCharacterMovement = null;
    public event Action OnCharacterMovement
    {
        add
        {
            onCharacterMovement -= value;
            onCharacterMovement += value;
        }
        remove
        {
            onCharacterMovement -= value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void OnCharacterMove()
    {
        if (onCharacterMovement != null)
            onCharacterMovement();
    }
}
