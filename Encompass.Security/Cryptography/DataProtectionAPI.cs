// Decompiled with JetBrains decompiler
// Type: Encompass.Security.Cryptography.DataProtectionAPI
// Assembly: Encompass.Security, Version=24.3.0.5, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D0C66F5F-92EC-4221-917C-9A4B032D1E4C
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Encompass.Security.dll

using System;
using System.Security.Cryptography;

#nullable disable
namespace Encompass.Security.Cryptography
{
  public class DataProtectionAPI
  {
    public static void EncryptMemory(byte[] Buffer, MemoryProtectionScope Scope)
    {
      if (Buffer == null)
        throw new ArgumentNullException(nameof (Buffer));
      if (Buffer.Length == 0)
        throw new ArgumentException("The buffer length was 0.", nameof (Buffer));
      ProtectedMemory.Protect(Buffer, Scope);
    }

    public static void DecryptMemory(byte[] Buffer, MemoryProtectionScope Scope)
    {
      if (Buffer == null)
        throw new ArgumentNullException(nameof (Buffer));
      if (Buffer.Length == 0)
        throw new ArgumentException("The buffer length was 0.", nameof (Buffer));
      ProtectedMemory.Unprotect(Buffer, Scope);
    }

    public static byte[] EncryptStream(byte[] buffer, DataProtectionScope scope)
    {
      if (buffer == null)
        throw new ArgumentNullException(nameof (buffer));
      return buffer.Length != 0 ? ProtectedData.Protect(buffer, (byte[]) null, scope) : throw new ArgumentException("The buffer length was 0.", nameof (buffer));
    }

    public static byte[] DecryptStream(byte[] buffer, DataProtectionScope Scope)
    {
      return buffer.Length > 0 ? ProtectedData.Unprotect(buffer, (byte[]) null, Scope) : throw new ArgumentException("The given length was 0.", "length");
    }
  }
}
