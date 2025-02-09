// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.DynamicDataManagement.DDMField
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.DataEngine;

#nullable disable
namespace EllieMae.EMLite.Setup.DynamicDataManagement
{
  public class DDMField
  {
    public bool IsDirty;

    protected bool Equals(DDMField other)
    {
      return string.Equals(this.FieldId, other.FieldId) && string.Equals(this.Description, other.Description) && this.ComortgagorPair == other.ComortgagorPair;
    }

    public override int GetHashCode()
    {
      return ((this.FieldId != null ? this.FieldId.GetHashCode() : 0) * 397 ^ (this.Description != null ? this.Description.GetHashCode() : 0)) * 397 ^ this.ComortgagorPair;
    }

    public string FieldId { get; set; }

    public string FieldIdWithPair
    {
      get
      {
        return this.ComortgagorPair != 1 ? FieldPairParser.GetFieldIDForBorrowerPair(this.FieldId, this.ComortgagorPair) : this.FieldId;
      }
    }

    public string Description { get; set; }

    public int ComortgagorPair { get; set; }

    public string PairText
    {
      get
      {
        string pairText = "";
        switch (this.ComortgagorPair)
        {
          case 1:
            pairText = "1st";
            break;
          case 2:
            pairText = "2nd";
            break;
          case 3:
            pairText = "3rd";
            break;
          case 4:
            pairText = "4th";
            break;
          case 5:
            pairText = "5th";
            break;
          case 6:
            pairText = "6th";
            break;
        }
        return pairText;
      }
    }

    public DDMField(LoanXDBField xdbField)
    {
      this.FieldId = xdbField.FieldID;
      this.Description = xdbField.Description;
      this.ComortgagorPair = xdbField.ComortgagorPair;
    }

    public DDMField()
    {
    }

    public DDMField(FieldPairInfo pairInfo)
    {
      this.FieldId = pairInfo.FieldID;
      this.ComortgagorPair = pairInfo.PairIndex;
    }

    public override string ToString() => this.FieldId;

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      if (this == obj)
        return true;
      return !(obj.GetType() != this.GetType()) && this.Equals((DDMField) obj);
    }
  }
}
