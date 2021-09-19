﻿using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI;

namespace AdvancedRaiders
{
    public class MentalState_UkuphilaResurrected : MentalState
    {
        public override bool ForceHostileTo(Thing t) => (t is Pawn p) && this.ForceHostileTo(p.Faction);
        public override bool ForceHostileTo(Faction f) => pawn.Faction == null ? true : pawn.Faction != f;
        public override RandomSocialMode SocialModeMax() => RandomSocialMode.Off;
    }
}