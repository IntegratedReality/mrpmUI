using UnityEngine;
 using UnityEngine.UI;
 using UnityEngine.Networking;
 using System.Collections;

 public class GET_Stream : MonoBehaviour {

     public InputField urlInput;
     private string url;

     public float GetRate = 0.15f;
     private float nextGet = 0;
     CanvasRenderer canvasRenderer;

     IEnumerator GetTexture() {
         url = urlInput.text;
         Renderer renderer = GetComponent<Renderer>();
         UnityWebRequest www = UnityWebRequest.GetTexture("http://" + url + ":8080/?action=snapshot");
         yield return www.Send();
         renderer.material.mainTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
         //canvasRenderer.SetTexture(((DownloadHandlerTexture)www.downloadHandler).texture);
     }

     void Start(){
     	//canvasRenderer = GetComponent<CanvasRenderer>();
     }

     void Update() {
         if (Time.time > nextGet) {
             StartCoroutine (GetTexture ());
             nextGet = Time.time + GetRate;
         }
     }
 }
