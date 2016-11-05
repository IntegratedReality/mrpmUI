using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MRPM
{
public class MRPM_Player : MonoBehaviour {

	[HeaderAttribute ("Player's Info")]
	[SerializeField]
	[TooltipAttribute ("True if this is the player object this UI attached to")]
	bool isMine;
	[SerializeField]
	[TooltipAttribute ("the IPAddress of Raspbery Pi")]
	string piAddress;

	[HeaderAttribute ("Status List")]
	[SerializeField] float stamina;
	[SerializeField] float life;
	Vector3 position;
	Vector3 rotation;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}
}

}
