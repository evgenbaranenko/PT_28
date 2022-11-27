using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    private Animator animator;
    private bool onGround = false;
    private float jumpForce = 200f;
    private Rigidbody _rigidbody;
    private float maxSlope = 30f;   //������������ �����, �� �������� ����� ���� ��������

    private void Awake()
    {
        animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
    }
    void Start()
    {

    }
    private void FixedUpdate()
    {
        ApplyMovingForce();
    }
        

    //��������� ��������� ���������� �������������� � �����-�� ������ �����������
    private void OnCollisionExit(Collision collision)
    {
        onGround = false;
    }
    //��������� ��������� �������� �������������� � �����-�� ������ �����������
    private void OnCollisionStay(Collision collision)
    {
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
        animator.SetFloat("vSpeed", Input.GetAxis("Vertical"));
        animator.SetFloat("hSpeed", Input.GetAxis("Horizontal"));
    }
       
    public void ApplyJump()
    {
        if (onGround)  //���� ����� �� ����
        {
            animator.applyRootMotion = true;

            if (Input.GetKeyDown(KeyCode.Space))
            //���� ������� �������� "�����"
            {
                animator.SetTrigger("jump");

                _rigidbody?.AddForce(Vector3.up * jumpForce); Debug.Log(_rigidbody);
            }

            animator.SetBool("inAir", false);
        }
        else
        {
            transform.Translate(new
            Vector3(Input.GetAxis("Horizontal") * 0.1f, 0, Input.GetAxis("Vertical") * 0.1f));

            animator.applyRootMotion = false;

            animator.SetBool("inAir", true);
        }
    }
           
                
    void Update()
    {
        ApplyJump();
        Debug.Log(_rigidbody);
    }
}