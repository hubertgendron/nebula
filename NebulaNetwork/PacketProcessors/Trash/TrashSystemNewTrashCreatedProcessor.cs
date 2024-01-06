﻿#region

using NebulaAPI.Extensions;
using NebulaAPI.GameState;
using NebulaAPI.Packets;
using NebulaModel.Networking;
using NebulaModel.Packets;
using NebulaModel.Packets.Trash;
using NebulaWorld;
using NebulaWorld.Trash;

#endregion

namespace NebulaNetwork.PacketProcessors.Trash;

[RegisterPacketProcessor]
internal class TrashSystemNewTrashCreatedProcessor : PacketProcessor<TrashSystemNewTrashCreatedPacket>
{
    public TrashSystemNewTrashCreatedProcessor()
    {
    }

    protected override void ProcessPacket(TrashSystemNewTrashCreatedPacket packet, NebulaConnection conn)
    {
        var valid = true;
        if (IsHost)
        {
            var player = Players.Connected().GetPlayer(conn);
            if (player != null)
            {
                Server.SendToAllExcept(packet, conn);
            }
            else
            {
                valid = false;
            }
        }

        if (!valid)
        {
            return;
        }
        var myId = Multiplayer.Session.World.GenerateTrashOnPlayer(packet);

        //Check if myID is same as the ID from the host
        if (myId != packet.TrashId)
        {
            TrashManager.SwitchTrashWithIds(myId, packet.TrashId);
        }
    }
}
