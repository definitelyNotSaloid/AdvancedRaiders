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
    public class CompTargetEffect_ZhivayaResurrect : CompTargetEffect
    {
        public override void DoEffectOn(Pawn user, Thing target)
        {
            if (!user.IsColonistPlayerControlled || !user.CanReserveAndReach(target, PathEndMode.Touch, Danger.Deadly))
                return;
            Job job = JobMaker.MakeJob(AdvancedRaidersDefOf.ZhivayaResurrection, target, parent);
            job.count = 1;
            user.jobs.TryTakeOrderedJob(job);
        }
    }
}
