using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MRPM
{
public class MRPM_GeneralManager : MonoBehaviour {

	public string myRasPiHostName;
	public int myRobotID;

	public int PORT_ROBOT = 8000;
	public int PORT_OPERATOR = 8001;
	public int PORT_MAINRCV = 8001;
	public string ADDRESS_TO_ROBOT = "/operator/operation";
	public string ADDRESS_TO_Main_SHOOT =  "/operator/shot";

	OscIn oscIn;
	OscOut oscOut;

	static public MRPM_GeneralManager instance;
	void Awake(){
		if (instance == null){
			instance = this;
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
