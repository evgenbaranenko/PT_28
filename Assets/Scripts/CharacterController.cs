using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    private Animator _animator;
    private bool onGround = false;
    private float jumpForce = 200f;
    private Rigidbody _rigidbody;
    private float maxSlope = 30f;   //������������ �����, �� �������� ����� ���� ��������
    private GameObject gun;
    public GameObject Gun { get; set; }

    [SerializeField] private Transform RightHandPlace;
    [SerializeField] private Transform LeftHandPlace;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
        gun = GameObject.Find("Gun"); 
    }
    void Start()
    {

    }
    private void FixedUpdate()
    {
        ApplyMovingForce();
    }
    void Update()
    {

        ApplyJump();

    }

    public void OnAnimatorIK()
    {
        //////˳�� ����:

        //������������ ���� ��������� �� ���������
        _animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
        _animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
        //���������� ��������� �� ���������
        _animator.SetIKPosition(AvatarIKGoal.LeftHand, LeftHandPlace.position);
        _animator.SetIKRotation(AvatarIKGoal.LeftHand, LeftHandPlace.rotation);
        //////����� ����:
        //������������ ����
        _animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
        _animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
        //������������ ��������� �� ���������
        _animator.SetIKPosition(AvatarIKGoal.RightHand, RightHandPlace.position);
        _animator.SetIKRotation(AvatarIKGoal.RightHand, RightHandPlace.rotation);

        //////������:

        //������������ ����
        _animator.SetLookAtWeight(0);
        // ������ ������ �������
        _animator.SetLookAtPosition(gun.transform.position + (gun.transform.forward * 10));
    }

    //��������� ��������� ���������� �������������� � �����-�� ������ �����������
    private void OnCollisionExit(Collision collision)
    {
        onGround = false;
    }
    //��������� ��������� �������� �������������� � �����-�� ������ �����������
    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.CompareTag("Ground"))
            onGround = CheckIsOnGround(collision);
    }

    //���������, �������� �� ����������� ���������� ��� ����, ����� �������� �� ��� �����.
    //������ Collision ��� ��������.
    //return true, ���� ����������� ����������, false - ���� ���.
    private bool CheckIsOnGround(Collision collision)
    {
        for (int i = 0; i < collision.contacts.Length; i++) //��������� ��� ����� ���������������
        {
            if (collision.contacts[i].point.y < transform.position.y)   //���� ����� ��������������� ��������� ���� ������ ������ ���������
            {
                if (Vector3.Angle(collision.contacts[i].normal, Vector3.up) < maxSlope)   //���� ����� ����������� �� ��������� ���������� ��������
                {
                    return true;    //������� ����� ��������������� � ���������� ������������ - ������� �� �������, ���������� �������� true.
                }
            }
        }
        return false;   //���������� ����������� �� �������, ���������� �������� false.
    }

    //������� ������ ���������� WSAD
    public void ApplyMovingForce()
    {
        if (onGround)
        {
            _animator.SetFloat("vSpeed", Input.GetAxis("Vertical"));
            _animator.SetFloat("hSpeed", Input.GetAxis("Horizontal"));
        }
    }

    //������������ �� ��������� ��䳳 ������� (Jump)
    public void ApplyJumpForce()
    {
        _rigidbody?.AddForce(Vector3.up * jumpForce); 
    }
    public void ApplyJump()
    {
        if (onGround)  //���� ����� �� ����
        {
            _animator.applyRootMotion = true;

            if (Input.GetKeyDown(KeyCode.Space))
            //���� ������� �������� "�����"
            {
                _animator.SetTrigger("jump");
            }

            _animator.SetBool("inAir", false);
        }
        else
        {
            transform.Translate(
            new Vector3(Input.GetAxis("Horizontal") * 0.1f, 0, Input.GetAxis("Vertical") * 0.1f));

            _animator.applyRootMotion = false;

            _animator.SetBool("inAir", true);
        }
    }
}