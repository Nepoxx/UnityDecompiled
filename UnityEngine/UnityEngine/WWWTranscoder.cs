// Decompiled with JetBrains decompiler
// Type: UnityEngine.WWWTranscoder
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.IO;
using System.Text;

namespace UnityEngine
{
  internal class WWWTranscoder
  {
    private static byte[] ucHexChars = WWWForm.DefaultEncoding.GetBytes("0123456789ABCDEF");
    private static byte[] lcHexChars = WWWForm.DefaultEncoding.GetBytes("0123456789abcdef");
    private static byte urlEscapeChar = 37;
    private static byte[] urlSpace = new byte[1]{ (byte) 43 };
    private static byte[] dataSpace = WWWForm.DefaultEncoding.GetBytes("%20");
    private static byte[] urlForbidden = WWWForm.DefaultEncoding.GetBytes("@&;:<>=?\"'/\\!#%+$,{}|^[]`");
    private static byte qpEscapeChar = 61;
    private static byte[] qpSpace = new byte[1]{ (byte) 95 };
    private static byte[] qpForbidden = WWWForm.DefaultEncoding.GetBytes("&;=?\"'%+_");

    private static byte Hex2Byte(byte[] b, int offset)
    {
      byte num1 = 0;
      for (int index = offset; index < offset + 2; ++index)
      {
        byte num2 = (byte) ((uint) num1 * 16U);
        int num3 = (int) b[index];
        if (num3 >= 48 && num3 <= 57)
          num3 -= 48;
        else if (num3 >= 65 && num3 <= 75)
          num3 -= 55;
        else if (num3 >= 97 && num3 <= 102)
          num3 -= 87;
        if (num3 > 15)
          return 63;
        num1 = (byte) ((uint) num2 + (uint) (byte) num3);
      }
      return num1;
    }

    private static byte[] Byte2Hex(byte b, byte[] hexChars)
    {
      return new byte[2]{ hexChars[(int) b >> 4], hexChars[(int) b & 15] };
    }

    public static string URLEncode(string toEncode)
    {
      return WWWTranscoder.URLEncode(toEncode, Encoding.UTF8);
    }

    public static string URLEncode(string toEncode, Encoding e)
    {
      byte[] bytes = WWWTranscoder.Encode(e.GetBytes(toEncode), WWWTranscoder.urlEscapeChar, WWWTranscoder.urlSpace, WWWTranscoder.urlForbidden, false);
      return WWWForm.DefaultEncoding.GetString(bytes, 0, bytes.Length);
    }

    public static byte[] URLEncode(byte[] toEncode)
    {
      return WWWTranscoder.Encode(toEncode, WWWTranscoder.urlEscapeChar, WWWTranscoder.urlSpace, WWWTranscoder.urlForbidden, false);
    }

    public static string DataEncode(string toEncode)
    {
      return WWWTranscoder.DataEncode(toEncode, Encoding.UTF8);
    }

    public static string DataEncode(string toEncode, Encoding e)
    {
      byte[] bytes = WWWTranscoder.Encode(e.GetBytes(toEncode), WWWTranscoder.urlEscapeChar, WWWTranscoder.dataSpace, WWWTranscoder.urlForbidden, false);
      return WWWForm.DefaultEncoding.GetString(bytes, 0, bytes.Length);
    }

    public static byte[] DataEncode(byte[] toEncode)
    {
      return WWWTranscoder.Encode(toEncode, WWWTranscoder.urlEscapeChar, WWWTranscoder.dataSpace, WWWTranscoder.urlForbidden, false);
    }

    public static string QPEncode(string toEncode)
    {
      return WWWTranscoder.QPEncode(toEncode, Encoding.UTF8);
    }

    public static string QPEncode(string toEncode, Encoding e)
    {
      byte[] bytes = WWWTranscoder.Encode(e.GetBytes(toEncode), WWWTranscoder.qpEscapeChar, WWWTranscoder.qpSpace, WWWTranscoder.qpForbidden, true);
      return WWWForm.DefaultEncoding.GetString(bytes, 0, bytes.Length);
    }

    public static byte[] QPEncode(byte[] toEncode)
    {
      return WWWTranscoder.Encode(toEncode, WWWTranscoder.qpEscapeChar, WWWTranscoder.qpSpace, WWWTranscoder.qpForbidden, true);
    }

    public static byte[] Encode(byte[] input, byte escapeChar, byte[] space, byte[] forbidden, bool uppercase)
    {
      using (MemoryStream memoryStream = new MemoryStream(input.Length * 2))
      {
        for (int index = 0; index < input.Length; ++index)
        {
          if ((int) input[index] == 32)
            memoryStream.Write(space, 0, space.Length);
          else if ((int) input[index] < 32 || (int) input[index] > 126 || WWWTranscoder.ByteArrayContains(forbidden, input[index]))
          {
            memoryStream.WriteByte(escapeChar);
            memoryStream.Write(WWWTranscoder.Byte2Hex(input[index], !uppercase ? WWWTranscoder.lcHexChars : WWWTranscoder.ucHexChars), 0, 2);
          }
          else
            memoryStream.WriteByte(input[index]);
        }
        return memoryStream.ToArray();
      }
    }

    private static bool ByteArrayContains(byte[] array, byte b)
    {
      int length = array.Length;
      for (int index = 0; index < length; ++index)
      {
        if ((int) array[index] == (int) b)
          return true;
      }
      return false;
    }

    public static string URLDecode(string toEncode)
    {
      return WWWTranscoder.URLDecode(toEncode, Encoding.UTF8);
    }

    public static string URLDecode(string toEncode, Encoding e)
    {
      byte[] bytes = WWWTranscoder.Decode(WWWForm.DefaultEncoding.GetBytes(toEncode), WWWTranscoder.urlEscapeChar, WWWTranscoder.urlSpace);
      return e.GetString(bytes, 0, bytes.Length);
    }

    public static byte[] URLDecode(byte[] toEncode)
    {
      return WWWTranscoder.Decode(toEncode, WWWTranscoder.urlEscapeChar, WWWTranscoder.urlSpace);
    }

    public static string DataDecode(string toDecode)
    {
      return WWWTranscoder.DataDecode(toDecode, Encoding.UTF8);
    }

    public static string DataDecode(string toDecode, Encoding e)
    {
      byte[] bytes = WWWTranscoder.Decode(WWWForm.DefaultEncoding.GetBytes(toDecode), WWWTranscoder.urlEscapeChar, WWWTranscoder.dataSpace);
      return e.GetString(bytes, 0, bytes.Length);
    }

    public static byte[] DataDecode(byte[] toDecode)
    {
      return WWWTranscoder.Decode(toDecode, WWWTranscoder.urlEscapeChar, WWWTranscoder.dataSpace);
    }

    public static string QPDecode(string toEncode)
    {
      return WWWTranscoder.QPDecode(toEncode, Encoding.UTF8);
    }

    public static string QPDecode(string toEncode, Encoding e)
    {
      byte[] bytes = WWWTranscoder.Decode(WWWForm.DefaultEncoding.GetBytes(toEncode), WWWTranscoder.qpEscapeChar, WWWTranscoder.qpSpace);
      return e.GetString(bytes, 0, bytes.Length);
    }

    public static byte[] QPDecode(byte[] toEncode)
    {
      return WWWTranscoder.Decode(toEncode, WWWTranscoder.qpEscapeChar, WWWTranscoder.qpSpace);
    }

    private static bool ByteSubArrayEquals(byte[] array, int index, byte[] comperand)
    {
      if (array.Length - index < comperand.Length)
        return false;
      for (int index1 = 0; index1 < comperand.Length; ++index1)
      {
        if ((int) array[index + index1] != (int) comperand[index1])
          return false;
      }
      return true;
    }

    public static byte[] Decode(byte[] input, byte escapeChar, byte[] space)
    {
      using (MemoryStream memoryStream1 = new MemoryStream(input.Length))
      {
        for (int index = 0; index < input.Length; ++index)
        {
          if (WWWTranscoder.ByteSubArrayEquals(input, index, space))
          {
            index += space.Length - 1;
            memoryStream1.WriteByte((byte) 32);
          }
          else if ((int) input[index] == (int) escapeChar && index + 2 < input.Length)
          {
            int num1 = index + 1;
            MemoryStream memoryStream2 = memoryStream1;
            byte[] b = input;
            int offset = num1;
            index = offset + 1;
            int num2 = (int) WWWTranscoder.Hex2Byte(b, offset);
            memoryStream2.WriteByte((byte) num2);
          }
          else
            memoryStream1.WriteByte(input[index]);
        }
        return memoryStream1.ToArray();
      }
    }

    public static bool SevenBitClean(string s)
    {
      return WWWTranscoder.SevenBitClean(s, Encoding.UTF8);
    }

    public static bool SevenBitClean(string s, Encoding e)
    {
      return WWWTranscoder.SevenBitClean(e.GetBytes(s));
    }

    public static bool SevenBitClean(byte[] input)
    {
      for (int index = 0; index < input.Length; ++index)
      {
        if ((int) input[index] < 32 || (int) input[index] > 126)
          return false;
      }
      return true;
    }
  }
}
