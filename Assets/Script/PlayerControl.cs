using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Boundary
{
    public float xMin, xMax, zMin, zMax;
}
public class PlayerControl : MonoBehaviour
{

    public GameObject humanControlPlayer;
    public Boundary boundary;
    public GameObject[] attackPlayerList;
    public GameObject[] defensePlayerList;
    public CourtManage _courtManage;
    public GameObject ball;
    public BallController _ballController;

    private GameObject targetPlayer ;

    // Creat by Nat
    private GameObject Leader;
    public bool IsLockTime;

    private string Player_Z;
    private string Player_X;
    private string Player_C;

    private int[] Def_Target;

    // Randon Set

    private const int period = 50;
    private int RandCount = -150;

    private float Hmove1 = 0;
    private float Hmove2 = -0.75f;
    private float Hmove3 = 0;
    private float Hmove4 = 0.75f;


    void Start()
    {
        IsLockTime = true;
        Def_Target = new int[4];
        ResetKeyForPlayers();
        ResetDefenseMatching();

        targetPlayer = null;

    }

    void Update()
    {
       
        humanControlPlayer = FindPickBallPlayerToControl(attackPlayerList);
        Leader = attackPlayerList[0];

    }

    void FixedUpdate()
    {


        if (IsLockTime)
            return;

      
        HumanControlMove(humanControlPlayer);


        if(humanControlPlayer.GetComponent<PlayerProperties>().pickingBall)
        {
            HumanThrowBall(humanControlPlayer);
        }


        UpdateSuitableDefenseMatching();
        AttackerControlMove(attackPlayerList,humanControlPlayer);
        DefenseControlMove(defensePlayerList);


    }

    GameObject FindPickBallPlayerToControl(GameObject[] playerlist)
    {
        foreach (GameObject player in playerlist)
        {
            if (player.GetComponent<PlayerProperties>().pickingBall)
            {
                return player;
            }
        }
        //Debug.Log("No one pick the ball, control default player!");
        return playerlist[0];

    }
    void HumanControlMove(GameObject ControledPlayer)
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        movement = movement.normalized;
        float speed = ControledPlayer.GetComponent<PlayerProperties>().GetTeamProperty("speed");
        ControledPlayer.GetComponent<Rigidbody>().velocity = movement * speed;
        ControledPlayer.GetComponent<Rigidbody>().position = new Vector3
        (
            Mathf.Clamp(ControledPlayer.GetComponent<Rigidbody>().position.x, boundary.xMin, boundary.xMax),
            0.85f,
            Mathf.Clamp(ControledPlayer.GetComponent<Rigidbody>().position.z, boundary.zMin, boundary.zMax)
        );
    }
    void HumanThrowBall(GameObject ControledPlayer)
    {

        if (Input.anyKeyDown)
        {
            if (Input.GetKey(KeyCode.Z))
            {
                targetPlayer = GameObject.Find(Player_Z);
                _ballController.ThrowBall(humanControlPlayer, targetPlayer, humanControlPlayer.GetComponent<PlayerProperties>().GetTeamProperty("passAccuracy"));
                Player_Z = ControledPlayer.name;


            }
            else if (Input.GetKey(KeyCode.X))
            {
                targetPlayer = GameObject.Find(Player_X);
                _ballController.ThrowBall(humanControlPlayer, targetPlayer, humanControlPlayer.GetComponent<PlayerProperties>().GetTeamProperty("passAccuracy"));
                Player_X = ControledPlayer.name;


            }
            else if (Input.GetKey(KeyCode.C))
            {
                targetPlayer = GameObject.Find(Player_C);
                _ballController.ThrowBall(humanControlPlayer, targetPlayer, humanControlPlayer.GetComponent<PlayerProperties>().GetTeamProperty("passAccuracy"));
                Player_C = ControledPlayer.name;


            }
            else
            {

            }

        }
    }


    int GetPlayerNumber(GameObject player)
    {
        return int.Parse(player.name.Substring(player.name.Length - 1, 1));
    }


    void AttackerControlMove(GameObject[] playerlist,GameObject human)
    {
        foreach(GameObject player in playerlist)
        {
            if (player != human)
            {

                if (!_ballController.picked){
                    ChaseBall(player);
                }
                else
                    GetRidOfDefenser(player, human);

            }
        }
    }
    void DefenseControlMove(GameObject[] playerlist)
    {

        foreach (GameObject player in playerlist)
        {

            if (BehindLeader(player))
                HuntLeader(player, Leader);
            else
                BlockAttacker(player);

        }
    }
    void GetRidOfDefenser(GameObject player, GameObject human)
    {
        // 接球者保持的距離取決於傳球者的傳球精準度

        float dist_receive = 25;

        int playerNum = GetPlayerNumber(player);
        int humanNum = GetPlayerNumber(human);
        CourtIdx player_cood= _courtManage.GetPlayerCourtIdx( playerNum );
        CourtIdx human_cood = _courtManage.GetPlayerCourtIdx(humanNum);

        float distance  = Vector2.Distance(new Vector2(player_cood.x, player_cood.y), new Vector2(human_cood.x, human_cood.y));

        float moveHorizontal = 0;
        float moveVertical = 0;

        if (  Mathf.Abs(dist_receive - distance) < dist_receive &&  Mathf.Abs(dist_receive - distance) > 5 )
        {

            RandCount++;
            if(RandCount == period){
                Hmove1 = Random.Range(-0.5f, 0.5f);
                Hmove2 = Random.Range(-1f, 0f);
                Hmove3 = Random.Range(-0.5f, 0.5f);
                Hmove4 = Random.Range( 0f, 1f);
                RandCount = 0;

            }


            switch (playerNum)
            {
                case 1:
                     moveHorizontal = Hmove1;
                     moveVertical = 1;
                    break;
                case 2:
                     moveHorizontal = Hmove2;
                     moveVertical = 1;
                    break;
                case 3:
                     moveHorizontal = Hmove3;
                     moveVertical = 1;
                    break;
                case 4:
                     moveHorizontal = Hmove4;
                     moveVertical = 1; 
                    break;
                default:
                    Debug.Log("Fatal error occurs when assign player direction");
                    break;
            }
            Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical)*Mathf.Sign(20-distance);
            movement = movement.normalized;
            float speed = player.GetComponent<PlayerProperties>().GetTeamProperty("speed");
            player.GetComponent<Rigidbody>().velocity = movement * speed;
            player.GetComponent<Rigidbody>().position = new Vector3
            (
                Mathf.Clamp(player.GetComponent<Rigidbody>().position.x, boundary.xMin, boundary.xMax),
                0.85f,
                Mathf.Clamp(player.GetComponent<Rigidbody>().position.z, boundary.zMin, boundary.zMax)
            );
        }
    }
    void ChaseBall(GameObject player)
    {

        float distance = Vector3.Distance(player.transform.position, _ballController.PredictFallPoint);
        float speed = player.GetComponent<PlayerProperties>().GetTeamProperty("speed");

        Vector3 movement = Vector3.zero;
        Vector3 v_ball = ball.GetComponent<Rigidbody>().velocity;

        if(Mathf.Abs(distance ) > 0.4)
        {
            movement = _ballController.PredictFallPoint - player.transform.position;

        }
        movement = movement.normalized;

        player.GetComponent<Rigidbody>().velocity = movement * speed;
        player.GetComponent<Rigidbody>().position = new Vector3
        (
            Mathf.Clamp(player.GetComponent<Rigidbody>().position.x, boundary.xMin, boundary.xMax),
            0.85f,
            Mathf.Clamp(player.GetComponent<Rigidbody>().position.z, boundary.zMin, boundary.zMax)
        );
    }
    void BlockAttacker(GameObject player)
    {

        if (_ballController.picked == false && Random.value < 0.1f) {
            ChaseBall(player);
            return;
        }


        int WantToBlockPlayerNum = Def_Target[(int)player.name[player.name.Length - 1] - '1'];
        GameObject WantToBlockPlayer = attackPlayerList[WantToBlockPlayerNum];

        if (WantToBlockPlayer.GetComponent<PlayerProperties>().pickingBall)
        {
            HuntLeader(player, WantToBlockPlayer);
            return;
        }

        float RoutineDist = Vector3.Distance(ball.transform.position , WantToBlockPlayer.transform.position);
        RoutineDist = Mathf.Abs(RoutineDist);
        Vector3 blockPoint = WantToBlockPlayer.transform.position + (ball.transform.position - WantToBlockPlayer.transform.position) / 5;
        Vector3 movement = blockPoint - player.transform.position;
        movement = movement.normalized;

        movement.y = 0f;


        float distance = Vector3.Distance(player.transform.position, WantToBlockPlayer.transform.position);
        if (Mathf.Abs(distance) > 3 )
        {
            float speed = player.GetComponent<PlayerProperties>().GetTeamProperty("speed");
            player.GetComponent<Rigidbody>().velocity = movement * speed;
            player.GetComponent<Rigidbody>().position = new Vector3
            (Mathf.Clamp(player.GetComponent<Rigidbody>().position.x, boundary.xMin, boundary.xMax),
             player.GetComponent<Rigidbody>().position.y,
             Mathf.Clamp(player.GetComponent<Rigidbody>().position.z, boundary.zMin, boundary.zMax)
            );
        }

    }

    // Creat by Nat
    private bool BehindLeader(GameObject hunter)
    {

        foreach (GameObject player in attackPlayerList){
            if (player.GetComponent<PlayerProperties>().pickingBall)
                Leader = player;
        }

        if (Leader.GetComponent<PlayerProperties>().pickingBall == false)
            return false;

        if (Leader.transform.position.z > hunter.transform.position.z + 1 || Leader.transform.position.z > 110)
            return true;
        else
            return false;


    }

    // Creat by Nat
    private void HuntLeader(GameObject hunter, GameObject leader)
    {
        Vector3 hugOrient = (leader.transform.position.z < hunter.transform.position.z) ? new Vector3(0, 0, -0.5f) : new Vector3(0, 0, 0.5f);
        Vector3 movement = leader.transform.position - hunter.transform.position + hugOrient;

        movement = movement.normalized;

        if (Mathf.Abs(hunter.transform.position.x - leader.transform.position.x) < 1)
            movement.x /= 1.2f;
        if (hunter.transform.position.x < leader.transform.position.x)
            movement += new Vector3(0.1f, 0, 0);
        else if (hunter.transform.position.x > leader.transform.position.x)
            movement += new Vector3(-0.1f, 0, 0);

        movement = movement.normalized;

        float speed = hunter.GetComponent<PlayerProperties>().GetTeamProperty("speed");
        hunter.GetComponent<Rigidbody>().velocity = movement * speed;
        hunter.GetComponent<Rigidbody>().position = new Vector3
        (
            Mathf.Clamp(hunter.GetComponent<Rigidbody>().position.x, boundary.xMin, boundary.xMax),
            0.85f,
            Mathf.Clamp(hunter.GetComponent<Rigidbody>().position.z, boundary.zMin, boundary.zMax)
        );

    }

    // Creat by Nat
    public void ResetKeyForPlayers(){
        Player_Z = "Player_B2";
        Player_X = "Player_B3";
        Player_C = "Player_B4";

        Hmove1 = 0;
        Hmove2 = -0.75f;
        Hmove3 = 0;
        Hmove4 = 0.75f;

        RandCount = -150;

    }

    // Creat by Nat
    public void ResetDefenseMatching()
    {
        Def_Target[0] = 0;
        Def_Target[1] = 1;
        Def_Target[2] = 2;
        Def_Target[3] = 3;
    }


    // Creat by Nat
    private void UpdateSuitableDefenseMatching(){

        int randomDefPlayerNum = ((int)Random.Range(1, 100)) % 16;

        if (randomDefPlayerNum >= 4)
            return;

        int temp;
        GameObject randomDefPlayer = defensePlayerList[randomDefPlayerNum];
        Vector3 D0 = randomDefPlayer.transform.position;
        Vector3 A0 = attackPlayerList[Def_Target[randomDefPlayerNum]].transform.position;

        for (int i = 0; i < 4;i++) {
            if (i == randomDefPlayerNum)
                continue;

            Vector3 D1 = defensePlayerList[i].transform.position;
            Vector3 A1 = attackPlayerList[Def_Target[i]].transform.position;


            if (Mathf.Abs(Vector3.Distance(D0,A0)) > Mathf.Abs(Vector3.Distance(D0,A1))
               && Mathf.Abs(Vector3.Distance(D1,A1)) > Mathf.Abs(Vector3.Distance(D1,D0)))
            {

                temp = Def_Target[randomDefPlayerNum];
                Def_Target[randomDefPlayerNum] = Def_Target[i];
                Def_Target[i] = temp;
                break;
            }


        }


    }



}
