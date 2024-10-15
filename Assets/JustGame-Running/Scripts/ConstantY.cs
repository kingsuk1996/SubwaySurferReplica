using UnityEngine;

public class ConstantY : MonoBehaviour
{
    [SerializeField] private float height;

    void Update()
    {
        this.transform.position = new Vector3(this.transform.position.x, height, this.transform.position.z);
    }
}
