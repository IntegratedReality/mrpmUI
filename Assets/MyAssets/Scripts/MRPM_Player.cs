using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace MRPM
{
public class MRPM_Player : MonoBehaviour
{
	[HeaderAttribute("Player's Info")]
	[SerializeField]
	[TooltipAttribute("True if this is the player object this UI attached to")] public bool isMine;
	[SerializeField][TooltipAttribute("Robot ID")] public int robotID;
	[SerializeField] string mySyncState;

	[HeaderAttribute("Status List")]
	ReactiveProperty<int> Energy;
	ReactiveProperty<int> HitPoint;
	ReactiveProperty<int> IsDead;

	Vector3 positionVelocity;
	Vector3 rotationVelocity;
	MRPM_GeneralManager gm;
	public ParticleSystem _deathEffect;
	public Slider _healthBar;
	public MRPM_HUDController _hudController;

	public static GameObject Instantiate(GameObject prefab,  int robotID, bool isMine, Vector3 position, Quaternion rotation, Transform parent = null)
	{
		GameObject obj = Instantiate(prefab);
		obj.GetComponent<MRPM_Player>().robotID = robotID;
		obj.GetComponent<MRPM_Player>().isMine = isMine;
		obj.transform.position = position;
		obj.transform.rotation = rotation;
		if (parent != null) {
			obj.transform.parent = parent.transform;
		}
		return obj;
	}

	// Use this for initialization
	void Start()
	{
		gm = MRPM_GeneralManager._instance;
		Energy = new ReactiveProperty<int>(100);
		HitPoint = new ReactiveProperty<int>(100);
		IsDead = new ReactiveProperty<int>(0);

		HitPoint.ObserveEveryValueChanged(x => x.Value).Buffer(2)
		.Subscribe(x =>
		{
			if (x[0] - x[1] > 0)
			{
				// regenerating
				if (isMine){

				}
			}
			else
			{
				// damaged
				if (isMine){

				}
			}
			//update health bar
			_healthBar.value = x[0];
		});

		var isDeadStream = IsDead.ObserveEveryValueChanged(x => x.Value);
		isDeadStream.Subscribe(
		  x =>
		{
			if (x > 0)
			{
				if (isMine){
					// show death timer
					_hudController.ShowDeathTimer(IsDead.Value);
				}
				_deathEffect.Play();
			}
			else
			{
				// revive
				if (isMine)
				{
					// remove death screen
					_hudController.ShowDeathTimer(IsDead.Value);
				}
				// show revive effect
				//
			}
		});

		/*MRPM_SceneSyncManager._instance.SceneVariables
		.ObserveEveryValueChanged(x => x)
		.Subscribe(x =>
		{
			Sync(x);
		})
		.AddTo(this);*/

		Observable.EveryUpdate()
		.Subscribe(_ =>
		{
			Sync(MRPM_SceneSyncManager._instance.SceneVariables);
		})
		.AddTo(this);

		if (isMine)
		{
			_hudController = MRPM_HUDController._instance;
		}
	}

	void Sync(ReactiveDictionary<int, string> syncState)
	{
		if (robotID%10 != 0){
			return;
		}
		if (syncState.TryGetValue(robotID, out mySyncState))
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
			// sync rotation smoothly
			transform.rotation.eulerAngles.Set(0, rot, 0);
			int.TryParse(splitState[3], out en);
			Energy.Value = en;
			int.TryParse(splitState[4], out hp);
			HitPoint.Value = hp;
			int.TryParse(splitState[5], out intDead);
			IsDead.Value = intDead;
		}
	}
}

}
