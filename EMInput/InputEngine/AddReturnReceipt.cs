// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.AddReturnReceipt
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class AddReturnReceipt : Form
  {
    private Sessions.Session session;
    private DocTrackingType docTrackingType;
    private LoanData loanData;
    private Hashtable docTypeOvrdAsked;
    private IContainer components;
    private Label label_SelectCheckBoxItem;
    private Panel pnlDocType;
    private CheckBox cbFtp;
    private CheckBox cbDot;
    private DatePickerCustom dpRcvdDt;
    private Label label2;
    private Label label3;
    private TextBox tbRcvdBy;
    private RichTextBox rtbComments;
    private Label label4;
    private Button btnOk;
    private Button btnCancel;

    public AddReturnReceipt(DocTrackingType docTrackingType, Sessions.Session session)
    {
      this.InitializeComponent();
      this.docTrackingType = docTrackingType;
      this.session = session;
      this.loanData = session.LoanData;
      this.InitData();
    }

    public AddReturnReceipt(DocumentTrackingLog dtl)
    {
      this.InitializeComponent();
      this.EnableDisableAddRtnRcvd(false);
      this.btnOk.Visible = false;
      this.btnCancel.Text = "Close";
      this.InitDataFromLog(dtl);
    }

    private void checkBox_Click(object sender, EventArgs e)
    {
      CheckBox checkBox = (CheckBox) sender;
      if (!checkBox.Checked)
        return;
      string key = checkBox.Tag.ToString();
      string field = this.loanData.GetField(string.Format(DocTrackingUtils.FieldPrefix + "{0}.ReturnRequest.Received", (object) key.ToString()));
      if (key.Equals((object) this.docTrackingType) || !checkBox.Checked || Utils.ParseBoolean(this.docTypeOvrdAsked[(object) key]) || field.Equals("//"))
        return;
      if (Utils.Dialog((IWin32Window) this, DocTrackingUtils.ReturnReceipt_AddReceipt_Override, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
      {
        checkBox.Checked = true;
        this.docTypeOvrdAsked[(object) key] = (object) true;
      }
      else
        checkBox.Checked = false;
    }

    private void InitData()
    {
      this.docTypeOvrdAsked = new Hashtable();
      foreach (CheckBox control in (ArrangedElementCollection) this.pnlDocType.Controls)
      {
        string key = control.Tag.ToString();
        if (key != this.docTrackingType.ToString())
        {
          this.docTypeOvrdAsked[(object) key] = (object) false;
          control.Checked = false;
          control.Enabled = Utils.ParseBoolean(DocTrackingUtils.DocTrackingSettings[(object) ("Enable" + key)]);
        }
        else
        {
          this.docTypeOvrdAsked[(object) key] = (object) true;
          control.Checked = true;
          control.Enabled = false;
        }
      }
      this.dpRcvdDt.Text = DateTime.Now.ToString("MM/dd/yyy");
      this.tbRcvdBy.Text = this.session.UserInfo.FullName;
    }

    private void btnOk_Click(object sender, EventArgs e)
    {
      if (!this.validateData())
      {
        int num = (int) Utils.Dialog((IWin32Window) this, DocTrackingUtils.Validate_Error_message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        this.DialogResult = DialogResult.OK;
        this.setLoanDataField((Control) this.dpRcvdDt, this.dpRcvdDt.Text);
        foreach (CheckBox control in (ArrangedElementCollection) this.pnlDocType.Controls)
        {
          if (control.Checked)
            this.session.LoanData.SetField(string.Format(DocTrackingUtils.FieldPrefix + "{0}.ReturnRequest.{1}", control.Tag, (object) "NextFollowUpDate"), "");
        }
        if (!string.IsNullOrEmpty(this.rtbComments.Text))
          DocTrackingUtils.SaveComments(new DocumentTrackingComment()
          {
            UserName = this.session.UserInfo.FullName,
            LogDate = DateTime.Now,
            CommentText = this.rtbComments.Text
          });
        this.loanData.GetLogList().AddRecord((LogRecordBase) new DocumentTrackingLog(DateTime.Now)
        {
          ActionCd = DocTrackingActionCd.ActionReturnReceived,
          Action = DocTrackingUtils.Action_Return_Received,
          LogDate = this.dpRcvdDt.Text.Trim(),
          LogBy = this.tbRcvdBy.Text.Trim(),
          Dot = this.cbDot.Checked,
          Ftp = this.cbFtp.Checked,
          DocTrackingSnapshot = new Hashtable()
          {
            {
              (object) "Comments",
              (object) this.rtbComments.Text
            }
          }
        });
        this.Close();
      }
    }

    private bool validateData()
    {
      bool flag1 = true;
      bool flag2 = false;
      foreach (CheckBox control in (ArrangedElementCollection) this.pnlDocType.Controls)
      {
        if (control.Checked)
        {
          flag2 = true;
          break;
        }
      }
      if (!flag2)
      {
        foreach (Control control in (ArrangedElementCollection) this.pnlDocType.Controls)
          DocTrackingUtils.HilightFields(control, true);
        flag1 = false;
      }
      else
      {
        foreach (Control control in (ArrangedElementCollection) this.pnlDocType.Controls)
          DocTrackingUtils.HilightFields(control, false);
      }
      DocTrackingUtils.HilightFields((Control) this.dpRcvdDt, false);
      if (string.IsNullOrEmpty(this.dpRcvdDt.Text.Trim()))
      {
        DocTrackingUtils.HilightFields((Control) this.dpRcvdDt, true);
        flag1 = false;
      }
      this.dpRcvdDt.Refresh();
      DocTrackingUtils.HilightFields((Control) this.tbRcvdBy, false);
      if (string.IsNullOrEmpty(this.tbRcvdBy.Text.Trim()))
      {
        DocTrackingUtils.HilightFields((Control) this.tbRcvdBy, true);
        flag1 = false;
      }
      return flag1;
    }

    private void setLoanDataField(Control ctrl, string value)
    {
      foreach (CheckBox control in (ArrangedElementCollection) this.pnlDocType.Controls)
      {
        if (control.Checked)
          this.loanData.SetField(DocTrackingUtils.GetFieldId(DocTrackingUtils.FieldPrefix + control.Tag + ".ReturnRequest.", ctrl), value);
      }
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
      this.Close();
    }

    private void EnableDisableAddRtnRcvd(bool isEnable)
    {
      this.cbDot.Enabled = isEnable;
      this.cbFtp.Enabled = isEnable;
      this.dpRcvdDt.Enabled = isEnable;
      this.tbRcvdBy.Enabled = isEnable;
      this.rtbComments.Enabled = isEnable;
    }

    private void InitDataFromLog(DocumentTrackingLog dtl)
    {
      this.cbDot.Checked = dtl.Dot;
      this.cbFtp.Checked = dtl.Ftp;
      this.dpRcvdDt.Text = dtl.LogDate;
      this.tbRcvdBy.Text = dtl.LogBy;
      this.rtbComments.Text = dtl.DocTrackingSnapshot == null ? "" : (string) dtl.DocTrackingSnapshot[(object) "Comments"];
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.label_SelectCheckBoxItem = new Label();
      this.pnlDocType = new Panel();
      this.cbFtp = new CheckBox();
      this.cbDot = new CheckBox();
      this.dpRcvdDt = new DatePickerCustom();
      this.label2 = new Label();
      this.label3 = new Label();
      this.tbRcvdBy = new TextBox();
      this.rtbComments = new RichTextBox();
      this.label4 = new Label();
      this.btnOk = new Button();
      this.btnCancel = new Button();
      this.pnlDocType.SuspendLayout();
      this.SuspendLayout();
      this.label_SelectCheckBoxItem.AutoSize = true;
      this.label_SelectCheckBoxItem.Location = new Point(28, 19);
      this.label_SelectCheckBoxItem.Name = "label_SelectCheckBoxItem";
      this.label_SelectCheckBoxItem.Size = new Size(152, 13);
      this.label_SelectCheckBoxItem.TabIndex = 0;
      this.label_SelectCheckBoxItem.Text = "Select the items you requested";
      this.pnlDocType.Controls.Add((Control) this.cbFtp);
      this.pnlDocType.Controls.Add((Control) this.cbDot);
      this.pnlDocType.Location = new Point(14, 35);
      this.pnlDocType.Name = "pnlDocType";
      this.pnlDocType.Size = new Size(235, 68);
      this.pnlDocType.TabIndex = 1;
      this.cbFtp.AutoSize = true;
      this.cbFtp.Location = new Point(17, 37);
      this.cbFtp.Name = "cbFtp";
      this.cbFtp.Size = new Size(102, 17);
      this.cbFtp.TabIndex = 1;
      this.cbFtp.Tag = (object) "FTP";
      this.cbFtp.Text = "Final Title Policy";
      this.cbFtp.UseVisualStyleBackColor = true;
      this.cbFtp.Click += new EventHandler(this.checkBox_Click);
      this.cbDot.AutoSize = true;
      this.cbDot.Location = new Point(17, 13);
      this.cbDot.Name = "cbDot";
      this.cbDot.Size = new Size(99, 17);
      this.cbDot.TabIndex = 0;
      this.cbDot.Tag = (object) "DOT";
      this.cbDot.Text = "DOT/Mortgage";
      this.cbDot.UseVisualStyleBackColor = true;
      this.cbDot.Click += new EventHandler(this.checkBox_Click);
      this.dpRcvdDt.Location = new Point(130, 119);
      this.dpRcvdDt.MaxValue = new DateTime(2199, 12, 31, 0, 0, 0, 0);
      this.dpRcvdDt.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dpRcvdDt.Name = "dpRcvdDt";
      this.dpRcvdDt.Size = new Size(113, 21);
      this.dpRcvdDt.TabIndex = 2;
      this.dpRcvdDt.Tag = (object) "Received";
      this.dpRcvdDt.ToolTip = "";
      this.dpRcvdDt.Value = new DateTime(0L);
      this.label2.AutoSize = true;
      this.label2.Location = new Point(31, 125);
      this.label2.Name = "label2";
      this.label2.Size = new Size(79, 13);
      this.label2.TabIndex = 3;
      this.label2.Text = "Received Date";
      this.label3.AutoSize = true;
      this.label3.Location = new Point(31, 154);
      this.label3.Name = "label3";
      this.label3.Size = new Size(67, 13);
      this.label3.TabIndex = 4;
      this.label3.Text = "Received by";
      this.tbRcvdBy.Location = new Point(130, 151);
      this.tbRcvdBy.Name = "tbRcvdBy";
      this.tbRcvdBy.Size = new Size(202, 20);
      this.tbRcvdBy.TabIndex = 5;
      this.tbRcvdBy.Tag = (object) "ReceivedBy";
      this.rtbComments.Location = new Point(34, 222);
      this.rtbComments.MaxLength = 500;
      this.rtbComments.Name = "rtbComments";
      this.rtbComments.Size = new Size(298, 96);
      this.rtbComments.TabIndex = 6;
      this.rtbComments.Tag = (object) "";
      this.rtbComments.Text = "";
      this.label4.AutoSize = true;
      this.label4.Location = new Point(31, 199);
      this.label4.Name = "label4";
      this.label4.Size = new Size(56, 13);
      this.label4.TabIndex = 7;
      this.label4.Text = "Comments";
      this.btnOk.Location = new Point(174, 345);
      this.btnOk.Name = "btnOk";
      this.btnOk.Size = new Size(75, 23);
      this.btnOk.TabIndex = 8;
      this.btnOk.Text = "OK";
      this.btnOk.UseVisualStyleBackColor = true;
      this.btnOk.Click += new EventHandler(this.btnOk_Click);
      this.btnCancel.Location = new Point(259, 345);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 9;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(367, 388);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnOk);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.rtbComments);
      this.Controls.Add((Control) this.tbRcvdBy);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.dpRcvdDt);
      this.Controls.Add((Control) this.pnlDocType);
      this.Controls.Add((Control) this.label_SelectCheckBoxItem);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (AddReturnReceipt);
      this.ShowIcon = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Add Return Receipt";
      this.pnlDocType.ResumeLayout(false);
      this.pnlDocType.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
