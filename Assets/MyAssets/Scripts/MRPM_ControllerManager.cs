using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MRPM
{

[RequireComponent(typeof(OscOut))]
public class MRPM_ControllerManager : MonoBehaviour {

	[SerializeField] int inputPattern;
	OscOut oscOut;

	// Use this for initialization
	void Start () {
		oscOut = GetComponent<OscOut>();
	}

	// Update is called once per frame
	void Update () {

	}

}

}