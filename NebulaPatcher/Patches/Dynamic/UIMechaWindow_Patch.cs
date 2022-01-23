﻿using HarmonyLib;
using NebulaModel;
using NebulaModel.Packets.Players;
using NebulaWorld;

namespace NebulaPatcher.Patches.Dynamic
{
    [HarmonyPatch(typeof(UIMechaWindow))]
    internal class UIMechaWindow_Patch
    {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(UIMechaWindow._OnClose))]
        public static void _OnClose_Postfix()
        {
            if (!Multiplayer.IsActive)
            {
                return;
            }

            var appearanceArray = (GameMain.mainPlayer.mecha.diyAppearance ?? GameMain.mainPlayer.mecha.appearance).ToByte();
            Multiplayer.Session.Network.SendPacket(new PlayerAppearanceChanged(Multiplayer.Session.LocalPlayer.Id, appearanceArray));
            Config.Options.SetMechaAppearance();
        }
    }
}
