using DevCommon;
using UnityEngine;
using UnityEngine.UI;

namespace JustGame.SubwaySurfer
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] private PlayerController playerController;
        [SerializeField] private PlayerManager playerManager;
        [SerializeField] private UIManager uIManager;
        [SerializeField] private PanelController panelController;
        [SerializeField] private GameSettings gameSettings;

        protected override void Awake()
        {
            base.Awake();
            InitMethod();
        }

        private void InitMethod()
        {
            playerController.Init();
            playerManager.Init();
            uIManager.Init();
        }

        internal void ChangePanelState(PanelStates _panelState)
        {
            GameConstants.CurrentPanelState = _panelState;
            switch (_panelState)
            {
                case PanelStates.Loading:
                    panelController.Panelhandler(GameConstants.CurrentPanelState);
                    break;

                case PanelStates.Instruction:
                    panelController.Panelhandler(GameConstants.CurrentPanelState);
                    break;

                case PanelStates.Intro:
                    panelController.Panelhandler(GameConstants.CurrentPanelState);
                    break;

                case PanelStates.GamePlay:
                    panelController.Panelhandler(GameConstants.CurrentPanelState);
                    break;

                case PanelStates.GameOver:
                    panelController.Panelhandler(GameConstants.CurrentPanelState);
                    uIManager.InitGameOver();
                    GameConstants.CurrentGamePlayState = GamePlayState.None;
                    break;
            }
        }
    }
}