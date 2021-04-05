using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{

    private Transform currentTarget = null;
    public float cameraDistance;
    public float cameraHeight;
    public float lookAroundAngle;
    public float lookAtAroundXAngle;
    public float lookAtAroundZAngle;

    public List<Transform> cameraTargets = null;
    private int currentIndex = 0;

    public void CameraStart(GameObject mainCharacter)
    {
        this.cameraTargets.Add(mainCharacter.transform);

        if (cameraTargets.Count > 0)
        {
            currentIndex = 0;
            currentTarget = cameraTargets[currentIndex];
        }
    }

    private void ChangeCameraTarget(int step)
    {
        if (cameraTargets.Count == 0) { return; }
        currentIndex += step;
        if (currentIndex > cameraTargets.Count - 1) { currentIndex = 0; }
        if (currentIndex < 0) { currentIndex = cameraTargets.Count - 1; }
        currentTarget = cameraTargets[currentIndex];
    }

    public void NextTarget() { ChangeCameraTarget(1); }
    public void PreviousTarget() { ChangeCameraTarget(-1); }

    private void Update()
    {
        if (cameraTargets.Count == 0) { return; }
    }

    private void LateUpdate()
    {
        if (currentTarget == null) { return; }

        Debug.Log(currentTarget.position);

        float targetHeight = currentTarget.position.y + cameraHeight;
        float currentRotationAngle = lookAroundAngle;

        Quaternion currentRotation = Quaternion.Euler(lookAtAroundXAngle, currentRotationAngle, lookAtAroundZAngle);

        Vector3 position = currentTarget.position;
        position -= currentRotation * Vector3.forward * cameraDistance;
        position.y = /*targetHeight*/20;

        transform.position = position;
        transform.LookAt(currentTarget.position + new Vector3(0, cameraHeight, 0));
    }
}
