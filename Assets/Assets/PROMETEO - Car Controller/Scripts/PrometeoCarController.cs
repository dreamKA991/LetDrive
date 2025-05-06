using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

[RequireComponent(typeof(Rigidbody))]
public class PrometeoCarController : MonoBehaviour, ICharacterCar
{

    //CAR SETUP

      [Space(20)]
      [Header("CAR SETUP")]
      [Space(10)]
      [Range(20, 190)]
      public int maxSpeed = 90; //The maximum speed that the car can reach in km/h.
      [Range(10, 120)]
      public int maxReverseSpeed = 45; //The maximum speed that the car can reach while going on reverse in km/h.
      [Range(1, 10)]
      public int accelerationMultiplier = 2; // How fast the car can accelerate. 1 is a slow acceleration and 10 is the fastest.
      [Space(10)]
      [Range(10, 45)]
      public int maxSteeringAngle = 27; // The maximum angle that the tires can reach while rotating the steering wheel.
      [Range(0.1f, 1f)]
      public float steeringSpeed = 0.5f; // How fast the steering wheel turns.
      [Space(10)]
      [Range(100, 600)]
      public int brakeForce = 350; // The strength of the wheel brakes.
      [Space(10)]
      public Vector3 bodyMassCenter; // This is a vector that contains the center of mass of the car. I recommend to set this value
                                    // in the points x = 0 and z = 0 of your car. You can select the value that you want in the y axis,
                                    // however, you must notice that the higher this value is, the more unstable the car becomes.
                                    // Usually the y value goes from 0 to 1.5.
      

    //WHEELS

      [Header("WHEELS")]

      /*
      The following variables are used to store the wheels' data of the car. We need both the mesh-only game objects and wheel
      collider components of the wheels. The wheel collider components and 3D meshes of the wheels cannot come from the same
      game object; they must be separate game objects.
      */
      public GameObject frontLeftMesh;
      public WheelCollider frontLeftCollider;
      [Space(10)]
      public GameObject frontRightMesh;
      public WheelCollider frontRightCollider;
      [Space(10)]
      public GameObject rearLeftMesh;
      public WheelCollider rearLeftCollider;
      [Space(10)]
      public GameObject rearRightMesh;
      public WheelCollider rearRightCollider;

    //PARTICLE SYSTEMS

      [Space(20)]
      [Header("EFFECTS")]
      [Space(10)]
      public bool useEffects = false;
      
      public ParticleSystem RLWParticleSystem;
      public ParticleSystem RRWParticleSystem;

      [Space(10)]
      public TrailRenderer RLWTireSkid;
      public TrailRenderer RRWTireSkid;

    //SPEED TEXT (UI)

      [Space(20)]
      [Header("UI")]
      [Space(10)]
      [SerializeField] private bool useUI = false;
      [SerializeField] private Text carSpeedText; 

    //SOUNDS

      [Space(20)]
      [Header("Sounds")]
      [Space(10)]
      [SerializeField] private bool useSounds = false;
      [SerializeField] private AudioSource carEngineSound; 
      [SerializeField] private AudioSource tireScreechSound; 
      private float initialCarEngineSoundPitch; 

    //CONTROLS

      [Space(20)]
      [Header("CONTROLS")]
      [Space(10)]
      [SerializeField] private bool useTouchControls = false;
      public GameObject throttleButton;
      PrometeoTouchInput throttlePTI;
      public GameObject reverseButton;
      PrometeoTouchInput reversePTI;
      public GameObject turnRightButton;
      PrometeoTouchInput turnRightPTI;
      public GameObject turnLeftButton;
      PrometeoTouchInput turnLeftPTI;

    // AI GAME DATA

     public bool IsBot = true;
     public float MaxBotCarSpeed = 5f;
     public float MinGameCarSpeed = 5f;
     public int RoadLaneTaked;
     private RoadConfig _roadConfig;
    //CAR DATA

      public float carSpeed { get; private set; } // Used to store the speed of the car.
      private bool isDrifting; // Used to know whether the car is drifting or not.
      private bool isTractionLocked; // Used to know whether the traction of the car is locked or not.
      private bool isLeftTurning; 
      private bool isRightTurning; 

    //PRIVATE VARIABLES

      /*
      IMPORTANT: The following variables should not be modified manually since their values are automatically given via script.
      */
      private Rigidbody carRigidbody; 
      private float steeringAxis; // Used to know whether the steering wheel has reached the maximum value. It goes from -1 to 1.
      private float throttleAxis; // Used to know whether the throttle has reached the maximum value. It goes from -1 to 1.
      private float driftingAxis;
      private float localVelocityZ;
      private float localVelocityX;
      private bool deceleratingCar;
      private bool touchControlsSetup = false;
      /*
      The following variables are used to store information about sideways friction of the wheels (such as
      extremumSlip,extremumValue, asymptoteSlip, asymptoteValue and stiffness). We change this values to
      make the car to start drifting.
      */
      private WheelFrictionCurve FLwheelFriction;
      private float FLWextremumSlip;
      private WheelFrictionCurve FRwheelFriction;
      private float FRWextremumSlip;
      private WheelFrictionCurve RLwheelFriction;
      private float RLWextremumSlip;
      private WheelFrictionCurve RRwheelFriction;
      private float RRWextremumSlip;
      
      [Inject]
      private void Construct(RoadConfig roadConfig)
      {
        _roadConfig = roadConfig;
      }

    private void Start()
    {
      carRigidbody = GetComponent<Rigidbody>();
      carRigidbody.centerOfMass = bodyMassCenter;

      //Initial setup to calculate the drift value of the car. This part could look a bit
      //complicated, but do not be afraid, the only thing we're doing here is to save the default
      //friction values of the car wheels so we can set an appropiate drifting value later.
      FLwheelFriction = new WheelFrictionCurve ();
        FLwheelFriction.extremumSlip = frontLeftCollider.sidewaysFriction.extremumSlip;
        FLWextremumSlip = frontLeftCollider.sidewaysFriction.extremumSlip;
        FLwheelFriction.extremumValue = frontLeftCollider.sidewaysFriction.extremumValue;
        FLwheelFriction.asymptoteSlip = frontLeftCollider.sidewaysFriction.asymptoteSlip;
        FLwheelFriction.asymptoteValue = frontLeftCollider.sidewaysFriction.asymptoteValue;
        FLwheelFriction.stiffness = frontLeftCollider.sidewaysFriction.stiffness;
      FRwheelFriction = new WheelFrictionCurve ();
        FRwheelFriction.extremumSlip = frontRightCollider.sidewaysFriction.extremumSlip;
        FRWextremumSlip = frontRightCollider.sidewaysFriction.extremumSlip;
        FRwheelFriction.extremumValue = frontRightCollider.sidewaysFriction.extremumValue;
        FRwheelFriction.asymptoteSlip = frontRightCollider.sidewaysFriction.asymptoteSlip;
        FRwheelFriction.asymptoteValue = frontRightCollider.sidewaysFriction.asymptoteValue;
        FRwheelFriction.stiffness = frontRightCollider.sidewaysFriction.stiffness;
      RLwheelFriction = new WheelFrictionCurve ();
        RLwheelFriction.extremumSlip = rearLeftCollider.sidewaysFriction.extremumSlip;
        RLWextremumSlip = rearLeftCollider.sidewaysFriction.extremumSlip;
        RLwheelFriction.extremumValue = rearLeftCollider.sidewaysFriction.extremumValue;
        RLwheelFriction.asymptoteSlip = rearLeftCollider.sidewaysFriction.asymptoteSlip;
        RLwheelFriction.asymptoteValue = rearLeftCollider.sidewaysFriction.asymptoteValue;
        RLwheelFriction.stiffness = rearLeftCollider.sidewaysFriction.stiffness;
      RRwheelFriction = new WheelFrictionCurve ();
        RRwheelFriction.extremumSlip = rearRightCollider.sidewaysFriction.extremumSlip;
        RRWextremumSlip = rearRightCollider.sidewaysFriction.extremumSlip;
        RRwheelFriction.extremumValue = rearRightCollider.sidewaysFriction.extremumValue;
        RRwheelFriction.asymptoteSlip = rearRightCollider.sidewaysFriction.asymptoteSlip;
        RRwheelFriction.asymptoteValue = rearRightCollider.sidewaysFriction.asymptoteValue;
        RRwheelFriction.stiffness = rearRightCollider.sidewaysFriction.stiffness;
        
        if(carEngineSound != null){
          initialCarEngineSoundPitch = carEngineSound.pitch;
        }

        // We invoke 2 methods inside this script. CarSpeedUI() changes the text of the UI object that stores
        // the speed of the car and CarSounds() controls the engine and drifting sounds. Both methods are invoked
        // in 0 seconds, and repeatedly called every 0.1 seconds.
        if(useUI){
          InvokeRepeating("CarSpeedUI", 0f, 0.1f);
        }else if(!useUI){
          if(carSpeedText != null){
            carSpeedText.text = "0";
          }
        }

        if(useSounds){
          InvokeRepeating("CarSounds", 0f, 0.1f);
        }else if(!useSounds){
          if(carEngineSound != null){
            carEngineSound.Stop();
          }
          if(tireScreechSound != null){
            tireScreechSound.Stop();
          }
        }

        if(!useEffects){
          if(RLWParticleSystem != null){
            RLWParticleSystem.Stop();
          }
          if(RRWParticleSystem != null){
            RRWParticleSystem.Stop();
          }
          if(RLWTireSkid != null){
            RLWTireSkid.emitting = false;
          }
          if(RRWTireSkid != null){
            RRWTireSkid.emitting = false;
          }
        }

        if(useTouchControls && !IsBot){
          if(throttleButton != null && reverseButton != null &&
          turnRightButton != null && turnLeftButton != null){

            throttlePTI = throttleButton.GetComponent<PrometeoTouchInput>();
            reversePTI = reverseButton.GetComponent<PrometeoTouchInput>();
            turnLeftPTI = turnLeftButton.GetComponent<PrometeoTouchInput>();
            turnRightPTI = turnRightButton.GetComponent<PrometeoTouchInput>();
            touchControlsSetup = true;

          }else{
            String ex = "Touch controls are not completely set up. You must drag and drop your scene buttons in the" +
            " PrometeoCarController component.";
            Debug.LogWarning(ex);
          }
        }

    }
    
    private void Update()
    {

      //CAR DATA
      carSpeed = (2 * Mathf.PI * frontLeftCollider.radius * frontLeftCollider.rpm * 60) / 1000;
      // Save the local velocity of the car in the x axis. Used to know if the car is drifting.
      localVelocityX = transform.InverseTransformDirection(carRigidbody.linearVelocity).x;
      // Save the local velocity of the car in the z axis. Used to know if the car is going forward or backwards.
      localVelocityZ = transform.InverseTransformDirection(carRigidbody.linearVelocity).z;
      
      AnimateWheelMeshes();
      
      if (carSpeed < MinGameCarSpeed)
      {
        GoForward();
      }
      if(carSpeed > MaxBotCarSpeed) Brakes();
      if (IsBot) return;
      
      if (useTouchControls && touchControlsSetup){

        if(throttlePTI.buttonPressed){
          deceleratingCar = false;
          GoForward();
        }
        if(reversePTI.buttonPressed){
          deceleratingCar = false;
          GoReverse();
        }

        if(turnLeftPTI.buttonPressed){
          TurnLeft();
        }
        if(turnRightPTI.buttonPressed){
          TurnRight();
        }
        if((!throttlePTI.buttonPressed && !reversePTI.buttonPressed)){
          ThrottleOff();
        }
        if((!reversePTI.buttonPressed && !throttlePTI.buttonPressed) && !deceleratingCar){
          deceleratingCar = true;
        }
        if(!turnLeftPTI.buttonPressed && !turnRightPTI.buttonPressed && steeringAxis != 0f){
          ResetSteeringAngle();
        }

      }else{

        if(Input.GetKey(KeyCode.W)){
          deceleratingCar = false;
          GoForward();
        }
        if(Input.GetKey(KeyCode.S)){
          deceleratingCar = false;
          if (carSpeed > MinGameCarSpeed) GoReverse();
        }

        if(Input.GetKey(KeyCode.A)){
          TurnLeft();
        }
        if(Input.GetKey(KeyCode.D)){
          TurnRight();
        }
        if((!Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W))){
          ThrottleOff();
        }
        if((!Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W)) && !Input.GetKey(KeyCode.Space) && !deceleratingCar){
          deceleratingCar = true;
        }
        if(!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D) && steeringAxis != 0f){
          ResetSteeringAngle();
        }

      }
    }
    
    public void CarSpeedUI(){

      if(useUI){
          try{
            float absoluteCarSpeed = Mathf.Abs(carSpeed);
            carSpeedText.text = Mathf.RoundToInt(absoluteCarSpeed).ToString();
          }catch(Exception ex){
            Debug.LogWarning(ex);
          }
      }

    }

    // This method controls the car sounds. For example, the car engine will sound slow when the car speed is low because the
    // pitch of the sound will be at its lowest point. On the other hand, it will sound fast when the car speed is high because
    // the pitch of the sound will be the sum of the initial pitch + the car speed divided by 100f.
    // Apart from that, the tireScreechSound will play whenever the car starts drifting or losing traction.
    public void CarSounds(){

      if(useSounds){
        try{
          if(carEngineSound != null){
            float engineSoundPitch = initialCarEngineSoundPitch + (Mathf.Abs(carRigidbody.linearVelocity.magnitude) / 25f);
            carEngineSound.pitch = engineSoundPitch;
          }
          if((isDrifting) || (isTractionLocked && Mathf.Abs(carSpeed) > 12f)){
            if(!tireScreechSound.isPlaying){
              tireScreechSound.Play();
            }
          }else if((!isDrifting) && (!isTractionLocked || Mathf.Abs(carSpeed) < 12f)){
            tireScreechSound.Stop();
          }
        }catch(Exception ex){
          Debug.LogWarning(ex);
        }
      }else if(!useSounds){
        if(carEngineSound != null && carEngineSound.isPlaying){
          carEngineSound.Stop();
        }
        if(tireScreechSound != null && tireScreechSound.isPlaying){
          tireScreechSound.Stop();
        }
      }

    }

    private void OnCollisionEnter(Collision other)
    {
      Debug.Log("OnCollisionEnter called. " + IsBot);
      if (IsBot) return;
      
      SignalBus.onCharacterLose?.Invoke();
    }

    //
    //STEERING METHODS
    //
    
    public void TurnLeft(){
      steeringAxis = steeringAxis - (Time.deltaTime * 10f * steeringSpeed);
      if(steeringAxis < -1f){
        steeringAxis = -1f;
      }
      var steeringAngle = steeringAxis * maxSteeringAngle;
      frontLeftCollider.steerAngle = Mathf.Lerp(frontLeftCollider.steerAngle, steeringAngle, steeringSpeed);
      frontRightCollider.steerAngle = Mathf.Lerp(frontRightCollider.steerAngle, steeringAngle, steeringSpeed);
      rearLeftCollider.steerAngle = Mathf.Lerp(frontLeftCollider.steerAngle, steeringAngle, steeringSpeed);
      rearRightCollider.steerAngle = Mathf.Lerp(frontRightCollider.steerAngle, steeringAngle, steeringSpeed);
      carRigidbody.AddForce(transform.forward * Math.Clamp(carSpeed, 0, 1), ForceMode.Acceleration);
      isLeftTurning = true;
      DriftCarPS();
    }
    
    public void TurnRight(){
      steeringAxis = steeringAxis + (Time.deltaTime * 10f * steeringSpeed);
      if(steeringAxis > 1f){
        steeringAxis = 1f;
      }
      var steeringAngle = steeringAxis * maxSteeringAngle;
      frontLeftCollider.steerAngle = Mathf.Lerp(frontLeftCollider.steerAngle, steeringAngle, steeringSpeed);
      frontRightCollider.steerAngle = Mathf.Lerp(frontRightCollider.steerAngle, steeringAngle, steeringSpeed);
      rearLeftCollider.steerAngle = Mathf.Lerp(frontLeftCollider.steerAngle, steeringAngle, steeringSpeed);
      rearRightCollider.steerAngle = Mathf.Lerp(frontRightCollider.steerAngle, steeringAngle, steeringSpeed);
      isRightTurning = true;
      DriftCarPS();
    }
    
    public void ResetSteeringAngle(){
      if(steeringAxis < 0f){
        steeringAxis = steeringAxis + (Time.deltaTime * 10f * steeringSpeed);
      }else if(steeringAxis > 0f){
        steeringAxis = steeringAxis - (Time.deltaTime * 10f * steeringSpeed);
      }
      if(Mathf.Abs(frontLeftCollider.steerAngle) < 1f){
        steeringAxis = 0f;
      }
      var steeringAngle = steeringAxis * maxSteeringAngle;
      frontLeftCollider.steerAngle = Mathf.Lerp(frontLeftCollider.steerAngle, steeringAngle, steeringSpeed);
      frontRightCollider.steerAngle = Mathf.Lerp(frontRightCollider.steerAngle, steeringAngle, steeringSpeed);
      rearLeftCollider.steerAngle = Mathf.Lerp(frontLeftCollider.steerAngle, steeringAngle, steeringSpeed);
      rearRightCollider.steerAngle = Mathf.Lerp(frontRightCollider.steerAngle, steeringAngle, steeringSpeed);
      isLeftTurning = false;
      isRightTurning = false;
      DriftCarPS();
    }
    
    public void AIResetSteeringAngle()
    {
      steeringAxis = 0f;
      float steeringAngle = 0f;

      frontLeftCollider.steerAngle = steeringAngle;
      frontRightCollider.steerAngle = steeringAngle;
      rearLeftCollider.steerAngle = steeringAngle;
      rearRightCollider.steerAngle = steeringAngle;

      isLeftTurning = false;
      isRightTurning = false;

      DriftCarPS();
    }

    
    void AnimateWheelMeshes(){
      try{
        Vector3 Position;
        Quaternion Rotation;
        frontLeftCollider.GetWorldPose(out Position, out Rotation);
        frontLeftMesh.transform.position = Position;
        frontLeftMesh.transform.rotation = Rotation;
        
        frontRightCollider.GetWorldPose(out Position, out Rotation);
        frontRightMesh.transform.position = Position;
        frontRightMesh.transform.rotation = Rotation;
        
        rearLeftCollider.GetWorldPose(out Position, out Rotation);
        rearLeftMesh.transform.position = Position;
        rearLeftMesh.transform.rotation = new Quaternion(Rotation.x, 0, 0, Rotation.w);
        
        rearRightCollider.GetWorldPose(out Position, out Rotation);
        rearRightMesh.transform.position = Position;
        rearRightMesh.transform.rotation = new Quaternion(Rotation.x, 0, 0, Rotation.w);
      }catch(Exception ex){
        Debug.LogWarning(ex);
      }
    }

    //
    //ENGINE AND BRAKING METHODS
    //

    // This method apply positive torque to the wheels in order to go forward.
    public void GoForward(){
      throttleAxis = throttleAxis + (Time.deltaTime * 3f);
      if(throttleAxis > 1f){
        throttleAxis = 1f;
      }
      
      if(localVelocityZ < -1f){ //If the car is going backwards, then apply brakes in order to avoid strange behaviours
        Brakes();
      }else{
        if(Mathf.RoundToInt(carSpeed) < maxSpeed){
          //Apply positive torque in all wheels to go forward if maxSpeed has not been reached.
          frontLeftCollider.brakeTorque = 0;
          frontLeftCollider.motorTorque = (accelerationMultiplier * 50f) * throttleAxis;
          frontRightCollider.brakeTorque = 0;
          frontRightCollider.motorTorque = (accelerationMultiplier * 50f) * throttleAxis;
          rearLeftCollider.brakeTorque = 0;
          rearLeftCollider.motorTorque = (accelerationMultiplier * 50f) * throttleAxis;
          rearRightCollider.brakeTorque = 0;
          rearRightCollider.motorTorque = (accelerationMultiplier * 50f) * throttleAxis;
        }else {
          // If the maxSpeed has been reached, then stop applying torque to the wheels.
          // IMPORTANT: The maxSpeed variable should be considered as an approximation; the speed of the car
          // could be a bit higher than expected.
    			frontLeftCollider.motorTorque = 0;
    			frontRightCollider.motorTorque = 0;
          rearLeftCollider.motorTorque = 0;
    			rearRightCollider.motorTorque = 0;
    		}
      }
    }

    // This method apply negative torque to the wheels in order to go backwards.
    public void GoReverse(){
      throttleAxis = throttleAxis - (Time.deltaTime * 3f);
      if(throttleAxis < -1f){
        throttleAxis = -1f;
      }
      //If the car is still going forward, then apply brakes in order to avoid strange
      //behaviours. If the local velocity in the 'z' axis is greater than 1f, then it
      //is safe to apply negative torque to go reverse.
      if(localVelocityZ > 1f){
        Brakes();
      }else{
        if(Mathf.Abs(Mathf.RoundToInt(carSpeed)) < maxReverseSpeed){
          //Apply negative torque in all wheels to go in reverse if maxReverseSpeed has not been reached.
          frontLeftCollider.brakeTorque = 0;
          frontLeftCollider.motorTorque = (accelerationMultiplier * 50f) * throttleAxis;
          frontRightCollider.brakeTorque = 0;
          frontRightCollider.motorTorque = (accelerationMultiplier * 50f) * throttleAxis;
          rearLeftCollider.brakeTorque = 0;
          rearLeftCollider.motorTorque = (accelerationMultiplier * 50f) * throttleAxis;
          rearRightCollider.brakeTorque = 0;
          rearRightCollider.motorTorque = (accelerationMultiplier * 50f) * throttleAxis;
        }else {
          //If the maxReverseSpeed has been reached, then stop applying torque to the wheels.
          // IMPORTANT: The maxReverseSpeed variable should be considered as an approximation; the speed of the car
          // could be a bit higher than expected.
    			frontLeftCollider.motorTorque = 0;
    			frontRightCollider.motorTorque = 0;
          rearLeftCollider.motorTorque = 0;
    			rearRightCollider.motorTorque = 0;
    		}
      }
    }
    
    public void ThrottleOff(){
      frontLeftCollider.motorTorque = 0;
      frontRightCollider.motorTorque = 0;
      rearLeftCollider.motorTorque = 0;
      rearRightCollider.motorTorque = 0;
    }

    public void Brakes(){
      frontLeftCollider.brakeTorque = brakeForce;
      frontRightCollider.brakeTorque = brakeForce;
      rearLeftCollider.brakeTorque = brakeForce;
      rearRightCollider.brakeTorque = brakeForce;
    }
    
    public void DriftCarPS(){

      if(useEffects){
        try{
          if(isLeftTurning){
            RLWParticleSystem.Play();
          }
          else if(isRightTurning) {
            RRWParticleSystem.Play();
          }
          else
          {
            RLWParticleSystem.Stop();
            RRWParticleSystem.Stop();
          }
        }catch(Exception ex){
          Debug.LogWarning(ex);
        }

        try{
          if((isTractionLocked || Mathf.Abs(localVelocityX) > 5f) && Mathf.Abs(carSpeed) > 12f){
            RLWTireSkid.emitting = true;
            RRWTireSkid.emitting = true;
          }else {
            RLWTireSkid.emitting = false;
            RRWTireSkid.emitting = false;
          }
        }catch(Exception ex){
          Debug.LogWarning(ex);
        }
      }else {
        if(RLWParticleSystem != null){
          RLWParticleSystem.Stop();
        }
        if(RRWParticleSystem != null){
          RRWParticleSystem.Stop();
        }
        if(RLWTireSkid != null){
          RLWTireSkid.emitting = false;
        }
        if(RRWTireSkid != null){
          RRWTireSkid.emitting = false;
        }
      }
    }
    
  public Transform GetTransform() => transform;
  
  public void ChangeLine()
  {

    switch (RoadLaneTaked)
    {
      case 0: // right lane
        StartCoroutine(AILeftTurnCoroutine(_roadConfig.XCoordLines[1]));
        break;
      case 1: // left lane
        StartCoroutine(AIRightTurnCoroutine(_roadConfig.XCoordLines[0]));
        break;
      default:
        Debug.LogWarning("Неизвестная линия дороги: " + RoadLaneTaked);
        break;
    }
  }

  private IEnumerator AILeftTurnCoroutine(float tillXCoord)
  {
    while (transform.position.x > tillXCoord)
    {
      TurnLeft(); 
      yield return null;
    }
    AIResetSteeringAngle();
    RoadLaneTaked = 1;
  }
  
  private IEnumerator AIRightTurnCoroutine(float tillXCoord)
  {
    while (transform.position.x < tillXCoord)
    {
      TurnRight();
      yield return null;
    }
    AIResetSteeringAngle();
    RoadLaneTaked = 0;
  }

}
