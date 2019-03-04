using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour {
    public string lastCollisionPlayerName;
    public bool picked;
    public GameManage gameManager;

    public Vector3 PredictFallPoint;



    void OnCollisionEnter (Collision col)
    {
            

        if(col.gameObject.tag == "Player" && lastCollisionPlayerName != col.gameObject.name && picked == false)
        {

            lastCollisionPlayerName = col.gameObject.name;
            LockBallPosWhenPickOrPutDown("pick");
            gameObject.transform.parent = col.transform;
            gameObject.transform.localPosition = new Vector3(0f, 1.0f, 0.5f);
            gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.67f);
            col.gameObject.GetComponent<PlayerProperties>().pickingBall = true;

        }

        if (col.gameObject.name == "GrasslandGround")
        {
            gameManager.NewAtkRound();
        }

    }



    private void Update()
    {


        if(picked)
        {
            gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
        }
        else
        {


            if (gameObject.GetComponent<Rigidbody>().velocity.y > 1 )
            {
                //GameObject.Find("Player_B2").transform.position = PredictFallPoint;   
            }

            gameObject.transform.rotation.Set(0,0,0,0);
            gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX;
            gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationY;

        }


    }
    void LockBallPosWhenPickOrPutDown(string move)
    {
        switch(move)
        {
            case "pick":
                picked = true;
                gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
                break;
            case "put":
                picked = false;
                gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                break;
            default:
                Debug.Log("pick situation is not changed");
                break;
        }
    }
    public void ThrowBall(GameObject thrower, GameObject passTarget,float acc)
    {

        if (passTarget == null)
            return;
        gameObject.transform.parent = null;
        Vector3 force = passTarget.transform.position - thrower.transform.position;
        LockBallPosWhenPickOrPutDown("put");
        

        float x_offset = Mathf.Abs(passTarget.transform.position.x - thrower.transform.position.x);
        float z_offset = passTarget.transform.position.z - thrower.transform.position.z;
        float distance = Vector3.Distance(passTarget.transform.position,thrower.transform.position);


        force.x *= 1.5f;
        force.y = 4.5f;
        force.z *= 1.5f;



        if (x_offset + z_offset > 25 || z_offset >= 10)
        { // 長傳
            force.y = 2.5f + thrower.GetComponent<PlayerProperties>().GetTeamProperty("passAccuracy")/20;
            force.z *= 1.15f;
            gameObject.GetComponent<Rigidbody>().velocity = force;
            Debug.Log(string.Format("長傳"));
        }
        else
        { // 直傳
            force.x *= 2f + thrower.GetComponent<PlayerProperties>().GetTeamProperty("passAccuracy") / 10;
            force.y = -0.5f;
            force.z *= 2f + thrower.GetComponent<PlayerProperties>().GetTeamProperty("passAccuracy") / 10;
            gameObject.GetComponent<Rigidbody>().velocity = force;
            Debug.Log(string.Format("直傳"));
        }


        thrower.GetComponent<PlayerProperties>().pickingBall = false;


        /* 預測落點 */
        Vector3 Pos = gameObject.transform.position;
        Vector3 V = gameObject.GetComponent<Rigidbody>().velocity;
        float a = -Physics.gravity.y;
        float s = gameObject.transform.position.y - 1.6f;
        float t = (2 * V.y + Mathf.Sqrt(4 * Mathf.Pow(V.y, 2) + 8 * a * s)) / (2 * a);

        PredictFallPoint = new Vector3(Pos.x + V.x * t, 0.85f, Pos.z + V.z * t);

        /* 準確度 */
        

    }
    public void AssignBall(GameObject player)
    {
        lastCollisionPlayerName = player.name;
        LockBallPosWhenPickOrPutDown("pick");
        gameObject.transform.parent = player.transform;
        gameObject.transform.localPosition = new Vector3(0f, 1.0f, 0.5f);
        player.GetComponent<PlayerProperties>().pickingBall = true;

    }
}
