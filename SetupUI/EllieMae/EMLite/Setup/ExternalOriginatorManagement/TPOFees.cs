// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ExternalOriginatorManagement.TPOFees
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.ExternalOriginatorManagement;
using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Serialization;
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
  public class TPOFees : UserControl, IOnlineHelpTarget
  {
    private Sessions.Session session;
    private List<ExternalFeeManagement> fees;
    private bool hasFeesDeleteRight = true;
    private bool hasFeesAddEditRight = true;
    private IContainer components;
    private GroupContainer grpTPOFees;
    private StandardIconButton btnDeleteSetting;
    private StandardIconButton btnEditSetting;
    private StandardIconButton btnAddSetting;
    private GradientPanel gradientPanel1;
    private GridView gvFees;
    private ComboBox cmbChannelType;
    private Label lblChannelType;
    private Label lblStatus;
    private ComboBox cmbStatus;
    private Button btnLateFeeSettings;
    private PictureBox picSearch;
    private TextBox txtSearch;
    private ImageList imgList;

    public TPOFees(Sessions.Session session)
      : this(session, false)
    {
    }

    public TPOFees(Sessions.Session session, bool allowMultiSelect)
    {
      this.InitializeComponent();
      this.session = session;
      FeaturesAclManager aclManager = (FeaturesAclManager) session.ACL.GetAclManager(AclCategory.Features);
      this.hasFeesAddEditRight = aclManager.GetUserApplicationRight(AclFeature.ExternalSettings_EditTPOFees);
      this.hasFeesDeleteRight = aclManager.GetUserApplicationRight(AclFeature.ExternalSettings_DeleteTPOFees);
      this.cmbChannelType.SelectedIndexChanged -= new EventHandler(this.cmbChannelType_SelectedIndexChanged);
      this.cmbStatus.SelectedIndexChanged -= new EventHandler(this.cmbStatus_SelectedIndexChanged);
      this.cmbChannelType.SelectedIndex = 0;
      this.cmbStatus.SelectedIndex = 0;
      this.cmbChannelType.SelectedIndexChanged += new EventHandler(this.cmbChannelType_SelectedIndexChanged);
      this.cmbStatus.SelectedIndexChanged += new EventHandler(this.cmbStatus_SelectedIndexChanged);
      this.populateFees(true);
      this.gvFees_SelectedIndexChanged((object) null, (EventArgs) null);
      if (this.hasFeesAddEditRight)
        this.gvFees.ItemDoubleClick += new GVItemEventHandler(this.btnEditSetting_Click);
      this.setIconButton();
      this.gvFees.AllowMultiselect = allowMultiSelect;
    }

    private void populateFees(bool reload)
    {
      this.gvFees.Items.Clear();
      if (reload || this.fees == null)
        this.fees = this.session.ConfigurationManager.GetFeeManagement(-1);
      foreach (ExternalFeeManagement fee in this.fees)
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

    private void populateFees(List<ExternalFeeManagement> feesPop)
    {
      this.gvFees.Items.Clear();
      foreach (ExternalFeeManagement fee in feesPop)
      {
        bool flag = true;
        if (this.cmbChannelType.SelectedItem.ToString() != "All" && fee.Channel.ToString() != this.cmbChannelType.SelectedItem.ToString())
          flag = false;
        if (this.cmbStatus.SelectedItem.ToString() != "All" && fee.Status.ToString() != this.cmbStatus.SelectedItem.ToString())
          flag = false;
        if (this.txtSearch.Text != "" && fee.FeeName.ToUpper() != this.txtSearch.Text.ToUpper())
          flag = false;
        if (flag)
          this.gvFees.Items.Add(this.createGVItem(fee));
      }
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

    private void btnAddSetting_Click(object sender, EventArgs e)
    {
      using (AddEditTPOFee addEditTpoFee = new AddEditTPOFee(this.session, (ExternalFeeManagement) null))
      {
        if (addEditTpoFee.ShowDialog((IWin32Window) this) == DialogResult.OK)
          this.session.ConfigurationManager.InsertFeeManagementSettings(addEditTpoFee.Fees, -1);
      }
      this.populateFees(true);
    }

    private void btnEditSetting_Click(object sender, EventArgs e)
    {
      if (this.gvFees.SelectedItems.Count == 0)
        return;
      using (AddEditTPOFee addEditTpoFee = new AddEditTPOFee(this.session, (ExternalFeeManagement) this.gvFees.SelectedItems[0].Tag))
      {
        if (addEditTpoFee.ShowDialog((IWin32Window) this) == DialogResult.OK)
          this.session.ConfigurationManager.UpdateFeeManagementSettings(addEditTpoFee.Fees);
      }
      this.populateFees(true);
    }

    private void btnDeleteSetting_Click(object sender, EventArgs e)
    {
      if (this.gvFees.SelectedItems.Count <= 0 || Utils.Dialog((IWin32Window) this, "Are you sure you want to delete the selected fee(s)?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
        return;
      List<int> feeManagementIDs = new List<int>();
      foreach (GVItem selectedItem in this.gvFees.SelectedItems)
        feeManagementIDs.Add(((ExternalFeeManagement) selectedItem.Tag).FeeManagementID);
      this.session.ConfigurationManager.DeleteFeeManagementSettings(feeManagementIDs);
      this.populateFees(true);
    }

    private void btnLateFeeSettings_Click(object sender, EventArgs e)
    {
      using (LateFeeSettings lateFeeSettings = new LateFeeSettings(this.session, -1, this.session.ConfigurationManager.GetGlobalLateFeeSettings(), 0, !this.hasFeesAddEditRight))
      {
        int num = (int) lateFeeSettings.ShowDialog((IWin32Window) this);
      }
    }

    private void gvFees_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.btnEditSetting.Enabled = this.hasFeesAddEditRight && this.gvFees.SelectedItems.Count == 1;
      this.btnAddSetting.Enabled = this.hasFeesAddEditRight;
      this.btnDeleteSetting.Enabled = this.hasFeesDeleteRight && this.gvFees.SelectedItems.Count > 0;
    }

    private void cmbChannelType_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.populateFees(false);
    }

    private void cmbStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.populateFees(false);
    }

    private void picSearch_Click(object sender, EventArgs e)
    {
      if (this.txtSearch.Text == string.Empty)
        this.populateFees(false);
      else
        this.populateFees(this.fees.Where<ExternalFeeManagement>((Func<ExternalFeeManagement, bool>) (a => a.FeeName.ToUpper().Contains(this.txtSearch.Text.ToUpper()))).ToList<ExternalFeeManagement>());
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

    string IOnlineHelpTarget.GetHelpTargetName() => "Setup\\TPO Fees";

    public string[] SelectedFees
    {
      get
      {
        return this.gvFees.SelectedItems.Count == 0 ? (string[]) null : this.gvFees.SelectedItems.Select<GVItem, string>((Func<GVItem, string>) (item => ((ExternalFeeManagement) item.Tag).FeeManagementID.ToString() + "_" + ((ExternalFeeManagement) item.Tag).FeeName)).ToArray<string>();
      }
      set
      {
        for (int index = 0; index < value.Length; ++index)
        {
          for (int nItemIndex = 0; nItemIndex < this.gvFees.Items.Count; ++nItemIndex)
          {
            if (Convert.ToString(((ExternalFeeManagement) this.gvFees.Items[nItemIndex].Tag).FeeManagementID) == value[index].Split('_')[0])
            {
              this.gvFees.Items[nItemIndex].Selected = true;
              break;
            }
          }
        }
      }
    }

    public string[] SelectedLoanCustomFields
    {
      get
      {
        if (this.gvFees.SelectedItems.Count == 0)
          return (string[]) null;
        IEnumerable<string> strings = this.gvFees.SelectedItems.Select<GVItem, string>((Func<GVItem, string>) (item => ((ExternalFeeManagement) item.Tag).AdvancedCodeXml));
        List<string> stringList = new List<string>();
        foreach (string xmlData in strings)
        {
          if (!string.IsNullOrEmpty(xmlData))
          {
            FieldFilterList fieldFilterList = (FieldFilterList) new XmlSerializer().Deserialize(xmlData, typeof (FieldFilterList));
            stringList.AddRange((IEnumerable<string>) ((IEnumerable<string>) fieldFilterList.GetFieldIDList()).ToList<string>());
          }
        }
        return stringList.Count == 0 ? (string[]) null : stringList.ToArray();
      }
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (TPOFees));
      this.grpTPOFees = new GroupContainer();
      this.gvFees = new GridView();
      this.gradientPanel1 = new GradientPanel();
      this.picSearch = new PictureBox();
      this.txtSearch = new TextBox();
      this.btnLateFeeSettings = new Button();
      this.cmbStatus = new ComboBox();
      this.lblStatus = new Label();
      this.cmbChannelType = new ComboBox();
      this.lblChannelType = new Label();
      this.btnDeleteSetting = new StandardIconButton();
      this.btnEditSetting = new StandardIconButton();
      this.btnAddSetting = new StandardIconButton();
      this.imgList = new ImageList(this.components);
      this.grpTPOFees.SuspendLayout();
      this.gradientPanel1.SuspendLayout();
      ((ISupportInitialize) this.picSearch).BeginInit();
      ((ISupportInitialize) this.btnDeleteSetting).BeginInit();
      ((ISupportInitialize) this.btnEditSetting).BeginInit();
      ((ISupportInitialize) this.btnAddSetting).BeginInit();
      this.SuspendLayout();
      this.grpTPOFees.Controls.Add((Control) this.gvFees);
      this.grpTPOFees.Controls.Add((Control) this.gradientPanel1);
      this.grpTPOFees.Controls.Add((Control) this.btnDeleteSetting);
      this.grpTPOFees.Controls.Add((Control) this.btnEditSetting);
      this.grpTPOFees.Controls.Add((Control) this.btnAddSetting);
      this.grpTPOFees.Dock = DockStyle.Fill;
      this.grpTPOFees.HeaderForeColor = SystemColors.ControlText;
      this.grpTPOFees.Location = new Point(0, 0);
      this.grpTPOFees.Name = "grpTPOFees";
      this.grpTPOFees.Size = new Size(769, 567);
      this.grpTPOFees.TabIndex = 0;
      this.grpTPOFees.Text = "TPO Fees";
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
      gvColumn5.SortMethod = GVSortMethod.Numeric;
      gvColumn5.Text = "Fee Amount";
      gvColumn5.Width = 100;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "Column5";
      gvColumn6.Text = "Code";
      gvColumn6.Width = 50;
      gvColumn7.ImageIndex = -1;
      gvColumn7.Name = "Column6";
      gvColumn7.SortMethod = GVSortMethod.DateTime;
      gvColumn7.Text = "Start Date";
      gvColumn7.Width = 150;
      gvColumn8.ImageIndex = -1;
      gvColumn8.Name = "Column7";
      gvColumn8.SortMethod = GVSortMethod.DateTime;
      gvColumn8.Text = "End Date";
      gvColumn8.Width = 150;
      gvColumn9.ImageIndex = -1;
      gvColumn9.Name = "Column8";
      gvColumn9.SortMethod = GVSortMethod.DateTime;
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
      this.gvFees.Dock = DockStyle.Fill;
      this.gvFees.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvFees.Location = new Point(1, 64);
      this.gvFees.Name = "gvFees";
      this.gvFees.Size = new Size(767, 502);
      this.gvFees.TabIndex = 8;
      this.gvFees.SelectedIndexChanged += new EventHandler(this.gvFees_SelectedIndexChanged);
      this.gradientPanel1.Controls.Add((Control) this.picSearch);
      this.gradientPanel1.Controls.Add((Control) this.txtSearch);
      this.gradientPanel1.Controls.Add((Control) this.btnLateFeeSettings);
      this.gradientPanel1.Controls.Add((Control) this.cmbStatus);
      this.gradientPanel1.Controls.Add((Control) this.lblStatus);
      this.gradientPanel1.Controls.Add((Control) this.cmbChannelType);
      this.gradientPanel1.Controls.Add((Control) this.lblChannelType);
      this.gradientPanel1.Dock = DockStyle.Top;
      this.gradientPanel1.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gradientPanel1.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gradientPanel1.Location = new Point(1, 26);
      this.gradientPanel1.Name = "gradientPanel1";
      this.gradientPanel1.Size = new Size(767, 38);
      this.gradientPanel1.TabIndex = 7;
      this.picSearch.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.picSearch.BackColor = Color.Transparent;
      this.picSearch.Location = new Point(621, 10);
      this.picSearch.Name = "picSearch";
      this.picSearch.Size = new Size(20, 20);
      this.picSearch.TabIndex = 37;
      this.picSearch.TabStop = false;
      this.picSearch.Click += new EventHandler(this.picSearch_Click);
      this.picSearch.MouseEnter += new EventHandler(this.picSearch_MouseEnter);
      this.picSearch.MouseLeave += new EventHandler(this.picSearch_MouseLeave);
      this.txtSearch.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.txtSearch.Location = new Point(442, 10);
      this.txtSearch.Name = "txtSearch";
      this.txtSearch.Size = new Size(176, 20);
      this.txtSearch.TabIndex = 36;
      this.btnLateFeeSettings.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnLateFeeSettings.Location = new Point(656, 8);
      this.btnLateFeeSettings.Name = "btnLateFeeSettings";
      this.btnLateFeeSettings.Size = new Size(104, 23);
      this.btnLateFeeSettings.TabIndex = 4;
      this.btnLateFeeSettings.Text = "Late Fee Settings";
      this.btnLateFeeSettings.UseVisualStyleBackColor = true;
      this.btnLateFeeSettings.Click += new EventHandler(this.btnLateFeeSettings_Click);
      this.cmbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbStatus.FormattingEnabled = true;
      this.cmbStatus.Items.AddRange(new object[4]
      {
        (object) "All",
        (object) "Pending",
        (object) "Active",
        (object) "Expired"
      });
      this.cmbStatus.Location = new Point(60, 10);
      this.cmbStatus.Name = "cmbStatus";
      this.cmbStatus.Size = new Size(121, 21);
      this.cmbStatus.TabIndex = 3;
      this.cmbStatus.SelectedIndexChanged += new EventHandler(this.cmbStatus_SelectedIndexChanged);
      this.lblStatus.AutoSize = true;
      this.lblStatus.BackColor = Color.Transparent;
      this.lblStatus.Location = new Point(14, 13);
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
      this.cmbChannelType.Location = new Point(289, 10);
      this.cmbChannelType.Name = "cmbChannelType";
      this.cmbChannelType.Size = new Size(121, 21);
      this.cmbChannelType.TabIndex = 1;
      this.cmbChannelType.Visible = false;
      this.cmbChannelType.SelectedIndexChanged += new EventHandler(this.cmbChannelType_SelectedIndexChanged);
      this.lblChannelType.AutoSize = true;
      this.lblChannelType.BackColor = Color.Transparent;
      this.lblChannelType.Location = new Point(208, 13);
      this.lblChannelType.Name = "lblChannelType";
      this.lblChannelType.Size = new Size(76, 13);
      this.lblChannelType.TabIndex = 0;
      this.lblChannelType.Text = "Channel Type:";
      this.lblChannelType.Visible = false;
      this.btnDeleteSetting.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnDeleteSetting.BackColor = Color.Transparent;
      this.btnDeleteSetting.Location = new Point(745, 5);
      this.btnDeleteSetting.Margin = new Padding(2, 3, 2, 3);
      this.btnDeleteSetting.MouseDownImage = (Image) null;
      this.btnDeleteSetting.Name = "btnDeleteSetting";
      this.btnDeleteSetting.Size = new Size(16, 17);
      this.btnDeleteSetting.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnDeleteSetting.TabIndex = 6;
      this.btnDeleteSetting.TabStop = false;
      this.btnDeleteSetting.Click += new EventHandler(this.btnDeleteSetting_Click);
      this.btnEditSetting.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnEditSetting.BackColor = Color.Transparent;
      this.btnEditSetting.Location = new Point(725, 4);
      this.btnEditSetting.Margin = new Padding(2, 3, 2, 3);
      this.btnEditSetting.MouseDownImage = (Image) null;
      this.btnEditSetting.Name = "btnEditSetting";
      this.btnEditSetting.Size = new Size(16, 18);
      this.btnEditSetting.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.btnEditSetting.TabIndex = 5;
      this.btnEditSetting.TabStop = false;
      this.btnEditSetting.Click += new EventHandler(this.btnEditSetting_Click);
      this.btnAddSetting.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnAddSetting.BackColor = Color.Transparent;
      this.btnAddSetting.Location = new Point(705, 5);
      this.btnAddSetting.Margin = new Padding(2, 3, 2, 3);
      this.btnAddSetting.MouseDownImage = (Image) null;
      this.btnAddSetting.Name = "btnAddSetting";
      this.btnAddSetting.Size = new Size(16, 17);
      this.btnAddSetting.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnAddSetting.TabIndex = 4;
      this.btnAddSetting.TabStop = false;
      this.btnAddSetting.Click += new EventHandler(this.btnAddSetting_Click);
      this.imgList.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imgList.ImageStream");
      this.imgList.TransparentColor = Color.Transparent;
      this.imgList.Images.SetKeyName(0, "picSearch");
      this.imgList.Images.SetKeyName(1, "picSearchMouseOver");
      this.imgList.Images.SetKeyName(2, "picSearchDisabled");
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.grpTPOFees);
      this.Name = nameof (TPOFees);
      this.Size = new Size(769, 567);
      this.grpTPOFees.ResumeLayout(false);
      this.gradientPanel1.ResumeLayout(false);
      this.gradientPanel1.PerformLayout();
      ((ISupportInitialize) this.picSearch).EndInit();
      ((ISupportInitialize) this.btnDeleteSetting).EndInit();
      ((ISupportInitialize) this.btnEditSetting).EndInit();
      ((ISupportInitialize) this.btnAddSetting).EndInit();
      this.ResumeLayout(false);
    }
  }
}
