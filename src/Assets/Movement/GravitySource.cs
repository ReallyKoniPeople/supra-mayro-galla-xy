// The MIT License (MIT)
// Copyright (c) 2016 David Evans @phosphoer

// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using UnityEngine;
using System.Collections.Generic;
using System.Linq;

// USAGE: 
// Put this on objects that you wish to have mario galaxy style gravity attraction
// You must have at least one collider in the gravityColliders list, it will be used to determine gravity direction 
// All rigidbodies that use gravity and enter the trigger zone of this object will be affected  
// Each object with this component needs to have a trigger collider on it that defines the
// boundaries of the gravitational effect 
public class GravitySource : MonoBehaviour
{
    [Tooltip("How much gravity force to apply to objects within range")]
    public float Gravity = 9.81f;

    [Tooltip("The maximum distance from the surface of the gravity source that is still affected by gravity")]
    public float Radius = 5.0f;

    [Tooltip("List of colliders to use as gravity sources, will be raycasted against")]
    [SerializeField]
    private Collider[] gravityColliders;

    // How far should raycasts go, make this the maximum distance you need gravity to affect objects from
    private const float kRaycastDistance = 100.0f;

    private List<Rigidbody> objectsInRange = new List<Rigidbody>();

    private void OnDrawGizmos()
    {
        if (Camera.current == null)
            return;

        // Visualize gravity radius 
        Gizmos.color = Color.blue;
        for (var i = 0; gravityColliders != null && i < gravityColliders.Length; ++i)
        {
            var col = gravityColliders[i];
            var raycastFrom = col.transform.position + Camera.current.transform.up * 1000.0f;
            var raycastDir = (col.transform.position - raycastFrom).normalized;
            var ray = new Ray(raycastFrom, raycastDir);
            RaycastHit hitInfo;
            if (col.Raycast(ray, out hitInfo, 2000.0f))
            {
                Gizmos.DrawLine(hitInfo.point, hitInfo.point + hitInfo.normal * Radius);
            }
        }
    }

    private void Start()
    {
        if (gravityColliders == null || gravityColliders.Length == 0)
        {
            Debug.LogWarning("GravitySource has no colliders, will not be functional");
        }
    }

    private void OnTriggerStay(Collider c)
    {
        var rb = c.GetComponent<Rigidbody>();
        if (rb != null && !objectsInRange.Contains(rb))
        {
            objectsInRange.Add(rb);

            var item = rb.GetComponent<GravityItem>() ?? rb.gameObject.AddComponent<GravityItem>();
            ++item.ActiveFieldCount;
        }
    }

    private void OnTriggerExit(Collider c)
    {
        var rb = c.GetComponent<Rigidbody>();
        if (rb != null && objectsInRange.Contains(rb))
        {
            objectsInRange.Remove(rb);

            var item = rb.GetComponent<GravityItem>() ?? rb.gameObject.AddComponent<GravityItem>();
            --item.ActiveFieldCount;
            item.CurrentDistance = Mathf.Infinity;
            item.CurrentGravitySource = null;
        }
    }

    private void FixedUpdate()
    {
        if (objectsInRange.Any())
        {
            Physics.gravity = Vector3.zero;
        }
        else
        {
            Physics.gravity = new Vector3(0, -9.81f, 0);
        }

        // Iterate over each object within range of our gravity
        for (var i = 0; objectsInRange != null && i < objectsInRange.Count; ++i)
        {
            if (objectsInRange[i] == null || !objectsInRange[i].useGravity)
                continue;

            // Calculate initial gravity direction, just towards the gravity source transform
            var rb = objectsInRange[i];
            var gravityDir = (transform.position - rb.transform.position).normalized;

            // Find out which of our child colliders is closest
            var closestHit = Mathf.Infinity;
            for (var j = 0; j < gravityColliders.Length; ++j)
            {
                // Step 1, raycast in general direction of collider to find a normal of the 
                // surface
                RaycastHit hitInfo = new RaycastHit();
                var raycastTo = gravityColliders[j].transform.position;
                var toCollider = (raycastTo - rb.transform.position).normalized;
                var gravityRay = new Ray(rb.transform.position, toCollider);

                //todo: find out what this block does and why it throws an exception
                if (gravityColliders[j].Raycast(gravityRay, out hitInfo, kRaycastDistance))
                {
                    Debug.DrawRay(gravityRay.origin, gravityRay.direction * 2, Color.red);
                    Debug.DrawRay(hitInfo.point, hitInfo.normal * 2, Color.red);

                    // Now, set our new ray to point in the opposite direction of this normal, 
                    // to raycast 'down' towards the closest point on the plane formed by the normal
                    gravityRay = new Ray(rb.transform.position, -hitInfo.normal);

                    // Update gravity direction guess if this was a closer hit
                    var dist = Vector3.Distance(hitInfo.point, gravityRay.origin);
                    if (dist < closestHit)
                    {
                        gravityDir = -hitInfo.normal;
                        closestHit = dist;
                    }
                }


                //todo: find out what this block does and why it throws an exception
                if (gravityColliders[j].Raycast(gravityRay, out hitInfo, kRaycastDistance))
                {
                    Debug.DrawRay(gravityRay.origin, gravityRay.direction * 2, Color.green);
                    Debug.DrawRay(hitInfo.point, hitInfo.normal * 2, Color.green);
                    var dist = Vector3.Distance(hitInfo.point, gravityRay.origin);
                    if (dist < closestHit)
                    {
                        gravityDir = -hitInfo.normal;
                        closestHit = dist;
                    }
                }
            }

            Debug.DrawRay(rb.transform.position, gravityDir * 2, Color.blue);

            // Now apply gravity if we are the closest source (only 1 source at a time applies gravity)
            var item = rb.GetComponent<GravityItem>();
            //todo: remove 'true' from if-statement. Cannot be removed right now because the previous if-statements dont work yet.
            if (item.CurrentGravitySource == this || closestHit < item.CurrentDistance || true)
            {
                // Update tracking vars 
                item.CurrentDistance = closestHit;
                item.CurrentGravitySource = this;
                item.Up = Vector3.Lerp(item.Up, -gravityDir.normalized, Time.deltaTime * 2.0f);

                // Calculate force
                var force = gravityDir.normalized * Gravity;
                var distRatio = Mathf.Clamp01(closestHit / Radius);

                // Gravity gets scaled up with distance because games
                force *= 1.0f + distRatio;
                rb.AddForce(force * rb.mass);
            }
        }
    }
}