using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Player
{
    public class RotateWithCamera : MonoBehaviour
    {
        Camera _playerCamera;
        float _yRotation = 0.0f;

        const float RotationCutoff = 5.0f;
        const float InterpolationMultiplier = 2.0f;

        void Start()
        {
            _playerCamera = Camera.main;
        }

        void Update()
        {
            RotateObject();
        }

        private void RotateObject()
        {
            if (Mathf.Abs(Quaternion.Angle(transform.rotation, _playerCamera.transform.rotation)) < RotationCutoff)
                return;

            _yRotation = Mathf.LerpAngle(transform.rotation.eulerAngles.y, _playerCamera.transform.rotation.eulerAngles.y, Time.deltaTime * InterpolationMultiplier);

            transform.rotation = Quaternion.Euler(0.0f, _yRotation, 0.0f);
        }
    }
}