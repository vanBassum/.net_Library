﻿using System;

namespace STDLib.JBVProtocol
{
    public enum CommandList : uint
    {

        /// <summary> Is send back to the sender when the router couln't find a path to the reciever id. </summary>
        RoutingInvalid = 0xFFFFFFF4,

        /// <summary> Send to the <see cref="LeaseServer"/> to request a <see cref="Lease"/>. </summary>
        RequestLease = 0xFFFFFFF5,

        /// <summary> Reply send by the <see cref="LeaseServer"/> with a valid <see cref="Lease"/> to use. </summary>
        ReplyLease = 0xFFFFFFF6,

        /// <summary> Send as a <see cref="Frame.OPT.Broadcast"/>, The device with matching <see cref="RequestID.ID"/> will respond with a broadcast. Used by the <see cref="Router"/> to find missing <see cref="Router.Route"/>. </summary>
        RequestID = 0xFFFFFFF7,

        /// <summary> Send as a <see cref="Frame.OPT.Broadcast"/>, The reply to <see cref="RequestID"/>. </summary>
        ReplyID = 0xFFFFFFF8,

        /// <summary> Send as a <see cref="Frame.OPT.Broadcast"/>, The device with matching <see cref="RequestSID.SID"/> will respond with a broadcast. </summary>
        RequestSID = 0xFFFFFFF9,

        /// <summary> The reply to <see cref="RequestSID"/>. </summary>
        ReplySID = 0xFFFFFFFA,

        /// <summary> A generic command that indicates that a command was not executed. </summary>
        ReplyNACK = 0xFFFFFFFB,

        /// <summary> A generic command that indicates that a command was executed sucessfully. </summary>
        ReplyACK = 0xFFFFFFFC,

        /// <summary> A generic command that indicates that the command doesn't exist. </summary>
        ReplyCMDINV = 0xFFFFFFFD,

        /// <summary> Do not use. </summary>
        INVALID = 0xFFFFFFFF,
    }

}