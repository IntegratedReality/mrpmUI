using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using MRPM;

public class GET_Stream : MonoBehaviour {

	public MRPM_GeneralManager _generalManager;
	public bool IsStreamingEnabled {get; set;}
	public string url;
	[SerializeField]
	const float GetRate = 0.15f;
	private float nextGet ;
	RawImage rawImage;

	IEnumerator GetTexture() {
		if (url != null) {
			var www = new WWW(url);
			yield return www;
			try {
				rawImage.texture = www.textureNonReadable;
				rawImage.SetNativeSize();
			} catch (System.Exception e) {
				Debug.Log(e);
			}
		}
	}

	void Start() {
		_generalManager = MRPM_GeneralManager._instance;
		url = "http://" + _generalManager.myRobotHostName + ":8080/?action=snapshot";
		//url ="http://mrpmpi2.local:8080/?action=snapshot";
		Debug.Log("Connect to " + url);
		rawImage = GetComponent<RawImage>();
		IsStreamingEnabled = true;
	}

	void Update() {
		if (IsStreamingEnabled) {
			if (Time.time > nextGet) {
				StartCoroutine (GetTexture ());
				nextGet = Time.time + GetRate;
			}
		}
	}
}
