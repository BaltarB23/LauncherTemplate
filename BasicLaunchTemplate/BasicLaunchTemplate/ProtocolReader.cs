using System.IO;
using System.Text;

namespace BasicLaunchTemplate
{
    public class ProtocolReader : BinaryReader
    {
        public ProtocolReader(byte[] buffer) : base(new MemoryStream(buffer))
        {
        }
        public int ReadLength()
        {
            return this.ReadUInt16();
        }
        public override string ReadString()
        {
            int num = this.ReadLength();
            if (num > 0)
            {
                byte[] array = new byte[num];
                this.Read(array, 0, array.Length);
                Encoding utf = Encoding.UTF8;
                return utf.GetString(array);
            }
            return string.Empty;
        }
    }
}
