using UnityEngine;
using UnityEngine.InputSystem;



public class CarController : MonoBehaviour {

    public Wheel[] wheels;
    public Vector2 moveInput;
    public float powerMultiplier = 1;
    public float maxSteer = 30, wheelbase = 2.5f, trackwidth = 1.5f;

    public void OnMove(InputValue value) {
        moveInput = value.Get<Vector2>();
    }

    void FixedUpdate() {
        foreach (var wheel in wheels) {
            wheel.collider.motorTorque = moveInput.y * powerMultiplier;
        }
        float steer = moveInput.x * maxSteer;
        if (moveInput.x > 0) {
            wheels[0].collider.steerAngle = Mathf.Rad2Deg * Mathf.Atan(wheelbase / (trackwidth / 2 + Mathf.Tan(Mathf.Deg2Rad * steer) * wheelbase));
            wheels[1].collider.steerAngle = steer;
        } else if (moveInput.x < 0) {
            wheels[0].collider.steerAngle = steer;
            wheels[1].collider.steerAngle = Mathf.Rad2Deg * Mathf.Atan(wheelbase / (-trackwidth / 2 + Mathf.Tan(Mathf.Deg2Rad * steer) * wheelbase));
        } else {
            wheels[0].collider.steerAngle = wheels[1].collider.steerAngle = 0;
        }

        for (int i = 0; i < wheels.Length; i++) {
            wheels[i].collider.transform.localRotation = Quaternion.Euler(0, wheels[i].collider.steerAngle, 0);
        }
    }

}

[System.Serializable]
public class Wheel {
    public WheelCollider collider;
    public WheelType wheelType;
}


[System.Serializable]
public enum WheelType {
    front, rear
}
