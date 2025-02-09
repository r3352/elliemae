// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.LastDisclosedTILField
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine.Log;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public class LastDisclosedTILField : VirtualField
  {
    public const string FieldPrefix = "DISCLOSEDTIL.Snapshot";
    public const string FieldDescPrefix = "Last Disclosed TIL Snapshot";
    private FieldDefinition baseField;
    private int index = 1;

    public LastDisclosedTILField(FieldDefinition baseField)
      : base(LastDisclosedTILField.generateVirtualFieldID(baseField.FieldID), LastDisclosedTILField.generateVirtualFieldDesc(baseField.Description), baseField.Format, FieldInstanceSpecifierType.Index)
    {
      this.baseField = baseField;
    }

    public LastDisclosedTILField(LastDisclosedTILField parent, int index)
      : base((VirtualField) parent, (object) index)
    {
      this.baseField = parent.baseField;
      this.index = index;
    }

    public override VirtualFieldType VirtualFieldType => VirtualFieldType.LastDisclosedTILFields;

    protected override FieldDefinition CreateInstanceFromSpecifier(object instanceSpecifier)
    {
      return (FieldDefinition) new LastDisclosedTILField(this, Utils.ParseInt(instanceSpecifier, true));
    }

    private static string generateVirtualFieldID(string fieldID)
    {
      return "DISCLOSEDTIL.Snapshot." + fieldID;
    }

    private static string generateVirtualFieldDesc(string fieldDesc)
    {
      return "Last Disclosed TIL Snapshot - " + fieldDesc;
    }

    protected override string Evaluate(LoanData loan)
    {
      DisclosureTrackingLog[] disclosureTrackingLog1 = loan.GetLogList().GetAllDisclosureTrackingLog(true, DisclosureTrackingLog.DisclosureTrackingType.TIL);
      if (disclosureTrackingLog1 == null || disclosureTrackingLog1.Length == 0 || disclosureTrackingLog1.Length < this.index)
        return "";
      DisclosureTrackingLog disclosureTrackingLog2 = disclosureTrackingLog1[disclosureTrackingLog1.Length - this.index];
      string key = this.FieldID.Replace("DISCLOSEDTIL.Snapshot.", "").Replace("." + (object) this.index, "");
      Dictionary<string, string> disclosedFields = disclosureTrackingLog2.GetDisclosedFields(new List<string>()
      {
        key
      });
      return disclosedFields != null && disclosedFields.ContainsKey(key) ? disclosedFields[key] : "";
    }

    public int Index => this.index;
  }
}
