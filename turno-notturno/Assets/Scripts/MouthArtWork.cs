using FMODUnity;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class MouthArtWork : MonoBehaviour
{
    public bool disabled = false;
    public float degreesPerSecond = 45f;
    public float maxDistanceFromCamera = 6f;
    public float minDistanceFromCamera = 1f;
    
    private float targetDistance;
    private float targetYaw;

    private float lengthBase; // Base
    private float lengthArm1; // Arm 1
    private float lengthArm2; // Arm 2
    private float lengthMouth; // Mouth

    private float totalLength;
    private Transform target;

    private bool[] soundIsPlaying;
    private Transform[] joints;
    private Quaternion[] originalRotations;
    private Quaternion[] targetRotations;
    private StudioEventEmitter[] eventEmitters;


    // Start is called before the first frame update
    void Start()
    {
        target = Camera.main.transform;

        // Get joints, sound emitters and original rotations
        joints = new Transform[5];
        eventEmitters = new StudioEventEmitter[5];
        originalRotations = new Quaternion[5];
        targetRotations = new Quaternion[5];
        soundIsPlaying = new bool[5];
        string baseName = "joint";
        Transform prevJoint = transform;

        for (int i = 0; i < 5; i++)
        {
            Transform joint = prevJoint.Find(baseName + i); // Transform hierarchy is expected to be "joint0/joint1/joint2/joint3/joint4"
            joints[i] = joint;
            eventEmitters[i] = joint.GetComponent<StudioEventEmitter>(); // Don't check for nulls, because we want the joint index match to emitter index
            originalRotations[i] = joint.localRotation;
            targetRotations[i] = joint.localRotation;
            prevJoint = joint;
        }

        // Store lengths for convenience
        lengthBase = Vector3.Distance(joints[0].position, joints[1].position);
        lengthArm1 = Vector3.Distance(joints[1].position, joints[2].position);
        lengthArm2 = Vector3.Distance(joints[2].position, joints[3].position);
        lengthMouth = Vector3.Distance(joints[3].position, joints[4].position) * 2; // Joint 5 is in middle of mouth, multiply by two to get mouth diameter
        totalLength = lengthArm1 + lengthArm2 + lengthMouth;  // Base is static and does not affect length
    }

    void Update()
    {
        // Update target rotations
        bool playerIsDownstairs = target.position.y < joints[0].position.y;
        if (!disabled && !playerIsDownstairs && Vector3.Distance(joints[0].position, target.position) < maxDistanceFromCamera)
        {
            ComputeTargetRotations();
        }
        else
        {
            originalRotations.CopyTo(targetRotations, 0);
        }
    }

    void FixedUpdate()
    {
        // Update position and sounds
        HandleSounds();
        HandleRotation();
    }

    void HandleSounds()
    {
        if (disabled) return;
        for (int i = 0; i < eventEmitters.Length; i++)
        {
            // Parameters are named from joint1On to joint5On (Index starts from 1)
            string parameter = "joint" + (i + 1) + "On"; 
            if (!soundIsPlaying[i] && Quaternion.Angle(joints[i].localRotation, targetRotations[i]) > 0.1f)
            {
                // Play sounds when joint is moving
                eventEmitters[i].Play();
                eventEmitters[i].SetParameter(parameter, 1);
                soundIsPlaying[i] = true;
            }
            else if (soundIsPlaying[i] && Quaternion.Angle(joints[i].localRotation, targetRotations[i]) < 0.001f)
            {
                // Stop sounds when joint is approximately at the correct position
                eventEmitters[i].SetParameter(parameter, 0);
                soundIsPlaying[i] = false;
            }
        }
    }

    void HandleRotation()
    {
        for (int i = 0; i < joints.Length; i++)
        {
            joints[i].localRotation = Quaternion.RotateTowards(joints[i].localRotation, targetRotations[i], degreesPerSecond * Time.fixedDeltaTime);
        }
    }

    void ComputeTargetRotations()
    {
        // Add offset to target position, so the mouth does not clip inside the target
        Vector3 baseToAdjustedTarget = target.position - joints[1].position;
        Vector3 mouthOffset = baseToAdjustedTarget;
        mouthOffset.y = 0;
        mouthOffset = mouthOffset.normalized * (lengthMouth + minDistanceFromCamera);
        baseToAdjustedTarget -= mouthOffset;
        
        // Check if adjusted target is behind the robot (but target is in front)
        Vector3 original = target.position - joints[0].position;
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
        
        // Inverse kinematics for 2-joint 2d robot
        float D1 = Mathf.Atan2(y, x);
        float D2 = LawOfCosines(distance, lengthArm1, lengthArm2);
        float angle1 = D1 + D2;
        float angle2 = LawOfCosines(lengthArm1, lengthArm2, distance);
        
        // Hardcoded angle-offsets depend on the original joint rotations in the 3d-model
        angle1 = 90 - Mathf.Rad2Deg * angle1;
        angle2 = 180 - Mathf.Rad2Deg * angle2;
        
        // Yaw angle for base-joint
        Vector3 baseToTarget = target.position - joints[0].position;
        baseToTarget.y = 0;
        float yaw = Vector3.SignedAngle(new Vector3(0, 0, 1), baseToTarget, Vector3.up);
        
        // Set rotations based on their original rotations in the 3d-model
        targetRotations[0] = Quaternion.Euler(new Vector3(0, yaw, 0));
        targetRotations[1] = Quaternion.Euler(new Vector3(angle1, 0, -90));
        targetRotations[2] = Quaternion.Euler(new Vector3(-90, 0, angle2));

        // Mouth angle
        // TODO: This angle is wrong until the other joints reach their target (mouth movement looks weird)
        Vector3 mouthForward = joints[3].position - joints[2].position;  
        Vector3 mouthToTarget = target.position - joints[3].position;
        float mouthAngle = Vector3.SignedAngle(mouthForward, mouthToTarget, joints[3].right);
        Debug.Log(mouthAngle);
        targetRotations[3] = Quaternion.Euler(mouthAngle, -90, 0);
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
