// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.WarehouseSelectionDialog
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.ExternalOriginatorManagement;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class WarehouseSelectionDialog : Form
  {
    private List<ExternalOrgWarehouse> externalOrgWarehouses;
    private ExternalOriginatorManagementData company;
    private Sessions.Session session;
    private GenericFormInputHandler inputHandler;
    private IHtmlInput inputData;
    private IContainer components;
    private Button btnSelect;
    private Button btnCancel;
    private GroupContainer groupContainer1;
    private GridView WarehouseView;

    public WarehouseSelectionDialog(IHtmlInput inputData, Sessions.Session session)
    {
      this.inputData = inputData;
      this.session = session;
      this.InitializeComponent();
      this.inputHandler = new GenericFormInputHandler(this.inputData, this.Controls, this.session);
      this.InitForm();
    }

    public void InitForm()
    {
      this.company = this.loadCompany();
      this.externalOrgWarehouses = this.loadBankList();
      this.buildGrid();
      this.WarehouseView_SelectedIndexChanged((object) null, (EventArgs) null);
    }

    private ExternalOriginatorManagementData loadCompany()
    {
      ExternalOriginatorManagementData originatorManagementData1 = (ExternalOriginatorManagementData) null;
      string field = this.session.LoanData.GetField("TPO.X15");
      if (field == "")
        return (ExternalOriginatorManagementData) null;
      foreach (ExternalOriginatorManagementData originatorManagementData2 in Session.ConfigurationManager.GetExternalOrganizationByTPOID(field))
      {
        if (originatorManagementData2.OrganizationType == ExternalOriginatorOrgType.Company)
        {
          originatorManagementData1 = originatorManagementData2;
          break;
        }
      }
      return originatorManagementData1;
    }

    private List<ExternalOrgWarehouse> loadBankList()
    {
      if (this.company == null)
        return (List<ExternalOrgWarehouse>) null;
      string branchName = this.session.LoanData.GetField("TPO.X38");
      return branchName == "" ? Session.ConfigurationManager.GetExternalOrgWarehousesbyCompanies(this.company.oid).Where<ExternalOrgWarehouse>((Func<ExternalOrgWarehouse, bool>) (c => c.Approved)).ToList<ExternalOrgWarehouse>() : Session.ConfigurationManager.GetExternalOrgWarehousesbyCompanies(this.company.oid).Where<ExternalOrgWarehouse>((Func<ExternalOrgWarehouse, bool>) (c => c.OrgName == branchName && c.Approved)).ToList<ExternalOrgWarehouse>();
    }

    private GVItem createitem(ExternalOrgWarehouse wareHouse)
    {
      return new GVItem(new string[7]
      {
        wareHouse.OrgName,
        Enum.GetName(typeof (ExternalOriginatorOrgType), (object) wareHouse.OrgType),
        wareHouse.BankName,
        wareHouse.City,
        wareHouse.AcctNumber,
        wareHouse.ABANumber,
        Convert.ToString(wareHouse.Approved)
      });
    }

    private void buildGrid()
    {
      this.WarehouseView.Items.Clear();
      if (this.externalOrgWarehouses == null)
        return;
      foreach (ExternalOrgWarehouse externalOrgWarehouse in this.externalOrgWarehouses)
      {
        GVItem gvItem = this.createitem(externalOrgWarehouse);
        gvItem.Tag = (object) externalOrgWarehouse;
        gvItem.SubItems[6].Value = (object) null;
        gvItem.SubItems[6].Checked = true;
        gvItem.SubItems[6].CheckBoxEnabled = false;
        this.WarehouseView.Items.Add(gvItem);
      }
      this.WarehouseView.Refresh();
    }

    private void btnSelect_Click(object sender, EventArgs e)
    {
      if (this.WarehouseView.SelectedItems.Count != 1)
        return;
      this.setWareHouse((ExternalOrgWarehouse) this.WarehouseView.SelectedItems[0].Tag);
      this.DialogResult = DialogResult.OK;
    }

    private void setWareHouse(ExternalOrgWarehouse wareHouse)
    {
      this.session.LoanData.SetField("3964", wareHouse.BankID.ToString());
      this.session.LoanData.SetField("3943", wareHouse.BankName);
      this.session.LoanData.SetField("3944", wareHouse.AcctNumber);
      this.session.LoanData.SetField("3945", wareHouse.ABANumber);
      this.session.LoanData.SetField("4202", wareHouse.AcctName);
      this.session.LoanData.SetField("4203", wareHouse.CreditAcctNumber);
      this.session.LoanData.SetField("4204", wareHouse.CreditAcctName);
      this.session.LoanData.SetField("3946", "");
      this.session.LoanData.SetField("3947", "");
      this.session.LoanData.SetField("3948", wareHouse.Address);
      this.session.LoanData.SetField("3949", wareHouse.Address1);
      this.session.LoanData.SetField("3950", wareHouse.City);
      this.session.LoanData.SetField("3951", wareHouse.State);
      this.session.LoanData.SetField("3952", wareHouse.Zip);
      this.session.LoanData.SetField("3953", wareHouse.Description);
      this.session.LoanData.SetField("3954", this.intToBool(wareHouse.SelfFunder));
      this.session.LoanData.SetField("3955", this.intToBool(wareHouse.BaileeReq));
      if (wareHouse.Expiration != DateTime.MinValue)
        this.session.LoanData.SetField("3956", wareHouse.Expiration.ToString());
      else
        this.session.LoanData.SetField("3956", "");
      this.session.LoanData.SetField("3957", this.intToBool(wareHouse.TriParty));
      this.session.LoanData.SetField("3963", "Y");
      this.session.LoanData.Calculator.CalcDefaultTpoBankContact();
    }

    private string intToBool(int value)
    {
      if (value == 1)
        return "Y";
      return value == 2 ? "N" : "";
    }

    private void WarehouseView_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.btnSelect.Enabled = this.WarehouseView.SelectedItems.Count == 1;
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
      GVColumn gvColumn7 = new GVColumn();
      this.btnSelect = new Button();
      this.btnCancel = new Button();
      this.WarehouseView = new GridView();
      this.groupContainer1 = new GroupContainer();
      this.groupContainer1.SuspendLayout();
      this.SuspendLayout();
      this.btnSelect.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnSelect.Location = new Point(803, 288);
      this.btnSelect.Name = "btnSelect";
      this.btnSelect.Size = new Size(75, 23);
      this.btnSelect.TabIndex = 1;
      this.btnSelect.Text = "&Select";
      this.btnSelect.UseVisualStyleBackColor = true;
      this.btnSelect.Click += new EventHandler(this.btnSelect_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(897, 287);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 2;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.WarehouseView.AllowMultiselect = false;
      this.WarehouseView.BorderColor = Color.Transparent;
      this.WarehouseView.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Org Name";
      gvColumn1.Width = 167;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Org Type";
      gvColumn2.Width = 100;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.Text = "Bank Name";
      gvColumn3.Width = 180;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column4";
      gvColumn4.SpringToFit = true;
      gvColumn4.Text = "City";
      gvColumn4.Width = 156;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Column5";
      gvColumn5.Text = "Account #";
      gvColumn5.Width = 180;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "Column6";
      gvColumn6.Text = "ABA#";
      gvColumn6.Width = 180;
      gvColumn7.Name = "Column7";
      gvColumn7.Text = "Approved";
      gvColumn7.Width = 80;
      gvColumn7.CheckBoxes = true;
      gvColumn7.TextAlignment = ContentAlignment.MiddleCenter;
      this.WarehouseView.Columns.AddRange(new GVColumn[7]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5,
        gvColumn6,
        gvColumn7
      });
      this.WarehouseView.Dock = DockStyle.Fill;
      this.WarehouseView.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.WarehouseView.ItemWordWrap = true;
      this.WarehouseView.Location = new Point(1, 26);
      this.WarehouseView.MinimumSize = new Size(629, 229);
      this.WarehouseView.Name = "WarehouseView";
      this.WarehouseView.Size = new Size(963, 229);
      this.WarehouseView.TabIndex = 0;
      this.WarehouseView.SelectedIndexChanged += new EventHandler(this.WarehouseView_SelectedIndexChanged);
      this.WarehouseView.DoubleClick += new EventHandler(this.btnSelect_Click);
      this.groupContainer1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.groupContainer1.Controls.Add((Control) this.WarehouseView);
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(9, 9);
      this.groupContainer1.MinimumSize = new Size(631, 226);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(965, 256);
      this.groupContainer1.TabIndex = 0;
      this.groupContainer1.Text = "Warehouse Banks";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(983, 321);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnSelect);
      this.Controls.Add((Control) this.groupContainer1);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.MinimumSize = new Size(665, 359);
      this.Name = nameof (WarehouseSelectionDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.Text = "Warehouse Bank Selector";
      this.groupContainer1.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
