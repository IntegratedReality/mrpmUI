using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MRPM
{

    public class MRPM_ControllerManager : MonoBehaviour
    {

        public OscOut _oscOutToMain;
        public OscOut _oscOutToRobot;
        string robotID;

        MRPM_GeneralManager gm;

        static public MRPM_ControllerManager _instance;
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
            if (_oscOutToMain)
            {
                _oscOutToMain.Open(gm.PORT_MAINRCV, gm.mainHostAddress);
            }
            if (_oscOutToRobot)
            {
                _oscOutToRobot.Open(gm.PORT_ROBOT, gm.myRobotAddress);
            }
            robotID = gm.myRobotID;
        }

        // Update is called once per frame
        void Update()
        {
            if (_oscOutToMain != null && isFire())
            {
                _oscOutToMain.Send(gm.ADDRESS_TO_Main_SHOOT, robotID);
            }
            if (_oscOutToRobot != null)
            {
                int order = RobotControlOrder();
                Debug.Log(order);
                _oscOutToRobot.Send(gm.ADDRESS_TO_ROBOT, order);
            }
        }

        void OnEnabled()
        {
            if (_oscOutToMain)
            {
                _oscOutToMain.Open(gm.PORT_MAINRCV, gm.mainHostAddress);
            }
            if (_oscOutToRobot)
            {
                _oscOutToMain.Open(gm.PORT_ROBOT, gm.myRobotAddress);
            }
            robotID = gm.myRobotID;
        }

        void OnDisabled()
        {
            _oscOutToMain.Close();
            _oscOutToRobot.Close();
        }

        int RobotControlOrder()
        {
            int h = 1;
            int v = 1;
            if (Input.GetKey(KeyCode.RightArrow) ){
                h += 1;
            }
            if (Input.GetKey(KeyCode.LeftArrow) ){
                h -= 1;
            }
            if (Input.GetKey(KeyCode.UpArrow) ){
                v += 1;
            }
            if (Input.GetKey(KeyCode.DownArrow) ){
                v -= 1;
            }
            int op = h + v*3;
            return directionConvertArray[op];
        }

        int[] directionConvertArray = {6, 5, 4, 7, 0, 3, 8, 1, 2};

        /*
        enum EDirection {
    NO_INPUT,
    TOP,
    TOP_RIGHT,
    RIGHT,
    BOTTOM_RIGHT,
    BOTTOM,
    BOTTOM_LEFT,
    LEFT,
    TOP_LEFT
};
        */

        bool isFire()
        {
            if (Input.GetKeyDown("space"))
            {
                Debug.Log("Fire!");
                return true;
            }
            return false;
        }
    }

}