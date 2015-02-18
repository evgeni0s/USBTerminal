using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USBTetminal2.Utils
{
    public static class Extensions
    {
        //http://stackoverflow.com/questions/472906/converting-a-string-to-byte-array
        public static byte[] GetBytes(this string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        public static string GetString(this byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }

        public static byte[] PackUInt12(this ushort[] input)
        {
            byte[] result = new byte[(input.Length * 3 + 1) / 2]; // the +1 leaves space if we have an odd number of UInt12s. It's the unused half byte at the end of the array.
            for (int i = 0; i < input.Length / 2; i++)
            {
                result[i * 3 + 0] = (byte)input[i * 2 + 0];
                result[i * 3 + 1] = (byte)(input[i * 2 + 0] >> 8 | input[i * 2 + 1] << 4);
                result[i * 3 + 2] = (byte)(input[i * 2 + 1] >> 4);
                if (input.Length % 2 == 1)
                {
                    result[i * 3 + 0] = (byte)input[i * 2 + 0];
                    result[i * 3 + 1] = (byte)(input[i * 2 + 0] >> 8);
                }
            }
            return result;
        }

        public static ushort[] UnpackUInt12(this byte[] input)
        {
            ushort[] result = new ushort[input.Length * 2 / 3];
            for (int i = 0; i < input.Length / 3; i++)
            {
                result[i * 2 + 0] = (ushort)(((ushort)input[i * 3 + 1]) << 8 & 0x0F00 | input[i * 3 + 0]);
                result[i * 2 + 1] = (ushort)(((ushort)input[i * 3 + 1]) << 4 | input[i * 3 + 1] >> 4);
                if (result.Length % 2 == 1)
                {
                    result[i * 2 + 0] = (ushort)(((ushort)input[i * 3 + 1]) << 8 & 0x0F00 | input[i * 3 + 0]);
                }
            }
            return result;
        }

    }
}
