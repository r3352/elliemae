// Decompiled with JetBrains decompiler
// Type: Encompass.Security.Cryptography.IKeyProvider
// Assembly: Encompass.Security, Version=24.3.0.5, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D0C66F5F-92EC-4221-917C-9A4B032D1E4C
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Encompass.Security.dll

#nullable disable
namespace Encompass.Security.Cryptography
{
  public interface IKeyProvider
  {
    int KeySize { get; }

    byte Identifier { get; }

    byte[] GenerateKey();

    byte[] GetDataProtectionKey();

    object GetSignatureKey();

    object GetValidationKey();
  }
}
