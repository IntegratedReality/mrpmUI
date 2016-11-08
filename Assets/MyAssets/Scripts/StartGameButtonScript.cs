using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MRPM;

[RequireComponent(typeof(OscOut))]
public class StartGameButtonScript : MonoBehaviour
{

    MRPM_GeneralManager _generalManager;
    Button startButton;
    OscOut oscOut;

    // Use this for initialization
    void Start()
    {
        _generalManager = MRPM_GeneralManager._instance;
        startButton = GetComponent<Button>();
        startButton.interactable = false;
        oscOut = GetComponent<OscOut>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnAuthorized()
    {
        startButton.interactable = true;
    }

    public void SendACK()
    {
        string myRobotID = _generalManager.myRobotID;
        if (myRobotID != null)
        {
            Debug.Log("Send ACK");
            oscOut.Open(_generalManager.PORT_MAINRCV, _generalManager.mainHostAddress);
            oscOut.Send(_generalManager.ADDRESS_ACK, int.Parse(myRobotID));
        }
    }
}
