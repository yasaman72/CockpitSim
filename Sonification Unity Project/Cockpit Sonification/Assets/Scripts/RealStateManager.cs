using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RealStateManager : MonoBehaviour
{
    public Text timerText;
    public float simulationTime;
    public float timerIncreaseAmount;

    public TMP_Text speedResultTxt, altitudeResultText, directionResultText, rotationResultText;
    private float speedAccuracyPerc, rotationAccuracyPerc, directionAccuracyPerc, altitudeAccuracyPerc;
    [Space] public int speedGuessTolerance;
    [Space] public int altitudeGuessTolerance;
    [Space] public int directionGuessTolerance;
    [Space] public int rotationGuessTolerance;

    public RealStateEditor realStateEditor;

    [Space, Header("speed"), SerializeField] private int _realSpeed;
    public int speedMin, speedMax;
    public int RealSpeed
    {
        get { return _realSpeed; }
        set
        {
            float oldValue = _realSpeed;
            _realSpeed = Mathf.Clamp(value, speedMin, speedMax);
            if (_realSpeed != oldValue)
                ChangeRealSpeed();
        }
    }

    public AudioSource speedAudioSource;
    public float minSpeedPitch, maxSpeedPitch;

    [Space, Header(("altitude")), SerializeField] private int _realAltitude;
    public int altitudeMin, altitudeMax;
    public int RealAltitude
    {
        get { return _realAltitude; }
        set
        {
            float oldValue = _realAltitude;
            _realAltitude = Mathf.Clamp(value, altitudeMin, altitudeMax);
            if (_realAltitude != oldValue)
                ChangeRealAltitude();
        }
    }

    public AudioSource maxAltitudeAudioSource, minAltitudeAudioSource;

    [Space, Header("direction"), SerializeField] private int _realDirection;
    public int directionMin, directionMax;
    public int RealDirection
    {
        get { return _realDirection; }
        set
        {
            float oldValue = _realDirection;
            _realDirection = Mathf.Clamp(value, directionMin, directionMax);
            if (value == 360) _realDirection = 0;
            if (value == -1) _realDirection = 359;
            if (_realDirection != oldValue)
                ChangeRealDirection();
        }
    }

    public Transform directionTransform;
    public AudioSource directionAudioSource;

    [Space, Header("rotation"), SerializeField] private int _realRotation;
    public int rotationMin, rotationMax;
    private bool rotationToRight;

    public int RealRotation
    {
        get { return _realRotation; }
        set
        {
            float oldValue = _realRotation;

            rotationToRight = value < oldValue;

            _realRotation = Mathf.Clamp(value, rotationMin, rotationMax);

            if (oldValue != _realRotation)
            {
                ChangeRealRotation();
            }
            else
            {
                rotationLeftAudioSource.Pause();
                rotationRightAudioSource.Pause();
            }
        }
    }

    public AudioSource rotationRightAudioSource, rotationLeftAudioSource;

    public static RealStateManager instance;

    private void Start()
    {
        instance = this;
    }

    public void StartSimulation()
    {
        gameObject.transform.localPosition = Vector3.zero;

        GameManager.instance.SetupSimulationUI();

        RealSpeed = Random.Range(speedMin, speedMax);
        RealAltitude = Random.Range(altitudeMin, altitudeMax);
        RealDirection = Random.Range(directionMin, directionMax);
        RealRotation = Random.Range(rotationMin, rotationMax);

        GameManager.instance.SetAllValues(RealSpeed, RealAltitude, RealDirection, RealRotation);

        speedAudioSource.Play();
        directionAudioSource.Play();
        maxAltitudeAudioSource.Play();
        minAltitudeAudioSource.Play();

        rotationRightAudioSource.Stop();
        rotationLeftAudioSource.Stop();


        //ChangeAllPlaneVariables();
        StartCoroutine(SimulationPeriod());
    }

    public void ChangeAllPlaneVariables()
    {
        ChangeRealSpeed();
        ChangeRealAltitude();
        ChangeRealDirection();
        ChangeRealRotation();
    }

    public void ChangeAllPlaneVariables(int speed, int altitude, int direction, int rotation)
    {
        RealSpeed = speed;
        RealAltitude = altitude;
        RealDirection = direction;
        RealRotation = rotation;
    }

    public void ChangeRealSpeed()
    {
        speedAudioSource.pitch = GameManager.Map(speedMin, speedMax, minSpeedPitch, maxSpeedPitch, RealSpeed);
    }

    public void ChangeRealAltitude()
    {
        gameObject.transform.localPosition = Vector3.up * RealAltitude;
    }

    public void ChangeRealDirection()
    {
        directionTransform.rotation = Quaternion.Euler(0, RealDirection, 0);
    }

    public void ChangeRealRotation()
    {
        if (!rotationRightAudioSource.isPlaying)
            rotationRightAudioSource.Play();

        if (!rotationLeftAudioSource.isPlaying)
            rotationLeftAudioSource.Play();

        if (rotationToRight)
        {
            rotationLeftAudioSource.volume = 0;
            rotationRightAudioSource.volume = GameManager.Map(rotationMin, rotationMax, 1f, 0f, RealRotation);

            //Debug.Log("Right rotation volume amount: "+ GameManager.Map(rotationMin, 0f, 0f, 1f, RealRotation));
        }
        else
        {
            rotationRightAudioSource.volume = 0;
            rotationLeftAudioSource.volume = GameManager.Map(rotationMin, rotationMax, 0f, 1f, RealRotation);

            //Debug.Log("Left rotation volume amount: "+ GameManager.Map(0f, rotationMax, 0f, 1f, RealRotation));
        }
    }


    private float timer = 0;
    IEnumerator SimulationPeriod()
    {
        while (timer < simulationTime)
        {
            if (timer % 1 == 0)
            {
                CompareGuesses();
            }

            if (timer % stateChangingInterval == 0)
            {
                ChangePlaneRealState();
            }

            timer += timerIncreaseAmount;
            timerText.text = ((int)(simulationTime - timer)).ToString();
            yield return new WaitForSeconds(timerIncreaseAmount);
        }
        GameManager.instance.SimuationFinished();

        speedAudioSource.Stop();
        directionAudioSource.Stop();
        maxAltitudeAudioSource.Stop();
        minAltitudeAudioSource.Stop();
        rotationRightAudioSource.Stop();
        rotationLeftAudioSource.Stop();

        speedResultTxt.text = Mathf.RoundToInt(speedAccuracyPerc) + "%";
        altitudeResultText.text = Mathf.RoundToInt(altitudeAccuracyPerc) + "%";
        directionResultText.text = Mathf.RoundToInt(directionAccuracyPerc) + "%";
        rotationResultText.text = Mathf.RoundToInt(rotationAccuracyPerc) + "%";

        Debug.Log("Simulaion Finished!");
    }

    private void CompareGuesses()
    {
        // SPEED
        if (GameManager.instance.Speed > (RealSpeed - speedGuessTolerance) &&
            GameManager.instance.Speed < (RealSpeed + speedGuessTolerance))
        {
            speedAccuracyPerc += 100 / simulationTime;
        }
        else
        {
            speedAccuracyPerc -= 100 / simulationTime;
            if (speedAccuracyPerc < 0)
                speedAccuracyPerc = 0;
        }


        // ALTITUDE
        if (GameManager.instance.Altitude > (RealAltitude - altitudeGuessTolerance) &&
            GameManager.instance.Altitude < (RealAltitude + altitudeGuessTolerance))
        {
            altitudeAccuracyPerc += 100 / simulationTime;
        }
        else
        {
            altitudeAccuracyPerc -= 100 / simulationTime;
            if (altitudeAccuracyPerc < 0)
                altitudeAccuracyPerc = 0;
        }

        // DIRECTION
        if (GameManager.instance.Direction > (RealDirection - directionGuessTolerance) &&
            GameManager.instance.Direction < (RealDirection + directionGuessTolerance))
        {
            directionAccuracyPerc += 100 / simulationTime;
        }
        else
        {
            directionAccuracyPerc -= 100 / simulationTime;
            if (directionAccuracyPerc < 0)
                directionAccuracyPerc = 0;
        }

        // ROTATION
        if (GameManager.instance.Rotation > (RealRotation - rotationGuessTolerance) &&
            GameManager.instance.Rotation < (RealRotation + rotationGuessTolerance))
        {
            rotationAccuracyPerc += 100 / simulationTime;
        }
        else
        {
            rotationAccuracyPerc -= 100 / simulationTime;
            if (rotationAccuracyPerc < 0)
                rotationAccuracyPerc = 0;
        }

        //Debug.Log("speedAccuracyPerc: " + speedAccuracyPerc +
        //          " | rotationAccuracyPerc: " + rotationAccuracyPerc +
        //          " | directionAccuracyPerc: " + directionAccuracyPerc +
        //          " | altitudeAccuracyPerc: " + altitudeAccuracyPerc);
    }

    [Space] public int stateChangingInterval;
    [Space] public int stateNotChangeChancePrc;
    public int speedChangeChancePrc;
    public int rotationChangeChancePrc;
    public int altitudeChangeChancePrc;
    public int directionChangeChancePrc;

    private void ChangePlaneRealState()
    {
        // NOTHING CHANGED
        if (Random.Range(0, 101) <= stateNotChangeChancePrc)
        {
            Debug.Log("state didn't change");
            return;
        }

        // SPEED
        if (Random.Range(0, 101) <= speedChangeChancePrc)
        {
            Debug.Log("speed changed");

            if (Random.Range(0, 2) == 0)
            {
                realStateEditor.speedupEvent.Invoke();
            }
            else
            {
                realStateEditor.speedDownEvent.Invoke();
            }
            //return;
        }

        // ROTATION
        if (Random.Range(0, 101) <= rotationChangeChancePrc)
        {
            Debug.Log("rotation changed");
            rotationLeftAudioSource.Pause();
            rotationRightAudioSource.Pause();

            if (Random.Range(0, 2) == 0)
            {

                realStateEditor.rotationLeftEvent.Invoke();
            }
            else
            {
                realStateEditor.rotationRightEvent.Invoke();
            }
            //return;
        }

        // DIRECTION
        if (Random.Range(0, 101) <= directionChangeChancePrc)
        {
            Debug.Log("direction changed");

            if (Random.Range(0, 2) == 0)
            {
                realStateEditor.directionUpEvent.Invoke();
            }
            else
            {
                realStateEditor.directionDownEvent.Invoke();
            }
            //return;
        }

        // ALTITUDE
        if (Random.Range(0, 101) <= altitudeChangeChancePrc)
        {
            Debug.Log("direction changed");

            if (Random.Range(0, 2) == 0)
            {
                realStateEditor.altitudeDownEvent.Invoke();
            }
            else
            {
                realStateEditor.altitudeUpEvent.Invoke();
            }
            //return;
        }
    }
}
