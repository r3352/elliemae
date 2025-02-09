// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.DocumentTrackingManagement
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using EllieMae.Encompass.AsmResolver;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class DocumentTrackingManagement : UserControl, IOnlineHelpTarget
  {
    private LoanData loanData;
    private DocStatus dotMortgageControl;
    private DocStatus ftpControl;
    private DocStatus enControl;
    private ImageList imageList;
    private IContainer components;
    private ImageList imageList1;
    private Panel panel4;
    private Label label27;
    private GroupContainer gpSetup;
    private Panel panel1;
    private GroupContainer groupContainer3;
    private RichTextBox richTextBox_Comments;
    private Button btnAddConmment;
    private GroupContainer gcTrackHistory;
    private GridView gvTrackHistory;
    private TabControl tbcDocTracking;
    private TabPage tbpDot;
    private Panel pnlDotStatus;
    private TabPage tbpFTP;
    private Panel pnlFTPStatus;
    private Panel pnlENStatus;
    private TabPage tbpEN;

    public DocumentTrackingManagement(Sessions.Session session)
    {
      DocTrackingUtils.Session = session;
      DocTrackingUtils.DocTrackingSettings = session.SessionObjects.GetCompanySettingsFromCache("Doctracking");
      DocTrackingUtils.PoliciesSetting = session.ServerManager.GetServerSettings("Policies");
      DocTrackingUtils.DocTrackingManagementForm = this;
      FeaturesAclManager aclManager = (FeaturesAclManager) session.ACL.GetAclManager(AclCategory.Features);
      bool flag = Session.UserInfo.IsSuperAdministrator() || Session.UserInfo.IsAdministrator();
      if (!flag)
        flag = aclManager.GetUserApplicationRight(AclFeature.ToolsTab_ImportShippingDetails);
      this.loanData = session.LoanData;
      this.InitializeComponent();
      this.Dock = DockStyle.Fill;
      this.imageList = new ImageList();
      this.imageList.Images.Add(Image.FromFile(AssemblyResolver.GetResourceFileFullPath(Path.Combine(SystemSettings.ImageRelDir, "check-mark-green.png"), SystemSettings.LocalAppDir)));
      this.gvTrackHistory.ImageList = this.imageList;
      this.dotMortgageControl = new DocStatus(DocTrackingType.DOT);
      this.pnlDotStatus.Controls.Add((Control) this.dotMortgageControl);
      this.ftpControl = new DocStatus(DocTrackingType.FTP);
      this.pnlFTPStatus.Controls.Add((Control) this.ftpControl);
      if (!flag)
        this.tbcDocTracking.TabPages.Remove(this.tbpEN);
      this.enControl = new DocStatus(DocTrackingType.EN);
      this.pnlENStatus.Controls.Add((Control) this.enControl);
      DateTime date = Utils.ParseDate((object) this.loanData.GetField(DocTrackingUtils.FieldPrefix + "DOT.InitialRequest.PurchaseDate"));
      DocTrackingUtils.IsCalculate = false;
      if (date == DateTime.MinValue || date != DocTrackingUtils.GetNotePurchaseDt())
        DocTrackingUtils.IsCalculate = true;
      if (DocTrackingUtils.IsImport)
        this.tbcDocTracking.SelectedIndex = 2;
      this.EnableDisableDocTracking();
      this.RefreshTrackingHistory();
      this.RefreshComment();
      this.InitData();
    }

    private void EnableDisableDocTracking()
    {
      this.dotMortgageControl.EnableDisableSubTabs(Utils.ParseBoolean(DocTrackingUtils.DocTrackingSettings[(object) "EnableDOT"]));
      this.ftpControl.EnableDisableSubTabs(Utils.ParseBoolean(DocTrackingUtils.DocTrackingSettings[(object) "EnableFTP"]));
      this.enControl.EnableDisableSubTabs(true);
    }

    private void InitData()
    {
      this.dotMortgageControl.InitData();
      this.ftpControl.InitData();
      this.enControl.InitData();
    }

    public string GetHelpTargetName() => "Collateral Tracking";

    private void btnAddConmment_Click(object sender, EventArgs e)
    {
      DocumentTrackingAddComment trackingAddComment = new DocumentTrackingAddComment();
      int num = (int) trackingAddComment.ShowDialog();
      if (trackingAddComment.DialogResult != DialogResult.OK)
        return;
      this.RefreshComment();
    }

    private void RefreshTrackingHistory()
    {
      DocumentTrackingLog[] documentTrackingLogs = DocTrackingUtils.Session.LoanData.GetLogList().GetAllDocumentTrackingLogs();
      this.gvTrackHistory.Items.Clear();
      this.gvTrackHistory.BeginUpdate();
      foreach (DocumentTrackingLog documentTrackingLog in ((IEnumerable<DocumentTrackingLog>) documentTrackingLogs).Reverse<DocumentTrackingLog>())
      {
        int num1 = 0;
        GVItem gvItem = new GVItem();
        gvItem.SubItems.Add((object) documentTrackingLog.Action);
        if (DocTrackingUtils.ActionType[(object) documentTrackingLog.Action] != null && ((ActionObject) DocTrackingUtils.ActionType[(object) documentTrackingLog.Action]).IsBold)
          gvItem.SubItems[0].Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
        int num2 = num1 + 1;
        gvItem.SubItems.Add((object) documentTrackingLog.LogDate);
        int num3 = num2 + 1;
        gvItem.SubItems.Add((object) documentTrackingLog.LogBy);
        int subItemIdx1 = num3 + 1;
        gvItem.SubItems.Add((object) "");
        this.SetCheckMarkImage(documentTrackingLog.Dot, gvItem, subItemIdx1);
        int subItemIdx2 = subItemIdx1 + 1;
        gvItem.SubItems.Add((object) "");
        this.SetCheckMarkImage(documentTrackingLog.Ftp, gvItem, subItemIdx2);
        int subItemIdx3 = subItemIdx2 + 1;
        gvItem.SubItems.Add((object) "");
        this.SetCheckMarkImage(documentTrackingLog.En, gvItem, subItemIdx3);
        int num4 = subItemIdx3 + 1;
        gvItem.SubItems.Add((object) documentTrackingLog.Organization);
        int num5 = num4 + 1;
        gvItem.SubItems.Add((object) documentTrackingLog.Contact);
        int subItemIdx4 = num5 + 1;
        gvItem.SubItems.Add((object) "");
        this.SetCheckMarkImage(documentTrackingLog.Email, gvItem, subItemIdx4);
        int subItemIdx5 = subItemIdx4 + 1;
        gvItem.SubItems.Add((object) "");
        this.SetCheckMarkImage(documentTrackingLog.Phone, gvItem, subItemIdx5);
        int num6 = subItemIdx5 + 1;
        gvItem.Tag = (object) documentTrackingLog;
        this.gvTrackHistory.Items.Add(gvItem);
      }
      this.gvTrackHistory.EndUpdate();
    }

    private void RefreshComment()
    {
      List<DocumentTrackingComment> comments = DocTrackingUtils.GetComments();
      this.richTextBox_Comments.Clear();
      for (int index = comments.Count - 1; index >= 0; --index)
      {
        string str = string.Format("{0} {1} >> ", (object) comments[index].UserName, (object) comments[index].LogDate.ToString("MM/dd/yyyy hh:mm tt"));
        string text = string.Format("{0}{1}\n\n", (object) str, (object) comments[index].CommentText);
        int length1 = this.richTextBox_Comments.Text.Length;
        int length2 = str.Length;
        this.richTextBox_Comments.AppendText(text);
        this.richTextBox_Comments.SelectionStart = length1;
        this.richTextBox_Comments.SelectionLength = length2;
        this.richTextBox_Comments.SelectionFont = new Font(this.Font, FontStyle.Bold);
      }
    }

    public DocStatus GetDOTControls() => this.dotMortgageControl;

    public DocStatus GetFTPControls() => this.ftpControl;

    public DocStatus GetENControls() => this.enControl;

    public void RefreshContent()
    {
      this.dotMortgageControl.RefreshContent();
      this.ftpControl.RefreshContent();
      this.enControl.RefreshContent();
      this.RefreshTrackingHistory();
      this.RefreshComment();
    }

    public void SetCheckMarkImage(bool flag, GVItem item, int subItemIdx)
    {
      if (!flag)
        return;
      item.SubItems[subItemIdx].ImageAlignment = HorizontalAlignment.Center;
      item.SubItems[subItemIdx].ImageIndex = 0;
    }

    private void gvTrackHistory_ItemDoubleClick(object source, GVItemEventArgs e)
    {
      DocumentTrackingLog tag = (DocumentTrackingLog) e.Item.Tag;
      string action = tag.Action;
      int num = (int) ((Form) Activator.CreateInstance(System.Type.GetType(((ActionObject) DocTrackingUtils.ActionType[(object) action]).PopupClassName), (object) tag)).ShowDialog();
    }

    private void richTextBox_Comments_MouseWheel(object sender, MouseEventArgs e)
    {
      if (!(e is HandledMouseEventArgs handledMouseEventArgs) || Control.ModifierKeys != Keys.Control)
        return;
      handledMouseEventArgs.Handled = true;
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (DocumentTrackingManagement));
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
      this.imageList1 = new ImageList(this.components);
      this.gpSetup = new GroupContainer();
      this.panel4 = new Panel();
      this.panel1 = new Panel();
      this.groupContainer3 = new GroupContainer();
      this.richTextBox_Comments = new RichTextBox();
      this.btnAddConmment = new Button();
      this.gcTrackHistory = new GroupContainer();
      this.gvTrackHistory = new GridView();
      this.tbcDocTracking = new TabControl();
      this.tbpDot = new TabPage();
      this.pnlDotStatus = new Panel();
      this.tbpFTP = new TabPage();
      this.pnlFTPStatus = new Panel();
      this.tbpEN = new TabPage();
      this.pnlENStatus = new Panel();
      this.label27 = new Label();
      this.gpSetup.SuspendLayout();
      this.panel4.SuspendLayout();
      this.panel1.SuspendLayout();
      this.groupContainer3.SuspendLayout();
      this.gcTrackHistory.SuspendLayout();
      this.tbcDocTracking.SuspendLayout();
      this.tbpDot.SuspendLayout();
      this.tbpFTP.SuspendLayout();
      this.tbpEN.SuspendLayout();
      this.SuspendLayout();
      this.imageList1.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imageList1.ImageStream");
      this.imageList1.TransparentColor = Color.Transparent;
      this.imageList1.Images.SetKeyName(0, "rate-lock-request.png");
      this.imageList1.Images.SetKeyName(1, "rate-locked.png");
      this.imageList1.Images.SetKeyName(2, "rate-expired.png");
      this.imageList1.Images.SetKeyName(3, "rate-locked-extension-indicator.png");
      this.imageList1.Images.SetKeyName(4, "cancel.png");
      this.imageList1.Images.SetKeyName(5, "rate-unlocked.png");
      this.gpSetup.Controls.Add((Control) this.panel4);
      this.gpSetup.Dock = DockStyle.Fill;
      this.gpSetup.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.gpSetup.HeaderForeColor = SystemColors.ControlText;
      this.gpSetup.Location = new Point(0, 0);
      this.gpSetup.Name = "gpSetup";
      this.gpSetup.Size = new Size(986, 768);
      this.gpSetup.TabIndex = 3;
      this.gpSetup.Text = "Collateral Tracking";
      this.panel4.AutoScroll = true;
      this.panel4.Controls.Add((Control) this.panel1);
      this.panel4.Controls.Add((Control) this.tbcDocTracking);
      this.panel4.Controls.Add((Control) this.label27);
      this.panel4.Dock = DockStyle.Fill;
      this.panel4.Location = new Point(1, 26);
      this.panel4.Name = "panel4";
      this.panel4.Size = new Size(984, 741);
      this.panel4.TabIndex = 25;
      this.panel1.Controls.Add((Control) this.groupContainer3);
      this.panel1.Controls.Add((Control) this.gcTrackHistory);
      this.panel1.Location = new Point(3, 379);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(967, 347);
      this.panel1.TabIndex = 348;
      this.groupContainer3.AutoScroll = true;
      this.groupContainer3.Controls.Add((Control) this.richTextBox_Comments);
      this.groupContainer3.Controls.Add((Control) this.btnAddConmment);
      this.groupContainer3.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer3.Location = new Point(4, 199);
      this.groupContainer3.Name = "groupContainer3";
      this.groupContainer3.Size = new Size(963, 145);
      this.groupContainer3.TabIndex = 13;
      this.groupContainer3.Text = "Comments (All)";
      this.richTextBox_Comments.Location = new Point(4, 29);
      this.richTextBox_Comments.Name = "richTextBox_Comments";
      this.richTextBox_Comments.ReadOnly = true;
      this.richTextBox_Comments.Size = new Size(808, 112);
      this.richTextBox_Comments.TabIndex = 4;
      this.richTextBox_Comments.Text = "";
      this.richTextBox_Comments.MouseWheel += new MouseEventHandler(this.richTextBox_Comments_MouseWheel);
      this.btnAddConmment.Location = new Point(852, 1);
      this.btnAddConmment.Name = "btnAddConmment";
      this.btnAddConmment.Size = new Size(88, 23);
      this.btnAddConmment.TabIndex = 3;
      this.btnAddConmment.Text = "Add Comment";
      this.btnAddConmment.UseVisualStyleBackColor = true;
      this.btnAddConmment.Click += new EventHandler(this.btnAddConmment_Click);
      this.gcTrackHistory.AutoScroll = true;
      this.gcTrackHistory.Controls.Add((Control) this.gvTrackHistory);
      this.gcTrackHistory.HeaderForeColor = SystemColors.ControlText;
      this.gcTrackHistory.Location = new Point(3, 7);
      this.gcTrackHistory.Name = "gcTrackHistory";
      this.gcTrackHistory.Size = new Size(962, 181);
      this.gcTrackHistory.TabIndex = 12;
      this.gcTrackHistory.Text = "Tracking History (All)";
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "colAction";
      gvColumn1.Text = "Action";
      gvColumn1.Width = 140;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "colDate";
      gvColumn2.Text = "Date";
      gvColumn2.Width = 102;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "colBy";
      gvColumn3.Text = "By";
      gvColumn3.Width = 140;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "colDOT";
      gvColumn4.Text = "DOT";
      gvColumn4.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn4.Width = 60;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "colFTP";
      gvColumn5.Text = "FTP";
      gvColumn5.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn5.Width = 60;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "colEN";
      gvColumn6.Text = "NOTE";
      gvColumn6.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn6.Width = 60;
      gvColumn7.ImageIndex = -1;
      gvColumn7.Name = "colOrganization";
      gvColumn7.Text = "Organization";
      gvColumn7.Width = 140;
      gvColumn8.ImageIndex = -1;
      gvColumn8.Name = "colContact";
      gvColumn8.Text = "Contact";
      gvColumn8.Width = 140;
      gvColumn9.ImageIndex = -1;
      gvColumn9.Name = "colEmail";
      gvColumn9.Text = "Email";
      gvColumn9.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn9.Width = 60;
      gvColumn10.ImageIndex = -1;
      gvColumn10.Name = "colPhone";
      gvColumn10.Text = "Phone";
      gvColumn10.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn10.Width = 60;
      this.gvTrackHistory.Columns.AddRange(new GVColumn[10]
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
        gvColumn10
      });
      this.gvTrackHistory.Dock = DockStyle.Fill;
      this.gvTrackHistory.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvTrackHistory.Location = new Point(1, 26);
      this.gvTrackHistory.Name = "gvTrackHistory";
      this.gvTrackHistory.Size = new Size(960, 154);
      this.gvTrackHistory.TabIndex = 0;
      this.gvTrackHistory.ItemDoubleClick += new GVItemEventHandler(this.gvTrackHistory_ItemDoubleClick);
      this.tbcDocTracking.Controls.Add((Control) this.tbpDot);
      this.tbcDocTracking.Controls.Add((Control) this.tbpFTP);
      this.tbcDocTracking.Controls.Add((Control) this.tbpEN);
      this.tbcDocTracking.Location = new Point(3, 3);
      this.tbcDocTracking.Name = "tbcDocTracking";
      this.tbcDocTracking.SelectedIndex = 0;
      this.tbcDocTracking.Size = new Size(967, 367);
      this.tbcDocTracking.TabIndex = 347;
      this.tbpDot.Controls.Add((Control) this.pnlDotStatus);
      this.tbpDot.Location = new Point(4, 22);
      this.tbpDot.Name = "tbpDot";
      this.tbpDot.Padding = new Padding(3);
      this.tbpDot.Size = new Size(959, 341);
      this.tbpDot.TabIndex = 0;
      this.tbpDot.Text = "DOT/Mortgage";
      this.tbpDot.UseVisualStyleBackColor = true;
      this.pnlDotStatus.Location = new Point(1, 3);
      this.pnlDotStatus.Name = "pnlDotStatus";
      this.pnlDotStatus.Size = new Size(957, 338);
      this.pnlDotStatus.TabIndex = 13;
      this.tbpFTP.AutoScroll = true;
      this.tbpFTP.Controls.Add((Control) this.pnlFTPStatus);
      this.tbpFTP.Location = new Point(4, 22);
      this.tbpFTP.Name = "tbpFTP";
      this.tbpFTP.Padding = new Padding(3);
      this.tbpFTP.Size = new Size(959, 341);
      this.tbpFTP.TabIndex = 1;
      this.tbpFTP.Text = "Final Title Policy";
      this.tbpFTP.UseVisualStyleBackColor = true;
      this.pnlFTPStatus.Location = new Point(1, 3);
      this.pnlFTPStatus.Name = "pnlFTPStatus";
      this.pnlFTPStatus.Size = new Size(957, 335);
      this.pnlFTPStatus.TabIndex = 14;
      this.tbpEN.AutoScroll = true;
      this.tbpEN.Controls.Add((Control) this.pnlENStatus);
      this.tbpEN.Location = new Point(4, 22);
      this.tbpEN.Name = "tbpEN";
      this.tbpEN.Padding = new Padding(3);
      this.tbpEN.Size = new Size(959, 341);
      this.tbpEN.TabIndex = 2;
      this.tbpEN.Text = "Executed Note";
      this.tbpEN.UseVisualStyleBackColor = true;
      this.pnlENStatus.Location = new Point(1, 3);
      this.pnlENStatus.Name = "pnlENStatus";
      this.pnlENStatus.Size = new Size(957, 335);
      this.pnlENStatus.TabIndex = 14;
      this.label27.AutoSize = true;
      this.label27.Location = new Point(198, 253);
      this.label27.Name = "label27";
      this.label27.Size = new Size(0, 13);
      this.label27.TabIndex = 346;
      this.AutoScaleMode = AutoScaleMode.Inherit;
      this.Controls.Add((Control) this.gpSetup);
      this.Name = nameof (DocumentTrackingManagement);
      this.Size = new Size(986, 768);
      this.gpSetup.ResumeLayout(false);
      this.panel4.ResumeLayout(false);
      this.panel4.PerformLayout();
      this.panel1.ResumeLayout(false);
      this.groupContainer3.ResumeLayout(false);
      this.gcTrackHistory.ResumeLayout(false);
      this.tbcDocTracking.ResumeLayout(false);
      this.tbpDot.ResumeLayout(false);
      this.tbpFTP.ResumeLayout(false);
      this.tbpEN.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
