using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public struct CourtIdx{
    public int x;
    public int y;

}


public class CourtManage : MonoBehaviour {

    // 場地資訊
    private int CourtWidth = 64;
    private int CourtLength = 140;
    private float GridSize = 1;
    public int widthOffset;
    public int lengthOffset;

    // 場地物件

    private int[,] Court;
    public GameObject floorUnit;
    private GameObject[,] CourtFloor;
   
     
    // 地面顏色
    public Material M_colorless;
    public Material M_blue;
    public Material M_orange;
    public Material M_gray;
    public Material M_purple;

    private const int COLORLESS = 0;
    private const int BLUE = 1;
    private const int ORANGE = 2;
    private const int PURPLE = 3;

    // 球員物件
    public GameObject AtkPlayer_1;
    public GameObject AtkPlayer_2;
    public GameObject AtkPlayer_3;
    public GameObject AtkPlayer_4;

    public GameObject DefPlayer_1;
    public GameObject DefPlayer_2;
    public GameObject DefPlayer_3;
    public GameObject DefPlayer_4;

    // 球員代號
    private const int NoPlayer = 0;
    private const int A1 = 1;
    private const int A2 = 2;
    private const int A3 = 3;
    private const int A4 = 4;

    private const int D1 = 5;
    private const int D2 = 6;
    private const int D3 = 7;
    private const int D4 = 8;


    // 球員座標 ( CrtIdx = Court Index )

    private CourtIdx CrtIdx_A1;
    private CourtIdx CrtIdx_A2;
    private CourtIdx CrtIdx_A3;
    private CourtIdx CrtIdx_A4;
    private CourtIdx CrtIdx_D1;
    private CourtIdx CrtIdx_D2;
    private CourtIdx CrtIdx_D3;
    private CourtIdx CrtIdx_D4;



    private int EastSite;
    private int WestSite;
    private int NorthSite;
    private int SouthSite;


    // 事件類別
    public EventManage eventManger;
    


    // Use this for initialization
    void Start () {

        // Create 後台地面網格(Court) 跟 實體地面網格(CourtFloor)
        Court = new int[CourtWidth,CourtLength];
        CourtFloor = new GameObject[CourtWidth,CourtLength];

 

        widthOffset = CourtWidth / 2;
        lengthOffset = CourtLength / 2;


        // 設定實體地面網格


        for (int i = 0; i < CourtWidth; i++)
        {
            for (int j = 0; j < CourtLength; j++)
            {

                // 與實體地面網格連結
                CourtFloor[i, j] = Instantiate(floorUnit);

                // 設置CourtFloor.transform.posistion
                float fx = (float)(GridSize*(i+0.5- widthOffset));
                float fz = (float)(GridSize*(j+0.5- lengthOffset));
                CourtFloor[i, j].transform.position = new Vector3(fx,0.225f,fz);

                // Initialize material of CourtFloor[i, j] into 透明色材質
                CourtFloor[i, j].GetComponent<Renderer>().material = M_colorless;
                CourtFloor[i, j].GetComponent<Renderer>().enabled = false;
            }
        }


        // 設定事件管理員的球員物件
        eventManger.SetupPlayerGameObj(AtkPlayer_1, A1);
        eventManger.SetupPlayerGameObj(AtkPlayer_2, A2);
        eventManger.SetupPlayerGameObj(AtkPlayer_3, A3);
        eventManger.SetupPlayerGameObj(AtkPlayer_4, A4);
        eventManger.SetupPlayerGameObj(DefPlayer_1, D1);
        eventManger.SetupPlayerGameObj(DefPlayer_2, D2);
        eventManger.SetupPlayerGameObj(DefPlayer_3, D3);
        eventManger.SetupPlayerGameObj(DefPlayer_4, D4);

    }


	// Update is called once per frame
	void Update () {


        UpdatePlayerPosition();

        ColorCourt(3,false);

        EventDetect();



    }



    // 更新球場上各球員的座標
    private void UpdatePlayerPosition()
    {
    

        // 清除上一Frame的球員座標
        Court[CrtIdx_A1.x, CrtIdx_A1.y] = NoPlayer;
        Court[CrtIdx_A2.x, CrtIdx_A2.y] = NoPlayer;
        Court[CrtIdx_A3.x, CrtIdx_A3.y] = NoPlayer;
        Court[CrtIdx_A4.x, CrtIdx_A4.y] = NoPlayer;

        Court[CrtIdx_D1.x, CrtIdx_D1.y] = NoPlayer;
        Court[CrtIdx_D2.x, CrtIdx_D2.y] = NoPlayer;
        Court[CrtIdx_D3.x, CrtIdx_D3.y] = NoPlayer;
        Court[CrtIdx_D4.x, CrtIdx_D4.y] = NoPlayer;


        // 輸入這一Frame的球員座標
        CrtIdx_A1 = Position2CourtIdx(AtkPlayer_1.transform.position);
        CrtIdx_A2 = Position2CourtIdx(AtkPlayer_2.transform.position);
        CrtIdx_A3 = Position2CourtIdx(AtkPlayer_3.transform.position);
        CrtIdx_A4 = Position2CourtIdx(AtkPlayer_4.transform.position);

        CrtIdx_D1 = Position2CourtIdx(DefPlayer_1.transform.position);
        CrtIdx_D2 = Position2CourtIdx(DefPlayer_2.transform.position);
        CrtIdx_D3 = Position2CourtIdx(DefPlayer_3.transform.position);
        CrtIdx_D4 = Position2CourtIdx(DefPlayer_4.transform.position);


        Court[CrtIdx_A1.x, CrtIdx_A1.y] = A1;
        Court[CrtIdx_A2.x, CrtIdx_A2.y] = A2;
        Court[CrtIdx_A3.x, CrtIdx_A3.y] = A3;
        Court[CrtIdx_A4.x, CrtIdx_A4.y] = A4;

        Court[CrtIdx_D1.x, CrtIdx_D1.y] = D1;
        Court[CrtIdx_D2.x, CrtIdx_D2.y] = D2;
        Court[CrtIdx_D3.x, CrtIdx_D3.y] = D3;
        Court[CrtIdx_D4.x, CrtIdx_D4.y] = D4;


        // 更新極東、極西、極男、極北座標

        EastSite = MaxOf8(CrtIdx_A1.x, CrtIdx_A2.x, CrtIdx_A3.x, CrtIdx_A4.x, CrtIdx_D1.x, CrtIdx_D2.x, CrtIdx_D3.x, CrtIdx_D4.x);
        WestSite = MinOf8(CrtIdx_A1.x, CrtIdx_A2.x, CrtIdx_A3.x, CrtIdx_A4.x, CrtIdx_D1.x, CrtIdx_D2.x, CrtIdx_D3.x, CrtIdx_D4.x);
        NorthSite = MaxOf8(CrtIdx_A1.y, CrtIdx_A2.y, CrtIdx_A3.y, CrtIdx_A4.y, CrtIdx_D1.y, CrtIdx_D2.y, CrtIdx_D3.y, CrtIdx_D4.y);
        SouthSite = MinOf8(CrtIdx_A1.y, CrtIdx_A2.y, CrtIdx_A3.y, CrtIdx_A4.y, CrtIdx_D1.y, CrtIdx_D2.y, CrtIdx_D3.y, CrtIdx_D4.y);


    }


    // 以球場上各球員的座標觀察是否有接觸事件發生
    private void EventDetect(){



        // 檢查進攻球員 A1
        CheckPlayerAround(A1, CrtIdx_A1, 1);

        // 檢查進攻球員 A2
        CheckPlayerAround(A2, CrtIdx_A2, 1);

        // 檢查進攻球員 A3
        CheckPlayerAround(A3, CrtIdx_A3, 1);

        // 檢查進攻球員 A4
        CheckPlayerAround(A4, CrtIdx_A4, 1);

        // 檢查防守球員 D1
        CheckPlayerAround(D1, CrtIdx_D1, 1);

        // 檢查防守球員 D2
        CheckPlayerAround(D2, CrtIdx_D2, 1);

        // 檢查防守球員 D3
        CheckPlayerAround(D3, CrtIdx_D3, 1);

        // 檢查防守球員 D4
        CheckPlayerAround(D4, CrtIdx_D4, 1);



    }

    
    private void CheckPlayerAround(int playerCode,CourtIdx playerCrtIdx,int range){


        int[] adjStatus = { 0, 0, 0, 0, 0, 0, 0, 0, 0 };


        for (int i = playerCrtIdx.x - range; i <= playerCrtIdx.x + range; i++){
            for (int j = playerCrtIdx.y - range; j <= playerCrtIdx.y + range;j++){
            
                if(Court[i,j] != NoPlayer && Court[i, j] != playerCode){ // 靈氣範圍內有其他球員

                    // 相鄰矩陣設定
                    adjStatus[Court[i, j]] = 1;


                }

            }
        }



        eventManger.AdjRegister(playerCode,adjStatus);


    }



    // 球場地板染色
    public void ColorCourt(int range , bool ALLCourt){

        if(ALLCourt){

            for (int i = 0; i < CourtWidth; i++)
            {
                for (int j = 0 ; j < CourtLength; j++)
                {
                    CourtFloor[i, j].GetComponent<Renderer>().enabled = false;
                    CourtFloor[i, j].GetComponent<Renderer>().material = M_colorless;
                }
            }

        }
        else{
            for (int i = WestSite - range; i <= EastSite + range; i++)
            {
                for (int j = SouthSite - range; j < NorthSite + range; j++)
                {
                    ColorFloor(i, j, range);
                }
            }
        }


    }


    // 單一地板染色
    private void ColorFloor(int x,int y,int windowSize){


        int colorType = 0;
        int CourtContent = 0;
        int length = ( windowSize - 1 )/ 2 ;


        for (int i = x - length; i <= x + length; i++)
        {
            for (int j = y - length; j <= y + length; j++)
            {

                CourtContent = Court[i, j];

                if (A1 <= CourtContent && CourtContent <= A4)
                {
                    colorType = colorType | BLUE;
                }
                else if (D1 <= CourtContent && CourtContent <= D4)
                {
                    colorType = colorType | ORANGE;
                }

            }
        }


        switch (colorType)
        {
            case BLUE:
                CourtFloor[x, y].GetComponent<Renderer>().enabled = true;
                CourtFloor[x, y].GetComponent<Renderer>().material = M_blue;
                break;
            case ORANGE:
                CourtFloor[x, y].GetComponent<Renderer>().enabled = true;
                CourtFloor[x, y].GetComponent<Renderer>().material = M_orange;
                break;
            case PURPLE:
                CourtFloor[x, y].GetComponent<Renderer>().enabled = true;
                CourtFloor[x, y].GetComponent<Renderer>().material = M_purple;
                break;
            case COLORLESS:
                CourtFloor[x, y].GetComponent<Renderer>().enabled = false;
                CourtFloor[x, y].GetComponent<Renderer>().material = M_colorless;
                break;
            default:
                break;
        }


    }


    // 輔助函數： Unit世界位置轉換成球場index
    private CourtIdx Position2CourtIdx( Vector3 position ){

        CourtIdx playerIdx = new CourtIdx();

        playerIdx.x = (int)(Mathf.Floor(position.x / GridSize) + widthOffset);
        playerIdx.y = (int)(Mathf.Floor(position.z / GridSize) + lengthOffset);


        return playerIdx;
    }

    // 輔助函數：
    private int MaxOf8(int x1,int x2,int x3,int x4,int x5,int x6,int x7,int x8){

        return new[] { x1,x2,x3,x4,x5,x6,x7,x8 }.Max();

    }

    // 輔助函數：
    private int MinOf8(int x1, int x2, int x3, int x4, int x5, int x6, int x7, int x8)
    {

        return new[] { x1, x2, x3, x4, x5, x6, x7, x8 }.Min();

    }





    // 對外介面(Get)


    public CourtIdx GetPlayerCourtIdx(int playerCode){


        CourtIdx error;

        error.x = error.y = -1;

        switch (playerCode)
        {
            case A1:
                return CrtIdx_A1;
            case A2:
                return CrtIdx_A2;
            case A3:
                return CrtIdx_A3;
            case A4:
                return CrtIdx_A4;
            case D1:
                return CrtIdx_D1;
            case D2:
                return CrtIdx_D2;
            case D3:
                return CrtIdx_D3;
            case D4:
                return CrtIdx_D4;
            default:
                return error;
        }



    }



}
