// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Log.eSignConsentAlertPanel
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.Log.DisclosureTracking2015;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Log
{
  public class eSignConsentAlertPanel : AlertPanelBase
  {
    public eSignConsentAlertPanel(PipelineInfo.Alert alert) => this.SetAlert(alert);

    public override bool AllowClearAlert => true;

    protected override void ConfigureTriggerFieldList()
    {
      this.gvFields.Columns.Clear();
      this.gvFields.Columns.Add("Field ID", 150);
      this.gvFields.Columns.Add("Description", 250);
      this.gvFields.Columns.Add("Borrower Name", 250);
      this.gvFields.Columns.Add("eConsent Status", 200);
    }

    protected override void PopulateTriggerFields()
    {
      string[] strArray1 = new string[12]
      {
        "3985",
        "3989",
        "3993",
        "3997",
        "4024",
        "4028",
        "4032",
        "4036",
        "4040",
        "4044",
        "4048",
        "4052"
      };
      string[] strArray2 = new string[12]
      {
        "3984",
        "3988",
        "3992",
        "3996",
        "4023",
        "4027",
        "4031",
        "4035",
        "4039",
        "4043",
        "4047",
        "4051"
      };
      int index = 0;
      foreach (BorrowerPair borrowerPair in Session.LoanData.GetBorrowerPairs())
      {
        this.gvFields.Items.Add(this.createGVItem(strArray2[index], Session.LoanData.GetFieldDefinition(strArray2[index]).Description, borrowerPair.Borrower.FirstName + " " + borrowerPair.Borrower.LastName, Session.LoanData.GetField(strArray2[index])));
        if ((borrowerPair.CoBorrower.FirstName + " " + borrowerPair.CoBorrower.LastName).Trim() != "")
          this.gvFields.Items.Add(this.createGVItem(strArray2[index + 1], Session.LoanData.GetFieldDefinition(strArray2[index + 1]).Description, borrowerPair.CoBorrower.FirstName + " " + borrowerPair.CoBorrower.LastName, Session.LoanData.GetField(strArray2[index + 1])));
        index += 2;
      }
      foreach (int linkedVestingIdx in Session.LoanData.GetNBOLinkedVestingIdxList())
      {
        string str1 = "NBOC";
        string str2 = linkedVestingIdx.ToString("00");
        string id = str1 + str2 + "17";
        string str3 = Session.LoanData.GetField(str1 + str2 + "01") + " " + Session.LoanData.GetField(str1 + str2 + "02");
        string str4 = Session.LoanData.GetField(str1 + str2 + "03") + " " + Session.LoanData.GetField(str1 + str2 + "04");
        this.gvFields.Items.Add(this.createGVItem(id, Session.LoanData.GetFieldDefinition("NBOC0017").Description, str3.Trim() + " " + str4.Trim(), Session.LoanData.GetField(id)));
      }
    }

    private GVItem createGVItem(string id, string def, string name, string status)
    {
      GVItem gvItem = new GVItem();
      gvItem.SubItems[0].Text = id;
      gvItem.SubItems[1].Text = def;
      gvItem.SubItems[2].Text = name;
      gvItem.SubItems[3].Text = status;
      if (status != "Accepted")
        gvItem.ForeColor = EncompassColors.Alert2;
      return gvItem;
    }

    private void requestConsent_Click(object sender, EventArgs e)
    {
      Session.Application.GetService<IEFolder>().SendConsentRequest(Session.LoanDataMgr);
    }

    private void updateConsent_Click(object sender, EventArgs e)
    {
      int num = (int) new eSignConsent().ShowDialog();
      this.gvFields.Items.Clear();
      this.RefreshAlert();
    }

    protected override void OnLoad(EventArgs e)
    {
      base.OnLoad(e);
      Button c1 = new Button();
      Button c2 = new Button();
      c2.Text = "View eConsent";
      c2.AutoSize = true;
      c2.Margin = new Padding(3, 0, 2, 0);
      c2.Click += new EventHandler(this.updateConsent_Click);
      this.AddTriggerFieldsControl((Control) c2);
      c1.Text = "Request eConsent";
      c1.AutoSize = true;
      c1.Margin = new Padding(3, 0, 2, 0);
      c1.Click += new EventHandler(this.requestConsent_Click);
      this.AddTriggerFieldsControl((Control) c1);
      int width1 = c1.Width;
      int width2 = c2.Width;
      c1.AutoSize = false;
      c2.AutoSize = false;
      c2.Height = c1.Height = 22;
      c1.Width = width1;
      c1.Enabled = Session.LoanDataMgr.LoanData.LinkSyncType != LinkSyncType.ConstructionLinked;
      c2.Width = width2;
    }

    private void InitializeComponent()
    {
      this.pnlAltDate.SuspendLayout();
      this.grpDetails.SuspendLayout();
      this.SuspendLayout();
      this.gvFields.Size = new Size(1358, 531);
      this.pnlAltDate.Location = new Point(218, 96);
      this.pnlAltDate.Size = new Size(199, 21);
      this.grpDetails.Size = new Size(1360, 126);
      this.txtAlertDate.Location = new Point(71, 96);
      this.txtAltDate.Location = new Point(62, 0);
      this.txtMessage.Location = new Point(71, 58);
      this.txtMessage.Size = new Size(1280, 36);
      this.txtAlertName.Location = new Point(71, 36);
      this.txtAlertName.Size = new Size(1280, 20);
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.Name = nameof (eSignConsentAlertPanel);
      this.Size = new Size(1360, 684);
      this.pnlAltDate.ResumeLayout(false);
      this.pnlAltDate.PerformLayout();
      this.grpDetails.ResumeLayout(false);
      this.grpDetails.PerformLayout();
      this.ResumeLayout(false);
    }

    protected override void OnClearAlert(EventArgs e)
    {
      RegulationAlert definition = (RegulationAlert) this.AlertConfig.Definition;
      Session.LoanData.SetCurrentField("4072", "Y");
      Session.Application.GetService<ILoanEditor>().RefreshLogPanel();
      base.OnClearAlert(e);
    }
  }
}
