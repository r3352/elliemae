// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.CustomFields.CustomFieldOptionDefinitionInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer.BizLayer;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.CustomFields
{
  [Serializable]
  public class CustomFieldOptionDefinitionInfo : BusinessInfoBase
  {
    public int FieldId;
    public int OptionNumber;
    public string OptionValue = string.Empty;
    [NotUndoable]
    public bool IsNew;
    [NotUndoable]
    public bool IsDirty;
    [NotUndoable]
    public bool IsDeleted;

    public CustomFieldOptionDefinitionInfo()
    {
    }

    public CustomFieldOptionDefinitionInfo(int fieldId, int optionNumber, string optionValue)
    {
      this.FieldId = fieldId;
      this.OptionNumber = optionNumber;
      this.OptionValue = optionValue;
    }
  }
}
