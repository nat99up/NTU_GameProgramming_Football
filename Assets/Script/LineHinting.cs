using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineHinting : MonoBehaviour {


    private int count = -1;
    private int z = 0;

	// Use this for initialization
	void Start () {
        Shine(10);
    }
	
	// Update is called once per frame
	void Update () {


        if(count>=0){

            count -= 1;

            if (count % 40 == 20){
                GetComponent<LineRenderer>().SetPosition(0, new Vector3(-26.66f, 0.3f, z));
                GetComponent<LineRenderer>().SetPosition(1, new Vector3(26.66f, 0.3f, z));
            }
            else if (count % 40 == 0){
                GetComponent<LineRenderer>().SetPosition(0, new Vector3(-26.66f, 0.0f, z));
                GetComponent<LineRenderer>().SetPosition(1, new Vector3(26.66f, 0.0f, z));
            }


        }


    }


    public void Shine(int pos){

        z = pos;

        count = 180;
    }



}
