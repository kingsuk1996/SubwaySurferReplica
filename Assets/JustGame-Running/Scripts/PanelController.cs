using System.Collections.Generic;
using UnityEngine;

namespace JustGame.SubwaySurfer
{
    public class PanelController : MonoBehaviour
    {
        [Space(10)]
        [SerializeField] private List<GameObject> Panels;

        internal void Panelhandler(PanelStates _currentGameState)
        {
            foreach (var panel in Panels)
            {
                if (panel.GetComponent<PanelStateKeeper>().Panel_State == _currentGameState)
                {
                    panel.gameObject.SetActive(true);
                }
                else
                {
                    panel.gameObject.SetActive(false);
                }
            }
        }
    }
}