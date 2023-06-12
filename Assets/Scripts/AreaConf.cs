using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AreaConf : MonoBehaviour
{
    private int food;
    public Text foodText;
    private int foodpoint = 10;
    private int sodapoint = 20;

    public void Start()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        void OnTriggerExit2DControl(bool param)
        {
            if(collision.tag=="food")
                {
                    
                }
        }

        if(collision.tag=="Enemy")
        {
            
        }
        
            Debug.Log("hit");
    }
}
