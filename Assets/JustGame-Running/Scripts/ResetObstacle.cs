using UnityEngine;

namespace JustGame.SubwaySurfer
{
    public class ResetObstacle : MonoBehaviour
    {
        [SerializeField] private PoolObjectType _currentObstacleSet;
        [SerializeField] private EnableCoinsInCoinSet enableCoinsInCoinSet;

        internal void ActivateObstacle()
        {
            enableCoinsInCoinSet.ActivateCoin();
            foreach (Transform item in transform)
            {
                item.gameObject.SetActive(true);
            }
        }

        void Update()
        {
            if (gameObject.transform.position.z >= 2)
            {
                ObjectPool.OnReturningToPool(gameObject, _currentObstacleSet);
            }
        }
    }
}
