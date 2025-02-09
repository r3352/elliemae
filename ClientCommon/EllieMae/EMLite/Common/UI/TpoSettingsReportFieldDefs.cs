// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.TpoSettingsReportFieldDefs
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class TpoSettingsReportFieldDefs : ReportFieldDefs
  {
    public TpoSettingsReportFieldDefs()
      : base(Session.DefaultInstance)
    {
    }

    protected TpoSettingsReportFieldDefs(string fileName)
      : base(Session.DefaultInstance, fileName)
    {
    }

    internal override ReportFieldDef CreateReportFieldDef(string category, XmlElement fieldElement)
    {
      return (ReportFieldDef) new TpoSettingsReportFieldDef(category, fieldElement);
    }

    internal override ReportFieldDef CreateReportFieldDef(FieldDefinition fieldDef)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public override string GetFieldPrefix() => throw new NotImplementedException();

    public static TpoSettingsReportFieldDefs GetFieldDefs(Sessions.Session session)
    {
      TpoSettingsReportFieldDefs fieldDefs = new TpoSettingsReportFieldDefs();
      foreach (TpoSettingsReportFieldDef fieldDef in (ReportFieldDefContainer) new TpoSettingsReportFieldDefs("TpoSettingsMap.xml"))
        fieldDefs.Add((ReportFieldDef) fieldDef);
      return fieldDefs;
    }
  }
}
