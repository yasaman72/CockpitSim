﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityScript.Steps;

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
    [SerializeField] private float _changeInterval;

    private int lastChangeAmount;

    public SubmitChange(Indicators variableToChange)
    {
        _variableToChange = variableToChange;
    }

    public void ChangeIndicatorAmount(int changeAmount)
    {
        StopAllCoroutines();

        if (lastChangeAmount != changeAmount)
        {
            StartCoroutine(KeepChangingValue(changeAmount));
            lastChangeAmount = changeAmount;
        }
        else
        {
            lastChangeAmount = changeAmount + 1000;
        }
    }

    IEnumerator KeepChangingValue(int changeAmount)
    {
        while (true)
        {
            switch (_variableToChange)
            {
                case Indicators.Speed:
                    RealStateManager.instance.RealSpeed += changeAmount;
                    break;

                case Indicators.Altitude:
                    RealStateManager.instance.RealAltitude += changeAmount;
                    break;

                case Indicators.Direction:
                    RealStateManager.instance.RealDirection += changeAmount;
                    break;

                case Indicators.Rotation:
                    RealStateManager.instance.RealRotation += changeAmount;
                    break;

                default:
                    break;
            }

            yield return new WaitForSeconds(_changeInterval);
        }
    }

}
