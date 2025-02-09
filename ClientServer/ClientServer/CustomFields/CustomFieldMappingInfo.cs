// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.CustomFields.CustomFieldMappingInfo
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
  public class CustomFieldMappingInfo : BusinessInfoBase
  {
    public CustomFieldsType CustomFieldsType;
    public int CategoryId;
    public int FieldNumber;
    public int RecordId;
    public FieldFormat FieldFormat;
    public string LoanFieldId = string.Empty;
    public bool TwoWayTransfer;
    [NotUndoable]
    public bool IsNew;
    [NotUndoable]
    public bool IsDirty;
    [NotUndoable]
    public bool IsDeleted;

    public CustomFieldMappingInfo()
    {
    }

    public CustomFieldMappingInfo(
      CustomFieldsType customFieldsType,
      int categoryId,
      int fieldNumber,
      int recordId,
      FieldFormat fieldFormat,
      string loanFieldId,
      bool twoWayTransfer)
    {
      this.CustomFieldsType = customFieldsType;
      this.CategoryId = categoryId;
      this.FieldNumber = fieldNumber;
      this.RecordId = recordId;
      this.FieldFormat = fieldFormat;
      this.LoanFieldId = loanFieldId;
      this.TwoWayTransfer = twoWayTransfer;
    }
  }
}
