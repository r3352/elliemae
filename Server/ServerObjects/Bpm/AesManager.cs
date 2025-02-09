// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.Bpm.AesManager
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using System;
using System.IO;
using System.Security.Cryptography;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.Bpm
{
  public class AesManager
  {
    protected const string StoredKey = "eewK0+oVF9xR1dQOVdx8u2GMmGNHpGVDLTLm3VSae/g=�";
    protected const string StoredIV = "6DP4orzN3MCZAzDAx/g5lQ==�";

    public static string Encrypt(string plainText)
    {
      if (plainText == null || plainText.Length <= 0)
        throw new ArgumentNullException(nameof (plainText));
      byte[] numArray1 = Convert.FromBase64String("eewK0+oVF9xR1dQOVdx8u2GMmGNHpGVDLTLm3VSae/g=");
      byte[] numArray2 = Convert.FromBase64String("6DP4orzN3MCZAzDAx/g5lQ==");
      if (numArray1 == null || numArray1.Length == 0)
        throw new ArgumentNullException("Key");
      if (numArray2 == null || numArray2.Length == 0)
        throw new ArgumentNullException("IV");
      byte[] array;
      using (RijndaelManaged rijndaelManaged = new RijndaelManaged())
      {
        rijndaelManaged.Key = numArray1;
        rijndaelManaged.IV = numArray2;
        ICryptoTransform encryptor = rijndaelManaged.CreateEncryptor(rijndaelManaged.Key, rijndaelManaged.IV);
        using (MemoryStream memoryStream = new MemoryStream())
        {
          using (CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream, encryptor, CryptoStreamMode.Write))
          {
            using (StreamWriter streamWriter = new StreamWriter((Stream) cryptoStream))
              streamWriter.Write(plainText);
            array = memoryStream.ToArray();
          }
        }
      }
      return Convert.ToBase64String(array, 0, array.Length);
    }

    public static string Decrypt(string cipherText)
    {
      byte[] buffer = Convert.FromBase64String(cipherText);
      if (buffer == null || buffer.Length == 0)
        throw new ArgumentNullException("cipherBytes");
      byte[] numArray1 = Convert.FromBase64String("eewK0+oVF9xR1dQOVdx8u2GMmGNHpGVDLTLm3VSae/g=");
      byte[] numArray2 = Convert.FromBase64String("6DP4orzN3MCZAzDAx/g5lQ==");
      if (numArray1 == null || numArray1.Length == 0)
        throw new ArgumentNullException("Key");
      if (numArray2 == null || numArray2.Length == 0)
        throw new ArgumentNullException("IV");
      using (RijndaelManaged rijndaelManaged = new RijndaelManaged())
      {
        rijndaelManaged.Key = numArray1;
        rijndaelManaged.IV = numArray2;
        ICryptoTransform decryptor = rijndaelManaged.CreateDecryptor(rijndaelManaged.Key, rijndaelManaged.IV);
        using (MemoryStream memoryStream = new MemoryStream(buffer))
        {
          using (CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream, decryptor, CryptoStreamMode.Read))
          {
            using (StreamReader streamReader = new StreamReader((Stream) cryptoStream))
              return streamReader.ReadToEnd();
          }
        }
      }
    }
  }
}
