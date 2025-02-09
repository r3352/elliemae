// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.CountyLimitConflict
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.UI;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class CountyLimitConflict : UserControl
  {
    private IContainer components;
    private RadioButton rdoOriginal;
    private RadioButton rdoNewLimit;
    private GroupContainer gcConflict;
    private ListView lsvWebSite;
    private ColumnHeader columnHeader5;
    private ColumnHeader columnHeader6;
    private ColumnHeader columnHeader7;
    private ColumnHeader columnHeader8;
    private ListView lsvCurrent;
    private ColumnHeader columnHeader1;
    private ColumnHeader columnHeader2;
    private ColumnHeader columnHeader3;
    private ColumnHeader columnHeader4;

    public CountyLimitConflict(CountyLimit originalLimit, CountyLimit newLimit)
    {
      this.InitializeComponent();
      this.gcConflict.Text = "County: " + originalLimit.CountyName + ", " + originalLimit.StateAbbreviation;
      this.lsvCurrent.Items.Add(new ListViewItem(this.formatToDouble(string.Concat((object) originalLimit.LimitFor1Unit)))
      {
        SubItems = {
          this.formatToDouble(string.Concat((object) originalLimit.LimitFor2Units)),
          this.formatToDouble(string.Concat((object) originalLimit.LimitFor3Units)),
          this.formatToDouble(string.Concat((object) originalLimit.LimitFor4Units))
        }
      });
      this.lsvWebSite.Items.Add(new ListViewItem(this.formatToDouble(string.Concat((object) newLimit.LimitFor1Unit)))
      {
        SubItems = {
          this.formatToDouble(string.Concat((object) newLimit.LimitFor2Units)),
          this.formatToDouble(string.Concat((object) newLimit.LimitFor3Units)),
          this.formatToDouble(string.Concat((object) newLimit.LimitFor4Units))
        }
      });
      this.rdoOriginal.Tag = (object) originalLimit;
      this.rdoNewLimit.Tag = (object) newLimit;
      this.rdoOriginal.Checked = true;
    }

    private string formatToDouble(string strValue)
    {
      if (strValue == string.Empty || strValue == null)
        return "0";
      string str = double.Parse(strValue.Replace(",", string.Empty)).ToString("N2");
      return str.Substring(0, str.IndexOf('.'));
    }

    public CountyLimit SelectedCountyLimit
    {
      get
      {
        return this.rdoOriginal.Checked ? (CountyLimit) this.rdoOriginal.Tag : (CountyLimit) this.rdoNewLimit.Tag;
      }
    }

    public void SetRecord(CountyLimitConflict.Type type)
    {
      this.rdoOriginal.Enabled = true;
      this.rdoNewLimit.Enabled = true;
      if (type == CountyLimitConflict.Type.Customized)
      {
        this.rdoOriginal.Checked = true;
        this.rdoNewLimit.Enabled = false;
      }
      else
      {
        if (type != CountyLimitConflict.Type.Imported)
          return;
        this.rdoNewLimit.Checked = true;
        this.rdoOriginal.Enabled = false;
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
      this.rdoOriginal = new RadioButton();
      this.rdoNewLimit = new RadioButton();
      this.gcConflict = new GroupContainer();
      this.lsvCurrent = new ListView();
      this.columnHeader1 = new ColumnHeader();
      this.columnHeader2 = new ColumnHeader();
      this.columnHeader3 = new ColumnHeader();
      this.columnHeader4 = new ColumnHeader();
      this.lsvWebSite = new ListView();
      this.columnHeader5 = new ColumnHeader();
      this.columnHeader6 = new ColumnHeader();
      this.columnHeader7 = new ColumnHeader();
      this.columnHeader8 = new ColumnHeader();
      this.gcConflict.SuspendLayout();
      this.SuspendLayout();
      this.rdoOriginal.AutoSize = true;
      this.rdoOriginal.Location = new Point(19, 37);
      this.rdoOriginal.Name = "rdoOriginal";
      this.rdoOriginal.Size = new Size(59, 17);
      this.rdoOriginal.TabIndex = 1;
      this.rdoOriginal.TabStop = true;
      this.rdoOriginal.Text = "Current";
      this.rdoOriginal.UseVisualStyleBackColor = true;
      this.rdoNewLimit.AutoSize = true;
      this.rdoNewLimit.Location = new Point(19, 105);
      this.rdoNewLimit.Name = "rdoNewLimit";
      this.rdoNewLimit.Size = new Size(64, 17);
      this.rdoNewLimit.TabIndex = 2;
      this.rdoNewLimit.TabStop = true;
      this.rdoNewLimit.Text = "Website";
      this.rdoNewLimit.UseVisualStyleBackColor = true;
      this.gcConflict.Controls.Add((Control) this.lsvWebSite);
      this.gcConflict.Controls.Add((Control) this.lsvCurrent);
      this.gcConflict.Controls.Add((Control) this.rdoOriginal);
      this.gcConflict.Controls.Add((Control) this.rdoNewLimit);
      this.gcConflict.Dock = DockStyle.Fill;
      this.gcConflict.Location = new Point(0, 0);
      this.gcConflict.Name = "gcConflict";
      this.gcConflict.Size = new Size(463, 182);
      this.gcConflict.TabIndex = 19;
      this.gcConflict.Text = "Location:";
      this.lsvCurrent.Columns.AddRange(new ColumnHeader[4]
      {
        this.columnHeader1,
        this.columnHeader2,
        this.columnHeader3,
        this.columnHeader4
      });
      this.lsvCurrent.GridLines = true;
      this.lsvCurrent.HeaderStyle = ColumnHeaderStyle.Nonclickable;
      this.lsvCurrent.Location = new Point(19, 60);
      this.lsvCurrent.Name = "lsvCurrent";
      this.lsvCurrent.Size = new Size(420, 39);
      this.lsvCurrent.TabIndex = 19;
      this.lsvCurrent.UseCompatibleStateImageBehavior = false;
      this.lsvCurrent.View = View.Details;
      this.columnHeader1.Text = "Limit for 1 Unit";
      this.columnHeader1.Width = 110;
      this.columnHeader2.Text = "Limit for 2 Units";
      this.columnHeader2.Width = 102;
      this.columnHeader3.Text = "Limit for 3 Units";
      this.columnHeader3.Width = 105;
      this.columnHeader4.Text = "Limit for 4 Units";
      this.columnHeader4.Width = 97;
      this.lsvWebSite.Columns.AddRange(new ColumnHeader[4]
      {
        this.columnHeader5,
        this.columnHeader6,
        this.columnHeader7,
        this.columnHeader8
      });
      this.lsvWebSite.GridLines = true;
      this.lsvWebSite.HeaderStyle = ColumnHeaderStyle.Nonclickable;
      this.lsvWebSite.Location = new Point(19, 128);
      this.lsvWebSite.Name = "lsvWebSite";
      this.lsvWebSite.Size = new Size(420, 39);
      this.lsvWebSite.TabIndex = 20;
      this.lsvWebSite.UseCompatibleStateImageBehavior = false;
      this.lsvWebSite.View = View.Details;
      this.columnHeader5.Text = "Limit for 1 Unit";
      this.columnHeader5.Width = 110;
      this.columnHeader6.Text = "Limit for 2 Units";
      this.columnHeader6.Width = 102;
      this.columnHeader7.Text = "Limit for 3 Units";
      this.columnHeader7.Width = 105;
      this.columnHeader8.Text = "Limit for 4 Units";
      this.columnHeader8.Width = 97;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.gcConflict);
      this.Name = nameof (CountyLimitConflict);
      this.Size = new Size(463, 182);
      this.gcConflict.ResumeLayout(false);
      this.gcConflict.PerformLayout();
      this.ResumeLayout(false);
    }

    public enum Type
    {
      Customized,
      Imported,
      Mixed,
    }
  }
}
