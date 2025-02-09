// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.LoanTemplate
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Serialization;
using System;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  [Serializable]
  public class LoanTemplate : FieldDataTemplate
  {
    private const string className = "LoanTemplate";
    private static string sw = Tracing.SwOutsideLoan;

    public LoanTemplate()
    {
    }

    public LoanTemplate(XmlSerializationInfo info)
      : base(info)
    {
    }

    public static explicit operator LoanTemplate(BinaryObject obj)
    {
      return (LoanTemplate) BinaryConvertibleObject.Parse(obj, typeof (LoanTemplate));
    }
  }
}
