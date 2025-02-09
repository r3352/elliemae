// Decompiled with JetBrains decompiler
// Type: Encompass.Security.Cryptography.DataProtection
// Assembly: Encompass.Security, Version=24.3.0.5, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D0C66F5F-92EC-4221-917C-9A4B032D1E4C
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Encompass.Security.dll

using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;

#nullable disable
namespace Encompass.Security.Cryptography
{
  public class DataProtection
  {
    private const ushort _signature = 21317;
    private const byte _version = 1;
    public static readonly int KeySize = 128;
    public static readonly int BlockSize = 128;
    public static readonly int Iterations = 100;
    private readonly byte[] _key;
    private readonly int _headerSize;
    private readonly IKeyProvider _keyProvider;

    public DataProtection(IKeyProvider keyProvider)
    {
      this._keyProvider = keyProvider;
      this._headerSize = Marshal.SizeOf(typeof (DataProtection.CryptographyInfo));
    }

    ~DataProtection()
    {
    }

    public byte[] Encrypt(byte[] plainText)
    {
      AesCryptoServiceProvider cryptoServiceProvider1 = new AesCryptoServiceProvider();
      cryptoServiceProvider1.KeySize = DataProtection.KeySize;
      cryptoServiceProvider1.BlockSize = DataProtection.BlockSize;
      cryptoServiceProvider1.Mode = CipherMode.CBC;
      cryptoServiceProvider1.Padding = PaddingMode.PKCS7;
      using (AesCryptoServiceProvider cryptoServiceProvider2 = cryptoServiceProvider1)
      {
        cryptoServiceProvider2.GenerateIV();
        using (ICryptoTransform encryptor = cryptoServiceProvider2.CreateEncryptor(this._keyProvider.GetDataProtectionKey(), cryptoServiceProvider2.IV))
        {
          using (MemoryStream memoryStream = new MemoryStream())
          {
            byte[] byteArray = new DataProtection.CryptographyInfo((byte) cryptoServiceProvider2.IV.Length, this._keyProvider).ToByteArray();
            memoryStream.Write(byteArray, 0, byteArray.Length);
            memoryStream.Write(cryptoServiceProvider2.IV, 0, cryptoServiceProvider2.IV.Length);
            using (CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream, encryptor, CryptoStreamMode.Write))
            {
              cryptoStream.Write(plainText, 0, plainText.Length);
              cryptoStream.FlushFinalBlock();
              return memoryStream.ToArray();
            }
          }
        }
      }
    }

    public void Encrypt(Stream source, Stream outStream)
    {
      AesCryptoServiceProvider cryptoServiceProvider1 = new AesCryptoServiceProvider();
      cryptoServiceProvider1.KeySize = DataProtection.KeySize;
      cryptoServiceProvider1.BlockSize = DataProtection.BlockSize;
      cryptoServiceProvider1.Mode = CipherMode.CBC;
      cryptoServiceProvider1.Padding = PaddingMode.PKCS7;
      using (AesCryptoServiceProvider cryptoServiceProvider2 = cryptoServiceProvider1)
      {
        cryptoServiceProvider2.GenerateIV();
        using (ICryptoTransform encryptor = cryptoServiceProvider2.CreateEncryptor(this._keyProvider.GetDataProtectionKey(), cryptoServiceProvider2.IV))
        {
          byte[] byteArray = new DataProtection.CryptographyInfo((byte) cryptoServiceProvider2.IV.Length, this._keyProvider).ToByteArray();
          outStream.Write(byteArray, 0, byteArray.Length);
          outStream.Write(cryptoServiceProvider2.IV, 0, cryptoServiceProvider2.IV.Length);
          using (CryptoStream dest = new CryptoStream(outStream, encryptor, CryptoStreamMode.Write))
          {
            source.CopyTo((Stream) dest, source.Length);
            dest.FlushFinalBlock();
          }
        }
      }
    }

    public void Encrypt(
      Stream source,
      Stream outStream,
      ProgressState userState,
      IProgressNotify notify)
    {
      AesCryptoServiceProvider cryptoServiceProvider1 = new AesCryptoServiceProvider();
      cryptoServiceProvider1.KeySize = DataProtection.KeySize;
      cryptoServiceProvider1.BlockSize = DataProtection.BlockSize;
      cryptoServiceProvider1.Mode = CipherMode.CBC;
      cryptoServiceProvider1.Padding = PaddingMode.PKCS7;
      using (AesCryptoServiceProvider cryptoServiceProvider2 = cryptoServiceProvider1)
      {
        cryptoServiceProvider2.GenerateIV();
        using (ICryptoTransform encryptor = cryptoServiceProvider2.CreateEncryptor(this._keyProvider.GetDataProtectionKey(), cryptoServiceProvider2.IV))
        {
          byte[] byteArray = new DataProtection.CryptographyInfo((byte) cryptoServiceProvider2.IV.Length, this._keyProvider).ToByteArray();
          outStream.Write(byteArray, 0, byteArray.Length);
          outStream.Write(cryptoServiceProvider2.IV, 0, cryptoServiceProvider2.IV.Length);
          using (CryptoStream dest = new CryptoStream(outStream, encryptor, CryptoStreamMode.Write))
          {
            source.CopyTo((Stream) dest, source.Length, userState, notify);
            dest.FlushFinalBlock();
          }
        }
      }
    }

    public byte[] Decrypt(byte[] cipherText)
    {
      AesCryptoServiceProvider cryptoServiceProvider1 = new AesCryptoServiceProvider();
      cryptoServiceProvider1.KeySize = DataProtection.KeySize;
      cryptoServiceProvider1.BlockSize = DataProtection.BlockSize;
      cryptoServiceProvider1.Mode = CipherMode.CBC;
      cryptoServiceProvider1.Padding = PaddingMode.PKCS7;
      using (AesCryptoServiceProvider cryptoServiceProvider2 = cryptoServiceProvider1)
      {
        int sourceIndex = this._headerSize;
        if (cipherText[0] != (byte) 69 || cipherText[1] != (byte) 83)
          sourceIndex = 0;
        byte[] numArray = new byte[DataProtection.KeySize / 8];
        Array.Copy((Array) cipherText, sourceIndex, (Array) numArray, 0, numArray.Length);
        using (ICryptoTransform decryptor = cryptoServiceProvider2.CreateDecryptor(this._keyProvider.GetDataProtectionKey(), numArray))
        {
          using (MemoryStream memoryStream = new MemoryStream())
          {
            using (CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream, decryptor, CryptoStreamMode.Write))
            {
              cryptoStream.Write(cipherText, numArray.Length + sourceIndex, cipherText.Length - (numArray.Length + sourceIndex));
              cryptoStream.FlushFinalBlock();
              return memoryStream.ToArray();
            }
          }
        }
      }
    }

    public void Decrypt(Stream source, Stream outStream)
    {
      AesCryptoServiceProvider cryptoServiceProvider1 = new AesCryptoServiceProvider();
      cryptoServiceProvider1.KeySize = DataProtection.KeySize;
      cryptoServiceProvider1.BlockSize = DataProtection.BlockSize;
      cryptoServiceProvider1.Mode = CipherMode.CBC;
      cryptoServiceProvider1.Padding = PaddingMode.PKCS7;
      using (AesCryptoServiceProvider cryptoServiceProvider2 = cryptoServiceProvider1)
      {
        byte[] numArray1 = new byte[this._headerSize];
        source.Read(numArray1, 0, this._headerSize);
        if (!DataProtection.CryptographyInfo.FromByteArray(numArray1).IsValidSignature())
          source.Position = 0L;
        byte[] numArray2 = new byte[DataProtection.KeySize / 8];
        source.Read(numArray2, 0, numArray2.Length);
        using (ICryptoTransform decryptor = cryptoServiceProvider2.CreateDecryptor(this._keyProvider.GetDataProtectionKey(), numArray2))
        {
          CryptoStream destination = new CryptoStream(outStream, decryptor, CryptoStreamMode.Write);
          source.CopyTo((Stream) destination);
          destination.FlushFinalBlock();
        }
      }
    }

    public void Decrypt(
      Stream source,
      Stream outStream,
      ProgressState userState,
      IProgressNotify notify)
    {
      AesCryptoServiceProvider cryptoServiceProvider1 = new AesCryptoServiceProvider();
      cryptoServiceProvider1.KeySize = DataProtection.KeySize;
      cryptoServiceProvider1.BlockSize = DataProtection.BlockSize;
      cryptoServiceProvider1.Mode = CipherMode.CBC;
      cryptoServiceProvider1.Padding = PaddingMode.PKCS7;
      using (AesCryptoServiceProvider cryptoServiceProvider2 = cryptoServiceProvider1)
      {
        byte[] numArray1 = new byte[this._headerSize];
        source.Read(numArray1, 0, this._headerSize);
        if (!DataProtection.CryptographyInfo.FromByteArray(numArray1).IsValidSignature())
          source.Position = 0L;
        byte[] numArray2 = new byte[DataProtection.KeySize / 8];
        source.Read(numArray2, 0, numArray2.Length);
        using (ICryptoTransform decryptor = cryptoServiceProvider2.CreateDecryptor(this._keyProvider.GetDataProtectionKey(), numArray2))
        {
          CryptoStream dest = new CryptoStream(outStream, decryptor, CryptoStreamMode.Write);
          source.CopyTo((Stream) dest, source.Length, userState, notify);
          dest.FlushFinalBlock();
        }
      }
    }

    public static DataProtection.CryptographyInfo GetCryptographyInfo(byte[] cipherText)
    {
      return DataProtection.CryptographyInfo.FromByteArray(cipherText);
    }

    public static bool CanDecrypt(byte[] cipherText)
    {
      return cipherText[0] == (byte) 69 && cipherText[1] == (byte) 83;
    }

    public static byte GetProviderIdentifier(byte[] cipherText)
    {
      return !DataProtection.CanDecrypt(cipherText) ? new BaseKeyProvider((byte[]) null).Identifier : DataProtection.GetCryptographyInfo(cipherText).Reserved;
    }

    public static bool CanDecrypt(Stream cipherText)
    {
      byte[] buffer = new byte[2];
      try
      {
        cipherText.Read(buffer, 0, 2);
        cipherText.Position -= 2L;
        return buffer[0] == (byte) 69 && buffer[1] == (byte) 83;
      }
      catch
      {
        return false;
      }
    }

    private static byte[] SecureStringToBytes(SecureString secureString)
    {
      IntPtr bstr = Marshal.SecureStringToBSTR(secureString);
      try
      {
        int length = Marshal.ReadInt32(bstr, -4);
        byte[] bytes = new byte[length];
        GCHandle gcHandle = GCHandle.Alloc((object) bytes, GCHandleType.Pinned);
        try
        {
          for (int ofs = 0; ofs < length; ++ofs)
            bytes[ofs] = Marshal.ReadByte(bstr, ofs);
          return bytes;
        }
        finally
        {
          Array.Clear((Array) bytes, 0, bytes.Length);
          gcHandle.Free();
        }
      }
      finally
      {
        Marshal.ZeroFreeBSTR(bstr);
      }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private byte[] GetAncillaryKeyMaterial()
    {
      return new byte[16]
      {
        (byte) 116,
        (byte) 179,
        (byte) 71,
        (byte) 204,
        (byte) 117,
        (byte) 213,
        (byte) 22,
        (byte) 116,
        (byte) 98,
        (byte) 40,
        (byte) 30,
        (byte) 199,
        (byte) 201,
        (byte) 163,
        (byte) 15,
        (byte) 217
      };
    }

    public struct CryptographyInfo
    {
      public ushort Signature;
      public byte Algorithm;
      public byte Version;
      public byte IVSize;
      public byte Reserved;

      public CryptographyInfo(byte ivSize, IKeyProvider keyProv = null)
      {
        this.Signature = (ushort) 21317;
        this.Algorithm = (byte) 2;
        this.Version = (byte) 1;
        this.IVSize = ivSize;
        this.Reserved = (byte) 0;
        if (keyProv == null)
          return;
        this.Reserved = keyProv.Identifier;
      }

      public byte[] ToByteArray()
      {
        IntPtr num = IntPtr.Zero;
        int length = Marshal.SizeOf<DataProtection.CryptographyInfo>(this);
        byte[] destination = new byte[length];
        try
        {
          num = Marshal.AllocHGlobal(length);
          Marshal.StructureToPtr<DataProtection.CryptographyInfo>(this, num, true);
          Marshal.Copy(num, destination, 0, length);
        }
        finally
        {
          if (num != IntPtr.Zero)
            Marshal.FreeHGlobal(num);
        }
        return destination;
      }

      public bool IsValidSignature() => this.Signature == (ushort) 21317;

      public static DataProtection.CryptographyInfo FromByteArray(byte[] data)
      {
        IntPtr num1 = IntPtr.Zero;
        int num2 = Marshal.SizeOf(typeof (DataProtection.CryptographyInfo));
        try
        {
          num1 = Marshal.AllocHGlobal(num2);
          Marshal.Copy(data, 0, num1, num2);
          return (DataProtection.CryptographyInfo) Marshal.PtrToStructure(num1, typeof (DataProtection.CryptographyInfo));
        }
        finally
        {
          if (num1 != IntPtr.Zero)
            Marshal.FreeHGlobal(num1);
        }
      }
    }
  }
}
