// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.LoanAssignmentFromFileImport
// Assembly: TradeManagement, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 30F9401A-5385-498E-9C5B-D147722AC94C
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\TradeManagement.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Trading;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite
{
  public class LoanAssignmentFromFileImport : Form
  {
    private List<string> fileList = new List<string>();
    private int maxCount = -1;
    private FileImportType fileType;
    private IContainer components;
    private TableContainer grpLoans;
    private GridView gvLoans;
    private FlowLayoutPanel flpMenu;
    private Button btnAssign;
    private StandardIconButton siBtnNew;
    private Button btnImport;
    private Button btnCancel;
    private Label lblFileName;
    private Label lblCount;
    private StandardIconButton siBtnDelete;
    private ToolTip toolTip1;
    private Label label1;

    public int MaxLoanCount
    {
      get => this.maxCount;
      set => this.maxCount = value;
    }

    public FileImportType FileType
    {
      set
      {
        switch (value)
        {
          case FileImportType.LoanNumberContractNumberProductName:
            this.label1.Text = "Import a CSV file for loans, commitment contract numbers and product names for each loan separated by commas per row or manually enter.";
            this.gvLoans.Columns.Add(new GVColumn("Commitment Contract #"));
            this.gvLoans.Columns.Add(new GVColumn("Product Name"));
            this.gvLoans.Columns[0].Width = Utils.ParseInt((object) ((double) this.gvLoans.Width * 0.34));
            this.gvLoans.Columns[1].Width = Utils.ParseInt((object) ((double) this.gvLoans.Width * 0.33));
            this.gvLoans.Columns[2].Width = Utils.ParseInt((object) ((double) this.gvLoans.Width * 0.33));
            break;
          case FileImportType.LoanNumberPrice:
            this.label1.Text = "Import a CSV file for loans and total prices for each loan separated by commas per row or manually enter.";
            this.gvLoans.Columns.Add(new GVColumn("Total Price"));
            this.gvLoans.Columns[0].Width = Utils.ParseInt((object) ((double) this.gvLoans.Width * 0.34));
            this.gvLoans.Columns[1].Width = Utils.ParseInt((object) ((double) this.gvLoans.Width * 0.33));
            break;
        }
        this.fileType = value;
      }
      get => this.fileType;
    }

    public LoanAssignmentFromFileImport()
    {
      this.InitializeComponent();
      this.RefreshList();
      this.siBtnDelete.Enabled = false;
      this.btnAssign.Enabled = false;
    }

    public GVItemCollection LoanNumberList => this.gvLoans.Items;

    private void LoanAssignmentFromFileImport_Closing(object sender, FormClosingEventArgs e)
    {
      if (this.DialogResult != DialogResult.OK || this.MaxLoanCount <= 0 || this.LoanNumberList.Count <= this.MaxLoanCount)
        return;
      int num = (int) Utils.Dialog((IWin32Window) this, "This delivery type does not allow more than one loan to be assigned to the trade.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      e.Cancel = true;
    }

    private bool IsTitle(string str)
    {
      return str.Trim().Contains("Loan Number") || str.Trim().Contains("Loan #") || str.Trim().Contains("LoanNumber") || str.Trim().Contains("Loan#");
    }

    private void btnImport_Click(object sender, EventArgs e)
    {
      OpenFileDialog openFileDialog = new OpenFileDialog();
      openFileDialog.Title = "Open CSV File";
      openFileDialog.Filter = "CSV files|*.csv";
      openFileDialog.InitialDirectory = "C:\\";
      if (openFileDialog.ShowDialog() != DialogResult.OK)
        return;
      string fileName = openFileDialog.FileName;
      if (!string.Equals(Path.GetExtension(fileName), ".csv", StringComparison.OrdinalIgnoreCase))
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "The type of the selected file is not supported. Please select a CSV file.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else if (this.fileList.Contains(fileName))
      {
        int num2 = (int) Utils.Dialog((IWin32Window) this, "File has been imported.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        this.fileList.Add(fileName);
        this.lblFileName.Text = fileName;
        try
        {
          using (StreamReader streamReader = new StreamReader(fileName))
          {
            List<GVItem> gvItemList = new List<GVItem>();
            while (!streamReader.EndOfStream)
            {
              string input = streamReader.ReadLine();
              if (!string.IsNullOrEmpty(input))
              {
                string[] strArray = new Regex(",(?=(?:[^\"']*\"[^\"']*\")*(?![^\"']*\"))").Split(input);
                if (this.FileType == FileImportType.LoanNumberOnly)
                {
                  if (strArray.Length == 1)
                  {
                    if (!this.IsTitle(strArray[0]) && strArray[0].Trim().Replace("\"", "").Length > 0)
                      gvItemList.Add(new GVItem(strArray[0].Trim().Replace("\"", "")));
                  }
                  else
                  {
                    for (int index = 0; index < strArray.Length; ++index)
                    {
                      if (!this.IsTitle(strArray[index]) && strArray[index].Trim().Replace("\"", "").Length > 0)
                        gvItemList.Add(new GVItem(strArray[index].Trim().Replace("\"", "")));
                    }
                  }
                }
                else if (this.FileType == FileImportType.LoanNumberContractNumberProductName)
                {
                  if (strArray.Length > 3 || strArray.Length == 0)
                  {
                    int num3 = (int) Utils.Dialog((IWin32Window) this, "Invalid File Format. Please import a CSV file of loan numbers, commitment contract numbers and product names for each loan separated by commas per row.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    return;
                  }
                  GVItem gvItem = new GVItem();
                  foreach (string str1 in strArray)
                  {
                    string str2 = str1.Trim().Replace("\"", "");
                    if (!this.IsTitle(str2))
                      gvItem.SubItems.Add((object) str2);
                    else
                      break;
                  }
                  gvItemList.Add(gvItem);
                }
                else if (this.FileType == FileImportType.LoanNumberPrice)
                {
                  if (strArray.Length > 2 || strArray.Length == 0)
                  {
                    int num4 = (int) Utils.Dialog((IWin32Window) this, "Invalid File Format. Please import a CSV file of loan numbers and total price for each loan separated by commas per row.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    return;
                  }
                  GVItem gvItem = new GVItem();
                  foreach (string str3 in strArray)
                  {
                    string str4 = str3.Trim().Replace("\"", "");
                    if (!this.IsTitle(str4))
                      gvItem.SubItems.Add((object) str4);
                    else
                      break;
                  }
                  gvItemList.Add(gvItem);
                }
              }
            }
            if (gvItemList.Count > 0)
            {
              this.gvLoans.Items.AddRange(gvItemList.ToArray());
              this.RefreshList();
              this.btnAssign.Enabled = true;
            }
            else
              this.btnAssign.Enabled = false;
          }
        }
        catch (Exception ex)
        {
          int num5 = (int) MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
        }
      }
    }

    private void siBtnNew_Click(object sender, EventArgs e)
    {
      MultiInputDialog multiInputDialog = new MultiInputDialog(this.FileType);
      if (multiInputDialog.ShowDialog((IWin32Window) this) == DialogResult.OK)
      {
        foreach (string str1 in multiInputDialog.ReturnValue)
        {
          char[] chArray = new char[1]{ ',' };
          string[] strArray = str1.Split(chArray);
          if (this.FileType == FileImportType.LoanNumberOnly)
          {
            foreach (string str2 in strArray)
            {
              if (str2.Trim().Length > 0)
                this.gvLoans.Items.Add(new GVItem()
                {
                  SubItems = {
                    (object) str2
                  }
                });
            }
          }
          else if (this.FileType == FileImportType.LoanNumberContractNumberProductName)
          {
            if (strArray.Length > 3 || strArray.Length == 0)
            {
              int num = (int) Utils.Dialog((IWin32Window) this, "Invalid Format. Enter loan numbers, commitment contract numbers, and product names separated by commas per row for each loan number.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
              return;
            }
            GVItem gvItem = new GVItem();
            foreach (string str3 in strArray)
            {
              string str4 = str3.Trim().Replace("\"", "");
              if (!this.IsTitle(str4))
                gvItem.SubItems.Add((object) str4);
              else
                break;
            }
            this.gvLoans.Items.Add(gvItem);
          }
          else if (this.FileType == FileImportType.LoanNumberPrice)
          {
            if (strArray.Length > 2 || strArray.Length == 0)
            {
              int num = (int) Utils.Dialog((IWin32Window) this, "Invalid Format. Enter loan numbers and prices separated by commas per row for each loan number.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
              return;
            }
            GVItem gvItem = new GVItem();
            foreach (string str5 in strArray)
            {
              string str6 = str5.Trim().Replace("\"", "");
              if (!this.IsTitle(str6))
                gvItem.SubItems.Add((object) str6);
              else
                break;
            }
            this.gvLoans.Items.Add(gvItem);
          }
        }
        this.RefreshList();
      }
      this.btnAssign.Enabled = this.gvLoans.Items.Count > 0;
    }

    private void gvLoans_EditorClosing(object source, GVSubItemEditingEventArgs e)
    {
      if (e == null)
        return;
      string text = e.EditorControl.Text;
      List<GVItem> gvItemList = new List<GVItem>();
      this.gvLoans.CancelEditing();
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvLoans.Items)
      {
        if (gvItem != e.SubItem.Item)
        {
          gvItemList.Add(gvItem);
        }
        else
        {
          string str1 = text;
          char[] separator = new char[2]{ '\r', '\n' };
          foreach (string str2 in str1.Split(separator, StringSplitOptions.RemoveEmptyEntries))
          {
            char[] chArray = new char[1]{ ',' };
            foreach (string str3 in str2.Split(chArray))
            {
              if (!string.IsNullOrEmpty(str3.Trim()))
                gvItemList.Add(new GVItem()
                {
                  SubItems = {
                    (object) str3.Trim()
                  }
                });
            }
          }
        }
      }
      this.gvLoans.Items.Clear();
      this.gvLoans.Items.AddRange(gvItemList.ToArray());
      this.RefreshList();
    }

    private void siBtnDelete_Click(object sender, EventArgs e)
    {
      this.gvLoans.CancelEditing();
      foreach (GVItem selectedItem in this.gvLoans.SelectedItems)
        this.gvLoans.Items.Remove(selectedItem);
      this.lblCount.Text = this.gvLoans.Items.Count<GVItem>().ToString() + " loans";
      this.btnAssign.Enabled = this.gvLoans.Items.Count > 0;
    }

    private void gvLoans_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.siBtnDelete.Enabled = this.gvLoans.SelectedItems.Count != 0;
    }

    private void RefreshList()
    {
      this.gvLoans.ReSort();
      this.lblCount.Text = this.gvLoans.Items.Count<GVItem>().ToString() + " loans";
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
      GVColumn gvColumn = new GVColumn();
      this.grpLoans = new TableContainer();
      this.lblFileName = new Label();
      this.gvLoans = new GridView();
      this.flpMenu = new FlowLayoutPanel();
      this.btnImport = new Button();
      this.siBtnDelete = new StandardIconButton();
      this.siBtnNew = new StandardIconButton();
      this.btnAssign = new Button();
      this.btnCancel = new Button();
      this.lblCount = new Label();
      this.toolTip1 = new ToolTip(this.components);
      this.label1 = new Label();
      this.grpLoans.SuspendLayout();
      this.flpMenu.SuspendLayout();
      ((ISupportInitialize) this.siBtnDelete).BeginInit();
      ((ISupportInitialize) this.siBtnNew).BeginInit();
      this.SuspendLayout();
      this.grpLoans.Controls.Add((Control) this.lblFileName);
      this.grpLoans.Controls.Add((Control) this.gvLoans);
      this.grpLoans.Controls.Add((Control) this.flpMenu);
      this.grpLoans.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.grpLoans.Location = new Point(9, 43);
      this.grpLoans.Margin = new Padding(0);
      this.grpLoans.Name = "grpLoans";
      this.grpLoans.Size = new Size(401, 361);
      this.grpLoans.Style = TableContainer.ContainerStyle.HeaderOnly;
      this.grpLoans.TabIndex = 4;
      this.lblFileName.AutoSize = true;
      this.lblFileName.Location = new Point(5, 6);
      this.lblFileName.Name = "lblFileName";
      this.lblFileName.Size = new Size(0, 14);
      this.lblFileName.TabIndex = 5;
      this.gvLoans.AllowColumnReorder = true;
      gvColumn.ActivatedEditorType = GVActivatedEditorType.TextArea;
      gvColumn.ImageIndex = -1;
      gvColumn.Name = "Column1";
      gvColumn.Text = "Loan Numbers";
      gvColumn.Width = 370;
      this.gvLoans.Columns.AddRange(new GVColumn[1]
      {
        gvColumn
      });
      this.gvLoans.HeaderHeight = 22;
      this.gvLoans.HotColumnTracking = true;
      this.gvLoans.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvLoans.Location = new Point(0, 26);
      this.gvLoans.Name = "gvLoans";
      this.gvLoans.Size = new Size(401, 332);
      this.gvLoans.TabIndex = 4;
      this.gvLoans.SelectedIndexChanged += new EventHandler(this.gvLoans_SelectedIndexChanged);
      this.gvLoans.EditorClosing += new GVSubItemEditingEventHandler(this.gvLoans_EditorClosing);
      this.flpMenu.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.flpMenu.BackColor = Color.Transparent;
      this.flpMenu.Controls.Add((Control) this.btnImport);
      this.flpMenu.Controls.Add((Control) this.siBtnDelete);
      this.flpMenu.Controls.Add((Control) this.siBtnNew);
      this.flpMenu.FlowDirection = FlowDirection.RightToLeft;
      this.flpMenu.Location = new Point(281, 2);
      this.flpMenu.Margin = new Padding(0);
      this.flpMenu.Name = "flpMenu";
      this.flpMenu.Size = new Size(119, 22);
      this.flpMenu.TabIndex = 3;
      this.btnImport.Location = new Point(55, 0);
      this.btnImport.Margin = new Padding(0);
      this.btnImport.Name = "btnImport";
      this.btnImport.Size = new Size(64, 23);
      this.btnImport.TabIndex = 2;
      this.btnImport.Text = "&Import";
      this.toolTip1.SetToolTip((Control) this.btnImport, "Import Loan Numbers From a CSV File");
      this.btnImport.UseVisualStyleBackColor = true;
      this.btnImport.Click += new EventHandler(this.btnImport_Click);
      this.siBtnDelete.BackColor = Color.Transparent;
      this.siBtnDelete.Location = new Point(37, 4);
      this.siBtnDelete.Margin = new Padding(3, 4, 2, 3);
      this.siBtnDelete.MouseDownImage = (Image) null;
      this.siBtnDelete.Name = "siBtnDelete";
      this.siBtnDelete.Size = new Size(16, 16);
      this.siBtnDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.siBtnDelete.TabIndex = 12;
      this.siBtnDelete.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.siBtnDelete, "Delete Loan Number");
      this.siBtnDelete.Click += new EventHandler(this.siBtnDelete_Click);
      this.siBtnNew.BackColor = Color.Transparent;
      this.siBtnNew.Location = new Point(16, 4);
      this.siBtnNew.Margin = new Padding(3, 4, 2, 3);
      this.siBtnNew.MouseDownImage = (Image) null;
      this.siBtnNew.Name = "siBtnNew";
      this.siBtnNew.Size = new Size(16, 16);
      this.siBtnNew.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.siBtnNew.TabIndex = 10;
      this.siBtnNew.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.siBtnNew, "Add Loan Number");
      this.siBtnNew.Click += new EventHandler(this.siBtnNew_Click);
      this.btnAssign.DialogResult = DialogResult.OK;
      this.btnAssign.Location = new Point(258, 412);
      this.btnAssign.Margin = new Padding(0);
      this.btnAssign.Name = "btnAssign";
      this.btnAssign.Size = new Size(85, 23);
      this.btnAssign.TabIndex = 0;
      this.btnAssign.Text = "&Assign Loans";
      this.btnAssign.UseVisualStyleBackColor = true;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(347, 412);
      this.btnCancel.Margin = new Padding(0);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(65, 23);
      this.btnCancel.TabIndex = 5;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.lblCount.AutoSize = true;
      this.lblCount.Location = new Point(14, 417);
      this.lblCount.Name = "lblCount";
      this.lblCount.Size = new Size(0, 13);
      this.lblCount.TabIndex = 6;
      this.label1.Location = new Point(9, 9);
      this.label1.Name = "label1";
      this.label1.Size = new Size(402, 34);
      this.label1.TabIndex = 7;
      this.label1.Text = "Import a single column CSV file of loans or manually enter loan numbers (Separated by commas or line break).";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(420, 442);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.lblCount);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnAssign);
      this.Controls.Add((Control) this.grpLoans);
      this.FormBorderStyle = FormBorderStyle.FixedSingle;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (LoanAssignmentFromFileImport);
      this.ShowIcon = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Assign Loans from a File/Manual Input";
      this.FormClosing += new FormClosingEventHandler(this.LoanAssignmentFromFileImport_Closing);
      this.grpLoans.ResumeLayout(false);
      this.grpLoans.PerformLayout();
      this.flpMenu.ResumeLayout(false);
      ((ISupportInitialize) this.siBtnDelete).EndInit();
      ((ISupportInitialize) this.siBtnNew).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
