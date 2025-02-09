// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.FannieMaePEMBS.FannieMaeProductNames
// Assembly: TradeManagement, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 30F9401A-5385-498E-9C5B-D147722AC94C
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\TradeManagement.dll

using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Trading.FannieMaePEMBS
{
  public class FannieMaeProductNames : Form
  {
    private DataTable selected;
    private FannieMaeProductGrid gridfunction;
    private IContainer components;
    private Button btnCancel;
    private Button btnSelect;
    private GridView gridFannieMaeProductsName;

    public FannieMaeProductNames()
    {
      this.InitializeComponent();
      this.gridfunction = new FannieMaeProductGrid();
      this.initFannieMaeProductNameList();
    }

    private void initFannieMaeProductNameList()
    {
      GVItem[] items = this.gridfunction.ConvertProductName(Session.ConfigurationManager.GetFannieMaeProductNames());
      this.gridFannieMaeProductsName.Items.Clear();
      this.gridFannieMaeProductsName.Items.AddRange(items);
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
      this.Close();
    }

    private void btnSelect_Click(object sender, EventArgs e)
    {
      this.selected = this.gridfunction.ConvertSelectedProductName(this.gridFannieMaeProductsName.Items);
      this.DialogResult = DialogResult.Yes;
      this.Close();
    }

    public DataTable Selected => this.selected;

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
      this.btnCancel = new Button();
      this.btnSelect = new Button();
      this.gridFannieMaeProductsName = new GridView();
      this.SuspendLayout();
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.AutoSize = true;
      this.btnCancel.Location = new Point(464, 218);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 2;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.btnSelect.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnSelect.Location = new Point(365, 218);
      this.btnSelect.Name = "btnSelect";
      this.btnSelect.Size = new Size(75, 23);
      this.btnSelect.TabIndex = 1;
      this.btnSelect.Text = "Select";
      this.btnSelect.UseVisualStyleBackColor = true;
      this.btnSelect.Click += new EventHandler(this.btnSelect_Click);
      this.gridFannieMaeProductsName.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "colName";
      gvColumn1.Text = "Fannie Mae Product Name";
      gvColumn1.Width = 300;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "colDisplayName";
      gvColumn2.Text = "Product Display Name";
      gvColumn2.Width = 300;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "colDescription";
      gvColumn3.Text = "Product Description";
      gvColumn3.Width = 300;
      this.gridFannieMaeProductsName.Columns.AddRange(new GVColumn[3]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3
      });
      this.gridFannieMaeProductsName.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gridFannieMaeProductsName.Location = new Point(0, 0);
      this.gridFannieMaeProductsName.Name = "gridFannieMaeProductsName";
      this.gridFannieMaeProductsName.Size = new Size(551, 208);
      this.gridFannieMaeProductsName.TabIndex = 0;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(551, 253);
      this.Controls.Add((Control) this.gridFannieMaeProductsName);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnSelect);
      this.MaximumSize = new Size(567, (int) short.MaxValue);
      this.Name = nameof (FannieMaeProductNames);
      this.ShowIcon = false;
      this.Text = "Select Product Name";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
