using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    private Animator animator;
    private bool onGround = false;
    private float jumpForce = 200f;
    private Rigidbody _rigidbody;
    private float maxSlope = 30f;   //Максимальный уклон, по которому может идти персонаж

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
        

    //Коллайдер персонажа прекращает взаимодействие с каким-то другим коллайдером
    private void OnCollisionExit(Collision collision)
    {
        onGround = false;
    }
    //Коллайдер персонажа начинает взаимодействие с каким-то другим коллайдером
    private void OnCollisionStay(Collision collision)
    {
        onGround = CheckIsOnGround(collision);
    }


    //Проверяем, подходит ли поверхность коллайдера для того, чтобы персонаж на ней стоял.
    //Объект Collision для проверки.
    //return true, если поверхность подходящая, false - если нет.
    private bool CheckIsOnGround(Collision collision)
    {
        for (int i = 0; i < collision.contacts.Length; i++) //Проверяем все точки соприкосновения
        {
            if (collision.contacts[i].point.y < transform.position.y)   //если точка соприкосновения находится ниже центра нашего персонажа
            {
                if (Vector3.Angle(collision.contacts[i].normal, Vector3.up) < maxSlope)   //Если уклон поверхности не превышает допустимое значение
                {
                    return true;    //найдена точка соприкосновения с подходящей поверхностью - выходим из функции, возвращаем значение true.
                }
            }
        }
        return false;   //Подходящая поверхность не найдена, возвращаем значение false.
    }

    //зчитуємо поточні натискання WSAD
    public void ApplyMovingForce()
    {
        animator.SetFloat("vSpeed", Input.GetAxis("Vertical"));
        animator.SetFloat("hSpeed", Input.GetAxis("Horizontal"));
    }
       
    public void ApplyJump()
    {
        if (onGround)  //якщо стоїмо на землі
        {
            animator.applyRootMotion = true;

            if (Input.GetKeyDown(KeyCode.Space))
            //Якщо гравець натиснув "пробіл"
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