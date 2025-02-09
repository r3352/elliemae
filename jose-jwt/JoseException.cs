// Decompiled with JetBrains decompiler
// Type: Jose.JoseException
// Assembly: jose-jwt, Version=1.8.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 80B48F01-9E85-4307-AD69-2D3CD0AE6B86
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\jose-jwt.dll

using System;

#nullable disable
namespace Jose
{
  public class JoseException : Exception
  {
    public JoseException(string message)
      : base(message)
    {
    }

    public JoseException(string message, Exception innerException)
      : base(message, innerException)
    {
    }
  }
}
