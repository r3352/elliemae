// Decompiled with JetBrains decompiler
// Type: Elli.Common.Security.PasswordEncryptor
// Assembly: Elli.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 5A516607-8D77-4351-85BB-54751A6A69B4
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Common.dll

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

#nullable disable
namespace Elli.Common.Security
{
  public class PasswordEncryptor
  {
    public const int DefaultHashIterations = 10000;
    public const int MaxHashIterations = 10000;
    public const int DefaultSaltSize = 32;
    public const int MaxSaltSize = 256;
    public const int DefaultHashSize = 32;
    public const int MaxHashSize = 256;

    public PasswordEncryptor(int hashIterations = 10000, int saltSize = 32, int hashSize = 32)
    {
      this.HashIterations = hashIterations;
      this.SaltSize = saltSize;
      this.HashSize = hashSize;
    }

    public int HashIterations { get; private set; }

    public int SaltSize { get; private set; }

    public int HashSize { get; private set; }

    public byte[] Hash(string password)
    {
      return this.computeHash(password, (byte[]) null, this.HashIterations, this.HashSize);
    }

    private byte[] computeHash(string password, byte[] salt, int iterations, int hashBytes)
    {
      if (iterations < 1 || iterations > 10000)
        throw new ArgumentOutOfRangeException(nameof (iterations), (object) iterations, "Number of iterations is outside of allowed range");
      byte[] bytes1 = Encoding.Default.GetBytes(password);
      byte[] hash = SHA256.Create().ComputeHash(bytes1);
      if (salt == null)
      {
        salt = new byte[this.SaltSize];
        RandomNumberGenerator.Create().GetBytes(salt);
      }
      byte[] salt1 = salt;
      int iterations1 = iterations;
      byte[] bytes2 = new Rfc2898DeriveBytes(hash, salt1, iterations1).GetBytes(hashBytes);
      return PasswordEncryptor.createHashVector(salt, bytes2, iterations);
    }

    public bool Compare(string password, byte[] hashedPassword)
    {
      byte[] hash1;
      try
      {
        byte[] salt;
        byte[] hash2;
        int iterations;
        PasswordEncryptor.parseHashVector(hashedPassword, out salt, out hash2, out iterations);
        hash1 = this.computeHash(password, salt, iterations, hash2.Length);
      }
      catch
      {
        return false;
      }
      for (int index = 0; index < hashedPassword.Length; ++index)
      {
        if ((int) hashedPassword[index] != (int) hash1[index])
          return false;
      }
      return true;
    }

    private static byte[] createHashVector(byte[] salt, byte[] hash, int iterations)
    {
      using (MemoryStream memoryStream = new MemoryStream())
      {
        byte[] bytes1 = BitConverter.GetBytes(iterations);
        memoryStream.Write(bytes1, 0, bytes1.Length);
        byte[] bytes2 = BitConverter.GetBytes(salt.Length);
        memoryStream.Write(bytes2, 0, bytes2.Length);
        memoryStream.Write(salt, 0, salt.Length);
        byte[] bytes3 = BitConverter.GetBytes(hash.Length);
        memoryStream.Write(bytes3, 0, bytes3.Length);
        memoryStream.Write(hash, 0, hash.Length);
        return memoryStream.ToArray();
      }
    }

    private static void parseHashVector(
      byte[] hashVector,
      out byte[] salt,
      out byte[] hash,
      out int iterations)
    {
      salt = (byte[]) null;
      hash = (byte[]) null;
      iterations = 0;
      try
      {
        using (MemoryStream memoryStream = new MemoryStream(hashVector))
        {
          byte[] buffer1 = new byte[4];
          memoryStream.Read(buffer1, 0, buffer1.Length);
          iterations = BitConverter.ToInt32(buffer1, 0);
          if (iterations < 1 || iterations > 10000)
            throw new Exception("Out of range iteration count detected");
          byte[] buffer2 = new byte[4];
          memoryStream.Read(buffer2, 0, buffer2.Length);
          int int32_1 = BitConverter.ToInt32(buffer2, 0);
          salt = int32_1 >= 1 && int32_1 <= 256 ? new byte[int32_1] : throw new Exception("Out of range salt size detected");
          memoryStream.Read(salt, 0, salt.Length);
          byte[] buffer3 = new byte[4];
          memoryStream.Read(buffer3, 0, buffer3.Length);
          int int32_2 = BitConverter.ToInt32(buffer3, 0);
          if (int32_2 < 1 || int32_2 > 256)
            throw new Exception("Out of range hash size detected");
          hash = new byte[BitConverter.ToInt32(buffer3, 0)];
          memoryStream.Read(hash, 0, hash.Length);
        }
      }
      catch (Exception ex)
      {
        throw new Exception("Invalid hash vector format", ex);
      }
    }
  }
}
