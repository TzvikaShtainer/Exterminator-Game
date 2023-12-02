using UnityEngine;

namespace Prefabs.Framework.AI
{
    public class MovementComponent : MonoBehaviour
    {
        [SerializeField] private float turnSpeed = 8f;
        
        public float RotateTowards(Vector3 aimDir)
        {
            float currentTurnSpeed = 0; //so we will be in idle if the is no movement

            if (aimDir.magnitude != 0)
            {
                Quaternion prevRot = transform.rotation;

                float turnLerpAlpha = turnSpeed * Time.deltaTime;
                //transform.LookAt(rightDir * moveInput.x + upDir * moveInput.y); //another way to rotate the obj to the walking dir
                //lerp for smoother rotation when we start to move
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(aimDir, Vector3.up),
                    turnLerpAlpha); //up makes us look up with the head

                Quaternion currentRot = transform.rotation;
                float dir = Vector3.Dot(aimDir, transform.right) > 0 ? 1 : -1;
                float rotationDelta = Quaternion.Angle(prevRot, currentRot) * dir;
                currentTurnSpeed = rotationDelta / Time.deltaTime; //normalized it with the time
            }

            return currentTurnSpeed;
        }
    }
}
