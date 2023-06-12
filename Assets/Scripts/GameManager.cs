using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager instance; //GameManager���ǂ��ł��Ăяo����悤�ɂ���
    BoardManager boardManager;          //BoardManager��ϐ��錾

    public bool playerTurn = true;      //player�^�[�����Ɛ錾
    public bool enemiesMoving=false;    //enemy�^�[���ł͂Ȃ��Ɛ錾

    public int level = 1;       //level = 1��錾
    private bool doingSetup;    //�^�U��doingSetup��錾
    public Text levelText;      //levelText���A�^�b�`�o����悤�ɂɐ錾
    public GameObject levelImage;   //levelImage���A�^�b�`�o����悤�ɂɐ錾

    public int food = 100;      //�̗͂̏����l��100�ɐ錾

    private List<Enemy> enemies;
    private void Awake()    //��ԍŏ��ɌĂяo�����
    {
        if(instance == null)    //instance�̑��݂��m�F
        {
            instance = this;    //����������awake
        }
        else if(instance != this)   
        {
            Destroy(gameObject);    //�����Q�[���}�l�[�W���[����������j��
        }
        DontDestroyOnLoad(gameObject);//Scene���؂�ւ���ꂽ�Ƃ��j�󂳂�Ȃ�

        enemies = new List<Enemy>();

        boardManager=GetComponent<BoardManager>();  //boardManager��BoardManager���擾

        InitGame();
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]static public void Call()//�Q�[�����J�n������1�x�����Ăяo���i�V�[�������[�h������
    {
        SceneManager.sceneLoaded += OnSceneLoaded;//���[�h���邽�т�SceneManager���Ăяo��
    }

    static private void OnSceneLoaded(Scene next,LoadSceneMode a)//�V�[�������[�h������
    {
        //static������Ƃ��Ăяo�����ł��Ȃ�����instance�ŌĂяo����
        instance.level++;       //�V�[�������[�h���邽�т�level��+1����B
        instance.InitGame();//map�����̌Ăяo��
    }

    public void InitGame()  
    {
        doingSetup = true;//true�̂Ƃ�player�������Ȃ��悤�ɂ���
        levelImage = GameObject.Find("LevelImage");//LevelImage�Ƃ����I�u�W�F�N�g��T��
        levelText = GameObject.Find("LevelText").GetComponent<Text>();//LevelText�Ƃ����I�u�W�F�N�g��T��Text���擾
        levelText.text = "Day" + level;//���[�h���邽�т�n���ƕ\������
        levelImage.SetActive(true);
        Invoke("HideLevelImage", 2f);

        enemies.Clear();

        boardManager.SetupScene(level);     //boardManager���Ăяo��
    }

    public void HideLevelImage()
    {
        levelImage.SetActive(false);
        doingSetup = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(playerTurn||enemiesMoving||doingSetup)//��Ő錾�����Ƃ����player�^�[����enemy�^�[������Ȃ�������
        {
            return;//���̂܂�
        }
        StartCoroutine(MoveEnemies());//player�^�[���łȂ�������enemy�^�[��
    }

    public void AddEnemy(Enemy script)
    {
        enemies.Add(script);
    }

    public void DestroyEnemy(Enemy script)
    {
        enemies.Remove(script);
    }
    IEnumerator MoveEnemies()
    {
        enemiesMoving=true;//enemy�^�[��

        yield return new WaitForSeconds(0.1f);//�������񏈗����~�߂�A0.1�b��ɍĊJ

        if(enemies.Count==0)
        {
            yield return new WaitForSeconds(0.1f);
        }

        for(int i=0; i<enemies.Count; i++)
        {
            enemies[i].MoveEnemy();

            yield return new WaitForSeconds(0.1f);
        }

        playerTurn = true;//player�^�[���ɂ���

        enemiesMoving = false;//enemy�^�[���̏I��

    }

    public void GameOver()
    {
        levelText.text = "GameOver";
        levelImage.SetActive(true );
        enabled = false;
    }
}
