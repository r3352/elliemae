// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.DisclosedItemizationDialog
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using AxSHDocVw;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.InputEngine.Forms;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Setup;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class DisclosedItemizationDialog : Form, IOnlineHelpTarget, IHelp
  {
    private const string className = "DisclosedItemizationDialog";
    private static string sw = Tracing.SwInputEngine;
    private string formName = "2015 Itemization";
    private IDisclosureTracking2015Log log;
    private System.Runtime.InteropServices.ComTypes.IConnectionPoint conPt;
    private int cookie;
    private IHtmlInput input;
    private FrmBrowserHandler brwHandler;
    private static object nobj = (object) Missing.Value;
    private IContainer components;
    private Button btnCancel;
    private AxWebBrowser axWebBrowser;
    private Panel panel1;
    private PictureBox pictureBox2;
    private PictureBox pictureBox1;
    private ImageList imageList1;

    public DisclosedItemizationDialog(IDisclosureTracking2015Log log)
    {
      this.InitializeComponent();
      this.log = log;
      this.initialPage();
    }

    private void initialPage()
    {
      this.pictureBox1.Image = this.imageList1.Images[0];
      this.pictureBox2.Image = this.imageList1.Images[2];
      InputFormInfo formByName = new InputFormList(Session.SessionObjects).GetFormByName(this.formName);
      this.input = (IHtmlInput) new DisclosedItemizationHandler(this.log, LoanReportFieldDefs.GetFieldDefs(Session.DefaultInstance, LoanReportFieldFlags.AllLoanDataFields));
      this.brwHandler = new FrmBrowserHandler(Session.DefaultInstance, (IWin32Window) this, this.input);
      this.hookUpBrowserHandler();
      this.axWebBrowser.Navigate(FormStore.GetFormHTMLPath(Session.DefaultInstance, formByName), ref DisclosedItemizationDialog.nobj, ref DisclosedItemizationDialog.nobj, ref DisclosedItemizationDialog.nobj, ref DisclosedItemizationDialog.nobj);
    }

    protected void hookUpBrowserHandler()
    {
      System.Runtime.InteropServices.ComTypes.IConnectionPointContainer ocx = (System.Runtime.InteropServices.ComTypes.IConnectionPointContainer) this.axWebBrowser.GetOcx();
      Guid guid = typeof (SHDocVw.DWebBrowserEvents2).GUID;
      ocx.FindConnectionPoint(ref guid, out this.conPt);
      this.brwHandler.SetFieldsReadonly = true;
      this.conPt.Advise((object) this.brwHandler, out this.cookie);
      this.brwHandler.SetHelpTarget((IOnlineHelpTarget) this);
    }

    public string GetHelpTargetName() => "";

    public void ShowHelp()
    {
      JedHelp.ShowHelp((Control) this, SystemSettings.HelpFile, this.GetHelpTargetName());
    }

    private void pictureBox1_Click(object sender, EventArgs e)
    {
      ((REGZGFE_2015InputHandler) this.brwHandler.GetInputHandler()).RefreshLayout(true, false, (string) null);
    }

    private void pictureBox2_Click(object sender, EventArgs e)
    {
      ((REGZGFE_2015InputHandler) this.brwHandler.GetInputHandler()).RefreshLayout(false, true, (string) null);
    }

    private void pictureBox1_MouseLeave(object sender, EventArgs e)
    {
      PictureBox pictureBox = (PictureBox) sender;
      if (!pictureBox.Enabled)
        return;
      pictureBox.Image = this.imageList1.Images[0];
    }

    private void pictureBox1_MouseEnter(object sender, EventArgs e)
    {
      ((PictureBox) sender).Image = this.imageList1.Images[1];
    }

    private void pictureBox2_MouseLeave(object sender, EventArgs e)
    {
      PictureBox pictureBox = (PictureBox) sender;
      if (!pictureBox.Enabled)
        return;
      pictureBox.Image = this.imageList1.Images[2];
    }

    private void pictureBox2_MouseEnter(object sender, EventArgs e)
    {
      ((PictureBox) sender).Image = this.imageList1.Images[3];
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (DisclosedItemizationDialog));
      this.btnCancel = new Button();
      this.axWebBrowser = new AxWebBrowser();
      this.panel1 = new Panel();
      this.pictureBox2 = new PictureBox();
      this.pictureBox1 = new PictureBox();
      this.imageList1 = new ImageList(this.components);
      this.axWebBrowser.BeginInit();
      this.panel1.SuspendLayout();
      ((ISupportInitialize) this.pictureBox2).BeginInit();
      ((ISupportInitialize) this.pictureBox1).BeginInit();
      this.SuspendLayout();
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(766, 672);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 9;
      this.btnCancel.Text = "Close";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.axWebBrowser.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.axWebBrowser.Enabled = true;
      this.axWebBrowser.Location = new Point(0, 47);
      this.axWebBrowser.OcxState = (AxHost.State) componentResourceManager.GetObject("axWebBrowser.OcxState");
      this.axWebBrowser.Size = new Size(850, 619);
      this.axWebBrowser.TabIndex = 8;
      this.panel1.Controls.Add((Control) this.pictureBox2);
      this.panel1.Controls.Add((Control) this.pictureBox1);
      this.panel1.Dock = DockStyle.Top;
      this.panel1.Location = new Point(0, 0);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(853, 47);
      this.panel1.TabIndex = 10;
      this.pictureBox2.Location = new Point(806, 14);
      this.pictureBox2.Name = "pictureBox2";
      this.pictureBox2.Size = new Size(16, 16);
      this.pictureBox2.TabIndex = 1;
      this.pictureBox2.TabStop = false;
      this.pictureBox2.Click += new EventHandler(this.pictureBox2_Click);
      this.pictureBox2.MouseEnter += new EventHandler(this.pictureBox2_MouseEnter);
      this.pictureBox2.MouseLeave += new EventHandler(this.pictureBox2_MouseLeave);
      this.pictureBox1.Location = new Point(784, 14);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new Size(16, 16);
      this.pictureBox1.TabIndex = 0;
      this.pictureBox1.TabStop = false;
      this.pictureBox1.Click += new EventHandler(this.pictureBox1_Click);
      this.pictureBox1.MouseEnter += new EventHandler(this.pictureBox1_MouseEnter);
      this.pictureBox1.MouseLeave += new EventHandler(this.pictureBox1_MouseLeave);
      this.imageList1.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imageList1.ImageStream");
      this.imageList1.TransparentColor = Color.Transparent;
      this.imageList1.Images.SetKeyName(0, "expand.png");
      this.imageList1.Images.SetKeyName(1, "expand-over.png");
      this.imageList1.Images.SetKeyName(2, "collapse.png");
      this.imageList1.Images.SetKeyName(3, "collapse-over.png");
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(853, 708);
      this.Controls.Add((Control) this.panel1);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.axWebBrowser);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (DisclosedItemizationDialog);
      this.Text = "Itemization Snapshot";
      this.axWebBrowser.EndInit();
      this.panel1.ResumeLayout(false);
      ((ISupportInitialize) this.pictureBox2).EndInit();
      ((ISupportInitialize) this.pictureBox1).EndInit();
      this.ResumeLayout(false);
    }
  }
}
