using UnityEngine;
namespace RedApple.SubwaySurfer
{
    public class SpwanablesHightHandler : MonoBehaviour
    {
        [SerializeField] private float UpHight;
        [SerializeField] private float DownHight;
        private void OnEnable()
        {
            PlayerManager.PlayerFly += TransperToUpHight;
            PlayerManager.PlayerBackToSurface += TransperToDownHight;
        }
        private void OnDisable()
        {
            PlayerManager.PlayerFly -= TransperToUpHight;
            PlayerManager.PlayerBackToSurface -= TransperToDownHight;
        }
        private void TransperToUpHight()
        {
            transform.position = new Vector3(transform.position.x, UpHight, transform.position.z);
        }
        private void TransperToDownHight()
        {
            transform.position = new Vector3(transform.position.x, DownHight, transform.position.z);
        }
    }
}