// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.LoanProgramFileSystem
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.Properties;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Serialization;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class LoanProgramFileSystem(Sessions.Session session) : TemplateFileSystem(session, EllieMae.EMLite.ClientServer.TemplateSettingsType.LoanProgram)
  {
    public override string RootObjectDisplayName => "Loan Programs";

    public override string FileEntryDisplayName => "Loan Program";

    public override bool AllowPrivateAccess
    {
      get => this.Session.ACL.IsAuthorizedForFeature(AclFeature.SettingsTab_Personal_LoanPrograms);
    }

    public override AclFileType FileType => AclFileType.LoanProgram;

    public override bool CreateFile(IWin32Window parentWindow, FileSystemEntry fileEntry)
    {
      LoanProgram data = new LoanProgram();
      data.TemplateName = fileEntry.Name;
      data.SetField("LP130", "none");
      try
      {
        this.Session.ConfigurationManager.SaveTemplateSettings(EllieMae.EMLite.ClientServer.TemplateSettingsType.LoanProgram, fileEntry, (BinaryObject) (BinaryConvertibleObject) data);
        return true;
      }
      catch (PathTooLongException ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this.Session.Application, "Error creating template: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this.Session.Application, "Error creating template: " + (object) ex, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
    }

    private bool importFile(
      IWin32Window parentWindow,
      FileSystemEntry fileEntry,
      LoanProgram loanProgram)
    {
      try
      {
        this.Session.ConfigurationManager.SaveTemplateSettings(EllieMae.EMLite.ClientServer.TemplateSettingsType.LoanProgram, fileEntry, (BinaryObject) (BinaryConvertibleObject) loanProgram);
        return true;
      }
      catch (PathTooLongException ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this.Session.Application, "Error importing template: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this.Session.Application, "Error importing template: " + (object) ex, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
    }

    public override bool OpenFile(IWin32Window parentWindow, FileSystemEntry fileEntry)
    {
      LoanProgram templateObject = (LoanProgram) this.GetTemplateObject(fileEntry);
      using (LoanProgramDialog loanProgramDialog = new LoanProgramDialog(this.Session, templateObject, fileEntry.IsPublic))
      {
        if (loanProgramDialog.ShowDialog(parentWindow) == DialogResult.OK)
        {
          this.Session.ConfigurationManager.SaveTemplateSettings(EllieMae.EMLite.ClientServer.TemplateSettingsType.LoanProgram, fileEntry, (BinaryObject) (BinaryConvertibleObject) templateObject);
          return true;
        }
      }
      return false;
    }

    public override void ConfigureExplorer(FileSystemExplorer explorer)
    {
      int width = explorer.ClientRectangle.Width;
      explorer.ResizeColumn(0, Convert.ToInt32(0.4 * (double) width));
      explorer.AddColumn("Program Description", "Description", Convert.ToInt32(0.2 * (double) width));
      explorer.AddColumn("Plan Type", "PlanType", Convert.ToInt32(0.2 * (double) width));
      explorer.AddColumn("Investor", "PlanInvestor", Convert.ToInt32(0.2 * (double) width));
      explorer.AddColumn("Description", "PlanDescription", Convert.ToInt32(0.2 * (double) width));
    }

    public override void CustomizeListItem(ExplorerListItem listItem)
    {
      string str = string.Concat(listItem.FileFolderEntry.Properties[(object) "PlanType"]);
      if (listItem.FileFolderEntry.Type == FileSystemEntry.Types.Folder)
      {
        listItem.SetColumnSortValue(2, (object) -1);
      }
      else
      {
        switch (str)
        {
          case "eDisclosures":
            listItem.SetColumnSortValue(2, (object) 0);
            break;
          case "Closing Docs":
            listItem.SetColumnSortValue(2, (object) 1);
            break;
          default:
            listItem.SetColumnSortValue(2, (object) 2);
            break;
        }
      }
    }

    public override Image GetCustomDisplayIcon(FileSystemEntry entry)
    {
      return string.Concat(entry.Properties[(object) "LinkedToPlan"]) == "Y" ? (Image) Resources.tempate_linked : (Image) null;
    }

    public override bool Import(IWin32Window parentWindow, FileSystemEntry fileEntry)
    {
      foreach (string path in this.selectFilesToImport(parentWindow))
      {
        string xmlData = string.Empty;
        using (TextReader textReader = (TextReader) new StreamReader(path))
        {
          xmlData = textReader.ReadToEnd();
          textReader.Close();
        }
        try
        {
          Dictionary<string, string> fieldErrors = new Dictionary<string, string>();
          if (this.validateImportFile(xmlData, out fieldErrors) != LoanProgramFileSystem.ImportStatus.Success)
          {
            if (fieldErrors.Count > 0)
            {
              StringBuilder stringBuilder = new StringBuilder();
              foreach (KeyValuePair<string, string> keyValuePair in fieldErrors)
                stringBuilder.AppendFormat("{0}: {1}\r\n", (object) keyValuePair.Key, (object) keyValuePair.Value);
              string additionalInfo = stringBuilder.ToString();
              int num = (int) MessageDialog.Show(parentWindow, "Import file has invalid data. Click below to review the errors.", "Import Validations", "Field Errors", additionalInfo, MessageDialogButtons.OKMoreInfo, MessageBoxIcon.Hand);
            }
            else
            {
              int num1 = (int) Utils.Dialog(parentWindow, "Error importing the LoanProgram template. Please verify the file format and try again.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            return false;
          }
          LoanProgram loanProgram = (LoanProgram) new XmlSerializer().Deserialize(xmlData, typeof (LoanProgram));
          FileSystemEntry fileSystemEntry = ((IEnumerable<FileSystemEntry>) this.GetFileSystemEntries(fileEntry)).FirstOrDefault<FileSystemEntry>((Func<FileSystemEntry, bool>) (x => x.Name == loanProgram.TemplateName));
          if (fileSystemEntry != null && Utils.Dialog(parentWindow, "There is already an entry with name \"" + fileSystemEntry.Name + "\" in this folder. Do you want to overwrite it?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            return false;
          string templateName = loanProgram.TemplateName;
          FileSystemEntry fileEntry1 = new FileSystemEntry(fileEntry.Path, templateName, FileSystemEntry.Types.File, fileEntry.Owner);
          if (!this.importFile(parentWindow, fileEntry1, loanProgram))
          {
            int num = (int) Utils.Dialog(parentWindow, "Importing file " + path + " failed.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return false;
          }
          int num2 = (int) Utils.Dialog(parentWindow, "Loan Program template has been successfully imported.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
        catch (Exception ex)
        {
          Tracing.Log(true, "ERROR", nameof (LoanProgramFileSystem), "Importing file " + path + " failed. Error: " + ex.Message + Environment.NewLine + ex.StackTrace);
          int num = (int) Utils.Dialog(parentWindow, "Importing file " + path + " failed.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return false;
        }
      }
      return true;
    }

    private LoanProgramFileSystem.ImportStatus validateImportFile(
      string xmlData,
      out Dictionary<string, string> fieldErrors)
    {
      fieldErrors = new Dictionary<string, string>();
      XmlNodeList childNodes;
      try
      {
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(xmlData);
        if (xmlDocument.ChildNodes.Count != 1)
          return LoanProgramFileSystem.ImportStatus.FormatError;
        XmlElement firstChild1 = xmlDocument.FirstChild as XmlElement;
        if (firstChild1.Name != "objdata")
          return LoanProgramFileSystem.ImportStatus.FormatError;
        XmlElement firstChild2 = firstChild1.FirstChild as XmlElement;
        if (firstChild2.GetAttribute("name") != "root")
          return LoanProgramFileSystem.ImportStatus.FormatError;
        XmlNodeList xmlNodeList = firstChild2.SelectNodes("element[@name=\"tbl\"]");
        if (xmlNodeList == null || xmlNodeList.Count != 1)
          return LoanProgramFileSystem.ImportStatus.FormatError;
        childNodes = xmlNodeList[0].ChildNodes;
      }
      catch (Exception ex)
      {
        return LoanProgramFileSystem.ImportStatus.FormatError;
      }
      string[] templateFields = LoanProgram.TemplateFields;
      StandardFields standardFields = this.Session.LoanManager.GetStandardFields();
      FieldSettings fieldSettings = this.Session.LoanManager.GetFieldSettings();
      foreach (object obj in childNodes)
      {
        XmlElement xmlElement = obj as XmlElement;
        string field = xmlElement.GetAttribute("name");
        if (!(field.ToUpper() == "DTDESC"))
        {
          string innerText = xmlElement.InnerText;
          string fieldId = ((IEnumerable<string>) templateFields).FirstOrDefault<string>((Func<string, bool>) (fld => fld.ToLower() == field.ToLower()));
          if (fieldId == null)
          {
            try
            {
              fieldId = LoanProgram.GetMappingFieldIdForLpField(field);
            }
            catch
            {
              fieldErrors.Add(field, "This field is not allowed in the loan program template");
              continue;
            }
          }
          try
          {
            FieldDefinition fieldDefinition = EncompassFields.GetField(fieldId, fieldSettings);
            if (fieldDefinition == null && standardFields.VirtualFields.Contains(fieldId))
              fieldDefinition = standardFields.VirtualFields[fieldId];
            bool needsUpdate = false;
            Utils.FormatInput(innerText, fieldDefinition.Format, ref needsUpdate);
            if (needsUpdate)
              fieldErrors.Add(field, "Invalid value for the field");
          }
          catch
          {
            fieldErrors.Add(field, "This field is not allowed in the loan program template");
          }
        }
      }
      return fieldErrors.Any<KeyValuePair<string, string>>() ? LoanProgramFileSystem.ImportStatus.FieldError : LoanProgramFileSystem.ImportStatus.Success;
    }

    private string[] selectFilesToImport(IWin32Window parentWindow)
    {
      using (OpenFileDialog openFileDialog = new OpenFileDialog())
      {
        openFileDialog.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";
        openFileDialog.Multiselect = false;
        openFileDialog.ShowDialog(parentWindow);
        return openFileDialog.FileNames;
      }
    }

    private bool canImport()
    {
      bool flag;
      try
      {
        flag = this.Session.UserInfo.IsAdministrator() || this.Session.UserInfo.IsSuperAdministrator() || (bool) this.Session.StartupInfo.UserAclFeatureRights[(object) AclFeature.SettingsTab_Personal_LoanPrograms_AllowImport];
      }
      catch
      {
        flag = false;
      }
      return flag;
    }

    private enum ImportStatus
    {
      Success,
      FormatError,
      FieldError,
    }
  }
}
