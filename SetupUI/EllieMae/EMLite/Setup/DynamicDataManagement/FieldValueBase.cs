// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.DynamicDataManagement.FieldValueBase
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

#nullable disable
namespace EllieMae.EMLite.Setup.DynamicDataManagement
{
  public abstract class FieldValueBase
  {
    public const string DISPLAY_SEPARATOR = ",";
    public const string SEPARATOR = "|";
    private string _fieldID;

    public FieldValueBase(string fieldID, DDMCriteria criteria)
    {
      this._fieldID = fieldID;
      this.Criteria = criteria;
    }

    public DDMCriteria Criteria { get; set; }

    public new abstract string ToString();

    public abstract string ToDisplayString();

    public string FieldID => this._fieldID;

    protected string GenerateDelimitedValueList(string[] values, string delimiter)
    {
      string delimitedValueList = string.Empty;
      foreach (string str in values)
        delimitedValueList = delimitedValueList + str + delimiter;
      if (!string.IsNullOrEmpty(delimitedValueList))
        delimitedValueList = delimitedValueList.Substring(0, delimitedValueList.Length - 1);
      return delimitedValueList;
    }
  }
}
