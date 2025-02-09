// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.CustomLetters.AuditFields
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.RemotingServices;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.CustomLetters
{
  public class AuditFields : Form
  {
    private string selectedField = "";
    private IContainer components;
    private Label label1;
    private ListView lsvAuditFields;
    private Button btnOK;
    private Button btnCancel;
    private ColumnHeader columnHeader1;

    public AuditFields()
    {
      this.InitializeComponent();
      this.initialPageValue();
    }

    public string SelectedField => this.selectedField;

    private void initialPageValue()
    {
      this.lsvAuditFields.Items.Clear();
      this.lsvAuditFields.BeginUpdate();
      foreach (LoanXDBField loanXdbField in Session.LoanManager.GetAuditTrailLoanXDBField())
        this.lsvAuditFields.Items.Add(new ListViewItem(loanXdbField.Description)
        {
          Tag = (object) loanXdbField.FieldIDWithCoMortgagor.Replace("*", "")
        });
      this.lsvAuditFields.EndUpdate();
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      if (this.lsvAuditFields.SelectedItems.Count == 0)
      {
        this.btnCancel_Click((object) null, (EventArgs) null);
      }
      else
      {
        this.selectedField = string.Concat(this.lsvAuditFields.SelectedItems[0].Tag);
        this.DialogResult = DialogResult.OK;
      }
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
    }

    private void lsvAuditFields_DoubleClick(object sender, EventArgs e)
    {
      this.btnOK_Click((object) null, (EventArgs) null);
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
      this.lsvAuditFields = new ListView();
      this.columnHeader1 = new ColumnHeader();
      this.btnOK = new Button();
      this.btnCancel = new Button();
      this.SuspendLayout();
      this.label1.AutoSize = true;
      this.label1.Location = new Point(9, 9);
      this.label1.Name = "label1";
      this.label1.Size = new Size(133, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Please select an audit field";
      this.lsvAuditFields.Columns.AddRange(new ColumnHeader[1]
      {
        this.columnHeader1
      });
      this.lsvAuditFields.FullRowSelect = true;
      this.lsvAuditFields.GridLines = true;
      this.lsvAuditFields.HeaderStyle = ColumnHeaderStyle.None;
      this.lsvAuditFields.HideSelection = false;
      this.lsvAuditFields.Location = new Point(12, 26);
      this.lsvAuditFields.MultiSelect = false;
      this.lsvAuditFields.Name = "lsvAuditFields";
      this.lsvAuditFields.Size = new Size(232, 297);
      this.lsvAuditFields.TabIndex = 1;
      this.lsvAuditFields.UseCompatibleStateImageBehavior = false;
      this.lsvAuditFields.View = View.Details;
      this.lsvAuditFields.DoubleClick += new EventHandler(this.lsvAuditFields_DoubleClick);
      this.columnHeader1.Text = "Audit Field";
      this.columnHeader1.Width = 210;
      this.btnOK.Location = new Point(88, 330);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 2;
      this.btnOK.Text = "Insert";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.btnCancel.Location = new Point(169, 330);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 3;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(256, 357);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.lsvAuditFields);
      this.Controls.Add((Control) this.label1);
      this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
      this.Name = nameof (AuditFields);
      this.Text = " Audit Fields";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
