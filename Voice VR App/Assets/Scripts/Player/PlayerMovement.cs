using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace VVR
{
    public class PlayerMovement : MonoBehaviour
    {
        //First Person (FPV) Movement varibles
        const float DOWN_ANGLE_LIMIT = -90f;
        const float UP_ANGLE_LIMIT = 90f;
        const float RIGHT_TURN = 90f;
        const float LEFT_TURN = -90f;
        const float UP = 45f;
        const float DOWN = -45F;
        const float START_TURN = 0F;
        const float STOP_TURN = 360F;

        [SerializeField]
        private float mouseSensitivity = 100f;
        [SerializeField]
        private Transform playerBody;

        float X_Axis_Rotation = 0f;
        float mouseX = 50f;
        float mouseY = 50f;
        bool isUp = false;
        bool isDown = false;

        //Debug
        public bool debugToScreen;
        public Text debugText;        
        string debugString = "";
        

        /*
        FPV Player movement referenced from https://www.youtube.com/watch?v=_QajrabyTJc&t=509s
        */

        void Start()
        {
            debugToScreen = false;
            Input.location.Start();
            Debug.Log("Does System Support Gyro? " + SystemInfo.supportsGyroscope);
            Input.gyro.enabled = true;
        }
        void Update()
        {
            
            //This segment of code prints debug variables on screen
            
            if(debugText != null && debugToScreen == true)
            {
                debugString = "Does System Support Gyro? " + SystemInfo.supportsGyroscope + "\n"
                + "Is Gyro Value Null? " + (Input.gyro == null) + "\n"
                + "Is Gyro Enabled? " + Input.gyro.enabled + "\n"
                + "Input.gyro.rotationRateUnbiased.y " + Input.gyro.rotationRateUnbiased.y + "\n"
                + "Input.gyro.rotationRateUnbiased.x " + Input.gyro.rotationRateUnbiased.x + "\n"
                + "mouseX " + mouseX + "\n"
                + "mouseY " + mouseY + "\n"
                + "X_Axis_Rotation" + X_Axis_Rotation + "\n"
                + "FPS: " + (int)(  1.0f / Time.deltaTime) + "\n"
                ;
                
                debugText.text = debugString;
            }
            
#if (UNITY_STANDALONE || UNITY_EDITOR) && (!UNITY_IOS || !UNITY_ANDROID)

            #region Mouse Movement
            mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            X_Axis_Rotation -= mouseY;

            /* 
            This set of if statements will give actuate the up down voice commands.
            This is needed because of the way X_Axis_Rotation is used in conjuction with
            mouseY.
            */
            if (isUp)
            {
                X_Axis_Rotation -= mouseY + UP;
                isUp = false;
            }
                            
            if (isDown)
            {
                X_Axis_Rotation -= mouseY + DOWN;
                isDown = false;
            }

            X_Axis_Rotation = Mathf.Clamp(X_Axis_Rotation, DOWN_ANGLE_LIMIT, UP_ANGLE_LIMIT);

            transform.localRotation = Quaternion.Euler(X_Axis_Rotation, 0f, 0f);

            playerBody.Rotate(Vector3.up * mouseX);
            #endregion
#endif

#if (UNITY_IOS || UNITY_ANDROID) && (!UNITY_STANDALONE || !UNITY_EDITOR) 

            #region Gyroscopic Movement
            mouseX = Input.gyro.rotationRateUnbiased.y * mouseSensitivity * Time.deltaTime;
            mouseY = Input.gyro.rotationRateUnbiased.x * mouseSensitivity * Time.deltaTime;

            X_Axis_Rotation -= mouseY;

            /* 
            This set of if statements will give actuate the up down voice commands.
            This is needed because of the way X_Axis_Rotation is used in conjuction with
            mouseY.
            */
            if (isUp)
            {
                X_Axis_Rotation -= mouseY + UP;
                isUp = false;
            }

            if (isDown)
            {
                X_Axis_Rotation -= mouseY + DOWN;
                isDown = false;
            }

            X_Axis_Rotation = Mathf.Clamp(X_Axis_Rotation, DOWN_ANGLE_LIMIT, UP_ANGLE_LIMIT);

            transform.localRotation = Quaternion.Euler(X_Axis_Rotation, 0f, 0f);
            playerBody.Rotate(Vector3.up * -mouseX);
            #endregion
            
#endif
        }


        #region Player Movement via Voice
        public void turn_left()
        {
            //This method is intended to give actions in game space using voice commands
            // currentX += -90 degrees
        }

        public void turn_right()
        {
            //This method is intended to give actions in game space using voice commands
            // currentX += 90 degrees
        }

        public void snap_right()
        {
            playerBody.Rotate(Vector3.up * (mouseX + RIGHT_TURN));
        }

        public void snap_left()
        {
            playerBody.Rotate(Vector3.up * (mouseX + LEFT_TURN));
        }
        #endregion

        #region Cardinal Voice Functions
        /*
        These functions are centered on the actual north position of the phone
        Input.compass.trueHeading. This function may not work for Desktop or Oculus builds.
        */
        public void north()
        {   
            playerBody.rotation = Quaternion.Euler(0, Input.compass.trueHeading, 0);
        }

        public void south()
        {
            playerBody.rotation = Quaternion.Euler(0, (Input.compass.trueHeading + 270), 0);
        }

        public void east()
        {
            playerBody.rotation = Quaternion.Euler(0, (Input.compass.trueHeading + 90), 0);
        }

        public void west()
        {
            playerBody.rotation = Quaternion.Euler(0, (Input.compass.trueHeading + 180), 0);
        }

        public void look_up()
        {
            isUp = true;
        }

        public void look_down()
        {
            isDown = true;
        }
        #endregion
    
    }
}