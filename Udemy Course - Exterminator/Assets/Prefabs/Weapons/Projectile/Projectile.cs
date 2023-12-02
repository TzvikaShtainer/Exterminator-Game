using System;
using System.Runtime.CompilerServices;
using Prefabs.Framework;
using Prefabs.Framework.Damage;
using UnityEngine;

namespace Prefabs.Weapons.Projectile
{
    public class Projectile : MonoBehaviour, ITeamInterface
    {
        [SerializeField] private float flightHeight;
        [SerializeField] private Rigidbody rigidBody;
        [SerializeField] private DamageComponent damageComponent;
        [SerializeField] private ParticleSystem explosionVFX;

        private ITeamInterface instigatorTeamInterface;

        public void Launch(GameObject instigator, Vector3 destination)
        { 
            instigatorTeamInterface = instigator.GetComponent<ITeamInterface>();
            if (instigatorTeamInterface != null)
            {
                damageComponent.SetTeamInterfaceSrc(instigatorTeamInterface);
            }

            float gravity = Physics.gravity.magnitude;
            float halfFlightTime = Mathf.Sqrt((flightHeight * 2f) / gravity);
            
            Vector3 destinationVec = destination - transform.position;
            destinationVec.y = 0;
            float horizontalDist = destinationVec.magnitude;
            
            float upSpeed = halfFlightTime * gravity;
            float fwdSpeed = horizontalDist / (halfFlightTime * 2f);

            Vector3 flightVel = Vector3.up * upSpeed + destinationVec.normalized * fwdSpeed;
            
            rigidBody.AddForce(flightVel, ForceMode.VelocityChange);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (instigatorTeamInterface.GetRelationTowards(other.gameObject) != ETeamRelation.Friendly)
            {
                Explode();
            }
        }

        private void Explode()
        {
            Vector3 pawnPos = transform.position;
            Instantiate(explosionVFX, pawnPos, Quaternion.identity);
            
            Destroy(gameObject);
        }
    }
}
