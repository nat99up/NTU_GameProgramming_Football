using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class PlayerProperties : MonoBehaviour {

    XmlDocument PlayerXml;
    public int team;
    public float speed;
    public float passAccuracy;
    public float strength;
    public int nimbusNumber; 
    public float speedTemp;
    public float passAccuracyTemp;
    public float strengthTemp;
    public bool pickingBall;

	// Use this for initialization
	void Start () {
        /*

        //-----------------------loading playerProperty from xml-----------------------
        PlayerXml = new XmlDocument();
        //PlayerXml.Load(Application.dataPath + "/Data/PlayerProperty.xml");
        PlayerXml.Load("./Assets/Data/PlayerProperty.xml");
        //PlayerXml.Load("./PlayerProperty.xml");
        XmlNodeList nodeList = PlayerXml.SelectSingleNode("root").ChildNodes;
        foreach (XmlElement xet in  nodeList){
            if (xet.GetAttribute("id") == gameObject.name){
                foreach(XmlElement xet1 in xet.ChildNodes)
                {
                    switch (xet1.Name)
                    {
                        case "team":
                            team = int.Parse(xet1.InnerText);
                            break;
                        case "speed":
                            speed = float.Parse(xet1.InnerText);
                            break;
                        case "passAccuracy":
                            passAccuracy = float.Parse(xet1.InnerText);
                            break;
                        case "strength":
                            strength = float.Parse(xet1.InnerText);
                            break;
                        case "nimbusNumber":
                            nimbusNumber = int.Parse(xet1.InnerText);
                            break;
                        default:
                            Debug.Log("Fatal error occurs when reading Player's properties!");
                            break;
                    }
                }
            }
        }
        //-----------------------END Of loading playerProperty from xml-----------------------

        */

        GameObject TempPlayer;
        PlayerProperties TempPlayerProperty;

        TempPlayer = GameObject.Find("Player_B1");
        TempPlayerProperty = TempPlayer.GetComponent<PlayerProperties>();
        TempPlayerProperty.team = 1;
        TempPlayerProperty.speed = 6;
        TempPlayerProperty.passAccuracy = 13;
        TempPlayerProperty.strength = 1;
        TempPlayerProperty.nimbusNumber = 0;

        TempPlayer = GameObject.Find("Player_B2");
        TempPlayerProperty = TempPlayer.GetComponent<PlayerProperties>();
        TempPlayerProperty.team = 1;
        TempPlayerProperty.speed = 7;
        TempPlayerProperty.passAccuracy = 4;
        TempPlayerProperty.strength = 1;
        TempPlayerProperty.nimbusNumber = 0;

        TempPlayer = GameObject.Find("Player_B3");
        TempPlayerProperty = TempPlayer.GetComponent<PlayerProperties>();
        TempPlayerProperty.team = 1;
        TempPlayerProperty.speed = 6;
        TempPlayerProperty.passAccuracy = 10;
        TempPlayerProperty.strength = 1;
        TempPlayerProperty.nimbusNumber = 0;

        TempPlayer = GameObject.Find("Player_B4");
        TempPlayerProperty = TempPlayer.GetComponent<PlayerProperties>();
        TempPlayerProperty.team = 1;
        TempPlayerProperty.speed = 6.5f;
        TempPlayerProperty.passAccuracy = 4;
        TempPlayerProperty.strength = 1.5f;
        TempPlayerProperty.nimbusNumber = 0;

        TempPlayer = GameObject.Find("Player_O1");
        TempPlayerProperty = TempPlayer.GetComponent<PlayerProperties>();
        TempPlayerProperty.team = 2;
        TempPlayerProperty.speed = 7.5f;
        TempPlayerProperty.passAccuracy = 1;
        TempPlayerProperty.strength = 1;
        TempPlayerProperty.nimbusNumber = 3;

        TempPlayer = GameObject.Find("Player_O2");
        TempPlayerProperty = TempPlayer.GetComponent<PlayerProperties>();
        TempPlayerProperty.team = 2;
        TempPlayerProperty.speed = 7.5f;
        TempPlayerProperty.passAccuracy = 1;
        TempPlayerProperty.strength = 1;
        TempPlayerProperty.nimbusNumber = 1;

        TempPlayer = GameObject.Find("Player_O3");
        TempPlayerProperty = TempPlayer.GetComponent<PlayerProperties>();
        TempPlayerProperty.team = 2;
        TempPlayerProperty.speed = 8f;
        TempPlayerProperty.passAccuracy = 1;
        TempPlayerProperty.strength = 1;
        TempPlayerProperty.nimbusNumber = 3;

        TempPlayer = GameObject.Find("Player_O4");
        TempPlayerProperty = TempPlayer.GetComponent<PlayerProperties>();
        TempPlayerProperty.team = 2;
        TempPlayerProperty.speed = 9.5f;
        TempPlayerProperty.passAccuracy = 1;
        TempPlayerProperty.strength = 1;
        TempPlayerProperty.nimbusNumber = 2;


    }

    public void SetTeamProperties(string property , float temp)
    {
        switch (property)
        {
            case "speed":
                speedTemp += temp;
                break;
            case "passAccuracy":
                passAccuracyTemp += temp;
                break;
            case "strength":
                strengthTemp += temp;
                break;
            default:
                Debug.Log("No such property, Set fail!");
                break;
        }
    }

    public float GetTeamProperty(string property)
    {
        switch (property)
        {
            case "speed":
                return speed + speedTemp;
            case "passAccuracy":
                return passAccuracy + passAccuracyTemp;
            case "strength":
                return strength + strengthTemp;
            default:
                Debug.Log("No such property, Get fail!");
                return -999999.99f;
        }
    }


    // Update is called once per frame
    void Update () {
		
	}

}
