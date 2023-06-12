using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    public float moveTime = 0.1f;
    public bool isMoving = false;

    public LayerMask blockingLayer;
    private BoxCollider2D boxCollider;

    public int attackDamage = 1;
    private Animator animator;

    private Transform target;
    private bool skipMove = false;

    public AudioClip enemyAttack1;
    public AudioClip enemyAttack2;

    public Text questiontext;

    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();

        target = GameObject.FindGameObjectWithTag("Player").transform;

        GameManager.instance.AddEnemy(this);
    }

    
    public void MoveEnemy()
    {
        if(!skipMove)
        {
            skipMove = true;
            int xdir = 0;
            int ydir = 0;

            if(Mathf.Abs(target.position.x-transform.position.x)<float.Epsilon)
            {
                ydir=target.position.y>transform.position.y ? 1 : -1;
            }
            else
            {
                xdir = target.position.x > transform.position.x ? 1 : -1;
            }
            ATMove(xdir, ydir);
        }
        else
        {
            skipMove=false;
            return;
        }
    }
    // Update is called once per frame
    void Update() 
    {
        
    }
    public void ATMove(int horizontal, int vertical)
    {

        RaycastHit2D hit;

        bool canMove = Move(horizontal, vertical, out hit);

        if (hit.transform == null)
        {
            return;
        }
        Player hitConponent = hit.transform.GetComponent<Player>();

        if (!canMove && hitConponent != null)
        {
            OncantMove(hitConponent);
        }
    }
    public bool Move(int horizontal, int vertical, out RaycastHit2D hit)
    {
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(horizontal, vertical);

        boxCollider.enabled = false;

        hit = Physics2D.Linecast(start, end, blockingLayer);

        boxCollider.enabled = true;

        if (!isMoving && hit.transform == null)
        {
            StartCoroutine(Movement(end));
            return true;
        }
        return false;
    }
    IEnumerator Movement(Vector3 end)
    {
        isMoving = true;

        float remainingDistance = (transform.position - end).sqrMagnitude;

        while (remainingDistance > float.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, end, 1f / moveTime * Time.deltaTime);
            remainingDistance = (transform.position - end).sqrMagnitude;

            yield return null;
        }
        transform.position = end;
        isMoving = false;
    }
    public void OncantMove(Player hit)
    {
        
        hit.Enemyattack(attackDamage);

        animator.SetTrigger("Attack");
        SoundManager.instance.RandomSE(enemyAttack1, enemyAttack2);
    }
    
    public void Death()
    {
        GameManager.instance.DestroyEnemy(this);
        gameObject.SetActive(false);
    }
}
