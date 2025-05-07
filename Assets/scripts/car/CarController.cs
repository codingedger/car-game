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
        //ToDo: add checks if wheels are present
        foreach (var wheel in wheels) {
            wheel.collider.motorTorque = moveInput.y * powerMultiplier;
        }
        float steer = moveInput.x * maxSteer;
        //ToDo: add math lerp or some sort of interpolation
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
            Quaternion Rot;
            Vector3 Pos;
            wheels[i].collider.GetWorldPose(out Pos, out Rot);

            Transform[] ChildTranforms = new Transform[wheels[i].collider.transform.childCount];
            int index = 0;
            foreach (var item in ChildTranforms) {
                wheels[i].collider.transform.GetChild(index).position = Pos;
                wheels[i].collider.transform.GetChild(index).rotation = Rot;
                index++;
            }

            //ToDo: this can be used to rotate the brake calipers
            //wheels[i].collider.transform.localRotation = Quaternion.Euler(0 , wheels[i].collider.transform.rotation.eulerAngles.y, 0);
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
