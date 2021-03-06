using Verse;
using RimWorld;

namespace AdvancedRaiders
{
    public class CompAbilityEffect_Taunt : CompAbilityEffect
    {
        public new CompProperties_AbilityTaunt Props => (CompProperties_AbilityTaunt)props;
        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            if (target.Pawn!=null)
            {
                //base.Apply(target, dest);     //TODO make base apply work properly
                if (!TryTauntPawn(target.Pawn))
                    Log.Warning("Failed to taunt " + target.Pawn.Name);
            }
        }

        
        public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
        {
            Pawn pawn = target.Pawn;
            return pawn != null && pawn.MentalStateDef != AdvancedRaidersDefOf.MurderousRageTaunted;
        }

        public override bool CanApplyOn(LocalTargetInfo target, LocalTargetInfo dest) => Valid(target);

        private bool TryTauntPawn(Pawn target)
        {
            if (target.mindState.mentalStateHandler.TryStartMentalState(AdvancedRaidersDefOf.MurderousRageTaunted))
            {
                var mentalState = ((MentalState_MurderousRageTaunted)target.MentalState);
                mentalState.target = parent.pawn;
                mentalState.forceRecoverAfterTicks = Props.durationTicks;
                return true;
            }
            return false;
        }
    }

    public class CompProperties_AbilityTaunt : AbilityCompProperties
    {
        public int durationTicks=600;
        public CompProperties_AbilityTaunt() => this.compClass = typeof(CompAbilityEffect_Taunt);
    }
}
