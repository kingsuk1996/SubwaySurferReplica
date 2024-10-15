using UnityEngine;

namespace JustGame.SubwaySurfer
{
    public class EnableCoinsInCoinSet : MonoBehaviour
    {
        internal void ActivateCoin()
        {
            foreach (Transform item in transform)
            {
                item.gameObject.SetActive(true);
            }
        }
    }
}
