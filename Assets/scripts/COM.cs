using UnityEngine;
using System.Collections;
using System.IO.Ports;
using System;
using System.Threading;
using UnityEngine.UI;
using UnityEngine.Analytics;
using System.Collections.Generic;

public class COM : MonoBehaviour {
	
	public SerialPort sp;
	public int bytes;
	public Thread serialThread;
	// Params - movement and rotation
	public float x1,y1,z1;
	public string orientationValues;
	public bool readyToMove = false;
	public float prevY = 1.0f;
	public float prevX = 1.0f;
	public float speed;
	public float minAngle = 0f;
	public float maxAngle = 0f;
    bool angleChanged = false;

	void Start () {
		//initialize the serial port
		sp = new SerialPort("COM3", 9600);
	}

	// parse the incoming values
	void parseValues(string av) {
		orientationValues = "";
		string[] split = av.Split (',');
		if ((string.Compare (split [0], ">3")) == 0) {
			//orientationValues += split [2] + " " + split [3] + " " + split [4];
			if((float.Parse(split[3])) > maxAngle) {
				maxAngle = float.Parse(split[3]);
                angleChanged = true;
            }
			if((float.Parse(split[3])) < minAngle) {
				minAngle = float.Parse(split[3]);
                angleChanged = true;
			}
            x1 = prevX;
            y1 = prevY;
        } else {
			x1 = float.Parse (split [2]);
			y1 = float.Parse (split [3]);
		}
        readyToMove = true;
    }

	// move the object
	void moveObj(float x, float y) {
		speed = 15.0f;
		Vector3 move = Vector3.zero ;
		move.x = x;
		move.y = y;
		move.Normalize();
		// low pass filter
		prevY = (0.8f * prevY) + ((1.0f - 0.8f) * move.y);
		prevX = (0.8f * prevX) + ((1.0f - 0.8f) * move.x);

		transform.Translate((-prevX * speed) * Time.deltaTime,(prevY * speed)* Time.deltaTime, -0.1f, Space.World);
		readyToMove = false;

		//check position range
		if (gameObject.transform.position.y <=-4.0f || gameObject.transform.position.y >=4.0f)
		{
			// Create values between this range (minX to maxX) and store in xPos
			float yPos = Mathf.Clamp(gameObject.transform.position.y, -4.0f + 0.1f, 4.0f + 0.1f);

			// Assigns these values to the Transform.position component of the Player
			gameObject.transform.position = new Vector3(gameObject.transform.position.x,yPos,-0.1f);
		}

		if (gameObject.transform.position.x <=-2.0f || gameObject.transform.position.x >=2.0f)
		{
			// Create values between this range (minX to maxX) and store in xPos
			float xPos = Mathf.Clamp(gameObject.transform.position.x, -2.0f + 0.1f, 2.0f + 0.1f);
			
			// Assigns these values to the Transform.position component of the Player
			gameObject.transform.position = new Vector3(xPos,gameObject.transform.position.y, -0.1f);
		}
	}

	// Get data from the serial port
	void recData() {
		if ((sp != null) && (sp.IsOpen)) {
			byte tmp;
			string data = "";
			string avalues="";
			tmp = (byte) sp.ReadByte();
			while(tmp !=255) {
				data+=((char)tmp);
				tmp = (byte) sp.ReadByte();
				if((tmp=='>') && (data.Length > 30)){
					avalues = data;
					parseValues(avalues);
					data="";
				}
			}
		}
	}

	// connect to serial port
	void connect() {
		Debug.Log ("Connection started");	
		try {
			if(sp.IsOpen)
			sp.Close();
			sp.Open();
			sp.ReadTimeout = 400;
			serialThread = new Thread(recData);
			serialThread.Start ();
			Debug.Log("Port Opened!");
			}catch (SystemException e)
			{
				Debug.Log ("Error opening = "+e.Message);
			}

		}


		void Update () { 

			if (Input.GetKeyDown ("x")) {
				Debug.Log("Connection establishing...");
				connect ();
			}

        if (angleChanged)
        {
            Analytics.CustomEvent("Angles", new Dictionary<string, object>
                {
                    { "maxAngle", maxAngle },
                    { "minAngle", minAngle }
                });
            Debug.Log("Angle changed");
            angleChanged = false;
        }

        Vector3 resetZ = transform.position;
        resetZ.z = 0f;
        transform.position = resetZ;
    }

		void FixedUpdate() {
		if (readyToMove == true) {
			moveObj(x1, y1);
		}
	}
	}