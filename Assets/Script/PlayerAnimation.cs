using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour {

    public GameObject PlayerCube;
    public GameManage gameManage;

    public float initOrientation = 0;

    private Animator anim;
    private Vector3 offset;
    private Vector3 PV;
    private float orientation = 0;

	// Use this for initialization
	void Start () {

        anim = GetComponent<Animator>();
        offset = new Vector3(0, 0.4f, 0);
        orientation = initOrientation;

    }
	
	// Update is called once per frame
	void Update () {

        PV = PlayerCube.GetComponent<Rigidbody>().velocity;


        if (PV.x != 0 || PV.z != 0){
        

            if (PV.z == 0)
            {
                if (PV.x == 0)
                    orientation = 0;
                else if (PV.x > 0)
                    orientation = 90;
                else
                    orientation = -90;
            }
            else
            {
                orientation = Mathf.Atan(PV.x / PV.z);
                orientation = orientation * 180 / Mathf.PI;

                if (PV.z < 0)
                    orientation = orientation - 180;

            }

            anim.SetBool("isRun", true);
            

        }else{

            anim.SetBool("isRun", false);

        }

        transform.position = PlayerCube.transform.position - offset;
        transform.eulerAngles = new Vector3(0, orientation, 0);

        if (gameManage.roundOver)
            orientation = initOrientation;

    }
}
