// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.CustomFields.CustomFieldsValueInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer.BizLayer;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.CustomFields
{
  [Serializable]
  public class CustomFieldsValueInfo : BusinessInfoBase
  {
    public int ContactId;
    public int CategoryId;
    [NotUndoable]
    public CustomFieldValueInfo[] CustomFieldValues;
    [NotUndoable]
    public bool IsNew;
    [NotUndoable]
    public bool IsDirty;
    [NotUndoable]
    public bool IsDeleted;

    public CustomFieldsValueInfo()
    {
    }

    public CustomFieldsValueInfo(
      int contactId,
      int categoryId,
      CustomFieldValueInfo[] customFieldValues)
    {
      this.ContactId = contactId;
      this.CategoryId = categoryId;
      this.CustomFieldValues = customFieldValues;
    }
  }
}
