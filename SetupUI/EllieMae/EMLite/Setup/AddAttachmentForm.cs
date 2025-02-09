// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.AddAttachmentForm
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Properties;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class AddAttachmentForm : Form
  {
    private bool edit;
    private OpenFileDialog openFileDialog1 = new OpenFileDialog();
    private ExternalOrgAttachments attachment;
    private int oid;
    private string userID;
    private Guid guid = Guid.NewGuid();
    private string realFileName = "";
    public bool AttachmentChanged;
    private IContainer components;
    private Button btnOK;
    private Button btnCancel;
    private Label label1;
    private Label label2;
    private Label label3;
    private Label label4;
    private Label label5;
    private Label label6;
    private TextBox txtFileName;
    private TextBox txtDescription;
    private ComboBox cboCategory;
    private DatePicker datAttachment;
    private DatePicker datExpiration;
    private TextBox txtDays;
    private CheckBox chkAlert;
    private IconButton btnAttachBrowse;
    private PictureBox picWarning;
    private Panel panelWarning;
    private Label label7;
    private Label lblFile;
    private TextBox txtFilePath;
    private Label lblReq1;
    private Label label8;

    public AddAttachmentForm(
      ExternalOrgAttachments attachment,
      int oid,
      string userID,
      List<ExternalSettingValue> settings)
    {
      this.InitializeComponent();
      this.edit = attachment != null;
      this.oid = oid;
      this.userID = userID;
      this.attachment = attachment;
      if (this.edit)
        this.guid = attachment.Guid;
      this.cboCategory.DataSource = (object) settings;
      this.cboCategory.DisplayMember = "settingValue";
      this.cboCategory.ValueMember = "settingId";
      if (!this.edit)
        return;
      this.populateFields();
    }

    private void populateFields()
    {
      this.txtFilePath.Text = this.attachment.RealFileName;
      this.txtFileName.Text = this.attachment.FileName;
      this.txtDescription.Text = this.attachment.Description;
      this.cboCategory.SelectedValue = (object) this.attachment.Category;
      this.datAttachment.Value = this.attachment.FileDate;
      this.datExpiration.Value = this.attachment.ExpirationDate;
      this.datExpiration_ValueChanged((object) null, (EventArgs) null);
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      if (!this.validateValue())
        return;
      int category = this.cboCategory.SelectedIndex <= 0 ? -1 : ((ExternalSettingValue) this.cboCategory.SelectedItem).settingId;
      if (this.attachment == null)
      {
        this.attachment = new ExternalOrgAttachments(this.guid, this.oid, this.txtFileName.Text.Trim(), this.txtDescription.Text.Trim(), DateTime.Now, category, this.datAttachment.Value, this.userID, this.datExpiration.Value, this.txtDays.Text.Trim() == "" ? 0 : Convert.ToInt32(this.txtDays.Text.Trim()), this.realFileName);
      }
      else
      {
        this.attachment.FileName = this.txtFileName.Text.Trim();
        this.attachment.Description = this.txtDescription.Text.Trim();
        this.attachment.DateAdded = DateTime.Now;
        this.attachment.Category = category;
        this.attachment.FileDate = this.datAttachment.Value;
        this.attachment.UserWhoAdded = this.userID;
        this.attachment.ExpirationDate = this.datExpiration.Value;
        this.attachment.DaysToExpire = this.txtDays.Text.Trim() == "" ? 0 : Convert.ToInt32(this.txtDays.Text.Trim());
        if (this.realFileName != "")
          this.attachment.RealFileName = this.realFileName;
      }
      this.DialogResult = DialogResult.OK;
    }

    private bool validateValue()
    {
      if (this.txtFilePath.Text.Trim() == "")
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The file has not been specified.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.btnAttachBrowse_Click((object) null, (EventArgs) null);
        return false;
      }
      if (this.txtFileName.Text.Trim() == "")
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "File Name cannot be blank.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.txtFileName.Focus();
        return false;
      }
      if (this.datAttachment.Value < DateTime.MinValue || this.datAttachment.Value > DateTime.MaxValue)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Invalid data for File date.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.datAttachment.Focus();
        return false;
      }
      if (!(this.datExpiration.Value < DateTime.MinValue) && !(this.datExpiration.Value > DateTime.MaxValue))
        return true;
      int num1 = (int) Utils.Dialog((IWin32Window) this, "Invalid data for Expiration date.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      this.datExpiration.Focus();
      return false;
    }

    private void btnAttachBrowse_Click(object sender, EventArgs e)
    {
      if (this.edit && Utils.Dialog((IWin32Window) this, "If you select another source file, the original file will be overwritten. Continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No || this.openFileDialog1.ShowDialog((IWin32Window) this) == DialogResult.Cancel)
        return;
      if (this.openFileDialog1.FileName.LastIndexOf('.') < 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "This file does not have an extension. Please select another file.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        string fileName = this.openFileDialog1.FileName;
        string str = "";
        try
        {
          str = fileName.Substring(fileName.LastIndexOf('.') + 1, 3);
        }
        catch
        {
        }
        if (str != "" && (str == "exe" || str == "dll" || str == "msi"))
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this, "This file extension is not supported. Please select another file.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else if (new FileInfo(this.openFileDialog1.FileName).Length > 25000000L)
        {
          int num3 = (int) Utils.Dialog((IWin32Window) this, "File attachments cannot exceed 25 MB. Please select another file.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else
          this.setFileName(this.openFileDialog1.FileName);
      }
    }

    private void setFileName(string path)
    {
      this.txtFilePath.Text = path;
      if (this.txtFileName.Text.Trim() == "")
      {
        string str = ((IEnumerable<string>) this.txtFilePath.Text.Trim().Split('\\')).Last<string>();
        this.txtFileName.Text = str.Substring(0, str.LastIndexOf('.'));
      }
      this.realFileName = this.txtFilePath.Text.Trim();
      this.AttachmentChanged = true;
    }

    private void setRealPath(string path) => this.setFileName(path);

    private string getFileName() => this.txtFileName.Text.Trim();

    private void txtFileName_TextChanged(object sender, EventArgs e)
    {
      this.btnOK.Enabled = this.txtFileName.Text.Trim() != string.Empty;
    }

    private void txtDays_TextChanged(object sender, EventArgs e)
    {
      this.panelWarning.Visible = this.txtDays.Text != string.Empty && Utils.ParseInt((object) this.txtDays.Text) < 0;
    }

    private void datExpiration_ValueChanged(object sender, EventArgs e)
    {
      if (this.datExpiration.Text != string.Empty)
      {
        DateTime date1 = DateTime.Now.Date;
        DateTime dateTime = Utils.ParseDate((object) this.datExpiration.Text);
        DateTime date2 = dateTime.Date;
        int num;
        if (date1 == date2)
        {
          num = 0;
        }
        else
        {
          dateTime = DateTime.Now;
          DateTime date3 = dateTime.Date;
          dateTime = Utils.ParseDate((object) this.datExpiration.Text);
          DateTime date4 = dateTime.Date;
          num = Utils.GetTotalTimeSpanDays(date3, date4);
        }
        this.txtDays.Text = string.Concat((object) num);
      }
      else
        this.txtDays.Text = string.Empty;
    }

    public string Path => this.txtFilePath.Text;

    public string FileName
    {
      get
      {
        __Boxed<Guid> guid = (ValueType) this.guid;
        string str = ((IEnumerable<string>) this.txtFilePath.Text.Trim().Split('.')).Last<string>();
        return guid.ToString() + "." + str;
      }
    }

    public ExternalOrgAttachments Attachment => this.attachment;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.btnOK = new Button();
      this.btnCancel = new Button();
      this.label1 = new Label();
      this.label2 = new Label();
      this.label3 = new Label();
      this.label4 = new Label();
      this.label5 = new Label();
      this.label6 = new Label();
      this.txtFileName = new TextBox();
      this.txtDescription = new TextBox();
      this.cboCategory = new ComboBox();
      this.datAttachment = new DatePicker();
      this.datExpiration = new DatePicker();
      this.txtDays = new TextBox();
      this.chkAlert = new CheckBox();
      this.btnAttachBrowse = new IconButton();
      this.picWarning = new PictureBox();
      this.panelWarning = new Panel();
      this.label7 = new Label();
      this.lblFile = new Label();
      this.txtFilePath = new TextBox();
      this.lblReq1 = new Label();
      this.label8 = new Label();
      ((ISupportInitialize) this.btnAttachBrowse).BeginInit();
      ((ISupportInitialize) this.picWarning).BeginInit();
      this.panelWarning.SuspendLayout();
      this.SuspendLayout();
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.Location = new Point(391, 220);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 7;
      this.btnOK.Text = "&OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(472, 220);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 8;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(13, 37);
      this.label1.Name = "label1";
      this.label1.Size = new Size(92, 13);
      this.label1.TabIndex = 4;
      this.label1.Text = "Attachment Name";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(13, 60);
      this.label2.Name = "label2";
      this.label2.Size = new Size(60, 13);
      this.label2.TabIndex = 5;
      this.label2.Text = "Description";
      this.label3.AutoSize = true;
      this.label3.Location = new Point(13, 132);
      this.label3.Name = "label3";
      this.label3.Size = new Size(49, 13);
      this.label3.TabIndex = 6;
      this.label3.Text = "Category";
      this.label4.AutoSize = true;
      this.label4.Location = new Point(13, 158);
      this.label4.Name = "label4";
      this.label4.Size = new Size(49, 13);
      this.label4.TabIndex = 7;
      this.label4.Text = "File Date";
      this.label5.AutoSize = true;
      this.label5.Location = new Point(13, 180);
      this.label5.Name = "label5";
      this.label5.Size = new Size(79, 13);
      this.label5.TabIndex = 8;
      this.label5.Text = "Expiration Date";
      this.label6.AutoSize = true;
      this.label6.Location = new Point(13, 207);
      this.label6.Name = "label6";
      this.label6.Size = new Size(75, 13);
      this.label6.TabIndex = 9;
      this.label6.Text = "Days to Expire";
      this.txtFileName.Location = new Point(115, 34);
      this.txtFileName.MaxLength = 250;
      this.txtFileName.Name = "txtFileName";
      this.txtFileName.Size = new Size(431, 20);
      this.txtFileName.TabIndex = 0;
      this.txtFileName.TextChanged += new EventHandler(this.txtFileName_TextChanged);
      this.txtDescription.Location = new Point(115, 57);
      this.txtDescription.Multiline = true;
      this.txtDescription.Name = "txtDescription";
      this.txtDescription.ScrollBars = ScrollBars.Both;
      this.txtDescription.Size = new Size(432, 68);
      this.txtDescription.TabIndex = 1;
      this.cboCategory.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboCategory.FormattingEnabled = true;
      this.cboCategory.Location = new Point(115, 129);
      this.cboCategory.Name = "cboCategory";
      this.cboCategory.Size = new Size(432, 21);
      this.cboCategory.TabIndex = 2;
      this.datAttachment.BackColor = SystemColors.Window;
      this.datAttachment.Location = new Point(115, 155);
      this.datAttachment.MaxValue = new DateTime(2079, 6, 6, 0, 0, 0, 0);
      this.datAttachment.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.datAttachment.Name = "datAttachment";
      this.datAttachment.Size = new Size(85, 21);
      this.datAttachment.TabIndex = 3;
      this.datAttachment.Tag = (object) "763";
      this.datAttachment.ToolTip = "";
      this.datAttachment.Value = new DateTime(0L);
      this.datExpiration.BackColor = SystemColors.Window;
      this.datExpiration.Location = new Point(115, 180);
      this.datExpiration.MaxValue = new DateTime(2079, 6, 6, 0, 0, 0, 0);
      this.datExpiration.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.datExpiration.Name = "datExpiration";
      this.datExpiration.Size = new Size(85, 21);
      this.datExpiration.TabIndex = 4;
      this.datExpiration.Tag = (object) "763";
      this.datExpiration.ToolTip = "";
      this.datExpiration.Value = new DateTime(0L);
      this.datExpiration.ValueChanged += new EventHandler(this.datExpiration_ValueChanged);
      this.txtDays.Location = new Point(115, 207);
      this.txtDays.Name = "txtDays";
      this.txtDays.ReadOnly = true;
      this.txtDays.Size = new Size(85, 20);
      this.txtDays.TabIndex = 6;
      this.txtDays.TabStop = false;
      this.txtDays.TextChanged += new EventHandler(this.txtDays_TextChanged);
      this.chkAlert.AutoSize = true;
      this.chkAlert.Location = new Point(206, 184);
      this.chkAlert.Name = "chkAlert";
      this.chkAlert.Size = new Size(94, 17);
      this.chkAlert.TabIndex = 5;
      this.chkAlert.Text = "Generate Alert";
      this.chkAlert.UseVisualStyleBackColor = true;
      this.chkAlert.Visible = false;
      this.btnAttachBrowse.BackColor = Color.Transparent;
      this.btnAttachBrowse.DisabledImage = (Image) null;
      this.btnAttachBrowse.Image = (Image) Resources.attach_browse;
      this.btnAttachBrowse.Location = new Point(530, 14);
      this.btnAttachBrowse.Margin = new Padding(4, 3, 0, 3);
      this.btnAttachBrowse.MouseDownImage = (Image) null;
      this.btnAttachBrowse.MouseOverImage = (Image) null;
      this.btnAttachBrowse.Name = "btnAttachBrowse";
      this.btnAttachBrowse.Size = new Size(16, 16);
      this.btnAttachBrowse.TabIndex = 48;
      this.btnAttachBrowse.TabStop = false;
      this.btnAttachBrowse.Click += new EventHandler(this.btnAttachBrowse_Click);
      this.picWarning.Image = (Image) Resources.alert;
      this.picWarning.Location = new Point(0, 2);
      this.picWarning.Name = "picWarning";
      this.picWarning.Size = new Size(16, 16);
      this.picWarning.SizeMode = PictureBoxSizeMode.AutoSize;
      this.picWarning.TabIndex = 49;
      this.picWarning.TabStop = false;
      this.panelWarning.Controls.Add((Control) this.label7);
      this.panelWarning.Controls.Add((Control) this.picWarning);
      this.panelWarning.Location = new Point(215, 207);
      this.panelWarning.Name = "panelWarning";
      this.panelWarning.Size = new Size(94, 20);
      this.panelWarning.TabIndex = 50;
      this.panelWarning.Visible = false;
      this.label7.AutoSize = true;
      this.label7.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label7.ForeColor = Color.Red;
      this.label7.Location = new Point(20, 4);
      this.label7.Name = "label7";
      this.label7.Size = new Size(53, 13);
      this.label7.TabIndex = 50;
      this.label7.Text = "Expired!";
      this.lblFile.AutoSize = true;
      this.lblFile.Location = new Point(13, 14);
      this.lblFile.Name = "lblFile";
      this.lblFile.Size = new Size(23, 13);
      this.lblFile.TabIndex = 51;
      this.lblFile.Text = "File";
      this.txtFilePath.Location = new Point(115, 11);
      this.txtFilePath.Name = "txtFilePath";
      this.txtFilePath.ReadOnly = true;
      this.txtFilePath.Size = new Size(408, 20);
      this.txtFilePath.TabIndex = 52;
      this.lblReq1.AutoSize = true;
      this.lblReq1.BackColor = Color.Transparent;
      this.lblReq1.ForeColor = Color.Red;
      this.lblReq1.Location = new Point(102, 39);
      this.lblReq1.Name = "lblReq1";
      this.lblReq1.Size = new Size(11, 13);
      this.lblReq1.TabIndex = 53;
      this.lblReq1.Text = "*";
      this.label8.AutoSize = true;
      this.label8.ForeColor = Color.Red;
      this.label8.Location = new Point(34, 13);
      this.label8.Name = "label8";
      this.label8.Size = new Size(11, 13);
      this.label8.TabIndex = 54;
      this.label8.Text = "*";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(557, 250);
      this.Controls.Add((Control) this.label8);
      this.Controls.Add((Control) this.lblReq1);
      this.Controls.Add((Control) this.txtFilePath);
      this.Controls.Add((Control) this.lblFile);
      this.Controls.Add((Control) this.panelWarning);
      this.Controls.Add((Control) this.btnAttachBrowse);
      this.Controls.Add((Control) this.chkAlert);
      this.Controls.Add((Control) this.txtDays);
      this.Controls.Add((Control) this.datExpiration);
      this.Controls.Add((Control) this.datAttachment);
      this.Controls.Add((Control) this.cboCategory);
      this.Controls.Add((Control) this.txtDescription);
      this.Controls.Add((Control) this.txtFileName);
      this.Controls.Add((Control) this.label6);
      this.Controls.Add((Control) this.label5);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.btnCancel);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (AddAttachmentForm);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Attachment";
      ((ISupportInitialize) this.btnAttachBrowse).EndInit();
      ((ISupportInitialize) this.picWarning).EndInit();
      this.panelWarning.ResumeLayout(false);
      this.panelWarning.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
