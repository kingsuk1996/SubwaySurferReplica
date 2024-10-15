using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinPooling : MonoBehaviour
{
    [SerializeField] private GameObject CoinPrefab;
    [SerializeField] private GameSettings gameSettings;
    [SerializeField] private RectTransform coin2D;

    [Space(10)]
    [SerializeField] private List<GameObject> CoinInPool = new List<GameObject>();


    void Awake()
    {
        CreateCoinPool();
    }

    private void CreateCoinPool()
    {
        GameObject gameObject;
        for (int j = 0; j < gameSettings.MaxCoin; j++)
        {
            gameObject = Instantiate(CoinPrefab, coin2D);
            gameObject.SetActive(false);
            CoinInPool.Add(gameObject);
        }
    }

    public GameObject ReturnInactiveCoin()
    {
        GameObject tempGObj = null;
        for (int i = 0; i < CoinInPool.Count; i++)
        {
            if (!CoinInPool[i].activeInHierarchy)
            {
                tempGObj = CoinInPool[i];
                tempGObj.SetActive(true);
                return tempGObj;
            }
        }
        return tempGObj;
    }
}
