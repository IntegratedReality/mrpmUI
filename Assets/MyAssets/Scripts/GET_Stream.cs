using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GET_Stream : MonoBehaviour {

    //public InputField urlInput;
    private string url;
    [SerializeField]
    const float GetRate = 0.15f;
    private float nextGet ;
    RawImage rawImage;

    IEnumerator GetTexture() {
        if (url != null){
            var www = new WWW(url);
            yield return www;
            rawImage.texture = www.textureNonReadable;
            rawImage.SetNativeSize();
        }
    }

    void Start() {
        url = "http://" + PlayerPrefs.GetString("rasPiAddress") + ":8080/?action=snapshot";
        Debug.Log("Connect to " + url);
        rawImage = GetComponent<RawImage>();
    }

    void Update() {
        if (Time.time > nextGet) {
            StartCoroutine (GetTexture ());
            nextGet = Time.time + GetRate;
        }
    }
}
