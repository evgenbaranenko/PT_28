using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    CharacterController characterController;

    [SerializeField] private float currentCameraY;
    [SerializeField] private float cameraTurnSpeed = 7f;
    [SerializeField] private float cameraDistanceForward = 1f;
    [SerializeField] private float cameraDistanceUp = 2.5f;
    [SerializeField] private float cameraSmoothing = 0.01f;

    private Camera _camera;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
        characterController = _camera.GetComponentInParent<CharacterController>();

    }
   
    void Update()
    {
        CameraController();
        MouseLook();
    }

    /*����������� ��������� ������ ��������� � ��������� �� ���� ���� ��
   �� � (������-��������):*/
    public void MouseLook()
    {
        characterController.transform.RotateAround(characterController.transform.position, Vector3.up, Input.GetAxis("Mouse X") * cameraTurnSpeed);
    }
    public void CameraController()
    {
        // ����������� ��������� ������:
        Vector3 cameraPosition = characterController.transform.position - (characterController.transform.forward * cameraDistanceForward);
        cameraPosition.y += cameraDistanceUp;

        // ����������� ��������� �����, � ��� �������� ������:
        Vector3 cameraTargetPosition = characterController.transform.position; cameraTargetPosition.y += 2f;

        // ����������� ��������� ������ �� ��������
        currentCameraY -= Input.GetAxis("Mouse Y") * 0.3f;
        currentCameraY = Mathf.Clamp(currentCameraY, -3, 3);
        cameraPosition.y += currentCameraY;

        // ����������� �������� (���������) ������:
        Quaternion cameraRotation = Quaternion.LookRotation(cameraTargetPosition - cameraPosition);

        // ����������� ��������� �� ��������� �� ���������� ������:
        
        _camera.transform.position =
        Vector3.Lerp(_camera.transform.position, cameraPosition, cameraSmoothing);
        _camera.transform.rotation =
        Quaternion.Lerp(_camera.transform.rotation, cameraRotation, cameraSmoothing);
    }
}