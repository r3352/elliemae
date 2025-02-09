// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.Import.TPOInvalidRecordListForm
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using EllieMae.EMLite.Reporting;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ContactUI.Import
{
  public class TPOInvalidRecordListForm : Form
  {
    private EllieMae.EMLite.ContactUI.ContactType importType = EllieMae.EMLite.ContactUI.ContactType.TPO;
    private IContainer components;
    private GroupContainer grpContacts;
    private GridView gridViewContactsTPO;
    private Button btnCancel;
    private Button btnOK;
    private Label label1;
    private StandardIconButton btnExport;

    public TPOInvalidRecordListForm(
      ListView.ColumnHeaderCollection headerCollection,
      List<object[]> invalidItems,
      bool abortOnly,
      EllieMae.EMLite.ContactUI.ContactType importType)
    {
      this.importType = importType;
      this.InitializeComponent();
      this.gridViewContactsTPO.Columns.Clear();
      this.gridViewContactsTPO.Columns.Add(new GVColumn()
      {
        Text = "Error",
        Width = 200
      });
      for (int index = 0; index < headerCollection.Count; ++index)
      {
        if (string.Compare(headerCollection[index].Text, "(Unassigned)", true) != 0)
          this.gridViewContactsTPO.Columns.Add(new GVColumn()
          {
            Text = headerCollection[index].Text,
            Width = headerCollection[index].Width,
            TextAlign = headerCollection[index].TextAlign
          });
      }
      string empty = string.Empty;
      for (int index1 = 0; index1 < invalidItems.Count; ++index1)
      {
        ListViewItem listViewItem = (ListViewItem) invalidItems[index1][0];
        GVItem gvItem = new GVItem((string) invalidItems[index1][1]);
        for (int index2 = 0; index2 < headerCollection.Count; ++index2)
        {
          if (string.Compare(headerCollection[index2].Text, "(Unassigned)", true) != 0)
            gvItem.SubItems.Add(index2 >= listViewItem.SubItems.Count ? (object) "" : (object) listViewItem.SubItems[index2].Text);
        }
        this.gridViewContactsTPO.Items.Add(gvItem);
      }
      if (abortOnly)
      {
        this.btnOK.Visible = false;
        this.btnCancel.Text = "Abort";
      }
      if (this.importType != EllieMae.EMLite.ContactUI.ContactType.TPOCompany)
        return;
      this.Text = "TPO Company and Branch Import";
      this.label1.Text = "The following companies and branches will not be imported because of missing or invalid data. If you proceed, record with invalid data will not be imported.";
    }

    private void btnExport_Click(object sender, EventArgs e)
    {
      Cursor.Current = Cursors.WaitCursor;
      ExcelHandler excelHandler = new ExcelHandler();
      for (int nColumnIndex = 0; nColumnIndex < this.gridViewContactsTPO.Columns.Count; ++nColumnIndex)
        excelHandler.AddHeaderColumn(this.gridViewContactsTPO.Columns[nColumnIndex].Text, "@");
      for (int nItemIndex1 = 0; nItemIndex1 < this.gridViewContactsTPO.Items.Count; ++nItemIndex1)
      {
        string[] data = new string[this.gridViewContactsTPO.Columns.Count];
        for (int nItemIndex2 = 0; nItemIndex2 < this.gridViewContactsTPO.Columns.Count; ++nItemIndex2)
          data[nItemIndex2] = this.gridViewContactsTPO.Items[nItemIndex1].SubItems[nItemIndex2].Text;
        excelHandler.AddDataRow(data);
      }
      excelHandler.Export(true);
      Cursor.Current = Cursors.Default;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      this.grpContacts = new GroupContainer();
      this.gridViewContactsTPO = new GridView();
      this.btnCancel = new Button();
      this.btnOK = new Button();
      this.label1 = new Label();
      this.btnExport = new StandardIconButton();
      this.grpContacts.SuspendLayout();
      ((ISupportInitialize) this.btnExport).BeginInit();
      this.SuspendLayout();
      this.grpContacts.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.grpContacts.Controls.Add((Control) this.btnExport);
      this.grpContacts.Controls.Add((Control) this.gridViewContactsTPO);
      this.grpContacts.HeaderForeColor = SystemColors.ControlText;
      this.grpContacts.Location = new Point(12, 38);
      this.grpContacts.Name = "grpContacts";
      this.grpContacts.Size = new Size(927, 445);
      this.grpContacts.TabIndex = 7;
      this.gridViewContactsTPO.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "First Name";
      gvColumn1.Width = 150;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Last Name";
      gvColumn2.Width = 100;
      this.gridViewContactsTPO.Columns.AddRange(new GVColumn[2]
      {
        gvColumn1,
        gvColumn2
      });
      this.gridViewContactsTPO.Dock = DockStyle.Fill;
      this.gridViewContactsTPO.Location = new Point(1, 26);
      this.gridViewContactsTPO.Name = "gridViewContactsTPO";
      this.gridViewContactsTPO.Size = new Size(925, 418);
      this.gridViewContactsTPO.TabIndex = 1;
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(865, 491);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 8;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.DialogResult = DialogResult.OK;
      this.btnOK.Location = new Point(784, 491);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 9;
      this.btnOK.Text = "Con&tinue";
      this.btnOK.UseVisualStyleBackColor = true;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(13, 13);
      this.label1.Name = "label1";
      this.label1.Size = new Size(497, 13);
      this.label1.TabIndex = 10;
      this.label1.Text = "The following contacts will not be imported because of missing or invalid data. If you proceed, record with invalid data will not be imported.";
      this.btnExport.BackColor = Color.Transparent;
      this.btnExport.Location = new Point(903, 5);
      this.btnExport.MouseDownImage = (Image) null;
      this.btnExport.Name = "btnExport";
      this.btnExport.Size = new Size(16, 16);
      this.btnExport.StandardButtonType = StandardIconButton.ButtonType.ExcelButton;
      this.btnExport.TabIndex = 2;
      this.btnExport.TabStop = false;
      this.btnExport.Click += new EventHandler(this.btnExport_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(951, 524);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.grpContacts);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (TPOInvalidRecordListForm);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "TPO Contact Import";
      this.grpContacts.ResumeLayout(false);
      ((ISupportInitialize) this.btnExport).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
