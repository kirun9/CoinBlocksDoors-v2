using Exiled.API.Features;
using Exiled.Events.EventArgs;
using MEC;
using HarmonyLib;
using System;
using System.Collections.Generic;

namespace kirun9.scpsl.plugins.CoinBlocksDoors.Handlers
{
    public class Player
    {
        public static CBDConfig Config { get; internal set; }

        public Player(CBDConfig config)
        {
            Config = config;
        }

        public void OnInteractingDoor(InteractingDoorEventArgs ev)
        {
            try
            {
                var inv = ev.Player.Inventory;
                var item = inv.GetItemInHand().id;
                var doorId = ev.Door.GetInstanceID();
                if (ev.Door.PermissionLevels == Door.AccessRequirements.Checkpoints) return;

                if (item == ItemType.Coin)
                {
                    ev.IsAllowed = false;
                    if (Config.TimeLock)
                    {
                        CBDPlugin.Coroutines.AddItem(Timing.RunCoroutine(LockDoor(ev.Door), Segment.Update));
                    }
                    else
                    {
                        var random = (Config.MinUses > 0) ? (Config.MinUses < Config.MaxUses) ? UnityEngine.Random.Range(Config.MinUses, Config.MaxUses) : (Config.MinUses == Config.MaxUses) ? Config.MinUses : Config.MinUses : 1;
                        
                        if (CBDPlugin.Doors.ContainsKey(doorId)) CBDPlugin.Doors[doorId].MaxUses += random;
                        else CBDPlugin.Doors.Add(doorId, new DoorItem(ev.Door, random));

                        if (!Config.SilentBlock) ev.Door.UpdateLock();
                    }
                    inv.items.RemoveAt(inv.GetItemIndex());
                }
                else
                {
                    if (ev.Door.IsLockedByCoin())
                    {
                        ev.IsAllowed = false;
                        
                        if (!Config.UseBroadcast) ev.Player.ShowHint(Config.TimeLock ? Config.Translations.BlockedTimeInfo : Config.Translations.BlockedInfo, 3);
                        else ev.Player.Broadcast(3, Config.TimeLock ? Config.Translations.BlockedTimeInfo : Config.Translations.BlockedInfo, Broadcast.BroadcastFlags.Normal);
                        
                        if (!Config.TimeLock)
                        {
                            var door = CBDPlugin.Doors[doorId];
                            door.Used++;
                            if (door.Used >= door.MaxUses)
                            {
                                CBDPlugin.Doors.Remove(doorId);
                            }
                            else
                            {
                                CBDPlugin.Doors[doorId] = door;
                            }
                        }
                        ev.Door.UpdateLock();
                    }
                }

            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
        }

        public IEnumerator<float> LockDoor(Door door)
        {
            yield return Timing.WaitForOneFrame;
            yield return Timing.WaitUntilFalse(() => door.Networklocked);

            var doorItem = new DoorItem(door, 0);
            CBDPlugin.Doors.Add(door.GetInstanceID(), doorItem);
            if (!Config.SilentBlock) door.UpdateLock();
            
            var random = (Config.MinTime > 0) ? (Config.MinTime < Config.MaxTime) ? UnityEngine.Random.Range(Config.MinTime, Config.MaxTime) : (Config.MinTime == Config.MaxTime) ? Config.MinTime : Config.MinTime : 5f;
            yield return Timing.WaitForSeconds(random);

            CBDPlugin.Doors.Remove(door.GetInstanceID());
            door.UpdateLock();
        }
    }
}
