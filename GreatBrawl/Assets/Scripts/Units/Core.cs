using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MobileTestProject
{
    public class Core : Unit
    {
        protected override void Awake()
        {
            currentHealth = maxHealth;
        }

        public override void Place(ETeam unitTeam, Unit target)
        {
            Place(unitTeam);
        }

        public void Place(ETeam unitTeam)
        {
            team = unitTeam;
        }
        
        protected override void Update()
        {
            
        }

        protected override void FollowTriggered(Collider col)
        {
            
        }

        protected override void FollowUnTriggered(Collider col)
        {
            
        }

        protected override void OnDestroy()
        {
            
        }
    }
}
