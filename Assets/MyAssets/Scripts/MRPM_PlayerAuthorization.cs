using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MRPM
{

[RequireComponent (typeof(OscIn), typeof(OscOut))]
public class MRPM_PlayerAuthorization : MonoBehaviour {
	OscIn oscIn;
	OscOut oscOut;
	public Text _authDataText;

	[HeaderAttribute("Auth Data Info")]
	[SerializeField]
	string robotID;
	[SerializeField]
	string robotHos;

	MRPM_GeneralManager generalMagnager;
	public StartGameButtonScript _startButton;

	// Use this for initialization
	void Start () {
		generalMagnager = MRPM_GeneralManager._instance;
		oscIn = GetComponent<OscIn>();
		oscOut = GetComponent<OscOut>();
		oscIn.Open(generalMagnager.PORT_MAINRCV, null);
		oscIn.Map("/main/toCtrlr/assign", ReceiveAuthData);
	}

	public void OnClick(){
		Debug.Log("AuthButtonClick");
		if (!oscIn.isOpen){
			oscIn.Open(generalMagnager.PORT_MAINRCV, null);
		}
		// display wait message
	}

	// Update is called once per frame
	void Update () {

	}

	public void ReceiveAuthData(OscMessage message){
		//Show GUI
		if (message.TryGet(0, out robotID) && message.TryGet(1, out robotHos))
		{
			if (generalMagnager.mainHostName!=null)
			OnAuthorized();
		}
	}

	void OnAuthorized(){
		oscIn.Close();
		oscOut.Open(generalMagnager.PORT_MAINRCV, generalMagnager.mainHostName);
		generalMagnager.myRobotID = robotID;
		generalMagnager.myRobotHostName = robotHos;
		_authDataText.text = "robotID: " + robotID + " HostName: " + robotHos;
		_startButton.OnAuthorized();
	}
}

}

