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
                var item = ev.Player.Inventory.GetItemInHand().id;
                var doorId = ev.Door.GetInstanceID();
                if (ev.Door.PermissionLevels == Door.AccessRequirements.Checkpoints) return;

                if (item == ItemType.Coin)
                {
                    if (CBDPlugin.DoorsBlocked >= Config.MaxUsesPerRound && Config.MaxUsesPerRound != 0)
                    {
                        ev.IsAllowed = true;
                        if (Config.UseBroadcast) ev.Player.Broadcast(Config.MessageDisplayTime, Config.Translations.TooManyUses, Broadcast.BroadcastFlags.Normal);
                        else ev.Player.ShowHint(Config.Translations.TooManyUses, Config.MessageDisplayTime);
                    }
                    else
                    {
                        ev.IsAllowed = false;
                        if (Config.TimeLock)
                        {
                            CBDPlugin.Coroutines.AddItem(Timing.RunCoroutine(LockDoor(ev.Door, ev.Player), Segment.Update));
                        }
                        else
                        {
                            var random = (Config.MinUses > 0) ? (Config.MinUses < Config.MaxUses) ? UnityEngine.Random.Range(Config.MinUses, Config.MaxUses) : (Config.MinUses == Config.MaxUses) ? Config.MinUses : Config.MinUses : 1;

                            AddDoor(doorId, new DoorItem(ev.Door, random), ev.Player);

                            if (!Config.SilentBlock) ev.Door.UpdateLock();
                            ev.Player.Inventory.items.RemoveAt(ev.Player.Inventory.GetItemIndex());
                        }
                        return;
                    }
                }
                
                if (ev.Door.IsLockedByCoin())
                {
                    ev.IsAllowed = false;
                        
                    if (!Config.UseBroadcast) ev.Player.ShowHint(Config.TimeLock ? Config.Translations.BlockedTimeInfo : Config.Translations.BlockedInfo, Config.MessageDisplayTime);
                    else ev.Player.Broadcast(Config.MessageDisplayTime, Config.TimeLock ? Config.Translations.BlockedTimeInfo : Config.Translations.BlockedInfo, Broadcast.BroadcastFlags.Normal);
                        
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
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
        }

        public static void AddDoor(int doorId, DoorItem doorInfo, Exiled.API.Features.Player player)
        {
            if (CBDPlugin.Doors.ContainsKey(doorId)) CBDPlugin.Doors[doorId].MaxUses += doorInfo.MaxUses;
            else CBDPlugin.Doors.Add(doorId, doorInfo);
            CBDPlugin.DoorsBlocked++;
        }

        public IEnumerator<float> LockDoor(Door door, Exiled.API.Features.Player player)
        {
            yield return Timing.WaitForOneFrame;
            yield return Timing.WaitUntilFalse(() => door.Networklocked);

            var doorItem = new DoorItem(door, 0);
            AddDoor(door.GetInstanceID(), doorItem, player);
            
            player.Inventory.items.RemoveAt(player.Inventory.GetItemIndex());
            CBDPlugin.DoorsBlocked++;
            if (!Config.SilentBlock) door.UpdateLock();

            var random = (Config.MinTime > 0) ? (Config.MinTime < Config.MaxTime) ? UnityEngine.Random.Range(Config.MinTime, Config.MaxTime) : (Config.MinTime == Config.MaxTime) ? Config.MinTime : Config.MinTime : 5f;
            yield return Timing.WaitForSeconds(random);

            CBDPlugin.Doors.Remove(door.GetInstanceID());
            door.UpdateLock();
            
        }
    }
}
