// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.AddForms
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Serialization;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Diagnostics;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class AddForms : AddFormBaseDialog
  {
    private const string className = "AddForms";
    private static readonly string sw = Tracing.SwOutsideLoan;
    private Hashtable existForms;
    private InputFormInfo[] allFormInfos;
    private ArrayList selectedFormTable;

    public AddForms(Sessions.Session session, Hashtable existForms, bool multiselect)
      : base(session, false)
    {
      this.allFormInfos = this.session.FormManager.GetFormInfos(InputFormCategory.Form);
      this.gridViewTemplate.HeaderVisible = false;
      this.gridViewTemplate.Columns.RemoveAt(1);
      this.gridViewTemplate.Columns[0].SpringToFit = true;
      this.Text = "Add Input Forms";
      this.existForms = existForms;
      this.radioAll.Checked = true;
      this.ifsExplorer = new TemplateIFSExplorer(this.session, EllieMae.EMLite.ClientServer.TemplateSettingsType.FormList);
      this.fsExplorerTemplate.FileType = FSExplorer.FileTypes.FormLists;
      this.fsExplorerTemplate.HideAllButtons = true;
      this.fsExplorerTemplate.SingleSelection = true;
      this.fsExplorerTemplate.SetProperties(false, false, true, 4, true);
      this.gridViewAll.AllowMultiselect = this.gridViewTemplate.AllowMultiselect = this.radioTemplate.Visible = multiselect;
      this.initForm();
    }

    public ArrayList SelectedFormTable => this.selectedFormTable;

    private void initForm()
    {
      this.gridViewAll.BeginUpdate();
      this.gridViewAll.Items.Clear();
      string clientId = this.session.CompanyInfo.ClientID;
      bool enabledToExportFnmfre = this.session.MainScreen.IsClientEnabledToExportFNMFRE;
      try
      {
        if (Tracing.IsSwitchActive(AddForms.sw, TraceLevel.Verbose))
          Tracing.Log(AddForms.sw, TraceLevel.Verbose, nameof (AddForms), "initForm: Loading all accessible forms...");
        foreach (InputFormInfo allFormInfo in this.allFormInfos)
        {
          if ((this.session.StartupInfo.AllowURLA2020 || !ShipInDarkValidation.IsURLA2020Form(allFormInfo.FormID)) && (!(allFormInfo.FormID == "ULDD") || enabledToExportFnmfre) && !InputFormInfo.IsChildForm(allFormInfo.FormID) && !(allFormInfo.FormID == "MAX23K") && !this.existForms.ContainsKey((object) allFormInfo.FormID.ToString()))
            this.gridViewAll.Items.Add(new GVItem(allFormInfo.Name)
            {
              Tag = (object) allFormInfo.FormID
            });
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(AddForms.sw, TraceLevel.Error, nameof (AddForms), "initForm: Can't load accessible forms. Error: " + ex.Message);
      }
      this.gridViewAll.EndUpdate();
      this.initTemplates("FormListTemplate");
    }

    protected override void okBtn_Click(object sender, EventArgs e)
    {
      if (this.radioTemplate.Checked)
      {
        if (this.gridViewTemplate.SelectedItems.Count == 0)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "You have to select an input form first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return;
        }
        this.selectedFormTable = new ArrayList();
        for (int index = 0; index < this.gridViewTemplate.SelectedItems.Count; ++index)
          this.selectedFormTable.Add((object) new string[2]
          {
            this.gridViewTemplate.SelectedItems[index].Tag.ToString(),
            this.gridViewTemplate.SelectedItems[index].Text
          });
      }
      else
      {
        if (this.gridViewAll.SelectedItems.Count == 0)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "You have to select an input form first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return;
        }
        this.selectedFormTable = new ArrayList();
        for (int index = 0; index < this.gridViewAll.SelectedItems.Count; ++index)
          this.selectedFormTable.Add((object) new string[2]
          {
            this.gridViewAll.SelectedItems[index].Tag.ToString(),
            this.gridViewAll.SelectedItems[index].Text
          });
      }
      this.DialogResult = DialogResult.OK;
    }

    protected override void fsExplorerTemplate_SelectedEntryChanged(object sender, EventArgs e)
    {
      base.fsExplorerTemplate_SelectedEntryChanged(sender, e);
      FileSystemEntry tag;
      try
      {
        tag = (FileSystemEntry) this.fsExplorerTemplate.SelectedItems[0].Tag;
        if (tag.Type == FileSystemEntry.Types.Folder)
          return;
      }
      catch (Exception ex)
      {
        Tracing.Log(AddForms.sw, TraceLevel.Error, nameof (AddForms), "fsExplorerTemplate_SelectedEntryChanged: Can't access form template. Error: " + ex.Message);
        return;
      }
      if (!this.session.ConfigurationManager.TemplateSettingsObjectExists(EllieMae.EMLite.ClientServer.TemplateSettingsType.FormList, tag))
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "The form template '" + tag.Name + "' can't be accessed.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        FormTemplate formTemplate = (FormTemplate) null;
        try
        {
          formTemplate = (FormTemplate) this.session.ConfigurationManager.GetTemplateSettings(EllieMae.EMLite.ClientServer.TemplateSettingsType.FormList, tag);
        }
        catch (Exception ex)
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this, "Can't open " + tag.Name + " form template file. Message: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        XmlStringTable existingForms = formTemplate.GetExistingForms();
        ArrayList arrayList = new ArrayList();
        int num3 = -1;
        while (true)
        {
          string str;
          do
          {
            ++num3;
            if (existingForms.ContainsKey(num3.ToString()))
              str = (string) existingForms[num3.ToString()];
            else
              goto label_12;
          }
          while (!(str != string.Empty));
          arrayList.Add((object) str.ToLower());
        }
label_12:
        if (arrayList.Count > 0)
        {
          bool flag = false;
          foreach (InputFormInfo allFormInfo in this.allFormInfos)
          {
            for (int index = 0; index < arrayList.Count; ++index)
            {
              if (string.Compare(allFormInfo.Name, arrayList[index].ToString(), true) == 0 && !this.existForms.ContainsKey((object) allFormInfo.FormID))
              {
                this.gridViewTemplate.Items.Add(new GVItem(allFormInfo.Name)
                {
                  Tag = (object) allFormInfo.FormID.ToString()
                });
                flag = true;
              }
            }
          }
          if (!flag)
          {
            int num4 = (int) Utils.Dialog((IWin32Window) this, "All input forms in this template have been added to Input Form Rule.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          }
        }
        this.fsExplorerTemplate.SetFocusToFileListView();
      }
    }
  }
}
