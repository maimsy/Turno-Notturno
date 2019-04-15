using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class MouthArtWork : MonoBehaviour
{
    public bool disabled = false;
    public float degreesPerSecond = 45f;
    public float maxDistanceFromCamera = 6f;
    public float minDistanceFromCamera = 0.5f;

    public FMODUnity.StudioEventEmitter EventEmitter1;
    public FMODUnity.StudioEventEmitter EventEmitter2;
    public FMODUnity.StudioEventEmitter EventEmitter3;
    public FMODUnity.StudioEventEmitter EventEmitter4;

    public bool enableSound1 = true;
    public bool enableSound2 = true;
    public bool enableSound3 = true;
    public bool enableSound4 = true;

    private Transform joint0; // Yaw
    private Transform joint1; // Pitch 1
    private Transform joint2; // Pitch 2
    private Transform joint3; // Pitch 3
    private Transform joint4; // Center of mouth (rotation does nothing)
    
    private Quaternion targetRotation0;
    private Quaternion targetRotation1;
    private Quaternion targetRotation2;
    private Quaternion targetRotation3;

    private Quaternion originalRotation0;
    private Quaternion originalRotation1;
    private Quaternion originalRotation2;
    private Quaternion originalRotation3;
    
    private float targetDistance;
    private float targetYaw;

    private float lengthBase; // Base
    private float lengthArm1; // Arm 1
    private float lengthArm2; // Arm 2
    private float lengthMouth; // Mouth

    private float totalLength;
    private Transform target;

    private bool soundPlaying1;
    private bool soundPlaying2;
    private bool soundPlaying3;
    private bool soundPlaying4;


    // Start is called before the first frame update
    void Start()
    {

        joint0 = transform.Find("joint0").transform;
        joint1 = transform.Find("joint0/joint1").transform;
        joint2 = transform.Find("joint0/joint1/joint2").transform;
        joint3 = transform.Find("joint0/joint1/joint2/joint3").transform;
        joint4 = transform.Find("joint0/joint1/joint2/joint3/joint4").transform;
        lengthBase = Vector3.Distance(joint0.position, joint1.position);
        lengthArm1 = Vector3.Distance(joint1.position, joint2.position);
        lengthArm2 = Vector3.Distance(joint2.position, joint3.position);
        lengthMouth = Vector3.Distance(joint3.position, joint4.position) * 2; // Joint 4 is in middle of mouth
        totalLength = lengthArm1 + lengthArm2 + lengthMouth;  // Base is static and does not affect length
        target = Camera.main.transform;

        originalRotation0 = joint0.localRotation;
        originalRotation1 = joint1.localRotation;
        originalRotation2 = joint2.localRotation;
        originalRotation3 = joint3.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (!disabled && Vector3.Distance(joint0.position, target.position) < maxDistanceFromCamera)
        {
            ComputeTargetRotations();
        }
        else
        {
            targetRotation0 = originalRotation0;
            targetRotation1 = originalRotation1;
            targetRotation2 = originalRotation2;
            targetRotation3 = originalRotation3;
        }

        

    }

    void FixedUpdate()
    {
        HandleSounds();
        SmoothRotate();
    }

    void SmoothRotate()
    {
        joint0.localRotation =
            Quaternion.RotateTowards(joint0.localRotation, targetRotation0, degreesPerSecond * Time.fixedDeltaTime);
        joint1.localRotation =
            Quaternion.RotateTowards(joint1.localRotation, targetRotation1, degreesPerSecond * Time.fixedDeltaTime);
        joint2.localRotation =
            Quaternion.RotateTowards(joint2.localRotation, targetRotation2, degreesPerSecond * Time.fixedDeltaTime);
        joint3.localRotation =
            Quaternion.RotateTowards(joint3.localRotation, targetRotation3, degreesPerSecond * Time.fixedDeltaTime* 2);  // Double speed for mouth makes it look better

        
    }

    void HandleSounds()
    {
        if(!disabled)
        {
            if (!soundPlaying1 && enableSound1 && Quaternion.Angle(joint0.localRotation, targetRotation0) > 0.1f)
            {
                //FMODUnity.RuntimeManager.PlayOneShot("event:/fx/joint1");
                StartSound(EventEmitter1, "joint1On");
                soundPlaying1 = true;
            }
            if (!soundPlaying2 && enableSound2 && Quaternion.Angle(joint1.localRotation, targetRotation1) > 0.1f)
            {
                StartSound(EventEmitter2, "joint2On");
                soundPlaying2 = true;
            }
            if (!soundPlaying3 && enableSound3 && Quaternion.Angle(joint2.localRotation, targetRotation2) > 0.1f)
            {
                StartSound(EventEmitter3, "joint3On");
                soundPlaying3 = true;
            }
            if (!soundPlaying4 && enableSound4 && Quaternion.Angle(joint3.localRotation, targetRotation3) > 0.1f)
            {
                StartSound(EventEmitter4, "joint4On");
                soundPlaying4 = true;
            }

            if (soundPlaying1 && Quaternion.Angle(joint0.localRotation, targetRotation0) < 0.001f)
            {
                //FMODUnity.RuntimeManager.PlayOneShot("event:/fx/joint1");
                StopSound(EventEmitter1, "joint1On");
                soundPlaying1 = false;
            }
            if (soundPlaying2 && Quaternion.Angle(joint1.localRotation, targetRotation1) < 0.001f)
            {
                StopSound(EventEmitter2, "joint2On");
                soundPlaying2 = false;
            }
            if (soundPlaying3 && Quaternion.Angle(joint2.localRotation, targetRotation2) < 0.001f)
            {
                StopSound(EventEmitter3, "joint3On");
                soundPlaying3 = false;
            }
            if (soundPlaying4 && Quaternion.Angle(joint3.localRotation, targetRotation3) < 0.001f)
            {
                StopSound(EventEmitter4, "joint4On");
                soundPlaying4 = false;
            }
        }
        
    }

    void StartSound(FMODUnity.StudioEventEmitter emitter, string parameterName)
    {
        //Debug.Log("Start" + parameterName);
        emitter.Play();
        emitter.SetParameter(parameterName, 1);
    }

    void StopSound(FMODUnity.StudioEventEmitter emitter, string parameterName)
    {
        emitter.SetParameter(parameterName, 0);
    }


    void ComputeTargetRotations()
    {
        // Add offset to target position, so the mouth does not clip inside the target
        Vector3 baseToAdjustedTarget = target.position - joint1.position;
        Vector3 mouthOffset = baseToAdjustedTarget;
        mouthOffset.y = 0;
        mouthOffset = mouthOffset.normalized * (lengthMouth + minDistanceFromCamera);
        baseToAdjustedTarget -= mouthOffset;
        
        // Check if adjusted target is behind the robot (but target is in front)
        Vector3 original = target.position - joint0.position;
        Vector3 adjusted = baseToAdjustedTarget;
        original.y = 0;
        adjusted.y = 0;
        float dot = Vector3.Dot(original, adjusted);
        int direction = 1;
        if (dot < 0) direction = -1;
        
        // Absolute distance to target (diagonal of the triangle in 2d)
        float distance = baseToAdjustedTarget.magnitude;
        distance = Mathf.Min(lengthArm1 + lengthArm2, distance);
        
        // Calculate target x and y in 2D  (x = distance along ground, y = height)
        baseToAdjustedTarget = baseToAdjustedTarget.normalized * distance;  
        float y = baseToAdjustedTarget.y;
        baseToAdjustedTarget.y = 0;
        float x = baseToAdjustedTarget.magnitude * direction;
        
        // SCARA-robot inverse kinematics (SCARA = 2d, 2-joint robot arm)
        float D1 = Mathf.Atan2(y, x);
        float D2 = LawOfCosines(distance, lengthArm1, lengthArm2);
        float angle1 = D1 + D2;
        float angle2 = LawOfCosines(lengthArm1, lengthArm2, distance);
        
        
        // Hardcoded angle-offsets depend on the original joint rotations in the 3d-model
        angle1 = 90 - Mathf.Rad2Deg * angle1;
        angle2 = 180 - Mathf.Rad2Deg * angle2;
        
        // Yaw angle for base-joint
        Vector3 baseToTarget = target.position - joint0.position;
        baseToTarget.y = 0;
        float yaw = Vector3.SignedAngle(new Vector3(0, 0, 1), baseToTarget, Vector3.up);
        
        // Set rotations based on their original rotations in the 3d-model
        targetRotation0 = Quaternion.Euler(new Vector3(0, yaw, 0));
        targetRotation1 = Quaternion.Euler(new Vector3(angle1, 0, -90));
        targetRotation2 = Quaternion.Euler(new Vector3(-90, 0, angle2));
        
        // Mouth angle
        Vector3 mouthForward = joint3.position - joint2.position;
        Vector3 mouthToTarget = target.position - joint3.position;
        float mouthAngle = Vector3.SignedAngle(mouthForward, mouthToTarget, joint3.right);
        targetRotation3 = Quaternion.Euler(mouthAngle, -90, 0);
    }
    

    float LawOfCosines(float a, float b, float c)
    {
        float term1 = (a * a + b * b - c * c);
        float term2 = (2 * a * b);
        float value = term1 / term2;
        value = Mathf.Clamp(value, -1.0f, 1.0f); // Avoid NaN in case of small rounding errors
        return Mathf.Acos(value);
    }
}
