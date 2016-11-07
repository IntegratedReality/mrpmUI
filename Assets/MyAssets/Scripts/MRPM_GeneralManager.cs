using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MRPM
{
public class MRPM_GeneralManager : MonoBehaviour {

	public string myRobotHostName = null;
	public string mainHostName = null;
	public string myRobotID = null;

	public int PORT_ROBOT = 8000;
	public int PORT_OPERATOR = 8001;
	public int PORT_MAINRCV = 8000;
	public string ADDRESS_TO_ROBOT = "/operator/operation";
	public string ADDRESS_TO_Main_SHOOT =  "/operator/shot";
	public string ADDRESS_ACK = "/operator/ack";
	public string ADDRESS_SYNC = "/main/toCtrlr/sync";

	public OscIn _oscIn;
	public OscOut _oscOut;

	static public MRPM_GeneralManager _instance;
	void Awake(){
		if (_instance == null){
			_instance = this;
			DontDestroyOnLoad(gameObject);
		} else {
			Destroy(gameObject);
		}
	}

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {

	}

	public void LoadMainLevel(){
		SceneManager.LoadScene("Main", LoadSceneMode.Single);
	}
}

}
