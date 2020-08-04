using HarmonyLib;

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
}
