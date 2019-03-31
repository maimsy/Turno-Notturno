using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class MouthArtWork : MonoBehaviour
{
    public float minDistanceFromCamera = 0.5f;
    
    public bool a = false;
    public bool b = false;
    public bool c = false;
    
    public Transform joint0; // Yaw
    public Transform joint1; // Pitch 1
    public Transform joint2; // Pitch 2
    public Transform joint3; // Pitch 3
    public Transform joint4; // Center of mouth (rotation does nothing)
    
    private float targetHeight = 1.8f;
    private float targetDistance;
    private float targetYaw;

    private float lengthBase; // Base
    private float lengthArm1; // Arm 1
    private float lengthArm2; // Arm 2
    private float lengthMouth; // Mouth

    private float totalLength;
    private Transform target;
    
    
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
    }

    // Update is called once per frame
    void Update()
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
        joint0.localRotation = Quaternion.Euler(new Vector3(0, yaw, 0));
        joint1.localRotation = Quaternion.Euler(new Vector3(angle1, 0, -90));
        joint2.localRotation = Quaternion.Euler(new Vector3(-90, 0, angle2));
        
        // Mouth angle
        Vector3 mouthForward = joint3.position - joint2.position;
        Vector3 mouthToTarget = target.position - joint3.position;
        float mouthAngle = Vector3.SignedAngle(mouthForward, mouthToTarget, joint3.right);
        joint3.localRotation = Quaternion.Euler(mouthAngle, -90, 0);
        //Debug.Log(x);
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
