// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.AddDocumentFromSettings
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.ExternalOriginatorManagement;
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
  public class AddDocumentFromSettings : Form
  {
    private List<DocumentSettingInfo> documents;
    private List<ExternalSettingValue> settingsList;
    private IContainer components;
    private GridView gvDocs;
    private Button btnOK;
    private Button btnCancel;
    private GradientPanel gradientPanel1;
    private Label lblPlaceDocument;
    private ComboBox cboPlaceDocument;
    private TextBox txtDocDisplayName;
    private StandardIconButton btnSearch;
    private Label lblChooseExistingDoc;

    public AddDocumentFromSettings(
      List<DocumentSettingInfo> documents,
      List<ExternalSettingValue> settingsList)
    {
      this.InitializeComponent();
      this.settingsList = settingsList;
      this.documents = documents;
      this.cboPlaceDocument.SelectedIndex = 0;
      this.populateGridView(documents);
      this.btnOK.Enabled = false;
    }

    private void populateGridView(List<DocumentSettingInfo> docs)
    {
      this.gvDocs.Items.Clear();
      foreach (DocumentSettingInfo doc in docs)
        this.gvDocs.Items.Add(this.createGVItemForDocument(doc));
    }

    private GVItem createGVItemForDocument(DocumentSettingInfo document)
    {
      GVItem gvItemForDocument = new GVItem();
      gvItemForDocument.SubItems.Add((object) document.DisplayName);
      if (document.Category == -1)
        gvItemForDocument.SubItems.Add((object) "");
      else if (this.settingsList.FirstOrDefault<ExternalSettingValue>((Func<ExternalSettingValue, bool>) (it => it.settingId == document.Category)) != null)
        gvItemForDocument.SubItems.Add((object) this.settingsList.FirstOrDefault<ExternalSettingValue>((Func<ExternalSettingValue, bool>) (it => it.settingId == document.Category)).settingValue);
      else
        gvItemForDocument.SubItems.Add((object) "");
      gvItemForDocument.SubItems.Add((object) document.ChannelStr);
      GVSubItemCollection subItems1 = gvItemForDocument.SubItems;
      DateTime dateTime;
      string str1;
      if (!(document.StartDate != DateTime.MinValue))
      {
        str1 = "";
      }
      else
      {
        dateTime = document.StartDate;
        str1 = dateTime.ToString("d");
      }
      subItems1.Add((object) str1);
      GVSubItemCollection subItems2 = gvItemForDocument.SubItems;
      string str2;
      if (!(document.EndDate != DateTime.MinValue))
      {
        str2 = "";
      }
      else
      {
        dateTime = document.EndDate;
        str2 = dateTime.ToString("d");
      }
      subItems2.Add((object) str2);
      gvItemForDocument.SubItems.Add((object) document.StatusStr);
      gvItemForDocument.Tag = (object) document;
      return gvItemForDocument;
    }

    private void btnOK_Click(object sender, EventArgs e) => this.DialogResult = DialogResult.OK;

    private void btnCancel_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
    }

    public DocumentSettingInfo SelectedDoc
    {
      get => (DocumentSettingInfo) this.gvDocs.SelectedItems[0].Tag;
    }

    public bool IsTopOfCategory => this.cboPlaceDocument.SelectedIndex == 0;

    private void gvDocs_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.btnOK.Enabled = this.gvDocs.SelectedItems.Count == 1;
    }

    private void btnSearch_Click(object sender, EventArgs e)
    {
      if (this.txtDocDisplayName.Text.Trim() == string.Empty)
        this.populateGridView(this.documents);
      else
        this.populateGridView(this.documents.Where<DocumentSettingInfo>((Func<DocumentSettingInfo, bool>) (a => a.DisplayName.ToUpper().Contains(this.txtDocDisplayName.Text.Trim().ToUpper()))).ToList<DocumentSettingInfo>());
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
      GVColumn gvColumn3 = new GVColumn();
      GVColumn gvColumn4 = new GVColumn();
      GVColumn gvColumn5 = new GVColumn();
      GVColumn gvColumn6 = new GVColumn();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (AddDocumentFromSettings));
      this.gvDocs = new GridView();
      this.btnOK = new Button();
      this.btnCancel = new Button();
      this.gradientPanel1 = new GradientPanel();
      this.lblChooseExistingDoc = new Label();
      this.btnSearch = new StandardIconButton();
      this.txtDocDisplayName = new TextBox();
      this.lblPlaceDocument = new Label();
      this.cboPlaceDocument = new ComboBox();
      this.gradientPanel1.SuspendLayout();
      ((ISupportInitialize) this.btnSearch).BeginInit();
      this.SuspendLayout();
      this.gvDocs.AllowMultiselect = false;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "ColumnDocDisplayName";
      gvColumn1.Text = "Document Display Name";
      gvColumn1.Width = 200;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "ColumnCategory";
      gvColumn2.Text = "Category";
      gvColumn2.Width = 150;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "ColumnChannel";
      gvColumn3.Text = "Channel";
      gvColumn3.Width = 110;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "ColumnStartDate";
      gvColumn4.SortMethod = GVSortMethod.Date;
      gvColumn4.Text = "Start Date";
      gvColumn4.Width = 80;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "ColumnEndDate";
      gvColumn5.SortMethod = GVSortMethod.Date;
      gvColumn5.Text = "End Date";
      gvColumn5.Width = 80;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "ColumnStatus";
      gvColumn6.Text = "Status";
      gvColumn6.Width = 100;
      this.gvDocs.Columns.AddRange(new GVColumn[6]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5,
        gvColumn6
      });
      this.gvDocs.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvDocs.Location = new Point(25, 58);
      this.gvDocs.Name = "gvDocs";
      this.gvDocs.Size = new Size(726, 303);
      this.gvDocs.TabIndex = 9;
      this.gvDocs.SelectedIndexChanged += new EventHandler(this.gvDocs_SelectedIndexChanged);
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.Location = new Point(611, 423);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 11;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(692, 423);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 10;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.gradientPanel1.Controls.Add((Control) this.lblChooseExistingDoc);
      this.gradientPanel1.Controls.Add((Control) this.btnSearch);
      this.gradientPanel1.Controls.Add((Control) this.txtDocDisplayName);
      this.gradientPanel1.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gradientPanel1.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gradientPanel1.Location = new Point(25, 20);
      this.gradientPanel1.Name = "gradientPanel1";
      this.gradientPanel1.Size = new Size(726, 40);
      this.gradientPanel1.TabIndex = 12;
      this.lblChooseExistingDoc.AutoSize = true;
      this.lblChooseExistingDoc.Location = new Point(175, 13);
      this.lblChooseExistingDoc.Name = "lblChooseExistingDoc";
      this.lblChooseExistingDoc.Size = new Size(171, 13);
      this.lblChooseExistingDoc.TabIndex = 73;
      this.lblChooseExistingDoc.Text = "Choose an existing TPO document";
      this.btnSearch.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnSearch.BackColor = Color.Transparent;
      this.btnSearch.Location = new Point(687, 12);
      this.btnSearch.MouseDownImage = (Image) null;
      this.btnSearch.Name = "btnSearch";
      this.btnSearch.Size = new Size(16, 16);
      this.btnSearch.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.btnSearch.TabIndex = 61;
      this.btnSearch.TabStop = false;
      this.btnSearch.Click += new EventHandler(this.btnSearch_Click);
      this.txtDocDisplayName.Location = new Point(347, 10);
      this.txtDocDisplayName.MaxLength = 250;
      this.txtDocDisplayName.Name = "txtDocDisplayName";
      this.txtDocDisplayName.Size = new Size(334, 20);
      this.txtDocDisplayName.TabIndex = 60;
      this.lblPlaceDocument.AutoSize = true;
      this.lblPlaceDocument.Location = new Point(22, 386);
      this.lblPlaceDocument.Name = "lblPlaceDocument";
      this.lblPlaceDocument.Size = new Size(84, 13);
      this.lblPlaceDocument.TabIndex = 72;
      this.lblPlaceDocument.Text = "Place document";
      this.cboPlaceDocument.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboPlaceDocument.FormattingEnabled = true;
      this.cboPlaceDocument.Items.AddRange(new object[2]
      {
        (object) "Top of document's category",
        (object) "Bottom of document's category"
      });
      this.cboPlaceDocument.Location = new Point(141, 383);
      this.cboPlaceDocument.Name = "cboPlaceDocument";
      this.cboPlaceDocument.Size = new Size(191, 21);
      this.cboPlaceDocument.TabIndex = 71;
      this.AcceptButton = (IButtonControl) this.btnOK;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(779, 458);
      this.Controls.Add((Control) this.lblPlaceDocument);
      this.Controls.Add((Control) this.cboPlaceDocument);
      this.Controls.Add((Control) this.gradientPanel1);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.gvDocs);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (AddDocumentFromSettings);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Add document from existing TPO Docs";
      this.gradientPanel1.ResumeLayout(false);
      this.gradientPanel1.PerformLayout();
      ((ISupportInitialize) this.btnSearch).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
