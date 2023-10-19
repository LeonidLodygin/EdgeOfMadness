using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public float swayAmount;
    public float swaySmoothing;
    public float swayResetSmoothing;
    public float swayClampX;
    public float swayClampY;
    private InputManager inputManager;

    private bool isInitialised;
    
    private Vector2 newWeaponRotation;
    private Vector2 newWeaponRotationVelocity;
    private Vector2 targetWeaponRotation;
    private Vector2 targetWeaponRotationVelocity;
    
    public void Initialise(InputManager manager)
    {
        inputManager = manager;
        isInitialised = true;
    }

    private void Start()
    {
        newWeaponRotation = transform.localRotation.eulerAngles;
    }

    private void LateUpdate()
    {
        if (!isInitialised) return;
         
        var mouseX = inputManager.Look.x;
        var mouseY = inputManager.Look.y;

        targetWeaponRotation.x -= mouseX * swayAmount * Time.smoothDeltaTime;
        targetWeaponRotation.y -= mouseY * swayAmount * Time.smoothDeltaTime;
        targetWeaponRotation.x = Mathf.Clamp(targetWeaponRotation.x, -swayClampX, swayClampX);
        targetWeaponRotation.y = Mathf.Clamp(targetWeaponRotation.y, -swayClampY, swayClampY);

        targetWeaponRotation = Vector2.SmoothDamp(targetWeaponRotation, Vector3.zero, ref targetWeaponRotationVelocity, swayResetSmoothing);
        newWeaponRotation = Vector2.SmoothDamp(newWeaponRotation, targetWeaponRotation, ref newWeaponRotationVelocity, swaySmoothing);
        
        transform.localRotation = Quaternion.Euler(newWeaponRotation.y, 0, newWeaponRotation.x);
    }
}
