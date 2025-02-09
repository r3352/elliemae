// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.LoanExternalFieldConfig
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using System;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  [Serializable]
  public class LoanExternalFieldConfig
  {
    public string FieldID;
    public string FieldTypeTable;
    public FieldFormat FieldType;
    public string Description;
    public string ColumnName;

    public LoanExternalFieldConfig()
    {
    }

    public LoanExternalFieldConfig(
      string fieldId,
      string fieldTypeTable,
      FieldFormat fieldFormat,
      string description,
      string columnName)
    {
      this.FieldID = fieldId;
      this.FieldTypeTable = fieldTypeTable;
      this.FieldType = fieldFormat;
      this.Description = description;
      this.ColumnName = columnName;
    }
  }
}
