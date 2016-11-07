using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UniRx;

namespace MRPM
{
    public class MRPM_Player : MonoBehaviour
    {

        [HeaderAttribute("Player's Info")]
        [SerializeField]
        [TooltipAttribute("True if this is the player object this UI attached to")] public bool isMine;
        [SerializeField][TooltipAttribute("Robot ID")] public string robotID = null;
        [SerializeField] string mySyncState;

        [HeaderAttribute("Status List")]
        public ReactiveProperty<float> Energy;
        public ReactiveProperty<float> HitPoint;
        public ReactiveProperty<int> IsDead;

        MRPM_GeneralManager gm;
        public ParticleSystem _deathEffect;

        // Use this for initialization
        void Start()
        {
            gm = MRPM_GeneralManager._instance;
            Energy = new ReactiveProperty<float>();
            HitPoint = new ReactiveProperty<float>();
            IsDead = new ReactiveProperty<int>(0);
            IsDead.Subscribe(
                x =>
                {
                    if (x > 0)
                    {
                        StartCoroutine("DeathTimer");
                    }
                });

            MRPM_SceneSyncManager._instance.SceneVariables
            	.ObserveEveryValueChanged(x => x)
        		.Subscribe(x => Sync(x)).AddTo(this);
        }

        //DeathTimer
        IEnumerator DeathTimer()
        {
            _deathEffect.Play();
            while (IsDead.Value > 0)
            {

                yield return new WaitForSeconds(1);
            }
        }

        // Update is called once per frame
        void Update()
        {
        }

        void Sync(ReactiveDictionary<string,string> syncState)
        {
        	if (robotID == null) return;
            if (syncState.TryGetValue(robotID, out mySyncState))
            {
                var splitState = mySyncState.Split(new [] { '/' });
                float x, z, rot, en, hp;
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

}
