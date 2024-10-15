using System.Collections;
using UnityEngine;
namespace RedApple.SubwaySurfer
{
    public class CoinRotation : MonoBehaviour
    {
        private void Start()
        {
            StartCoroutine(CoinRotate());
        }


        IEnumerator CoinRotate()
        {
            while (true)
            {
                if (UImanager.Instance.CanMove)
                {
                    transform.Rotate(Vector3.forward * Time.deltaTime * 60);
                }
                yield return null;
            }
        }

        
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                transform.position = Vector3.MoveTowards(transform.position, PlayerManager.Instance.coinNextPos.position, Time.deltaTime * 75f);
                transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.zero, Time.deltaTime * 4f);
            }
        }
    }
}
