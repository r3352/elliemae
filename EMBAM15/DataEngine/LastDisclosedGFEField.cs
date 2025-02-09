// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.LastDisclosedGFEField
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine.Log;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public class LastDisclosedGFEField : VirtualField
  {
    public const string FieldPrefix = "DISCLOSEDGFE.Snapshot";
    public const string FieldDescPrefix = "GFE Snapshot";
    private FieldDefinition baseField;
    private int index = 1;

    public LastDisclosedGFEField(FieldDefinition baseField)
      : base(LastDisclosedGFEField.generateVirtualFieldID(baseField.FieldID), LastDisclosedGFEField.generateVirtualFieldDesc(baseField.Description), baseField.Format, FieldInstanceSpecifierType.Index)
    {
      this.baseField = baseField;
    }

    public LastDisclosedGFEField(LastDisclosedGFEField parent, int index)
      : base((VirtualField) parent, (object) index)
    {
      this.baseField = parent.baseField;
      this.index = index;
    }

    public override VirtualFieldType VirtualFieldType => VirtualFieldType.LastDisclosedGFEFields;

    protected override FieldDefinition CreateInstanceFromSpecifier(object instanceSpecifier)
    {
      return (FieldDefinition) new LastDisclosedGFEField(this, Utils.ParseInt(instanceSpecifier, true));
    }

    private static string generateVirtualFieldID(string fieldID)
    {
      return "DISCLOSEDGFE.Snapshot." + fieldID;
    }

    private static string generateVirtualFieldDesc(string fieldDesc)
    {
      return "GFE Snapshot - " + fieldDesc;
    }

    protected override string Evaluate(LoanData loan)
    {
      DisclosureTrackingLog[] disclosureTrackingLog1 = loan.GetLogList().GetAllDisclosureTrackingLog(true, DisclosureTrackingLog.DisclosureTrackingType.GFE);
      if (disclosureTrackingLog1 == null || disclosureTrackingLog1.Length == 0 || disclosureTrackingLog1.Length < this.index)
        return "";
      DisclosureTrackingLog disclosureTrackingLog2 = disclosureTrackingLog1[disclosureTrackingLog1.Length - this.index];
      string key = this.FieldID.Replace("DISCLOSEDGFE.Snapshot.", "").Replace("." + (object) this.index, "");
      Dictionary<string, string> disclosedFields = disclosureTrackingLog2.GetDisclosedFields(new List<string>()
      {
        key
      });
      return disclosedFields != null && disclosedFields.ContainsKey(key) ? disclosedFields[key] : "";
    }

    public int Index => this.index;
  }
}
