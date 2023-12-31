using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour
{
    public GameObject gamemanager;      //gameManagerの取得に変数
    public SoundManager soundmanager;   //soundManagerの取得に変数

    public void Awake()
    {
        if(GameManager.instance==null)  //GameManagerが無い時
        {
            Instantiate(gamemanager);   //gameManagerを取得
        }
        if(SoundManager.instance==null) //SoundManagerが無い時
        {
            Instantiate(soundmanager);  //soundManagerを取得
        }
    }
}
