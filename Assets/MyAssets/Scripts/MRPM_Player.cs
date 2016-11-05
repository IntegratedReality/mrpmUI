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
	[TooltipAttribute ("Robot ID")]
	string robotID;

	[HeaderAttribute ("Status List")]
	[SerializeField] float energy;
	[SerializeField] float hp;
	[SerializeField] int isDead;

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {

	}

	public void Fire() {

	}

	public void DamagedEffect(){

	}

	public void EffectDead(){

	}
}

}
