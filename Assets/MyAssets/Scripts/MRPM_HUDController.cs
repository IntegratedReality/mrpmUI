using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MRPM
{

    public class MRPM_HUDController : MonoBehaviour
    {
        public MRPM_Player _myPlayer;
        public Text _deathTimerText;
        public Image _characterImage;
        public Slider _healthSlider;
        public Slider _energySlider;

        static public MRPM_HUDController _instance;
        void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void UpdateHealthSlider(int currentHp, int maxHealth){
            _healthSlider.maxValue = maxHealth;
            _healthSlider.value = currentHp;
        }
    }
}
