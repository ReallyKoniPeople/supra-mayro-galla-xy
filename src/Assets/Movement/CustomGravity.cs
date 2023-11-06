using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomGravity : MonoBehaviour
{
   [SerializeField] private float gravityPower =20f;
   [SerializeField] private LayerMask groundMask ;
   [SerializeField] private Rigidbody rb;
   [SerializeField] private float groundCheckDistance;

   // this controls how player is rotated from current normal to next normal
   [SerializeField] private float playerSmoothRotation;
   private Vector3 newNormal;
   public Vector3 currentNormal;

   private void Start()
   {
      rb.useGravity = false; // disable this rigidbody gravity
      rb.freezeRotation = true; // disable this rigidbody from rotating with physics
   }

   private void FixedUpdate()
   {
      ApplyGravity();
   }
   private void Update()
   {
      CheckForGravityDir();
      alignToSurface();
   }
   private void ApplyGravity()
   {
      rb.AddForce(-currentNormal*gravityPower*Time.deltaTime,ForceMode.Acceleration);
   }
   
   void CheckForGravityDir()
   {
      var colliders =Physics.OverlapSphere(transform.position, groundCheckDistance);
      Vector3 closestPoint = Vector3.zero;
      Vector3 lastSurfacePos = Vector3.zero;
      float distance = Single.MaxValue;

      int totalCollision = 0;
      foreach (var collider in colliders)
      {
         // make sure to avoid checking own collider
         if (collider.gameObject.transform.root != transform)
         {
            totalCollision++;
            float newDistance = Vector3.Distance(collider.ClosestPoint(transform.position), transform.position);
            Debug.DrawLine(transform.position,collider.ClosestPoint(transform.position));
            if (newDistance < distance)
            {
               distance = newDistance;
               closestPoint = collider.ClosestPoint(transform.position);
            }
         }
         
      }
      Vector3 rayShootDir = (closestPoint - transform.position).normalized;
      bool hitSuccess = Physics.Raycast(transform.position, rayShootDir, out RaycastHit hitInfo, groundCheckDistance,
         groundMask);
      
      if (hitSuccess)
      {
         newNormal = hitInfo.normal;
         lastSurfacePos = hitInfo.point;
      }

   }
   private void alignToSurface()
   {
      // this will smooth out change from current normal to next normal
      currentNormal = Vector3.Lerp(currentNormal, newNormal, playerSmoothRotation);
      transform.up = currentNormal;
   }  
}
