﻿using NebulaModel.Attributes;
using Mirror;
using NebulaModel.Packets;
using NebulaModel.Packets.Logistics;

/*
 * host ntifies clients when a ship uses a warper to enter warp state
 */
namespace NebulaNetwork.PacketProcessors.Logistics
{
    [RegisterPacketProcessor]
    class ILSShipUpdateWarperCntProcessor : PacketProcessor<ILSShipUpdateWarperCnt>
    {
        public override void ProcessPacket(ILSShipUpdateWarperCnt packet, NetworkConnection conn)
        {
            StationComponent[] gStationPool = GameMain.data.galacticTransport.stationPool;
            if (gStationPool.Length > packet.stationGId)
            {
                gStationPool[packet.stationGId].workShipDatas[packet.shipIndex].warperCnt = packet.warperCnt;
            }
        }
    }
}
