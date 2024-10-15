using AmazingAssets.CurvedWorld;
using System.Collections;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] CurvedWorldController WorldCurve;
    private float Amount = 1;

    [SerializeField] float maxCurve = 1.2f;
    [SerializeField] float minCurve = -1.2f;

    float newOffset = 0;

    float smoothTime = 3f;

    public float rate = 0.2f;

    private void Start()
    {
        Debug.Log("Call");
        //StartCoroutine(BendSetting());
    }

    private void Update()
    {
       
        
    }
    private IEnumerator BendSetting()
    {
        while (true)
        {
            smoothTime -= rate * Time.deltaTime;

            Debug.Log(smoothTime);
            int pickTime = Random.Range(2, 11);
            yield return new WaitForSeconds(pickTime);
            float newOffset = Random.Range(minCurve, maxCurve);
            WorldCurve.SetBendHorizontalSize(newOffset);

            
            
        }
    }
}
