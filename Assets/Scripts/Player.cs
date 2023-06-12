using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{

    private Rigidbody2D rd2d;               //Rigidbody2Dを宣言    
    public float moveTime = 0.1f;           //移動速度              多分いる
    public bool isMoving = false;           //playerが動いているかの判断、始めは動かないからfalse

    public LayerMask blockingLayer;         //blockingLayerを宣言
    private BoxCollider2D boxCollider;      //boxColliderを宣言

    public int attackDamage = 1;            //攻撃力
    private Animator animator;              //攻撃するときのアニメーションを取得させるために宣言

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
        rd2d = GetComponent<Rigidbody2D>();             //Rigidbody2Dを取得
        boxCollider = GetComponent<BoxCollider2D>();    //BoxCollider2Dを取得
        animator = GetComponent<Animator>();            //Animatorを取得
        food =GameManager.instance.food;
        foodText.text = "Food" + food;
    }

    // Update is called once per frame
    void Update()
    {

        if(!GameManager.instance.playerTurn)
        {
            return;//playerのターンでは無かったらそのまま
        }

        int horizontal = (int)Input.GetAxisRaw("Horizontal");   //横矢印入力をhorizontalが取得
        int vertical = (int)Input.GetAxisRaw("Vertical");       //縦矢印入力をverticalが取得

        if (horizontal != 0)    //入力されてるかの確認
        {
            vertical = 0;   //横に入力されてるとき縦に動かないようにする処理
            if(horizontal == 1)
            {
                transform.localScale =new Vector3(1, 1, 1); //右に動いたら右向く
            }
            else if(horizontal ==-1)
            {
                transform.localScale=new Vector3(-1,1,1);   //左に動いたら左向く
            }
        }
        /*if(vertical !=0)    //急遽追加したやつ不具合合ったらここが原因
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
            horizontal = 0;     //縦に入力されてるとき横に動かないようにする処理
        }

        if(horizontal != 0||vertical !=0)       //矢印を受け取った値の判定
        {
            ATMove(horizontal, vertical);       //動いた後の処理の呼び出し
        }
    }

    public void ATMove(int horizontal,int vertical)
    {
        food--;                             //体力をマイナス
        foodText.text = "Food" + food;      //↑でやったのを表示

        RaycastHit2D hit;                   //当たり判定のための宣言

        bool canMove = Move(horizontal, vertical, out hit);//outは値が必ず入るよという宣言

        if(hit.transform==null)     
        {

            GameManager.instance.playerTurn = false;    //hitしなかったらplayerターン終了

            return;
        }
        Damage hitConponent=hit.transform.GetComponent<Damage>();//ぶつかった対象のポジションを取得して、攻撃

        if(!canMove&&hitConponent!=null)//ポジション変更なく攻撃するなら
        {
            OncantMove(hitConponent);//OncantMoveから攻撃を受け取る
        }
        CheckFood();
        GameManager.instance.playerTurn = false;        //動いたらターン終了
    }
    public bool Move(int horizontal,int vertical,out RaycastHit2D hit)
    {
        Vector2 start=transform.position;                           //現在地の取得
        Vector2 end=start+new Vector2(horizontal,vertical);         //移動後のポジションを取得

        boxCollider.enabled = false;        //動いたタイミングでboxColliderをオフ

        hit = Physics2D.Linecast(start, end, blockingLayer);        //ぶつかったかの判断

        boxCollider.enabled=true;

        if(!isMoving && hit.transform==null)        //動いてないとき,ぶつかってないとき
        {
            StartCoroutine(Movement(end));      //IEumeratorを呼び出す
            SoundManager.instance.RandomSE(moveSound1, moveSound2);
            return true;
        }
        return false;
    }

    IEnumerator Movement(Vector3 end)       //ターンの制作（多分
    {
        isMoving = true;                    //playerのターン

        float remainingDistance = (transform.position - end).sqrMagnitude;  //移動距離

        while(remainingDistance > float.Epsilon)        //0に近くなるまで実行
        {
            transform.position=Vector3.MoveTowards(transform.position,end,1f/moveTime*Time.deltaTime);//距離感とその時間
            remainingDistance=(transform.position - end).sqrMagnitude;                                

            yield return null;  //いったん処理の停止してwhileに戻る
        }
        transform.position=end;     //移動しきった
        isMoving = false;           //そしてplayerを制止させる
        CheckFood();
    }

    public void OncantMove(Damage hit)      //攻撃する
    {
        hit.AttckDamage(attackDamage);

        animator.SetTrigger("Attack");
    }
    private void OnTriggerEnter2D(Collider2D collision)     //itemとぶつかったとき
    {
        if(collision.tag == "Food") //foodだったとき
        {
            SoundManager.instance.RandomSE(eatSound1, eatSound2);
            food += foodpoint;  //foodに+
            foodText.text = "Food" + food;  //Foodxx    のようになる
            collision.gameObject.SetActive(false);  //オブジェクトをちゃんと非表示
        }
        else if(collision.tag == "Soda") //sodaだったとき
        {
            SoundManager.instance.RandomSE(drinkSound1, drinkSound2);
            food += sodapoint;
            foodText.text = "Food" + food;
            collision.gameObject.SetActive(false);
        }
        else if (collision.tag == "Exit")//出口だった時
        {
            Invoke("Restart", 1f);//1秒後に別の階層に行く
            enabled = false;
        }
    }
    public void Restart()
    {
        SceneManager.LoadScene(0);//mapを再読み込み
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
