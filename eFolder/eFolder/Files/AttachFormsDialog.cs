// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.Files.AttachFormsDialog
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.eFolder;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.DocumentConverter;
using EllieMae.EMLite.LoanServices;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.eFolder.Files
{
  public class AttachFormsDialog : Form
  {
    private LoanDataMgr loanDataMgr;
    private DocumentLog doc;
    private GridViewDataManager gvTemplatesMgr;
    private ArrayList fileList = new ArrayList();
    private IContainer components;
    private ComboBox pairCbo;
    private Label pairLbl;
    private GridView gvTemplates;
    private Button btnAttach;
    private Button btnCancel;
    private ToolTip tooltip;

    public AttachFormsDialog(LoanDataMgr loanDataMgr, DocumentLog doc)
    {
      this.InitializeComponent();
      this.loanDataMgr = loanDataMgr;
      this.doc = doc;
      this.loadBorrowerList();
      this.initTemplateList();
      this.loadTemplateList();
    }

    public FileAttachment[] Files
    {
      get => (FileAttachment[]) this.fileList.ToArray(typeof (FileAttachment));
    }

    private void loadBorrowerList()
    {
      foreach (BorrowerPair borrowerPair in this.loanDataMgr.LoanData.GetBorrowerPairs())
      {
        this.pairCbo.Items.Add((object) borrowerPair);
        if (this.doc != null && this.doc.PairId == borrowerPair.Id)
          this.pairCbo.SelectedItem = (object) borrowerPair;
      }
      if (this.pairCbo.SelectedItem != null)
        return;
      this.pairCbo.SelectedIndex = 0;
    }

    private void initTemplateList()
    {
      this.gvTemplatesMgr = new GridViewDataManager(this.gvTemplates, this.loanDataMgr);
      this.gvTemplatesMgr.CreateLayout(new TableLayout.Column[3]
      {
        GridViewDataManager.NameColumn,
        GridViewDataManager.DocTypeColumn,
        GridViewDataManager.DocSourceColumn
      });
    }

    private void loadTemplateList()
    {
      foreach (DocumentTemplate template in this.loanDataMgr.SystemConfiguration.DocumentTrackingSetup)
      {
        if (template.IsPrintable)
          this.gvTemplatesMgr.AddItem(template);
      }
      this.gvTemplates.Sort(0, SortOrder.Ascending);
    }

    private void gvTemplates_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.btnAttach.Enabled = this.gvTemplates.SelectedItems.Count > 0;
    }

    private void btnAttach_Click(object sender, EventArgs e)
    {
      if (Session.SessionObjects.RuntimeEnvironment == RuntimeEnvironment.Hosted && !Modules.IsModuleAvailableForUser(EncompassModule.EDM, true))
        return;
      BorrowerPair currentBorrowerPair = this.loanDataMgr.LoanData.CurrentBorrowerPair;
      BorrowerPair selectedItem1 = (BorrowerPair) this.pairCbo.SelectedItem;
      if (currentBorrowerPair != selectedItem1)
        this.loanDataMgr.LoanData.SetBorrowerPair(selectedItem1);
      List<DocumentTemplate> documentTemplateList = new List<DocumentTemplate>();
      foreach (GVItem selectedItem2 in this.gvTemplates.SelectedItems)
      {
        DocumentTemplate tag = (DocumentTemplate) selectedItem2.Tag;
        documentTemplateList.Add(tag);
      }
      FormExport formExport = new FormExport(this.loanDataMgr);
      Bam bam = new Bam(this.loanDataMgr);
      formExport.EntityList = bam.GetCompanyDisclosureEntities();
      Dictionary<DocumentTemplate, string[]> dictionary = formExport.ExportForms(documentTemplateList.ToArray(), false);
      foreach (DocumentTemplate key in documentTemplateList)
      {
        if (dictionary.ContainsKey(key))
        {
          foreach (string str in dictionary[key])
          {
            string pdfFile = str;
            string title = key.Name;
            using (PdfEditor pdfEditor = new PdfEditor(pdfFile))
            {
              switch (pdfEditor.IntendedFor)
              {
                case ForBorrowerType.Borrower:
                  title += "_Borrower";
                  break;
                case ForBorrowerType.CoBorrower:
                  title += "_CoBorrower";
                  break;
              }
            }
            FileAttachment file = (FileAttachment) null;
            if (this.loanDataMgr.UseSkyDriveClassic)
              Task.WaitAll((Task) Task.Run<FileAttachment>((Func<FileAttachment>) (() => file = this.loanDataMgr.FileAttachments.AddAttachment(AddReasonType.Forms, pdfFile, title, this.doc))));
            else
              file = this.loanDataMgr.FileAttachments.AddAttachment(AddReasonType.Forms, pdfFile, title, this.doc);
            this.fileList.Add((object) file);
          }
        }
      }
      if (Session.SessionObjects.RuntimeEnvironment == RuntimeEnvironment.Hosted)
        Transaction.Log(this.loanDataMgr, "Form");
      if (currentBorrowerPair != selectedItem1)
        this.loanDataMgr.LoanData.SetBorrowerPair(currentBorrowerPair);
      this.DialogResult = DialogResult.OK;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      this.pairCbo = new ComboBox();
      this.pairLbl = new Label();
      this.gvTemplates = new GridView();
      this.tooltip = new ToolTip(this.components);
      this.btnAttach = new Button();
      this.btnCancel = new Button();
      this.SuspendLayout();
      this.pairCbo.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.pairCbo.DropDownStyle = ComboBoxStyle.DropDownList;
      this.pairCbo.Location = new Point(112, 8);
      this.pairCbo.Name = "pairCbo";
      this.pairCbo.Size = new Size(430, 22);
      this.pairCbo.TabIndex = 1;
      this.pairLbl.AutoSize = true;
      this.pairLbl.Location = new Point(12, 12);
      this.pairLbl.Name = "pairLbl";
      this.pairLbl.Size = new Size(94, 14);
      this.pairLbl.TabIndex = 0;
      this.pairLbl.Text = "For Borrower Pair";
      this.gvTemplates.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.gvTemplates.ClearSelectionsOnEmptyRowClick = false;
      this.gvTemplates.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvTemplates.Location = new Point(12, 36);
      this.gvTemplates.Name = "gvTemplates";
      this.gvTemplates.Size = new Size(530, 280);
      this.gvTemplates.TabIndex = 2;
      this.gvTemplates.TextTrimming = StringTrimming.EllipsisCharacter;
      this.gvTemplates.SelectedIndexChanged += new EventHandler(this.gvTemplates_SelectedIndexChanged);
      this.btnAttach.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnAttach.Enabled = false;
      this.btnAttach.Location = new Point(392, 324);
      this.btnAttach.Name = "btnAttach";
      this.btnAttach.Size = new Size(75, 22);
      this.btnAttach.TabIndex = 3;
      this.btnAttach.Text = "Attach";
      this.btnAttach.Click += new EventHandler(this.btnAttach_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(468, 324);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 22);
      this.btnCancel.TabIndex = 4;
      this.btnCancel.Text = "Cancel";
      this.AcceptButton = (IButtonControl) this.btnAttach;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(554, 356);
      this.Controls.Add((Control) this.btnAttach);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.gvTemplates);
      this.Controls.Add((Control) this.pairCbo);
      this.Controls.Add((Control) this.pairLbl);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (AttachFormsDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Attach Encompass Forms";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
