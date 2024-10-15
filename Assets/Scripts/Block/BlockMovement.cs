using System.Collections;
using UnityEngine;
using System;
namespace RedApple.SubwaySurfer
{
    public class BlockMovement : MonoBehaviour
    {
        [SerializeField] private GameSettings gameSettings;

        public BlockType BlockType;
        public float xLength = 70;
        
        void Update()
        {
            if (UImanager.CanMove)
            {
                transform.Translate(Vector3.back * BlockSpeedController.Instance.BlockSpeed * Time.deltaTime);
            }
            if (transform.position.x <= (-gameSettings.DestroyZone))
            {
                this.transform.GetChild(0).gameObject.SetActive(true);
                gameObject.SetActive(false);
            }
        }
    }

    [System.Serializable]
    public enum BlockType
    {
        City1,
        City2,
        City3,
        Crossing1,
        Crossing2,
        Urban1,
        Urban2,
        Bridge1,
        Bridge2,
        Tunnel
    }
}
