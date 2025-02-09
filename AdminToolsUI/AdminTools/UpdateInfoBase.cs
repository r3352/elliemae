// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.AdminTools.UpdateInfoBase
// Assembly: AdminToolsUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: BCE9F231-878C-4206-826C-76CFCB8C9167
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\AdminToolsUI.dll

using System;
using System.IO;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.AdminTools
{
  public class UpdateInfoBase
  {
    public readonly string UpdateFile;
    public readonly string UpdateUrl;
    public readonly string Description;
    public readonly DateTime ReleaseDate = DateTime.MaxValue;
    public readonly string Version;
    public bool ServerRestartRequired = true;

    public UpdateInfoBase(
      string updateFile,
      string updateUrl,
      string description,
      DateTime releaseDate)
    {
      this.UpdateFile = updateFile;
      this.UpdateUrl = updateUrl;
      this.Description = description;
      this.ReleaseDate = releaseDate;
      this.Version = Path.GetFileNameWithoutExtension(updateFile);
    }

    public UpdateInfoBase(string xml)
    {
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.LoadXml(xml);
      XmlElement documentElement = xmlDocument.DocumentElement;
      this.UpdateFile = documentElement.GetAttribute("updateFile");
      this.UpdateUrl = documentElement.GetAttribute("updateUrl");
      this.Description = documentElement.GetAttribute("description");
      try
      {
        this.ReleaseDate = Convert.ToDateTime(documentElement.GetAttribute("releaseDate"));
      }
      catch
      {
      }
      this.Version = Path.GetFileNameWithoutExtension(this.UpdateFile);
    }
  }
}
