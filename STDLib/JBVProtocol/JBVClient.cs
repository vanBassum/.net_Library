﻿using STDLib.JBVProtocol.Commands;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;



namespace STDLib.JBVProtocol
{
    public class JBVClient
    {
        public event EventHandler<Command> CommandRecieved;
        SoftwareID softwareID = SoftwareID.Unknown;
        Lease lease = new Lease();
        IConnection connection;
        Task task;
        Framing framing;
        BlockingCollection<Frame> pendingFrames = new BlockingCollection<Frame>();

        public JBVClient(SoftwareID softId)
        {
            softwareID = softId;
            framing = new Framing();
            framing.OnFrameCollected += (sender, frame) => pendingFrames.Add(frame);

            task = new Task(Work);
            task.Start();
        }

        public void SetConnection(IConnection con)
        {
            connection = con;
            con.OnDataRecieved += (sender, data) =>
            {
                framing.Unstuff(data);
            };


        }

        void Work()
        {
            AutoResetEvent leaseExpire = new AutoResetEvent(false);

            while (true)
            {
                if (lease.ExpiresIn.TotalMinutes < 5 )
                {
                    RequestLease();
                }

                Frame frame;

                while(pendingFrames.TryTake(out frame, 10000))
                {
                    Command gcmd = Command.Create(frame);
                    if (gcmd == null)
                    {
                        Logger.LOGE($"Command not found '{frame.CommandID}'");
                    }
                    else
                    {
                        switch(gcmd)
                        {
                            case RequestID cmd:
                                if (cmd.ID == lease.ID)
                                {
                                    ReplyID();
                                }
                                break;
                            case ReplyLease cmd:
                                if (cmd.Lease.Key == lease.Key)
                                {
                                    lease = cmd.Lease;
                                    Logger.LOGI($"Lease acquired");
                                }
                                break;
                            case RequestSID cmd:
                                if(cmd.SID == SoftwareID.Unknown || cmd.SID == softwareID)
                                {
                                    ReplySID(cmd);
                                }
                                break;

                            default:
                                CommandRecieved?.Invoke(this, gcmd);
                                break;
                        }
                    }
                }
            }
        }

        void RequestLease()
        {
            Logger.LOGI("RequestLease");
            RequestLease cmd = new RequestLease();
            cmd.Key = lease.Key;
            SendCMD(cmd);
        }

        void ReplyID()
        {
            ReplyID cmd = new ReplyID();
            cmd.ID = lease.ID;
            SendCMD(cmd);
        }

        void ReplySID(Command rxCmd)
        {
            ReplySID cmd = new ReplySID();
            cmd.RxID = rxCmd.TxID;
            cmd.SID = softwareID;
            SendCMD(cmd);
        }

        public void SendCMD(Command cmd)
        {
            Frame frame = cmd.GetFrame();
            SendFrame(frame);
        }

        public void SendFrame(Frame frame)
        {
            if (lease.IsValid || frame.Options.HasFlag(Frame.OPT.Broadcast))
            {
                frame.TxID = lease.ID;
                if (connection != null)
                    connection.SendData(framing.Stuff(frame));
                else
                    Logger.LOGE("No connection");
            }
        }

    }
}

