using System.Collections.Generic;
using UnityEngine;
namespace RedApple.SubwaySurfer
{
    public class DisableOnCollision : MonoBehaviour
    {
        [SerializeField] private List<string> OtherColliderTags = new List<string>();
        private void OnCollisionEnter(Collision collision)
        {
            foreach (string tag in OtherColliderTags)
            {
                if (collision.gameObject.tag == tag)
                {
                    this.gameObject.SetActive(false);
                }
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            foreach (string tag in OtherColliderTags)
            {
                if (other.gameObject.tag == tag)
                {
                    this.gameObject.SetActive(false);
                }
            }
        }
        private void OnEnable()
        {
            
        }
    }
}