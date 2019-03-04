using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventManage : MonoBehaviour {


    private const int Yes = 1;
    private const int No = 0;


    // 靈氣陣列
    private int[,] NimbusMatrix;

    private const int SpeedDown = 1;
    private const int StrengthDown = 2;
    private const int PassAccuracyDown = 3;
    private const int SpeedUp = 4;
    private const int StrengthUp = 5;
    private const int PassAccuracyUp = 6;


    private const float Var_speed = 2.5f;
    private const float Var_strength = 0.5f;
    private const float Var_passAccuracy = 3f;



    // 擒抱難度
    public float DefenseLevel = 1.8f;


    // 持球者狀態
    public Text DisplayText;
    public int BallHolder;
    public float HealthPoint = 100.0f;
    private float HealthPointDecreaseRate = 0f;


    // 球員物件
    private GameObject[] Players;
 

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



    // 遊戲管理員
    public GameManage gameManager;

    // 球員管理員
    public PlayerControl playerControl;


    // 設定球員物件(call by CourtManage)
    public void SetupPlayerGameObj(GameObject playerApi, int playerCode)
    {
        Players[playerCode] = playerApi;
    }



    // Use this for initialization
    void Start () {

        NimbusMatrix = new int[9, 9];
        Players = new GameObject[9];
        DisplayText.text = "";



    }


    void Update()
    {
        if(gameManager.gameOver==false && gameManager.roundOver==false ){

            CheckBallHolder();
            HugedCountdown();
            DisplayText.text = string.Format(" {0:N0}%", HealthPoint);

        }


    }
   




    // 代號為PlayerCode的球員周圍的狀況
    public void AdjRegister(int playerCode,int[] adjStatus){

        for (int id = A1; id <= D4; id++){

            // 若此球員與其他球員之間的相鄰關係產生變化
            if(NimbusMatrix[playerCode,id] != adjStatus[id]){

                NimbusMatrix[playerCode, id] = adjStatus[id];

                if (adjStatus[id]==Yes){
                    //Debug.Log(string.Format("{0}受到了{1}的靈氣影響", playerCode, id));
                    InstallNimbus(playerCode, id);


                }
                else if(adjStatus[id] == No){
                    //Debug.Log(string.Format("{0}解除了{1}的靈氣影響", playerCode, id));
                    ReleaseNimbus(playerCode, id);

                }

            }

        }

    }

    // 安裝靈氣
    private void InstallNimbus(int Rx,int Tx)
    {
        int NimbusType = Players[Tx].GetComponent<PlayerProperties>().nimbusNumber;

        switch(NimbusType){
            case SpeedDown:
                Players[Rx].GetComponent<PlayerProperties>().SetTeamProperties("speed",-Var_speed);
                break;
            case StrengthDown:
                Players[Rx].GetComponent<PlayerProperties>().SetTeamProperties("strength", -Var_strength);
                break;
            case PassAccuracyDown:
                Players[Rx].GetComponent<PlayerProperties>().SetTeamProperties("passAccuracy", -Var_passAccuracy);
                break;
            case SpeedUp:
                Players[Rx].GetComponent<PlayerProperties>().SetTeamProperties("speed", +Var_speed);
                break;
            case StrengthUp:
                Players[Rx].GetComponent<PlayerProperties>().SetTeamProperties("strength", +Var_strength);
                break;
            case PassAccuracyUp:
                Players[Rx].GetComponent<PlayerProperties>().SetTeamProperties("passAccuracy", +Var_passAccuracy);
                break;
        }


    }

    // 卸載靈氣
    private void ReleaseNimbus(int Rx, int Tx)
    {
        int NimbusType = Players[Tx].GetComponent<PlayerProperties>().nimbusNumber;

        switch (NimbusType)
        {
            case SpeedDown:
                Players[Rx].GetComponent<PlayerProperties>().SetTeamProperties("speed", +Var_speed);
                break;
            case StrengthDown:
                Players[Rx].GetComponent<PlayerProperties>().SetTeamProperties("strength", +Var_strength);
                break;
            case PassAccuracyDown:
                Players[Rx].GetComponent<PlayerProperties>().SetTeamProperties("passAccuracy", +Var_passAccuracy);
                break;
            case SpeedUp:
                Players[Rx].GetComponent<PlayerProperties>().SetTeamProperties("speed", -Var_speed);
                break;
            case StrengthUp:
                Players[Rx].GetComponent<PlayerProperties>().SetTeamProperties("strength", -Var_strength);
                break;
            case PassAccuracyUp:
                Players[Rx].GetComponent<PlayerProperties>().SetTeamProperties("passAccuracy", -Var_passAccuracy);
                break;
        }


    }


    // 檢查持球者
    private void CheckBallHolder(){

        int currentHolder = unused;

        for (int id = A1; id <= D4; id++){
            // Unfinish
            if(Players[id].GetComponent<PlayerProperties>().pickingBall){
                currentHolder = id;
                break;
            }
        }

        
        if(currentHolder == unused){ // 球正在飛：隱藏健康指數字串及重置健康指數

            DisplayText.enabled = false;
            HealthPoint = 100.0f;
            HealthPointDecreaseRate = 0;
        }
        else if(D1 <= currentHolder && currentHolder <= D4){ // 球被敵人劫走->LOSE
            


            gameManager.Lose();

        }
        else if(currentHolder != BallHolder){ // 持球換人

            DisplayText.enabled = true;


        }


        BallHolder = currentHolder;

    }



    // 擒抱倒數計時
    private void HugedCountdown(){



        float count = 0f;
        float x1 = 1f;
        float x2 = 1f;

        for (int id = D1; id <= D4;id++){
            if(NimbusMatrix[BallHolder,id]==Yes){

                x1 = Players[A1].GetComponent<PlayerProperties>().GetTeamProperty("strength");
                x2 = Players[id].GetComponent<PlayerProperties>().GetTeamProperty("strength");

                count = count + x2 / x1;
            }
        }

        HealthPointDecreaseRate = count * DefenseLevel;

        HealthPoint = HealthPoint - HealthPointDecreaseRate;



        // 擒抱成功
        if (HealthPoint <= 0)
        {
            gameManager.NewAtkRound();

        }


    }







  


    // ---------------------- 各種靈氣function ---------------------------




}
