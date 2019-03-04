using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canvas_NameFollow : MonoBehaviour {
    public GameObject[] buttonList;
    public Vector3 pos;
    RectTransform canvasRT;
    public Camera camera;
    private Vector3 roboPos;
    private Vector3 roboScreenPos;

    // Use this for initialization
    void Start () {
        canvasRT = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update () {
        for (int i = 0; i<buttonList.Length; i++)
        {
            GameObject followObject = GameObject.Find("Player_B" + (i + 2).ToString());
            Vector3 followObjectPos = followObject.transform.position;
            RectTransform rt = buttonList[i].GetComponent<RectTransform>();
            Vector3 followObjectScreenPos = camera.WorldToViewportPoint(followObject.transform.TransformPoint(followObjectPos));
            rt.anchorMax = followObjectScreenPos;
            rt.anchorMin = followObjectScreenPos;
        }
	}
}
