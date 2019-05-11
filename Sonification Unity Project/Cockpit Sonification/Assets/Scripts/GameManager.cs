using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class GameManager : MonoBehaviour
    {

        public GameObject startScreen, flightScreen, resultScreen;

        [Space, Header("speed"), SerializeField] private float _speed;
        public float speedMin, speedMax;
        public float Speed
        {
            get { return _speed; }
            set
            {
                float oldValue = _speed;
                _speed = Mathf.Clamp(value, speedMin, speedMax);
                if (_speed != oldValue)
                    ChangeSpeedUi();

            }
        }

        [Space, Header(("altitude")), SerializeField] private float _altitude;
        public float altitudeMin, altitudeMax;
        public float Altitude
        {
            get { return _altitude; }
            set
            {
                float oldValue = _altitude;
                _altitude = Mathf.Clamp(value, altitudeMin, altitudeMax);
                if (_altitude != oldValue)
                    ChangeAltitudeUi();
            }
        }

        [Space, Header("direction"), SerializeField] private float _direction;
        public float directionMin, directionMax;
        public float Direction
        {
            get { return _direction; }
            set
            {
                float oldValue = _direction;
                _direction = Mathf.Clamp(value, directionMin, directionMax);
                if (value == 360) _direction = 0;
                if (value == -1) _direction = 359;
                if (_direction != oldValue)
                    ChangeDirectionUi();

            }
        }

        [Space, Header("rotation"), SerializeField] private float _rotation;
        public float rotationMin, rotationMax;
        public float Rotation
        {
            get { return _rotation; }
            set
            {
                float oldValue = _rotation;

                _rotation = Mathf.Clamp(value, rotationMin, rotationMax);

                if (_rotation != oldValue)
                    ChangeRotationUi();

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

        public void StartSimulation()
        {
            startScreen.SetActive(false);
            flightScreen.SetActive(true);
            resultScreen.SetActive(false);
        }

        public void SetAllValues(float speed, float altitude, float direction, float rotation)
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
                Debug.Log("changing speed UI");

                speedIndicator.rotation = Quaternion.Euler(0, 0, uiRotation);
            }

        }

        private void ChangeAltitudeUi()
        {
            Debug.Log("changing altitude UI");
            altitudeSlider.value = Map(altitudeMin, altitudeMax, 0, 1, Altitude);
        }

        private void ChangeDirectionUi()
        {
            if (directionIndicator != null)
            {
                Debug.Log("changing direction UI");

                directionIndicator.rotation = Quaternion.Euler(0, 0, -Direction);
            }
        }

        void ChangeRotationUi()
        {
            float uiRotation;

            if (_rotation <= 0)
            {
                uiRotation = Map(rotationMin, rotationMax, 0, -20, Rotation);
            }
            else
            {
                uiRotation = Map(rotationMin, rotationMax, 0, 20, Rotation);
            }

            if (rotationIndicator == null) return;
            Debug.Log("changing rotation UI");

            rotationIndicator.rotation = Quaternion.Euler(0, 0, uiRotation);

        }

        private static float Map(float a1, float a2, float b1, float b2, float s)
        {
            return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
        }
    }
}
