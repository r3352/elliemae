// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.AddEditTPODocument
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.ExternalOriginatorManagement;
using EllieMae.EMLite.Common;
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
  public class AddEditTPODocument : Form
  {
    private bool edit;
    private OpenFileDialog openFileDialog1 = new OpenFileDialog();
    private DocumentSettingInfo document;
    private string userName;
    private string filePath;
    private Guid guid = Guid.NewGuid();
    public bool DocumentChanged;
    private string fileSize;
    private IContainer components;
    private Label label8;
    private TextBox txtFileName;
    private Label lblFile;
    private IconButton btnAttachBrowse;
    private TextBox txtDocDisplayName;
    private Label lblDocDisplayName;
    private DatePicker dtpStartDate;
    private DatePicker dtpEndDate;
    private Label lblStartDate;
    private Label lblEndDate;
    private ComboBox cboCategory;
    private Label label1;
    private Label lblChannel;
    private ComboBox cboChannel;
    private Label lblPlaceDocument;
    private ComboBox cboPlaceDocument;
    private Button btnOK;
    private Button btnCancel;
    private Label label2;

    public DocumentSettingInfo Document => this.document;

    public string Path => this.filePath;

    public string FileName
    {
      get
      {
        __Boxed<Guid> guid = (ValueType) this.guid;
        string str = ((IEnumerable<string>) this.filePath.Trim().Split('.')).Last<string>();
        return guid.ToString() + "." + str;
      }
    }

    public bool IsTopOfCategory => this.cboPlaceDocument.SelectedIndex == 0;

    public AddEditTPODocument(
      DocumentSettingInfo document,
      List<ExternalSettingValue> documentCategories,
      string userName)
    {
      this.InitializeComponent();
      this.edit = document != null;
      this.userName = userName;
      this.document = document;
      if (this.edit)
        this.guid = document.Guid;
      this.cboCategory.DataSource = (object) documentCategories;
      this.cboCategory.DisplayMember = "settingValue";
      this.cboCategory.ValueMember = "settingId";
      this.cboPlaceDocument.SelectedIndex = 0;
      if (this.edit)
      {
        this.Text = "Edit Document";
        this.populateFields();
        this.cboPlaceDocument.Visible = false;
        this.lblPlaceDocument.Visible = false;
      }
      else
      {
        this.Text = "Add New Document";
        this.cboChannel.SelectedIndex = 0;
      }
    }

    private void populateFields()
    {
      this.txtFileName.Text = this.document.FileName;
      this.txtDocDisplayName.Text = this.document.DisplayName;
      this.dtpStartDate.Value = this.document.StartDate;
      this.dtpEndDate.Value = this.document.EndDate;
      this.cboCategory.SelectedValue = (object) this.document.Category;
      this.cboChannel.SelectedIndex = this.GetChannelInt(this.document.Channel);
      this.btnAttachBrowse.Visible = false;
    }

    private bool validateValue()
    {
      if (this.txtFileName.Text.Trim() == "")
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The file has not been specified.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.btnAttachBrowse_Click((object) null, (EventArgs) null);
        return false;
      }
      if (this.txtDocDisplayName.Text.Trim() == "")
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Document Display Name cannot be blank.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.txtDocDisplayName.Focus();
        return false;
      }
      if (this.dtpStartDate.Value < DateTime.MinValue || this.dtpStartDate.Value > DateTime.MaxValue)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Invalid data for Start Date.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.dtpStartDate.Focus();
        return false;
      }
      if (this.dtpEndDate.Value < DateTime.MinValue || this.dtpEndDate.Value > DateTime.MaxValue)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Invalid data for End Date.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.dtpEndDate.Focus();
        return false;
      }
      if (!(this.dtpEndDate.Value < this.dtpStartDate.Value) || !(this.dtpStartDate.Value != DateTime.MinValue) || !(this.dtpEndDate.Value != DateTime.MinValue))
        return true;
      int num1 = (int) Utils.Dialog((IWin32Window) this, "Start Date cannot be greater than End Date.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      this.dtpStartDate.Focus();
      return false;
    }

    private void btnAttachBrowse_Click(object sender, EventArgs e)
    {
      if (this.edit && Utils.Dialog((IWin32Window) this, "If you select another source file, the original file will be overwritten. Continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
        return;
      string[] array = new string[12]
      {
        ".pdf",
        ".doc",
        ".docx",
        ".xls",
        ".xlsx",
        ".txt",
        ".tif",
        ".jpg",
        ".jpeg",
        ".jpe",
        ".csv",
        ".xml"
      };
      using (OpenFileDialog openFileDialog = new OpenFileDialog())
      {
        openFileDialog.Title = "Browse and Attach";
        openFileDialog.CheckFileExists = true;
        openFileDialog.Multiselect = false;
        openFileDialog.ShowReadOnly = false;
        openFileDialog.Filter = "All Supported Formats|*.pdf;*.doc;*.docx;*.xls;*.xlsx;*.txt;*.tif;*.jpg;*.jpeg;*.jpe;*.csv;*.xml|Adobe PDF Documents (*.pdf)|*.pdf|Microsoft Word Documents (*.doc,*.docx)|*.doc;*.docx|Microsoft Excel Documents (*.xls,*.xlsx)|*.xls;*.xlsx|Text Documents (*.txt)|*.txt|TIFF Images (*.tif)|*.tif|JPEG Images (*.jpg, *.jpeg, *.jpe)|*.jpg;*.jpeg;*.jpe|CSV Files (*.csv)|*.csv|XML Files (*.xml)|*.xml";
        if (openFileDialog.ShowDialog((IWin32Window) Form.ActiveForm) != DialogResult.OK)
          return;
        string str = System.IO.Path.GetExtension(openFileDialog.FileName).ToLower().Trim();
        if (Array.IndexOf<string>(array, str) < 0)
        {
          int num1 = (int) Utils.Dialog((IWin32Window) this, "The '" + str + "' file type is not supported. The allowed file types are '.pdf', '.doc', '.docx', '.xls', '.xlsx', '.txt', '.tif', '.jpg', '.jpeg', '.jpe', '.csv', and '.xml'.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else
        {
          FileInfo fileInfo = new FileInfo(openFileDialog.FileName);
          if (fileInfo.Length > 25000000L)
          {
            int num2 = (int) Utils.Dialog((IWin32Window) this, "File attachments cannot exceed 25 MB. Please select another file.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          }
          else
          {
            this.fileSize = Utils.FormatByteSize(fileInfo.Length);
            this.setFileName(openFileDialog.FileName);
          }
        }
      }
    }

    private void setFileName(string path)
    {
      this.filePath = path;
      this.txtFileName.Text = ((IEnumerable<string>) path.Trim().Split('\\')).Last<string>();
      this.DocumentChanged = true;
    }

    private void txtFileName_TextChanged(object sender, EventArgs e)
    {
      this.btnOK.Enabled = this.txtFileName.Text.Trim() != string.Empty;
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      if (!this.validateValue())
        return;
      int num = this.cboCategory.SelectedIndex <= 0 ? -1 : ((ExternalSettingValue) this.cboCategory.SelectedItem).settingId;
      if (this.document == null)
      {
        this.document = new DocumentSettingInfo();
        this.document.Active = true;
        this.document.Guid = this.guid;
        this.document.FileSize = this.fileSize;
      }
      this.document.FileName = this.txtFileName.Text.Trim();
      this.document.DisplayName = this.txtDocDisplayName.Text.Trim();
      this.document.StartDate = this.dtpStartDate.Value;
      this.document.EndDate = this.dtpEndDate.Value;
      this.document.Category = num;
      this.document.Channel = this.GetSelectedChannel();
      this.document.DateAdded = DateTime.Now;
      this.document.AvailbleAllTPO = false;
      this.document.AddedBy = this.userName;
      this.document.IsArchive = false;
      this.DialogResult = DialogResult.OK;
    }

    public ExternalOriginatorEntityType GetSelectedChannel()
    {
      if (this.cboChannel.SelectedItem != null && !(this.cboChannel.SelectedItem.ToString() == "All"))
      {
        if (this.cboChannel.SelectedItem.ToString() == "Broker")
          return ExternalOriginatorEntityType.Broker;
        if (this.cboChannel.SelectedItem.ToString() == "Correspondent")
          return ExternalOriginatorEntityType.Correspondent;
      }
      return ExternalOriginatorEntityType.Both;
    }

    public int GetChannelInt(ExternalOriginatorEntityType channelType)
    {
      int channelInt = 0;
      switch (channelType)
      {
        case ExternalOriginatorEntityType.Broker:
        case ExternalOriginatorEntityType.Correspondent:
          channelInt = (int) channelType;
          break;
        case ExternalOriginatorEntityType.Both:
          channelInt = 0;
          break;
      }
      return channelInt;
    }

    private void btnCancel_Click(object sender, EventArgs e) => this.Close();

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (AddEditTPODocument));
      this.label8 = new Label();
      this.txtFileName = new TextBox();
      this.lblFile = new Label();
      this.btnAttachBrowse = new IconButton();
      this.txtDocDisplayName = new TextBox();
      this.lblDocDisplayName = new Label();
      this.dtpStartDate = new DatePicker();
      this.dtpEndDate = new DatePicker();
      this.lblStartDate = new Label();
      this.lblEndDate = new Label();
      this.cboCategory = new ComboBox();
      this.label1 = new Label();
      this.lblChannel = new Label();
      this.cboChannel = new ComboBox();
      this.lblPlaceDocument = new Label();
      this.cboPlaceDocument = new ComboBox();
      this.btnOK = new Button();
      this.btnCancel = new Button();
      this.label2 = new Label();
      ((ISupportInitialize) this.btnAttachBrowse).BeginInit();
      this.SuspendLayout();
      this.label8.AutoSize = true;
      this.label8.ForeColor = Color.Red;
      this.label8.Location = new Point(81, 25);
      this.label8.Name = "label8";
      this.label8.Size = new Size(11, 13);
      this.label8.TabIndex = 58;
      this.label8.Text = "*";
      this.txtFileName.Location = new Point(159, 22);
      this.txtFileName.Name = "txtFileName";
      this.txtFileName.ReadOnly = true;
      this.txtFileName.Size = new Size(372, 20);
      this.txtFileName.TabIndex = 57;
      this.txtFileName.TextChanged += new EventHandler(this.txtFileName_TextChanged);
      this.lblFile.AutoSize = true;
      this.lblFile.Location = new Point(21, 25);
      this.lblFile.Name = "lblFile";
      this.lblFile.Size = new Size(54, 13);
      this.lblFile.TabIndex = 56;
      this.lblFile.Text = "File Name";
      this.btnAttachBrowse.BackColor = Color.Transparent;
      this.btnAttachBrowse.DisabledImage = (Image) null;
      this.btnAttachBrowse.Image = (Image) componentResourceManager.GetObject("btnAttachBrowse.Image");
      this.btnAttachBrowse.Location = new Point(538, 25);
      this.btnAttachBrowse.Margin = new Padding(4, 3, 0, 3);
      this.btnAttachBrowse.MouseDownImage = (Image) null;
      this.btnAttachBrowse.MouseOverImage = (Image) null;
      this.btnAttachBrowse.Name = "btnAttachBrowse";
      this.btnAttachBrowse.Size = new Size(16, 16);
      this.btnAttachBrowse.TabIndex = 55;
      this.btnAttachBrowse.TabStop = false;
      this.btnAttachBrowse.Click += new EventHandler(this.btnAttachBrowse_Click);
      this.txtDocDisplayName.Location = new Point(159, 48);
      this.txtDocDisplayName.MaxLength = 250;
      this.txtDocDisplayName.Name = "txtDocDisplayName";
      this.txtDocDisplayName.Size = new Size(372, 20);
      this.txtDocDisplayName.TabIndex = 59;
      this.lblDocDisplayName.AutoSize = true;
      this.lblDocDisplayName.Location = new Point(21, 51);
      this.lblDocDisplayName.Name = "lblDocDisplayName";
      this.lblDocDisplayName.Size = new Size(124, 13);
      this.lblDocDisplayName.TabIndex = 60;
      this.lblDocDisplayName.Text = "Document Display Name";
      this.dtpStartDate.BackColor = SystemColors.Window;
      this.dtpStartDate.Location = new Point(159, 74);
      this.dtpStartDate.MaxValue = new DateTime(2079, 6, 6, 0, 0, 0, 0);
      this.dtpStartDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dtpStartDate.Name = "dtpStartDate";
      this.dtpStartDate.Size = new Size(85, 21);
      this.dtpStartDate.TabIndex = 61;
      this.dtpStartDate.Tag = (object) "763";
      this.dtpStartDate.ToolTip = "";
      this.dtpStartDate.Value = new DateTime(0L);
      this.dtpEndDate.BackColor = SystemColors.Window;
      this.dtpEndDate.Location = new Point(159, 103);
      this.dtpEndDate.MaxValue = new DateTime(2079, 6, 6, 0, 0, 0, 0);
      this.dtpEndDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dtpEndDate.Name = "dtpEndDate";
      this.dtpEndDate.Size = new Size(85, 21);
      this.dtpEndDate.TabIndex = 62;
      this.dtpEndDate.Tag = (object) "763";
      this.dtpEndDate.ToolTip = "";
      this.dtpEndDate.Value = new DateTime(0L);
      this.lblStartDate.AutoSize = true;
      this.lblStartDate.Location = new Point(20, 74);
      this.lblStartDate.Name = "lblStartDate";
      this.lblStartDate.Size = new Size(55, 13);
      this.lblStartDate.TabIndex = 63;
      this.lblStartDate.Text = "Start Date";
      this.lblEndDate.AutoSize = true;
      this.lblEndDate.Location = new Point(20, 103);
      this.lblEndDate.Name = "lblEndDate";
      this.lblEndDate.Size = new Size(52, 13);
      this.lblEndDate.TabIndex = 64;
      this.lblEndDate.Text = "End Date";
      this.cboCategory.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboCategory.FormattingEnabled = true;
      this.cboCategory.Location = new Point(159, 130);
      this.cboCategory.Name = "cboCategory";
      this.cboCategory.Size = new Size(201, 21);
      this.cboCategory.TabIndex = 65;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(21, 133);
      this.label1.Name = "label1";
      this.label1.Size = new Size(49, 13);
      this.label1.TabIndex = 66;
      this.label1.Text = "Category";
      this.lblChannel.AutoSize = true;
      this.lblChannel.Location = new Point(21, 161);
      this.lblChannel.Name = "lblChannel";
      this.lblChannel.Size = new Size(46, 13);
      this.lblChannel.TabIndex = 68;
      this.lblChannel.Text = "Channel";
      this.cboChannel.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboChannel.FormattingEnabled = true;
      this.cboChannel.Items.AddRange(new object[3]
      {
        (object) "All",
        (object) "Broker",
        (object) "Correspondent"
      });
      this.cboChannel.Location = new Point(159, 158);
      this.cboChannel.Name = "cboChannel";
      this.cboChannel.Size = new Size(201, 21);
      this.cboChannel.TabIndex = 67;
      this.lblPlaceDocument.AutoSize = true;
      this.lblPlaceDocument.Location = new Point(21, 191);
      this.lblPlaceDocument.Name = "lblPlaceDocument";
      this.lblPlaceDocument.Size = new Size(84, 13);
      this.lblPlaceDocument.TabIndex = 70;
      this.lblPlaceDocument.Text = "Place document";
      this.cboPlaceDocument.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboPlaceDocument.FormattingEnabled = true;
      this.cboPlaceDocument.Items.AddRange(new object[2]
      {
        (object) "Top of document's category",
        (object) "Bottom of document's category"
      });
      this.cboPlaceDocument.Location = new Point(159, 188);
      this.cboPlaceDocument.Name = "cboPlaceDocument";
      this.cboPlaceDocument.Size = new Size(201, 21);
      this.cboPlaceDocument.TabIndex = 69;
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.Location = new Point(374, 218);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 71;
      this.btnOK.Text = "&OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(455, 218);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 72;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.label2.AutoSize = true;
      this.label2.ForeColor = Color.Red;
      this.label2.Location = new Point(142, 51);
      this.label2.Name = "label2";
      this.label2.Size = new Size(11, 13);
      this.label2.TabIndex = 73;
      this.label2.Text = "*";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(575, 262);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.lblPlaceDocument);
      this.Controls.Add((Control) this.cboPlaceDocument);
      this.Controls.Add((Control) this.lblChannel);
      this.Controls.Add((Control) this.cboChannel);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.cboCategory);
      this.Controls.Add((Control) this.lblEndDate);
      this.Controls.Add((Control) this.lblStartDate);
      this.Controls.Add((Control) this.dtpEndDate);
      this.Controls.Add((Control) this.dtpStartDate);
      this.Controls.Add((Control) this.lblDocDisplayName);
      this.Controls.Add((Control) this.txtDocDisplayName);
      this.Controls.Add((Control) this.label8);
      this.Controls.Add((Control) this.txtFileName);
      this.Controls.Add((Control) this.lblFile);
      this.Controls.Add((Control) this.btnAttachBrowse);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (AddEditTPODocument);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Add new Document";
      ((ISupportInitialize) this.btnAttachBrowse).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
