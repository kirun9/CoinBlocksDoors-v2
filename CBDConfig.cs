using Exiled.API.Interfaces;

namespace kirun9.scpsl.plugins.CoinBlocksDoors
{
    public class CBDConfig : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool AllowCheckpoint { get; set; } = false;
        public bool UseBroadcast { get; set; } = false;
        public bool SilentBlock { get; set; } = true;
        public bool BlockScp079 { get; set; } = true;
        public int MinUses { get; set; } = 10;
        public int MaxUses { get; set; } = 30;
        public bool TimeLock { get; set; } = true;
        public float MinTime { get; set; } = 5f;
        public float MaxTime { get; set; } = 25f;
        public Translations Translations { get; set; } = new Translations();
    }

    public class Translations
    {
        public string BlockedInfo { get; set; } = "It looks like something is stuck in the door.\nTry to move the door several times";
        public string BlockedTimeInfo { get; set; } = "It looks like something is stuck in the door.\nOur unlocking system is cleaning the mechanism";
    }
}
