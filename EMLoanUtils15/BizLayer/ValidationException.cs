// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.BizLayer.ValidationException
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using System;
using System.Runtime.Serialization;

#nullable disable
namespace EllieMae.EMLite.BizLayer
{
  [Serializable]
  public class ValidationException : ApplicationException
  {
    public ValidationException()
    {
    }

    public ValidationException(string message)
      : base(message)
    {
    }

    public ValidationException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    protected ValidationException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
