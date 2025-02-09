// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.EPPSProgramTableExplorer
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.EPC2;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.ePass;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class EPPSProgramTableExplorer : SettingsUserControl
  {
    protected new SetUpContainer setupContainer;
    private Sessions.Session session;
    private bool isDirty;
    private IContainer components;
    private GridView gridViewEPPSPrograms;
    private GroupContainer grpContainerLock;
    private StandardIconButton iconBtnLockDelete;
    private VerticalSeparator verticalSeparator1;
    private ToolTip toolTip1;
    private Button btnPopulateEpps;

    public EPPSProgramTableExplorer(SetUpContainer setupContainer, Sessions.Session session)
      : base(setupContainer)
    {
      this.setupContainer = setupContainer;
      this.session = session;
      this.InitializeComponent();
      this.refresh();
      if (this.session.StartupInfo.ProductPricingPartner != null && this.session.StartupInfo.ProductPricingPartner.IsEPPS)
        this.btnPopulateEpps.Enabled = true;
      else
        this.btnPopulateEpps.Enabled = false;
    }

    public new virtual bool IsDirty => this.isDirty;

    private void buildGridViewItem(EPPSLoanProgram[] programs, GridView targetGridView)
    {
      int count = targetGridView.Items.Count;
      this.gridViewEPPSPrograms.Items.Clear();
      targetGridView.BeginUpdate();
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      for (int index = 0; index < programs.Length; ++index)
      {
        GVItem gvItem = new GVItem(programs[index].ProgramID);
        gvItem.SubItems.Add((object) programs[index].ProgramName);
        if (targetGridView.Items.ContainsTag((object) programs[index]))
        {
          GVItem itemByTag = targetGridView.Items.GetItemByTag((object) programs[index]);
          targetGridView.Items.Remove(itemByTag);
        }
        targetGridView.Items.Add(gvItem);
        gvItem.Tag = (object) programs[index];
      }
      targetGridView.EndUpdate();
      targetGridView.Sort(1, SortOrder.Ascending);
    }

    public override void Reset() => this.refresh();

    public void refresh()
    {
      List<EPPSLoanProgram> eppsLoanProgramList = new List<EPPSLoanProgram>();
      this.buildGridViewItem(Session.ConfigurationManager.GetEPPSLoanProgramsSettings().ToArray(), this.gridViewEPPSPrograms);
    }

    private void iconBtnLockDelete_Click(object sender, EventArgs e)
    {
      GVSelectedItemCollection selectedItems = this.gridViewEPPSPrograms.SelectedItems;
      if (Utils.Dialog((IWin32Window) this, "Are you sure you want to delete these entries?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
        return;
      foreach (GVItem gvItem in selectedItems)
        this.gridViewEPPSPrograms.Items.Remove(gvItem);
      this.setDirtyFlag(true);
    }

    public override void Save()
    {
      List<EPPSLoanProgram> programs = new List<EPPSLoanProgram>();
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gridViewEPPSPrograms.Items)
        programs.Add((EPPSLoanProgram) gvItem.Tag);
      if (programs != null)
        Session.ConfigurationManager.SaveEPPSLoanProgramsSettings(programs);
      this.setDirtyFlag(false);
    }

    private void btnPopulateEpps_Click(object sender, EventArgs e)
    {
      if (this.gridViewEPPSPrograms.Items.Count > 0)
      {
        if (Utils.Dialog((IWin32Window) this, "This action will clear all current contents of the ICE PPE Loan Program table and replace it with new entries from ICE PPE. Do you wish to continue?", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.OK)
          return;
        this.LoadEppsLoanProgram();
      }
      else
        this.LoadEppsLoanProgram();
    }

    private void LoadEppsLoanProgram()
    {
      Cursor.Current = Cursors.WaitCursor;
      IEPass service = this.session.Application.GetService<IEPass>();
      Bam.EppsLoanPrograms = (List<EPPSLoanProgram>) null;
      string str = this.session.StartupInfo.ProductPricingPartner.PartnerID;
      if (this.session.StartupInfo.ProductPricingPartner.VendorPlatform == VendorPlatform.EPC2)
        str = "MPS";
      service.ProcessURL("_EPASS_SIGNATURE;" + str + ";;GetClientPrograms;;SOURCE_FORM=GetPricing_LO_LOCK");
      if (Bam.EppsLoanPrograms != null && Bam.EppsLoanPrograms.Count > 0)
      {
        this.buildGridViewItem(Bam.EppsLoanPrograms.ToArray(), this.gridViewEPPSPrograms);
        int num = (int) Utils.Dialog((IWin32Window) this, Bam.EppsLoanPrograms.Count.ToString() + " Loan Programs have been added.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        this.setDirtyFlag(true);
      }
      Cursor.Current = Cursors.Default;
    }

    private void gridViewEPPSPrograms_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.iconBtnLockDelete.Enabled = this.gridViewEPPSPrograms.SelectedItems.Count > 0 && this.session.StartupInfo.ProductPricingPartner != null && this.session.StartupInfo.ProductPricingPartner.IsEPPS;
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
      this.gridViewEPPSPrograms = new GridView();
      this.grpContainerLock = new GroupContainer();
      this.btnPopulateEpps = new Button();
      this.iconBtnLockDelete = new StandardIconButton();
      this.verticalSeparator1 = new VerticalSeparator();
      this.toolTip1 = new ToolTip(this.components);
      this.grpContainerLock.SuspendLayout();
      ((ISupportInitialize) this.iconBtnLockDelete).BeginInit();
      this.SuspendLayout();
      this.gridViewEPPSPrograms.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Program ID";
      gvColumn1.Width = 120;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Program Name";
      gvColumn2.Width = 260;
      this.gridViewEPPSPrograms.Columns.AddRange(new GVColumn[2]
      {
        gvColumn1,
        gvColumn2
      });
      this.gridViewEPPSPrograms.Dock = DockStyle.Fill;
      this.gridViewEPPSPrograms.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gridViewEPPSPrograms.Location = new Point(1, 26);
      this.gridViewEPPSPrograms.Name = "gridViewEPPSPrograms";
      this.gridViewEPPSPrograms.Size = new Size(1020, 443);
      this.gridViewEPPSPrograms.SortIconVisible = false;
      this.gridViewEPPSPrograms.TabIndex = 0;
      this.gridViewEPPSPrograms.SelectedIndexChanged += new EventHandler(this.gridViewEPPSPrograms_SelectedIndexChanged);
      this.grpContainerLock.Controls.Add((Control) this.btnPopulateEpps);
      this.grpContainerLock.Controls.Add((Control) this.iconBtnLockDelete);
      this.grpContainerLock.Controls.Add((Control) this.verticalSeparator1);
      this.grpContainerLock.Controls.Add((Control) this.gridViewEPPSPrograms);
      this.grpContainerLock.Dock = DockStyle.Fill;
      this.grpContainerLock.HeaderForeColor = SystemColors.ControlText;
      this.grpContainerLock.Location = new Point(0, 0);
      this.grpContainerLock.Name = "grpContainerLock";
      this.grpContainerLock.Size = new Size(1022, 470);
      this.grpContainerLock.TabIndex = 1;
      this.grpContainerLock.Text = "ICE PPE Loan Program Table";
      this.btnPopulateEpps.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnPopulateEpps.BackColor = SystemColors.Control;
      this.btnPopulateEpps.Location = new Point(862, 1);
      this.btnPopulateEpps.Name = "btnPopulateEpps";
      this.btnPopulateEpps.Size = new Size(120, 22);
      this.btnPopulateEpps.TabIndex = 9;
      this.btnPopulateEpps.Text = "Populate from ICE PPE";
      this.btnPopulateEpps.UseVisualStyleBackColor = true;
      this.btnPopulateEpps.Click += new EventHandler(this.btnPopulateEpps_Click);
      this.iconBtnLockDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.iconBtnLockDelete.BackColor = Color.Transparent;
      this.iconBtnLockDelete.Enabled = false;
      this.iconBtnLockDelete.Location = new Point(988, 4);
      this.iconBtnLockDelete.MouseDownImage = (Image) null;
      this.iconBtnLockDelete.Name = "iconBtnLockDelete";
      this.iconBtnLockDelete.Size = new Size(16, 17);
      this.iconBtnLockDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.iconBtnLockDelete.TabIndex = 5;
      this.iconBtnLockDelete.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.iconBtnLockDelete, "Delete ICE PPE Loan Program");
      this.iconBtnLockDelete.Click += new EventHandler(this.iconBtnLockDelete_Click);
      this.verticalSeparator1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.verticalSeparator1.Location = new Point(1012, 4);
      this.verticalSeparator1.MaximumSize = new Size(2, 16);
      this.verticalSeparator1.MinimumSize = new Size(2, 16);
      this.verticalSeparator1.Name = "verticalSeparator1";
      this.verticalSeparator1.Size = new Size(2, 16);
      this.verticalSeparator1.TabIndex = 3;
      this.verticalSeparator1.Text = "verticalSeparator1";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.grpContainerLock);
      this.Name = nameof (EPPSProgramTableExplorer);
      this.Size = new Size(1022, 470);
      this.grpContainerLock.ResumeLayout(false);
      ((ISupportInitialize) this.iconBtnLockDelete).EndInit();
      this.ResumeLayout(false);
    }
  }
}
