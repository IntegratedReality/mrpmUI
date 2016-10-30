using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Net;

namespace MRPM
{

public  class GameStateManager : MonoBehaviour {

	string raspiAddressString = "rasPiAddress";

	static  public GameStateManager instance;
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

	void OnApplicationQuit(){
		PlayerPrefs.Save();
	}

	public void SetRasPiAddress(string arg){
		PlayerPrefs.SetString(raspiAddressString, arg);
	}

	public void LoadMainLevel(){
		var address = PlayerPrefs.GetString(raspiAddressString);
		try {
			IPAddress.Parse(address);
		}catch(System.Exception e){
			Debug.Log(e);
			//ポップアップ表示
			return;
		}
		SceneManager.LoadScene("Main");
	}
}

}
