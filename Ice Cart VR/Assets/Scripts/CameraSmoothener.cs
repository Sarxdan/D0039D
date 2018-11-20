using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSmoothener : MonoBehaviour {

    private Quaternion startRotation;
    // The object to be "followed" but with smooth camera
    public Transform smoothTarget;
    public float zLockAngle = 180;
    public float xLockAngle = 180;
    // How smooth the camera will move when lerping between rotations
    public float lerpSpeed = 1.0f;

	void Start () {
        // Saves start rotation of camera
        startRotation = new Quaternion(transform.localRotation.x, transform.localRotation.y, transform.localRotation.z, transform.localRotation.w);
    }

    void Update()
    {
        // Gets the needed correction of the camera for smoothening
        float targetZRotation = smoothTarget.eulerAngles.z;
        float targetXRotation = smoothTarget.eulerAngles.x;

        // Checks if correction is to big for specified smoothening setting
        if (targetZRotation > zLockAngle ^ -targetZRotation < zLockAngle - 360)
        {
            targetZRotation = 0;
        }
        if (targetXRotation > xLockAngle ^ -targetXRotation < xLockAngle - 360)
        {
            targetXRotation = 0;
        }

        // Lerp to target rotation
        Quaternion targetRot = Quaternion.Euler(startRotation.eulerAngles.x - targetXRotation, startRotation.eulerAngles.y, startRotation.eulerAngles.z - targetZRotation);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRot, lerpSpeed);
    }
}
