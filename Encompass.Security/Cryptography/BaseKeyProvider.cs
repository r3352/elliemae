// Decompiled with JetBrains decompiler
// Type: Encompass.Security.Cryptography.BaseKeyProvider
// Assembly: Encompass.Security, Version=24.3.0.5, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D0C66F5F-92EC-4221-917C-9A4B032D1E4C
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Encompass.Security.dll

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;

#nullable disable
namespace Encompass.Security.Cryptography
{
  public class BaseKeyProvider : IKeyProvider
  {
    private const byte ProviderIdentifier = 0;
    private readonly int _keySize = 128;
    private readonly int _iterations = 100;
    private readonly byte[] _key;

    public BaseKeyProvider(byte[] keyData) => this._key = this.Initialize(keyData);

    public BaseKeyProvider(SecureString key)
    {
      byte[] key1 = (byte[]) null;
      try
      {
        key1 = BaseKeyProvider.SecureStringToBytes(key);
        this._key = this.Initialize(key1);
      }
      finally
      {
        if (key1 != null)
          Array.Clear((Array) key1, 0, key1.Length);
      }
    }

    ~BaseKeyProvider()
    {
      if (this._key == null)
        return;
      Array.Clear((Array) this._key, 0, this._key.Length);
    }

    public int KeySize => this._keySize;

    public byte Identifier => 0;

    public byte[] GenerateKey() => throw new NotImplementedException();

    public byte[] GetDataProtectionKey() => this._key;

    public object GetSignatureKey() => throw new NotImplementedException();

    public object GetValidationKey() => throw new NotImplementedException();

    private byte[] Initialize(byte[] key)
    {
      using (Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(key, this.GetAncillaryKeyMaterial(), this._iterations))
        return rfc2898DeriveBytes.GetBytes(this.KeySize / 8);
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
  }
}
