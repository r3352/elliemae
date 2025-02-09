// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.GSECommitmentReportFieldDef
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.Common;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  public class GSECommitmentReportFieldDef : ReportFieldDef
  {
    public GSECommitmentReportFieldDef(
      string category,
      string fieldId,
      string name,
      string description,
      FieldFormat fieldType,
      string criFieldName)
      : base(category, fieldId, name, description, fieldType, criFieldName)
    {
    }

    public GSECommitmentReportFieldDef(string category, XmlElement fieldElement)
      : base(category, fieldElement)
    {
    }

    public override FilterDataSource DataSource => FilterDataSource.CorrespondentTrade;
  }
}
