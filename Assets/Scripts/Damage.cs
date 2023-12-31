using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{

    public int wallHp = 3;  //壁のhp
    public Sprite dmgWall;  //dmgwallの宣言 ひびが入った壁を呼び出すための


    public int enemyHp = 5; //敵のhp
    private Enemy enemy;    

    private SpriteRenderer spriteRenderer;//spriteRenderer宣言

    public AudioClip audioClip1;
    public AudioClip audioClip2;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer=GetComponent<SpriteRenderer>();  //spriteRendererを取得
        enemy =GetComponent<Enemy>();
    }

    public void AttckDamage(int loss)   //playerに攻撃された時呼び出したい
    {
        if(gameObject.CompareTag("Wall"))
        {
            SoundManager.instance.RandomSE(audioClip1,audioClip2);

            spriteRenderer.sprite = dmgWall;    //ひび入った壁の呼び出し

            wallHp -= loss; //lossはplayerの攻撃力

            if(wallHp<= 0)
            {
                gameObject.SetActive(false);
            }
        }
        else if(gameObject.CompareTag("Enemy"))
        {
            SoundManager.instance.RandomSE(audioClip1, audioClip2);
            enemyHp -= loss;
            if(enemyHp<= 0)
            {
                enemy.Death();
            }
        }
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
