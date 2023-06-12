using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager instance; //GameManagerをどこでも呼び出せるようにした
    BoardManager boardManager;          //BoardManagerを変数宣言

    public bool playerTurn = true;      //playerターンだと宣言
    public bool enemiesMoving=false;    //enemyターンではないと宣言

    public int level = 1;       //level = 1を宣言
    private bool doingSetup;    //真偽のdoingSetupを宣言
    public Text levelText;      //levelTextをアタッチ出来るようにに宣言
    public GameObject levelImage;   //levelImageをアタッチ出来るようにに宣言

    public int food = 100;      //体力の初期値を100に宣言

    private List<Enemy> enemies;
    private void Awake()    //一番最初に呼び出される
    {
        if(instance == null)    //instanceの存在を確認
        {
            instance = this;    //無かったらawake
        }
        else if(instance != this)   
        {
            Destroy(gameObject);    //複数ゲームマネージャーが合ったら破壊
        }
        DontDestroyOnLoad(gameObject);//Sceneが切り替えられたとき破壊されない

        enemies = new List<Enemy>();

        boardManager=GetComponent<BoardManager>();  //boardManagerがBoardManagerを取得

        InitGame();
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]static public void Call()//ゲームを開始したら1度だけ呼び出し（シーンをロードした後
    {
        SceneManager.sceneLoaded += OnSceneLoaded;//ロードするたびにSceneManagerを呼び出し
    }

    static private void OnSceneLoaded(Scene next,LoadSceneMode a)//シーンをロードしたら
    {
        //staticがあるとき呼び出しができないけどinstanceで呼び出せる
        instance.level++;       //シーンをロードするたびにlevelが+1する。
        instance.InitGame();//map生成の呼び出し
    }

    public void InitGame()  
    {
        doingSetup = true;//trueのときplayerが動かないようにする
        levelImage = GameObject.Find("LevelImage");//LevelImageというオブジェクトを探す
        levelText = GameObject.Find("LevelText").GetComponent<Text>();//LevelTextというオブジェクトを探しTextを取得
        levelText.text = "Day" + level;//ロードするたびにn日と表示する
        levelImage.SetActive(true);
        Invoke("HideLevelImage", 2f);

        enemies.Clear();

        boardManager.SetupScene(level);     //boardManagerを呼び出し
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
        if(playerTurn||enemiesMoving||doingSetup)//上で宣言したとおりにplayerターンでenemyターンじゃなかったら
        {
            return;//そのまま
        }
        StartCoroutine(MoveEnemies());//playerターンでなかったらenemyターン
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
        enemiesMoving=true;//enemyターン

        yield return new WaitForSeconds(0.1f);//いったん処理を止める、0.1秒後に再開

        if(enemies.Count==0)
        {
            yield return new WaitForSeconds(0.1f);
        }

        for(int i=0; i<enemies.Count; i++)
        {
            enemies[i].MoveEnemy();

            yield return new WaitForSeconds(0.1f);
        }

        playerTurn = true;//playerターンにして

        enemiesMoving = false;//enemyターンの終了

    }

    public void GameOver()
    {
        levelText.text = "GameOver";
        levelImage.SetActive(true );
        enabled = false;
    }
}
