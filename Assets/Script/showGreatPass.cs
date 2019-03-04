using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class showGreatPass : MonoBehaviour {


    private int countDown;
    public GameManage gameManager;
    private TMP_Text PassLengthText;

	void Start () {

        countDown = 0;
        transform.position += new Vector3(1800, 0, 0);
        PassLengthText = GetComponent<TMP_Text>();
        PassLengthText.text = string.Format("Show a nice pass");
    }
	
	// Update is called once per frame
	void Update () {




        if (gameManager.HaveGreatPass)
        {
            ShowUp();
        }

        if (countDown > 70)
        {
            countDown -= 1;
            transform.position += new Vector3(-60, 0, 0);

        }
        else if(countDown > 10) {

            countDown -= 1;

        }
        else if (countDown > 0)
        {
            countDown -= 1;
            transform.position += new Vector3(180, 0, 0);

        }

    }

    void ShowUp(){
        if(gameManager.GreatPassLength >= 25){
            PassLengthText.text = string.Format("God Pass For {0} Yards!", gameManager.GreatPassLength);
        }
        else if(gameManager.GreatPassLength >= 20){
            PassLengthText.text = string.Format("Great Pass For {0} Yards!", gameManager.GreatPassLength);
        }
        else{
            PassLengthText.text = string.Format("Nice Pass For {0} Yards!", gameManager.GreatPassLength);
        }

        countDown = 100;
        gameManager.HaveGreatPass = false;

    }
}
