using System.Collections.Generic;
using UnityEngine;
using OscSimpl;

namespace MRPM
{

[RequireComponent(typeof(OscIn), typeof(OscOut))]
public class MRPM_SceneSyncManager : MonoBehaviour {

    OscIn oscIn;
    OscOut oscOut;
    Dictionary<string, string> SceneVariables;

	// Use this for initialization
	void Start () {
        oscIn = GetComponent<OscIn>();
        oscOut = GetComponent<OscOut>();
		oscIn.Open(8001, null);
        SceneVariables = new Dictionary<string, string>();
	}

    void OnEnabled()
    {
        oscIn.Map("/main/toCtrlr/sync", ParseOscMessage);
    }

    void OnDisabled()
    {
        oscIn.Unmap(ParseOscMessage);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void ParseOscMessage(OscMessage oscMessage){
        if (oscMessage.args.Count%2!=0)
        {
            Debug.LogWarning("Invalid Argument Count");
            return;
        }
        int kvCount = oscMessage.args.Count/2;
        for (int i = 0; i < kvCount; i++){
            string tmp1, tmp2;
            if (oscMessage.TryGet(i, out tmp1) && oscMessage.TryGet(i+1, out tmp2))
            {
                SceneVariables.Add(tmp1, tmp2);
            } else {
                Debug.LogWarning("Error Getting Data from OscMessage");
            }
        }
    }

    
}

}
