﻿using NebulaAPI.DataStructures;
using UnityEngine;

namespace NebulaModel.Packets.Combat.GroundEnemy;

public class LaunchAssaultPacket
{
    public LaunchAssaultPacket() { }

    public LaunchAssaultPacket(in DFGBaseComponent dFGBase,
        in Vector3 tarPos, float expandRadius, int unitCount0, int unitCount1, int ap0, int ap1, int unitThreat)
    {
        PlanetId = dFGBase.groundSystem.planet.id;
        BaseId = dFGBase.id;
        EvolveThreat = dFGBase.evolve.threat;
        TarPos = tarPos.ToFloat3();
        ExpandRadius = expandRadius;
        UnitCount0 = unitCount0;
        UnitCount1 = unitCount1;
        Ap0 = ap0;
        Ap1 = ap1;
        UnitThreat = unitThreat;
    }

    public int PlanetId { get; set; }
    public int BaseId { get; set; }
    public int EvolveThreat { get; set; }
    public Float3 TarPos { get; set; }
    public float ExpandRadius { get; set; }
    public int UnitCount0 { get; set; }
    public int UnitCount1 { get; set; }
    public int Ap0 { get; set; }
    public int Ap1 { get; set; }
    public int UnitThreat { get; set; }
}
