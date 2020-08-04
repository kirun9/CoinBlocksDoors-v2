namespace kirun9.scpsl.plugins.CoinBlocksDoors
{
    public static class DoorExtension
    {
        public static bool IsLockedByCoin(this Door door)
        {
            return CBDPlugin.Doors.ContainsKey(door.GetInstanceID());
        }
    }
}
