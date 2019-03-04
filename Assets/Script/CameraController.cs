using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public GameObject player;
    public GameObject ball;
    public PlayerControl _playerControl;
    private Vector3 offset;

    // Use this for initialization
    void Start () {
        //offset = transform.position - player.transform.position;
        offset = new Vector3(0, 5, -10);
    }

    // Update is called once per frame
    void Update () {

        player = _playerControl.humanControlPlayer;
        if (ball.GetComponent<BallController>().picked)
            transform.position = player.transform.position + offset;
        else
            transform.position = ball.transform.position + offset;
    }
}
