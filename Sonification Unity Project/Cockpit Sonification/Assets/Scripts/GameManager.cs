using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{

    public GameObject startScreen, flightScreen, resultScreen;

    [Space, Header("speed"), SerializeField] private int _speed;
    public int speedMin, speedMax;
    public int Speed
    {
        get { return _speed; }
        set
        {
            float oldValue = _speed;
            _speed = Mathf.Clamp(value, speedMin, speedMax);
            if (_speed != oldValue)
            {
                ChangeSpeedUi();
                //ChangeRealState();
            }

        }
    }

    [Space, Header(("altitude")), SerializeField] private int _altitude;
    public int altitudeMin, altitudeMax;
    public int Altitude
    {
        get { return _altitude; }
        set
        {
            float oldValue = _altitude;
            _altitude = Mathf.Clamp(value, altitudeMin, altitudeMax);
            if (_altitude != oldValue)
            {
                ChangeAltitudeUi();
                //ChangeRealState();
            }
        }
    }

    [Space, Header("direction"), SerializeField] private int _direction;
    public int directionMin, directionMax;
    public int Direction
    {
        get { return _direction; }
        set
        {
            float oldValue = _direction;
            _direction = Mathf.Clamp(value, directionMin, directionMax);
            if (value == 360) _direction = 0;
            if (value == -1) _direction = 359;
            if (_direction != oldValue)
            {
                ChangeDirectionUi();
               // ChangeRealState();
            }

        }
    }

    [Space, Header("rotation"), SerializeField] private int _rotation;
    public int rotationMin, rotationMax;
    public int Rotation
    {
        get { return _rotation; }
        set
        {
            float oldValue = _rotation;

            _rotation = Mathf.Clamp(value, rotationMin, rotationMax);

            if (_rotation != oldValue)
            {
                ChangeRotationUi();
                //ChangeRealState();
            }
        }
    }


    [Space]
    public Transform speedIndicator;
    public Slider altitudeSlider;
    public Transform directionIndicator;
    public Transform rotationIndicator;

    public static GameManager instance;

    private void Start()
    {
        instance = this;

        startScreen.SetActive(true);
        flightScreen.SetActive(false);
        resultScreen.SetActive(false);
    }

    public void SetupSimulationUI()
    {
        startScreen.SetActive(false);
        flightScreen.SetActive(true);
        resultScreen.SetActive(false);
    }

    public void SetAllValues(int speed, int altitude, int direction, int rotation)
    {
        Speed = speed;
        Altitude = altitude;
        Direction = direction;
        Rotation = rotation;
    }

    private void ChangeSpeedUi()
    {
        float uiRotation = Map(a1: speedMin, a2: speedMax, b1: -35, b2: -315, s: Speed);

        if (speedIndicator != null)
        {
            //Debug.Log("changing speed UI");

            speedIndicator.rotation = Quaternion.Euler(0, 0, uiRotation);
        }
    }

    private void ChangeAltitudeUi()
    {
        //Debug.Log("changing altitude UI");
        altitudeSlider.value = Map(altitudeMin, altitudeMax, 0, 1, Altitude);
    }

    private void ChangeDirectionUi()
    {
        if (directionIndicator != null)
        {
            //Debug.Log("changing direction UI");

            directionIndicator.rotation = Quaternion.Euler(0, 0, -Direction);
        }
    }

    void ChangeRotationUi()
    {
        float uiRotation;

        if (_rotation <= 0)
        {
            uiRotation = Map(rotationMin, 0, -20, 0, Rotation);
        }
        else
        {
            uiRotation = Map(0, rotationMax, 0, 20, Rotation);
        }

        if (rotationIndicator == null) return;
        //Debug.Log("changing rotation UI, uiRotation: " + uiRotation);

        rotationIndicator.rotation = Quaternion.Euler(0, 0, uiRotation);
    }

    public static float Map(float a1, float a2, float b1, float b2, float s)
    {
        return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void ChangeRealState()
    {
        RealStateManager.instance.ChangeAllPlaneVariables(Speed, Altitude, Direction, Rotation);
    }

    public void SimuationFinished()
    {
        flightScreen.SetActive(false);
        resultScreen.SetActive(true);
    }
}

