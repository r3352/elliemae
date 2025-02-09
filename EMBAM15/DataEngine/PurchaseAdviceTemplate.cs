// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.PurchaseAdviceTemplate
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Serialization;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  [Serializable]
  public class PurchaseAdviceTemplate : FieldDataTemplate
  {
    private const string className = "PurchaseAdviceTemplate";
    private static string sw = Tracing.SwOutsideLoan;
    public static readonly string[] TemplateFields;

    static PurchaseAdviceTemplate()
    {
      List<string> stringList = new List<string>();
      for (int index = 2372; index <= 2394; index += 2)
        stringList.Add(index.ToString());
      for (int index = 2596; index <= 2607; ++index)
        stringList.Add(index.ToString());
      PurchaseAdviceTemplate.TemplateFields = stringList.ToArray();
    }

    public PurchaseAdviceTemplate()
    {
    }

    public PurchaseAdviceTemplate(XmlSerializationInfo info)
      : base(info)
    {
    }

    public override string[] GetAllowedFieldIDs() => PurchaseAdviceTemplate.TemplateFields;

    public static explicit operator PurchaseAdviceTemplate(BinaryObject obj)
    {
      return (PurchaseAdviceTemplate) BinaryConvertibleObject.Parse(obj, typeof (PurchaseAdviceTemplate));
    }
  }
}
