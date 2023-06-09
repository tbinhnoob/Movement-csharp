﻿using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[System.Serializable]
public class FPSMouseLook {

    public float xSensitivity = 2f;
    public float ySensitivity = 2f;
    public bool clampVerticalRotation = true;
    public bool clampHorizontalRotation = true;
    public float minimumX = -90F;
    public float minimumY = -90F;
    public float maximumX = 90F;
    public float maximumY = 90F;
    public bool smooth = false;
    public float smoothTime = 2f;
    public bool lockCursor = true;
    public GameObject player;

    private Transform m_camera;
    private Transform m_character;
    private Quaternion m_characterRot;
    private Quaternion m_cameraRot;
    private bool m_cursorIsLocked = true;

    public void Init(Transform character, Transform camera)
    {
        m_character = character;
        m_camera = camera;
        m_characterRot = character.localRotation;
        m_cameraRot = camera.localRotation;

        UpdateCursorLock();
    }

    public void LookRotation()
    {
        float yRot = Input.GetAxis("Mouse X") * xSensitivity;
        float xRot = Input.GetAxis("Mouse Y") * ySensitivity;

        m_characterRot *= Quaternion.Euler(0f, yRot, 0f);
        m_cameraRot *= Quaternion.Euler(-xRot, 0f, 0f);
        if (clampVerticalRotation)
            m_cameraRot = ClampRotationAroundXAxis(m_cameraRot);
        

        if (smooth)
        {
            m_character.localRotation = Quaternion.Lerp(m_character.localRotation, m_characterRot,
                    smoothTime * Time.deltaTime);
            m_camera.localRotation = Quaternion.Lerp(m_camera.localRotation, m_cameraRot,
                smoothTime * Time.deltaTime);
        }
        else
        {
            m_character.localRotation = m_characterRot;
            //m_camera.localrotation = m_characterRot;
            m_camera.localRotation = m_cameraRot;
        }
    }

    public void UpdateCursorLock()
    {
        if (m_cursorIsLocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else if (!m_cursorIsLocked)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private Quaternion ClampRotationAroundXAxis(Quaternion q)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);

        angleX = Mathf.Clamp(angleX, minimumX, maximumX);

        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

        return q;
    }
}