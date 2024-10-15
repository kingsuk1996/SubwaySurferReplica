using UnityEngine;
namespace JustGame.SubwaySurfer
{
    public class ObstacleSetSpawner : MonoBehaviour
    {
        internal void Init()
        {
            GameObject _obstacleSet = ObjectPool.OnFetchingFromPool(RandomObstacleSet());
            _obstacleSet.SetActive(true);
            _obstacleSet.transform.SetParent(gameObject.transform);
            _obstacleSet.transform.position = transform.position;
            _obstacleSet.GetComponent<ResetObstacle>().ActivateObstacle();
        }

        private PoolObjectType RandomObstacleSet()
        {
            return (PoolObjectType)(Random.Range(10, 12));
        }
    }
}