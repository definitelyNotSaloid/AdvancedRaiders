﻿using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace AdvancedRaiders
{
    public class JobDriver_MakeUkuphilaZombie : JobDriver
    {
        public Corpse TargetCorpse => TargetThingA as Corpse;
        public Thing UkuphilaHerb => TargetThingB;
        
        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return true;
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            AddEndCondition(delegate
            {
                if (!GetActor().CanReserve(TargetA) && !GetActor().HasReserved(TargetA))
                {
                    return JobCondition.Incompletable;
                }

                return JobCondition.Ongoing;
            });


            this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
            this.AddFailCondition(() => (!GetActor().CanReserve(TargetA) && !GetActor().HasReserved(TargetA)));

            yield return Toils_Reserve.Reserve(TargetIndex.A);
            yield return Toils_Goto.Goto(TargetIndex.A, PathEndMode.InteractionCell);
            yield return Toils_Misc.TakeItemFromInventoryToCarrier(GetActor(), TargetIndex.B);
            yield return Toils_General.Wait(50);
            yield return Toils_General.Do((Action)this.MakeUkuphilaZombie);
            yield return Toils_General.Do(() =>
            {
                if (GetActor().GetLord() != null && !GetActor().GetLord().ownedPawns.Contains(TargetCorpse.InnerPawn))
                    GetActor().GetLord().AddPawn(TargetCorpse.InnerPawn);
            });     //death is not an excuse for fleeing!
            yield return Toils_Reserve.Release(TargetIndex.A);
        }

        private void MakeUkuphilaZombie()
        {
            Pawn innerPawn = TargetCorpse.InnerPawn;
            MedicUtility.MakeUkuphilaZombie(innerPawn);
            UkuphilaHerb.Destroy();
        }
    }
}
