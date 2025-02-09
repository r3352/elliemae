// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ExternalOriginatorManagement.AddNewTPOFeesFromSettings
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
namespace EllieMae.EMLite.Setup.ExternalOriginatorManagement
{
  public class AddNewTPOFeesFromSettings : Form
  {
    private List<ExternalFeeManagement> fees;
    private IContainer components;
    private GridView gvFees;
    private Button btnOK;
    private Button btnCancel;
    private GradientPanel gradientPanel1;
    private TextBox txtSearch;
    private ComboBox cmbStatus;
    private Label lblStatus;
    private ComboBox cmbChannelType;
    private Label lblChannelType;
    private Label label1;
    private PictureBox picSearch;
    private ImageList imgList;

    public AddNewTPOFeesFromSettings(List<ExternalFeeManagement> fees)
    {
      this.InitializeComponent();
      this.fees = fees;
      this.cmbChannelType.SelectedIndexChanged -= new EventHandler(this.cmbChannelType_SelectedIndexChanged);
      this.cmbStatus.SelectedIndexChanged -= new EventHandler(this.cmbStatus_SelectedIndexChanged);
      this.cmbChannelType.SelectedIndex = 0;
      this.cmbStatus.SelectedIndex = 0;
      this.cmbChannelType.SelectedIndexChanged += new EventHandler(this.cmbChannelType_SelectedIndexChanged);
      this.cmbStatus.SelectedIndexChanged += new EventHandler(this.cmbStatus_SelectedIndexChanged);
      this.populateFees(fees);
      this.btnOK.Enabled = false;
      this.setIconButton();
    }

    private void populateFees(List<ExternalFeeManagement> feesPop)
    {
      this.btnOK.Enabled = false;
      this.gvFees.Items.Clear();
      foreach (ExternalFeeManagement fee in feesPop)
      {
        bool flag = true;
        if (this.cmbChannelType.SelectedItem.ToString() != "All")
        {
          ExternalOriginatorEntityType channel = fee.Channel;
          if (channel.ToString() != this.cmbChannelType.SelectedItem.ToString())
          {
            channel = fee.Channel;
            if (channel.ToString() != "Both")
              flag = false;
          }
        }
        if (this.cmbStatus.SelectedItem.ToString() != "All" && fee.Status.ToString() != this.cmbStatus.SelectedItem.ToString())
          flag = false;
        if (this.txtSearch.Text != "" && fee.FeeName.ToUpper() != this.txtSearch.Text.ToUpper())
          flag = false;
        if (flag)
          this.gvFees.Items.Add(this.createGVItem(fee));
      }
      this.gvFees.ReSort();
    }

    private GVItem createGVItem(ExternalFeeManagement fee)
    {
      GVItem gvItem = new GVItem();
      gvItem.SubItems.Add((object) fee.FeeName);
      gvItem.SubItems.Add(fee.Description.Length > 50 ? (object) (fee.Description.Substring(0, Math.Min(fee.Description.Length, 50)) + "...") : (object) fee.Description);
      gvItem.SubItems.Add(fee.Channel == ExternalOriginatorEntityType.Both ? (object) "All" : (object) fee.Channel.ToString());
      gvItem.SubItems.Add((object) fee.FeePercent);
      gvItem.SubItems.Add((object) fee.FeeAmount);
      gvItem.SubItems.Add((object) fee.Code);
      GVSubItemCollection subItems1 = gvItem.SubItems;
      DateTime dateTime;
      string str1;
      if (!(fee.StartDate != DateTime.MinValue))
      {
        str1 = "";
      }
      else
      {
        dateTime = fee.StartDate;
        str1 = dateTime.ToString("d");
      }
      subItems1.Add((object) str1);
      GVSubItemCollection subItems2 = gvItem.SubItems;
      string str2;
      if (!(fee.EndDate != DateTime.MinValue))
      {
        str2 = "";
      }
      else
      {
        dateTime = fee.EndDate;
        str2 = dateTime.ToString("d");
      }
      subItems2.Add((object) str2);
      GVSubItemCollection subItems3 = gvItem.SubItems;
      string str3;
      if (!(fee.DateUpdated != DateTime.MinValue))
      {
        str3 = "";
      }
      else
      {
        dateTime = fee.DateUpdated;
        str3 = dateTime.ToString("d");
      }
      subItems3.Add((object) str3);
      gvItem.SubItems.Add((object) fee.UpdatedBy);
      gvItem.SubItems.Add((object) fee.Status);
      gvItem.Tag = (object) fee;
      return gvItem;
    }

    private void btnOK_Click(object sender, EventArgs e) => this.DialogResult = DialogResult.OK;

    private void btnCancel_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
    }

    public List<ExternalFeeManagement> Value
    {
      get
      {
        List<ExternalFeeManagement> externalFeeManagementList = new List<ExternalFeeManagement>();
        foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvFees.Items)
        {
          if (gvItem.Checked)
            externalFeeManagementList.Add((ExternalFeeManagement) gvItem.Tag);
        }
        return externalFeeManagementList;
      }
    }

    private void gvFees_SubItemCheck(object source, GVSubItemEventArgs e)
    {
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvFees.Items)
      {
        if (gvItem.Checked)
        {
          this.btnOK.Enabled = true;
          return;
        }
      }
      this.btnOK.Enabled = false;
    }

    private void picSearch_Click(object sender, EventArgs e)
    {
      if (this.txtSearch.Text == string.Empty)
        this.populateFees(this.fees);
      else
        this.populateFees(this.fees.Where<ExternalFeeManagement>((Func<ExternalFeeManagement, bool>) (a => a.FeeName.ToUpper().Contains(this.txtSearch.Text.ToUpper()))).ToList<ExternalFeeManagement>());
    }

    private void cmbChannelType_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.populateFees(this.fees);
    }

    private void cmbStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.populateFees(this.fees);
    }

    private void setIconButton()
    {
      this.picSearch.Image = this.imgList.Images[this.picSearch.Name];
      this.picSearch.Enabled = true;
    }

    private void picSearch_MouseEnter(object sender, EventArgs e)
    {
      PictureBox pictureBox = (PictureBox) sender;
      pictureBox.Image = this.imgList.Images[pictureBox.Name + "MouseOver"];
    }

    private void picSearch_MouseLeave(object sender, EventArgs e)
    {
      PictureBox pictureBox = (PictureBox) sender;
      if (!pictureBox.Enabled)
        return;
      pictureBox.Image = this.imgList.Images[pictureBox.Name];
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
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      GVColumn gvColumn4 = new GVColumn();
      GVColumn gvColumn5 = new GVColumn();
      GVColumn gvColumn6 = new GVColumn();
      GVColumn gvColumn7 = new GVColumn();
      GVColumn gvColumn8 = new GVColumn();
      GVColumn gvColumn9 = new GVColumn();
      GVColumn gvColumn10 = new GVColumn();
      GVColumn gvColumn11 = new GVColumn();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (AddNewTPOFeesFromSettings));
      this.gvFees = new GridView();
      this.btnOK = new Button();
      this.btnCancel = new Button();
      this.gradientPanel1 = new GradientPanel();
      this.label1 = new Label();
      this.picSearch = new PictureBox();
      this.txtSearch = new TextBox();
      this.cmbStatus = new ComboBox();
      this.lblStatus = new Label();
      this.cmbChannelType = new ComboBox();
      this.lblChannelType = new Label();
      this.imgList = new ImageList(this.components);
      this.gradientPanel1.SuspendLayout();
      ((ISupportInitialize) this.picSearch).BeginInit();
      this.SuspendLayout();
      gvColumn1.CheckBoxes = true;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Fee Name";
      gvColumn1.Width = 150;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Description";
      gvColumn2.Width = 200;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.Text = "Channel";
      gvColumn3.Width = 100;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column11";
      gvColumn4.SortMethod = GVSortMethod.Numeric;
      gvColumn4.Text = "Fee %";
      gvColumn4.Width = 100;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Column4";
      gvColumn5.Text = "Fee Amount";
      gvColumn5.Width = 100;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "Column5";
      gvColumn6.Text = "Code";
      gvColumn6.Width = 50;
      gvColumn7.ImageIndex = -1;
      gvColumn7.Name = "Column6";
      gvColumn7.Text = "Start Date";
      gvColumn7.Width = 150;
      gvColumn8.ImageIndex = -1;
      gvColumn8.Name = "Column7";
      gvColumn8.Text = "End Date";
      gvColumn8.Width = 150;
      gvColumn9.ImageIndex = -1;
      gvColumn9.Name = "Column8";
      gvColumn9.Text = "Last Updated";
      gvColumn9.Width = 150;
      gvColumn10.ImageIndex = -1;
      gvColumn10.Name = "Column9";
      gvColumn10.Text = "Updated By";
      gvColumn10.Width = 100;
      gvColumn11.ImageIndex = -1;
      gvColumn11.Name = "Column10";
      gvColumn11.Text = "Status";
      gvColumn11.Width = 100;
      this.gvFees.Columns.AddRange(new GVColumn[11]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5,
        gvColumn6,
        gvColumn7,
        gvColumn8,
        gvColumn9,
        gvColumn10,
        gvColumn11
      });
      this.gvFees.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvFees.Location = new Point(-2, 85);
      this.gvFees.Name = "gvFees";
      this.gvFees.Size = new Size(779, 388);
      this.gvFees.TabIndex = 9;
      this.gvFees.SubItemCheck += new GVSubItemEventHandler(this.gvFees_SubItemCheck);
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.Location = new Point(611, 523);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 11;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(692, 523);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 10;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.gradientPanel1.Controls.Add((Control) this.label1);
      this.gradientPanel1.Dock = DockStyle.Top;
      this.gradientPanel1.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gradientPanel1.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gradientPanel1.Location = new Point(0, 0);
      this.gradientPanel1.Name = "gradientPanel1";
      this.gradientPanel1.Size = new Size(779, 38);
      this.gradientPanel1.TabIndex = 12;
      this.label1.AutoSize = true;
      this.label1.BackColor = Color.Transparent;
      this.label1.Location = new Point(2, 13);
      this.label1.Name = "label1";
      this.label1.Size = new Size(348, 13);
      this.label1.TabIndex = 36;
      this.label1.Text = "Select one or more fees from the TPO Fees library using the filters below.";
      this.picSearch.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.picSearch.BackColor = Color.Transparent;
      this.picSearch.Location = new Point(749, 51);
      this.picSearch.Name = "picSearch";
      this.picSearch.Size = new Size(20, 20);
      this.picSearch.TabIndex = 35;
      this.picSearch.TabStop = false;
      this.picSearch.Click += new EventHandler(this.picSearch_Click);
      this.picSearch.MouseEnter += new EventHandler(this.picSearch_MouseEnter);
      this.picSearch.MouseLeave += new EventHandler(this.picSearch_MouseLeave);
      this.txtSearch.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.txtSearch.Location = new Point(570, 51);
      this.txtSearch.Name = "txtSearch";
      this.txtSearch.Size = new Size(176, 20);
      this.txtSearch.TabIndex = 4;
      this.cmbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbStatus.FormattingEnabled = true;
      this.cmbStatus.Items.AddRange(new object[4]
      {
        (object) "All",
        (object) "Pending",
        (object) "Active",
        (object) "Expired"
      });
      this.cmbStatus.Location = new Point(60, 50);
      this.cmbStatus.Name = "cmbStatus";
      this.cmbStatus.Size = new Size(121, 21);
      this.cmbStatus.TabIndex = 3;
      this.cmbStatus.SelectedIndexChanged += new EventHandler(this.cmbStatus_SelectedIndexChanged);
      this.lblStatus.AutoSize = true;
      this.lblStatus.BackColor = Color.Transparent;
      this.lblStatus.Location = new Point(14, 53);
      this.lblStatus.Name = "lblStatus";
      this.lblStatus.Size = new Size(40, 13);
      this.lblStatus.TabIndex = 2;
      this.lblStatus.Text = "Status:";
      this.cmbChannelType.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbChannelType.FormattingEnabled = true;
      this.cmbChannelType.Items.AddRange(new object[3]
      {
        (object) "All",
        (object) "Broker",
        (object) "Correspondent"
      });
      this.cmbChannelType.Location = new Point(94, 50);
      this.cmbChannelType.Name = "cmbChannelType";
      this.cmbChannelType.Size = new Size(121, 21);
      this.cmbChannelType.TabIndex = 1;
      this.cmbChannelType.Visible = false;
      this.cmbChannelType.SelectedIndexChanged += new EventHandler(this.cmbChannelType_SelectedIndexChanged);
      this.lblChannelType.AutoSize = true;
      this.lblChannelType.BackColor = Color.Transparent;
      this.lblChannelType.Location = new Point(12, 53);
      this.lblChannelType.Name = "lblChannelType";
      this.lblChannelType.Size = new Size(76, 13);
      this.lblChannelType.TabIndex = 0;
      this.lblChannelType.Text = "Channel Type:";
      this.lblChannelType.Visible = false;
      this.imgList.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imgList.ImageStream");
      this.imgList.TransparentColor = Color.Transparent;
      this.imgList.Images.SetKeyName(0, "picSearch");
      this.imgList.Images.SetKeyName(1, "picSearchMouseOver");
      this.imgList.Images.SetKeyName(2, "picSearchDisabled");
      this.AcceptButton = (IButtonControl) this.btnOK;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(779, 558);
      this.Controls.Add((Control) this.gradientPanel1);
      this.Controls.Add((Control) this.picSearch);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.txtSearch);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.cmbStatus);
      this.Controls.Add((Control) this.gvFees);
      this.Controls.Add((Control) this.lblStatus);
      this.Controls.Add((Control) this.lblChannelType);
      this.Controls.Add((Control) this.cmbChannelType);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (AddNewTPOFeesFromSettings);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Add fees from Global TPO Fees list";
      this.gradientPanel1.ResumeLayout(false);
      this.gradientPanel1.PerformLayout();
      ((ISupportInitialize) this.picSearch).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
