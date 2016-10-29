using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RasPiAddressInputScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if (PlayerPrefs.HasKey("rasPiAddress")){
			GetComponent<InputField>().text = PlayerPrefs.GetString("rasPiAddress");
			Debug.Log("Loaded, " + PlayerPrefs.GetString("rasPiAddress"));
		}
	}

	// Update is called once per frame
	void Update () {

	}
}
