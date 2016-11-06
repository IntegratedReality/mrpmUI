using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace MRPM
{
public class MRPM_Player : MonoBehaviour {

	[HeaderAttribute ("Player's Info")]
	[SerializeField]
	[TooltipAttribute ("True if this is the player object this UI attached to")] bool isMine;
	[SerializeField][TooltipAttribute ("Robot ID")] string robotID;
	[SerializeField] string mySyncState;

	[HeaderAttribute ("Status List")]
	public ReactiveProperty<float> Energy;
	public ReactiveProperty<float> HitPoint;
	public ReactiveProperty<int> IsDead;

	MRPM_GeneralManager gm;

	// Use this for initialization
	void Start () {
		gm = MRPM_GeneralManager._instance;
		Energy = new ReactiveProperty<float>();
		HitPoint = new ReactiveProperty<float>();
		if(gm.myRobotID == robotID){
			isMine = true;
		}
	}

	// Update is called once per frame
	void Update () {
		Sync();
	}

	void Sync(){
		MRPM_SceneSyncManager._instance.SceneVariables.TryGetValue(robotID, out mySyncState);
		var splitState = mySyncState.Split(new [] {'/'});
		float x,z,rot,en,hp;
		int intDead;
		float.TryParse(splitState[0], out x);
		float.TryParse(splitState[1], out z);
		transform.position.Set(x, 0, z);
		float.TryParse(splitState[2], out rot);
		transform.rotation.eulerAngles.Set(0, rot, 0);
		float.TryParse(splitState[3], out en);
		Energy.Value = en;
		float.TryParse(splitState[4], out hp);
		HitPoint.Value = hp;
		int.TryParse(splitState[5], out intDead);
		IsDead.Value = intDead;
	}
}

}
