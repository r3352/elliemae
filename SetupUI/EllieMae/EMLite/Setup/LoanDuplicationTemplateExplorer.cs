// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.LoanDuplicationTemplateExplorer
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class LoanDuplicationTemplateExplorer : UserControl
  {
    private Sessions.Session session;
    private TemplateIFSExplorer ifsExplorer;
    private LoanDuplicationTemplateForm loanDuplicationTemplateForm;
    private IContainer components;
    private FSExplorer fsTemplateExplorer;

    public string[] SelectedNames
    {
      get
      {
        return this.fsTemplateExplorer.SelectedItems.Count == 0 ? (string[]) null : this.fsTemplateExplorer.SelectedItems.Select<GVItem, string>((Func<GVItem, string>) (item => item.SubItems[0].Text)).ToArray<string>();
      }
    }

    public LoanDuplicationTemplateExplorer(Sessions.Session session, SetUpContainer setUpContainer)
    {
      this.session = session;
      this.fsTemplateExplorer = new FSExplorer(this.session);
      this.InitializeComponent();
      this.ifsExplorer = new TemplateIFSExplorer(this.session, EllieMae.EMLite.ClientServer.TemplateSettingsType.LoanDuplicationTemplate);
      this.fsTemplateExplorer.FileType = FSExplorer.FileTypes.LoanDuplicationTemplate;
      this.fsTemplateExplorer.HasPublicRight = true;
      this.fsTemplateExplorer.SetProperties(false, false, false, 36, true);
      this.fsTemplateExplorer.Init((IFSExplorerBase) this.ifsExplorer, FileSystemEntry.PublicRoot, true);
      this.fsTemplateExplorer.SetupForPurchaseAdvice(true);
      this.fsTemplateExplorer.RenameButtonSize = new Size(62, 22);
      this.ifsExplorer.AdditionalFieldButtonClicked += new EventHandler(this.ifsExplorer_AdditionalFieldButtonClicked);
    }

    private void ifsExplorer_AdditionalFieldButtonClicked(object sender, EventArgs e)
    {
      this.loanDuplicationTemplateForm = (LoanDuplicationTemplateForm) sender;
      using (AddFields addFields = new AddFields(this.session))
      {
        addFields.OnAddMoreButtonClick += new EventHandler(this.addFieldDlg_OnAddMoreButtonClick);
        if (addFields.ShowDialog((IWin32Window) this.loanDuplicationTemplateForm) != DialogResult.OK || this.loanDuplicationTemplateForm == null)
          return;
        this.loanDuplicationTemplateForm.AddAdditionalFields(addFields.SelectedFieldIDs);
      }
    }

    private void addFieldDlg_OnAddMoreButtonClick(object sender, EventArgs e)
    {
      AddFields addFields = (AddFields) sender;
      if (addFields == null || this.loanDuplicationTemplateForm == null)
        return;
      this.loanDuplicationTemplateForm.AddAdditionalFields(addFields.SelectedFieldIDs);
    }

    public void HighlightSelectedRows(List<string> selectedNames)
    {
      for (int index = 0; index < selectedNames.Count; ++index)
      {
        for (int nItemIndex = 0; nItemIndex < this.fsTemplateExplorer.GVItems.Count; ++nItemIndex)
        {
          if (this.fsTemplateExplorer.GVItems[nItemIndex].SubItems[0].Text == selectedNames[index])
          {
            this.fsTemplateExplorer.GVItems[nItemIndex].Selected = true;
            break;
          }
        }
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.SuspendLayout();
      this.fsTemplateExplorer.Dock = DockStyle.Fill;
      this.fsTemplateExplorer.FolderComboSelectedIndex = -1;
      this.fsTemplateExplorer.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.fsTemplateExplorer.HasPublicRight = true;
      this.fsTemplateExplorer.Location = new Point(0, 0);
      this.fsTemplateExplorer.Name = "fsTemplateExplorer";
      this.fsTemplateExplorer.RenameButtonSize = new Size(62, 22);
      this.fsTemplateExplorer.RESPAMode = FSExplorer.RESPAFilter.All;
      this.fsTemplateExplorer.setContactType = EllieMae.EMLite.ContactUI.ContactType.BizPartner;
      this.fsTemplateExplorer.Size = new Size(980, 633);
      this.fsTemplateExplorer.TabIndex = 15;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.fsTemplateExplorer);
      this.Name = nameof (LoanDuplicationTemplateExplorer);
      this.Size = new Size(980, 633);
      this.ResumeLayout(false);
    }
  }
}
