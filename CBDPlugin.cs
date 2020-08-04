using Exiled.API.Enums;
using Exiled.API.Features;
using HarmonyLib;
using System;
using System.Collections.Generic;

namespace kirun9.scpsl.plugins.CoinBlocksDoors
{
    public class CBDPlugin : Plugin<CBDConfig>
    {
        public override string Author => "kirun9";
        public override string Name => "CoinBlocksDoors";
        public override string Prefix => "CBD";
        public override Version Version => new Version(2, 0, 0, 0);
        public override PluginPriority Priority => PluginPriority.Default;

        public static IEnumerable<MEC.CoroutineHandle> Coroutines;
        public static Dictionary<int, DoorItem> Doors = new Dictionary<int, DoorItem>();

        private Handlers.Player Player;

        private static Harmony harmony;
        private static int HarmonyCounter;

        public static CBDConfig StaticConfig { get; private set; }

        public override void OnEnabled()
        {
            base.OnEnabled();

            Config.AllowCheckpoint = false; // Not working yet
            StaticConfig = Config;

            Player = new Handlers.Player(Config);

            if (Config.IsEnabled)
            {
                Exiled.Events.Handlers.Player.InteractingDoor += Player.OnInteractingDoor2;
                harmony = new Harmony("kirun9.cbd." + ++HarmonyCounter);
                harmony.PatchAll();
            }
        }
        public override void OnDisabled()
        {
            base.OnDisabled();

            Exiled.Events.Handlers.Player.InteractingDoor -= Player.OnInteractingDoor2;
            harmony?.UnpatchAll(harmony.Id);
        }
    }
}
