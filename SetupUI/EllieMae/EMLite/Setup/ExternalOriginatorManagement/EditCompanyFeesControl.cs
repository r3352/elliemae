// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ExternalOriginatorManagement.EditCompanyFeesControl
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.ExternalOriginatorManagement;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Setup.ExternalOriginatorManagement.RestApi;
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
  public class EditCompanyFeesControl : UserControl
  {
    private Sessions.Session session;
    private IConfigurationManager mngr;
    private SessionObjects obj;
    private int oid;
    private int dataoid;
    private ExternalOriginatorManagementData parent;
    private ExternalOriginatorEntityType channel;
    private bool edit;
    private List<ExternalFeeManagement> fees;
    private bool hasFeesDeleteRight = true;
    private bool hasFeesAddEditRight = true;
    private bool readOnly;
    private IContainer components;
    private GroupContainer grpAll;
    private Panel panelHeader;
    private Label label33;
    private GradientPanel gradientPanel1;
    private Button btnLateFeeSettings;
    private ComboBox cmbStatus;
    private Label lblStatus;
    private ComboBox cmbChannelType;
    private Label lblChannelType;
    private GroupContainer grpTPOFees;
    private GridView gvFees;
    private StandardIconButton btnDeleteSetting;
    private StandardIconButton btnEditSetting;
    private StandardIconButton btnAddSetting;
    private GroupContainer grpFeeDetails;
    private StandardIconButton btnDeleteSetting1;
    private StandardIconButton btnEditSetting1;
    private Panel panel1;
    private Label label1;
    private Panel pnlFeeDetails;
    private Label lblLastUpdated;
    private Label label18;
    private Label lblCreatedBy;
    private Label label15;
    private Label lblFeeTrigger;
    private Label label13;
    private Label lblEndDate;
    private Label label11;
    private Label lblStartDate;
    private Label label9;
    private Label lblCode;
    private Label label7;
    private Label lblFeeAmt;
    private Label label5;
    private Label lblChannel;
    private Label label3;
    private Label lblDescription;
    private Label lblDesc;
    private Label lblFeeName;
    private Button btnResetToDefault;
    private Label lblCondition;
    private CollapsibleSplitter collapsibleSplitter1;
    private Label label2;
    private PictureBox picSearch;
    private TextBox txtSearch;
    private ImageList imgList;
    private Label lblDateUpdated;
    private Label label6;
    private Label lblDateCreated;
    private Label label10;

    public EditCompanyFeesControl(SessionObjects obj, int oid, bool edit, bool isTPOTool)
    {
      this.obj = obj;
      this.oid = oid;
      this.mngr = obj.ConfigurationManager;
      if (oid != -1)
        this.channel = this.mngr.GetExternalOrganization(false, oid).entityType;
      this.parent = this.mngr.GetRootOrganisation(false, oid);
      this.readOnly = this.parent == null || this.parent.oid != this.oid;
      if (isTPOTool)
        this.readOnly = true;
      if (this.parent != null)
        this.dataoid = this.parent.oid;
      this.edit = edit;
      this.InitializeComponent();
      this.Dock = DockStyle.Fill;
      this.populateFees(true);
      if (!isTPOTool)
        return;
      this.DisableControls();
    }

    public EditCompanyFeesControl(Sessions.Session session, int oid, bool edit, bool isTPOTool)
    {
      this.session = session;
      this.mngr = session.ConfigurationManager;
      this.oid = oid;
      if (oid != -1)
        this.channel = this.mngr.GetExternalOrganization(false, oid).entityType;
      this.parent = this.mngr.GetRootOrganisation(false, oid);
      this.readOnly = this.parent == null || this.parent.oid != this.oid;
      if (isTPOTool)
        this.readOnly = true;
      if (this.parent != null)
        this.dataoid = this.parent.oid;
      this.edit = edit;
      this.InitializeComponent();
      this.Dock = DockStyle.Fill;
      FeaturesAclManager aclManager = (FeaturesAclManager) session.ACL.GetAclManager(AclCategory.Features);
      this.hasFeesAddEditRight = aclManager.GetUserApplicationRight(AclFeature.ExternalSettings_EditTPOFeesTab);
      this.hasFeesDeleteRight = aclManager.GetUserApplicationRight(AclFeature.ExternalSettings_DeleteTPOFeesTab);
      this.cmbChannelType.SelectedIndexChanged -= new EventHandler(this.cmbChannelType_SelectedIndexChanged);
      this.cmbStatus.SelectedIndexChanged -= new EventHandler(this.cmbStatus_SelectedIndexChanged);
      this.cmbChannelType.SelectedIndex = 0;
      this.cmbStatus.SelectedIndex = 0;
      this.cmbChannelType.SelectedIndexChanged += new EventHandler(this.cmbChannelType_SelectedIndexChanged);
      this.cmbStatus.SelectedIndexChanged += new EventHandler(this.cmbStatus_SelectedIndexChanged);
      this.populateFees(true);
      this.gvFees_SelectedIndexChanged((object) null, (EventArgs) null);
      if (!this.readOnly && this.hasFeesAddEditRight)
        this.gvFees.ItemDoubleClick += new GVItemEventHandler(this.btnEditSetting_Click);
      this.btnResetToDefault.Enabled = !this.readOnly && this.hasFeesAddEditRight;
      this.setIconButton();
      if (!isTPOTool)
        return;
      this.DisableControls();
    }

    public void DisableControls()
    {
      this.btnEditSetting1.Visible = this.btnEditSetting.Visible = false;
      this.btnAddSetting.Visible = false;
      this.btnDeleteSetting1.Visible = this.btnDeleteSetting.Visible = false;
    }

    private void populateFees(bool reload)
    {
      this.gvFees.Items.Clear();
      if (reload || this.fees == null)
        this.fees = this.mngr.GetFeeManagement(this.dataoid);
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
      this.gvFees.ReSort();
    }

    private GVItem createGVItem(ExternalFeeManagement fee)
    {
      GVItem gvItem = new GVItem();
      gvItem.SubItems.Add((object) fee.FeeName);
      gvItem.SubItems.Add(fee.Description.Length > 50 ? (object) (fee.Description.Substring(0, Math.Min(fee.Description.Length, 50)) + "...") : (object) fee.Description);
      gvItem.SubItems.Add(fee.Channel == ExternalOriginatorEntityType.Both ? (object) "All" : (object) fee.Channel.ToString());
      GVSubItemCollection subItems1 = gvItem.SubItems;
      double num;
      string str1;
      if (fee.FeePercent != 0.0)
      {
        num = fee.FeePercent;
        str1 = num.ToString();
      }
      else
        str1 = "";
      subItems1.Add((object) str1);
      GVSubItemCollection subItems2 = gvItem.SubItems;
      string str2;
      if (fee.FeeAmount != 0.0)
      {
        num = fee.FeeAmount;
        str2 = num.ToString();
      }
      else
        str2 = "";
      subItems2.Add((object) str2);
      gvItem.SubItems.Add((object) fee.Code);
      GVSubItemCollection subItems3 = gvItem.SubItems;
      DateTime dateTime;
      string str3;
      if (!(fee.StartDate != DateTime.MinValue))
      {
        str3 = "";
      }
      else
      {
        dateTime = fee.StartDate;
        str3 = dateTime.ToString("d");
      }
      subItems3.Add((object) str3);
      GVSubItemCollection subItems4 = gvItem.SubItems;
      string str4;
      if (!(fee.EndDate != DateTime.MinValue))
      {
        str4 = "";
      }
      else
      {
        dateTime = fee.EndDate;
        str4 = dateTime.ToString("d");
      }
      subItems4.Add((object) str4);
      GVSubItemCollection subItems5 = gvItem.SubItems;
      string str5;
      if (!(fee.DateUpdated != DateTime.MinValue))
      {
        str5 = "";
      }
      else
      {
        dateTime = fee.DateUpdated;
        str5 = dateTime.ToString("d");
      }
      subItems5.Add((object) str5);
      gvItem.SubItems.Add((object) fee.UpdatedBy);
      gvItem.SubItems.Add((object) fee.Status);
      gvItem.Tag = (object) fee;
      return gvItem;
    }

    private void btnResetToDefault_Click(object sender, EventArgs e)
    {
      if (Utils.Dialog((IWin32Window) this, "Are you sure you want to reset to default fees? This will delete all recent updates to TPO fees on this tab and reset to the first configuration.", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
        return;
      this.mngr.DeleteTPOFeeManagementSettings(this.oid);
      this.mngr.SetDefaultFeeManagementListByChannel(this.oid, this.channel);
      WebhookApiHelper.PublishExternalOrgWebhookEvent(this.session.UserID, this.Parent.Text, this.oid);
      this.populateFees(true);
    }

    private void btnLateFeeSettings_Click(object sender, EventArgs e)
    {
      using (LateFeeSettings lateFeeSettings = new LateFeeSettings(this.session, this.dataoid, this.mngr.GetExternalOrgLateFeeSettings(this.dataoid, false), this.mngr.GetGlobalOrSpecificTPOSetting(this.dataoid), this.readOnly || !this.hasFeesAddEditRight, this.Parent.Text))
      {
        int num = (int) lateFeeSettings.ShowDialog((IWin32Window) this);
      }
    }

    private void btnAddSetting_Click(object sender, EventArgs e)
    {
      using (AddTabNewTPOFees addTabNewTpoFees = new AddTabNewTPOFees())
      {
        if (addTabNewTpoFees.ShowDialog((IWin32Window) this) == DialogResult.OK)
        {
          if (addTabNewTpoFees.Value == 0)
          {
            using (AddEditTPOFee addEditTpoFee = new AddEditTPOFee(this.session, (ExternalFeeManagement) null))
            {
              if (addEditTpoFee.ShowDialog((IWin32Window) this) == DialogResult.OK)
                this.mngr.InsertFeeManagementSettings(addEditTpoFee.Fees, this.dataoid);
            }
          }
          else
          {
            using (AddNewTPOFeesFromSettings feesFromSettings = new AddNewTPOFeesFromSettings(this.mngr.GetFeeManagementListByChannel(this.channel)))
            {
              if (feesFromSettings.ShowDialog((IWin32Window) this) == DialogResult.OK)
              {
                List<ExternalFeeManagement> externalFeeManagementList = feesFromSettings.Value;
                if (externalFeeManagementList.Count > 0)
                {
                  IEnumerable<\u003C\u003Ef__AnonymousType13<string>> source = externalFeeManagementList.Cast<ExternalFeeManagement>().Join((IEnumerable<ExternalFeeManagement>) this.fees, sf => new
                  {
                    FeeName = sf.FeeName
                  }, ff => new{ FeeName = ff.FeeName }, (sf, ff) => new
                  {
                    FeeName = sf.FeeName
                  });
                  string str = "";
                  foreach (var data in source)
                    str = str + data.FeeName + "\n";
                  if (str != "")
                    str = str.Substring(0, str.Length - 1);
                  if (source.Count() > 0)
                  {
                    if (Utils.Dialog((IWin32Window) this, "Some of the fees with same name already exist in this company. If you proceed, you will have duplicate fees. Do you want to proceed?\n\n" + str, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                      this.mngr.SetSelectedFeeManagementList(this.oid, externalFeeManagementList);
                  }
                  else
                    this.mngr.SetSelectedFeeManagementList(this.oid, externalFeeManagementList);
                }
              }
            }
          }
        }
      }
      WebhookApiHelper.PublishExternalOrgWebhookEvent(this.session.UserID, this.Parent.Text, this.oid);
      this.populateFees(true);
    }

    private void btnEditSetting_Click(object sender, EventArgs e)
    {
      if (this.gvFees.SelectedItems.Count == 0)
        return;
      using (AddEditTPOFee addEditTpoFee = new AddEditTPOFee(this.session, (ExternalFeeManagement) this.gvFees.SelectedItems[0].Tag))
      {
        if (addEditTpoFee.ShowDialog((IWin32Window) this) == DialogResult.OK)
          this.mngr.UpdateFeeManagementSettings(addEditTpoFee.Fees);
      }
      WebhookApiHelper.PublishExternalOrgWebhookEvent(this.session.UserID, this.Parent.Text, this.oid);
      this.populateFees(true);
    }

    private void btnDeleteSetting_Click(object sender, EventArgs e)
    {
      if (this.gvFees.SelectedItems.Count <= 0 || Utils.Dialog((IWin32Window) this, "Are you sure you want to delete the selected fee(s)?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
        return;
      List<int> feeManagementIDs = new List<int>();
      foreach (GVItem selectedItem in this.gvFees.SelectedItems)
        feeManagementIDs.Add(((ExternalFeeManagement) selectedItem.Tag).FeeManagementID);
      this.mngr.DeleteFeeManagementSettings(feeManagementIDs);
      WebhookApiHelper.PublishExternalOrgWebhookEvent(this.session.UserID, this.Parent.Text, this.oid);
      this.populateFees(true);
    }

    private void cmbStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.populateFees(false);
    }

    private void cmbChannelType_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.populateFees(false);
    }

    private void gvFees_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.btnEditSetting1.Enabled = this.btnEditSetting.Enabled = !this.readOnly && this.hasFeesAddEditRight && this.gvFees.SelectedItems.Count == 1;
      this.btnAddSetting.Enabled = !this.readOnly && this.hasFeesAddEditRight;
      this.btnDeleteSetting1.Enabled = this.btnDeleteSetting.Enabled = !this.readOnly && this.hasFeesDeleteRight && this.gvFees.SelectedItems.Count > 0;
      this.populateDetails();
    }

    private void populateDetails()
    {
      if (this.gvFees.SelectedItems.Count == 1)
      {
        ExternalFeeManagement tag = (ExternalFeeManagement) this.gvFees.SelectedItems[0].Tag;
        this.lblFeeName.Text = tag.FeeName;
        this.lblDescription.Text = tag.Description.Replace('\n', ' ');
        this.lblChannel.Text = tag.Channel.ToString();
        Label lblFeeAmt = this.lblFeeAmt;
        string[] strArray = new string[5];
        double num = tag.FeePercent;
        strArray[0] = num.ToString("#,0.00");
        strArray[1] = " % of ";
        strArray[2] = tag.FeeBasedOn == 0 ? "Total Loan Amount" : "Base Loan Amount";
        strArray[3] = " + $";
        num = tag.FeeAmount;
        strArray[4] = num.ToString("#,0.00");
        string str = string.Concat(strArray);
        lblFeeAmt.Text = str;
        this.lblCode.Text = tag.Code.Substring(0, Math.Min(30, tag.Code.Length)) + (tag.Code.Length > 30 ? "..." : "");
        this.lblStartDate.Text = tag.StartDate != DateTime.MinValue ? tag.StartDate.ToString("d") : "";
        this.lblEndDate.Text = tag.EndDate != DateTime.MinValue ? tag.EndDate.ToString("d") : "";
        this.lblFeeTrigger.Text = tag.Condition == 0 ? "Always" : "Conditional";
        this.lblCondition.Text = tag.Condition == 1 ? tag.AdvancedCode.Substring(0, Math.Min(200, tag.AdvancedCode.Length)) + (tag.AdvancedCode.Length > 200 ? "..." : "") : "";
        this.label2.Visible = tag.Condition == 1;
        this.lblCreatedBy.Text = tag.CreatedBy;
        this.lblDateCreated.Text = tag.DateCreated != DateTime.MinValue ? tag.DateCreated.ToString("d") : "";
        this.lblLastUpdated.Text = tag.UpdatedBy;
        this.lblDateUpdated.Text = tag.DateUpdated != DateTime.MinValue ? tag.DateUpdated.ToString("d") : "";
        if (!this.collapsibleSplitter1.IsCollapsed)
          return;
        this.collapsibleSplitter1.ToggleState();
      }
      else
      {
        if (this.collapsibleSplitter1.IsCollapsed)
          return;
        this.collapsibleSplitter1.ToggleState();
      }
    }

    private void collapsibleSplitter1_Click(object sender, EventArgs e)
    {
      if (this.gvFees.SelectedItems.Count != 0 || this.collapsibleSplitter1.IsCollapsed)
        return;
      this.collapsibleSplitter1.ToggleState();
    }

    public void AssignOid(int oid)
    {
      if (this.oid == -1)
        this.oid = oid;
      if (oid != -1)
        this.channel = this.mngr.GetExternalOrganization(false, oid).entityType;
      this.parent = this.mngr.GetRootOrganisation(false, oid);
      this.readOnly = this.parent == null || this.parent.oid != this.oid;
      if (this.parent != null)
        this.dataoid = this.parent.oid;
      this.btnResetToDefault.Enabled = !this.readOnly && this.hasFeesAddEditRight;
      this.populateFees(true);
      this.gvFees_SelectedIndexChanged((object) null, (EventArgs) null);
    }

    public int Oid => this.oid;

    public ExternalOriginatorEntityType Channel => this.channel;

    public void ChangeChannel(ExternalOriginatorEntityType channel) => this.channel = channel;

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

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (EditCompanyFeesControl));
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
      this.imgList = new ImageList(this.components);
      this.grpAll = new GroupContainer();
      this.grpTPOFees = new GroupContainer();
      this.gvFees = new GridView();
      this.collapsibleSplitter1 = new CollapsibleSplitter();
      this.grpFeeDetails = new GroupContainer();
      this.pnlFeeDetails = new Panel();
      this.lblDateUpdated = new Label();
      this.label6 = new Label();
      this.lblDateCreated = new Label();
      this.label10 = new Label();
      this.label2 = new Label();
      this.lblCondition = new Label();
      this.lblLastUpdated = new Label();
      this.label18 = new Label();
      this.lblCreatedBy = new Label();
      this.label15 = new Label();
      this.lblFeeTrigger = new Label();
      this.label13 = new Label();
      this.lblEndDate = new Label();
      this.label11 = new Label();
      this.lblStartDate = new Label();
      this.label9 = new Label();
      this.lblCode = new Label();
      this.label7 = new Label();
      this.lblFeeAmt = new Label();
      this.label5 = new Label();
      this.lblChannel = new Label();
      this.label3 = new Label();
      this.lblDescription = new Label();
      this.lblDesc = new Label();
      this.lblFeeName = new Label();
      this.btnDeleteSetting1 = new StandardIconButton();
      this.btnEditSetting1 = new StandardIconButton();
      this.panel1 = new Panel();
      this.label1 = new Label();
      this.btnDeleteSetting = new StandardIconButton();
      this.btnEditSetting = new StandardIconButton();
      this.btnAddSetting = new StandardIconButton();
      this.gradientPanel1 = new GradientPanel();
      this.picSearch = new PictureBox();
      this.txtSearch = new TextBox();
      this.btnResetToDefault = new Button();
      this.btnLateFeeSettings = new Button();
      this.cmbStatus = new ComboBox();
      this.lblStatus = new Label();
      this.cmbChannelType = new ComboBox();
      this.lblChannelType = new Label();
      this.panelHeader = new Panel();
      this.label33 = new Label();
      this.grpAll.SuspendLayout();
      this.grpTPOFees.SuspendLayout();
      this.grpFeeDetails.SuspendLayout();
      this.pnlFeeDetails.SuspendLayout();
      ((ISupportInitialize) this.btnDeleteSetting1).BeginInit();
      ((ISupportInitialize) this.btnEditSetting1).BeginInit();
      this.panel1.SuspendLayout();
      ((ISupportInitialize) this.btnDeleteSetting).BeginInit();
      ((ISupportInitialize) this.btnEditSetting).BeginInit();
      ((ISupportInitialize) this.btnAddSetting).BeginInit();
      this.gradientPanel1.SuspendLayout();
      ((ISupportInitialize) this.picSearch).BeginInit();
      this.panelHeader.SuspendLayout();
      this.SuspendLayout();
      this.imgList.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imgList.ImageStream");
      this.imgList.TransparentColor = Color.Transparent;
      this.imgList.Images.SetKeyName(0, "picSearch");
      this.imgList.Images.SetKeyName(1, "picSearchMouseOver");
      this.imgList.Images.SetKeyName(2, "picSearchDisabled");
      this.grpAll.Controls.Add((Control) this.grpTPOFees);
      this.grpAll.Controls.Add((Control) this.grpFeeDetails);
      this.grpAll.Controls.Add((Control) this.gradientPanel1);
      this.grpAll.Controls.Add((Control) this.panelHeader);
      this.grpAll.Dock = DockStyle.Fill;
      this.grpAll.HeaderForeColor = SystemColors.ControlText;
      this.grpAll.Location = new Point(5, 5);
      this.grpAll.Name = "grpAll";
      this.grpAll.Size = new Size(997, 676);
      this.grpAll.TabIndex = 0;
      this.grpAll.Text = "TPO Fees";
      this.grpTPOFees.Controls.Add((Control) this.gvFees);
      this.grpTPOFees.Controls.Add((Control) this.collapsibleSplitter1);
      this.grpTPOFees.Controls.Add((Control) this.btnDeleteSetting);
      this.grpTPOFees.Controls.Add((Control) this.btnEditSetting);
      this.grpTPOFees.Controls.Add((Control) this.btnAddSetting);
      this.grpTPOFees.Dock = DockStyle.Fill;
      this.grpTPOFees.HeaderForeColor = SystemColors.ControlText;
      this.grpTPOFees.Location = new Point(1, 90);
      this.grpTPOFees.Name = "grpTPOFees";
      this.grpTPOFees.Size = new Size(995, 317);
      this.grpTPOFees.TabIndex = 9;
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
      this.gvFees.Location = new Point(1, 26);
      this.gvFees.Name = "gvFees";
      this.gvFees.Size = new Size(993, 283);
      this.gvFees.TabIndex = 8;
      this.gvFees.SelectedIndexChanged += new EventHandler(this.gvFees_SelectedIndexChanged);
      this.collapsibleSplitter1.AnimationDelay = 20;
      this.collapsibleSplitter1.AnimationStep = 20;
      this.collapsibleSplitter1.BorderStyle3D = Border3DStyle.Flat;
      this.collapsibleSplitter1.ControlToHide = (Control) this.grpFeeDetails;
      this.collapsibleSplitter1.Dock = DockStyle.Bottom;
      this.collapsibleSplitter1.ExpandParentForm = false;
      this.collapsibleSplitter1.Location = new Point(1, 309);
      this.collapsibleSplitter1.Name = "collapsibleSplitter1";
      this.collapsibleSplitter1.TabIndex = 10;
      this.collapsibleSplitter1.TabStop = false;
      this.collapsibleSplitter1.UseAnimations = false;
      this.collapsibleSplitter1.VisualStyle = VisualStyles.Encompass;
      this.collapsibleSplitter1.Click += new EventHandler(this.collapsibleSplitter1_Click);
      this.grpFeeDetails.Controls.Add((Control) this.pnlFeeDetails);
      this.grpFeeDetails.Controls.Add((Control) this.btnDeleteSetting1);
      this.grpFeeDetails.Controls.Add((Control) this.btnEditSetting1);
      this.grpFeeDetails.Controls.Add((Control) this.panel1);
      this.grpFeeDetails.Dock = DockStyle.Bottom;
      this.grpFeeDetails.HeaderForeColor = SystemColors.ControlText;
      this.grpFeeDetails.Location = new Point(1, 407);
      this.grpFeeDetails.Name = "grpFeeDetails";
      this.grpFeeDetails.Size = new Size(995, 268);
      this.grpFeeDetails.TabIndex = 10;
      this.grpFeeDetails.Text = "Fee Details";
      this.pnlFeeDetails.BorderStyle = BorderStyle.FixedSingle;
      this.pnlFeeDetails.Controls.Add((Control) this.lblDateUpdated);
      this.pnlFeeDetails.Controls.Add((Control) this.label6);
      this.pnlFeeDetails.Controls.Add((Control) this.lblDateCreated);
      this.pnlFeeDetails.Controls.Add((Control) this.label10);
      this.pnlFeeDetails.Controls.Add((Control) this.label2);
      this.pnlFeeDetails.Controls.Add((Control) this.lblCondition);
      this.pnlFeeDetails.Controls.Add((Control) this.lblLastUpdated);
      this.pnlFeeDetails.Controls.Add((Control) this.label18);
      this.pnlFeeDetails.Controls.Add((Control) this.lblCreatedBy);
      this.pnlFeeDetails.Controls.Add((Control) this.label15);
      this.pnlFeeDetails.Controls.Add((Control) this.lblFeeTrigger);
      this.pnlFeeDetails.Controls.Add((Control) this.label13);
      this.pnlFeeDetails.Controls.Add((Control) this.lblEndDate);
      this.pnlFeeDetails.Controls.Add((Control) this.label11);
      this.pnlFeeDetails.Controls.Add((Control) this.lblStartDate);
      this.pnlFeeDetails.Controls.Add((Control) this.label9);
      this.pnlFeeDetails.Controls.Add((Control) this.lblCode);
      this.pnlFeeDetails.Controls.Add((Control) this.label7);
      this.pnlFeeDetails.Controls.Add((Control) this.lblFeeAmt);
      this.pnlFeeDetails.Controls.Add((Control) this.label5);
      this.pnlFeeDetails.Controls.Add((Control) this.lblChannel);
      this.pnlFeeDetails.Controls.Add((Control) this.label3);
      this.pnlFeeDetails.Controls.Add((Control) this.lblDescription);
      this.pnlFeeDetails.Controls.Add((Control) this.lblDesc);
      this.pnlFeeDetails.Controls.Add((Control) this.lblFeeName);
      this.pnlFeeDetails.Dock = DockStyle.Fill;
      this.pnlFeeDetails.Location = new Point(1, 52);
      this.pnlFeeDetails.Name = "pnlFeeDetails";
      this.pnlFeeDetails.Size = new Size(993, 215);
      this.pnlFeeDetails.TabIndex = 9;
      this.lblDateUpdated.AutoSize = true;
      this.lblDateUpdated.Location = new Point(745, 193);
      this.lblDateUpdated.Name = "lblDateUpdated";
      this.lblDateUpdated.Size = new Size(81, 13);
      this.lblDateUpdated.TabIndex = 24;
      this.lblDateUpdated.Text = "lblDateUpdated";
      this.label6.AutoSize = true;
      this.label6.ForeColor = SystemColors.ControlDarkDark;
      this.label6.Location = new Point(745, 171);
      this.label6.Name = "label6";
      this.label6.Size = new Size(77, 13);
      this.label6.TabIndex = 23;
      this.label6.Text = "Date Updated:";
      this.lblDateCreated.AutoSize = true;
      this.lblDateCreated.Location = new Point(745, 107);
      this.lblDateCreated.Name = "lblDateCreated";
      this.lblDateCreated.Size = new Size(77, 13);
      this.lblDateCreated.TabIndex = 22;
      this.lblDateCreated.Text = "lblDateCreated";
      this.label10.AutoSize = true;
      this.label10.ForeColor = SystemColors.ControlDarkDark;
      this.label10.Location = new Point(745, 85);
      this.label10.Name = "label10";
      this.label10.Size = new Size(73, 13);
      this.label10.TabIndex = 21;
      this.label10.Text = "Date Created:";
      this.label2.AutoSize = true;
      this.label2.ForeColor = SystemColors.ControlDarkDark;
      this.label2.Location = new Point(512, 95);
      this.label2.Name = "label2";
      this.label2.Size = new Size(106, 13);
      this.label2.TabIndex = 20;
      this.label2.Text = "Advanced Condition:";
      this.lblCondition.AutoSize = true;
      this.lblCondition.Location = new Point(512, 117);
      this.lblCondition.MaximumSize = new Size(220, 0);
      this.lblCondition.Name = "lblCondition";
      this.lblCondition.Size = new Size(218, 39);
      this.lblCondition.TabIndex = 19;
      this.lblCondition.Text = "dfhgjkdfghldkfghdlfkgjhdlsfjkghdsfjklghdfkjghsdfjkghdsfjkghdkfj,xcnbvxcmbxcjkvbxkjcvbdufghdfiub";
      this.lblLastUpdated.AutoSize = true;
      this.lblLastUpdated.Location = new Point(745, 148);
      this.lblLastUpdated.Name = "lblLastUpdated";
      this.lblLastUpdated.Size = new Size(78, 13);
      this.lblLastUpdated.TabIndex = 18;
      this.lblLastUpdated.Text = "lblLastUpdated";
      this.label18.AutoSize = true;
      this.label18.ForeColor = SystemColors.ControlDarkDark;
      this.label18.Location = new Point(745, 126);
      this.label18.Name = "label18";
      this.label18.Size = new Size(89, 13);
      this.label18.TabIndex = 17;
      this.label18.Text = "Last Updated By:";
      this.lblCreatedBy.AutoSize = true;
      this.lblCreatedBy.Location = new Point(745, 63);
      this.lblCreatedBy.Name = "lblCreatedBy";
      this.lblCreatedBy.Size = new Size(66, 13);
      this.lblCreatedBy.TabIndex = 16;
      this.lblCreatedBy.Text = "lblCreatedBy";
      this.label15.AutoSize = true;
      this.label15.ForeColor = SystemColors.ControlDarkDark;
      this.label15.Location = new Point(745, 41);
      this.label15.Name = "label15";
      this.label15.Size = new Size(62, 13);
      this.label15.TabIndex = 15;
      this.label15.Text = "Created By:";
      this.lblFeeTrigger.AutoSize = true;
      this.lblFeeTrigger.Location = new Point(512, 63);
      this.lblFeeTrigger.Name = "lblFeeTrigger";
      this.lblFeeTrigger.Size = new Size(68, 13);
      this.lblFeeTrigger.TabIndex = 14;
      this.lblFeeTrigger.Text = "lblFeeTrigger";
      this.label13.AutoSize = true;
      this.label13.ForeColor = SystemColors.ControlDarkDark;
      this.label13.Location = new Point(512, 41);
      this.label13.Name = "label13";
      this.label13.Size = new Size(75, 13);
      this.label13.TabIndex = 13;
      this.label13.Text = "Fee Condition:";
      this.lblEndDate.AutoSize = true;
      this.lblEndDate.Location = new Point(299, 171);
      this.lblEndDate.Name = "lblEndDate";
      this.lblEndDate.Size = new Size(59, 13);
      this.lblEndDate.TabIndex = 12;
      this.lblEndDate.Text = "lblEndDate";
      this.label11.AutoSize = true;
      this.label11.ForeColor = SystemColors.ControlDarkDark;
      this.label11.Location = new Point(299, 149);
      this.label11.Name = "label11";
      this.label11.Size = new Size(55, 13);
      this.label11.TabIndex = 11;
      this.label11.Text = "End Date:";
      this.lblStartDate.AutoSize = true;
      this.lblStartDate.Location = new Point(299, 117);
      this.lblStartDate.Name = "lblStartDate";
      this.lblStartDate.Size = new Size(62, 13);
      this.lblStartDate.TabIndex = 10;
      this.lblStartDate.Text = "lblStartDate";
      this.label9.AutoSize = true;
      this.label9.ForeColor = SystemColors.ControlDarkDark;
      this.label9.Location = new Point(299, 95);
      this.label9.Name = "label9";
      this.label9.Size = new Size(58, 13);
      this.label9.TabIndex = 9;
      this.label9.Text = "Start Date:";
      this.lblCode.AutoSize = true;
      this.lblCode.Location = new Point(25, 171);
      this.lblCode.Name = "lblCode";
      this.lblCode.Size = new Size(42, 13);
      this.lblCode.TabIndex = 8;
      this.lblCode.Text = "lblCode";
      this.label7.AutoSize = true;
      this.label7.ForeColor = SystemColors.ControlDarkDark;
      this.label7.Location = new Point(25, 149);
      this.label7.Name = "label7";
      this.label7.Size = new Size(35, 13);
      this.label7.TabIndex = 7;
      this.label7.Text = "Code:";
      this.lblFeeAmt.AutoSize = true;
      this.lblFeeAmt.Location = new Point(299, 63);
      this.lblFeeAmt.MaximumSize = new Size(200, 0);
      this.lblFeeAmt.Name = "lblFeeAmt";
      this.lblFeeAmt.Size = new Size(53, 13);
      this.lblFeeAmt.TabIndex = 6;
      this.lblFeeAmt.Text = "lblFeeAmt";
      this.label5.AutoSize = true;
      this.label5.ForeColor = SystemColors.ControlDarkDark;
      this.label5.Location = new Point(299, 41);
      this.label5.Name = "label5";
      this.label5.Size = new Size(67, 13);
      this.label5.TabIndex = 5;
      this.label5.Text = "Fee Amount:";
      this.lblChannel.AutoSize = true;
      this.lblChannel.Location = new Point(25, 126);
      this.lblChannel.Name = "lblChannel";
      this.lblChannel.Size = new Size(56, 13);
      this.lblChannel.TabIndex = 4;
      this.lblChannel.Text = "lblChannel";
      this.label3.AutoSize = true;
      this.label3.ForeColor = SystemColors.ControlDarkDark;
      this.label3.Location = new Point(25, 104);
      this.label3.Name = "label3";
      this.label3.Size = new Size(49, 13);
      this.label3.TabIndex = 3;
      this.label3.Text = "Channel:";
      this.lblDescription.AutoEllipsis = true;
      this.lblDescription.AutoSize = true;
      this.lblDescription.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 178);
      this.lblDescription.Location = new Point(25, 63);
      this.lblDescription.MaximumSize = new Size(262, 40);
      this.lblDescription.Name = "lblDescription";
      this.lblDescription.Size = new Size(262, 40);
      this.lblDescription.TabIndex = 2;
      this.lblDescription.Text = componentResourceManager.GetString("lblDescription.Text");
      this.lblDesc.AutoSize = true;
      this.lblDesc.ForeColor = SystemColors.ControlDarkDark;
      this.lblDesc.Location = new Point(25, 41);
      this.lblDesc.Name = "lblDesc";
      this.lblDesc.Size = new Size(63, 13);
      this.lblDesc.TabIndex = 1;
      this.lblDesc.Text = "Description:";
      this.lblFeeName.AutoSize = true;
      this.lblFeeName.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblFeeName.Location = new Point(25, 14);
      this.lblFeeName.Name = "lblFeeName";
      this.lblFeeName.Size = new Size(93, 16);
      this.lblFeeName.TabIndex = 0;
      this.lblFeeName.Text = "lblFeeName";
      this.btnDeleteSetting1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnDeleteSetting1.BackColor = Color.Transparent;
      this.btnDeleteSetting1.Location = new Point(971, 5);
      this.btnDeleteSetting1.Margin = new Padding(2, 3, 2, 3);
      this.btnDeleteSetting1.MouseDownImage = (Image) null;
      this.btnDeleteSetting1.Name = "btnDeleteSetting1";
      this.btnDeleteSetting1.Size = new Size(16, 17);
      this.btnDeleteSetting1.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnDeleteSetting1.TabIndex = 8;
      this.btnDeleteSetting1.TabStop = false;
      this.btnDeleteSetting1.Click += new EventHandler(this.btnDeleteSetting_Click);
      this.btnEditSetting1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnEditSetting1.BackColor = Color.Transparent;
      this.btnEditSetting1.Location = new Point(951, 4);
      this.btnEditSetting1.Margin = new Padding(2, 3, 2, 3);
      this.btnEditSetting1.MouseDownImage = (Image) null;
      this.btnEditSetting1.Name = "btnEditSetting1";
      this.btnEditSetting1.Size = new Size(16, 18);
      this.btnEditSetting1.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.btnEditSetting1.TabIndex = 7;
      this.btnEditSetting1.TabStop = false;
      this.btnEditSetting1.Click += new EventHandler(this.btnEditSetting_Click);
      this.panel1.Controls.Add((Control) this.label1);
      this.panel1.Dock = DockStyle.Top;
      this.panel1.Location = new Point(1, 26);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(993, 26);
      this.panel1.TabIndex = 4;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(6, 6);
      this.label1.Name = "label1";
      this.label1.Size = new Size(209, 13);
      this.label1.TabIndex = 35;
      this.label1.Text = "Select a single fee to view the details here.";
      this.btnDeleteSetting.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnDeleteSetting.BackColor = Color.Transparent;
      this.btnDeleteSetting.Location = new Point(971, 5);
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
      this.btnEditSetting.Location = new Point(951, 4);
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
      this.btnAddSetting.Location = new Point(931, 5);
      this.btnAddSetting.Margin = new Padding(2, 3, 2, 3);
      this.btnAddSetting.MouseDownImage = (Image) null;
      this.btnAddSetting.Name = "btnAddSetting";
      this.btnAddSetting.Size = new Size(16, 17);
      this.btnAddSetting.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnAddSetting.TabIndex = 4;
      this.btnAddSetting.TabStop = false;
      this.btnAddSetting.Click += new EventHandler(this.btnAddSetting_Click);
      this.gradientPanel1.Controls.Add((Control) this.picSearch);
      this.gradientPanel1.Controls.Add((Control) this.txtSearch);
      this.gradientPanel1.Controls.Add((Control) this.btnResetToDefault);
      this.gradientPanel1.Controls.Add((Control) this.btnLateFeeSettings);
      this.gradientPanel1.Controls.Add((Control) this.cmbStatus);
      this.gradientPanel1.Controls.Add((Control) this.lblStatus);
      this.gradientPanel1.Controls.Add((Control) this.cmbChannelType);
      this.gradientPanel1.Controls.Add((Control) this.lblChannelType);
      this.gradientPanel1.Dock = DockStyle.Top;
      this.gradientPanel1.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gradientPanel1.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gradientPanel1.Location = new Point(1, 52);
      this.gradientPanel1.Name = "gradientPanel1";
      this.gradientPanel1.Size = new Size(995, 38);
      this.gradientPanel1.TabIndex = 8;
      this.picSearch.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.picSearch.BackColor = Color.Transparent;
      this.picSearch.Location = new Point(735, 10);
      this.picSearch.Name = "picSearch";
      this.picSearch.Size = new Size(20, 20);
      this.picSearch.TabIndex = 39;
      this.picSearch.TabStop = false;
      this.picSearch.Click += new EventHandler(this.picSearch_Click);
      this.picSearch.MouseEnter += new EventHandler(this.picSearch_MouseEnter);
      this.picSearch.MouseLeave += new EventHandler(this.picSearch_MouseLeave);
      this.txtSearch.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.txtSearch.Location = new Point(556, 10);
      this.txtSearch.Name = "txtSearch";
      this.txtSearch.Size = new Size(176, 20);
      this.txtSearch.TabIndex = 38;
      this.btnResetToDefault.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnResetToDefault.Location = new Point(760, 8);
      this.btnResetToDefault.Name = "btnResetToDefault";
      this.btnResetToDefault.Size = new Size(118, 23);
      this.btnResetToDefault.TabIndex = 4;
      this.btnResetToDefault.Text = "Reset to Default Fees";
      this.btnResetToDefault.UseVisualStyleBackColor = true;
      this.btnResetToDefault.Click += new EventHandler(this.btnResetToDefault_Click);
      this.btnLateFeeSettings.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnLateFeeSettings.Location = new Point(884, 8);
      this.btnLateFeeSettings.Name = "btnLateFeeSettings";
      this.btnLateFeeSettings.Size = new Size(104, 23);
      this.btnLateFeeSettings.TabIndex = 5;
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
      this.cmbStatus.Location = new Point(53, 10);
      this.cmbStatus.Name = "cmbStatus";
      this.cmbStatus.Size = new Size(121, 21);
      this.cmbStatus.TabIndex = 3;
      this.cmbStatus.SelectedIndexChanged += new EventHandler(this.cmbStatus_SelectedIndexChanged);
      this.lblStatus.AutoSize = true;
      this.lblStatus.BackColor = Color.Transparent;
      this.lblStatus.Location = new Point(7, 13);
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
      this.cmbChannelType.Location = new Point(279, 10);
      this.cmbChannelType.Name = "cmbChannelType";
      this.cmbChannelType.Size = new Size(121, 21);
      this.cmbChannelType.TabIndex = 1;
      this.cmbChannelType.Visible = false;
      this.cmbChannelType.SelectedIndexChanged += new EventHandler(this.cmbChannelType_SelectedIndexChanged);
      this.lblChannelType.AutoSize = true;
      this.lblChannelType.BackColor = Color.Transparent;
      this.lblChannelType.Location = new Point(197, 13);
      this.lblChannelType.Name = "lblChannelType";
      this.lblChannelType.Size = new Size(76, 13);
      this.lblChannelType.TabIndex = 0;
      this.lblChannelType.Text = "Channel Type:";
      this.lblChannelType.Visible = false;
      this.panelHeader.Controls.Add((Control) this.label33);
      this.panelHeader.Dock = DockStyle.Top;
      this.panelHeader.Location = new Point(1, 26);
      this.panelHeader.Name = "panelHeader";
      this.panelHeader.Size = new Size(995, 26);
      this.panelHeader.TabIndex = 3;
      this.label33.AutoSize = true;
      this.label33.Location = new Point(6, 6);
      this.label33.Name = "label33";
      this.label33.Size = new Size(211, 13);
      this.label33.TabIndex = 35;
      this.label33.Text = "Maintain the fees associated with this TPO.";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.grpAll);
      this.Margin = new Padding(0);
      this.Name = nameof (EditCompanyFeesControl);
      this.Padding = new Padding(5);
      this.Size = new Size(1007, 686);
      this.grpAll.ResumeLayout(false);
      this.grpTPOFees.ResumeLayout(false);
      this.grpFeeDetails.ResumeLayout(false);
      this.pnlFeeDetails.ResumeLayout(false);
      this.pnlFeeDetails.PerformLayout();
      ((ISupportInitialize) this.btnDeleteSetting1).EndInit();
      ((ISupportInitialize) this.btnEditSetting1).EndInit();
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      ((ISupportInitialize) this.btnDeleteSetting).EndInit();
      ((ISupportInitialize) this.btnEditSetting).EndInit();
      ((ISupportInitialize) this.btnAddSetting).EndInit();
      this.gradientPanel1.ResumeLayout(false);
      this.gradientPanel1.PerformLayout();
      ((ISupportInitialize) this.picSearch).EndInit();
      this.panelHeader.ResumeLayout(false);
      this.panelHeader.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
