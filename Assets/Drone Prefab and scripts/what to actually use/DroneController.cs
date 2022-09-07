using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityStandardAssets_1_1_2.CrossPlatformInput;
using UnityStandardAssets_1_1_2.Characters.ThirdPerson;


namespace SIGVerse.Drone
{
	public class DroneController : MonoBehaviour
	{

		public Rigidbody Drone;
		DronePropellerSpin propellerSpin;


		public int DirectionalSpeed = 25;
		public int Tilt = 25;
		public int rotationSpeed = 5;
		public int LiftSpeed = 14;
		private bool moving;
		public bool landing;


		private Vector3 DroneRotation;

		//rather than using update which updates every frame, fixed update runs at a fixed interval
		//this means taht it can update multiple times per frame
		private void FixedUpdate()
		{
			DroneRotation = Drone.transform.localEulerAngles;//provides angle relative to parent transforms rotation

			TiltCorrection();

			DroneControls();


		}
        #region Controls
        void DroneControls()
		{
			landing = false;
			if (Input.GetKey(KeyCode.K))
			{
				ScreenshotHandler.takeScreenShot_Static(500, 500);
			}
			#region drone controls
			if (!landing)
			{
				Drone.AddForce(0, 8f, 0);//keeps the drone from losing height quickly, since the drone is set to a rigidbody itll fall at 9.8 until it hits a collider
										 //9.8f(f makes it a float rather than a double
				if (moving == false)
				{
					UpArrowDown();
					DownArrowDown();
					LeftArrowDown();
					RightArrowDown();
					DKeyDown();
					AKeyDown();
					SKeyDown();
					WKeyDown();
					SpaceKeyDown();
				}
			}
			else
			{
				if (propellerSpin.propSpeed < 2255f)
				{
					if (propellerSpin.propSpeed == 0)
					{
						Drone.AddForce(0, (-9.8f) * Time.deltaTime, 0);//keeps the drone from losing height quickly, since the drone is set to a rigidbody itll fall at 9.8 until it hits a collider
																	   //9.8f(f makes it a float rather than a double
					}
					else
					{
						for (int i = 8; i > 0; i--)
						{
							Drone.AddForce(0, i, 0);//keeps the drone from losing height quickly, since the drone is set to a rigidbody itll fall at 9.8 until it hits a collider
													//9.8f(f makes it a float rather than a double
						}
					}
				}
			}
			#endregion
		}
		void UpArrowDown()
		{
			//lift
			if (Input.GetKey(KeyCode.UpArrow))
			{
				Drone.AddRelativeForce(0, LiftSpeed, 0);

			}
		}
		void DownArrowDown()
        {
			//drop
			if (Input.GetKey(KeyCode.DownArrow))
			{
				Drone.AddRelativeForce(0, -LiftSpeed, 0);

			}
		}
		void LeftArrowDown()
        {
			//rotate left
			if (Input.GetKey(KeyCode.LeftArrow))
			{
				transform.Rotate(0f, -rotationSpeed, 0f, Space.Self);

			}
		}
		void RightArrowDown()
        {
			//rotate right
			if (Input.GetKey(KeyCode.RightArrow))
			{
				transform.Rotate(0f, rotationSpeed, 0f, Space.Self);

			}
		}
		void WKeyDown()
        {
			//forward
			if (Input.GetKey(KeyCode.W))
			{
				Drone.AddRelativeForce(0, 0, DirectionalSpeed);
				Drone.AddRelativeTorque(10, 0, 0);
			}
		}
		void SKeyDown()
        {
			//backward
			if (Input.GetKey(KeyCode.S))
			{
				Drone.AddRelativeForce(0, 0, -DirectionalSpeed);
				Drone.AddRelativeTorque(-10, 0, 0);

			}
		}
		void AKeyDown()
        {
			//right
			if (Input.GetKey(KeyCode.D))
			{
				Drone.AddRelativeForce(DirectionalSpeed, 0, 0);
				Drone.AddRelativeTorque(0, 0, -10);

			}
		}
		void DKeyDown()
        {
			//left
			if (Input.GetKey(KeyCode.A))
			{
				Drone.AddRelativeForce(-DirectionalSpeed, 0, 0);
				Drone.AddRelativeTorque(0, 0, 10);

			}
		}
		void SpaceKeyDown()
        {
			//call land funciton
			if (Input.GetKeyDown(KeyCode.Space))
			{
				//call land and set land bool to true so that you just descend from the selected location
				//land();
				Drone.AddRelativeForce(0, (-LiftSpeed) * Time.deltaTime, 0);

			}
		}
        #endregion
        #region landing
        void land()
        {
			landing = true;


			//drop the drone at a slowish speed
			Drone.AddForce(0, (-LiftSpeed)* Time.deltaTime, 0);

			//stop landing
			if (Input.GetKeyUp(KeyCode.Space))
			{
				landing = false;
			}
			
		}
		#endregion
		#region tilt correction
		void TiltCorrection()
		{
			ZTilt();
			XTilt();
		}

		void ZTilt()
        {
			//if tilt too big(stabilizes drone on z-axis)
			if (DroneRotation.z > 10 && DroneRotation.z <= 180)
			{
				Drone.AddRelativeTorque(0, 0, -10);
			}
			if (DroneRotation.z > 180 && DroneRotation.z <= 350)
			{
				Drone.AddRelativeTorque(0, 0, 10);
			}

			//if tilt too small(stabilizes drone on z-axis)
			if (DroneRotation.z > 1 && DroneRotation.z <= 10)
			{
				Drone.AddRelativeTorque(0, 0, -3);
			}
			if (DroneRotation.z > 350 && DroneRotation.z <= 359)
			{
				Drone.AddRelativeTorque(0, 0, 3);
			}
		}

		void XTilt()
        {

			//if tilt too big(stabilizes drone on x-axis)
			if (DroneRotation.x > 10 && DroneRotation.x <= 180)
			{
				Drone.AddRelativeTorque(-10, 0, 0);
			}
			if (DroneRotation.x > 180 && DroneRotation.x <= 350)
			{
				Drone.AddRelativeTorque(10, 0, 0);
			}

			//if tilt too small(stabilizes drone on x-axis)
			if (DroneRotation.x > 1 && DroneRotation.x <= 10)
			{
				Drone.AddRelativeTorque(-3, 0, 0);
			}
			if (DroneRotation.x > 350 && DroneRotation.x <= 359)
			{
				Drone.AddRelativeTorque(3, 0, 0);
			}

		}

		#endregion
	}
}
