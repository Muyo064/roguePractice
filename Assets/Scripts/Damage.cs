using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{

    public int wallHp = 3;  //�ǂ�hp
    public Sprite dmgWall;  //dmgwall�̐錾 �Ђт��������ǂ��Ăяo�����߂�


    public int enemyHp = 5; //�G��hp
    private Enemy enemy;    

    private SpriteRenderer spriteRenderer;//spriteRenderer�錾

    public AudioClip audioClip1;
    public AudioClip audioClip2;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer=GetComponent<SpriteRenderer>();  //spriteRenderer���擾
        enemy =GetComponent<Enemy>();
    }

    public void AttckDamage(int loss)   //player�ɍU�����ꂽ���Ăяo������
    {
        if(gameObject.CompareTag("Wall"))
        {
            SoundManager.instance.RandomSE(audioClip1,audioClip2);

            spriteRenderer.sprite = dmgWall;    //�Ђѓ������ǂ̌Ăяo��

            wallHp -= loss; //loss��player�̍U����

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
