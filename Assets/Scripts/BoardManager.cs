using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public int colums = 8;          //�c���̉���
    public int rows = 8;            //�����̉���
    private List<Vector3> gridPositions = new List<Vector3>();  //�|�W�V�����ݒ肷�邽�߂̃��X�g
    public GameObject[] floorTiles;     //floor�̃I�u�W�F�N�g
    public GameObject[] wallTiles;      //wall�̃I�u�W�F�N�g
    public GameObject[] foodTiles;      //food�̃I�u�W�F�N�g
    public GameObject[] outwallTiles;   //outwall�̃I�u�W�F�N�g
    public GameObject[] enemyTiles;     //enemy�̃I�u�W�F�N�g

    public GameObject Exit;             //�o��

    public int wallMinmum = 5;          //����ǂ̍Œ�z�u��
    public int wallMaxmum = 9;          //����ǂ̍ő�z�u��
    public int foodMinmum = 1;          //�A�C�e���̍Œ�z�u��
    public int foodMaxmum = 3;          //�A�C�e���̍Œ�z�u��


    void InitialiseList()       //���X�g�̒��g���폜���ĐV�����|�W�V�����̎擾
    {
        gridPositions.Clear();  //���X�g�̍폜
        for (int x = 1; x < colums - 1; x++)    //1-1����6-1�܂Ŕ͈͂�錾�i��j
        {
            for (int y = 1; y < rows - 1; y++)  //1-1����1-6�܂Ŕ͈͂�錾�i�s�j
            {
                gridPositions.Add(new Vector3(x, y, 0));    //for�ŏ������͈͂��擾����
            }
        }
    }

    void BoardSetup()       //map����
    {
        for(int x=-1;x<colums+1;x++)        //colums��0~7�̉��͈͂ŕǂ̐������K�v��-1~9�ɂ��邽�߂�x=-1,colums+1
        {
            for(int y=-1;y<rows+1;y++)      //rows��0~7�̉��͈͂ŕǂ̐������K�v��-1~9�ɂ��邽�߂�x=-1,rows+1
            {
                GameObject toInstantiate;   //�I�u�W�F�N�g�̐���

                if(x==-1||x==colums||y==-1||y==rows)    //�|�W�V������x��-1,9   y��-1,9�̂ǂꂩ��ł����Ă͂܂�����ȉ�
                {
                    toInstantiate = outwallTiles[Random.Range(0,outwallTiles.Length)];  //if�̒��g���ǂꂩ�����Ă�����O�ǂ𐶐�
                }
                else
                {
                    toInstantiate=floorTiles[Random.Range(0,floorTiles.Length)];        //����Ȃ���Ώ��𐶐�
                }
                Instantiate(toInstantiate, new Vector3(x, y, 0), Quaternion.identity);  //���ƕǂ̐���
            }
        }
    }

    Vector3 RandomPosition()    //�ݒu���������_���Ƀ|�W�V��������点��
    {
        int randomIndex=Random.Range(0,gridPositions.Count);    //randomIndex�Ƃ����ϐ���錾�A���̒l��0~gridPositions�̗v�f���������_���Ƃ��ďo��

        Vector3 randamPosition=gridPositions[randomIndex];      //���Ń����_�����擾�B���̃����_���Ȑ����ɓ����Ă�v�f��Vector3�ɂȂ�B

        gridPositions.RemoveAt(randomIndex);                    //�����ꏊ�ɃA�C�e�������Ȃ��悤�ɁA���̃|�W�V�����������B

        return randamPosition;
    }

    void LayoutobjectRandom(GameObject[]tileArray,int min,int max)      //randamPosition���g���ăA�C�e���������_���ɐ���������B
    {
        int objectCount=Random.Range(min,max+1);        //objectCount�������_����

        for (int i=0;i<objectCount;i++)         
        {
            Vector3 randomPosition=RandomPosition();        //�A�C�e���̏o���̏ꏊ�������_����

            GameObject tileChoice = tileArray[Random.Range(0,tileArray.Length)];    //�o�Ă���A�C�e���������_����

            Instantiate(tileChoice,randomPosition,Quaternion.identity);     //�A�C�e����󂹂����o��������
        }
    }

    public void SetupScene(int level)       //���܂ō�����֐��̌Ăяo��
    {
        BoardSetup();
        InitialiseList();
        LayoutobjectRandom(wallTiles, wallMinmum, wallMaxmum);      //�󂹂�ǂ��Ăяo��
        LayoutobjectRandom(foodTiles,foodMinmum,foodMaxmum);        //�A�C�e�����Ăяo��

        int enemtCount = (int)Mathf.Log(level,2f);
        LayoutobjectRandom(enemyTiles, enemtCount,enemtCount);      //enemy���Ăяo��
        Instantiate(Exit, new Vector3(colums - 1, rows - 1, 0), Quaternion.identity);//�o�����E��ɌĂяo��
    }
}
