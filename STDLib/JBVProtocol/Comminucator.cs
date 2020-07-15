﻿using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace STDLib.JBVProtocol
{


    /// <summary>
    /// Manages connections with other devices and routes incomming and outgoing data to the right connection.
    /// </summary>
    public class Communicator
    {
        Connection connection;
        /// <summary>
        /// Our device id.
        /// </summary>
        public UInt16 ID { get; set; } = 0;

        public string Name { get; set; } = "";

        readonly Dictionary<UInt16, TaskCompletionSource<Frame>> pending = new Dictionary<ushort, TaskCompletionSource<Frame>>();

        /// <summary>
        /// A callback to handle incomming request messages.
        /// Make sure to reply something.
        /// </summary>
        public Func<object, byte[], byte[]> HandleRequestCallback = new Func<object, byte[], byte[]>((a, b) => { return Encoding.ASCII.GetBytes("This client doenst support request handling."); });

        /// <summary>
        /// A callback to handle incomming broadcast messages.
        /// </summary>
        public Action<object, byte[]> HandleBroadcastCallback;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="con">Supply the comminucator with the connection to be used</param>
        public Communicator(UInt16 id)
        {
            this.ID = id;
        }

        public void SetConnection(Connection con)
        {
            connection = con;
            connection.OnFrameReceived += Connection_OnFrameReceived;
        }

        /// <summary>
        /// Method to send a request frame
        /// </summary>
        /// <param name="RID">The ID of the receiving party.</param>
        /// <param name="payload">The data to be send</param>
        /// <param name="cts">Cancellation token to stop the async process</param>
        /// <returns>The reply of the client to witch this request was send</returns>
        public byte[] SendRequest(byte RID, byte[] payload, CancellationTokenSource cts = null)
        {
            Frame frame = new Frame();
            frame.SID = ID;
            frame.Reply = false;
            frame.Broadcast = false;
            frame.Error = false;
            frame.FID = 0;
            frame.RID = RID;
            frame.PAY = payload;

            return SendFrame(frame, cts).Result;
        }

        /// <summary>
        /// Method to send a message to all connected clients when a server is used.
        /// </summary>
        /// <param name="payload"></param>
        public void SendBroadcast(byte[] payload)
        {
            Frame frame = new Frame();
            frame.SID = ID;
            frame.Reply = false;
            frame.Broadcast = true;
            frame.Error = false;
            frame.FID = 0;
            frame.RID = 0;
            frame.PAY = payload;
            connection.SendFrame(frame);
        }


        private void Connection_OnFrameReceived(object sender, Frame e)
        {
            Connection c = sender as Connection;
            if (e.Reply)
            {
                lock (pending)
                    if(pending.ContainsKey(e.FID))
                        pending[e.FID].SetResult(e);
            }
            else
            {
                if (e.Broadcast)
                    HandleBroadcastCallback?.Invoke(this, e.PAY);
                else
                {
                    Frame reply = new Frame();
                    reply.SID = ID;
                    reply.Reply = true;
                    reply.Broadcast = false;
                    reply.Error = false;
                    reply.FID = e.FID;
                    reply.RID = e.SID;

                    if(e.RID != ID)
                    {
                        //Frame was not addressed to me.
                        reply.PAY = Encoding.ASCII.GetBytes($"Recieved frame that was not meant for '{e.RID}' but i am {ID}.");
                        reply.Error = true;

                    }
                    else
                    {
                        //Frame was addressed to me.
                        try
                        {
                            reply.PAY = HandleRequestCallback?.Invoke(this, e.PAY);
                        }
                        catch (Exception ex)
                        {
                            reply.PAY = Encoding.ASCII.GetBytes(ex.Message);
                            reply.Error = true;
                        }
                    }
                    c.SendFrame(reply);
                }
            }
        }

        private byte GetFreeFID()
        {
            byte fid = 0;
            lock (pending)
            {
                while (pending.ContainsKey(fid) && fid != 0xFF)
                    fid++;
                if (fid == 0xFF)
                    throw new Exception("No unique FID's left");
                pending[fid] = new TaskCompletionSource<Frame>();
            }
            return fid;
        }

        private async Task<byte[]> SendFrame(Frame frame, CancellationTokenSource cts = null)
        {
            frame.FID = GetFreeFID();

            cts?.Token.Register(() =>
            {
                lock (pending)
                    if (pending.ContainsKey(frame.FID))
                        pending[frame.FID].TrySetCanceled();
            });

            connection.SendFrame(frame);

            Frame reply = await pending[frame.FID].Task;

            lock (pending)
                pending.Remove(frame.FID);

            if (reply.Error)
                throw new Exception(Encoding.ASCII.GetString(reply.PAY));

            return reply.PAY;
        }


    }
}
