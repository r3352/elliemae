// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.AUSTrackingField
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine.Log;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public class AUSTrackingField : VirtualField
  {
    public const string FieldPrefix = "AUSTRACKING";
    public const string FieldDescPrefix = "Last Snapshot";
    private FieldDefinition baseField;
    private int index = 1;

    public AUSTrackingField(FieldDefinition baseField)
      : base(AUSTrackingField.generateVirtualFieldID(baseField.FieldID), AUSTrackingField.generateVirtualFieldDesc(baseField.Description), baseField.Format, FieldInstanceSpecifierType.Index)
    {
      this.baseField = baseField;
    }

    public AUSTrackingField(AUSTrackingField parent, int index)
      : base((VirtualField) parent, (object) index)
    {
      this.baseField = parent.baseField;
      this.index = index;
    }

    public override VirtualFieldType VirtualFieldType => VirtualFieldType.AUSTrackingFields;

    protected override FieldDefinition CreateInstanceFromSpecifier(object instanceSpecifier)
    {
      return (FieldDefinition) new AUSTrackingField(this, Utils.ParseInt(instanceSpecifier, true));
    }

    private static string generateVirtualFieldID(string fieldID) => "AUSTRACKING." + fieldID;

    private static string generateVirtualFieldDesc(string fieldDesc)
    {
      return "Last Snapshot - " + fieldDesc;
    }

    protected override string Evaluate(LoanData loan)
    {
      AUSTrackingHistoryList trackingHistoryList = loan.GetAUSTrackingHistoryList();
      trackingHistoryList.Sort();
      return trackingHistoryList == null || trackingHistoryList.HistoryCount == 0 || trackingHistoryList.HistoryCount < this.index ? "" : trackingHistoryList.GetHistoryAt(this.index - 1).GetField(this.baseField.FieldID);
    }

    public int Index => this.index;

    public FieldDefinition BaseField => this.baseField;
  }
}
