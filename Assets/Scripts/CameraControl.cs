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
    private Quaternion cameraRotation;
    private GameObject gun;

    private Camera _camera;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
        characterController = GetComponentInParent<CharacterController>();
        //gun = characterController.GetComponentInParent<GameObject>(); 
        gun = GameObject.Find("Gun"); Debug.Log(gun);
    }


    void Update()
    {
        CameraController();
        MouseLook();
    }

    /*повертатиме трансформ нашого персонажа в залежності від руху миші по
   осі Х (ліворуч-праворуч):*/
    public void MouseLook()
    {
        characterController.transform.RotateAround(characterController.transform.position,
            Vector3.up, Input.GetAxis("Mouse X") * cameraTurnSpeed);
    }
    public void CameraController()
    {
        // Розраховуємо положення камери:
        Vector3 cameraPosition = characterController.transform.position - (characterController.transform.forward * cameraDistanceForward);
        cameraPosition.y += cameraDistanceUp;

        // Розраховуємо положення точки, в яку дивиться камера:
        Vector3 cameraTargetPosition = characterController.transform.position; cameraTargetPosition.y += 2f;

        // регулювання положення камери по вертикалі
        currentCameraY -= Input.GetAxis("Mouse Y") * 0.3f;
        currentCameraY = Mathf.Clamp(currentCameraY, -3, 3);
        cameraPosition.y += currentCameraY;

        // Розраховуємо напрямок (обертання) камери:
        cameraRotation = Quaternion.LookRotation(cameraTargetPosition - cameraPosition);

        // Застосовуємо положення та обертання до трансформу камери:

        _camera.transform.position =
        Vector3.Lerp(_camera.transform.position, cameraPosition, cameraSmoothing);
        _camera.transform.rotation =
        Quaternion.Lerp(_camera.transform.rotation, cameraRotation, cameraSmoothing);

        //gun.transform.rotation = _camera.transform.rotation;
        //gun.transform.rotation = Quaternion.Euler(cameraTargetPosition);
        //gun.transform.parent.parent.rotation = Quaternion.Euler(cameraTargetPosition);

        /* Ми звертаємося з об'єкту Gun (тип transform), до його
           батька, та до батька з батька (це трансформ WeaponPivot ) */
        //gun.transform.parent.parent.rotation = Quaternion.Euler(_camera.transform.rotation.x,
        //    _camera.transform.rotation.y, _camera.transform.rotation.z);

        gun.transform.parent.parent.rotation = _camera.transform.rotation;
        //gun.transform.parent.parent.rotation.eulerAngles.x = _camera.transform.rotation.eulerAngles.x;

        //gun.transform.rotation =
        //    Quaternion.Euler(1 - cameraRotation.x, 1 - cameraRotation.y, 1 - cameraRotation.z);
    }
}