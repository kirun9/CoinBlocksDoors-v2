using HarmonyLib;

namespace kirun9.scpsl.plugins.CoinBlocksDoors.Patches
{
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
