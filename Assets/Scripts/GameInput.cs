using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput:MonoBehaviour {
    public event EventHandler OnInteractAction;
    private PlayerInput playerInput;

    private void Awake() {
        playerInput = new PlayerInput();
        playerInput.Player.Enable();
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        //checks if OnInteractAction is null b4 executing method
        OnInteractAction?.Invoke(this,EventArgs.Empty);
    }

    public Vector2 GetMovementVectorNormalized() {
        //Creates apprpiate vector for movement keys being pressed
        Vector2 inputVector = playerInput.Player.Movement.ReadValue<Vector2>();
        //corrects magnitude of vector to 1 when moving diagonally so u don't go faster NOTE: can also do this with PlayerInputAction using Processors section
        inputVector = inputVector.normalized;
        //Debug.Log(inputVector);

        return inputVector;
    }
}
