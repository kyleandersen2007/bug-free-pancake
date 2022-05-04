using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    PlayerControls inputActions;

    public float rotationAmount;
    public float rotationSpeed;

    Vector2 cameraInput;

    Vector3 currentRotation;
    Vector3 targetRotation;

    private void OnEnable()
    {
        if(inputActions == null)
        {
            inputActions = new PlayerControls();
            inputActions.PlayerMovement.Camera.performed += inputActions => cameraInput = inputActions.ReadValue<Vector2>();
        }

        inputActions.Enable();
    }

    private void Start()
    {
        currentRotation = transform.eulerAngles;
        targetRotation = transform.eulerAngles;
    }

    private void Update()
    {
        if(cameraInput.x < 0)
        {
            targetRotation.y = targetRotation.y + rotationAmount;
        }
        else if(cameraInput.x > 0)
        {
            targetRotation.y = targetRotation.y - rotationAmount;
        }

        currentRotation = Vector3.Lerp(currentRotation, targetRotation, rotationSpeed * Time.deltaTime);
        transform.eulerAngles = currentRotation;
    }
}
