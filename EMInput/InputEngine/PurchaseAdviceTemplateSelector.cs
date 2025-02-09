// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.PurchaseAdviceTemplateSelector
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.ContactUI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Setup;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class PurchaseAdviceTemplateSelector : Form, IHelp
  {
    private const string className = "PurchaseAdviceTemplateSelector";
    private static string sw = Tracing.SwInputEngine;
    private EllieMae.EMLite.ClientServer.TemplateSettingsType templateType;
    private Sessions.Session session;
    private ITemplateSetting selectedTemplate;
    private IContainer components;
    private FSExplorer tempExplorer;
    private Label label1;
    private Button buttonSelect;
    private Button buttonCancel;
    private CheckBox checkBoxAppend;

    public PurchaseAdviceTemplateSelector(
      Sessions.Session session,
      EllieMae.EMLite.ClientServer.TemplateSettingsType templateType)
    {
      this.session = session;
      this.templateType = templateType;
      this.InitializeComponent();
      this.initForm();
    }

    private void initForm()
    {
      bool canCreateEdit = true;
      FeaturesAclManager aclManager = (FeaturesAclManager) this.session.ACL.GetAclManager(AclCategory.Features);
      if (!this.session.UserInfo.IsSuperAdministrator() && !aclManager.GetUserApplicationRight(AclFeature.ToolsTab_PA_CreateEditTemplate))
        canCreateEdit = false;
      TemplateIFSExplorer ifsExplorer = new TemplateIFSExplorer(this.session, this.templateType);
      if (this.templateType == EllieMae.EMLite.ClientServer.TemplateSettingsType.FundingTemplate)
      {
        this.tempExplorer.FileType = FSExplorer.FileTypes.FundingTemplate;
        this.tempExplorer.RESPAMode = this.session.LoanData == null ? FSExplorer.RESPAFilter.All : (this.session.LoanData.Use2015RESPA ? FSExplorer.RESPAFilter.Respa2015 : (this.session.LoanData.Use2010RESPA ? FSExplorer.RESPAFilter.Respa2010 : FSExplorer.RESPAFilter.Respa2009));
        ifsExplorer.RespaMode = this.tempExplorer.RESPAMode;
      }
      else
        this.tempExplorer.FileType = FSExplorer.FileTypes.PurchaseAdvice;
      this.tempExplorer.HasPublicRight = true;
      this.tempExplorer.SetProperties(false, false, !canCreateEdit, (int) this.templateType, true);
      this.tempExplorer.Init((IFSExplorerBase) ifsExplorer, FileSystemEntry.PublicRoot, true);
      this.tempExplorer.SetupForPurchaseAdvice(canCreateEdit);
      this.tempExplorer.SelectedEntryChanged += new EventHandler(this.tempExplorer_SelectedEntryChanged);
      this.tempExplorer_SelectedEntryChanged((object) null, (EventArgs) null);
    }

    private void tempExplorer_SelectedEntryChanged(object sender, EventArgs e)
    {
      this.buttonSelect.Enabled = this.tempExplorer.SelectedItems.Count == 1;
    }

    private void buttonSelect_Click(object sender, EventArgs e)
    {
      if (this.tempExplorer.SelectedItems.Count == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "You need to select a template.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        FileSystemEntry tag = (FileSystemEntry) this.tempExplorer.SelectedItems[0].Tag;
        if (tag.Type != FileSystemEntry.Types.File)
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this, "You need to select a template.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else if (!this.session.ConfigurationManager.TemplateSettingsObjectExists(this.templateType, tag))
        {
          int num3 = (int) Utils.Dialog((IWin32Window) this, "The template has been deleted.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else
        {
          BinaryObject binaryObject = (BinaryObject) null;
          try
          {
            binaryObject = this.session.ConfigurationManager.GetTemplateSettings(this.templateType, tag);
          }
          catch (Exception ex)
          {
            int num4 = (int) Utils.Dialog((IWin32Window) this, "Encompass can't load " + tag.Name + " template. Error: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            Tracing.Log(PurchaseAdviceTemplateSelector.sw, TraceLevel.Error, nameof (PurchaseAdviceTemplateSelector), "Can't load " + (object) tag + " template. Error: " + ex.Message);
          }
          if (binaryObject == null)
            return;
          try
          {
            if (this.templateType == EllieMae.EMLite.ClientServer.TemplateSettingsType.FundingTemplate)
              this.selectedTemplate = (ITemplateSetting) (FundingTemplate) binaryObject;
            else if (this.templateType == EllieMae.EMLite.ClientServer.TemplateSettingsType.PurchaseAdvice)
              this.selectedTemplate = (ITemplateSetting) (PurchaseAdviceTemplate) binaryObject;
          }
          catch (Exception ex)
          {
            Tracing.Log(PurchaseAdviceTemplateSelector.sw, TraceLevel.Error, nameof (PurchaseAdviceTemplateSelector), "Can't load template. Error: " + ex.Message);
            return;
          }
          if (this.selectedTemplate == null)
            return;
          this.DialogResult = DialogResult.OK;
        }
      }
    }

    public ITemplateSetting SelectedTemplate => this.selectedTemplate;

    public bool AppendTemplate => this.checkBoxAppend.Checked;

    private void PurchaseAdviceTemplateSelector_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.F1)
      {
        this.ShowHelp();
      }
      else
      {
        if (e.KeyCode != Keys.Escape)
          return;
        this.buttonCancel.PerformClick();
      }
    }

    public void ShowHelp()
    {
      if (this.templateType == EllieMae.EMLite.ClientServer.TemplateSettingsType.PurchaseAdvice)
      {
        JedHelp.ShowHelp((Control) this, SystemSettings.HelpFile, "Purchase Advice");
      }
      else
      {
        if (this.templateType != EllieMae.EMLite.ClientServer.TemplateSettingsType.FundingTemplate)
          return;
        JedHelp.ShowHelp((Control) this, SystemSettings.HelpFile, "Funding Template");
      }
    }

    private void tempExplorer_SelectedCurrentFile(object sender, EventArgs e)
    {
      this.buttonSelect_Click((object) null, (EventArgs) null);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.label1 = new Label();
      this.buttonSelect = new Button();
      this.buttonCancel = new Button();
      this.checkBoxAppend = new CheckBox();
      this.tempExplorer = new FSExplorer();
      this.SuspendLayout();
      this.label1.AutoSize = true;
      this.label1.Location = new Point(4, 8);
      this.label1.Name = "label1";
      this.label1.Size = new Size(132, 13);
      this.label1.TabIndex = 2;
      this.label1.Text = "Select a template to apply:";
      this.buttonSelect.Location = new Point(434, 393);
      this.buttonSelect.Name = "buttonSelect";
      this.buttonSelect.Size = new Size(75, 23);
      this.buttonSelect.TabIndex = 3;
      this.buttonSelect.Text = "&Select";
      this.buttonSelect.UseVisualStyleBackColor = true;
      this.buttonSelect.Click += new EventHandler(this.buttonSelect_Click);
      this.buttonCancel.DialogResult = DialogResult.Cancel;
      this.buttonCancel.Location = new Point(518, 393);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = new Size(75, 23);
      this.buttonCancel.TabIndex = 4;
      this.buttonCancel.Text = "&Cancel";
      this.buttonCancel.UseVisualStyleBackColor = true;
      this.checkBoxAppend.AutoSize = true;
      this.checkBoxAppend.Location = new Point(8, 393);
      this.checkBoxAppend.Name = "checkBoxAppend";
      this.checkBoxAppend.Size = new Size(242, 17);
      this.checkBoxAppend.TabIndex = 5;
      this.checkBoxAppend.Text = "Only apply template fields that contain a value";
      this.checkBoxAppend.UseVisualStyleBackColor = true;
      this.tempExplorer.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.tempExplorer.FolderComboSelectedIndex = -1;
      this.tempExplorer.HasPublicRight = true;
      this.tempExplorer.Location = new Point(8, 27);
      this.tempExplorer.Name = "tempExplorer";
      this.tempExplorer.setContactType = ContactType.BizPartner;
      this.tempExplorer.Size = new Size(585, 360);
      this.tempExplorer.TabIndex = 1;
      this.tempExplorer.SelectedCurrentFile += new EventHandler(this.tempExplorer_SelectedCurrentFile);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(602, 426);
      this.Controls.Add((Control) this.checkBoxAppend);
      this.Controls.Add((Control) this.buttonCancel);
      this.Controls.Add((Control) this.buttonSelect);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.tempExplorer);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (PurchaseAdviceTemplateSelector);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Select Template";
      this.KeyDown += new KeyEventHandler(this.PurchaseAdviceTemplateSelector_KeyDown);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
