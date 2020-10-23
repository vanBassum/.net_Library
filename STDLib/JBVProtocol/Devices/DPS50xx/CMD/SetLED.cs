﻿using STDLib.JBVProtocol.Commands;
using System;

namespace STDLib.JBVProtocol.DSP50xx.CMD
{
    public class SetLED : Command
    {
        protected override bool IsBroadcast => false;
        public override UInt32 CommandID => 10;

        public bool Led { get; set; } = false;

        public override void Populate(byte[] data)
        {
            Led = data[0] > 0;
        }

        public override byte[] ToArray()
        {
            return new byte[] { (byte)(Led ? 1 : 0) };
        }
    }

}