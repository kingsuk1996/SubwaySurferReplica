using System;
using System.Collections;
using System.Collections.Generic;
using DevCommon;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace JustGame.SubwaySurfer
{
    public class UIManager : Singleton<UIManager>
    {
        [Space(10)]
        [SerializeField] private PlayerController playerController;
        [SerializeField] private Animator playerAnim;
        [SerializeField] private Animator introCamAnim;

        [Space(10)]
        [SerializeField] private Image loadingBar;
        [SerializeField] private Image autoPlayBar;
        [SerializeField] private Image audioMainImage;
        [SerializeField] private Sprite audioON;
        [SerializeField] private Sprite audioOFF;

        [Space(10)]
        [SerializeField] private RectTransform runButton;
        [SerializeField] private ParticleSystem speedEffect;

        [Space(10)]
        [Header("LifeSystem Related")]
        [SerializeField] private List<Image> life;
        [SerializeField] private Sprite lifeLost;

        [Space(10)]
        [Header("Camera Related")]
        [SerializeField] private GameObject introCam;
        [SerializeField] private GameObject followCam;

        [Space(10)]
        [Header("Gameplay Panel")]
        public TMP_Text CurrentDistance;
        public TMP_Text CurrentCoinCollection;

        [Space(10)]
        [Header("Gameover Panel")]
        [SerializeField] private TMP_Text totalDistanceCovered;
        [SerializeField] private TMP_Text totalCoinCollected;
        public TMP_Text TimesUp;

        private bool isAudioOn = false;
        private bool IsPauseGame = false;
        private float duration = 1;
        private int _distance = 0;

        internal void Init()
        {
            loadingBar.fillAmount = 0;
            autoPlayBar.fillAmount = 0;
            introCam.SetActive(true);
            followCam.SetActive(false);
            isAudioOn = true;

            if (PlayerPrefs.GetInt(GameConstants.Replay) == 1)
            {
                Invoke(nameof(ReadyToPlay), 0.1f);
            }
            else
            {
                LoadingState();
            }
            PlayerPrefs.SetInt(GameConstants.Replay, 0);
        }

        private void LoadingState()
        {
            GameManager.Instance.ChangePanelState(PanelStates.Loading);
            StartCoroutine(Loading());
        }

        IEnumerator Loading()
        {
            while (loadingBar.fillAmount != 1)
            {
                loadingBar.fillAmount += .07f;
                yield return null;
            }
            GameManager.Instance.ChangePanelState(PanelStates.Instruction);
            AudioManager.Instance.Play(AudioType.BackgroundMusic);
        }

        public void OnSkipButtonInInstructionPanel()
        {
            AudioController(AudioType.ButtonClick);
            GameManager.Instance.ChangePanelState(PanelStates.Intro);
            introCamAnim.enabled = true;
            playerAnim.SetBool("Intro", true);
            Invoke(nameof(ReadyToPlay), 18f);
        }

        public void OnSkipButtonInIntroPanel()
        {
            AudioController(AudioType.ButtonClick);
            ReadyToPlay();
        }

        private void ReadyToPlay()
        {
            introCam.SetActive(false);
            followCam.SetActive(true);
            playerAnim.SetBool("Intro", false);
            GameManager.Instance.ChangePanelState(PanelStates.GamePlay);
            runButton.DOAnchorPosY(200, duration).SetUpdate(true).OnComplete(() => DOTween.To(() => autoPlayBar.fillAmount, x => autoPlayBar.fillAmount = x, 1, 10f).SetUpdate(true).OnComplete(() => OnRunButtonClick()));
        }

        public void OnRunButtonClick()
        {
            AudioController(AudioType.ButtonClick);
            Invoke(nameof(GameStart), 0.1f);
        }

        private void GameStart()
        {
            runButton.DOAnchorPosY(-250, 0.2f).SetUpdate(true).OnComplete(() => runButton.gameObject.SetActive(false));
            GameConstants.CurrentGamePlayState = GamePlayState.Play;
            playerAnim.SetTrigger("Run");
            speedEffect.Play();
        }

        public void OnPauseButtonClick()
        {
            if (GameConstants.CurrentGamePlayState == GamePlayState.Play || GameConstants.CurrentGamePlayState == GamePlayState.Pause)
            {
                if (IsPauseGame)
                {
                    IsPauseGame = false;
                    playerAnim.enabled = true;
                    GameConstants.CurrentGamePlayState = GamePlayState.Play;
                }
                else
                {
                    IsPauseGame = true;
                    playerAnim.enabled = false;
                    GameConstants.CurrentGamePlayState = GamePlayState.Pause;
                }
            }
        }

        public void OnReplayButtonClick()
        {
            PlayerPrefs.SetInt(GameConstants.Replay, 1);
            SceneManager.LoadScene(0);
        }

        public void OnClickAudioButton()
        {
            if (isAudioOn)
            {
                isAudioOn = false;
                audioMainImage.sprite = audioOFF;
                AudioManager.Instance.Stop(AudioType.BackgroundMusic);
            }
            else
            {
                isAudioOn = true;
                audioMainImage.sprite = audioON;
                AudioManager.Instance.Play(AudioType.BackgroundMusic);
            }
        }

        internal void AudioController(AudioType _audioType)
        {
            if (isAudioOn)
            {
                AudioManager.Instance.PlayOneShot(_audioType);
            }
            else
            {
                AudioManager.Instance.Stop(_audioType);
            }
        }

        internal void UpdateLifeSystem(byte _noOfLives)
        {
            life[_noOfLives].sprite = lifeLost;
        }

        internal void InitGameOver()
        {
            totalCoinCollected.text = GameConstants.Score.ToString();
            totalDistanceCovered.text = _distance.ToString();
        }

        internal void SetDistance(float distance)
        {
            _distance = (int)distance;
            CurrentDistance.text = _distance.ToString();
        }
    }
}