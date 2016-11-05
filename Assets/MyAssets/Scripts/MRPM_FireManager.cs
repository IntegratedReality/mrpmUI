using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MRPM
{

[RequireComponent(typeof(OscOut))]
public class MRPM_FireManager : MonoBehaviour {

    OscOut oscOut;
    int robotID;

    MRPM_GeneralManager gm;

	// Use this for initialization
    void Start () {
        gm = MRPM_GeneralManager.instance;
        oscOut = GetComponent<OscOut>();
        oscOut.Open(gm.PORT_MAINRCV, gm.myRasPiHostName);
        robotID = gm.myRobotID;
    }

    void OnEnabled(){
        oscOut = GetComponent<OscOut>();
        oscOut.Open(gm.PORT_MAINRCV, gm.myRasPiHostName);
        robotID = gm.myRobotID;
    }

    void OnDisabled(){
        oscOut.Close();
    }
	
	// Update is called once per frame
	void Update () {
        if(Input.GetKey("space")){
            Fire();
       }
	}

    void Fire(){
        if (oscOut!=null){
            oscOut.Send(gm.ADDRESS_TO_Main_SHOOT, robotID);
        }
    }
}

}