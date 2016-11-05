using UnityEngine;
using UnityEngine.UI;

namespace MRPM{

[RequireComponent (typeof(OscIn), typeof(OscOut))]
public class MRPM_PlayerAuthorization : MonoBehaviour {
	OscIn oscIn;
	OscOut oscOut;
	public Text _authDataText;
	[SerializeField]
	int port_MAINRCV = 8001;
	[SerializeField]
	string mainServerAddress = "192.168.1.10";

	[HeaderAttribute("Auth Data Info")]
	[SerializeField]
	int robotID;
	[SerializeField]
	string rasPiHostName;
	[SerializeField]
	bool isAuthorized;

	// Use this for initialization
	void Start () {
		oscIn = GetComponent<OscIn>();
		oscOut = GetComponent<OscOut>();
		oscIn.Open(port_MAINRCV, null);
		oscIn.Map("/main/toCtrlr/assign", ReceiveAuthData);
	}

	void OnClick(){
		if (!oscIn.isOpen){
			oscIn.Open(port_MAINRCV, null);
		}
		// display wait message
	}

	// Update is called once per frame
	void Update () {

	}

	public void ReceiveAuthData(OscMessage message){
		//Show GUI
		if (message.TryGet(0, out robotID) && message.TryGet(1, out rasPiHostName))
		{
			if (mainServerAddress!=null)
			OnAuthorized();
		}
	}

	void OnAuthorized(){
		oscIn.Close();
		oscOut.Open(port_MAINRCV, mainServerAddress);
		isAuthorized = true;
		oscOut.Send("/operator/ack", robotID);
		_authDataText.text = "robotID: " + robotID + " HostName: " + rasPiHostName;
		oscOut.Close();
	}
}

}

