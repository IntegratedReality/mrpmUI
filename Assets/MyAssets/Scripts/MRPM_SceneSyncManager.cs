using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace MRPM
{

[RequireComponent(typeof(OscIn))]
public class MRPM_SceneSyncManager : MonoBehaviour {

    OscIn oscIn;
    public ReactiveDictionary<string, string> SceneVariables{get; private set;}
    MRPM_GeneralManager gm;

    static public MRPM_SceneSyncManager _instance;
    void Awake(){
        if (_instance == null){
            _instance = this;
        } else {
            Destroy(gameObject);
        }
    }

	// Use this for initialization
	void Start () {
        gm = MRPM_GeneralManager._instance;
        oscIn = GetComponent<OscIn>();
		oscIn.Open(gm.PORT_OPERATOR, null);
        oscIn.Map(gm.ADDRESS_SYNC, ParseOscMessage);
        SceneVariables = new ReactiveDictionary<string, string>();
	}

    void OnEnabled()
    {
        oscIn.Map(gm.ADDRESS_SYNC, ParseOscMessage);
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
            string objectID, syncValue;
            if (oscMessage.TryGet(i, out objectID) && oscMessage.TryGet(i+1, out syncValue))
            {
                SceneVariables.Add(objectID, syncValue);
            } else {
                Debug.LogWarning("Error Getting Data from OscMessage");
            }
        }
    }
}

}
