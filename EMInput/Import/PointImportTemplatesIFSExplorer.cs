// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Import.PointImportTemplatesIFSExplorer
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.RemotingServices;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Import
{
  public class PointImportTemplatesIFSExplorer : IFSExplorerBase
  {
    private EllieMae.EMLite.ClientServer.TemplateSettingsType tpType;
    private string templateType = string.Empty;

    public PointImportTemplatesIFSExplorer(string type)
    {
      this.templateType = type;
      switch (type)
      {
        case "CC":
          this.tpType = EllieMae.EMLite.ClientServer.TemplateSettingsType.ClosingCost;
          break;
        case "LP":
          this.tpType = EllieMae.EMLite.ClientServer.TemplateSettingsType.LoanProgram;
          break;
        case "CF":
          this.tpType = EllieMae.EMLite.ClientServer.TemplateSettingsType.CustomLetter;
          break;
      }
    }

    public override FileSystemEntry[] GetFileSystemEntries(FileSystemEntry entry)
    {
      if (this.templateType == "CF")
      {
        if (Session.ConfigurationManager.CustomLetterObjectExists(CustomLetterType.Generic, entry))
          return Session.ConfigurationManager.GetFilteredCustomLetterDirEntries(CustomLetterType.Generic, entry);
        int num = (int) Utils.Dialog((IWin32Window) null, "The folder cannot be found.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return (FileSystemEntry[]) null;
      }
      if (Session.ConfigurationManager.TemplateSettingsObjectExists(this.tpType, entry))
        return Session.ConfigurationManager.GetFilteredTemplateDirEntries(this.tpType, entry);
      int num1 = (int) Utils.Dialog((IWin32Window) null, "The folder cannot be found.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      return (FileSystemEntry[]) null;
    }

    public override bool EntryExists(FileSystemEntry entry)
    {
      return this.templateType == "CF" ? Session.ConfigurationManager.CustomLetterObjectExists(CustomLetterType.Generic, entry) : Session.ConfigurationManager.TemplateSettingsObjectExists(this.tpType, entry);
    }

    public override bool EntryExistsOfAnyType(FileSystemEntry entry)
    {
      return this.templateType == "CF" ? Session.ConfigurationManager.CustomLetterObjectExistsOfAnyType(CustomLetterType.Generic, entry) : Session.ConfigurationManager.TemplateSettingsObjectExistsOfAnyType(this.tpType, entry);
    }

    public override void CreateFolder(FileSystemEntry entry)
    {
      if (this.templateType == "CF")
        Session.ConfigurationManager.CreateCustomLetterFolder(CustomLetterType.Generic, entry);
      else
        Session.ConfigurationManager.CreateTemplateSettingsFolder(this.tpType, entry);
    }

    public override void MoveEntry(FileSystemEntry sourceEntry, FileSystemEntry targetEntry)
    {
      if (this.templateType == "CF")
        Session.ConfigurationManager.MoveCustomLetterObject(CustomLetterType.Generic, sourceEntry, targetEntry);
      else
        Session.ConfigurationManager.MoveTemplateSettingsObject(this.tpType, sourceEntry, targetEntry);
    }

    public override void DeleteEntry(FileSystemEntry entry)
    {
      if (entry.Type != FileSystemEntry.Types.Folder)
        return;
      if (this.templateType == "CF")
        Session.ConfigurationManager.DeleteCustomLetterObject(CustomLetterType.Generic, entry);
      else
        Session.ConfigurationManager.DeleteTemplateSettingsObject(this.tpType, entry);
    }
  }
}
