using DG.Tweening;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    [SerializeField] private RectTransform target;
    [SerializeField] private Ease easeType;
    [SerializeField] private float duration = 100f;


    public void MoveCoin(RectTransform _coin)
    {
        _coin.DOAnchorPos(target.anchoredPosition, duration).OnComplete(() =>
        {
            _coin.gameObject.SetActive(false);
        });
    }

}
