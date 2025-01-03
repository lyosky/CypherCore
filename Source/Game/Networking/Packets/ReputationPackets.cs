﻿// Copyright (c) CypherCore <http://github.com/CypherCore> All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE. See LICENSE file in the project root for full license information.

using Framework.Constants;
using System.Collections.Generic;

namespace Game.Networking.Packets
{
    public class InitializeFactions : ServerPacket
    {
        public InitializeFactions() : base(ServerOpcodes.InitializeFactions, ConnectionType.Instance) { }

        public override void Write()
        {
            _worldPacket.WriteInt32(Factions.Count);
            _worldPacket.WriteInt32(Bonuses.Count);

            foreach (FactionData faction in Factions)
                faction.Write(_worldPacket);

            foreach (FactionBonusData bonus in Bonuses)
                bonus.Write(_worldPacket);
        }

        public List<FactionData> Factions = new();
        public List<FactionBonusData> Bonuses = new();
    }

    class SetFactionStanding : ServerPacket
    {
        public SetFactionStanding() : base(ServerOpcodes.SetFactionStanding, ConnectionType.Instance) { }

        public override void Write()
        {
            _worldPacket.WriteFloat(BonusFromAchievementSystem);

            _worldPacket.WriteInt32(Faction.Count);
            foreach (FactionStandingData factionStanding in Faction)
                factionStanding.Write(_worldPacket);

            _worldPacket.WriteBit(ShowVisual);
            _worldPacket.FlushBits();
        }

        public float BonusFromAchievementSystem;
        public List<FactionStandingData> Faction = new();
        public bool ShowVisual;
    }

    public struct FactionData
    {
        public uint FactionID;
        public ushort Flags;
        public int Standing;

        public void Write(WorldPacket data)
        {
            data.WriteUInt32(FactionID);
            data.WriteUInt16(Flags);
            data.WriteInt32(Standing);
        }
    }

    public struct FactionBonusData
    {
        public uint FactionID;
        public bool FactionHasBonus;

        public void Write(WorldPacket data)
        {
            data.WriteUInt32(FactionID);
            data.WriteBit(FactionHasBonus);
            data.FlushBits();
        }
    }

    struct FactionStandingData
    {
        public FactionStandingData(int index, int standing)
        {
            Index = index;
            Standing = standing;
        }

        public FactionStandingData(int index, int standing, int factionId)
        {
            Index = index;
            Standing = standing;
            FactionID = factionId;
        }

        public void Write(WorldPacket data)
        {
            data.WriteInt32(Index);
            data.WriteInt32(Standing);
            data.WriteInt32(FactionID);
        }

        int Index;
        int Standing;
        int FactionID;
    }
}
