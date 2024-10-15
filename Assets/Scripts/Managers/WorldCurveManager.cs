//@Author Sabyasachi Thakur
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldCurveManager : MonoBehaviour
{
    [SerializeField] private List<Material> mats = new List<Material>();
    [SerializeField] private SideCurves Curves;
    [SerializeField] private float CurveSpeed;
    [SerializeField] private float BendAmount;
    private bool canCurve = true;
    private void Awake()
    {
        foreach (var item in mats)
        {
            item.SetVector("_QOffset", new Vector4(0, -10, 0));
        }
        Curves = SideCurves.Middle;
    }
    private void Update()
    {
        if (canCurve)
        {
            canCurve= false;
            StartCoroutine(CurveDelay());
        }
        Bend();
    }
    IEnumerator CurveDelay()
    {
        yield return new WaitForSeconds(RandomTime());
        Curves = (SideCurves)RandomCurve();
        canCurve = true;
    }
    private void Bend()
    {
        switch (Curves)
        {
            case SideCurves.Left:
                BendLeft(); 
                break;
            case SideCurves.Right:
                BendRight();
                break;
            case SideCurves.Middle:
                BendMid();
                break;
        }
    }
    private void BendLeft()
    {
        foreach (var item in mats)
        {
            item.SetVector("_QOffset", Vector3.MoveTowards(item.GetVector("_QOffset"), new Vector4(-BendAmount, -10, 0), CurveSpeed));
        }
    }
    private void BendRight()
    {
        foreach (var item in mats)
        {
            item.SetVector("_QOffset", Vector3.MoveTowards(item.GetVector("_QOffset"), new Vector4(BendAmount, -10, 0), CurveSpeed));
        }
    }
    private void BendMid()
    {
        foreach (var item in mats)
        {
            item.SetVector("_QOffset", Vector3.MoveTowards(item.GetVector("_QOffset"), new Vector4(0, -10, 0), CurveSpeed));
        }
    }
    private float RandomTime()
    {
        return Random.Range(10, 15);
    }
    private int RandomCurve()
    {
        return Random.Range(0, 3);
    }
}
public enum SideCurves
{
    Left,
    Right,
    Middle,
}
