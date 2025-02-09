// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Verification.TAX4506TPanel
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.InputEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

#nullable disable
namespace EllieMae.EMLite.Verification
{
  public class TAX4506TPanel : VOLPanel
  {
    private const string TAXTYPE = "IR";
    private GVColumn versionCol = new GVColumn();
    private GVColumn requestCol = new GVColumn();
    private GVColumn typeCol = new GVColumn();
    private GVColumn yearCol = new GVColumn();
    private GVColumn optionCol = new GVColumn();
    private string originalTitle = "Request for Transcript of Tax";
    private string helpTopic;
    private Button addFromTemplate = new Button();

    protected override void InitPanel()
    {
      this.className = nameof (TAX4506TPanel);
      this.toolTip.SetToolTip((Control) this.btnImport4506, "Copy data to this form from the Request for Transcript of Tax (Classic) form.");
      this.toolTip.SetToolTip((Control) this.btnExport4506, "Copy data from this form to the Request for Transcript of Tax (Classic) form.");
      this.addFromTemplate.Text = "Add From Template";
      this.addFromTemplate.Size = new Size(120, 22);
      this.addFromTemplate.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.addFromTemplate.BackColor = SystemColors.Control;
      this.addFromTemplate.TabStop = false;
      this.addFromTemplate.UseVisualStyleBackColor = true;
      this.addFromTemplate.TextAlign = ContentAlignment.MiddleCenter;
      this.addFromTemplate.Click += new EventHandler(this.btnAddFromTemplate_Click);
      this.addFromTemplate.Location = new Point(100, 3);
      this.PaintActionPanel();
      this.versionCol.Text = "Request Form and Version";
      this.versionCol.Width = 200;
      this.requestCol.Text = "Request For";
      this.requestCol.Width = 140;
      this.typeCol.Text = "Type";
      this.typeCol.Width = 200;
      this.yearCol.Text = "Year";
      this.yearCol.Width = 140;
      this.optionCol.Text = "Transcript Options";
      this.optionCol.Width = 300;
      this.gridList.Columns.AddRange(new GVColumn[5]
      {
        this.versionCol,
        this.requestCol,
        this.typeCol,
        this.yearCol,
        this.optionCol
      });
    }

    private void btnAddFromTemplate_Click(object sender, EventArgs e)
    {
      using (SelectTaxTranscriptTemplate transcriptTemplate = new SelectTaxTranscriptTemplate())
      {
        if (transcriptTemplate.ShowDialog((IWin32Window) Session.MainForm) != DialogResult.OK)
          return;
        this.popoulateLoanFromTemplate(transcriptTemplate.SelectedTemplate);
      }
    }

    private void popoulateLoanFromTemplate(IRS4506TFields template)
    {
      this.loan.setTaxTranscriptTemplate(template);
      this.RefreshListView((object) null, (EventArgs) null);
      this.gridList.SelectedItems.Clear();
      this.gridList.Items[this.gridList.Items.Count - 1].Selected = true;
      this.setButtonAccessForForm8821(this.gridList.Items.Count);
    }

    public TAX4506TPanel(IMainScreen mainScreen, IWorkArea workArea)
      : base("Request for Transcript of Tax", mainScreen, workArea)
    {
      this.AddHelpIconToTitle("4506_4506T_VersionInfo");
    }

    private void PaintActionPanel()
    {
      this.actionPanel.Controls.Clear();
      this.actionPanel.Controls.AddRange(new Control[11]
      {
        (Control) this.btnNew,
        (Control) this.btnDelete,
        (Control) this.btnUp,
        (Control) this.btnDown,
        (Control) this.verSeparator,
        (Control) this.btnImport4506,
        (Control) this.btnExport4506,
        (Control) this.addFromTemplate,
        (Control) this.btnAddDoc,
        (Control) this.btnOrderIRS,
        (Control) this.btnCheckIRS
      });
      this.actionPanel.SuspendLayout();
      int x = 1;
      foreach (Control control in (ArrangedElementCollection) this.actionPanel.Controls)
      {
        control.Location = new Point(x, control.Location.Y);
        x += control.Size.Width + 5;
      }
      this.actionPanel.Size = new Size(x + 2, this.actionPanel.Size.Height);
      this.actionPanel.ResumeLayout(false);
    }

    private void setButtonAccess(Button button, string prefix)
    {
      button.Enabled = true;
      switch (Session.LoanDataMgr.GetFieldAccessRights("Button_" + prefix + button.Text.Replace('&', ' ').Trim()))
      {
        case BizRule.FieldAccessRight.Hide:
          button.Visible = false;
          break;
        case BizRule.FieldAccessRight.ViewOnly:
          button.Enabled = false;
          break;
      }
    }

    private void setButtonAccessForForm8821(int index)
    {
      if (this.loan.GetField("IR" + index.ToString("00") + "93") == "8821")
      {
        this.btnExport4506.Enabled = this.btnImport4506.Enabled = this.btnCheckIRS.Enabled = this.btnOrderIRS.Enabled = this.btnAddDoc.Enabled = false;
      }
      else
      {
        this.btnImport4506.Enabled = this.checkIRS4506Class();
        this.btnExport4506.Enabled = true;
        this.btnCheckIRS.Enabled = this.btnOrderIRS.Enabled = this.btnAddDoc.Enabled = true;
      }
    }

    protected override GVItem addVerifToListView(int newIndex)
    {
      string sInd = newIndex.ToString("00");
      string field = this.loan.GetField("IR" + sInd + "93");
      GVItem listView = new GVItem(this.loan.GetField("IR" + sInd + "93"));
      listView.SubItems.Add((object) this.loan.GetField("IR" + sInd + "01"));
      listView.SubItems.Add(field == "8821" ? (object) this.retrieveFormNumbers(sInd) : (object) this.loan.GetField("IR" + sInd + "24"));
      listView.SubItems.Add((object) this.retrieveTaxYears(newIndex.ToString("00")));
      listView.SubItems.Add(field == "8821" ? (object) "" : (object) this.retrieveTranscriptOptions(newIndex.ToString("00")));
      this.gridList.Items.Add(listView);
      return listView;
    }

    protected override void refreshList()
    {
      this.gridList.Items.Clear();
      if (this.loan == null)
        return;
      int numberOfTaX4506Ts = this.loan.GetNumberOfTAX4506Ts(false);
      this.gridList.BeginUpdate();
      for (int newIndex = 1; newIndex <= numberOfTaX4506Ts; ++newIndex)
        this.addVerifToListView(newIndex);
      this.gridList.EndUpdate();
    }

    public override void RefreshListView(object sender, EventArgs e)
    {
      this.refreshList();
      if (this.gridList.Items.Count != 0)
        this.gridList.Items[0].Selected = true;
      this.editBtn_Click((object) null, (EventArgs) null);
      this.btnAddDoc.Enabled = this.btnDelete.Enabled = this.btnExport4506.Enabled = this.gridList.SelectedItems.Count == 1;
      this.setButtonAccess(this.btnCheckIRS, "TAX4506");
      this.setButtonAccess(this.btnOrderIRS, "TAX4506");
      if (this.btnCheckIRS.Visible && this.btnCheckIRS.Enabled)
        this.btnCheckIRS.Enabled = this.gridList.SelectedItems.Count == 1;
      if (this.btnOrderIRS.Visible && this.btnOrderIRS.Enabled)
        this.btnOrderIRS.Enabled = this.gridList.SelectedItems.Count == 1;
      this.btnUp.Enabled = this.btnDown.Enabled = this.gridList.SelectedItems.Count == 1 && this.gridList.Items.Count > 1;
      if (this.gridList.SelectedItems.Count != 0)
        this.setButtonAccessForForm8821(this.gridList.SelectedItems[0].Index + 1);
      else
        this.btnImport4506.Enabled = this.checkIRS4506Class();
    }

    protected override VerificationBase NewVerificationScreen()
    {
      return (VerificationBase) new TAX4506T((PanelBase) this, this.mainScreen, this.workArea);
    }

    protected override int RemoveVerificationAt(int i) => this.loan.RemoveTAX4506TAt(i, false);

    protected override int NewVerification() => this.loan.NewTAX4506T(false);

    public override void newBtn_Click(object sender, EventArgs e)
    {
      base.newBtn_Click(sender, e);
      if (this.gridList.SelectedItems.Count == 0)
        return;
      string sInd = (this.gridList.SelectedItems[0].Index + 1).ToString("00");
      this.gridList.SelectedItems[0].SubItems[0].Text = this.loan.GetField("IR" + sInd + "93");
      this.gridList.SelectedItems[0].SubItems[2].Text = this.loan.GetField("IR" + sInd + "24");
      this.gridList.SelectedItems[0].SubItems[3].Text = this.retrieveTaxYears(sInd);
      this.gridList.SelectedItems[0].SubItems[4].Text = this.retrieveTranscriptOptions(sInd);
      this.setButtonAccess(this.btnCheckIRS, "TAX4506");
      this.setButtonAccess(this.btnOrderIRS, "TAX4506");
    }

    protected override void upBtn_Click(object sender, EventArgs e)
    {
      if (this.gridList.SelectedItems.Count != 0)
      {
        int index = this.gridList.SelectedItems[0].Index;
        if (index == 0 || !Session.LoanDataMgr.LockLoanWithExclusiveA())
          return;
        InputHandlerBase inputHandler = (InputHandlerBase) this.ver.GetInputHandler();
        this.gridList.SelectedItems.Clear();
        this.UpVerification(index);
        this.refreshList();
        this.gridList.Items[index - 1].Selected = true;
        inputHandler?.RefreshContents();
        this.editBtn_Click((object) null, (EventArgs) null);
      }
      else
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You have to select a record first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    protected override void UpVerification(int i) => this.loan.UpTAX4506T(i, false);

    protected override void DownVerfication(int i) => this.loan.DownTAX4506T(i, false);

    protected override void downBtn_Click(object sender, EventArgs e)
    {
      if (this.gridList.SelectedItems.Count != 0)
      {
        int index = this.gridList.SelectedItems[0].Index;
        if (index >= this.gridList.Items.Count - 1 || !Session.LoanDataMgr.LockLoanWithExclusiveA())
          return;
        InputHandlerBase inputHandler = (InputHandlerBase) this.ver.GetInputHandler();
        this.gridList.SelectedItems.Clear();
        this.DownVerfication(index);
        this.refreshList();
        this.gridList.Items[index + 1].Selected = true;
        inputHandler?.RefreshContents();
        this.editBtn_Click((object) null, (EventArgs) null);
      }
      else
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You have to select a record first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    protected override void hookupEventHandler(IInputHandler inputHandler)
    {
      if (inputHandler == null || !(inputHandler is TAX4506TInputHandler))
        return;
      TAX4506TInputHandler x4506TinputHandler = (TAX4506TInputHandler) inputHandler;
      x4506TinputHandler.VerifSummaryChanged += new VerifSummaryChangedEventHandler(this.summaryInfoChangedHandler);
      if (x4506TinputHandler == null)
        return;
      this.setButtonAccessForForm8821(Utils.ParseInt((object) x4506TinputHandler.CurrentIndex));
    }

    private void summaryInfoChangedHandler(VerifSummaryChangeInfo info)
    {
      string sInd = (this.gridList.SelectedItems[0].Index + 1).ToString("00");
      string itemName = info.ItemName;
      // ISSUE: reference to a compiler-generated method
      switch (\u003CPrivateImplementationDetails\u003E.ComputeStringHash(itemName))
      {
        case 569251334:
          if (!(itemName == "IR0093"))
            return;
          this.gridList.SelectedItems[0].SubItems[0].Text = (string) info.ItemValue;
          this.setButtonAccessForForm8821(this.gridList.SelectedItems[0].Index + 1);
          return;
        case 618745548:
          if (!(itemName == "IR0046"))
            return;
          goto label_39;
        case 635523167:
          if (!(itemName == "IR0047"))
            return;
          goto label_39;
        case 719411262:
          if (!(itemName == "IR0048"))
            return;
          goto label_39;
        case 2089228336:
          if (!(itemName == "IR00A074"))
            return;
          goto label_40;
        case 2122783574:
          if (!(itemName == "IR00A076"))
            return;
          goto label_41;
        case 2156338812:
          if (!(itemName == "IR00A070"))
            return;
          goto label_40;
        case 2156485907:
          if (!(itemName == "IR00A066"))
            return;
          goto label_40;
        case 2189894050:
          if (!(itemName == "IR00A072"))
            return;
          goto label_41;
        case 2391372573:
          if (!(itemName == "IR00A068"))
            return;
          goto label_41;
        case 2731739804:
          if (!(itemName == "IR0024"))
            return;
          this.gridList.SelectedItems[0].SubItems[2].Text = (string) info.ItemValue;
          return;
        case 2748517423:
          if (!(itemName == "IR0025"))
            return;
          break;
        case 2765295042:
          if (!(itemName == "IR0026"))
            return;
          break;
        case 2765442137:
          if (!(itemName == "IR0030"))
            return;
          break;
        case 2815627899:
          if (!(itemName == "IR0029"))
            return;
          break;
        case 2833538351:
          if (!(itemName == "IR0050"))
            return;
          goto label_39;
        case 2883032565:
          if (!(itemName == "IR0001"))
            return;
          this.gridList.SelectedItems[0].SubItems[1].Text = (string) info.ItemValue;
          return;
        default:
          return;
      }
      this.gridList.SelectedItems[0].SubItems[3].Text = this.retrieveTaxYears(sInd);
      return;
label_39:
      this.gridList.SelectedItems[0].SubItems[4].Text = this.retrieveTranscriptOptions(sInd);
      return;
label_40:
      this.gridList.SelectedItems[0].SubItems[2].Text = this.retrieveFormNumbers(sInd);
      this.gridList.SelectedItems[0].SubItems[4].Text = "";
      return;
label_41:
      this.gridList.SelectedItems[0].SubItems[3].Text = this.retrieveTaxYears(sInd);
      this.gridList.SelectedItems[0].SubItems[4].Text = "";
    }

    private string retrieveTaxYears(string sInd)
    {
      string field1 = this.loan.GetField("IR" + sInd + "93");
      string str1 = string.Empty;
      if (field1 == "8821")
      {
        string[] strArray = new string[3]
        {
          "A068",
          "A072",
          "A076"
        };
        foreach (string str2 in strArray)
        {
          string field2 = this.loan.GetField("IR" + sInd + str2);
          if (!string.IsNullOrEmpty(field2))
            str1 = str1 + (!string.IsNullOrEmpty(str1) ? "," : "") + field2;
        }
      }
      else
      {
        DateTime minValue = DateTime.MinValue;
        DateTime date = Utils.ParseDate((object) this.loan.GetField("IR" + sInd + "25"));
        if (date != DateTime.MinValue)
          str1 = date.Year.ToString();
        date = Utils.ParseDate((object) this.loan.GetField("IR" + sInd + "26"));
        if (date != DateTime.MinValue)
          str1 = str1 + (str1 != string.Empty ? "," : "") + date.Year.ToString();
        date = Utils.ParseDate((object) this.loan.GetField("IR" + sInd + "29"));
        if (date != DateTime.MinValue)
          str1 = str1 + (str1 != string.Empty ? "," : "") + date.Year.ToString();
        date = Utils.ParseDate((object) this.loan.GetField("IR" + sInd + "30"));
        if (date != DateTime.MinValue)
          str1 = str1 + (str1 != string.Empty ? "," : "") + date.Year.ToString();
      }
      return str1;
    }

    private string retrieveFormNumbers(string sInd)
    {
      string str1 = string.Empty;
      string[] strArray = new string[3]
      {
        "A066",
        "A070",
        "A074"
      };
      foreach (string str2 in strArray)
      {
        string field = this.loan.GetField("IR" + sInd + str2);
        if (!string.IsNullOrEmpty(field))
          str1 = str1 + (!string.IsNullOrEmpty(str1) ? "," : "") + field;
      }
      return str1;
    }

    private string retrieveTranscriptOptions(string sInd)
    {
      string str = string.Empty;
      if (this.loan.GetField("IR" + sInd + "46") == "Y")
        str = "Return Transcript";
      if (this.loan.GetField("IR" + sInd + "47") == "Y")
        str = str + (str != string.Empty ? ", " : "") + "Account Transcript";
      if (this.loan.GetField("IR" + sInd + "48") == "Y")
        str = str + (str != string.Empty ? ", " : "") + "Record of Account";
      if (this.loan.GetField("IR" + sInd + "50") == "Y")
        str = str + (str != string.Empty ? ", " : "") + "W-2";
      return str;
    }

    public new void SetHelpTopic(string helpTopic)
    {
      if (string.IsNullOrEmpty(JedHelp.GetMapId(helpTopic)))
        return;
      this.helpTopic = helpTopic;
    }

    public override string GetHelpTargetName()
    {
      return this.helpTopic == null ? this.originalTitle : this.helpTopic;
    }

    protected override void btnExport4506_Click(object sender, EventArgs e)
    {
      if (this.gridList.SelectedItems.Count == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Please select a record to export new data to classic form.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        int recordIndex = this.gridList.SelectedItems[0].Index + 1;
        if (this.loan.GetSimpleField("IR" + recordIndex.ToString("00") + "93") == "8821")
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this, "The Request for Transcript of Tax (Classic) form doesn't have the form version 8821.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else if (Utils.Dialog((IWin32Window) this, "Are you sure you want to copy data from selected record to the Request for Transcript of Tax (Classic) form?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) != DialogResult.Yes)
        {
          this.gridList.Items[recordIndex - 1].Selected = true;
        }
        else
        {
          this.copyToClassic(recordIndex);
          this.refreshList();
          this.gridList.Items[recordIndex - 1].Selected = true;
          this.btnImport4506.Enabled = this.checkIRS4506Class();
        }
      }
    }

    protected override void btnImport4506_Click(object sender, EventArgs e)
    {
      int recordIndex = this.gridList.SelectedItems.Count == 0 ? -1 : this.gridList.SelectedItems[0].Index + 1;
      bool flag = recordIndex == -1;
      if (!flag && this.loan.GetSimpleField("IR" + recordIndex.ToString("00") + "93") == "8821")
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The Request for Transcript of Tax (Classic) form doesn't have the form version 8821.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        if (Utils.Dialog((IWin32Window) this, "Are you sure you want to copy data to " + (this.gridList.SelectedItems.Count == 0 ? "a new " : "selected") + " record from the Request for Transcript of Tax (Classic) form?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) != DialogResult.Yes)
          return;
        int newIndex = this.copyFromClassic(recordIndex);
        if (flag)
        {
          this.addVerifToListView(newIndex).Selected = true;
        }
        else
        {
          this.refreshList();
          this.gridList.Items[newIndex - 1].Selected = true;
        }
      }
    }

    private void copyToClassic(int recordIndex)
    {
      bool flag = false;
      int numberOfTaX4506Ts = this.loan.GetNumberOfTAX4506Ts(false);
      if (recordIndex > numberOfTaX4506Ts)
        flag = true;
      this.loan.SetField("IRS4506.X98", flag ? "" : recordIndex.ToString("N2"));
      string str = "IR" + recordIndex.ToString("00");
      for (int index = 1; index <= 13; ++index)
        this.loan.SetField("IRS4506.X" + (object) index, flag ? "" : this.loan.GetField(str + index.ToString("00")));
      for (int index = 24; index <= 30; ++index)
        this.loan.SetField("IRS4506.X" + (object) index, flag ? "" : this.loan.GetField(str + index.ToString("00")));
      for (int index = 35; index <= 50; ++index)
        this.loan.SetField("IRS4506.X" + (object) index, flag ? "" : this.loan.GetField(str + index.ToString("00")));
      for (int index = 57; index <= 60; ++index)
        this.loan.SetField("IRS4506.X" + (object) index, flag ? "" : this.loan.GetField(str + index.ToString("00")));
      for (int index = 70; index <= 89; ++index)
        this.loan.SetField("IRS4506.X" + (object) index, flag ? "" : this.loan.GetField(str + index.ToString("00")));
      for (int index = 90; index <= 91; ++index)
        this.loan.SetField("IRS4506.X" + (object) index, flag ? "" : this.loan.GetField(str + index.ToString("00")));
      this.loan.SetField("IRS4506.X63", flag ? "" : this.loan.GetField(str + "65"));
      this.loan.SetField("IRS4506.X64", flag ? "" : this.loan.GetField(str + "66"));
      this.loan.SetField("IRS4506.X68", flag ? "" : this.loan.GetField(str + "68"));
      this.loan.SetField("IRS4506.X69", flag ? "" : this.loan.GetField(str + "69"));
      this.loan.SetField("IRS4506.X93", flag ? "" : this.loan.GetField(str + "93"));
    }

    private int copyFromClassic(int recordIndex)
    {
      bool flag = false;
      if (recordIndex == -1)
        recordIndex = this.loan.NewTAX4506T(false);
      int numberOfTaX4506Ts = this.loan.GetNumberOfTAX4506Ts(false);
      if (recordIndex > numberOfTaX4506Ts)
        flag = true;
      this.loan.SetField("IRS4506.X98", flag ? "" : recordIndex.ToString("N2"));
      string str = "IR" + recordIndex.ToString("00");
      for (int index = 1; index <= 13; ++index)
        this.loan.SetField(str + index.ToString("00"), flag ? "" : this.loan.GetField("IRS4506.X" + (object) index));
      for (int index = 24; index <= 30; ++index)
        this.loan.SetField(str + index.ToString("00"), flag ? "" : this.loan.GetField("IRS4506.X" + (object) index));
      for (int index = 35; index <= 50; ++index)
        this.loan.SetField(str + index.ToString("00"), flag ? "" : this.loan.GetField("IRS4506.X" + (object) index));
      for (int index = 57; index <= 60; ++index)
        this.loan.SetField(str + index.ToString("00"), flag ? "" : this.loan.GetField("IRS4506.X" + (object) index));
      for (int index = 70; index <= 89; ++index)
        this.loan.SetField(str + index.ToString("00"), flag ? "" : this.loan.GetField("IRS4506.X" + (object) index));
      for (int index = 90; index <= 91; ++index)
        this.loan.SetField(str + index.ToString("00"), flag ? "" : this.loan.GetField("IRS4506.X" + (object) index));
      this.loan.SetField(str + "65", flag ? "" : this.loan.GetField("IRS4506.X63"));
      this.loan.SetField(str + "66", flag ? "" : this.loan.GetField("IRS4506.X64"));
      this.loan.SetField(str + "68", flag ? "" : this.loan.GetField("IRS4506.X68"));
      this.loan.SetField(str + "69", flag ? "" : this.loan.GetField("IRS4506.X69"));
      this.loan.SetField(str + "93", flag ? "" : this.loan.GetField("IRS4506.X93"));
      return recordIndex;
    }

    private bool checkIRS4506Class()
    {
      string empty = string.Empty;
      for (int index = 1; index <= 13; ++index)
      {
        string field = this.loan.GetField("IRS4506.X" + (object) index);
        if (field != string.Empty && field != "//")
          return true;
      }
      for (int index = 24; index <= 30; ++index)
      {
        string field = this.loan.GetField("IRS4506.X" + (object) index);
        if (field != string.Empty && field != "//")
          return true;
      }
      for (int index = 35; index <= 50; ++index)
      {
        string field = this.loan.GetField("IRS4506.X" + (object) index);
        if (field != string.Empty && field != "//" && (index < 46 || index > 50 || !(field == "N")))
          return true;
      }
      for (int index = 57; index <= 60; ++index)
      {
        string field = this.loan.GetField("IRS4506.X" + (object) index);
        if (field != string.Empty && field != "//" && field != "N")
          return true;
      }
      return false;
    }
  }
}
