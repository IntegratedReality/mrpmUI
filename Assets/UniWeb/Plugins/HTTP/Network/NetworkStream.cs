using System;
using System.IO;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;

namespace HTTP.Network
{
    /// <summary>
    /// A cross platform Network stream.
    /// </summary>
    public class NetworkStream : Stream
    {
        private readonly FileAccess access;
        private readonly bool ownsSocket;
        private readonly Socket socket;
        private bool isDisposed;

        public NetworkStream (Socket socket)
            : this(socket, FileAccess.ReadWrite, false)
        {
        }

        public NetworkStream (Socket socket, bool ownsSocket)
            : this(socket, FileAccess.ReadWrite, ownsSocket)
        {
        }

        public NetworkStream (Socket socket, FileAccess access)
            : this(socket, access, false)
        {
        }

        public NetworkStream (Socket socket, FileAccess access, bool ownsSocket)
        {
            this.socket = socket;
            this.access = access;
            this.ownsSocket = ownsSocket;
        }

        public override bool CanRead {
            get { return (access & FileAccess.Read) == FileAccess.Read; }
        }

        public override bool CanWrite {
            get { return (access & FileAccess.Write) == FileAccess.Write; }
        }

        public override int WriteTimeout { get; set; }

        public override int ReadTimeout { get; set; }

        public Socket Socket {
            get { return socket; }
        }

        public override bool CanSeek {
            get { return false; }
        }

        public override int Read ([In] [Out] byte[] buffer, int offset, int size)
        {
            ValidateReadArguments (buffer, offset, size);

            var e = new SocketAsyncEventArgs ();
            e.SetBuffer (buffer, offset, size);

            var readBlock = new ManualResetEvent (false);
            try {
                e.Completed += (sender, args) => readBlock.Set ();
                Socket.ReceiveAsync (e);

                readBlock.WaitOne ();

                return e.BytesTransferred;
            } catch (SocketException ex) {
                throw new IOException ("Socket error.", ex);
            }
        }

        public override void Write (byte[] buffer, int offset, int count)
        {

            ValidateWriteArguments (buffer, offset, count);

            var e = new SocketAsyncEventArgs ();
            e.SetBuffer (buffer, offset, count);

            var writeBlock = new ManualResetEvent (false);

            try {
                e.Completed += (sender, args) => writeBlock.Set ();
                Socket.SendAsync (e);

                writeBlock.WaitOne ();
            } catch (SocketException ex) {
                throw new IOException ("Socket error.", ex);
            }
        }

        public override void Close ()
        {
            base.Close ();
            if (ownsSocket && socket != null) {
                socket.Close ();
            }
        }
            
        void CheckDisposed ()
        {
            if (isDisposed)
                throw new ObjectDisposedException ("NetworkStream");
        }

        public new void Dispose ()
        {
            if (isDisposed) {
                return;
            }

            base.Dispose ();
            Close ();

            isDisposed = true;
        }

        private void ValidateWriteArguments (byte[] buffer, int offset, int size)
        {
            ValidateReadArguments (buffer, offset, size);
        }

        private void ValidateReadArguments (byte[] buffer, int offset, int size)
        {
            if (buffer == null) {
                throw new ArgumentNullException ("buffer", "Buffer must not be null.");
            }

            if (offset < 0) {
                throw new ArgumentOutOfRangeException ("offset",
                                                      "Offset must not be smaller than 0.");
            }

            if (offset > buffer.Length) {
                throw new ArgumentOutOfRangeException ("offset",
                                                      "Offset must not be larger than buffer size.");
            }

            if (size < 0) {
                throw new ArgumentOutOfRangeException ("size", "Size must not be smaller than 0.");
            }

            var overflow = (buffer.Length - offset) < size;
            if (overflow) {
                throw new ArgumentOutOfRangeException ("size",
                    "Size must not be greater than the difference between offset and length.");
            }
        }

        #region Not supported stream functionalities, like async operations

        public override long Length {
            get { throw new NotSupportedException (); }
        }

        public override long Position {
            get { throw new NotSupportedException (); }
            set { throw new NotSupportedException (); }
        }

        public override long Seek (long offset, SeekOrigin origin)
        {
            throw new NotSupportedException ();
        }

        public override IAsyncResult BeginRead (byte[] buffer, int offset, int size, AsyncCallback callback, object state)
        {
            this.CheckDisposed ();
            if (buffer == null) {
                throw new ArgumentNullException ("buffer is null");
            }
            int num = buffer.Length;
            if (offset < 0 || offset > num) {
                throw new ArgumentOutOfRangeException ("offset exceeds the size of buffer");
            }
            if (size < 0 || offset + size > num) {
                throw new ArgumentOutOfRangeException ("offset+size exceeds the size of buffer");
            }
            Socket socket = this.socket;
            if (socket == null) {
                throw new IOException ("Connection closed");
            }
            IAsyncResult result;
            try {
                result = socket.BeginReceive (buffer, offset, size, SocketFlags.None, callback, state);
            } catch (Exception innerException) {
                throw new IOException ("BeginReceive failure", innerException);
            }
            return result;
        }

        public override int EndRead (IAsyncResult ar)
        {
            this.CheckDisposed ();
            if (ar == null) {
                throw new ArgumentNullException ("async result is null");
            }
            Socket socket = this.socket;
            if (socket == null) {
                throw new IOException ("Connection closed");
            }
            int result;
            try {
                result = socket.EndReceive (ar);
            } catch (Exception innerException) {
                throw new IOException ("EndRead failure", innerException);
            }
            return result;
        }

        public override IAsyncResult BeginWrite (byte[] buffer, int offset, int size, AsyncCallback callback, object state)
        {
            this.CheckDisposed ();
            if (buffer == null) {
                throw new ArgumentNullException ("buffer is null");
            }
            int num = buffer.Length;
            if (offset < 0 || offset > num) {
                throw new ArgumentOutOfRangeException ("offset exceeds the size of buffer");
            }
            if (size < 0 || offset + size > num) {
                throw new ArgumentOutOfRangeException ("offset+size exceeds the size of buffer");
            }
            Socket socket = this.socket;
            if (socket == null) {
                throw new IOException ("Connection closed");
            }
            IAsyncResult result;
            try {
                result = socket.BeginSend (buffer, offset, size, SocketFlags.None, callback, state);
            } catch {
                throw new IOException ("BeginWrite failure");
            }
            return result;
        }

        public override void EndWrite (IAsyncResult ar)
        {
            this.CheckDisposed ();
            if (ar == null) {
                throw new ArgumentNullException ("async result is null");
            }
            Socket socket = this.socket;
            if (socket == null) {
                throw new IOException ("Connection closed");
            }
            try {
                socket.EndSend (ar);
            } catch (Exception innerException) {
                throw new IOException ("EndWrite failure", innerException);
            }
        }

        public override void SetLength (long value)
        {
            throw new NotSupportedException ();
        }

        public void Close (int timeout)
        {
            if (timeout < -1) {
                throw new ArgumentOutOfRangeException ("timeout", "Timeout must not be smaller than -1.");
            }

            throw new NotImplementedException ();
        }

        public override void Flush ()
        {
            // does nothing
        }

        #endregion

        #region Equals

        public new static bool Equals (object objA, object objB)
        {
            if (objA == null && objB == null) {
                return true;
            }

            return objA != null && objA.Equals (objB);
        }

        #endregion
    }
}
