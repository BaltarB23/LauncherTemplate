using System.IO;
using System.Text;

namespace BasicLaunchTemplate
{
    public class ProtocolWriter : BinaryWriter
    {
        private MemoryStream memoryStream;
        public ProtocolWriter() : base(new MemoryStream())
        {
            this.memoryStream = (MemoryStream)this.BaseStream;
            this.Write((byte)0);
        }

        public override void Write(string value)
        {
            Encoding utf = Encoding.UTF8;
            byte[] bytes = utf.GetBytes(value);
            this.Write((ushort)bytes.Length);
            if (bytes.Length > 0)
            {
                this.Write(bytes, 0, bytes.Length);
            }
        }

        public byte[] GetBuffer()
        {
            byte[] buffer = this.memoryStream.GetBuffer();
            this.WriteDataLength(buffer);
            return buffer;
        }

        private void WriteDataLength(byte[] data)
        {
            ushort num = (ushort)(this.memoryStream.Length - 1);
            data[0] = (byte)num;
        }
    }
}
