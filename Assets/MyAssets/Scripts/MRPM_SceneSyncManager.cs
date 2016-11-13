using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace MRPM
{

    [RequireComponent(typeof(OscIn))]
    public class MRPM_SceneSyncManager : MonoBehaviour
    {
        public GameObject _robotPrefab;
        public GameObject _bulletPrefab;
        public Transform _syncObjParent;

        OscIn oscIn;
        public ReactiveDictionary<int, string> SceneVariables { get; private set; }
        MRPM_GeneralManager gm;

        static public MRPM_SceneSyncManager _instance;
        void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        // Use this for initialization
        void Start()
        {
            gm = MRPM_GeneralManager._instance;
            oscIn = GetComponent<OscIn>();
            oscIn.Open(gm.PORT_OPERATOR, null);
            oscIn.Map(gm.ADDRESS_SYNC, ParseOscMessage);
            SceneVariables = new ReactiveDictionary<int, string>();
            //新しいオブジェクトを生成
            SceneVariables.ObserveAdd().Subscribe(x =>
                {
                    SpawnSyncedObject(x);
                });

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
        void Update()
        {

        }

        void ParseOscMessage(OscMessage oscMessage)
        {
            if (oscMessage.args.Count % 2 != 0)
            {
                Debug.LogWarning("Invalid Argument Count");
                return;
            }
            int kvCount = oscMessage.args.Count / 2;
            Debug.Log(kvCount);
            for (int i = 0; i < kvCount ; i++)
            {
                int objectID = 0;
                string stringID;
                string syncValue;
                bool isIDreadable = oscMessage.TryGet(i * 2, out stringID);
                bool isValueReadable = oscMessage.TryGet(i * 2 +1, out syncValue);
                if (!isIDreadable){
                    Debug.LogWarning("Error Getting Data from OscMessage");
                } else {
                    objectID = int.Parse(stringID);
                }
                if (!isValueReadable){
                    Debug.LogWarning("Error Getting Data from OscMessage");
                }
                if (isIDreadable && isValueReadable)
                {
                    if (!SceneVariables.ContainsKey(objectID))
                    {
                        SceneVariables.Add(objectID, syncValue);
                    }
                    else
                    {
                        SceneVariables[objectID] = syncValue;
                    }
                }
            }
        }

        void SpawnSyncedObject(DictionaryAddEvent<int, string> addEvent)
        {
            int keyInt = addEvent.Key;
            if (keyInt == 90)
            {
                //this is grobal info
                //Do nothing here
            }
            else if (keyInt % 10 == 0)
            {
                //this is robot's id
                SpawnRobot(addEvent);
            }
            else
            {
                //this is object's id
                SpawnObject(addEvent);
            }
        }

        void SpawnRobot(DictionaryAddEvent<int, string> addEvent)
        {
            var addEventvalue = addEvent.Value.Split(new [] { '/' });
            if (gm.myRobotID == addEvent.Key)
            {
                //Instantiate my robot with isMine = true;
                var robot = MRPM_Player.Instantiate(_robotPrefab, addEvent.Key, true, new Vector3(float.Parse(addEventvalue[0]), 0, float.Parse(addEventvalue[1])), Quaternion.Euler(0, float.Parse(addEventvalue[2]), 0), _syncObjParent);
                //this is my robot so attach camera
                Camera.main.transform.parent = robot.transform;
                Camera.main.transform.localPosition = new Vector3(0, 0.5f, 0);
                Camera.main.transform.rotation = Quaternion.identity;
            }
            else
            {
                MRPM_Player.Instantiate(_robotPrefab, addEvent.Key, false, new Vector3(float.Parse(addEventvalue[0]), 0, float.Parse(addEventvalue[1])), Quaternion.Euler(0, float.Parse(addEventvalue[2]), 0), _syncObjParent);
            }
        }

        void SpawnObject(DictionaryAddEvent<int, string> addEvent)
        {
            var addEventvalue = addEvent.Value.Split(new [] { '/' });
            if (gm.myRobotID == addEvent.Key)
            {
                //Instantiate my robot with isMine = true;
                MRPM_SyncedObject.Instantiate(_bulletPrefab, addEvent.Key, true, new Vector3(float.Parse(addEventvalue[0]), 0, float.Parse(addEventvalue[1])), Quaternion.Euler(0, float.Parse(addEventvalue[2]), 0), _syncObjParent);
            }
            else
            {
                MRPM_SyncedObject.Instantiate(_bulletPrefab, addEvent.Key, false, new Vector3(float.Parse(addEventvalue[0]), 0, float.Parse(addEventvalue[1])), Quaternion.Euler(0, float.Parse(addEventvalue[2]), 0), _syncObjParent);
            }
        }
    }

}
