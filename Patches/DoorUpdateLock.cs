using Exiled.API.Features;
using Exiled.Events;
using Exiled.Events.Handlers;
using HarmonyLib;
using System.Linq;

namespace kirun9.scpsl.plugins.CoinBlocksDoors.Patches
{
    [HarmonyPatch(typeof(Door), nameof(Door.UpdateLock))]
    public class DoorUpdateLock
    {
        public static bool Prefix(Door __instance)
        {
            __instance.Networklocked = (!__instance.PermissionLevels.HasPermission(Door.AccessRequirements.Unaccessible) &&
                (
                    __instance.commandlock ||
                    __instance.lockdown ||
                    __instance.warheadlock ||
                    __instance.decontlock ||
                    __instance.scp079Lockdown > 0f ||
                    __instance._isLockedBy079 ||
                    __instance.IsLockedByCoin()
                )
            );
            return false;
        }
    }

    [HarmonyPatch(typeof(Door), nameof(Door.ChangeState079))]
    public class DoorChangeState079
    {
        public static bool Prefix(Door __instance, ref bool __result)
        {
            if (__instance.curCooldown >= 0f || __instance.moving.moving || __instance._deniedInProgress || __instance.HardLock)
            {
                __result = false;
                return false;
            }
            if (__instance.IsLockedByCoin() && CBDPlugin.StaticConfig.BlockScp079)
            {
                __instance.UpdateLock();
                __result = false;
                return false;
            }
            __instance.moving.moving = true;
            __instance.SetState(!__instance.isOpen);
            __instance.RpcDoSound();
            __result = true;
            return false;
        }
    }
}
