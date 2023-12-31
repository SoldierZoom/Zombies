using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour {
    [SerializeField] private float mouseSensitivity = 100f;
    [SerializeField] private Transform player;
    [SerializeField] private float rotateSpeed = 10f;
     private float xRotation = 0f;
    void Start() {
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Update() {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation,-90f,90f);

        transform.localRotation = Quaternion.Lerp(transform.localRotation,Quaternion.Euler(xRotation,0f,0f),rotateSpeed * Time.deltaTime);
        player.Rotate(Vector3.up * mouseX);
    }
}
