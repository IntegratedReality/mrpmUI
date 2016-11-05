using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MRPM;

[RequireComponent(typeof(OscOut))]
public class StartGameButtonScript : MonoBehaviour {

    public MRPM_GeneralManager _generalManager;
    Button startButton;
    OscOut oscOut;

	// Use this for initialization
	void Start () {
		startButton = GetComponent<Button>();
        startButton.interactable = false;
        oscOut = GetComponent<OscOut>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnAuthorized(){
        startButton.interactable = true;
    }

    public void SendACK(){
        int myRobotID = _generalManager.myRobotID;
        if (myRobotID!=0){
            Debug.Log("Send ACK");
            oscOut.Send("/operator/ack", myRobotID);
        }
    }
}
