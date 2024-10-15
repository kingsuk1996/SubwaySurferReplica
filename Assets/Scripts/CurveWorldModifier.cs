using System;
using System.Collections;
using UnityEngine;
using AmazingAssets.CurvedWorld;
using RedApple.SubwaySurfer;
using DG.Tweening;


public class CurveWorldModifier : MonoBehaviour
{
    [SerializeField] private CurvedWorldController curvedWorldController;

    private float maxValue = 3.0f;
    private float minValue = -3.0f;

    public float prevhorizontal;
    public float prevvertical;

    public float horizontal;
    public float vertical;

    public bool canxCometoZero;
    public bool canyCometoZero;
    public float increaseSpeed;

    [SerializeField] private PlayerController playerController;
    private void StartBend()
    {
        SetHorizontalCurveValue();
        SetVerticalCurveValue();
    }
  

    private void SetHorizontalCurveValue()
    {
        
            canxCometoZero = !canxCometoZero;

        if (canxCometoZero)
        {
            horizontal = UnityEngine.Random.Range(minValue, maxValue);

        }
        else
        {
            horizontal =0;
        }
        StartCoroutine(SetHorizontalBendSize());
    }

    private IEnumerator SetHorizontalBendSize()
    {

        float x = prevhorizontal;
        float y = increaseSpeed;
        if(prevhorizontal > horizontal)
        {
            y = -increaseSpeed;
        }
        while(x.ToString("F2") != horizontal.ToString("F2"))
        {
            x += y;
            curvedWorldController.SetBendHorizontalSize(x);
            yield return null;
        }

        prevhorizontal = horizontal;
        SetHorizontalCurveValue();
    }


    private void SetVerticalCurveValue()
    {
        canyCometoZero = !canyCometoZero;

        if (canyCometoZero)
        {
            vertical = UnityEngine.Random.Range(minValue, maxValue);
        }
        else
        {
            vertical = 0;
        }
          
        StartCoroutine(SetVerticaBendSize());
    }

    private IEnumerator SetVerticaBendSize()
    {
        float x = prevvertical;
        float y = increaseSpeed;
        if (prevvertical > vertical)
        {
            y = -increaseSpeed;
        }
        while (x.ToString("F2") != vertical.ToString("F2"))
        {
            x += y;
            curvedWorldController.SetBendVerticalSize(x);
            yield return null;
        }

        prevvertical = vertical;
        SetVerticalCurveValue();

    }
    private void OnEnable()
    {
        playerController.OnStart += StartBend;
    }
    private void OnDisable()
    {
        playerController.OnStart += StartBend;
    }
}
