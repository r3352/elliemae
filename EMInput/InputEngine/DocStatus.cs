// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.DocStatus
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.UI;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class DocStatus : UserControl
  {
    private InitialRequest initialRequest;
    private ReturnRequest returnRequest;
    private ShippingStatus shippingStatus;
    private IContainer components;
    private GroupContainer gcTitle;
    private TabControl tabControl2;
    private TabPage tpInitialRequest;
    private TabPage tpShippingStatus;
    private TabPage tpReturnRequest;

    public DocStatus(DocTrackingType docTrackingType)
    {
      this.InitializeComponent();
      switch (docTrackingType)
      {
        case DocTrackingType.FTP:
          this.gcTitle.Text = DocTrackingUtils.Final_Title_Policy;
          break;
        case DocTrackingType.EN:
          this.gcTitle.Text = DocTrackingUtils.Executed_Note;
          break;
        default:
          this.gcTitle.Text = DocTrackingUtils.DOT_Mortgage;
          break;
      }
      if (docTrackingType == DocTrackingType.EN)
      {
        this.tabControl2.TabPages.Remove(this.tpInitialRequest);
      }
      else
      {
        this.initialRequest = new InitialRequest(docTrackingType);
        this.tpInitialRequest.Controls.Add((Control) this.initialRequest);
      }
      this.shippingStatus = new ShippingStatus(docTrackingType);
      this.tpShippingStatus.Controls.Add((Control) this.shippingStatus);
      if (docTrackingType == DocTrackingType.EN)
      {
        this.tabControl2.TabPages.Remove(this.tpReturnRequest);
      }
      else
      {
        this.returnRequest = new ReturnRequest(docTrackingType);
        this.tpReturnRequest.Controls.Add((Control) this.returnRequest);
      }
    }

    public void EnableDisableSubTabs(bool isEnable)
    {
      if (this.initialRequest != null)
        this.initialRequest.EnableDisableInitialRequest(isEnable);
      if (this.shippingStatus != null)
        this.shippingStatus.EnableDisableShippingStatus(isEnable);
      if (this.returnRequest == null)
        return;
      this.returnRequest.EnableDisableReturnRequst(isEnable);
    }

    public void InitData()
    {
      if (this.initialRequest != null)
        this.initialRequest?.InitData();
      if (this.shippingStatus != null)
        this.shippingStatus?.InitData();
      if (this.returnRequest == null)
        return;
      this.returnRequest?.InitData();
    }

    public ReturnRequest GetReturnReqestControls() => this.returnRequest;

    public void RefreshContent()
    {
      if (this.initialRequest != null)
        this.initialRequest.RefreshContent();
      if (this.shippingStatus != null)
        this.shippingStatus.InitData();
      if (this.returnRequest == null)
        return;
      this.returnRequest.InitData();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.gcTitle = new GroupContainer();
      this.tabControl2 = new TabControl();
      this.tpInitialRequest = new TabPage();
      this.tpShippingStatus = new TabPage();
      this.tpReturnRequest = new TabPage();
      this.gcTitle.SuspendLayout();
      this.tabControl2.SuspendLayout();
      this.SuspendLayout();
      this.gcTitle.AutoScroll = true;
      this.gcTitle.Controls.Add((Control) this.tabControl2);
      this.gcTitle.HeaderForeColor = SystemColors.ControlText;
      this.gcTitle.Location = new Point(0, 0);
      this.gcTitle.Name = "gcTitle";
      this.gcTitle.Size = new Size(963, 340);
      this.gcTitle.TabIndex = 14;
      this.gcTitle.Text = DocTrackingUtils.DOT_Mortgage;
      this.tabControl2.Controls.Add((Control) this.tpInitialRequest);
      this.tabControl2.Controls.Add((Control) this.tpShippingStatus);
      this.tabControl2.Controls.Add((Control) this.tpReturnRequest);
      this.tabControl2.Dock = DockStyle.Fill;
      this.tabControl2.Location = new Point(1, 26);
      this.tabControl2.Name = "tabControl2";
      this.tabControl2.SelectedIndex = 0;
      this.tabControl2.Size = new Size(961, 313);
      this.tabControl2.TabIndex = 0;
      this.tpInitialRequest.Location = new Point(4, 22);
      this.tpInitialRequest.Name = "tpInitialRequest";
      this.tpInitialRequest.Padding = new Padding(3);
      this.tpInitialRequest.Size = new Size(953, 287);
      this.tpInitialRequest.TabIndex = 0;
      this.tpInitialRequest.Text = "Initial Requests";
      this.tpInitialRequest.UseVisualStyleBackColor = true;
      this.tpShippingStatus.Location = new Point(4, 22);
      this.tpShippingStatus.Name = "tpShippingStatus";
      this.tpShippingStatus.Padding = new Padding(3);
      this.tpShippingStatus.Size = new Size(953, 287);
      this.tpShippingStatus.TabIndex = 1;
      this.tpShippingStatus.Text = "Shipping Status";
      this.tpShippingStatus.UseVisualStyleBackColor = true;
      this.tpReturnRequest.Location = new Point(4, 22);
      this.tpReturnRequest.Name = "tpReturnRequest";
      this.tpReturnRequest.Padding = new Padding(3);
      this.tpReturnRequest.Size = new Size(953, 287);
      this.tpReturnRequest.TabIndex = 2;
      this.tpReturnRequest.Text = "Return Requests";
      this.tpReturnRequest.UseVisualStyleBackColor = true;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.gcTitle);
      this.Name = nameof (DocStatus);
      this.Size = new Size(964, 344);
      this.gcTitle.ResumeLayout(false);
      this.tabControl2.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
