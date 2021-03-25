using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{

    private Transform m_currentTarget = null;
    public float m_distance;
    public float m_height;
    public float m_lookAtAroundAngle;
    public List<Transform> m_targets = null;
    private int m_currentIndex = 0;

    public void CameraStart(GameObject mainCharacter)
    {
        this.m_targets.Add(mainCharacter.transform);

        if (m_targets.Count > 0)
        {
            m_currentIndex = 0;
            m_currentTarget = m_targets[m_currentIndex];
        }
    }

    private void SwitchTarget(int step)
    {
        if (m_targets.Count == 0) { return; }
        m_currentIndex += step;
        if (m_currentIndex > m_targets.Count - 1) { m_currentIndex = 0; }
        if (m_currentIndex < 0) { m_currentIndex = m_targets.Count - 1; }
        m_currentTarget = m_targets[m_currentIndex];
    }

    public void NextTarget() { SwitchTarget(1); }
    public void PreviousTarget() { SwitchTarget(-1); }

    private void Update()
    {
        if (m_targets.Count == 0) { return; }
    }

    private void LateUpdate()
    {
        Debug.Log("running late");
        if (m_currentTarget == null) { return; }

        float targetHeight = m_currentTarget.position.y + m_height;
        float currentRotationAngle = m_lookAtAroundAngle;

        Quaternion currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

        Vector3 position = m_currentTarget.position;
        position -= currentRotation * Vector3.forward * m_distance;
        position.y = targetHeight;

        transform.position = position;
        transform.LookAt(m_currentTarget.position + new Vector3(0, m_height, 0));
    }
}
