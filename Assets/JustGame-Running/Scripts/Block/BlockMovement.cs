using UnityEngine;
namespace JustGame.SubwaySurfer
{
    public class BlockMovement : MonoBehaviour
    {
        [Space(10)]
        [SerializeField] private PoolObjectType _currentBlockType;

        private void Update()
        {
            if (GameConstants.CurrentGamePlayState == GamePlayState.Play)
            {
                MoveForward();
                BlockReturnToPool();
            }
        }

        private void MoveForward()
        {
            transform.Translate(Vector3.forward * BlockProperties.Instance.CurrentBlockSpeed * Time.deltaTime);
        }

        private void BlockReturnToPool()
        {
            if (gameObject.transform.position.z >= 5)
            {
                ObjectPool.OnReturningToPool(gameObject, _currentBlockType);
            }
        }
    }
}