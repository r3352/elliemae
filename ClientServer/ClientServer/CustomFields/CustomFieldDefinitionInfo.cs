// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.CustomFields.CustomFieldDefinitionInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer.BizLayer;
using EllieMae.EMLite.Common;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.CustomFields
{
  [Serializable]
  public class CustomFieldDefinitionInfo : BusinessInfoBase
  {
    public int FieldId;
    public int CategoryId;
    public int FieldNumber;
    public string FieldDescription = string.Empty;
    public FieldFormat FieldFormat;
    public string LoanFieldId = string.Empty;
    public bool TwoWayTransfer;
    [NotUndoable]
    public CustomFieldOptionDefinitionInfo[] CustomFieldOptionDefinitions;
    [NotUndoable]
    public bool IsNew;
    [NotUndoable]
    public bool IsDirty;
    [NotUndoable]
    public bool IsDeleted;

    public CustomFieldDefinitionInfo()
    {
    }

    public CustomFieldDefinitionInfo(
      int fieldId,
      int categoryId,
      int fieldNumber,
      string fieldDescription,
      FieldFormat fieldFormat,
      string loanFieldId,
      bool twoWayTransfer,
      CustomFieldOptionDefinitionInfo[] customFieldOptionDefinitions)
    {
      this.FieldId = fieldId;
      this.CategoryId = categoryId;
      this.FieldNumber = fieldNumber;
      this.FieldDescription = fieldDescription;
      this.FieldFormat = fieldFormat;
      this.LoanFieldId = loanFieldId;
      this.TwoWayTransfer = twoWayTransfer;
      this.CustomFieldOptionDefinitions = customFieldOptionDefinitions;
    }
  }
}
