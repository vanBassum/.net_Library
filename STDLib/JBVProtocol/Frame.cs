﻿using System;
using STDLib.JBVProtocol.Commands;


namespace STDLib.JBVProtocol
{
    public class Frame
    {
        public virtual OPT Options { get; set; } = 0;
        public byte Hops { get; set; } = 0;
        public UInt16 TxID { get; set; } = 0;
        public UInt16 RxID { get; set; } = 0;
        public UInt16 Sequence { get; set; } = 0;
        public virtual UInt32 CommandID { get; set; } = (UInt32)CommandList.INVALID;
        public UInt16 DataLength { get; set; } = 0;
        public byte[] Data { get; set; }
        public int TotalLength { get { return DataLength + 2 + 4 + 2 + 2 + 2 + 1 + 1; } }

        [Flags]
        public enum OPT
        { 
            None        = 0,
            Broadcast   = 1,
        }

        int ReplaceByte(int index, int value, byte replaceByte)
        {
            return (value & ~(0xFF << index)) | (replaceByte << index);
        }

        public byte this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return (byte)Options;
                    case 1: return (byte)Hops;
                    case 2: return (byte)(TxID >> 8);
                    case 3: return (byte)TxID;
                    case 4: return (byte)(RxID >> 8);
                    case 5: return (byte)RxID;
                    case 6: return (byte)(Sequence >> 8);
                    case 7: return (byte)Sequence;
                    case 8: return (byte)(CommandID >> 24);
                    case 9: return (byte)(CommandID >> 16);
                    case 10: return (byte)(CommandID >> 8);
                    case 11: return (byte)CommandID;
                    case 12: return (byte)(DataLength >> 8);
                    case 13: return (byte)DataLength;
                    default: return Data[index - 14];
                }
            }
            set
            {
                switch (index)
                {
                    case 0: Options = (OPT)value; break;
                    case 1: Hops = value; break;
                    case 2: TxID = (UInt16)ReplaceByte(8, TxID, value); break;
                    case 3: TxID = (UInt16)ReplaceByte(0, TxID, value); break;
                    case 4: RxID = (UInt16)ReplaceByte(8, RxID, value); break;
                    case 5: RxID = (UInt16)ReplaceByte(0, RxID, value); break;
                    case 6: Sequence = (UInt16)ReplaceByte(8, Sequence, value); break;
                    case 7: Sequence = (UInt16)ReplaceByte(0, Sequence, value); break;
                    case 8: CommandID = (UInt32)ReplaceByte(24, (int)CommandID, value); break;
                    case 9: CommandID = (UInt32)ReplaceByte(16, (int)CommandID, value); break;
                    case 10: CommandID = (UInt32)ReplaceByte(08, (int)CommandID, value); break;
                    case 11: CommandID = (UInt32)ReplaceByte(00, (int)CommandID, value); break;
                    case 12: DataLength = (UInt16)ReplaceByte(8, DataLength, value); break;
                    case 13: DataLength = (UInt16)ReplaceByte(0, DataLength, value); break;
                    default:
                        if (Data == null)
                            Data = new byte[DataLength];
                        Data[index - 14] = value;
                        break;


                }
            }
        }

    }

}