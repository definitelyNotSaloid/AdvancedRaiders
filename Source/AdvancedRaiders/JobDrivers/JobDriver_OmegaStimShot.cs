using System;
using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace AdvancedRaiders
{
    public class JobDriver_OmegaStimShot : JobDriver
    {
        public Pawn FirstAidTarget => TargetThingA as Pawn;
        public Thing FirstAidDrug => TargetThingB;

        private bool NoNeedInOmegaStim()
        {
            if (FirstAidTarget.health.State != PawnHealthState.Down)        
                return true;

            if (FirstAidTarget.health.hediffSet.HasHediff(AdvancedRaidersDefOf.OmegaStimulantHigh))
                return true;

            return false;

        }

        public bool FirstAidTargetIsMobile => FirstAidTarget.health.State == PawnHealthState.Mobile;
    
        protected override IEnumerable<Toil> MakeNewToils()
        {

            this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
            this.AddFailCondition(NoNeedInOmegaStim);
            this.AddFailCondition(() => (!GetActor().CanReserve(TargetA) && !GetActor().HasReserved(TargetA)));
           
            yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch);
            
            yield return Toils_Misc.TakeItemFromInventoryToCarrier(GetActor(), TargetIndex.B);
            yield return Toils_Ingest.ChewIngestible(FirstAidTarget, 1f, TargetIndex.B).FailOnCannotTouch<Toil>(TargetIndex.B, PathEndMode.Touch);

            Toil bringDownedBackToFight = new Toil();
            bringDownedBackToFight.defaultCompleteMode = ToilCompleteMode.Instant;
            bringDownedBackToFight.initAction = (Action)(() =>
            {
                if (GetActor().GetLord()!=null && !GetActor().GetLord().ownedPawns.Contains(FirstAidTarget))
                    GetActor().GetLord().AddPawn(FirstAidTarget);
            });
            yield return bringDownedBackToFight;

            yield return Toils_Ingest.FinalizeIngest(FirstAidTarget, TargetIndex.B);
          
        }

        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return pawn.Map.reservationManager.Reserve(pawn, job, job.targetA); 
        }
    }
}
