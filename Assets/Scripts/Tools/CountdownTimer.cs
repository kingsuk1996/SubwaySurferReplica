using System;
using UnityEngine;
using UnityEngine.UI;

namespace RedApple.SubwaySurfer
{
    public class CountdownTimer : MonoBehaviour
    {
        [SerializeField] private float timeLeft; // Set the initial time here (in seconds)
        [SerializeField] private Text timerText;

        public Action TimesUp;
        public static CountdownTimer Instance;

        public float IncreaseSpeedOverTime = 30;
        private bool isSpeedIncrease = false;
        private void Awake()
        {
            Instance = this;
        }

        public float TimeLeft { get => timeLeft; set => timeLeft = value; }

        private void Start()
        {
            Timer();
        }
        void Update()
        {
            if (UImanager.Instance.CanMove)
            {
                Timer();
            }
        }

        private void Timer()
        {
            TimeLeft -= Time.deltaTime;

            int hours = Mathf.FloorToInt(TimeLeft / 3600.0f);
            int minutes = Mathf.FloorToInt((TimeLeft - (hours * 3600.0f)) / 60.0f);
            int seconds = Mathf.FloorToInt(TimeLeft - (hours * 3600.0f) - (minutes * 60.0f));

            timerText.text = /*"Time Left : " + hours.ToString("00") + ":" + */minutes.ToString("00") + ":" + seconds.ToString("00");

            if (TimeLeft <= 0)
            {
                hours = 0;
                minutes = 0;
                seconds = 0;
                timerText.text = /*hours.ToString("00") + ":" +*/ minutes.ToString("00") + ":" + seconds.ToString("00");
                TimesUp?.Invoke();
            }
            if (Mathf.Round(timeLeft) == IncreaseSpeedOverTime && isSpeedIncrease == false)
            {
                isSpeedIncrease = true;
                BlockSpeedController.Instance.OnBlockSpeed?.Invoke();
            }

        }
    }
}
