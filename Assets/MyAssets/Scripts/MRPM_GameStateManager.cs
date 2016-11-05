using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MRPM
{

public  class MRPM_GameStateManager : MonoBehaviour {

	[SerializeField]
	public string RasPiAddress{get; set;}
	[SerializeField]
	public bool IsPlayerAuthorized{get; set;}

	static  public MRPM_GameStateManager instance;
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
		RasPiAddress = arg;
	}

	public void LoadMainLevel(){
		SceneManager.LoadScene("Main", LoadSceneMode.Single);
	}
}

}
