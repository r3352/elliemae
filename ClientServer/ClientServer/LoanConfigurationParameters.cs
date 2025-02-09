// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.LoanConfigurationParameters
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class LoanConfigurationParameters
  {
    public DateTime FieldRulesModificationTime = DateTime.MinValue;
    public DateTime TriggersModificationTime = DateTime.MinValue;
    public DateTime PrintSelectionModificationTime = DateTime.MinValue;
    public DateTime DDMLastModifiedDateTime = DateTime.MinValue;
    public DateTime CustomFieldsModificationTime = DateTime.MinValue;
    public DateTime RolesModificationTime = DateTime.MinValue;

    public LoanConfigurationParameters Clone()
    {
      return new LoanConfigurationParameters()
      {
        FieldRulesModificationTime = this.FieldRulesModificationTime,
        TriggersModificationTime = this.TriggersModificationTime,
        PrintSelectionModificationTime = this.PrintSelectionModificationTime,
        DDMLastModifiedDateTime = this.DDMLastModifiedDateTime,
        CustomFieldsModificationTime = this.CustomFieldsModificationTime,
        RolesModificationTime = this.RolesModificationTime
      };
    }
  }
}
