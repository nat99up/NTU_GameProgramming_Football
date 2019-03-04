using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManage : MonoBehaviour {


    // 球場類別類別
    public CourtManage courtManager;
    public EventManage eventManager;
    public PlayerControl playerControl;

    // 球員及足球物件
    public GameObject[] Players;
    public GameObject Ball;


    // 球員代號
    private const int unused = 0;
    private const int A1 = 1;
    private const int A2 = 2;
    private const int A3 = 3;
    private const int A4 = 4;

    private const int D1 = 5;
    private const int D2 = 6;
    private const int D3 = 7;
    private const int D4 = 8;


    // 陣形
    private Vector3[,] Formation;


    // 字幕
    public Text AtkChanceText;
    public Text RemainingMarksText;
    public Text GameResultText;

    // 計時器
    private int CountDown;

    // 提示線
    public LineHinting hintLine;




    // 比賽資訊
    private int startMarks;     // 本輪四次進攻機會的起始碼數
    private int currentMarks;   // 現在碼數
    private int remainMarks;    // 保持進攻機會的剩餘碼數

    private int attackChance = 4;
    private const int attackChance_total = 4;

    public bool roundOver = false;
    public bool gameOver = false;


    // 長傳好球提示資訊
    public bool HaveGreatPass = false;
    public int GreatPassLength = 4;
    private int oldMarks;



    // 遊戲難度
    private int level = 0;      

    // Use this for initialization
    void Start () {


        startMarks = 70;
        oldMarks = 70;
        currentMarks = 67;

        CountDown = 200;
        GameResultText.text = "Game Start!";
        AtkChanceText.text = string.Format("進攻機會 : {0}/{1}", 4, 4);
        RemainingMarksText.text = string.Format("尚須推進碼數：{0}", 10);



        //陣形編輯器
        const int numOfFormation = 4;
        Formation = new Vector3[numOfFormation, 5];

        Formation[0, 1] = new Vector3(-0.5f, 0, -4);
        Formation[0, 2] = new Vector3(-6.5f, 0, -2);
        Formation[0, 3] = new Vector3(0.5f, 0, -11);
        Formation[0, 4] = new Vector3(6.5f, 0, -2);

        Formation[1, 1] = new Vector3(0.5f, 0, -3);
        Formation[1, 2] = new Vector3(-8.5f, 0, -4.5f);
        Formation[1, 3] = new Vector3(-1.5f, 0, -10);
        Formation[1, 4] = new Vector3(8.5f, 0, -4.5f);

        Formation[2, 1] = new Vector3(-0.5f, 0, -6.5f);
        Formation[2, 2] = new Vector3(-1.25f, 0, -2);
        Formation[2, 3] = new Vector3(6.5f, 0, -7.5f);
        Formation[2, 4] = new Vector3(1.25f, 0, -2);

        Formation[3, 1] = new Vector3(-1.5f, 0, -5.5f);
        Formation[3, 2] = new Vector3(-3, 0, -2.5f);
        Formation[3, 3] = new Vector3(0.5f, 0, -4);
        Formation[3, 4] = new Vector3(6.5f, 0, -1.5f);


    }

    // Update is called once per frame
    void Update () {

        if (CountDown>0){

            CountDown -= 1;

            if (CountDown == 195)
            {
                eventManager.HealthPoint = 100;

            }
            else if (CountDown == 150){
                GameResultText.enabled = true;
                GameResultText.text = "3";
            }
            else if(CountDown == 100){
                GameResultText.text = "2";
            }
            else if(CountDown == 50){
                GameResultText.text = "1";
            }
            else if(CountDown == 10)
            {
                GameResultText.enabled = false;
                roundOver = false;
            }


        }
        else if(CountDown==0){
            CountDown -= 1;
            playerControl.IsLockTime = false;

            if (gameOver)
                SceneManager.LoadScene(0);

        }


        if(gameOver == false){


            if (A1 <= eventManager.BallHolder && eventManager.BallHolder <= A4)
            {
                if(courtManager.GetPlayerCourtIdx(eventManager.BallHolder).y > 0)
                    currentMarks = courtManager.GetPlayerCourtIdx(eventManager.BallHolder).y;

            }
            

            if (currentMarks - oldMarks >= 15)
            {
                GreatPassLength = currentMarks - oldMarks;
                HaveGreatPass = true;

            }

            oldMarks = currentMarks;



            if (currentMarks > 120)
            {
                Win();
            }
            else if (currentMarks > 110)
            {
                SetGameLevel(10);
            }
            else if (currentMarks > 105)
            {
                SetGameLevel(11);
            }
            else if (currentMarks > 100)
            {
                SetGameLevel(12);
            }
            else if (currentMarks > 95)
            {
                SetGameLevel(11);
            }
            else if (currentMarks > 90)
            {
                SetGameLevel(10);
            }
            else if (currentMarks > 85)
            {
                SetGameLevel(9);
            }
            else if (currentMarks > 80)
            {
                SetGameLevel(7);
            }
            else if (currentMarks > 75)
            {
                SetGameLevel(5);
            }
            else if (currentMarks > 70)
            {
                SetGameLevel(3);
            }else{

                SetGameLevel(1);
            }

        }





    }



    public void NewAtkRound()
    {
        roundOver = true;

        attackChance -= 1;

        playerControl.IsLockTime = true;
        playerControl.ResetKeyForPlayers();
        playerControl.ResetDefenseMatching();

        eventManager.HealthPoint = 100;

        remainMarks = 10 - (currentMarks - startMarks);
        remainMarks = (remainMarks > 0) ? remainMarks : 0;

        if (remainMarks > 0 && attackChance == 0)
        {
            Lose();
        }
        else
        {

            CountDown = 160;

            for (int i = A1; i <= D4; i++)
            {
                Players[i].GetComponent<PlayerProperties>().pickingBall = false;
                Players[i].GetComponent<Rigidbody>().velocity = Vector3.zero;
                Players[i].GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            }


            if (remainMarks == 0)
            {
                startMarks = currentMarks;
                remainMarks = 10;
                attackChance = 4;
            }


            AtkChanceText.text = string.Format("進攻機會 : {0}/{1}", attackChance,attackChance_total);
            RemainingMarksText.text = string.Format("尚須推進碼數：{0}", remainMarks);

            Vector3 startLine = new Vector3(0,0.4f,currentMarks-courtManager.lengthOffset);

            int formationNum = ((int)Random.Range(0, 10)) % 4;


            Players[A1].transform.position = startLine + Formation[formationNum, A1];
            Players[A2].transform.position = startLine + Formation[formationNum, A2];
            Players[A3].transform.position = startLine + Formation[formationNum, A3]; ;
            Players[A4].transform.position = startLine + Formation[formationNum, A4];

            Players[D1].transform.position = startLine + new Vector3(-2.5f, 0, 2);
            Players[D2].transform.position = startLine + new Vector3(-7.5f, 0, 4);
            Players[D3].transform.position = startLine + new Vector3(2.5f, 0, 4);
            Players[D4].transform.position = startLine + new Vector3(7.5f, 0, 2);

            int targetPos = startMarks - courtManager.lengthOffset + 10;
            targetPos = targetPos > 50 ? 50 : targetPos;
            hintLine.Shine(targetPos);


        }

        courtManager.ColorCourt(0, true);

        Ball.GetComponent<BallController>().AssignBall(Players[A1]);



    }


    private void SetGameLevel(int newLevel)
    {

        for (int i = D1; i <= D4; i++)
        {
            Players[i].GetComponent<PlayerProperties>().SetTeamProperties("speed",-0.25f * level);
            Players[i].GetComponent<PlayerProperties>().SetTeamProperties("speed",+0.25f * newLevel);
        }

        eventManager.DefenseLevel = 0.8f + 0.1f * level;

        level = newLevel;


    }


    public void Lose(){
        GameResultText.text = "Lose";
        GameResultText.enabled = true;

        playerControl.IsLockTime = true;

        CountDown = 200;
        gameOver = true;


    }


    public void Win()
    {
        GameResultText.text = "Win!";
        GameResultText.enabled = true;

        playerControl.IsLockTime = true;

        CountDown = 250;
        gameOver = true;

    }


}
