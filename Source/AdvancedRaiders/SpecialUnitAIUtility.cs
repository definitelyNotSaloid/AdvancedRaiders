﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;
using Verse.AI;

namespace AdvancedRaiders
{
    public static class SpecialUnitAIUtility
    {
        public static bool TryFindOmegaStimShotTarget(Pawn medic, float searchRadius, out Pawn target)
        {
            if (medic.Faction == null)
            {
                Log.Warning("Tried to search for omega stim shot targets for medic with no faction");
                target = null;
                return false;
            }
            var curMap = medic.Map;

            Predicate<Thing> validator = (Predicate<Thing>)(t =>        
            {
                Pawn pawn = t as Pawn;
                return 
                pawn!=null &&
                pawn.RaceProps.Humanlike && 
                pawn.Downed && 
                pawn.Faction == medic.Faction && 
                (medic.CanReserve((LocalTargetInfo)(Thing)pawn) && 
                !pawn.health.hediffSet.HasHediff(AdvancedRaidersDefOf.OmegaStimulantHigh));
            });


            target = (Pawn) GenClosest.ClosestThingReachable(
                medic.Position, 
                curMap, 
                ThingRequest.ForGroup(ThingRequestGroup.Pawn), 
                PathEndMode.OnCell, 
                TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Some), 
                searchRadius, 
                validator);

            if (target == null)
            {
                return false;
            }

            return true;
        }

        public static bool TryFindUkuphilaHerbResurrectionTarget(Pawn medic, float searchRadius, out Corpse target)
        {
            if (medic.Faction == null)
            {
                Log.Warning("Tried to search for ukuphila resurrection targets for medic with no faction");
                target = null;
                return false;
            }
            var curMap = medic.Map;

            Predicate<Thing> validator = (Predicate<Thing>)(t =>
            {
                Corpse corpse = t as Corpse;
                return 
                corpse!=null &&
                corpse.InnerPawn.RaceProps.Humanlike &&
                corpse.InnerPawn.health.hediffSet.HasHead &&
                corpse.InnerPawn.Faction == medic.Faction &&
                (medic.CanReserve((LocalTargetInfo)(Thing)corpse) &&
                !corpse.InnerPawn.health.hediffSet.HasHediff(AdvancedRaidersDefOf.UkuphilaResurrection));
            });

            target = (Corpse) GenClosest.ClosestThingReachable(
                medic.Position,
                curMap,
                ThingRequest.ForGroup(ThingRequestGroup.Corpse),
                PathEndMode.OnCell,
                TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Some),
                searchRadius,
                validator);

            if (target == null)
                return false;

            return true;
        }


        public static bool TryFindBreakableEnemyTurret(Pawn technician, float searchRadius, out Building turret)
        {
            Predicate<Thing> validator = (t =>
            {
                var building = t as Building;
                var turretComp = building.GetComp<CompBreakdownable>();
                return
                building != null &&
                turretComp != null &&
                building.def.weaponTags.Contains("TurretGun") &&
                !turretComp.BrokenDown &&
                technician.Faction.HostileTo(building.Faction);
            }
            );

            turret = (Building) GenClosest.ClosestThingReachable(
                technician.Position,
                technician.Map,
                ThingRequest.ForGroup(ThingRequestGroup.BuildingArtificial),
                PathEndMode.OnCell,
                TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Some),
                searchRadius,
                validator);

            if (turret == null)
                return false;

            return true;
        }

        public static bool TryFindBreakableEnemyTrap(Pawn technician, float searchRadius, out Building trap)
        {
            Predicate<Thing> validator = (t =>
            {
                var building = t as Building;
                var turretComp = building.GetComp<CompBreakdownable>();
                return
                building != null &&
                turretComp != null &&
                building.def.weaponTags.Contains("TurretGun") &&
                !turretComp.BrokenDown &&
                technician.Faction.HostileTo(building.Faction);
            }
            );

            trap = null;
            return false;
        }
    }
}