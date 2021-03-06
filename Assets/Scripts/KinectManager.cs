﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Windows.Kinect;
using System.Linq;


public class KinectManager : MonoBehaviour {

    private KinectSensor _sensor;
    private BodyFrameReader _bodyFrameReader;
    private Body[] _bodies = null;

    public GameObject kinectAvailableText;
    public Text handXText;

    public bool isAvailable;
    public float paddlePosition;
    public bool isFire;

    public static KinectManager instance = null;

    public Body[] GetBodies()
    {
        return _bodies;
    }

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

    }

	// Use this for initialization
	void Start () {

        _sensor = KinectSensor.GetDefault();
        if(_sensor != null)
        {
            isAvailable = _sensor.IsAvailable;
            kinectAvailableText.SetActive(isAvailable);
            _bodyFrameReader = _sensor.BodyFrameSource.OpenReader();
            if (!_sensor.IsOpen)
            {
                _sensor.Open();
            }
            _bodies = new Body[_sensor.BodyFrameSource.BodyCount];
        }
	}
	
	// Update is called once per frame
	void Update () {

        isAvailable = _sensor.IsAvailable;
        if (_bodyFrameReader != null)
        {
            var frame = _bodyFrameReader.AcquireLatestFrame();

            if(frame != null)
            {
                frame.GetAndRefreshBodyData(_bodies);
                foreach (var body in _bodies.Where(b => b.IsTracked))
                {
                    isAvailable = true;
                    if (body.HandRightConfidence == TrackingConfidence.High && body.HandRightState == HandState.Lasso)
                    {
                        isFire = true;
                    }
                    else
                    {
                        paddlePosition = RescalingToRangesB(-1, 1, -8, 8, body.Lean.X);
                        handXText.text = paddlePosition.ToString();
                    }
                }
                frame.Dispose();
                frame = null;
            }
        }
	
	}

    static float RescalingToRangesB(float scaleAStart, float scaleAEnd, float scaleBStart, float scaleBEnd, float valueA)
    {
        return (((valueA - scaleAStart) * (scaleBEnd - scaleBStart)) / (scaleAEnd - scaleAStart)) + scaleBStart;
    }

    void OnApplicationQuit()
    {
        if(_bodyFrameReader!= null)
        {
            _bodyFrameReader.IsPaused = true;
            _bodyFrameReader.Dispose();
            _bodyFrameReader = null;

        }
        if(_sensor != null)
        {
            if (_sensor.IsOpen)
            {
                _sensor.Close();
            }
            _sensor = null;
        }
    }
}
