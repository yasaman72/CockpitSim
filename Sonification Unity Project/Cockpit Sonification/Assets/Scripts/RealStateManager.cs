using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEditor;
using UnityEngine;

public class RealStateManager : MonoBehaviour
{

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

    //public AudioSource altitudeAudioSource;
    //public Transform minAltitudeTransform, maxAltitudeTransform;


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

    [Space, Header("rotation"), SerializeField] private int _realRotation;
    public int rotationMin, rotationMax;
    public int RealRotation
    {
        get { return _realRotation; }
        set
        {
            float oldValue = _realRotation;

            _realRotation = Mathf.Clamp(value, rotationMin, rotationMax);

            if (_realRotation != oldValue)
                ChangeRealRotation();
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
        GameManager.instance.StartSimulation();

        RealSpeed = Random.Range(speedMin, speedMax);
        RealAltitude = Random.Range(altitudeMin, altitudeMax);
        RealDirection = Random.Range(directionMin, directionMax);
        RealRotation = Random.Range(rotationMin, rotationMax);

        GameManager.instance.SetAllValues(RealSpeed, RealAltitude, RealDirection, RealRotation);

        gameObject.transform.localPosition = Vector3.zero;

        ChangeAllPlaneVariables();
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
        speedAudioSource.pitch = GameManager.Map(speedMin, speedMax, -0.5f, 2, RealSpeed);
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
        if (RealRotation < 0)
        {
            rotationLeftAudioSource.volume = 0;
            rotationRightAudioSource.volume = GameManager.Map(rotationMin, 0f, 1f, 0f, RealRotation);

            Debug.Log("Right rotation volume amount: "+ GameManager.Map(rotationMin, 0f, 0f, 1f, RealRotation));
        }
        else if (RealRotation > 0)
        {
            rotationRightAudioSource.volume = 0;
            rotationLeftAudioSource.volume = GameManager.Map(0f, rotationMax, 0f, 1f, RealRotation);

            Debug.Log("Left rotation volume amount: "+ GameManager.Map(0f, rotationMax, 0f, 1f, RealRotation));
        }

        if (RealRotation == 0)
        {
            rotationLeftAudioSource.volume = 0;
            rotationRightAudioSource.volume = 0;
        }
    }
}
