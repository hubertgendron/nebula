using NebulaModel.Attributes;
using System;
using System.ComponentModel;

namespace NebulaModel
{
    [System.Serializable]
    public class MultiplayerOptions : ICloneable
    {
        [DisplayName("Nickname")]
        public string Nickname { get; set; } = string.Empty;

        [DisplayName("Mecha Color Red")]
        [UIRange(0, 255, true)]
        public float MechaColorR { get; set; } = 255;

        [DisplayName("Mecha Color Green")]
        [UIRange(0, 255, true)]
        public float MechaColorG { get; set; } = 174;

        [DisplayName("Mecha Color Blue")]
        [UIRange(0, 255, true)]
        public float MechaColorB { get; set; } = 61;

        [DisplayName("Host Port")]
        [UIRange(1, ushort.MaxValue)]
        public ushort HostPort { get; set; } = 8469;

        [DisplayName("Remember Last IP")]
        public bool RememberLastIP { get; set; } = true;

        public string LastIP { get; set; } = string.Empty;

        // Networking options
        [DisplayName("Connection Timeout (Seconds)")]
        public int Timeout { get; set; } = 30;

        [DisplayName("Max Packet Size (B/K/M/G)")]
        public string MaxMessageSize { get; set; } = "50M";

        public int GetMaxMessageSizeInBytes()
        {
            int msgSizeInBytes = 0;
            char endChar = char.ToUpper(MaxMessageSize[MaxMessageSize.Length - 1]);
            int multiplier;
            switch (endChar)
            {
                case 'G':
                    multiplier = 1024 * 1024 * 1024;
                    break;
                case 'M':
                    multiplier = 1024 * 1024;
                    break;
                case 'K':
                    multiplier = 1024;
                    break;
                case 'B':
                default:
                    multiplier = 1;
                    break;
            }
            if (int.TryParse(MaxMessageSize.TrimEnd(endChar), out int result))
            {
                msgSizeInBytes = result * multiplier;
            }
            return msgSizeInBytes;
        }

        [DisplayName("Queue Limit")]
        public int QueueLimit { get; set; } = 10000;

        [DisplayName("Packets per Tick")]
        public int PacketsPerTick { get; set; } = 1000;

        // Detail function group buttons
        public bool PowerGridEnabled { get; set; } = false;
        public bool VeinDistributionEnabled { get; set; } = false;
        public bool SpaceNavigationEnabled { get; set; } = true;
        public bool BuildingWarningEnabled { get; set; } = true;
        public bool BuildingIconEnabled { get; set; } = true;
        public bool GuidingLightEnabled { get; set; } = true;

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
