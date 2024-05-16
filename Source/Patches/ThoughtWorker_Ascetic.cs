using HarmonyLib;
using RimWorld;
using Verse;

namespace BetterAscetics.Patches 
{
    [HarmonyPatch(typeof(ThoughtWorker_Ascetic), "CurrentStateInternal")]
    class ThoughtWorker_Ascetic_CurrentStateInternal 
    {
        static bool Prefix(ThoughtWorker_Ascetic __instance, Pawn p, ref ThoughtState __result) 
        {
            if (!p.IsColonist)
            {
                return false;
            }

            Room ownedRoom = p.ownership.OwnedRoom;

            if (ownedRoom == null)
            {
                return false;
            }

            bool roomHasAsceticPartner = false, roomHasPartner = false;

            // Check if the room is shared with a non-ascetic partner.
            foreach (Pawn o in ownedRoom.Owners)
            {
                if (p == o) continue;

                if (LovePartnerRelationUtility.LovePartnerRelationExists(p, o))
                {
                    roomHasPartner = true;
                    if (o.story.traits.HasTrait(TraitDefOf.Ascetic))
                    {
                        roomHasAsceticPartner = true;
                        break;
                    }
                }
            }

            if (!roomHasAsceticPartner && roomHasPartner) 
            {
                int scoreStageIndex = RoomStatDefOf.Impressiveness.GetScoreStageIndex(ownedRoom.GetStat(RoomStatDefOf.Impressiveness));

                // If the room isn't impressive we don't have to change anything.
                if (scoreStageIndex < 5) return false;

                /* NOTE: This might cause problems if another mod tacks a stage onto the end of the def for this thought.
                   If it does re-evaluating the way this works might be necessary. */
                __result = ThoughtState.ActiveAtStage(__instance.def.stages.Count - 1); 

                return false; // Skip original function.
            }

            return true; // Move on to original method.
        }
    }
}
