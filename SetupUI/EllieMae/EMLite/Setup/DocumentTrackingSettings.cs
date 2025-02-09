// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.DocumentTrackingSettings
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class DocumentTrackingSettings : SettingsUserControl
  {
    private bool changesSaved;
    private string dotTriggerDays;
    private string dotInitReqFuDays;
    private string dotRtnFuDays;
    private string ftpTriggerDays;
    private string ftpInitReqFuDays;
    private string ftpRtnFuDays;
    private IContainer components;
    private GroupContainer groupContainer1;
    private GroupContainer gcFTP;
    private ToolTip toolTip1;
    private Label label5;
    private Label label4;
    private Label label3;
    private Label label2;
    private Label label1;
    private CheckBox cboxFtpEnabled;
    private TextBox tbFtpRtnFuDays;
    private TextBox tbFtpInitReqFuDays;
    private TextBox tbFtpTriggerDays;
    private GroupContainer gcDOT;
    private CheckBox cboxDotEnabled;
    private TextBox tbDotRtnFuDays;
    private TextBox tbDotInitReqFuDays;
    private TextBox tbDotTriggerDays;
    private Label label6;
    private Label label7;
    private Label label8;
    private Label label9;
    private Label label10;
    private GroupContainer groupContainer2;
    private Label label12;
    private Label label11;
    private GridView listCarriers;
    private CheckBox cboxSchShipOpt;
    private Label label13;
    private StandardIconButton buttonEdit;
    private StandardIconButton buttonUp;
    private StandardIconButton buttonDown;
    private StandardIconButton buttonDelete;
    private StandardIconButton buttonNew;

    public DocumentTrackingSettings(SetUpContainer setupContainer)
      : base(setupContainer)
    {
      this.InitializeComponent();
      this.Reset();
    }

    private void buttonNew_Click(object sender, EventArgs e)
    {
      GridView listCarriers = this.listCarriers;
      using (SecondaryFieldDatilForm secondaryFieldDatilForm = new SecondaryFieldDatilForm(SecondaryFieldTypes.PreferredCarrier, "", this.collectListView(listCarriers, -1)))
      {
        if (secondaryFieldDatilForm.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        listCarriers.BeginUpdate();
        listCarriers.Items.Add(new GVItem(secondaryFieldDatilForm.NewOption)
        {
          Selected = true
        });
        listCarriers.EndUpdate();
        this.setDirtyFlag(true);
      }
    }

    private void buttonEdit_Click(object sender, EventArgs e) => this.editCarrier();

    private void listCarriers_DoubleClick(object sender, EventArgs e) => this.editCarrier();

    private void editCarrier()
    {
      GridView listCarriers = this.listCarriers;
      if (listCarriers.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select an option first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        using (SecondaryFieldDatilForm secondaryFieldDatilForm = new SecondaryFieldDatilForm(SecondaryFieldTypes.PreferredCarrier, listCarriers.SelectedItems[0].Text, this.collectListView(listCarriers, listCarriers.SelectedItems[0].Index)))
        {
          if (secondaryFieldDatilForm.ShowDialog((IWin32Window) this) != DialogResult.OK)
            return;
          listCarriers.BeginUpdate();
          listCarriers.SelectedItems[0].Text = secondaryFieldDatilForm.NewOption;
          listCarriers.EndUpdate();
          this.setDirtyFlag(true);
        }
      }
    }

    private void buttonUp_Click(object sender, EventArgs e)
    {
      GridView listCarriers = this.listCarriers;
      if (listCarriers.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select an option first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        if (listCarriers.SelectedItems[0].Index == 0)
          return;
        string text = listCarriers.Items[listCarriers.SelectedItems[0].Index - 1].Text;
        listCarriers.Items[listCarriers.SelectedItems[0].Index - 1].Text = listCarriers.SelectedItems[0].Text;
        listCarriers.SelectedItems[0].Text = text;
        listCarriers.Items[listCarriers.SelectedItems[0].Index - 1].Selected = true;
        this.setDirtyFlag(true);
      }
    }

    private void buttonDown_Click(object sender, EventArgs e)
    {
      GridView listCarriers = this.listCarriers;
      if (listCarriers.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select an option first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        if (listCarriers.SelectedItems[0].Index == listCarriers.Items.Count - 1)
          return;
        string text = listCarriers.Items[listCarriers.SelectedItems[0].Index + 1].Text;
        listCarriers.Items[listCarriers.SelectedItems[0].Index + 1].Text = listCarriers.SelectedItems[0].Text;
        listCarriers.SelectedItems[0].Text = text;
        listCarriers.Items[listCarriers.SelectedItems[0].Index + 1].Selected = true;
        this.setDirtyFlag(true);
      }
    }

    private void buttonDelete_Click(object sender, EventArgs e)
    {
      GridView listCarriers = this.listCarriers;
      if (listCarriers.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select an option first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        int index = listCarriers.SelectedItems[0].Index;
        listCarriers.Items.Remove(listCarriers.SelectedItems[0]);
        if (listCarriers.Items.Count == 0)
          return;
        if (index + 1 > listCarriers.Items.Count)
          listCarriers.Items[listCarriers.Items.Count - 1].Selected = true;
        else
          listCarriers.Items[index].Selected = true;
        this.setDirtyFlag(true);
      }
    }

    private void listCarriers_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.buttonUp.Enabled = this.buttonDown.Enabled = this.buttonEdit.Enabled = this.listCarriers.SelectedItems.Count == 1;
      this.buttonDelete.Enabled = this.listCarriers.SelectedItems.Count > 0;
    }

    public bool ChangesSaved => this.changesSaved;

    public override void Save()
    {
      this.changesSaved = false;
      if (!this.validateData())
        return;
      Session.ConfigurationManager.SetSecondaryFields(this.collectListView(this.listCarriers, -1), SecondaryFieldTypes.PreferredCarrier);
      IConfigurationManager configurationManager1 = Session.ConfigurationManager;
      bool flag = this.cboxSchShipOpt.Checked;
      string str1 = flag.ToString();
      configurationManager1.SetCompanySetting("DOCTRACKING", "EnableScheduledShipment", str1);
      IConfigurationManager configurationManager2 = Session.ConfigurationManager;
      flag = this.cboxDotEnabled.Checked;
      string str2 = flag.ToString();
      configurationManager2.SetCompanySetting("DOCTRACKING", "EnableDOT", str2);
      if (this.cboxDotEnabled.Checked)
      {
        Session.ConfigurationManager.SetCompanySetting("DOCTRACKING", "DOTInitReqTriggerDays", this.tbDotTriggerDays.Text);
        Session.ConfigurationManager.SetCompanySetting("DOCTRACKING", "DOTInitReqDaysBtwnFollowUp", this.tbDotInitReqFuDays.Text);
        Session.ConfigurationManager.SetCompanySetting("DOCTRACKING", "DOTRtrnDaysBtwnFollowUp", this.tbDotRtnFuDays.Text);
      }
      IConfigurationManager configurationManager3 = Session.ConfigurationManager;
      flag = this.cboxFtpEnabled.Checked;
      string str3 = flag.ToString();
      configurationManager3.SetCompanySetting("DOCTRACKING", "EnableFTP", str3);
      if (this.cboxFtpEnabled.Checked)
      {
        Session.ConfigurationManager.SetCompanySetting("DOCTRACKING", "FTPInitReqTriggerDays", this.tbFtpTriggerDays.Text);
        Session.ConfigurationManager.SetCompanySetting("DOCTRACKING", "FTPInitReqDaysBtwnFollowUp", this.tbFtpInitReqFuDays.Text);
        Session.ConfigurationManager.SetCompanySetting("DOCTRACKING", "FTPRtrnDaysBtwnFollowUp", this.tbFtpRtnFuDays.Text);
      }
      this.setDirtyFlag(false);
      this.changesSaved = true;
    }

    private bool validateData()
    {
      bool flag = true;
      if (this.cboxDotEnabled.Checked && (this.tbDotTriggerDays.Text.Trim() == string.Empty || Utils.ParseInt((object) this.tbDotTriggerDays.Text.Trim()) <= 0))
      {
        this.tbDotTriggerDays.BackColor = ColorTranslator.FromHtml("#FCDCDF");
        flag = false;
      }
      else
        this.tbDotTriggerDays.BackColor = Color.White;
      if (this.cboxDotEnabled.Checked && (this.tbDotInitReqFuDays.Text.Trim() == string.Empty || Utils.ParseInt((object) this.tbDotInitReqFuDays.Text.Trim()) <= 0))
      {
        this.tbDotInitReqFuDays.BackColor = ColorTranslator.FromHtml("#FCDCDF");
        flag = false;
      }
      else
        this.tbDotInitReqFuDays.BackColor = Color.White;
      if (this.cboxDotEnabled.Checked && (this.tbDotRtnFuDays.Text.Trim() == string.Empty || Utils.ParseInt((object) this.tbDotRtnFuDays.Text.Trim()) <= 0))
      {
        this.tbDotRtnFuDays.BackColor = ColorTranslator.FromHtml("#FCDCDF");
        flag = false;
      }
      else
        this.tbDotRtnFuDays.BackColor = Color.White;
      if (this.cboxFtpEnabled.Checked && (this.tbFtpTriggerDays.Text.Trim() == string.Empty || Utils.ParseInt((object) this.tbFtpTriggerDays.Text.Trim()) <= 0))
      {
        this.tbFtpTriggerDays.BackColor = ColorTranslator.FromHtml("#FCDCDF");
        flag = false;
      }
      else
        this.tbFtpTriggerDays.BackColor = Color.White;
      if (this.cboxFtpEnabled.Checked && (this.tbFtpInitReqFuDays.Text.Trim() == string.Empty || Utils.ParseInt((object) this.tbFtpInitReqFuDays.Text.Trim()) <= 0))
      {
        this.tbFtpInitReqFuDays.BackColor = ColorTranslator.FromHtml("#FCDCDF");
        flag = false;
      }
      else
        this.tbFtpInitReqFuDays.BackColor = Color.White;
      if (this.cboxFtpEnabled.Checked && (this.tbFtpRtnFuDays.Text.Trim() == string.Empty || Utils.ParseInt((object) this.tbFtpRtnFuDays.Text.Trim()) <= 0))
      {
        this.tbFtpRtnFuDays.BackColor = ColorTranslator.FromHtml("#FCDCDF");
        flag = false;
      }
      else
        this.tbFtpRtnFuDays.BackColor = Color.White;
      if (!flag)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Required fields must be entered and greater than zero before these settings can be saved.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      return flag;
    }

    private ArrayList collectListView(GridView listview, int excludeMe)
    {
      ArrayList arrayList = new ArrayList();
      for (int nItemIndex = 0; nItemIndex < listview.Items.Count; ++nItemIndex)
      {
        if (nItemIndex != excludeMe)
          arrayList.Add((object) listview.Items[nItemIndex].Text);
      }
      return arrayList;
    }

    public override void Reset()
    {
      this.buttonNew.Enabled = true;
      this.buttonEdit.Enabled = false;
      this.buttonUp.Enabled = false;
      this.buttonDown.Enabled = false;
      this.buttonDelete.Enabled = false;
      this.tbDotTriggerDays.BackColor = Color.White;
      this.tbDotInitReqFuDays.BackColor = Color.White;
      this.tbDotRtnFuDays.BackColor = Color.White;
      this.tbFtpTriggerDays.BackColor = Color.White;
      this.tbFtpInitReqFuDays.BackColor = Color.White;
      this.tbFtpRtnFuDays.BackColor = Color.White;
      this.initCarrierList();
      this.cboxSchShipOpt.Checked = string.Equals(Session.ConfigurationManager.GetCompanySetting("DOCTRACKING", "EnableScheduledShipment"), "true", StringComparison.CurrentCultureIgnoreCase);
      this.cboxDotEnabled.Checked = string.Equals(Session.ConfigurationManager.GetCompanySetting("DOCTRACKING", "EnableDOT"), "true", StringComparison.CurrentCultureIgnoreCase);
      if (this.cboxDotEnabled.Checked)
      {
        this.tbDotTriggerDays.Text = Session.ConfigurationManager.GetCompanySetting("DOCTRACKING", "DOTInitReqTriggerDays");
        this.tbDotInitReqFuDays.Text = Session.ConfigurationManager.GetCompanySetting("DOCTRACKING", "DOTInitReqDaysBtwnFollowUp");
        this.tbDotRtnFuDays.Text = Session.ConfigurationManager.GetCompanySetting("DOCTRACKING", "DOTRtrnDaysBtwnFollowUp");
        this.dotTriggerDays = this.tbDotTriggerDays.Text.Trim();
        this.dotInitReqFuDays = this.tbDotInitReqFuDays.Text.Trim();
        this.dotRtnFuDays = this.tbDotRtnFuDays.Text.Trim();
      }
      this.cboxFtpEnabled.Checked = string.Equals(Session.ConfigurationManager.GetCompanySetting("DOCTRACKING", "EnableFTP"), "true", StringComparison.CurrentCultureIgnoreCase);
      if (this.cboxFtpEnabled.Checked)
      {
        this.tbFtpTriggerDays.Text = Session.ConfigurationManager.GetCompanySetting("DOCTRACKING", "FTPInitReqTriggerDays");
        this.tbFtpInitReqFuDays.Text = Session.ConfigurationManager.GetCompanySetting("DOCTRACKING", "FTPInitReqDaysBtwnFollowUp");
        this.tbFtpRtnFuDays.Text = Session.ConfigurationManager.GetCompanySetting("DOCTRACKING", "FTPRtrnDaysBtwnFollowUp");
        this.ftpTriggerDays = this.tbFtpTriggerDays.Text.Trim();
        this.ftpInitReqFuDays = this.tbFtpInitReqFuDays.Text.Trim();
        this.ftpRtnFuDays = this.tbFtpRtnFuDays.Text.Trim();
      }
      this.setDotTriggerFollowDays();
      this.setFtpTriggerFollowDays();
      this.setDirtyFlag(false);
    }

    private void initCarrierList()
    {
      GridView listCarriers = this.listCarriers;
      ArrayList secondaryFields = Session.ConfigurationManager.GetSecondaryFields(SecondaryFieldTypes.PreferredCarrier);
      listCarriers.Items.Clear();
      if (secondaryFields == null || secondaryFields.Count == 0)
        return;
      listCarriers.BeginUpdate();
      for (int index = 0; index < secondaryFields.Count; ++index)
      {
        GVItem gvItem = new GVItem(secondaryFields[index].ToString());
        listCarriers.Items.Add(gvItem);
      }
      listCarriers.EndUpdate();
    }

    private void cboxDotEnabled_CheckedChanged(object sender, EventArgs e)
    {
      this.setDotTriggerFollowDays();
      this.setDirtyFlag(true);
    }

    private void cboxFtpEnabled_CheckedChanged(object sender, EventArgs e)
    {
      this.setFtpTriggerFollowDays();
      this.setDirtyFlag(true);
    }

    private void setFtpTriggerFollowDays()
    {
      if (this.cboxFtpEnabled.Checked)
      {
        this.tbFtpTriggerDays.Enabled = true;
        this.tbFtpInitReqFuDays.Enabled = true;
        this.tbFtpRtnFuDays.Enabled = true;
        this.tbFtpTriggerDays.Text = this.ftpTriggerDays;
        this.tbFtpInitReqFuDays.Text = this.ftpInitReqFuDays;
        this.tbFtpRtnFuDays.Text = this.ftpRtnFuDays;
      }
      else
      {
        this.ftpTriggerDays = this.tbFtpTriggerDays.Text.Trim();
        this.ftpInitReqFuDays = this.tbFtpInitReqFuDays.Text.Trim();
        this.ftpRtnFuDays = this.tbFtpRtnFuDays.Text.Trim();
        this.tbFtpTriggerDays.Enabled = false;
        this.tbFtpInitReqFuDays.Enabled = false;
        this.tbFtpRtnFuDays.Enabled = false;
        this.tbFtpTriggerDays.Text = "";
        this.tbFtpInitReqFuDays.Text = "";
        this.tbFtpRtnFuDays.Text = "";
        this.tbFtpTriggerDays.BackColor = Color.White;
        this.tbFtpInitReqFuDays.BackColor = Color.White;
        this.tbFtpRtnFuDays.BackColor = Color.White;
      }
    }

    private void setDotTriggerFollowDays()
    {
      if (this.cboxDotEnabled.Checked)
      {
        this.tbDotTriggerDays.Enabled = true;
        this.tbDotInitReqFuDays.Enabled = true;
        this.tbDotRtnFuDays.Enabled = true;
        this.tbDotTriggerDays.Text = this.dotTriggerDays;
        this.tbDotInitReqFuDays.Text = this.dotInitReqFuDays;
        this.tbDotRtnFuDays.Text = this.dotRtnFuDays;
      }
      else
      {
        this.dotTriggerDays = this.tbDotTriggerDays.Text.Trim();
        this.dotInitReqFuDays = this.tbDotInitReqFuDays.Text.Trim();
        this.dotRtnFuDays = this.tbDotRtnFuDays.Text.Trim();
        this.tbDotTriggerDays.Enabled = false;
        this.tbDotInitReqFuDays.Enabled = false;
        this.tbDotRtnFuDays.Enabled = false;
        this.tbDotTriggerDays.Text = "";
        this.tbDotInitReqFuDays.Text = "";
        this.tbDotRtnFuDays.Text = "";
        this.tbDotTriggerDays.BackColor = Color.White;
        this.tbDotInitReqFuDays.BackColor = Color.White;
        this.tbDotRtnFuDays.BackColor = Color.White;
      }
    }

    private void inputChanged(object sender, EventArgs e) => this.setDirtyFlag(true);

    private void numKeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar.Equals((object) Keys.Delete) || char.IsControl(e.KeyChar))
        return;
      if (char.IsDigit(e.KeyChar))
        e.Handled = false;
      else
        e.Handled = true;
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
      this.toolTip1 = new ToolTip(this.components);
      this.buttonNew = new StandardIconButton();
      this.buttonDelete = new StandardIconButton();
      this.buttonDown = new StandardIconButton();
      this.buttonUp = new StandardIconButton();
      this.buttonEdit = new StandardIconButton();
      this.gcDOT = new GroupContainer();
      this.cboxDotEnabled = new CheckBox();
      this.tbDotRtnFuDays = new TextBox();
      this.tbDotInitReqFuDays = new TextBox();
      this.tbDotTriggerDays = new TextBox();
      this.label6 = new Label();
      this.label7 = new Label();
      this.label8 = new Label();
      this.label9 = new Label();
      this.label10 = new Label();
      this.gcFTP = new GroupContainer();
      this.cboxFtpEnabled = new CheckBox();
      this.tbFtpRtnFuDays = new TextBox();
      this.tbFtpInitReqFuDays = new TextBox();
      this.tbFtpTriggerDays = new TextBox();
      this.label5 = new Label();
      this.label4 = new Label();
      this.label3 = new Label();
      this.label2 = new Label();
      this.label1 = new Label();
      this.groupContainer1 = new GroupContainer();
      this.cboxSchShipOpt = new CheckBox();
      this.label13 = new Label();
      this.groupContainer2 = new GroupContainer();
      this.label12 = new Label();
      this.label11 = new Label();
      this.listCarriers = new GridView();
      ((ISupportInitialize) this.buttonNew).BeginInit();
      ((ISupportInitialize) this.buttonDelete).BeginInit();
      ((ISupportInitialize) this.buttonDown).BeginInit();
      ((ISupportInitialize) this.buttonUp).BeginInit();
      ((ISupportInitialize) this.buttonEdit).BeginInit();
      this.gcDOT.SuspendLayout();
      this.gcFTP.SuspendLayout();
      this.groupContainer1.SuspendLayout();
      this.groupContainer2.SuspendLayout();
      this.SuspendLayout();
      this.buttonNew.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.buttonNew.BackColor = Color.Transparent;
      this.buttonNew.Location = new Point(325, 4);
      this.buttonNew.MouseDownImage = (Image) null;
      this.buttonNew.Name = "buttonNew";
      this.buttonNew.Size = new Size(16, 16);
      this.buttonNew.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.buttonNew.TabIndex = 85;
      this.buttonNew.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.buttonNew, "New Carrier");
      this.buttonNew.Click += new EventHandler(this.buttonNew_Click);
      this.buttonDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.buttonDelete.BackColor = Color.Transparent;
      this.buttonDelete.Location = new Point(413, 4);
      this.buttonDelete.MouseDownImage = (Image) null;
      this.buttonDelete.Name = "buttonDelete";
      this.buttonDelete.Size = new Size(16, 16);
      this.buttonDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.buttonDelete.TabIndex = 84;
      this.buttonDelete.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.buttonDelete, "Delete Carrier");
      this.buttonDelete.Click += new EventHandler(this.buttonDelete_Click);
      this.buttonDown.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.buttonDown.BackColor = Color.Transparent;
      this.buttonDown.Location = new Point(391, 4);
      this.buttonDown.MouseDownImage = (Image) null;
      this.buttonDown.Name = "buttonDown";
      this.buttonDown.Size = new Size(16, 16);
      this.buttonDown.StandardButtonType = StandardIconButton.ButtonType.DownArrowButton;
      this.buttonDown.TabIndex = 83;
      this.buttonDown.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.buttonDown, "Move Carrier Down");
      this.buttonDown.Click += new EventHandler(this.buttonDown_Click);
      this.buttonUp.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.buttonUp.BackColor = Color.Transparent;
      this.buttonUp.Location = new Point(369, 4);
      this.buttonUp.MouseDownImage = (Image) null;
      this.buttonUp.Name = "buttonUp";
      this.buttonUp.Size = new Size(16, 16);
      this.buttonUp.StandardButtonType = StandardIconButton.ButtonType.UpArrowButton;
      this.buttonUp.TabIndex = 82;
      this.buttonUp.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.buttonUp, "Move Carrier Up");
      this.buttonUp.Click += new EventHandler(this.buttonUp_Click);
      this.buttonEdit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.buttonEdit.BackColor = Color.Transparent;
      this.buttonEdit.Location = new Point(347, 4);
      this.buttonEdit.MouseDownImage = (Image) null;
      this.buttonEdit.Name = "buttonEdit";
      this.buttonEdit.Size = new Size(16, 16);
      this.buttonEdit.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.buttonEdit.TabIndex = 81;
      this.buttonEdit.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.buttonEdit, "Edit Carrier");
      this.buttonEdit.Click += new EventHandler(this.buttonEdit_Click);
      this.gcDOT.BackColor = Color.White;
      this.gcDOT.Controls.Add((Control) this.cboxDotEnabled);
      this.gcDOT.Controls.Add((Control) this.tbDotRtnFuDays);
      this.gcDOT.Controls.Add((Control) this.tbDotInitReqFuDays);
      this.gcDOT.Controls.Add((Control) this.tbDotTriggerDays);
      this.gcDOT.Controls.Add((Control) this.label6);
      this.gcDOT.Controls.Add((Control) this.label7);
      this.gcDOT.Controls.Add((Control) this.label8);
      this.gcDOT.Controls.Add((Control) this.label9);
      this.gcDOT.Controls.Add((Control) this.label10);
      this.gcDOT.HeaderForeColor = SystemColors.ControlText;
      this.gcDOT.Location = new Point(0, 269);
      this.gcDOT.Name = "gcDOT";
      this.gcDOT.Size = new Size(837, 137);
      this.gcDOT.TabIndex = 3;
      this.gcDOT.Text = "DOT/Mortgage";
      this.cboxDotEnabled.AutoSize = true;
      this.cboxDotEnabled.BackColor = Color.Transparent;
      this.cboxDotEnabled.Location = new Point(154, 6);
      this.cboxDotEnabled.Name = "cboxDotEnabled";
      this.cboxDotEnabled.Size = new Size(65, 17);
      this.cboxDotEnabled.TabIndex = 8;
      this.cboxDotEnabled.Text = "Enabled";
      this.cboxDotEnabled.UseVisualStyleBackColor = false;
      this.cboxDotEnabled.CheckedChanged += new EventHandler(this.cboxDotEnabled_CheckedChanged);
      this.tbDotRtnFuDays.BackColor = Color.White;
      this.tbDotRtnFuDays.Location = new Point(435, 69);
      this.tbDotRtnFuDays.MaxLength = 2;
      this.tbDotRtnFuDays.Name = "tbDotRtnFuDays";
      this.tbDotRtnFuDays.Size = new Size(55, 20);
      this.tbDotRtnFuDays.TabIndex = 7;
      this.tbDotRtnFuDays.TextChanged += new EventHandler(this.inputChanged);
      this.tbDotRtnFuDays.KeyPress += new KeyPressEventHandler(this.numKeyPress);
      this.tbDotInitReqFuDays.Location = new Point(153, 92);
      this.tbDotInitReqFuDays.MaxLength = 2;
      this.tbDotInitReqFuDays.Name = "tbDotInitReqFuDays";
      this.tbDotInitReqFuDays.Size = new Size(55, 20);
      this.tbDotInitReqFuDays.TabIndex = 6;
      this.tbDotInitReqFuDays.TextChanged += new EventHandler(this.inputChanged);
      this.tbDotInitReqFuDays.KeyPress += new KeyPressEventHandler(this.numKeyPress);
      this.tbDotTriggerDays.BackColor = Color.White;
      this.tbDotTriggerDays.Location = new Point(153, 67);
      this.tbDotTriggerDays.MaxLength = 3;
      this.tbDotTriggerDays.Name = "tbDotTriggerDays";
      this.tbDotTriggerDays.Size = new Size(55, 20);
      this.tbDotTriggerDays.TabIndex = 5;
      this.tbDotTriggerDays.TextChanged += new EventHandler(this.inputChanged);
      this.tbDotTriggerDays.KeyPress += new KeyPressEventHandler(this.numKeyPress);
      this.label6.AutoSize = true;
      this.label6.Location = new Point(18, 95);
      this.label6.Name = "label6";
      this.label6.Size = new Size(129, 13);
      this.label6.TabIndex = 4;
      this.label6.Text = "Days Between Follow-ups";
      this.label7.AutoSize = true;
      this.label7.Location = new Point(300, 72);
      this.label7.Name = "label7";
      this.label7.Size = new Size(129, 13);
      this.label7.TabIndex = 3;
      this.label7.Text = "Days Between Follow-ups";
      this.label8.AutoSize = true;
      this.label8.Location = new Point(18, 72);
      this.label8.Name = "label8";
      this.label8.Size = new Size(67, 13);
      this.label8.TabIndex = 2;
      this.label8.Text = "Trigger Days";
      this.label9.AutoSize = true;
      this.label9.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label9.Location = new Point(300, 40);
      this.label9.Name = "label9";
      this.label9.Size = new Size(96, 13);
      this.label9.TabIndex = 1;
      this.label9.Text = "Return Request";
      this.label10.AutoSize = true;
      this.label10.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label10.Location = new Point(18, 40);
      this.label10.Name = "label10";
      this.label10.Size = new Size(89, 13);
      this.label10.TabIndex = 0;
      this.label10.Text = "Initial Request";
      this.gcFTP.BackColor = Color.White;
      this.gcFTP.Controls.Add((Control) this.cboxFtpEnabled);
      this.gcFTP.Controls.Add((Control) this.tbFtpRtnFuDays);
      this.gcFTP.Controls.Add((Control) this.tbFtpInitReqFuDays);
      this.gcFTP.Controls.Add((Control) this.tbFtpTriggerDays);
      this.gcFTP.Controls.Add((Control) this.label5);
      this.gcFTP.Controls.Add((Control) this.label4);
      this.gcFTP.Controls.Add((Control) this.label3);
      this.gcFTP.Controls.Add((Control) this.label2);
      this.gcFTP.Controls.Add((Control) this.label1);
      this.gcFTP.HeaderForeColor = SystemColors.ControlText;
      this.gcFTP.Location = new Point(0, 406);
      this.gcFTP.Name = "gcFTP";
      this.gcFTP.Size = new Size(837, 135);
      this.gcFTP.TabIndex = 2;
      this.gcFTP.Text = "Final Title Policy";
      this.cboxFtpEnabled.AutoSize = true;
      this.cboxFtpEnabled.BackColor = Color.Transparent;
      this.cboxFtpEnabled.Location = new Point(152, 6);
      this.cboxFtpEnabled.Name = "cboxFtpEnabled";
      this.cboxFtpEnabled.Size = new Size(65, 17);
      this.cboxFtpEnabled.TabIndex = 8;
      this.cboxFtpEnabled.Text = "Enabled";
      this.cboxFtpEnabled.UseVisualStyleBackColor = false;
      this.cboxFtpEnabled.CheckedChanged += new EventHandler(this.cboxFtpEnabled_CheckedChanged);
      this.tbFtpRtnFuDays.Location = new Point(435, 69);
      this.tbFtpRtnFuDays.MaxLength = 2;
      this.tbFtpRtnFuDays.Name = "tbFtpRtnFuDays";
      this.tbFtpRtnFuDays.Size = new Size(55, 20);
      this.tbFtpRtnFuDays.TabIndex = 7;
      this.tbFtpRtnFuDays.TextChanged += new EventHandler(this.inputChanged);
      this.tbFtpRtnFuDays.KeyPress += new KeyPressEventHandler(this.numKeyPress);
      this.tbFtpInitReqFuDays.Location = new Point(153, 92);
      this.tbFtpInitReqFuDays.MaxLength = 2;
      this.tbFtpInitReqFuDays.Name = "tbFtpInitReqFuDays";
      this.tbFtpInitReqFuDays.Size = new Size(55, 20);
      this.tbFtpInitReqFuDays.TabIndex = 6;
      this.tbFtpInitReqFuDays.TextChanged += new EventHandler(this.inputChanged);
      this.tbFtpInitReqFuDays.KeyPress += new KeyPressEventHandler(this.numKeyPress);
      this.tbFtpTriggerDays.Location = new Point(153, 67);
      this.tbFtpTriggerDays.MaxLength = 3;
      this.tbFtpTriggerDays.Name = "tbFtpTriggerDays";
      this.tbFtpTriggerDays.Size = new Size(55, 20);
      this.tbFtpTriggerDays.TabIndex = 5;
      this.tbFtpTriggerDays.TextChanged += new EventHandler(this.inputChanged);
      this.tbFtpTriggerDays.KeyPress += new KeyPressEventHandler(this.numKeyPress);
      this.label5.AutoSize = true;
      this.label5.Location = new Point(18, 95);
      this.label5.Name = "label5";
      this.label5.Size = new Size(129, 13);
      this.label5.TabIndex = 4;
      this.label5.Text = "Days Between Follow-ups";
      this.label4.AutoSize = true;
      this.label4.Location = new Point(300, 72);
      this.label4.Name = "label4";
      this.label4.Size = new Size(129, 13);
      this.label4.TabIndex = 3;
      this.label4.Text = "Days Between Follow-ups";
      this.label3.AutoSize = true;
      this.label3.Location = new Point(18, 72);
      this.label3.Name = "label3";
      this.label3.Size = new Size(67, 13);
      this.label3.TabIndex = 2;
      this.label3.Text = "Trigger Days";
      this.label2.AutoSize = true;
      this.label2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label2.Location = new Point(300, 40);
      this.label2.Name = "label2";
      this.label2.Size = new Size(96, 13);
      this.label2.TabIndex = 1;
      this.label2.Text = "Return Request";
      this.label1.AutoSize = true;
      this.label1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(18, 40);
      this.label1.Name = "label1";
      this.label1.Size = new Size(89, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Initial Request";
      this.groupContainer1.BackColor = Color.White;
      this.groupContainer1.Controls.Add((Control) this.cboxSchShipOpt);
      this.groupContainer1.Controls.Add((Control) this.label13);
      this.groupContainer1.Controls.Add((Control) this.groupContainer2);
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(0, 0);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(837, 272);
      this.groupContainer1.TabIndex = 0;
      this.groupContainer1.Text = "Collateral Tracking Options";
      this.cboxSchShipOpt.AutoSize = true;
      this.cboxSchShipOpt.BackColor = Color.Transparent;
      this.cboxSchShipOpt.Location = new Point(486, 122);
      this.cboxSchShipOpt.Name = "cboxSchShipOpt";
      this.cboxSchShipOpt.Size = new Size(65, 17);
      this.cboxSchShipOpt.TabIndex = 9;
      this.cboxSchShipOpt.Text = "Enabled";
      this.cboxSchShipOpt.UseVisualStyleBackColor = false;
      this.cboxSchShipOpt.CheckedChanged += new EventHandler(this.inputChanged);
      this.label13.AutoSize = true;
      this.label13.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label13.Location = new Point(483, 92);
      this.label13.Name = "label13";
      this.label13.Size = new Size(164, 13);
      this.label13.TabIndex = 1;
      this.label13.Text = "Scheduled Shipment Option";
      this.groupContainer2.Controls.Add((Control) this.buttonNew);
      this.groupContainer2.Controls.Add((Control) this.buttonDelete);
      this.groupContainer2.Controls.Add((Control) this.buttonDown);
      this.groupContainer2.Controls.Add((Control) this.buttonUp);
      this.groupContainer2.Controls.Add((Control) this.buttonEdit);
      this.groupContainer2.Controls.Add((Control) this.label12);
      this.groupContainer2.Controls.Add((Control) this.label11);
      this.groupContainer2.Controls.Add((Control) this.listCarriers);
      this.groupContainer2.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer2.Location = new Point(0, 25);
      this.groupContainer2.Name = "groupContainer2";
      this.groupContainer2.Size = new Size(446, 244);
      this.groupContainer2.TabIndex = 0;
      this.groupContainer2.Text = "Preferred Carrier Dropdown List";
      this.label12.AutoSize = true;
      this.label12.BackColor = Color.Transparent;
      this.label12.Location = new Point(7, 52);
      this.label12.Name = "label12";
      this.label12.Size = new Size(262, 13);
      this.label12.TabIndex = 1;
      this.label12.Text = "First entry in Dropdown list will be the Preferred Carrier.";
      this.label11.AutoSize = true;
      this.label11.BackColor = Color.Transparent;
      this.label11.Location = new Point(7, 32);
      this.label11.Name = "label11";
      this.label11.Size = new Size(248, 13);
      this.label11.TabIndex = 0;
      this.label11.Text = "Create and edit description for the Preferred Carrier.";
      this.listCarriers.AllowMultiselect = false;
      this.listCarriers.BorderStyle = BorderStyle.None;
      this.listCarriers.CellPadding = new Padding(10, 1, 5, 1);
      gvColumn.ImageIndex = -1;
      gvColumn.Name = "colCarrier";
      gvColumn.Text = "Carrier";
      gvColumn.Width = 400;
      this.listCarriers.Columns.AddRange(new GVColumn[1]
      {
        gvColumn
      });
      this.listCarriers.ForeColor = SystemColors.ControlText;
      this.listCarriers.HeaderHeight = 0;
      this.listCarriers.HeaderVisible = false;
      this.listCarriers.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.listCarriers.Location = new Point(1, 68);
      this.listCarriers.Name = "listCarriers";
      this.listCarriers.Size = new Size(441, 179);
      this.listCarriers.TabIndex = 2;
      this.listCarriers.SelectedIndexChanged += new EventHandler(this.listCarriers_SelectedIndexChanged);
      this.listCarriers.DoubleClick += new EventHandler(this.buttonEdit_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.AutoScroll = true;
      this.AutoSize = true;
      this.Controls.Add((Control) this.gcDOT);
      this.Controls.Add((Control) this.gcFTP);
      this.Controls.Add((Control) this.groupContainer1);
      this.Name = nameof (DocumentTrackingSettings);
      this.Size = new Size(840, 653);
      ((ISupportInitialize) this.buttonNew).EndInit();
      ((ISupportInitialize) this.buttonDelete).EndInit();
      ((ISupportInitialize) this.buttonDown).EndInit();
      ((ISupportInitialize) this.buttonUp).EndInit();
      ((ISupportInitialize) this.buttonEdit).EndInit();
      this.gcDOT.ResumeLayout(false);
      this.gcDOT.PerformLayout();
      this.gcFTP.ResumeLayout(false);
      this.gcFTP.PerformLayout();
      this.groupContainer1.ResumeLayout(false);
      this.groupContainer1.PerformLayout();
      this.groupContainer2.ResumeLayout(false);
      this.groupContainer2.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
