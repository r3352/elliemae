// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.MainUI.FindAuditTrailField
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.MainUI
{
  public class FindAuditTrailField : Form
  {
    private LoanXDBField selectedField;
    private IContainer components;
    private Button findBtn;
    private TextBox descTxt;
    private GridView gvFields;
    private DialogButtons dlgButtons;

    public FindAuditTrailField(LoanXDBField[] dbFields, string selectedFieldID)
    {
      this.InitializeComponent();
      this.initializeDBListView(dbFields);
      if (!(selectedFieldID != ""))
        return;
      this.descTxt.Text = selectedFieldID;
      this.findBtn_Click((object) null, (EventArgs) null);
    }

    public LoanXDBField SelectedField => this.selectedField;

    private void initializeDBListView(LoanXDBField[] dbFields)
    {
      this.gvFields.Items.Clear();
      this.gvFields.BeginUpdate();
      for (int index = 0; index < dbFields.Length; ++index)
      {
        GVItem gvItem = new GVItem(dbFields[index].Description);
        gvItem.SubItems.Add((object) dbFields[index].FieldIDWithCoMortgagor);
        if (dbFields[index].ComortgagorPair > 1)
          gvItem.SubItems.Add((object) string.Concat((object) dbFields[index].ComortgagorPair));
        else
          gvItem.SubItems.Add((object) "");
        gvItem.Tag = (object) dbFields[index];
        this.gvFields.Items.Add(gvItem);
      }
      this.gvFields.EndUpdate();
    }

    private void okBtn_Click(object sender, EventArgs e)
    {
      if (this.gvFields.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must select a field.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        this.selectedField = (LoanXDBField) this.gvFields.SelectedItems[0].Tag;
        this.DialogResult = DialogResult.OK;
      }
    }

    private void findBtn_Click(object sender, EventArgs e)
    {
      if (this.descTxt.Text.Trim() == string.Empty)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "You must enter a keyword in the Find field.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        this.findBtn.Text = "&Find Next";
        int num2 = 0;
        if (this.gvFields.SelectedItems.Count > 0)
          num2 = this.gvFields.SelectedItems[0].Index;
        string upper = this.descTxt.Text.Trim().ToUpper();
        int num3;
        int num4 = num3 = num2 + 1;
        for (int index = 0; index < this.gvFields.Items.Count; ++index)
        {
          if (num4 + 1 > this.gvFields.Items.Count)
            num4 = 0;
          if (this.gvFields.Items[num4].SubItems[0].Text.Trim().ToUpper().ToLower().IndexOf(upper.ToLower()) > -1)
          {
            this.gvFields.Items[num4].Selected = true;
            this.gvFields.EnsureVisible(num4);
            this.descTxt.Focus();
            return;
          }
          LoanXDBField tag = (LoanXDBField) this.gvFields.Items[num4].Tag;
          if (upper.IndexOf("#") < 0)
          {
            if (string.Compare(tag.FieldIDWithCoMortgagor, upper, StringComparison.OrdinalIgnoreCase) == 0)
            {
              this.gvFields.Items[num4].Selected = true;
              this.gvFields.EnsureVisible(num4);
              this.descTxt.Focus();
              return;
            }
          }
          else if (tag.FieldIDWithCoMortgagor.ToLower().IndexOf(upper.ToLower()) > -1)
          {
            this.gvFields.Items[num4].Selected = true;
            this.gvFields.EnsureVisible(num4);
            this.descTxt.Focus();
            return;
          }
          ++num4;
        }
        int num5 = (int) Utils.Dialog((IWin32Window) this, "There are no results that match your search criteria.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
    }

    private void descTxt_TextChanged(object sender, EventArgs e) => this.findBtn.Text = "&Find";

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
      GVColumn gvColumn3 = new GVColumn();
      this.findBtn = new Button();
      this.descTxt = new TextBox();
      this.gvFields = new GridView();
      this.dlgButtons = new DialogButtons();
      this.SuspendLayout();
      this.findBtn.Location = new Point(10, 10);
      this.findBtn.Name = "findBtn";
      this.findBtn.Size = new Size(75, 22);
      this.findBtn.TabIndex = 6;
      this.findBtn.Text = "&Find";
      this.findBtn.Click += new EventHandler(this.findBtn_Click);
      this.descTxt.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.descTxt.Location = new Point(94, 11);
      this.descTxt.Name = "descTxt";
      this.descTxt.Size = new Size(407, 20);
      this.descTxt.TabIndex = 5;
      this.descTxt.TextChanged += new EventHandler(this.descTxt_TextChanged);
      this.gvFields.AllowMultiselect = false;
      this.gvFields.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.SpringToFit = true;
      gvColumn1.Text = "Description";
      gvColumn1.Width = 250;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Field ID";
      gvColumn2.Width = 136;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.Text = "Borrower Pair";
      gvColumn3.Width = 103;
      this.gvFields.Columns.AddRange(new GVColumn[3]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3
      });
      this.gvFields.Location = new Point(10, 41);
      this.gvFields.Name = "gvFields";
      this.gvFields.Size = new Size(491, 311);
      this.gvFields.TabIndex = 7;
      this.gvFields.DoubleClick += new EventHandler(this.okBtn_Click);
      this.dlgButtons.Dock = DockStyle.Bottom;
      this.dlgButtons.Location = new Point(0, 365);
      this.dlgButtons.Name = "dlgButtons";
      this.dlgButtons.OKText = "&Select";
      this.dlgButtons.Size = new Size(520, 44);
      this.dlgButtons.TabIndex = 8;
      this.dlgButtons.OK += new EventHandler(this.okBtn_Click);
      this.AcceptButton = (IButtonControl) this.dlgButtons;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(520, 409);
      this.Controls.Add((Control) this.dlgButtons);
      this.Controls.Add((Control) this.findBtn);
      this.Controls.Add((Control) this.descTxt);
      this.Controls.Add((Control) this.gvFields);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (FindAuditTrailField);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.Text = "Find Field";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
