using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
namespace RedApple.SubwaySurfer
{
    public class UImanager : MonoBehaviour
    {
        [SerializeField] private List<GameObject> Panels;

        [Space(10)]
        [SerializeField] private Text fpsCounter;
        [SerializeField] private Text totalDistanceCovered;
        [SerializeField] private Text totalCoinCollect;

        [Space(10)]
        [SerializeField] private Animator playerAnim;
        [SerializeField] private Animator dogAnim;
        [SerializeField] private Animator beeAnim;
        [SerializeField] private Animator laceAnim;
        [SerializeField] private Animator cameraAnim;
        [SerializeField] private Animator loadingAnim;

        [Space(10)]
        [SerializeField] private Image loadingBar;
        [SerializeField] private List<Image> lifeActive = new List<Image>();
        [SerializeField] private Sprite lifeInactive;
        [SerializeField] private Image autoPlayBar;

        [Space(10)]
        [SerializeField] private GameObject dontShow;
        [SerializeField] private GameObject startButtonINPagination;
        [SerializeField] private GameObject bee;
        [SerializeField] private GameObject shoelace;

        [Space(10)]
        [SerializeField] private RectTransform playButton;
        [SerializeField] private RectTransform start;
        [SerializeField] private RectTransform end;

        [Space(10)]
        [SerializeField] private int checkGameplayCount = 0;
        [SerializeField] private int setFps = 240;
        [SerializeField] private Toggle toggle;
        [SerializeField] private PlayerController playerController;
        [SerializeField] private GameSettings gameDataContainer;

        [Space(10)]
        [SerializeField] private Button skipButton;
        [SerializeField] private Button exitButton;

        [Space(10)]
        public float TotalDistance;
        public float TotalCoin;

        public Text Distance;
        public Text coinCounter;

        public static bool CanMove = false;
        public static UImanager Instance;

        private float deltaTime;

        private void Awake()
        {
            Instance = this;
            loadingBar.fillAmount = 0;
            autoPlayBar.fillAmount = 0;
            playButton.gameObject.SetActive(false);
            startButtonINPagination.SetActive(false);
            Panelhandler(GameConstants.LoadingPanel);
            StartCoroutine(Loading());

            if (PlayerPrefs.GetInt("Play") == 0)
            {
                skipButton.gameObject.SetActive(false);
                dontShow.SetActive(false);
                startButtonINPagination.SetActive(true);
            }

            if (PlayerPrefs.GetInt("Skip") == 1)
            {
                StartCoroutine(Loading());
            }
        }

        private IEnumerator Loading()
        {
            while (loadingBar.fillAmount != 1)
            {
                loadingBar.fillAmount += .004f;
                loadingAnim.SetTrigger("LoadingAnim");
                if (loadingBar.fillAmount == 1 && PlayerPrefs.GetInt("Skip") == 0)
                {
                    Panelhandler(GameConstants.InstructionPanel);
                }
                else if (loadingBar.fillAmount == 1)
                {
                    Panelhandler(GameConstants.IntroPanel);
                    StartCoroutine(IntroAnimStart());
                }
                yield return null;
            }
        }

        private void Start()
        {
            CanMove = false;
            Distance.text = "Distance : " + 0;
            Application.targetFrameRate = setFps;
            QualitySettings.vSyncCount = 0;
            AudioManager.Instance.Stop("GameOver");
        }

        public void Panelhandler(string PanelID)
        {
            foreach (GameObject panel in Panels)
            {
                panel.SetActive(panel.GetComponent<PanelIDKeeper>().panelID.Equals(PanelID));
            }
        }

        private void Update()
        {
            deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
            float fps = 1.0f / deltaTime;
            fpsCounter.text = "FPS : " + Mathf.Ceil(fps).ToString();
        }

      
        private void GameOver()
        {
            CanMove = false;
            playerAnim.enabled = false;
            AudioManager.Instance.Stop("BG Music");
            AudioManager.Instance.Play("GameOver");
            Panelhandler(GameConstants.GameOverPanel);
            totalCoinCollect.text = TotalCoin.ToString();
            totalDistanceCovered.text = TotalDistance.ToString();
            exitButton.onClick.AddListener(Application.Quit);
        }

        public void RestartButton()
        {
            CanMove = true;
            SceneManager.LoadScene(0);
        }

        private void PageChange(int _page)
        {
            if (_page == 3)
            {
                Panelhandler(GameConstants.IntroPanel);
                StartCoroutine(IntroAnimStart());
            }
        }

        private IEnumerator IntroAnimStart()
        {
            float duration = .5f;
            AudioManager.Instance.Play("Intro");
            cameraAnim.enabled = true;

            playerAnim.SetTrigger("Intro");
            dogAnim.SetTrigger("Intro");
            beeAnim.SetTrigger("Intro");
            laceAnim.SetTrigger("Intro");
            yield return new WaitForSeconds(13f);

            playerController.enabled = false;
            DogController.OnStart?.Invoke();
            playButton.gameObject.SetActive(true);
            yield return new WaitForSeconds(1f);

            Time.timeScale = 0;
            Debug.LogError("Cure");
            playButton.DOAnchorPos(end.anchoredPosition, duration).SetUpdate(true);
            DOTween.To(() => autoPlayBar.fillAmount, x => autoPlayBar.fillAmount = x, 1, 10f).SetUpdate(true).OnComplete(() => OnPlayButtonClick());

        }


        public void OnStartButtonClick()
        {
            checkGameplayCount++;
            if (checkGameplayCount >= 1)
            {
                PlayerPrefs.SetInt("Play", 1);
            }
            Panelhandler(GameConstants.IntroPanel);
            StartCoroutine(IntroAnimStart());
        }

        public void OnPlayButtonClick()
        {
            Time.timeScale = 1f;
            float duration = 1f;
            AudioManager.Instance.Stop("Intro");
            AudioManager.Instance.Play("BG Music");
            playButton.DOAnchorPos(start.anchoredPosition, duration).SetUpdate(true);
            playerController.enabled = true;
            Panelhandler(GameConstants.GamePlayPanel);
            PlayerController.OnStart?.Invoke();
            bee.SetActive(false);
            shoelace.SetActive(false);
        }

        public void OnSkipButtonClick()
        {
            if (toggle.isOn)
            {
                PlayerPrefs.SetInt("Skip", 1);
            }
           
            Panelhandler(GameConstants.IntroPanel);
            StartCoroutine(IntroAnimStart());
        }

        public void OnPauseButtonClick()
        {
            Time.timeScale = 0;
        }

        public void LifeSystem(int _life)
        {
            if (_life != 0)
            {
                lifeActive[_life].sprite = lifeInactive;
            }
        }

        private void OnEnable()
        {
            PlayerManager.OnPlayerCrushed += GameOver;
            CountdownTimer.Instance.TimesUp += GameOver;
            //ScrollSnapBase.OnPageChange += PageChange;
        }

        private void OnDisable()
        {
            PlayerManager.OnPlayerCrushed -= GameOver;
            CountdownTimer.Instance.TimesUp -= GameOver;
            //ScrollSnapBase.OnPageChange -= PageChange;
        }
    }
}