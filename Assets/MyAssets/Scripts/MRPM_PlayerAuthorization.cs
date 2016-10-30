using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MRPM{

[RequireComponent (typeof(OscIn))]
public class MRPM_PlayerAuthorization : MonoBehaviour {
	OscIn oscIn;
	bool isAuthorized;
	public Text _authDataText;
	[HeaderAttribute("Auth Data Info")]
	[SerializeField]
	string robotID;
	[SerializeField]
	string robotIPAddress;

	// Use this for initialization
	void Start () {
		oscIn = GetComponent<OscIn>();
		oscIn.Open(8001, null);
		oscIn.Map("/main", ReceiveAuthData);
	}

	// Update is called once per frame
	void Update () {

	}

	public void ReceiveAuthData(OscMessage message){
		//Show GUI
		if (message.TryGet(0, out robotID) && message.TryGet(1, out robotIPAddress))
		{
			OnAuthorized();
		}
	}

	void OnAuthorized(){
		oscIn.Close();
		isAuthorized = true;
		_authDataText.text = robotID + robotIPAddress;
	}
}

}

