using UnityEngine;

namespace RedApple.SubwaySurfer
{
    public class PowerUpObs : MonoBehaviour
    {
        private bool CanChoseNumber = true;


        private void Update()
        {
            PowerUpSpwanProbablity();
        }

        void PowerUpSpwanProbablity()
        {
            if (this.gameObject.activeInHierarchy && CanChoseNumber)
            {
                CanChoseNumber = false;
                int Num = Random.Range(0, 10);
                Disable(Num);
            }
        }

        void Disable(int num)
        {
            if(num < 5)
            {
                Debug.Log("Fly Power Spwaned");
                CanChoseNumber = true;
                this.gameObject.SetActive(false);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if(collision.gameObject.tag == "Player")
            {
                CanChoseNumber = true;
            }
        }
    }
}