using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour
{
    public GameObject gamemanager;      //gameManager‚Ìæ“¾‚É•Ï”
    public SoundManager soundmanager;   //soundManager‚Ìæ“¾‚É•Ï”

    public void Awake()
    {
        if(GameManager.instance==null)  //GameManager‚ª–³‚¢
        {
            Instantiate(gamemanager);   //gameManager‚ğæ“¾
        }
        if(SoundManager.instance==null) //SoundManager‚ª–³‚¢
        {
            Instantiate(soundmanager);  //soundManager‚ğæ“¾
        }
    }
}
