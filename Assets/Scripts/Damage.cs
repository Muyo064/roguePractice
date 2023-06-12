using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{

    public int wallHp = 3;  //•Ç‚Ìhp
    public Sprite dmgWall;  //dmgwall‚ÌéŒ¾ ‚Ğ‚Ñ‚ª“ü‚Á‚½•Ç‚ğŒÄ‚Ño‚·‚½‚ß‚Ì


    public int enemyHp = 5; //“G‚Ìhp
    private Enemy enemy;    

    private SpriteRenderer spriteRenderer;//spriteRendereréŒ¾

    public AudioClip audioClip1;
    public AudioClip audioClip2;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer=GetComponent<SpriteRenderer>();  //spriteRenderer‚ğæ“¾
        enemy =GetComponent<Enemy>();
    }

    public void AttckDamage(int loss)   //player‚ÉUŒ‚‚³‚ê‚½ŒÄ‚Ño‚µ‚½‚¢
    {
        if(gameObject.CompareTag("Wall"))
        {
            SoundManager.instance.RandomSE(audioClip1,audioClip2);

            spriteRenderer.sprite = dmgWall;    //‚Ğ‚Ñ“ü‚Á‚½•Ç‚ÌŒÄ‚Ño‚µ

            wallHp -= loss; //loss‚Íplayer‚ÌUŒ‚—Í

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
