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
        public ReactiveDictionary<string, string> SceneVariables { get; private set; }
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
            SceneVariables = new ReactiveDictionary<string, string>();
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
            for (int i = 0; i < kvCount; i++)
            {
                string objectID, syncValue;
                if (oscMessage.TryGet(i, out objectID) && oscMessage.TryGet(i + 1, out syncValue))
                {
                    SceneVariables.Add(objectID, syncValue);
                }
                else
                {
                    Debug.LogWarning("Error Getting Data from OscMessage");
                }
            }
        }

        void SpawnSyncedObject(DictionaryAddEvent<string, string> addEvent)
        {
            int keyInt = int.Parse(addEvent.Key);
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
                //this is bullet's id
                SpawnBullet(addEvent);
            }
        }

        void SpawnRobot(DictionaryAddEvent<string, string> addEvent)
        {
            var addEventvalue = addEvent.Value.Split(new [] { '/' });
            //var robot = Instantiate(_robotPrefab, new Vector3(float.Parse(addEventvalue[0]), 0, float.Parse(addEventvalue[1])), Quaternion.Euler(0, float.Parse(addEventvalue[2]), 0), _syncObjParent);
            if (gm.myRobotID == addEvent.Key)
            {
                //Instantiate my robot with isMine = true;
                var robot = MRPM_Player.Instantiate(_robotPrefab, addEvent.Key, true, new Vector3(float.Parse(addEventvalue[0]), 0, float.Parse(addEventvalue[1])), Quaternion.Euler(0, float.Parse(addEventvalue[2]), 0), _syncObjParent);
                //this is my robot so attach camera
                Camera.main.transform.parent = robot.transform;
                Camera.main.transform.localPosition = new Vector3(0, 0.5f, 0);
                Camera.main.transform.rotation = Quaternion.identity;
                // connect HUD with this robot
                MRPM_HUDController._instance._myPlayer = robot.GetComponent<MRPM_Player>();
            }
            else
            {
                var robot = MRPM_Player.Instantiate(_robotPrefab, addEvent.Key, false, new Vector3(float.Parse(addEventvalue[0]), 0, float.Parse(addEventvalue[1])), Quaternion.Euler(0, float.Parse(addEventvalue[2]), 0), _syncObjParent);
            }
        }

        void SpawnBullet(DictionaryAddEvent<string, string> addEvent)
        {
            var addEventvalue = addEvent.Value.Split(new [] { '/' });
            var bullet = Instantiate(_bulletPrefab, new Vector3(float.Parse(addEventvalue[0]), 0, float.Parse(addEventvalue[1]))
                                 , Quaternion.Euler(0, float.Parse(addEventvalue[2]), 0), _syncObjParent);
            bullet.GetComponent<MRPM_Bullet>()._objectID = addEvent.Key;
        }
    }

}
