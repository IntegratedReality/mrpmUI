using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MRPM
{

    public class MRPM_HUDController : MonoBehaviour
    {
        public Text _deathTimerText;
        public Image _characterImage;
        public Slider _healthSlider;
        public Slider _energySlider;
        public Image _deathScreen;

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
            // initialize the HUD param
            _deathScreen.enabled = false;
            _deathTimerText.text = "";
        }

        // Update is called once per frame

        public void UpdateHealthSlider(int currentHp, int maxHealth){
            _healthSlider.maxValue = maxHealth;
            _healthSlider.value = currentHp;
        }

        public void ShowDeathTimer(int timer = 0){
            if (timer == 0){
                _deathScreen.enabled = false;
                _deathTimerText.text = "";
            }
            else
            {
                _deathScreen.enabled= true;
                _deathTimerText.text = "You revive in"+ timer + "seconds...";
            }
        }
    }
}
