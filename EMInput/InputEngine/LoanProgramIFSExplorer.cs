// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.LoanProgramIFSExplorer
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.RemotingServices;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class LoanProgramIFSExplorer : IFSExplorerBase
  {
    private EllieMae.EMLite.ClientServer.TemplateSettingsType type;
    private bool noAccessCheck;
    private Sessions.Session session = Session.DefaultInstance;

    public LoanProgramIFSExplorer(EllieMae.EMLite.ClientServer.TemplateSettingsType type, Sessions.Session session)
    {
      this.type = type;
      this.session = session;
    }

    public LoanProgramIFSExplorer(
      EllieMae.EMLite.ClientServer.TemplateSettingsType type,
      bool noAccessCheck,
      Sessions.Session session)
    {
      this.type = type;
      this.noAccessCheck = noAccessCheck;
      this.session = session;
    }

    public override FileSystemEntry[] GetFileSystemEntries(FileSystemEntry parentEntry)
    {
      try
      {
        return !this.noAccessCheck ? this.session.ConfigurationManager.GetFilteredTemplateDirEntries(this.type, parentEntry) : this.session.ConfigurationManager.GetTemplateDirEntries(this.type, parentEntry);
      }
      catch (ObjectNotFoundException ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) null, "The folder cannot be found.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return (FileSystemEntry[]) null;
      }
    }

    public override string GetDescription(FileSystemEntry entry)
    {
      string description = string.Concat(entry.Properties[(object) "Description"]);
      if (description != string.Empty)
        description = description.Replace("\r\n", "");
      return description;
    }

    public override string GetRESPA(FileSystemEntry entry)
    {
      return entry.Properties.ContainsKey((object) "RESPAVERSION") && string.Concat(entry.Properties[(object) "RESPAVERSION"]) != "" ? string.Concat(entry.Properties[(object) "RESPAVERSION"]) : string.Concat(entry.Properties[(object) "For2010GFE"]);
    }
  }
}
