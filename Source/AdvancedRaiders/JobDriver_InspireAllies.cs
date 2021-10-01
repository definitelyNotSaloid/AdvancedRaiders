﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse.AI;
using RimWorld;
using Verse;
namespace AdvancedRaiders
{
    public class JobDriver_InspireAllies : JobDriver
    {
        
        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return true;
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            Ability ability = GetActor().abilities.GetAbility(AdvancedRaidersDefOf.InspiringDrumming, true);
            AddFailCondition(() => ability == null);
            AddFailCondition(() => !ability.CanCast || ability.Casting);

            yield return null;
        }

        
    }
}