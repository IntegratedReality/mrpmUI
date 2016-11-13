using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace MRPM
{

[RequireComponent(typeof(OscIn))]
public class MRPM_SyncedObject : MonoBehaviour {
	int objectID;
	bool isMine;
	// Received String
	string mySyncState;

	Vector3 positionVelocity;
	Vector3 rotationVelocity;

	public static GameObject Instantiate(GameObject prefab,  int objectID, bool isMine, Vector3 position, Quaternion rotation, Transform parent = null, bool autoDestroyOnSyncStop = false)
	{
		GameObject obj = Instantiate(prefab);
		obj.GetComponent<MRPM_SyncedObject>().objectID = objectID;
		obj.GetComponent<MRPM_SyncedObject>().isMine = isMine;
		obj.transform.position = position;
		obj.transform.rotation = rotation;
		if (parent != null) {
			obj.transform.parent = parent.transform;
		}
		return obj;
	}

	// Use this for initialization
	void Start ()
	{

	}

	void Sync(ReactiveDictionary<int, string> syncState)
	{
		if (syncState.TryGetValue(objectID, out mySyncState))
		{
			var splitState = mySyncState.Split(new [] { '/' });
			float x, z, rot;
			int intDead, en, hp;
			//sync position smoothly
			//transform.position.Set(x, 0, z);
			float.TryParse(splitState[0], out x);
			float.TryParse(splitState[1], out z);
			transform.position = Vector3.SmoothDamp(transform.position, new Vector3(x, 0, z),  ref positionVelocity , Time.deltaTime);
			float.TryParse(splitState[2], out rot);
			// sync rotation
			transform.rotation.eulerAngles.Set(0, rot, 0);
		}
	}
}

}