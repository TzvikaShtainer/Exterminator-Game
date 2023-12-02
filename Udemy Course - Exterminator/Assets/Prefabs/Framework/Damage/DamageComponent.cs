using UnityEngine;

namespace Prefabs.Framework.Damage
{
    public abstract class DamageComponent : MonoBehaviour, ITeamInterface
    {
        [SerializeField] protected bool damageFriendly;
        [SerializeField] protected bool damageEnemy;
        [SerializeField] protected bool damageNeutral;
        ITeamInterface teamInterface;

        public int GetTeamID()
        {
            if (teamInterface != null)
                return teamInterface.GetTeamID();
            return -1;
        }
        public void SetTeamInterfaceSrc(ITeamInterface teamInterface)
        {
            this.teamInterface = teamInterface;
        }
        public bool ShouldDamage(GameObject other)
        {
            //Debug.Log(teamInterface);
            if (teamInterface == null)
                return false;
            
            ETeamRelation relation = teamInterface.GetRelationTowards(other);

            if (damageFriendly && relation == ETeamRelation.Friendly)
                return true;

            if (damageEnemy && relation == ETeamRelation.Enemy)
                return true;

            if (damageNeutral && relation == ETeamRelation.Neutral)
                return true;

            return false;
        }
    }
}
