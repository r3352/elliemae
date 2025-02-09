// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.LastDisclosedLEField
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine.Log;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public class LastDisclosedLEField : VirtualField
  {
    public const string FieldPrefix = "DISCLOSEDLE.Snapshot";
    public const string FieldDescPrefix = "LE Snapshot";
    private FieldDefinition baseField;
    private int index = 1;

    public int Index => this.index;

    public LastDisclosedLEField(FieldDefinition baseField)
      : base(LastDisclosedLEField.generateVirtualFieldID(baseField.FieldID), LastDisclosedLEField.generateVirtualFieldDesc(baseField.Description), baseField.Format, FieldInstanceSpecifierType.Index)
    {
      this.baseField = baseField;
    }

    public LastDisclosedLEField(LastDisclosedLEField parent, int index)
      : base((VirtualField) parent, (object) index)
    {
      this.baseField = parent.baseField;
      this.index = index;
    }

    public override VirtualFieldType VirtualFieldType => VirtualFieldType.LastDisclosedLEFields;

    protected override FieldDefinition CreateInstanceFromSpecifier(object instanceSpecifier)
    {
      return (FieldDefinition) new LastDisclosedLEField(this, Utils.ParseInt(instanceSpecifier, true));
    }

    private static string generateVirtualFieldID(string fieldID)
    {
      return "DISCLOSEDLE.Snapshot." + fieldID;
    }

    private static string generateVirtualFieldDesc(string fieldDesc)
    {
      return "LE Snapshot - " + fieldDesc;
    }

    protected override string Evaluate(LoanData loan)
    {
      IDisclosureTracking2015Log[] idisclosureTracking2015Log = loan.GetLogList().GetAllIDisclosureTracking2015Log(true, DisclosureTracking2015Log.DisclosureTrackingType.LE);
      if (idisclosureTracking2015Log == null || idisclosureTracking2015Log.Length == 0 || idisclosureTracking2015Log.Length < this.index)
        return "";
      IDisclosureTracking2015Log disclosureTracking2015Log = idisclosureTracking2015Log[idisclosureTracking2015Log.Length - this.index];
      string key = this.FieldID.Replace("DISCLOSEDLE.Snapshot.", "").Replace("." + (object) this.index, "");
      Dictionary<string, string> disclosedFields = disclosureTracking2015Log.GetDisclosedFields(new List<string>()
      {
        key
      });
      return disclosedFields != null && disclosedFields.ContainsKey(key) ? disclosedFields[key] : "";
    }
  }
}
