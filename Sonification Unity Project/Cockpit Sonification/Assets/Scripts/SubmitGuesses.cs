using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmitGuesses : MonoBehaviour
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

    public SubmitGuesses(Indicators variableToChange)
    {
        _variableToChange = variableToChange;
    }

    public void ChangeGuessAmount(int changeAmount)
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

            yield return new WaitForSeconds(_changeInterval);
        }

    }
}
