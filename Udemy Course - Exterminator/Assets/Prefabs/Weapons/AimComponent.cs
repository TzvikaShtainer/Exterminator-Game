using UnityEngine;

namespace Prefabs.Weapons
{
    public class AimComponent : MonoBehaviour
    {
        [SerializeField] private Transform muzzle;
        [SerializeField] private float aimRange = 1000;
        [SerializeField] private LayerMask aimMask;

        public GameObject GetAimTarget(out Vector3 aimDir)
        {
            Vector3 aimStart = muzzle.position; 
            aimDir = GetAimDir();
        
            if ( Physics.Raycast(aimStart, aimDir, out RaycastHit gitInfo, aimRange, aimMask))
            {
                return gitInfo.collider.gameObject;
            }

            return null;
        }

        Vector3 GetAimDir()
        {
            Vector3 muzzleDir = muzzle.forward;
            return new Vector3(muzzleDir.x, 0f, muzzleDir.z).normalized;
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(muzzle.position, GetAimDir() * aimRange);
        }
    }
}
