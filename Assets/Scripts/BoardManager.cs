using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public int colums = 8;          //縦軸の可動域
    public int rows = 8;            //横軸の可動域
    private List<Vector3> gridPositions = new List<Vector3>();  //ポジション設定するためのリスト
    public GameObject[] floorTiles;     //floorのオブジェクト
    public GameObject[] wallTiles;      //wallのオブジェクト
    public GameObject[] foodTiles;      //foodのオブジェクト
    public GameObject[] outwallTiles;   //outwallのオブジェクト
    public GameObject[] enemyTiles;     //enemyのオブジェクト

    public GameObject Exit;             //出口

    public int wallMinmum = 5;          //壊れる壁の最低配置数
    public int wallMaxmum = 9;          //壊れる壁の最大配置数
    public int foodMinmum = 1;          //アイテムの最低配置数
    public int foodMaxmum = 3;          //アイテムの最低配置数


    void InitialiseList()       //リストの中身を削除して新しくポジションの取得
    {
        gridPositions.Clear();  //リストの削除
        for (int x = 1; x < colums - 1; x++)    //1-1から6-1まで範囲を宣言（列）
        {
            for (int y = 1; y < rows - 1; y++)  //1-1から1-6まで範囲を宣言（行）
            {
                gridPositions.Add(new Vector3(x, y, 0));    //forで書いた範囲を取得する
            }
        }
    }

    void BoardSetup()       //map制作
    {
        for(int x=-1;x<colums+1;x++)        //columsが0~7の可動範囲で壁の生成が必要で-1~9にするためのx=-1,colums+1
        {
            for(int y=-1;y<rows+1;y++)      //rowsが0~7の可動範囲で壁の生成が必要で-1~9にするためのx=-1,rows+1
            {
                GameObject toInstantiate;   //オブジェクトの生成

                if(x==-1||x==colums||y==-1||y==rows)    //ポジションでxが-1,9   yが-1,9のどれか一つでも当てはまったら以下
                {
                    toInstantiate = outwallTiles[Random.Range(0,outwallTiles.Length)];  //ifの中身がどれか合っていたら外壁を生成
                }
                else
                {
                    toInstantiate=floorTiles[Random.Range(0,floorTiles.Length)];        //合わなければ床を生成
                }
                Instantiate(toInstantiate, new Vector3(x, y, 0), Quaternion.identity);  //床と壁の生成
            }
        }
    }

    Vector3 RandomPosition()    //設置物をランダムにポジションを取らせる
    {
        int randomIndex=Random.Range(0,gridPositions.Count);    //randomIndexという変数を宣言、その値を0~gridPositionsの要素数をランダムとして出す

        Vector3 randamPosition=gridPositions[randomIndex];      //↑でランダムを取得。そのランダムな数字に入ってる要素がVector3になる。

        gridPositions.RemoveAt(randomIndex);                    //同じ場所にアイテムが被らないように、そのポジションを消す。

        return randamPosition;
    }

    void LayoutobjectRandom(GameObject[]tileArray,int min,int max)      //randamPositionを使ってアイテムをランダムに生成させる。
    {
        int objectCount=Random.Range(min,max+1);        //objectCountをランダムに

        for (int i=0;i<objectCount;i++)         
        {
            Vector3 randomPosition=RandomPosition();        //アイテムの出現の場所をランダムに

            GameObject tileChoice = tileArray[Random.Range(0,tileArray.Length)];    //出てくるアイテムをランダムに

            Instantiate(tileChoice,randomPosition,Quaternion.identity);     //アイテムや壊せる岩を出現させる
        }
    }

    public void SetupScene(int level)       //今まで作った関数の呼び出し
    {
        BoardSetup();
        InitialiseList();
        LayoutobjectRandom(wallTiles, wallMinmum, wallMaxmum);      //壊せる壁を呼び出し
        LayoutobjectRandom(foodTiles,foodMinmum,foodMaxmum);        //アイテムを呼び出し

        int enemtCount = (int)Mathf.Log(level,2f);
        LayoutobjectRandom(enemyTiles, enemtCount,enemtCount);      //enemyを呼び出し
        Instantiate(Exit, new Vector3(colums - 1, rows - 1, 0), Quaternion.identity);//出口を右上に呼び出し
    }
}
