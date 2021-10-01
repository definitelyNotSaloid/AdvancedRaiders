﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace AdvancedRaiders
{
    public static class RaidExtentions
    {
        public static bool OwnsAnyDrummers(this Lord lord)
        {
            foreach (var pawn in lord.ownedPawns)
            {
                if (pawn.kindDef == AdvancedRaidersDefOf.Tribal_Drummer)
                    return true;
            }

            return false;
        }
    }
}