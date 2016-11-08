using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System;

namespace MRPM
{

    [RequireComponent(typeof(OscIn), typeof(OscOut))]
    public class MRPM_PlayerAuthorization : MonoBehaviour
    {
        OscIn oscIn;
        OscOut oscOut;
        public Text _authDataText;

        [HeaderAttribute("Auth Data Info")]
        [SerializeField]
        string robotID;
        [SerializeField]
        string robotHostName;

        MRPM_GeneralManager generalMagnager;
        public StartGameButtonScript _startButton;
        UniRx.IObservable<OscMessage> authStream;
        IDisposable stream;

        // Use this for initialization
        void Start()
        {
            generalMagnager = MRPM_GeneralManager._instance;
            oscIn = GetComponent<OscIn>();
            oscOut = GetComponent<OscOut>();
            authStream = oscIn.onAnyMessage.AsObservable();
        }

        public void OnClick()
        {
            Debug.Log("AuthButtonClick");
            if (!oscIn.isOpen)
            {
                oscIn.Open(generalMagnager.PORT_MAINRCV, null);
                oscIn.Map("/main/toCtrlr/assign", CheckAuthMessage);
            }
            _authDataText.text = "Authorizing...";
            stream = authStream.Timeout(System.TimeSpan.FromSeconds(20)).Subscribe(
            	x =>
            	{
            		CheckAuthMessage(x);
            	},
            	ex =>
            	{
            		Debug.Log("timeout");
            		_authDataText.text = "Timeout";
            	}).AddTo(this);
        }



        // Update is called once per frame
        void Update()
        {

        }

        public void CheckAuthMessage(OscMessage message)
        {
            //Show GUI
            if (message.TryGet(0, out robotID) && message.TryGet(1, out robotHostName))
            {
                if (generalMagnager.mainHostName != null)
                    OnAuthorized();
            }
        }

        void OnAuthorized()
        {
            oscIn.Close();
            oscOut.Open(generalMagnager.PORT_MAINRCV, generalMagnager.mainHostName);
            generalMagnager.myRobotID = robotID;
            generalMagnager.myRobotHostName = robotHostName;
            _authDataText.text = "robotID: " + robotID + " robotHostName: " + robotHostName;
            _startButton.OnAuthorized();
            stream.Dispose();
        }
    }

}

