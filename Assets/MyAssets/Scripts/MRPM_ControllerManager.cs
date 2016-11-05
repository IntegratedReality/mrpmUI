using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MRPM
{

[RequireComponent(typeof(OscOut))]
public class MRPM_ControllerManager : MonoBehaviour {

	OscOut oscOut;
    int robotID;

    MRPM_GeneralManager gm;

    static public MRPM_ControllerManager instance;
    void Awake(){
        if (instance == null){
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }

	// Use this for initialization
	void Start () {
        gm = MRPM_GeneralManager.instance;
        oscOut = GetComponent<OscOut>();
        oscOut.Open(gm.PORT_ROBOT, gm.myRasPiHostName);
        robotID = gm.myRobotID;
	}

	// Update is called once per frame
	void Update () {
        oscOut.Send(gm.ADDRESS_TO_ROBOT, RobotControlOrder());
	}

    void OnEnabled(){
        oscOut = GetComponent<OscOut>();
        oscOut.Open(gm.PORT_ROBOT, gm.myRasPiHostName);
        robotID = gm.myRobotID;
    }

    void OnDisabled(){
        oscOut.Close();
    }

    int RobotControlOrder(){
        int h = (int)Input.GetAxis("Horizontal");
        int v = (int)Input.GetAxis("Vertical");
        switch(3*h + v){
            default: return 0;
            case 0: return 0;
            case 1: return 1;
        }
    }
}

}