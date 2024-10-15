using TMPro;
using UnityEngine;

namespace JustGame.SubwaySurfer
{
    public class GamePlayTimer : MonoBehaviour
    {
        [SerializeField] private float timeLeft; // Set the initial time here (in seconds)
        [SerializeField] private TMP_Text timerText;
        [SerializeField] private float IncreaseSpeedOverTime = 30;

        private float initialTime;
        private bool isSpeedIncrease = false;


        private void OnEnable()
        {
            Timer();
            initialTime = timeLeft;
        }

        private void Update()
        {
            if (GameConstants.CurrentGamePlayState == GamePlayState.Play)
            {
                Timer();
            }
        }

        private void Timer()
        {
            timeLeft -= Time.deltaTime;

            int hours = Mathf.FloorToInt(timeLeft / 3600.0f);
            int minutes = Mathf.FloorToInt((timeLeft - (hours * 3600.0f)) / 60.0f);
            int seconds = Mathf.FloorToInt(timeLeft - (hours * 3600.0f) - (minutes * 60.0f));

            timerText.text = minutes.ToString("00") + ":" + seconds.ToString("00");

            if (timeLeft <= 0)
            {
                hours = 0;
                minutes = 0;
                seconds = 0;
                timerText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
                GameManager.Instance.ChangePanelState(PanelStates.GameOver);
                UIManager.Instance.TimesUp.text = "Time's UP";
            }

            if (Mathf.Round(timeLeft) == (Mathf.Round(initialTime) - IncreaseSpeedOverTime) && isSpeedIncrease == false)
            {
                isSpeedIncrease = true;
                //BlockProperties.OnBlockSpeed?.Invoke();
            }
        }
    }
}
