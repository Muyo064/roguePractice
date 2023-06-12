using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour
{
    public GameObject gamemanager;      //gameManager�̎擾�ɕϐ�
    public SoundManager soundmanager;   //soundManager�̎擾�ɕϐ�

    public void Awake()
    {
        if(GameManager.instance==null)  //GameManager��������
        {
            Instantiate(gamemanager);   //gameManager���擾
        }
        if(SoundManager.instance==null) //SoundManager��������
        {
            Instantiate(soundmanager);  //soundManager���擾
        }
    }
}
