using System.Collections.Generic;
using UnityEngine;

namespace MRPM
{

public class MRPM_Bullet : MonoBehaviour {

    MRPM_SceneSyncManager sceneSyncManager;
    public int _objectID;

    // Use this for initialization
    void Start () {
        sceneSyncManager = MRPM_SceneSyncManager._instance;
    }

    // Update is called once per frame
    void Update () {

    }
}

}