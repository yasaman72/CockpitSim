using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class SubmitChange : MonoBehaviour
{
    public enum Indicators
    {
        Speed,
        Altitude,
        Direction,
        Rotation
    }

    [SerializeField] private Indicators _variableToChange;

    public SubmitChange(Indicators variableToChange)
    {
        this._variableToChange = variableToChange;
    }

    public void ChangeIndicatorAmount(float changeAmount)
    {
        switch (_variableToChange)
        {
            case Indicators.Speed:
                GameManager.instance.Speed += changeAmount;
                break;

            case Indicators.Altitude:
                GameManager.instance.Altitude += changeAmount;
                break;

            case Indicators.Direction:
                GameManager.instance.Direction += changeAmount;
                break;

            case Indicators.Rotation:
                GameManager.instance.Rotation += changeAmount;
                break;

            default:
                break;
        }
        //StartCoroutine(KeepChangingValue(changeAmount));
    }

    //IEnumerator KeepChangingValue(float changeAmount)
    //{
    //    while (true)
    //    {

    //        yield return new WaitForSeconds(0.5f);
    //    }
    //}

}
