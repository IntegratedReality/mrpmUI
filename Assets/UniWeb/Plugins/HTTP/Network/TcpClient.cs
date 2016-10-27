
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace HTTP.Network
{
    /// <summary>
    /// The TcpClient class provides simple methods for connecting, sending, and receiving stream data over a network in synchronous blocking mode.
    /// </summary>
    public class TcpClient : IDisposable
    {
        private readonly AutoResetEvent autoResetEvent;
        private readonly HTTP.Network.NetworkStream networkStream;
        private EndPoint endpoint;
        private bool responsePending;
        
        public TcpClient ()
            : this(AddressFamily.InterNetwork)
        {
        }
        
        public TcpClient (IPEndPoint endpoint)
            : this(AddressFamily.InterNetwork)
        {
            Connect (endpoint);
        }
        
        public TcpClient (string host, int port) : this(AddressFamily.InterNetwork)
        {
            this.Connect (host, port);
            
        }
        
        public TcpClient (AddressFamily addressFamily)
        {
            autoResetEvent = new AutoResetEvent (false);
            
            Client = new Socket (addressFamily, SocketType.Stream, ProtocolType.Tcp);
            networkStream = new NetworkStream (Client);
        }
        
        public int Available {
            get { throw new NotSupportedException (); }
        }
        
        public Socket Client { get; set; }
        
        public bool Connected {
            get { return Client != null && Client.Connected; }
        }
        
        public bool Active {
            get { return Connected; }
        }
        
        public bool ExclusiveAddressUse {
            get { return false; }
        }
        
        public bool NoDelay {
            get { return true; }
            set { throw new NotImplementedException (); }
        }
        
        #region IDisposable Members
        
        public void Dispose ()
        {
            var stream = GetStream ();
            stream.Dispose ();
            
            try {
                Client.Shutdown (SocketShutdown.Both);
                Client.Close ();
            } catch (ObjectDisposedException ex) {
                Debug.WriteLine (ex.Message);
            } catch (SocketException ex) {
                Debug.WriteLine (ex.Message);
            }
        }
        
        #endregion
        

        private void OnConnected (object sender, SocketAsyncEventArgs e)
        {
            Continue ();
        }

        public void Connect (IPEndPoint myEndpoint)
        {
            InnerConnect (myEndpoint);
        }
        
        public void Connect (IPAddress address, int port)
        {
            var myEndpoint = new IPEndPoint (address, port);
            InnerConnect (myEndpoint);
        }
   
        public void Connect (string host, int port)
        {
            var hostInfo = Dns.GetHostAddresses (host);
            var myEndpoint = new IPEndPoint (hostInfo [0], port);
            InnerConnect (myEndpoint);
        }
        
        protected void InnerConnect (EndPoint myEndpoint)
        {
            this.endpoint = myEndpoint;

            var e = new SocketAsyncEventArgs { RemoteEndPoint = this.endpoint };
            e.Completed += OnConnected;
            try {
                Client.ConnectAsync (e);
                WaitOne ();
                if(e.SocketError != SocketError.Success) throw new SocketException((int)e.SocketError);
                HandleConnectionReady ();
            } catch (SocketException ex) {
                Continue (ex);
            }
        }
        
        protected virtual void HandleConnectionReady ()
        {
            
        }
        
        public void EndConnect (IAsyncResult asyncResult)
        {
            if (!responsePending) {
                WaitOne ();
            }
        }

        private void Continue (SocketException ex)
        {
            responsePending = false;
            autoResetEvent.Set ();
            throw ex;
        }
        
        private void Continue ()
        {
            responsePending = false;
            autoResetEvent.Set ();
        }
        
        private void WaitOne ()
        {
            autoResetEvent.WaitOne ();
        }
        
        public virtual Stream GetStream ()
        {
            return networkStream;
        }
    }
}
