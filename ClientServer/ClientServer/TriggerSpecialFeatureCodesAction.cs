// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.TriggerSpecialFeatureCodesAction
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using Elli.ElliEnum.Triggers;
using EllieMae.EMLite.Serialization;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class TriggerSpecialFeatureCodesAction : TriggerAction
  {
    private Dictionary<string, string> specialFeatureCodes = new Dictionary<string, string>();

    public TriggerSpecialFeatureCodesAction()
    {
    }

    public TriggerSpecialFeatureCodesAction(Dictionary<string, string> specialFeatureCodes)
    {
      this.specialFeatureCodes = specialFeatureCodes;
    }

    public TriggerSpecialFeatureCodesAction(XmlSerializationInfo info)
    {
      this.specialFeatureCodes = info.GetValue<Dictionary<string, string>>(nameof (SpecialFeatureCodes));
    }

    public Dictionary<string, string> SpecialFeatureCodes => this.specialFeatureCodes;

    public override TriggerActionType ActionType => TriggerActionType.AddSpecialFeatureCode;

    public override void GetXmlObjectData(XmlSerializationInfo info)
    {
      info.AddValue("SpecialFeatureCodes", (object) this.SpecialFeatureCodes);
    }
  }
}
