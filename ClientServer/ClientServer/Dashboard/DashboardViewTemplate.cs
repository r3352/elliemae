// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Dashboard.DashboardViewTemplate
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Serialization;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Dashboard
{
  [Serializable]
  public class DashboardViewTemplate : BinaryConvertible<DashboardViewTemplate>, ITemplateSetting
  {
    private string guid;
    private string name;
    private string templatePath;
    public const string IsNew = "IsNewTemplate�";

    public DashboardViewTemplate()
    {
    }

    public DashboardViewTemplate(string guid, string name)
    {
      this.guid = guid;
      this.name = name;
    }

    public DashboardViewTemplate(XmlSerializationInfo info)
    {
      this.guid = info.GetString(nameof (guid));
      this.name = info.GetString(nameof (name));
    }

    public string ViewGuid
    {
      get => this.guid;
      set => this.guid = value;
    }

    public string Name
    {
      get => this.name;
      set => this.name = value;
    }

    public string TemplatePath
    {
      get => this.templatePath;
      set => this.templatePath = value;
    }

    public override string ToString() => this.Name;

    public override void GetXmlObjectData(XmlSerializationInfo info)
    {
      info.AddValue("guid", (object) this.guid);
      info.AddValue("name", (object) this.name);
    }

    string ITemplateSetting.TemplateName
    {
      get => this.name;
      set => this.name = value;
    }

    public string Description
    {
      get => "";
      set
      {
      }
    }

    public Hashtable GetProperties()
    {
      return new Hashtable((IEqualityComparer) StringComparer.CurrentCultureIgnoreCase)
      {
        {
          (object) "ViewGuid",
          (object) this.guid
        },
        {
          (object) "ViewName",
          (object) this.name
        }
      };
    }

    public ITemplateSetting Duplicate()
    {
      DashboardViewTemplate dashboardViewTemplate = (DashboardViewTemplate) this.Clone();
      dashboardViewTemplate.guid = Guid.NewGuid().ToString("D");
      dashboardViewTemplate.name = "";
      return (ITemplateSetting) dashboardViewTemplate;
    }

    public static explicit operator DashboardViewTemplate(BinaryObject binaryObject)
    {
      return BinaryConvertible<DashboardViewTemplate>.Parse(binaryObject);
    }
  }
}
