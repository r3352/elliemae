// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.ReportFieldDefs
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.AsmResolver;
using System;
using System.Diagnostics;
using System.IO;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public abstract class ReportFieldDefs : ReportFieldDefContainer
  {
    private const string className = "ReportFieldDefs";
    private static readonly string sw = Tracing.SwCommon;
    protected Sessions.Session session;

    internal abstract ReportFieldDef CreateReportFieldDef(string category, XmlElement fieldElement);

    internal abstract ReportFieldDef CreateReportFieldDef(FieldDefinition fieldDef);

    internal ReportFieldDef CreateReportFieldDef(
      Sessions.Session session,
      string category,
      XmlElement fieldElement)
    {
      return this.CreateReportFieldDef(category, fieldElement);
    }

    internal ReportFieldDef CreateReportFieldDef(Sessions.Session session, FieldDefinition fieldDef)
    {
      return this.CreateReportFieldDef(fieldDef);
    }

    public abstract string GetFieldPrefix();

    public ReportFieldDefs(Sessions.Session session)
    {
      ReportFieldClientExtension.session = this.session = session;
    }

    public ReportFieldDefs(ReportFieldDefs source)
      : base((ReportFieldDefContainer) source)
    {
      ReportFieldClientExtension.session = this.session = source.session;
    }

    protected ReportFieldDefs(Sessions.Session session, string fileName)
    {
      ReportFieldClientExtension.session = this.session = session;
      string resourceFileFullPath = AssemblyResolver.GetResourceFileFullPath(Path.Combine(SystemSettings.DocDirRelPath, fileName), SystemSettings.LocalAppDir);
      if (!File.Exists(resourceFileFullPath))
        throw new ApplicationException("'" + resourceFileFullPath + "' Report Definition file cannot be found.");
      try
      {
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.Load(resourceFileFullPath);
        foreach (XmlElement selectNode1 in xmlDocument.SelectNodes("//Catagory"))
        {
          string attribute = selectNode1.GetAttribute("desc");
          foreach (XmlElement selectNode2 in selectNode1.SelectNodes("Field"))
            this.Add(this.CreateReportFieldDef(this.session, attribute, selectNode2));
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(ReportFieldDefs.sw, nameof (ReportFieldDefs), TraceLevel.Error, ex.ToString());
      }
    }
  }
}
