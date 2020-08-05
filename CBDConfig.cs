using Exiled.API.Interfaces;
using System.ComponentModel;

namespace kirun9.scpsl.plugins.CoinBlocksDoors
{
    public class CBDConfig : IConfig
    {
        [Description("Enables/Disables plugin")]
        public bool IsEnabled { get; set; } = true;
        [Description("Allows using coins on checkpoints *NOT USED AT THE MOMENT* (default true, but code changes to false)")]
        public bool AllowCheckpoint { get; set; } = false;
        [Description("If enabled plugin will broadcast messages on \"broadcast\" system rather than on hint")]
        public bool UseBroadcast { get; set; } = false;
        [Description("Indicates if blocking door should be silent (not visible until first open try)")]
        public bool SilentBlock { get; set; } = true;
        [Description("Enable blocking of scp079 interaction with blocked door")]
        public bool BlockScp079 { get; set; } = true;
        [Description("Indicates how many times (in total) doors can be blocked during one round. Set 0 to unlimited")]
        public int MaxUsesPerRound { get; set; } = 0;
        [Description("Message display time")]
        public ushort MessageDisplayTime { get; set; } = 3;
        [Description("Minimum amount of interaction needed to unlock door")]
        public int MinUses { get; set; } = 10;
        [Description("Maximum amount of interaction needed to unlock door")]
        public int MaxUses { get; set; } = 30;
        [Description("Enables automatic cleaning of unwanted objects from the door mechanism, making the system locked for a specified period of time")]
        public bool TimeLock { get; set; } = true;
        [Description("Mimimum amount of time needed to automaticaly unlock door (in seconds)")]
        public float MinTime { get; set; } = 5f;
        [Description("MAximum amount of time needed to automaticaly unlock door (in seconds)")]
        public float MaxTime { get; set; } = 25f;

        public Translations Translations { get; set; } = new Translations();
    }

    public class Translations
    {
        [Description("Message to apear when user ries to open locked door")]
        public string BlockedInfo { get; set; } = "It looks like something is stuck in the door.\nTry to move the door several times";
        [Description("Message to apear when user ries to open locked door that is locked for a specified amount of time")]
        public string BlockedTimeInfo { get; set; } = "It looks like something is stuck in the door.\nOur Facility Advanced Door Mechanism Cleaning System is cleaning door mechanism right now";
        [Description("Message to display when user can't block another door")]
        public string TooManyUses { get; set; } = "Your block was unsuccessful.\nFacility Advanced Door Mechanism Cleaning System removed coin immediately.";
    }
}
