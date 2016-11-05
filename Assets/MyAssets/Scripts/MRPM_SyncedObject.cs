using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MRPM
{

[RequireComponent(typeof(OscIn))]
public class MRPM_SyncedObject : MonoBehaviour {
	OscIn oscIn;

	[SerializeField]
	string ID;
	[SerializeField]
	int SyncPort;

	// Use this for initialization
	void Start ()
	{
		oscIn = GetComponent<OscIn>();
	}

	// Update is called once per frame
	void Update ()
	{

	}

	void Open()
	{
		if (!oscIn.isOpen){
			oscIn.Open(SyncPort, null);
		}
	}
}

}