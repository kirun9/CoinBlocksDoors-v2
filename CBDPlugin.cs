using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events;
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
        public override Version Version => new Version(2, 1, 0);
		public override Version RequiredExiledVersion => new Version(2, 0, 10);
		public override PluginPriority Priority => PluginPriority.Default;

        public static int DoorsBlocked { get; set; } = 0;

        public static IEnumerable<MEC.CoroutineHandle> Coroutines;
        public static Dictionary<int, DoorItem> Doors = new Dictionary<int, DoorItem>();
        public static Dictionary<string, int> Players = new Dictionary<string, int>();

        private Handlers.Player Player;
        private Handlers.Server Server;

        private static Harmony harmony;
        private static int HarmonyCounter;

        public static CBDConfig StaticConfig { get; private set; }

        public override void OnEnabled()
        {
            base.OnEnabled();

            Config.AllowCheckpoint = false; // Not working yet
            StaticConfig = Config;

            Player = new Handlers.Player(Config);
            Server = new Handlers.Server(Config);

            if (Config.IsEnabled)
            {
                Exiled.Events.Handlers.Player.InteractingDoor += Player.OnInteractingDoor;
                Exiled.Events.Handlers.Server.RoundStarted += Server.OnRoundStarted;
                harmony = new Harmony("kirun9.cbd." + ++HarmonyCounter);
                harmony.PatchAll();
            }
        }

        public override void OnDisabled()
        {
            base.OnDisabled();

            Exiled.Events.Handlers.Player.InteractingDoor -= Player.OnInteractingDoor;
            Exiled.Events.Handlers.Server.RoundStarted -= Server.OnRoundStarted;
            harmony?.UnpatchAll(harmony.Id);
        }
    }
}
