// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.DynamicDataManagement.CellData
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using System;

#nullable disable
namespace EllieMae.EMLite.Setup.DynamicDataManagement
{
  public class CellData : IComparable<CellData>
  {
    private static CellData _invalidCell = new CellData(-1, -1, (string) null);

    public int Row { get; private set; }

    public int Column { get; private set; }

    public string Data { get; private set; }

    public DDMCriteria Criteria { get; set; }

    public static CellData InvalidCell => CellData._invalidCell;

    public CellData(int row, int column, string data)
    {
      this.Row = row;
      this.Column = column;
      this.Data = data;
      if (data == null)
        return;
      this.ExtractCriteria();
    }

    private void ExtractCriteria()
    {
      if (this.CheckEmptyCell() || this.CheckNoValueInLoanFile() || this.CheckAdvancedCode() || this.CheckStringCriteria() || this.CheckNumericCriteria() || this.CheckRange() || this.CheckMultiValue() || this.CheckClearValueInLoanFile())
        return;
      this.Criteria = DDMCriteria.Equals;
    }

    private bool CheckEmptyCell()
    {
      if (!string.IsNullOrEmpty(this.Data.Trim()))
        return false;
      this.Criteria = DDMCriteria.IgnoreValueInLoanFile;
      this.Data = this.Data.Trim();
      return true;
    }

    private bool CheckNoValueInLoanFile()
    {
      if (!(this.Data.Trim() == "NoValueInLoanFile"))
        return false;
      this.Criteria = DDMCriteria.NoValueInLoanFile;
      this.Data = "";
      return true;
    }

    private bool CheckClearValueInLoanFile()
    {
      if (!(this.Data.Trim() == "ClearValueInLoanFile"))
        return false;
      this.Criteria = DDMCriteria.OP_ClearValueInLoanFile;
      this.Data = "";
      return true;
    }

    private bool CheckAdvancedCode()
    {
      if (!this.Data.StartsWith("Adv("))
        return false;
      this.Criteria = DDMCriteria.OP_AdvancedCoding;
      this.Data = this.Data.Remove(0, 4);
      this.Data = this.Data.Remove(this.Data.Length - 5);
      return true;
    }

    private bool CheckMultiValue()
    {
      if (this.Data.Split('|').Length <= 1)
        return false;
      this.Criteria = DDMCriteria.ListOfValues;
      return true;
    }

    private bool CheckRange()
    {
      if (this.Data.Contains("--"))
      {
        this.Data = this.Data.Remove(this.Data.IndexOf('-'), 1);
        if (this.Data.Split('-').Length == 2)
        {
          this.Criteria = DDMCriteria.Range;
          return true;
        }
      }
      return false;
    }

    private bool CheckNumericCriteria()
    {
      if (this.Data.StartsWith("<>"))
      {
        this.Criteria = DDMCriteria.NotEqual;
        this.Data = this.Data.Remove(0, 2).Trim();
        return true;
      }
      if (this.Data.StartsWith(">="))
      {
        this.Criteria = DDMCriteria.GreaterThanOrEqual;
        this.Data = this.Data.Remove(0, 2).Trim();
        return true;
      }
      if (this.Data.StartsWith("<="))
      {
        this.Criteria = DDMCriteria.LessThanOrEqual;
        this.Data = this.Data.Remove(0, 2).Trim();
        return true;
      }
      if (this.Data.StartsWith(">"))
      {
        this.Criteria = DDMCriteria.GreaterThan;
        this.Data = this.Data.Remove(0, 1).Trim();
        return true;
      }
      if (this.Data.StartsWith("<"))
      {
        this.Criteria = DDMCriteria.LessThan;
        this.Data = this.Data.Remove(0, 1).Trim();
        return true;
      }
      if (!this.Data.StartsWith("="))
        return false;
      this.Criteria = DDMCriteria.Equals;
      this.Data = this.Data.Remove(0, 1).Trim();
      return true;
    }

    private bool CheckStringCriteria()
    {
      if (this.Data.StartsWith("Equals("))
      {
        this.Criteria = DDMCriteria.strEquals;
        this.Data = this.Data.Remove(0, 7);
        this.Data = this.Data.Remove(this.Data.Length - 1).Trim();
        return true;
      }
      if (this.Data.StartsWith("DoesNotEqual("))
      {
        this.Criteria = DDMCriteria.strNotEqual;
        this.Data = this.Data.Remove(0, 13);
        this.Data = this.Data.Remove(this.Data.Length - 1).Trim();
        return true;
      }
      if (this.Data.StartsWith("NotContains("))
      {
        this.Criteria = DDMCriteria.strNotContains;
        this.Data = this.Data.Remove(0, 12);
        this.Data = this.Data.Remove(this.Data.Length - 1).Trim();
        return true;
      }
      if (this.Data.StartsWith("Contains("))
      {
        this.Criteria = DDMCriteria.strContains;
        this.Data = this.Data.Remove(0, 9);
        this.Data = this.Data.Remove(this.Data.Length - 1).Trim();
        return true;
      }
      if (this.Data.StartsWith("BeginsWith("))
      {
        this.Criteria = DDMCriteria.strBegins;
        this.Data = this.Data.Remove(0, 11);
        this.Data = this.Data.Remove(this.Data.Length - 1).Trim();
        return true;
      }
      if (!this.Data.StartsWith("EndsWith("))
        return false;
      this.Criteria = DDMCriteria.strEnds;
      this.Data = this.Data.Remove(0, 9);
      this.Data = this.Data.Remove(this.Data.Length - 1).Trim();
      return true;
    }

    public int CompareTo(CellData other)
    {
      return this.Row != other.Row && this.Column != other.Column && string.Compare(this.Data, other.Data) != 0 ? Math.Max(Math.Max(Math.Max(Math.Max(this.Row, other.Row), this.Column), other.Column), string.Compare(this.Data, other.Data)) : 0;
    }
  }
}
