using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{

    private Rigidbody2D rd2d;               //Rigidbody2D��錾    
    public float moveTime = 0.1f;           //�ړ����x              ��������
    public bool isMoving = false;           //player�������Ă��邩�̔��f�A�n�߂͓����Ȃ�����false

    public LayerMask blockingLayer;         //blockingLayer��錾
    private BoxCollider2D boxCollider;      //boxCollider��錾

    public int attackDamage = 1;            //�U����
    private Animator animator;              //�U������Ƃ��̃A�j���[�V�������擾�����邽�߂ɐ錾

    private int food;
    public Text foodText;
    private int foodpoint = 10;
    private int sodapoint = 20;

    public AudioClip moveSound1;
    public AudioClip moveSound2;
    public AudioClip eatSound1;
    public AudioClip eatSound2;
    public AudioClip drinkSound1;
    public AudioClip drinkSound2;
    public AudioClip gameOverSound;

    // Start is called before the first frame update
    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>();             //Rigidbody2D���擾
        boxCollider = GetComponent<BoxCollider2D>();    //BoxCollider2D���擾
        animator = GetComponent<Animator>();            //Animator���擾
        food =GameManager.instance.food;
        foodText.text = "Food" + food;
    }

    // Update is called once per frame
    void Update()
    {

        if(!GameManager.instance.playerTurn)
        {
            return;//player�̃^�[���ł͖��������炻�̂܂�
        }

        int horizontal = (int)Input.GetAxisRaw("Horizontal");   //�������͂�horizontal���擾
        int vertical = (int)Input.GetAxisRaw("Vertical");       //�c�����͂�vertical���擾

        if (horizontal != 0)    //���͂���Ă邩�̊m�F
        {
            vertical = 0;   //���ɓ��͂���Ă�Ƃ��c�ɓ����Ȃ��悤�ɂ��鏈��
            if(horizontal == 1)
            {
                transform.localScale =new Vector3(1, 1, 1); //�E�ɓ�������E����
            }
            else if(horizontal ==-1)
            {
                transform.localScale=new Vector3(-1,1,1);   //���ɓ������獶����
            }
        }
        /*if(vertical !=0)    //�}篒ǉ�������s��������炱��������
        {
            horizontal=0;
            if(vertical == 1)
            {
                transform.localScale=new Vector3(1,1,1);
            }
            else if(vertical ==-1)
            {
                transform.localScale=new Vector3(1,-1,1);
            }
        }*/
        else if (vertical != 0) 
        {
            horizontal = 0;     //�c�ɓ��͂���Ă�Ƃ����ɓ����Ȃ��悤�ɂ��鏈��
        }

        if(horizontal != 0||vertical !=0)       //�����󂯎�����l�̔���
        {
            ATMove(horizontal, vertical);       //��������̏����̌Ăяo��
        }
    }

    public void ATMove(int horizontal,int vertical)
    {
        food--;                             //�̗͂��}�C�i�X
        foodText.text = "Food" + food;      //���ł�����̂�\��

        RaycastHit2D hit;                   //�����蔻��̂��߂̐錾

        bool canMove = Move(horizontal, vertical, out hit);//out�͒l���K�������Ƃ����錾

        if(hit.transform==null)     
        {

            GameManager.instance.playerTurn = false;    //hit���Ȃ�������player�^�[���I��

            return;
        }
        Damage hitConponent=hit.transform.GetComponent<Damage>();//�Ԃ������Ώۂ̃|�W�V�������擾���āA�U��

        if(!canMove&&hitConponent!=null)//�|�W�V�����ύX�Ȃ��U������Ȃ�
        {
            OncantMove(hitConponent);//OncantMove����U�����󂯎��
        }
        CheckFood();
        GameManager.instance.playerTurn = false;        //��������^�[���I��
    }
    public bool Move(int horizontal,int vertical,out RaycastHit2D hit)
    {
        Vector2 start=transform.position;                           //���ݒn�̎擾
        Vector2 end=start+new Vector2(horizontal,vertical);         //�ړ���̃|�W�V�������擾

        boxCollider.enabled = false;        //�������^�C�~���O��boxCollider���I�t

        hit = Physics2D.Linecast(start, end, blockingLayer);        //�Ԃ��������̔��f

        boxCollider.enabled=true;

        if(!isMoving && hit.transform==null)        //�����ĂȂ��Ƃ�,�Ԃ����ĂȂ��Ƃ�
        {
            StartCoroutine(Movement(end));      //IEumerator���Ăяo��
            SoundManager.instance.RandomSE(moveSound1, moveSound2);
            return true;
        }
        return false;
    }

    IEnumerator Movement(Vector3 end)       //�^�[���̐���i����
    {
        isMoving = true;                    //player�̃^�[��

        float remainingDistance = (transform.position - end).sqrMagnitude;  //�ړ�����

        while(remainingDistance > float.Epsilon)        //0�ɋ߂��Ȃ�܂Ŏ��s
        {
            transform.position=Vector3.MoveTowards(transform.position,end,1f/moveTime*Time.deltaTime);//�������Ƃ��̎���
            remainingDistance=(transform.position - end).sqrMagnitude;                                

            yield return null;  //�������񏈗��̒�~����while�ɖ߂�
        }
        transform.position=end;     //�ړ���������
        isMoving = false;           //������player�𐧎~������
        CheckFood();
    }

    public void OncantMove(Damage hit)      //�U������
    {
        hit.AttckDamage(attackDamage);

        animator.SetTrigger("Attack");
    }
    private void OnTriggerEnter2D(Collider2D collision)     //item�ƂԂ������Ƃ�
    {
        if(collision.tag == "Food") //food�������Ƃ�
        {
            SoundManager.instance.RandomSE(eatSound1, eatSound2);
            food += foodpoint;  //food��+
            foodText.text = "Food" + food;  //Foodxx    �̂悤�ɂȂ�
            collision.gameObject.SetActive(false);  //�I�u�W�F�N�g�������Ɣ�\��
        }
        else if(collision.tag == "Soda") //soda�������Ƃ�
        {
            SoundManager.instance.RandomSE(drinkSound1, drinkSound2);
            food += sodapoint;
            foodText.text = "Food" + food;
            collision.gameObject.SetActive(false);
        }
        else if (collision.tag == "Exit")//�o����������
        {
            Invoke("Restart", 1f);//1�b��ɕʂ̊K�w�ɍs��
            enabled = false;
        }
    }
    public void Restart()
    {
        SceneManager.LoadScene(0);//map���ēǂݍ���
    }

    public void OnDisable()
    {
        GameManager.instance.food = food;
    }


    private void CheckFood()
    {
        if(food <=0)
        {
            SoundManager.instance.PlaySingle(gameOverSound);
            GameManager.instance.GameOver();
        }
    }

    public void Enemyattack(int loss)
    {
        animator.SetTrigger("Hit");
        food -= loss;
        foodText.text = "-" + loss + "Food" + food;

        CheckFood();
    }
}
