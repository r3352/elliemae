// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.FormSelectionDialog
// Assembly: EMePass, Version=3.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A610697F-A1EC-4CC3-A30A-403E37B2B276
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMePass.dll

using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.eFolder
{
  public class FormSelectionDialog : Form
  {
    private ArrayList docList = new ArrayList();
    private ListViewSortManager formsSort;
    private Button attachBtn;
    private Button cancelBtn;
    private ListView formsLvw;
    private ColumnHeader titleHdr;
    private ColumnHeader typeHdr;
    private ColumnHeader sourceHdr;
    private Label borLbl;
    private ComboBox borCbo;
    private System.ComponentModel.Container components;

    public string[] DocumentList => (string[]) this.docList.ToArray(typeof (string));

    public FormSelectionDialog(bool selectBorrowerPair)
    {
      this.InitializeComponent();
      this.formsSort = new ListViewSortManager(this.formsLvw, new System.Type[3]
      {
        typeof (ListViewTextCaseInsensitiveSort),
        typeof (ListViewTextCaseInsensitiveSort),
        typeof (ListViewTextCaseInsensitiveSort)
      });
      this.formsSort.Sort(0);
      this.loadBorrowers(selectBorrowerPair);
      this.loadForms();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.attachBtn = new Button();
      this.cancelBtn = new Button();
      this.borLbl = new Label();
      this.borCbo = new ComboBox();
      this.formsLvw = new ListView();
      this.titleHdr = new ColumnHeader();
      this.typeHdr = new ColumnHeader();
      this.sourceHdr = new ColumnHeader();
      this.SuspendLayout();
      this.attachBtn.Enabled = false;
      this.attachBtn.Location = new Point(336, 332);
      this.attachBtn.Name = "attachBtn";
      this.attachBtn.Size = new Size(76, 24);
      this.attachBtn.TabIndex = 3;
      this.attachBtn.Text = "Add";
      this.attachBtn.Click += new EventHandler(this.attachBtn_Click);
      this.cancelBtn.DialogResult = DialogResult.Cancel;
      this.cancelBtn.Location = new Point(416, 332);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(76, 24);
      this.cancelBtn.TabIndex = 4;
      this.cancelBtn.Text = "Cancel";
      this.borLbl.AutoSize = true;
      this.borLbl.Location = new Point(8, 12);
      this.borLbl.Name = "borLbl";
      this.borLbl.Size = new Size(94, 14);
      this.borLbl.TabIndex = 0;
      this.borLbl.Text = "For Borrower Pair";
      this.borCbo.DropDownStyle = ComboBoxStyle.DropDownList;
      this.borCbo.Location = new Point(108, 8);
      this.borCbo.Name = "borCbo";
      this.borCbo.Size = new Size(381, 22);
      this.borCbo.TabIndex = 1;
      this.formsLvw.CheckBoxes = true;
      this.formsLvw.Columns.AddRange(new ColumnHeader[3]
      {
        this.titleHdr,
        this.typeHdr,
        this.sourceHdr
      });
      this.formsLvw.FullRowSelect = true;
      this.formsLvw.Location = new Point(8, 36);
      this.formsLvw.MultiSelect = false;
      this.formsLvw.Name = "formsLvw";
      this.formsLvw.Size = new Size(484, 288);
      this.formsLvw.TabIndex = 0;
      this.formsLvw.UseCompatibleStateImageBehavior = false;
      this.formsLvw.View = View.Details;
      this.formsLvw.ItemCheck += new ItemCheckEventHandler(this.formsLvw_ItemCheck);
      this.titleHdr.Text = "Document";
      this.titleHdr.Width = 210;
      this.typeHdr.Text = "Type";
      this.typeHdr.Width = 100;
      this.sourceHdr.Text = "Source";
      this.sourceHdr.Width = 140;
      this.AcceptButton = (IButtonControl) this.attachBtn;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.BackColor = Color.WhiteSmoke;
      this.CancelButton = (IButtonControl) this.cancelBtn;
      this.ClientSize = new Size(502, 364);
      this.Controls.Add((Control) this.formsLvw);
      this.Controls.Add((Control) this.borCbo);
      this.Controls.Add((Control) this.borLbl);
      this.Controls.Add((Control) this.attachBtn);
      this.Controls.Add((Control) this.cancelBtn);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (FormSelectionDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Add Encompass Forms";
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private void loadBorrowers(bool selectBorrowerPair)
    {
      if (selectBorrowerPair)
      {
        foreach (object borrowerPair in Session.LoanData.GetBorrowerPairs())
          this.borCbo.Items.Add(borrowerPair);
      }
      else
        this.borCbo.Items.Add((object) Session.LoanData.CurrentBorrowerPair);
      this.borCbo.SelectedIndex = 0;
    }

    private void loadForms()
    {
      foreach (DocumentTemplate documentTemplate in Session.LoanDataMgr.SystemConfiguration.DocumentTrackingSetup)
      {
        if (documentTemplate.IsPrintable)
        {
          ListViewItem listViewItem = this.formsLvw.Items.Add(documentTemplate.Name);
          listViewItem.SubItems.Add(documentTemplate.SourceType);
          listViewItem.SubItems.Add(documentTemplate.Source);
          listViewItem.Tag = (object) documentTemplate;
        }
      }
      if (this.formsLvw.Items.Count <= 0)
        return;
      this.formsLvw.Items[0].Selected = true;
    }

    private void formsLvw_ItemCheck(object sender, ItemCheckEventArgs e)
    {
      bool flag = false;
      if (e.NewValue == CheckState.Checked)
        flag = true;
      else if (this.formsLvw.CheckedItems.Count > 1)
        flag = true;
      this.attachBtn.Enabled = flag;
    }

    private void attachBtn_Click(object sender, EventArgs e)
    {
      Session.LoanData.SetBorrowerPair((BorrowerPair) this.borCbo.SelectedItem);
      foreach (ListViewItem checkedItem in this.formsLvw.CheckedItems)
        this.docList.Add((object) ((DocumentTemplate) checkedItem.Tag).Name);
      this.DialogResult = DialogResult.OK;
    }
  }
}
