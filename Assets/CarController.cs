using UnityEngine;

public class CarController : MonoBehaviour
{
    // Odwołania do Wheel Colliderów
    public WheelCollider FrontLeftWheel;
    public WheelCollider FrontRightWheel;
    public WheelCollider RearLeftWheel;
    public WheelCollider RearRightWheel;

    // Odwołania do wizualnych modeli kół
    public Transform FrontLeftTransform;
    public Transform FrontRightTransform;
    public Transform RearLeftTransform;
    public Transform RearRightTransform;

    // Parametry sterowania
    public float motorForce = 8000f;          // Siła napędowa
    public float brakeForce = 1500f;         // Siła hamowania
    public float maxSteerAngle = 30f;        // Maksymalny kąt skrętu
    public float antiRollForce = 3000f;      // Siła stabilizatora
    public float mass = 10f;

    private Rigidbody rb;
    private float horizontalInput;
    private float verticalInput;
    private bool isBraking;
    private float steerAngle;

    void Start()
    {
        // Pobranie komponentu Rigidbody i obniżenie środka ciężkości
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0, -0.7f, 0); // Niżej dla lepszej stabilności
    }

    void Update()
    {
        GetInput();        // Pobranie danych wejściowych od gracza
        Steer();           // Skręt kół przednich
        Accelerate();      // Przyspieszenie/hamowanie
        UpdateWheelPoses(); // Aktualizacja pozycji wizualnych modeli kół
    }

    void FixedUpdate()
    {
        ApplyAntiRollBar(FrontLeftWheel, FrontRightWheel);  // Stabilizator przód
        ApplyAntiRollBar(RearLeftWheel, RearRightWheel);    // Stabilizator tył
    }

    // Funkcja pobierająca dane wejściowe od użytkownika
    void GetInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        isBraking = Input.GetKey(KeyCode.Space); // Hamowanie ręczne
    }

    // Funkcja odpowiadająca za skręt kół przednich
    void Steer()
    {
        steerAngle = maxSteerAngle * horizontalInput;
        FrontLeftWheel.steerAngle = steerAngle;
        FrontRightWheel.steerAngle = steerAngle;
    }

    // Funkcja odpowiadająca za przyspieszanie i hamowanie
    void Accelerate()
    {
        if (!isBraking)
        {
            FrontLeftWheel.motorTorque = verticalInput * motorForce;
            FrontRightWheel.motorTorque = verticalInput * motorForce;
            ApplyBrake(0); // Brak hamowania
        }
        else
        {
            ApplyBrake(brakeForce); // Hamowanie
        }
    }

    // Funkcja odpowiadająca za aktualizację pozycji i rotacji modelu wizualnego koła
    void UpdateWheelPose(WheelCollider collider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion quat;
        collider.GetWorldPose(out pos, out quat);
        wheelTransform.position = pos;
        wheelTransform.rotation = quat;
    }

    void UpdateWheelPoses()
    {
        UpdateWheelPose(FrontLeftWheel, FrontLeftTransform);
        UpdateWheelPose(FrontRightWheel, FrontRightTransform);
        UpdateWheelPose(RearLeftWheel, RearLeftTransform);
        UpdateWheelPose(RearRightWheel, RearRightTransform);
    }

    // Funkcja stabilizatora (anti-roll bar) do przeciwdziałania przechylaniu
    void ApplyAntiRollBar(WheelCollider leftWheel, WheelCollider rightWheel)
    {
        WheelHit hit;
        float travelL = 1.0f;
        float travelR = 1.0f;

        bool groundedL = leftWheel.GetGroundHit(out hit);
        if (groundedL)
            travelL = (-leftWheel.transform.InverseTransformPoint(hit.point).y - leftWheel.radius) / leftWheel.suspensionDistance;

        bool groundedR = rightWheel.GetGroundHit(out hit);
        if (groundedR)
            travelR = (-rightWheel.transform.InverseTransformPoint(hit.point).y - rightWheel.radius) / rightWheel.suspensionDistance;

        float antiRoll = (travelL - travelR) * antiRollForce;

        if (groundedL)
            rb.AddForceAtPosition(leftWheel.transform.up * -antiRoll, leftWheel.transform.position);
        if (groundedR)
            rb.AddForceAtPosition(rightWheel.transform.up * antiRoll, rightWheel.transform.position);
    }

    // Funkcja odpowiadająca za hamowanie
    void ApplyBrake(float brakeForce)
    {
        FrontLeftWheel.brakeTorque = brakeForce;
        FrontRightWheel.brakeTorque = brakeForce;
        RearLeftWheel.brakeTorque = brakeForce;
        RearRightWheel.brakeTorque = brakeForce;
    }
}
