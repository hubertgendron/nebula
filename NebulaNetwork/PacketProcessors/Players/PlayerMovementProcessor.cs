﻿#region

using NebulaAPI.Extensions;
using NebulaAPI.GameState;
using NebulaAPI.Packets;
using NebulaModel.Networking;
using NebulaModel.Packets;
using NebulaModel.Packets.Players;
using NebulaWorld;

#endregion

namespace NebulaNetwork.PacketProcessors.Players;

[RegisterPacketProcessor]
public class PlayerMovementProcessor : PacketProcessor<PlayerMovement>
{
    public PlayerMovementProcessor()
    {
    }

    protected override void ProcessPacket(PlayerMovement packet, NebulaConnection conn)
    {
        var valid = true;
        if (IsHost)
        {
            var player = Players.Connected().GetPlayer(conn);
            if (player != null)
            {
                player.Data.LocalPlanetId = packet.LocalPlanetId;
                player.Data.UPosition = packet.UPosition;
                player.Data.Rotation = packet.Rotation;
                player.Data.BodyRotation = packet.BodyRotation;
                player.Data.LocalPlanetPosition = packet.LocalPlanetPosition;

                Server.SendToAllExcept(packet, conn);
            }
            else
            {
                valid = false;
            }
        }

        if (valid)
        {
            Multiplayer.Session.World.UpdateRemotePlayerRealtimeState(packet);
        }
    }
}
