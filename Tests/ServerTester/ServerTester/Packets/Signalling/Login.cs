﻿using System;
using System.Collections.Generic;
using System.Text;
using VoiceCraft.Core.Packets.Interfaces;

namespace VoiceCraft.Core.Packets.Signalling
{
    public class Login : IPacketData
    {
        public PositioningTypes PositioningType { get; set; }
        public ushort LoginKey { get; set; }
        public bool IsDeafened { get; set; }
        public bool IsMuted { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Version { get; set; } = string.Empty;

        public Login()
        {
            PositioningType = PositioningTypes.ServerSided;
            LoginKey = 0;
            Name = string.Empty;
            Version = string.Empty;
        }

        public Login(byte[] dataStream, int readOffset = 0)
        {
            PositioningType = (PositioningTypes)BitConverter.ToUInt16(dataStream, readOffset); //Read positioning type - 2 Bytes.
            LoginKey = BitConverter.ToUInt16(dataStream, readOffset + 2); //Read login key - 2 bytes.
            IsDeafened = BitConverter.ToBoolean(dataStream, readOffset + 4); //Read deafen variable - 1 byte.
            IsMuted = BitConverter.ToBoolean(dataStream, readOffset + 5); //Read muted variable - 1 byte.

            int nameLength = BitConverter.ToInt32(dataStream, readOffset + 6); //Read name length - 4 bytes.
            int versionLength = BitConverter.ToInt32(dataStream, readOffset + 10); //Read version length - 4 bytes.

            if (nameLength > 0)
                Name = Encoding.UTF8.GetString(dataStream, readOffset + 14, nameLength);
            else
                Name = string.Empty;

            if (versionLength > 0)
                Version = Encoding.UTF8.GetString(dataStream, readOffset + nameLength + 14, versionLength);
            else
                Version = string.Empty;
        }

        public byte[] GetPacketStream()
        {
            var dataStream = new List<byte>();

            dataStream.AddRange(BitConverter.GetBytes((ushort)PositioningType));
            dataStream.AddRange(BitConverter.GetBytes(LoginKey));
            dataStream.AddRange(BitConverter.GetBytes(IsDeafened));
            dataStream.AddRange(BitConverter.GetBytes(IsMuted));

            if (!string.IsNullOrWhiteSpace(Name))
                dataStream.AddRange(BitConverter.GetBytes(Name.Length));
            else
                dataStream.AddRange(BitConverter.GetBytes(0));

            if (!string.IsNullOrWhiteSpace(Version))
                dataStream.AddRange(BitConverter.GetBytes(Version.Length));
            else
                dataStream.AddRange(BitConverter.GetBytes(0));

            if (!string.IsNullOrWhiteSpace(Name))
                dataStream.AddRange(Encoding.UTF8.GetBytes(Name));

            if (!string.IsNullOrWhiteSpace(Version))
                dataStream.AddRange(Encoding.UTF8.GetBytes(Version));

            return dataStream.ToArray();
        }
    }
}
