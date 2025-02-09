// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.ContactReportFieldDef
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  public abstract class ContactReportFieldDef : ReportFieldDef
  {
    public ContactReportFieldDef(
      string category,
      string fieldId,
      string name,
      string description,
      FieldFormat fieldType,
      string criFieldName)
      : base(category, fieldId, name, description, fieldType, criFieldName)
    {
    }

    public ContactReportFieldDef(string category, FieldDefinition fieldDef)
      : base(category, fieldDef)
    {
    }

    protected ContactReportFieldDef(string category, XmlElement fieldElement)
      : base(category, fieldElement)
    {
    }
  }
}
