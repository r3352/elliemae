// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ThinThick.ThinThickEditorDialog
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.DocEngine;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.InputEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using EllieMae.EMLite.Verification;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

#nullable disable
namespace EllieMae.EMLite.ThinThick
{
  public class ThinThickEditorDialog : 
    Form,
    IOnlineHelpTarget,
    IHelp,
    ILoanEditor,
    IWorkArea,
    IWin32Window
  {
    private const string className = "ThinThickEditorDialog";
    private static string sw = Tracing.SwInputEngine;
    private static Color highlightColor = Color.FromArgb((int) byte.MaxValue, 237, 146);
    private Sessions.Session session;
    private string loanGuid;
    private AuditMessage auditMessage;
    private LoanDataMgr loanDataMgr;
    private InputFormInfo currForm;
    private LoanScreen freeScreen;
    private Routine fieldChangeRoutine;
    private List<string> fieldChangeFields;
    private SearchFields searchFields;
    private VORPanel vorPan;
    private VOEPanel voePan;
    private VOLPanel volPan;
    private VODPanel vodPan;
    private VOGGPanel voggPan;
    private VOOIPanel vooiPan;
    private VOOLPanel voolPan;
    private VOOAPanel vooaPan;
    private VOALPanel voalPan;
    private VOMPanel vomPan;
    private TAX4506TPanel tax4506TPan;
    private TAX4506Panel tax4506Pan;
    private Control topControl;
    private IContainer components;
    private BorderPanel pnlBottom;
    private Button btnCancel;
    private Button btnSave;
    private GroupContainer gcForms;
    private Panel pnlWeb;
    private EMFormMenu emFormMenuBox;
    private EMHelpLink emHelpLink;
    private CheckBox chkAlphaOrder;
    private TabControl tcMain;
    private TabPage tabDataResults;
    private TabPage tabComplianceResults;
    private TabPage tabLoanFormResults;
    private GridView gvDataResults;
    private GridView gvComplianceResults;
    private GridView gvLoanFormResults;
    private BorderPanel pnlTop;
    private BorderPanel pnlAlphaOrder;
    private CollapsibleSplitter csAudit;
    private BorderPanel pnlMiddle;
    private CollapsibleSplitter csForms;
    private TabControl tcFormsTools;
    private TabPage tabForms;
    private TabPage tabTools;
    private EMFormMenu emToolMenuBox;
    private CheckBox chkAllForms;

    public ThinThickEditorDialog(
      Sessions.Session session,
      string loanGuid,
      AuditMessage auditMessage)
    {
      this.InitializeComponent();
      this.setWindowSize();
      this.session = session;
      this.loanGuid = loanGuid;
      this.auditMessage = auditMessage;
      this.fieldChangeRoutine = new Routine(this.onFieldChanged);
      this.fieldChangeFields = new List<string>();
      this.emHelpLink.AssignSession(this.session);
      this.gvDataResults.Sort(0, SortOrder.Ascending);
      this.gvComplianceResults.Sort(0, SortOrder.Ascending);
      this.gvLoanFormResults.Sort(0, SortOrder.Ascending);
    }

    public new DialogResult ShowDialog() => this.ShowDialog((IWin32Window) null);

    public new DialogResult ShowDialog(IWin32Window owner)
    {
      try
      {
        if (this.session.LoanDataMgr != null && this.session.LoanDataMgr.LoanData != null && this.session.LoanDataMgr.LoanData.GUID.Equals(this.loanGuid, StringComparison.CurrentCultureIgnoreCase))
        {
          this.loanDataMgr = this.session.LoanDataMgr;
          this.loanDataMgr.Refresh(false);
        }
        else
        {
          this.loanDataMgr = LoanDataMgr.OpenLoan(this.session.SessionObjects, this.loanGuid, false);
          this.loanDataMgr.Lock(LoanInfo.LockReason.OpenForWork, LockInfo.ExclusiveLock.Exclusive);
        }
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "Unable to open the loan file for editing.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return DialogResult.Cancel;
      }
      this.loadFormsList();
      this.loadDataAuditResults(this.auditMessage);
      this.loadComplianceReviewResults(this.auditMessage);
      this.loadLoanFormPrintRuleResults(this.auditMessage);
      this.searchFields = new SearchFields(this.loanDataMgr.InputFormSettings, this.loanDataMgr.LoanData);
      return base.ShowDialog(owner);
    }

    private void setWindowSize()
    {
      if (Form.ActiveForm != null)
      {
        Form form = Form.ActiveForm;
        while (form.Owner != null)
          form = form.Owner;
        this.Width = Convert.ToInt32((double) form.Width * 0.95);
        this.Height = Convert.ToInt32((double) form.Height * 0.95);
      }
      else
      {
        Rectangle workingArea = Screen.PrimaryScreen.WorkingArea;
        this.Width = Convert.ToInt32((double) workingArea.Width * 0.95);
        workingArea = Screen.PrimaryScreen.WorkingArea;
        this.Height = Convert.ToInt32((double) workingArea.Height * 0.95);
      }
    }

    private void loadFormsList()
    {
      InputFormInfo[] formList1 = this.loanDataMgr.InputFormSettings.GetFormList("All");
      List<string> stringList1 = new List<string>();
      foreach (InputFormInfo inputFormInfo in formList1)
        stringList1.Add(inputFormInfo.Name);
      InputFormInfo[] formList2 = this.loanDataMgr.InputFormSettings.GetFormList("All");
      string field = this.loanDataMgr.LoanData.GetField("3969");
      bool flag = this.loanDataMgr.LoanData.GetField("1825") == "2020";
      string[] source1 = new string[5]
      {
        "GFE - Itemization",
        "HUD-1 Page 1",
        "HUD-1 Page 2",
        "REGZ - TIL",
        "Closing RegZ"
      };
      string[] source2 = new string[7]
      {
        "2010 GFE",
        "2010 Itemization",
        "2010 HUD-1 Page 1",
        "2010 HUD-1 Page 2",
        "2010 HUD-1 Page 3",
        "REGZ - TIL",
        "Closing RegZ"
      };
      string[] source3 = new string[11]
      {
        "2015 Itemization",
        "RegZ - LE",
        "Loan Estimate Page 1",
        "Loan Estimate Page 2",
        "Loan Estimate Page 3",
        "RegZ - CD",
        "Closing Disclosure Page 1",
        "Closing Disclosure Page 2",
        "Closing Disclosure Page 3",
        "Closing Disclosure Page 4",
        "Closing Disclosure Page 5"
      };
      string[] strArray1 = new string[5]
      {
        "1003 Page 1",
        "1003 Page 2",
        "1003 Page 3",
        "1003 Page 4",
        "FNMA Streamlined 1003"
      };
      string[] strArray2 = new string[12]
      {
        "1003 URLA Part 1",
        "1003 URLA Part 2",
        "1003 URLA Part 3",
        "1003 URLA Part 4",
        "1003 URLA - Lender",
        "1003 URLA Continuation",
        "Verification of Other Income",
        "Verification of Gifts and Grants",
        "Verification of Other Liability",
        "Verification of Other Assets",
        "Verification of Additional Loans",
        "Fannie Mae Additional Data"
      };
      List<string> stringList2 = new List<string>();
      foreach (InputFormInfo inputFormInfo in formList2)
      {
        if (!(inputFormInfo == (InputFormInfo) null) && (!(inputFormInfo.FormID == "ULDD") || Session.MainScreen.IsClientEnabledToExportFNMFRE) && !InputFormInfo.IsChildForm(inputFormInfo.FormID))
        {
          string name = inputFormInfo.Name;
          if (name != null)
          {
            if (inputFormInfo.Name.ToLower() == "gfe - itemization" && field == "RESPA 2010 GFE and HUD-1" && !stringList2.Contains("2010 GFE") && stringList1.Contains("REGZGFEHUD"))
              stringList2.Add("2010 GFE");
            if (name == "-" || !stringList2.Contains(name))
              stringList2.Add(name);
            if (inputFormInfo.Name.ToLower() == "hud-1 page 2" && field == "RESPA 2010 GFE and HUD-1" && !stringList2.Contains("2010 HUD-1 Page 3") && stringList1.Contains("HUD1PG3_2010"))
              stringList2.Add("2010 HUD-1 Page 3");
          }
        }
      }
      string empty = string.Empty;
      for (int index = 0; index < stringList2.Count; ++index)
      {
        string str = "";
        if (Utils.CheckIf2015RespaTila(field))
        {
          if (stringList2[index].ToLower() == "2010 itemization" || stringList2[index].ToLower() == "gfe - itemization")
            str = "2015 Itemization";
          else if (stringList2[index].ToLower() == "regz - til")
            str = "RegZ - LE";
          else if (stringList2[index].ToLower() == "closing regz")
            str = "RegZ - CD";
        }
        else
        {
          switch (field)
          {
            case "RESPA 2010 GFE and HUD-1":
              if (stringList2[index].ToLower() == "2015 itemization" || stringList2[index].ToLower() == "old gfe and hud-1")
              {
                str = "2010 Itemization";
                break;
              }
              if (stringList2[index].ToLower() == "regZ - le")
              {
                str = "REGZ - TIL";
                break;
              }
              if (stringList2[index].ToLower() == "regz - cd")
              {
                str = "Closing RegZ";
                break;
              }
              break;
            case "Old GFE and HUD-1":
              if (stringList2[index].ToLower() == "2015 itemization" || stringList2[index].ToLower() == "2010 itemization")
              {
                str = "GFE - Itemization";
                break;
              }
              if (stringList2[index].ToLower() == "regZ - le")
              {
                str = "REGZ - TIL";
                break;
              }
              if (stringList2[index].ToLower() == "regz - cd")
              {
                str = "Closing RegZ";
                break;
              }
              break;
            case "203k Max Mortgage WS":
              if (!stringList2.Contains("FHA Management"))
              {
                str = "FHA Management";
                break;
              }
              break;
          }
        }
        if (str != "" && !stringList2.Contains(str) && stringList1.Contains(str))
          stringList2[index] = str;
      }
      if (stringList2.Contains("203k Max Mortgage WS"))
        stringList2.Remove("203k Max Mortgage WS");
      switch (field)
      {
        case "Old GFE and HUD-1":
          foreach (string str in source2)
          {
            if (stringList2.Contains(str) && !((IEnumerable<string>) source1).Contains<string>(str))
              stringList2.Remove(str);
          }
          foreach (string str in source3)
          {
            if (stringList2.Contains(str) && !((IEnumerable<string>) source1).Contains<string>(str))
              stringList2.Remove(str);
          }
          break;
        case "RESPA 2010 GFE and HUD-1":
          foreach (string str in source1)
          {
            if (stringList2.Contains(str) && !((IEnumerable<string>) source2).Contains<string>(str))
              stringList2.Remove(str);
          }
          foreach (string str in source3)
          {
            if (stringList2.Contains(str) && !((IEnumerable<string>) source2).Contains<string>(str))
              stringList2.Remove(str);
          }
          break;
        default:
          if (Utils.CheckIf2015RespaTila(field))
          {
            foreach (string str in source2)
            {
              if (stringList2.Contains(str) && !((IEnumerable<string>) source3).Contains<string>(str))
                stringList2.Remove(str);
            }
            foreach (string str in source1)
            {
              if (stringList2.Contains(str) && !((IEnumerable<string>) source3).Contains<string>(str))
                stringList2.Remove(str);
            }
            break;
          }
          break;
      }
      if (this.session.StartupInfo.AllowURLA2020)
      {
        if (flag)
        {
          foreach (string str in strArray1)
          {
            if (stringList2.Contains(str))
              stringList2.Remove(str);
          }
        }
        else
        {
          foreach (string str in strArray2)
          {
            if (stringList2.Contains(str))
              stringList2.Remove(str);
          }
        }
      }
      else
      {
        for (int index = 0; index < ShipInDarkValidation.URLA2020FormNames.Count; ++index)
        {
          if (stringList2.Contains(ShipInDarkValidation.URLA2020FormNames[index]))
            stringList2.Remove(ShipInDarkValidation.URLA2020FormNames[index]);
        }
      }
      if (!this.emFormMenuBox.CompareToFormList(stringList2.ToArray()))
        this.emFormMenuBox.LoadFormList(stringList2.ToArray());
      if (this.emFormMenuBox.Items.Count <= 0)
        return;
      this.emFormMenuBox.SelectedIndex = 0;
    }

    private void loadDataAuditResults(AuditMessage auditMessage)
    {
      if (auditMessage != null && auditMessage.data.dataAuditResult != null && auditMessage.data.dataAuditResult.auditRuleMessages != null)
      {
        foreach (AuditRuleMessage auditRuleMessage in auditMessage.data.dataAuditResult.auditRuleMessages)
        {
          GVItem gvItem = new GVItem();
          gvItem.Tag = (object) auditRuleMessage;
          gvItem.SubItems[0].Text = auditRuleMessage.severity;
          if (auditRuleMessage.fields != null)
          {
            MultiValueElement multiValueElement = new MultiValueElement();
            string str = "";
            foreach (string field in auditRuleMessage.fields)
            {
              if (multiValueElement.ElementCount > 0)
                multiValueElement.AddElement((Element) new TextElement(", "));
              if (str.Length > 0)
                str += ", ";
              multiValueElement.AddElement((Element) this.createFieldHyperlink(field));
              str += field;
              if (!this.fieldChangeFields.Contains(field))
              {
                this.loanDataMgr.LoanData.RegisterFieldValueChangeEventHandler(field, this.fieldChangeRoutine);
                this.fieldChangeFields.Add(field);
              }
            }
            gvItem.SubItems[1].Value = (object) multiValueElement;
            gvItem.SubItems[1].SortValue = (object) str;
          }
          gvItem.SubItems[2].Text = auditRuleMessage.description;
          this.gvDataResults.Items.Add(gvItem);
        }
      }
      if (this.gvDataResults.Items.Count <= 0)
        return;
      this.gvDataResults.ReSort();
    }

    private void loadComplianceReviewResults(AuditMessage auditMessage)
    {
      if (auditMessage != null && auditMessage.data.complianceReviewResult != null && auditMessage.data.complianceReviewResult.auditRuleMessages != null)
      {
        foreach (AuditRuleMessage auditRuleMessage in auditMessage.data.complianceReviewResult.auditRuleMessages)
        {
          GVItem gvItem = new GVItem();
          gvItem.Tag = (object) auditRuleMessage;
          gvItem.SubItems[0].Text = auditRuleMessage.severity;
          if (auditRuleMessage.fields != null)
          {
            MultiValueElement multiValueElement = new MultiValueElement();
            string str = "";
            foreach (string field in auditRuleMessage.fields)
            {
              if (multiValueElement.ElementCount > 0)
                multiValueElement.AddElement((Element) new TextElement(", "));
              if (str.Length > 0)
                str += ", ";
              multiValueElement.AddElement((Element) this.createFieldHyperlink(field));
              str += field;
              if (!this.fieldChangeFields.Contains(field))
              {
                this.loanDataMgr.LoanData.RegisterFieldValueChangeEventHandler(field, this.fieldChangeRoutine);
                this.fieldChangeFields.Add(field);
              }
            }
            gvItem.SubItems[1].Value = (object) multiValueElement;
            gvItem.SubItems[1].SortValue = (object) str;
          }
          gvItem.SubItems[2].Text = auditRuleMessage.description;
          this.gvComplianceResults.Items.Add(gvItem);
        }
      }
      if (this.gvComplianceResults.Items.Count <= 0)
        return;
      this.gvComplianceResults.ReSort();
    }

    private void loadLoanFormPrintRuleResults(AuditMessage auditMessage)
    {
      if (auditMessage != null && auditMessage.data.loanFormPrintRuleResults != null)
      {
        foreach (LoanFormPrintRuleResult formPrintRuleResult in auditMessage.data.loanFormPrintRuleResults)
        {
          if (formPrintRuleResult.reasons != null)
          {
            foreach (Reason reason in formPrintRuleResult.reasons)
            {
              GVItem gvItem = new GVItem();
              gvItem.Tag = (object) reason;
              gvItem.SubItems[0].Text = formPrintRuleResult.form.type;
              gvItem.SubItems[1].Value = (object) this.createFieldHyperlink(reason.field);
              if (!this.fieldChangeFields.Contains(reason.field))
              {
                this.loanDataMgr.LoanData.RegisterFieldValueChangeEventHandler(reason.field, this.fieldChangeRoutine);
                this.fieldChangeFields.Add(reason.field);
              }
              gvItem.SubItems[2].Text = formPrintRuleResult.form.name;
              this.gvLoanFormResults.Items.Add(gvItem);
            }
          }
        }
      }
      if (this.gvLoanFormResults.Items.Count > 0)
        this.gvLoanFormResults.ReSort();
      else
        this.tcMain.TabPages.Remove(this.tabLoanFormResults);
    }

    private void selectAuditTabAndField(AuditMessage auditMessage)
    {
      switch (auditMessage.field.type)
      {
        case "dataAuditResult":
          this.tcMain.SelectedTab = this.tabDataResults;
          break;
        case "complianceReviewItems":
          this.tcMain.SelectedTab = this.tabComplianceResults;
          break;
        case "loanFormPrintRuleResults":
          this.tcMain.SelectedTab = this.tabLoanFormResults;
          break;
      }
      this.GoToField(auditMessage.field.id);
    }

    private Hyperlink createFieldHyperlink(string fieldId)
    {
      Hyperlink fieldHyperlink = new Hyperlink(fieldId, new EventHandler(this.onFieldLinkClick));
      fieldHyperlink.AlwaysUnderline = true;
      fieldHyperlink.Tag = (object) fieldId;
      return fieldHyperlink;
    }

    private void onFieldLinkClick(object sender, EventArgs e)
    {
      this.GoToField(((Element) sender).Tag.ToString(), true);
    }

    private void onFieldChanged(string fieldId, string value)
    {
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvDataResults.Items)
      {
        if (this.containsField(((AuditRuleMessage) gvItem.Tag).fields, fieldId))
          gvItem.BackColor = ThinThickEditorDialog.highlightColor;
      }
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvComplianceResults.Items)
      {
        if (this.containsField(((AuditRuleMessage) gvItem.Tag).fields, fieldId))
          gvItem.BackColor = ThinThickEditorDialog.highlightColor;
      }
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvLoanFormResults.Items)
      {
        if (((Reason) gvItem.Tag).field == fieldId)
          gvItem.BackColor = ThinThickEditorDialog.highlightColor;
      }
    }

    private bool containsField(string[] fieldList, string fieldId)
    {
      if (fieldList != null)
      {
        foreach (string field in fieldList)
        {
          if (string.Compare(field, fieldId, true) == 0)
            return true;
        }
      }
      return false;
    }

    private void emFormMenuList_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.emFormMenuBox.SelectedIndex < 0)
        return;
      this.OpenForm(this.emFormMenuBox.SelectedFormName);
      if (InputHandlerBase.InputFormAutoFocus(this.session))
        return;
      this.emFormMenuBox.Select();
      this.emFormMenuBox.Focus();
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      if (!this.loanDataMgr.LoanData.Dirty)
      {
        this.DialogResult = DialogResult.Cancel;
      }
      else
      {
        using (CursorActivator.Wait())
        {
          if (!this.loanDataMgr.Save())
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "There was a problem saving the loan file.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            return;
          }
          Session.Application.GetService<ILoanEditor>().RefreshContents();
        }
        this.DialogResult = DialogResult.OK;
      }
    }

    private void chkAlphaOrder_Click(object sender, EventArgs e)
    {
      this.emFormMenuBox.ListDisplayMode = this.chkAlphaOrder.Checked ? EMFormMenu.DisplayMode.Alphabetical : EMFormMenu.DisplayMode.Default;
    }

    private void ThinThickEditorDialog_Shown(object sender, EventArgs e)
    {
      this.selectAuditTabAndField(this.auditMessage);
    }

    private void ThinThickEditorDialog_FormClosing(object sender, FormClosingEventArgs e)
    {
      foreach (string fieldChangeField in this.fieldChangeFields)
        this.loanDataMgr.LoanData.UnregisterFieldValueChangeEventHandler(fieldChangeField, this.fieldChangeRoutine);
      if (this.loanDataMgr != null && this.session.LoanDataMgr != this.loanDataMgr)
        this.loanDataMgr.Dispose();
      if (this.freeScreen != null)
        this.freeScreen.Dispose();
      if (this.vodPan != null)
      {
        this.vodPan.Dispose();
        this.vodPan = (VODPanel) null;
      }
      if (this.voggPan != null)
      {
        this.voggPan.Dispose();
        this.voggPan = (VOGGPanel) null;
      }
      if (this.vooiPan != null)
      {
        this.vooiPan.Dispose();
        this.vooiPan = (VOOIPanel) null;
      }
      if (this.vooaPan != null)
      {
        this.vooaPan.Dispose();
        this.vooaPan = (VOOAPanel) null;
      }
      if (this.voePan != null)
      {
        this.voePan.Dispose();
        this.voePan = (VOEPanel) null;
      }
      if (this.volPan != null)
      {
        this.volPan.Dispose();
        this.volPan = (VOLPanel) null;
      }
      if (this.voolPan != null)
      {
        this.voolPan.Dispose();
        this.voolPan = (VOOLPanel) null;
      }
      if (this.vomPan != null)
      {
        this.vomPan.Dispose();
        this.vomPan = (VOMPanel) null;
      }
      if (this.vorPan != null)
      {
        this.vorPan.Dispose();
        this.vorPan = (VORPanel) null;
      }
      if (this.voalPan != null)
      {
        this.voalPan.Dispose();
        this.voalPan = (VOALPanel) null;
      }
      if (this.tax4506Pan != null)
      {
        this.tax4506Pan.Dispose();
        this.tax4506Pan = (TAX4506Panel) null;
      }
      if (this.tax4506TPan == null)
        return;
      this.tax4506TPan.Dispose();
      this.tax4506TPan = (TAX4506TPanel) null;
    }

    public string GetHelpTargetName() => nameof (ThinThickEditorDialog);

    public void ShowHelp()
    {
      JedHelp.ShowHelp((Control) this, SystemSettings.HelpFile, this.GetHelpTargetName());
    }

    public TriggerEmailTemplate MilestoneTemplateEmailTemplate
    {
      get => throw new Exception("The method or operation is not implemented.");
      set => throw new Exception("The method or operation is not implemented.");
    }

    public bool SelectSettlementServiceProviders()
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public bool SelectAffilatesTemplate()
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public void ShowMilestoneWorksheet(MilestoneLog ms)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public void ShoweDisclosureTrackingRecord(string packageID)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public void RefreshLoan() => throw new Exception("The method or operation is not implemented.");

    public void ShowVerifPanel(string verifType)
    {
      PanelBase panelBase = (PanelBase) null;
      switch (verifType)
      {
        case "TAX4506":
          this.tax4506Pan = new TAX4506Panel(this.session.MainScreen, (IWorkArea) this);
          panelBase = (PanelBase) this.tax4506Pan;
          break;
        case "TAX4506T":
          this.tax4506TPan = new TAX4506TPanel(this.session.MainScreen, (IWorkArea) this);
          panelBase = (PanelBase) this.tax4506TPan;
          break;
        case "VOAL":
          this.voalPan = new VOALPanel(this.session.MainScreen, (IWorkArea) this);
          panelBase = (PanelBase) this.voalPan;
          break;
        case "VOD":
          this.vodPan = new VODPanel(this.session.MainScreen, (IWorkArea) this);
          panelBase = (PanelBase) this.vodPan;
          break;
        case "VOE":
          this.voePan = new VOEPanel(this.session.MainScreen, (IWorkArea) this);
          panelBase = (PanelBase) this.voePan;
          break;
        case "VOGG":
          this.voggPan = new VOGGPanel(this.session.MainScreen, (IWorkArea) this);
          panelBase = (PanelBase) this.voggPan;
          break;
        case "VOL":
          this.volPan = new VOLPanel(this.session.MainScreen, (IWorkArea) this);
          panelBase = (PanelBase) this.volPan;
          break;
        case "VOM":
          this.vomPan = new VOMPanel(this.session.MainScreen, (IWorkArea) this);
          panelBase = (PanelBase) this.vomPan;
          break;
        case "VOOA":
          this.vooaPan = new VOOAPanel(this.session.MainScreen, (IWorkArea) this);
          panelBase = (PanelBase) this.vooaPan;
          break;
        case "VOOI":
          this.vooiPan = new VOOIPanel(this.session.MainScreen, (IWorkArea) this);
          panelBase = (PanelBase) this.vooiPan;
          break;
        case "VOOL":
          this.voolPan = new VOOLPanel(this.session.MainScreen, (IWorkArea) this);
          panelBase = (PanelBase) this.voolPan;
          break;
        case "VOR":
          this.vorPan = new VORPanel(this.session.MainScreen, (IWorkArea) this);
          panelBase = (PanelBase) this.vorPan;
          break;
      }
      this.pnlWeb.Controls.Add((Control) panelBase);
      this.currForm = this.loanDataMgr.InputFormSettings.GetForm(verifType);
      panelBase.RefreshListView((object) null, (EventArgs) null);
    }

    public bool ContainsControl(Control con)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public Control TopControl
    {
      set => this.topControl = value;
    }

    public void RefreshContents()
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public void RefreshLoanContents()
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public void RefreshContents(string fieldId)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public void RefreshLogPanel()
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public void StartConversation(ConversationLog con)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public void RefreshLoanTeamMembers()
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public void AddMilestoneWorksheet(MilestoneLog milestoneLog)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public void ApplyBusinessRules()
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public void SetMilestoneStatus(MilestoneLog milestoneLog, int milestoneIndex, bool finished)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public void ClearMilestoneLogArea()
    {
      throw new Exception("The method or operation is not implemented.");
    }

    private void clearWorkArea()
    {
      ArrayList arrayList = new ArrayList();
      foreach (Control control in (ArrangedElementCollection) this.pnlWeb.Controls)
        arrayList.Add((object) control);
      this.pnlWeb.Controls.Clear();
      this.topControl = (Control) null;
      foreach (Control control in arrayList)
      {
        if (!(control is WorkAreaPanelBase))
          control.Dispose();
      }
    }

    public void AddToWorkArea(Control worksheet) => this.AddToWorkArea(worksheet, false);

    public void AddToWorkArea(Control newControl, bool rememberCurrentFormID)
    {
      this.clearWorkArea();
      if (!rememberCurrentFormID)
        this.currForm = (InputFormInfo) null;
      else if (newControl is TabLinksControl && this.currForm == (InputFormInfo) null)
        this.currForm = ((TabLinksControl) newControl).GetCurrentFormInfo();
      if (newControl is Form)
        newControl.Parent = (Control) this.pnlWeb;
      this.pnlWeb.Controls.Add(newControl);
      newControl.Focus();
      this.topControl = newControl;
    }

    public void RemoveFromWorkArea()
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public DateTime AddDays(DateTime date, int dayCount)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public int MinusBusinessDays(DateTime previous, DateTime currentLog)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public void SaveLoan() => throw new Exception("The method or operation is not implemented.");

    public string CurrentForm
    {
      get => throw new Exception("The method or operation is not implemented.");
      set => throw new Exception("The method or operation is not implemented.");
    }

    public object GetFormScreen()
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public object GetVerifScreen()
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public void GoToField(string fieldID) => this.GoToField(fieldID, false);

    public void GoToField(string fieldID, BorrowerPair targetPair)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public void GoToField(string fieldID, string formName)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public void GoToField(string fieldID, bool findNext)
    {
      this.GoToField(fieldID, findNext, false);
    }

    public void BAMGoToField(string fieldID, bool findNext)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public void GoToField(string fieldID, bool findNext, bool searchToolPageOnly)
    {
      if (fieldID == string.Empty)
        return;
      if (fieldID.StartsWith("AR") && this.tax4506Pan == null)
        this.tax4506Pan = new TAX4506Panel(this.session.MainScreen, (IWorkArea) this);
      if (fieldID.StartsWith("IR") && this.tax4506TPan == null)
        this.tax4506TPan = new TAX4506TPanel(this.session.MainScreen, (IWorkArea) this);
      this.searchFields.GoToField(fieldID, findNext, false, this.topControl, this.emFormMenuBox, this.emToolMenuBox, this.currForm, this.tcFormsTools, this.volPan, this.vorPan, this.voePan, this.vomPan, this.tax4506Pan, this.tax4506TPan, this.chkAllForms, this.freeScreen, this.session, (ILoanEditor) this);
    }

    public void ApplyOnDemandBusinessRules()
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public bool OpenForm(string formOrToolName) => this.OpenForm(formOrToolName, (Control) null);

    public bool OpenForm(string formOrToolName, Control navControl)
    {
      if (formOrToolName == "MLDS - CA GFE (RE882)" || formOrToolName.StartsWith("State Specific Information"))
        return this.OpenFormByID(formOrToolName, navControl);
      InputFormInfo inputFormInfo = this.loanDataMgr.InputFormSettings.GetFormByName(formOrToolName);
      if (inputFormInfo == (InputFormInfo) null)
      {
        switch (formOrToolName)
        {
          case "2018 HMDA Originated/Adverse Action Loans":
            inputFormInfo = new InputFormInfo("HMDA2018_Originated", "2018 HMDA Originated/Adverse Action Loans");
            break;
          case "2018 HMDA Purchased Loans":
            inputFormInfo = new InputFormInfo("HMDA2018_Purchased", "2018 HMDA Purchased Loans");
            break;
          case "Repurchased Loans":
            inputFormInfo = new InputFormInfo("HMDA2018_Repurchased", "Repurchased Loans");
            break;
          default:
            return false;
        }
      }
      return this.OpenFormByID(inputFormInfo.FormID, navControl);
    }

    public bool OpenFormByID(string formOrToolID)
    {
      return this.OpenFormByID(formOrToolID, (Control) null);
    }

    private void DisposeWebPanelControls()
    {
      foreach (Control control in (ArrangedElementCollection) this.pnlWeb.Controls)
      {
        if (!(control is LoanScreen))
          control.Dispose();
      }
      GC.Collect();
      GC.WaitForPendingFinalizers();
    }

    public bool OpenFormByID(string formOrToolID, Control navControl)
    {
      Cursor.Current = Cursors.WaitCursor;
      this.currForm = !(formOrToolID == "MLDS - CA GFE (RE882)") ? (!formOrToolID.StartsWith("State Specific Information") ? this.loanDataMgr.InputFormSettings.GetForm(formOrToolID) : new InputFormInfo(this.getStateForm(formOrToolID), formOrToolID, InputFormType.Standard)) : new InputFormInfo("RE882", "MLDS - CA GFE", InputFormType.Standard);
      if (this.currForm == (InputFormInfo) null)
      {
        switch (formOrToolID)
        {
          case "HMDA2018_Originated":
            this.currForm = new InputFormInfo("HMDA2018_Originated", "2018 HMDA Originated/Adverse Action Loans");
            break;
          case "HMDA2018_Purchased":
            this.currForm = new InputFormInfo("HMDA2018_Purchased", "2018 HMDA Purchased Loans");
            break;
          case "HMDA2018_Repurchased":
            this.currForm = new InputFormInfo("HMDA2018_Repurchased", "Repurchased Loans");
            break;
        }
      }
      if (this.currForm.FormID == "HMDA_DENIAL")
        this.currForm = new InputFormInfo("HMDA_DENIAL04", this.currForm.MnemonicName);
      if (this.currForm.FormID == "FUNDINGWORKSHEET")
        this.currForm = new InputFormInfo("FUNDERWORKSHEET", this.currForm.MnemonicName);
      this.DisposeWebPanelControls();
      this.pnlWeb.Controls.Clear();
      if (this.freeScreen == null)
      {
        this.freeScreen = new LoanScreen(this.session, (IWin32Window) this, (IHtmlInput) this.loanDataMgr.LoanData);
        this.freeScreen.FormLoaded += new EventHandler(this.freeScreen_FormLoaded);
        this.freeScreen.SetHelpTarget((IOnlineHelpTarget) this);
        this.pnlWeb.Controls.Add((Control) this.freeScreen);
      }
      if (this.currForm.Name == "Self-Employed Income 1084")
      {
        this.currForm = new InputFormInfo("FM1084A", "108&4A Cash Analysis");
        this.freeScreen.SetTitle(this.currForm.Name, (Control) new QuickLinksControl((ILoanEditor) this, QuickLinksControl.GetQuickLinksForForm(this.currForm.FormID, this.session), this.currForm.FormID, this.session));
      }
      else if (this.currForm.Name == "Custom Fields")
        this.freeScreen.SetTitle(this.currForm.Name, (Control) new CustomFieldsPanel(this.freeScreen, this.session));
      else if (this.currForm.Name == "Borrower Information - Vesting" || this.currForm.Name == "File Contacts" || this.currForm.Name == "1003 Page 4" || this.currForm.Name == "Home Counseling Providers")
      {
        this.freeScreen.LoadWindowsForm(this.currForm.FormID);
        this.freeScreen.SetTitleOnly(this.currForm.Name);
      }
      else if (this.currForm.FormID == "VOR" || this.currForm.FormID == "VOL" || this.currForm.FormID == "VOM" || this.currForm.FormID == "VOD" || this.currForm.FormID == "VOGG" || this.currForm.FormID == "VOOI" || this.currForm.FormID == "VOOA" || this.currForm.FormID == "VOOL" || this.currForm.FormID == "VOAL" || this.currForm.FormID == "VOE" || this.currForm.FormID == "TAX4506T" || this.currForm.FormID == "TAX4506")
      {
        this.ShowVerifPanel(this.currForm.FormID);
        if (this.currForm.Name == "VOL" && this.loanDataMgr.LoanData != null && !this.loanDataMgr.LoanData.Calculator.ValidateRevolvingVOLs() && this.loanDataMgr.LoanData.Calculator.UpdateRevolvingLiabilities((string) null, (string) null, true, true, true))
          this.ShowVerifPanel(this.currForm.Name);
      }
      else
      {
        if (TabLinksControl.UseTabLinks(this.session, this.currForm, this.loanDataMgr.LoanData))
        {
          this.freeScreen.BrwHandler.UnloadForm();
          this.pnlWeb.Controls.Add((Control) new TabLinksControl(this.session, this.currForm, (IWin32Window) this, this.loanDataMgr.LoanData));
          return true;
        }
        this.freeScreen.SetTitle(this.currForm.Name);
        this.freeScreen.LoadForm(this.currForm);
      }
      this.pnlWeb.Controls.Add((Control) this.freeScreen);
      return true;
    }

    private void freeScreen_FormLoaded(object sender, EventArgs e)
    {
      this.freeScreen.ApplyTemplateFieldAccessRights();
    }

    private void loadHMDAForm(string id, string val)
    {
      InputFormInfo inputFormInfo = new InputFormInfo("HMDA_DENIAL04", "HMDA Information");
      this.OpenForm("HMDA Information");
    }

    private string getStateForm(string formName)
    {
      int num = formName.IndexOf("-");
      return "State_Specific_" + Utils.GetStateAbbr(formName.Substring(num + 1).Trim());
    }

    public bool OpenLogRecord(LogRecordBase rec)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public void OpenMilestoneLogReview(MilestoneLog log, MilestoneHistoryLog historyLog)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public void OpenMilestoneLogReview(MilestoneLog log)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public void SendLockRequest(bool closeFile)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public void PromptCreateNewLogRecord()
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public bool IsPrimaryEditor => false;

    public bool GetInputEngineService(LoanData loan, InputEngineServiceType serviceType)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public bool GetInputEngineService(LoanDataMgr loanDataMgr, InputEngineServiceType serviceType)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public bool Print(string[] formNames)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public bool ShowRegulationAlerts()
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public bool ShowRegulationAlertsOrderDoc()
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public void ShoweDisclosureTrackingRecord(
      DisclosureTrackingBase selectedLog,
      bool clearNotification)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public void ShowPlanCodeComparison(string fieldId, DocumentOrderType orderType)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public void ShowAUSTrackingTool()
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public string[] SelectLinkAndSyncTemplate()
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public void ShowAIQAnalyzerMessage(
      string analyzerType,
      DateTime alertDateTime,
      string description,
      string messageID)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public void LaunchAIQIncomeAnalyzer()
    {
      throw new Exception("The method or operation is not implemented.");
    }

    DialogResult ILoanEditor.OpenModal(string openModalOptions)
    {
      throw new NotImplementedException();
    }

    public void RedirectToUrl(string targetName) => throw new NotImplementedException();

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
      GVColumn gvColumn8 = new GVColumn();
      GVColumn gvColumn9 = new GVColumn();
      this.pnlWeb = new Panel();
      this.gcForms = new GroupContainer();
      this.emFormMenuBox = new EMFormMenu();
      this.pnlAlphaOrder = new BorderPanel();
      this.chkAlphaOrder = new CheckBox();
      this.tcFormsTools = new TabControl();
      this.tabForms = new TabPage();
      this.chkAllForms = new CheckBox();
      this.tabTools = new TabPage();
      this.emToolMenuBox = new EMFormMenu();
      this.pnlTop = new BorderPanel();
      this.tcMain = new TabControl();
      this.tabDataResults = new TabPage();
      this.gvDataResults = new GridView();
      this.tabComplianceResults = new TabPage();
      this.gvComplianceResults = new GridView();
      this.tabLoanFormResults = new TabPage();
      this.gvLoanFormResults = new GridView();
      this.pnlBottom = new BorderPanel();
      this.emHelpLink = new EMHelpLink();
      this.btnCancel = new Button();
      this.btnSave = new Button();
      this.csAudit = new CollapsibleSplitter();
      this.pnlMiddle = new BorderPanel();
      this.csForms = new CollapsibleSplitter();
      this.gcForms.SuspendLayout();
      this.pnlAlphaOrder.SuspendLayout();
      this.tcFormsTools.SuspendLayout();
      this.tabForms.SuspendLayout();
      this.tabTools.SuspendLayout();
      this.pnlTop.SuspendLayout();
      this.tcMain.SuspendLayout();
      this.tabDataResults.SuspendLayout();
      this.tabComplianceResults.SuspendLayout();
      this.tabLoanFormResults.SuspendLayout();
      this.pnlBottom.SuspendLayout();
      this.pnlMiddle.SuspendLayout();
      this.SuspendLayout();
      this.pnlWeb.Dock = DockStyle.Fill;
      this.pnlWeb.Location = new Point(203, 0);
      this.pnlWeb.Name = "pnlWeb";
      this.pnlWeb.Size = new Size(611, 292);
      this.pnlWeb.TabIndex = 11;
      this.gcForms.Controls.Add((Control) this.emFormMenuBox);
      this.gcForms.Controls.Add((Control) this.pnlAlphaOrder);
      this.gcForms.Controls.Add((Control) this.tcFormsTools);
      this.gcForms.Dock = DockStyle.Left;
      this.gcForms.HeaderForeColor = SystemColors.ControlText;
      this.gcForms.Location = new Point(0, 0);
      this.gcForms.Name = "gcForms";
      this.gcForms.Size = new Size(196, 292);
      this.gcForms.TabIndex = 7;
      this.gcForms.Text = "Forms";
      this.emFormMenuBox.AlternatingColors = false;
      this.emFormMenuBox.BorderStyle = BorderStyle.None;
      this.emFormMenuBox.Dock = DockStyle.Fill;
      this.emFormMenuBox.GridLines = false;
      this.emFormMenuBox.Location = new Point(1, 26);
      this.emFormMenuBox.Name = "emFormMenuBox";
      this.emFormMenuBox.Size = new Size(194, 238);
      this.emFormMenuBox.TabIndex = 8;
      this.emFormMenuBox.SelectedIndexChanged += new EventHandler(this.emFormMenuList_SelectedIndexChanged);
      this.pnlAlphaOrder.Borders = AnchorStyles.Top;
      this.pnlAlphaOrder.Controls.Add((Control) this.chkAlphaOrder);
      this.pnlAlphaOrder.Dock = DockStyle.Bottom;
      this.pnlAlphaOrder.Location = new Point(1, 264);
      this.pnlAlphaOrder.Name = "pnlAlphaOrder";
      this.pnlAlphaOrder.Size = new Size(194, 27);
      this.pnlAlphaOrder.TabIndex = 9;
      this.pnlAlphaOrder.TabStop = true;
      this.chkAlphaOrder.AutoSize = true;
      this.chkAlphaOrder.BackColor = Color.Transparent;
      this.chkAlphaOrder.Location = new Point(3, 4);
      this.chkAlphaOrder.Name = "chkAlphaOrder";
      this.chkAlphaOrder.Size = new Size((int) sbyte.MaxValue, 18);
      this.chkAlphaOrder.TabIndex = 10;
      this.chkAlphaOrder.Text = "Show in Alpha Order";
      this.chkAlphaOrder.UseVisualStyleBackColor = false;
      this.chkAlphaOrder.Click += new EventHandler(this.chkAlphaOrder_Click);
      this.tcFormsTools.Controls.Add((Control) this.tabForms);
      this.tcFormsTools.Controls.Add((Control) this.tabTools);
      this.tcFormsTools.Location = new Point(8, 40);
      this.tcFormsTools.Name = "tcFormsTools";
      this.tcFormsTools.SelectedIndex = 0;
      this.tcFormsTools.Size = new Size(176, 204);
      this.tcFormsTools.TabIndex = 16;
      this.tcFormsTools.Visible = false;
      this.tabForms.Controls.Add((Control) this.chkAllForms);
      this.tabForms.Location = new Point(4, 23);
      this.tabForms.Name = "tabForms";
      this.tabForms.Padding = new Padding(3);
      this.tabForms.Size = new Size(168, 177);
      this.tabForms.TabIndex = 0;
      this.tabForms.Text = "Forms";
      this.tabForms.UseVisualStyleBackColor = true;
      this.chkAllForms.AutoSize = true;
      this.chkAllForms.BackColor = Color.Transparent;
      this.chkAllForms.Checked = true;
      this.chkAllForms.CheckState = CheckState.Checked;
      this.chkAllForms.Location = new Point(4, 4);
      this.chkAllForms.Name = "chkAllForms";
      this.chkAllForms.Size = new Size((int) sbyte.MaxValue, 18);
      this.chkAllForms.TabIndex = 17;
      this.chkAllForms.Text = "Show in Alpha Order";
      this.chkAllForms.UseVisualStyleBackColor = false;
      this.tabTools.Controls.Add((Control) this.emToolMenuBox);
      this.tabTools.Location = new Point(4, 22);
      this.tabTools.Name = "tabTools";
      this.tabTools.Padding = new Padding(3);
      this.tabTools.Size = new Size(168, 178);
      this.tabTools.TabIndex = 1;
      this.tabTools.Text = "Tools";
      this.tabTools.UseVisualStyleBackColor = true;
      this.emToolMenuBox.AlternatingColors = false;
      this.emToolMenuBox.BorderStyle = BorderStyle.None;
      this.emToolMenuBox.Dock = DockStyle.Fill;
      this.emToolMenuBox.GridLines = false;
      this.emToolMenuBox.Location = new Point(3, 3);
      this.emToolMenuBox.Name = "emToolMenuBox";
      this.emToolMenuBox.Size = new Size(162, 171);
      this.emToolMenuBox.TabIndex = 18;
      this.pnlTop.Controls.Add((Control) this.tcMain);
      this.pnlTop.Dock = DockStyle.Top;
      this.pnlTop.Location = new Point(0, 0);
      this.pnlTop.Name = "pnlTop";
      this.pnlTop.Size = new Size(814, 172);
      this.pnlTop.TabIndex = 0;
      this.pnlTop.TabStop = true;
      this.tcMain.Controls.Add((Control) this.tabDataResults);
      this.tcMain.Controls.Add((Control) this.tabComplianceResults);
      this.tcMain.Controls.Add((Control) this.tabLoanFormResults);
      this.tcMain.Dock = DockStyle.Fill;
      this.tcMain.HotTrack = true;
      this.tcMain.ItemSize = new Size(74, 28);
      this.tcMain.Location = new Point(1, 1);
      this.tcMain.Margin = new Padding(0);
      this.tcMain.Name = "tcMain";
      this.tcMain.Padding = new Point(11, 3);
      this.tcMain.SelectedIndex = 0;
      this.tcMain.Size = new Size(812, 170);
      this.tcMain.TabIndex = 1;
      this.tabDataResults.Controls.Add((Control) this.gvDataResults);
      this.tabDataResults.Location = new Point(4, 32);
      this.tabDataResults.Name = "tabDataResults";
      this.tabDataResults.Padding = new Padding(0, 2, 2, 2);
      this.tabDataResults.Size = new Size(804, 134);
      this.tabDataResults.TabIndex = 0;
      this.tabDataResults.Text = "Data Audit Results";
      this.tabDataResults.UseVisualStyleBackColor = true;
      this.gvDataResults.AllowMultiselect = false;
      this.gvDataResults.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "typeHdr";
      gvColumn1.Text = "Type";
      gvColumn1.Width = 100;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "fieldHdr";
      gvColumn2.Text = "Field Id";
      gvColumn2.Width = 150;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "descriptionHdr";
      gvColumn3.Text = "Description";
      gvColumn3.Width = 500;
      this.gvDataResults.Columns.AddRange(new GVColumn[3]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3
      });
      this.gvDataResults.Dock = DockStyle.Fill;
      this.gvDataResults.HotItemTracking = false;
      this.gvDataResults.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvDataResults.Location = new Point(0, 2);
      this.gvDataResults.Name = "gvDataResults";
      this.gvDataResults.Selectable = false;
      this.gvDataResults.Size = new Size(802, 130);
      this.gvDataResults.TabIndex = 2;
      this.gvDataResults.TextTrimming = StringTrimming.EllipsisCharacter;
      this.tabComplianceResults.Controls.Add((Control) this.gvComplianceResults);
      this.tabComplianceResults.Location = new Point(4, 32);
      this.tabComplianceResults.Name = "tabComplianceResults";
      this.tabComplianceResults.Padding = new Padding(0, 2, 2, 2);
      this.tabComplianceResults.Size = new Size(804, 134);
      this.tabComplianceResults.TabIndex = 2;
      this.tabComplianceResults.Text = "Compliance Review Results";
      this.tabComplianceResults.UseVisualStyleBackColor = true;
      this.gvComplianceResults.AllowMultiselect = false;
      this.gvComplianceResults.BorderStyle = BorderStyle.None;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "typeHdr";
      gvColumn4.Text = "Type";
      gvColumn4.Width = 100;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "fieldHdr";
      gvColumn5.Text = "Field Id";
      gvColumn5.Width = 150;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "descriptionHdr";
      gvColumn6.Text = "Description";
      gvColumn6.Width = 500;
      this.gvComplianceResults.Columns.AddRange(new GVColumn[3]
      {
        gvColumn4,
        gvColumn5,
        gvColumn6
      });
      this.gvComplianceResults.Dock = DockStyle.Fill;
      this.gvComplianceResults.HotItemTracking = false;
      this.gvComplianceResults.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvComplianceResults.Location = new Point(0, 2);
      this.gvComplianceResults.Name = "gvComplianceResults";
      this.gvComplianceResults.Selectable = false;
      this.gvComplianceResults.Size = new Size(802, 130);
      this.gvComplianceResults.TabIndex = 3;
      this.gvComplianceResults.TextTrimming = StringTrimming.EllipsisCharacter;
      this.tabLoanFormResults.Controls.Add((Control) this.gvLoanFormResults);
      this.tabLoanFormResults.Location = new Point(4, 32);
      this.tabLoanFormResults.Name = "tabLoanFormResults";
      this.tabLoanFormResults.Padding = new Padding(0, 2, 2, 2);
      this.tabLoanFormResults.Size = new Size(804, 134);
      this.tabLoanFormResults.TabIndex = 3;
      this.tabLoanFormResults.Text = "Loan Form Printing Rules Results";
      this.tabLoanFormResults.UseVisualStyleBackColor = true;
      this.gvLoanFormResults.AllowMultiselect = false;
      this.gvLoanFormResults.BorderStyle = BorderStyle.None;
      gvColumn7.ImageIndex = -1;
      gvColumn7.Name = "typeHdr";
      gvColumn7.Text = "Type";
      gvColumn7.Width = 100;
      gvColumn8.ImageIndex = -1;
      gvColumn8.Name = "fieldHdr";
      gvColumn8.Text = "Field Id";
      gvColumn8.Width = 150;
      gvColumn9.ImageIndex = -1;
      gvColumn9.Name = "descriptionHdr";
      gvColumn9.Text = "Description";
      gvColumn9.Width = 500;
      this.gvLoanFormResults.Columns.AddRange(new GVColumn[3]
      {
        gvColumn7,
        gvColumn8,
        gvColumn9
      });
      this.gvLoanFormResults.Dock = DockStyle.Fill;
      this.gvLoanFormResults.HotItemTracking = false;
      this.gvLoanFormResults.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvLoanFormResults.Location = new Point(0, 2);
      this.gvLoanFormResults.Name = "gvLoanFormResults";
      this.gvLoanFormResults.Selectable = false;
      this.gvLoanFormResults.Size = new Size(802, 130);
      this.gvLoanFormResults.TabIndex = 4;
      this.gvLoanFormResults.TextTrimming = StringTrimming.EllipsisCharacter;
      this.pnlBottom.Borders = AnchorStyles.None;
      this.pnlBottom.Controls.Add((Control) this.emHelpLink);
      this.pnlBottom.Controls.Add((Control) this.btnCancel);
      this.pnlBottom.Controls.Add((Control) this.btnSave);
      this.pnlBottom.Dock = DockStyle.Bottom;
      this.pnlBottom.Location = new Point(0, 472);
      this.pnlBottom.Name = "pnlBottom";
      this.pnlBottom.Size = new Size(814, 44);
      this.pnlBottom.TabIndex = 12;
      this.emHelpLink.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.emHelpLink.BackColor = Color.Transparent;
      this.emHelpLink.Cursor = Cursors.Hand;
      this.emHelpLink.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.emHelpLink.HelpTag = "Data Templates";
      this.emHelpLink.Location = new Point(12, 16);
      this.emHelpLink.Name = "emHelpLink";
      this.emHelpLink.Size = new Size(90, 16);
      this.emHelpLink.TabIndex = 13;
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(727, 12);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 24);
      this.btnCancel.TabIndex = 15;
      this.btnCancel.Text = "&Cancel";
      this.btnSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnSave.Location = new Point(647, 12);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(75, 24);
      this.btnSave.TabIndex = 14;
      this.btnSave.Text = "&Save";
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.csAudit.AnimationDelay = 20;
      this.csAudit.AnimationStep = 20;
      this.csAudit.BorderStyle3D = Border3DStyle.Flat;
      this.csAudit.ControlToHide = (Control) this.pnlTop;
      this.csAudit.Dock = DockStyle.Top;
      this.csAudit.ExpandParentForm = false;
      this.csAudit.Location = new Point(0, 172);
      this.csAudit.Name = "csAudit";
      this.csAudit.TabIndex = 14;
      this.csAudit.TabStop = false;
      this.csAudit.UseAnimations = false;
      this.csAudit.VisualStyle = VisualStyles.Encompass;
      this.pnlMiddle.Borders = AnchorStyles.Bottom;
      this.pnlMiddle.Controls.Add((Control) this.pnlWeb);
      this.pnlMiddle.Controls.Add((Control) this.csForms);
      this.pnlMiddle.Controls.Add((Control) this.gcForms);
      this.pnlMiddle.Dock = DockStyle.Fill;
      this.pnlMiddle.Location = new Point(0, 179);
      this.pnlMiddle.Name = "pnlMiddle";
      this.pnlMiddle.Size = new Size(814, 293);
      this.pnlMiddle.TabIndex = 6;
      this.pnlMiddle.TabStop = true;
      this.csForms.AnimationDelay = 20;
      this.csForms.AnimationStep = 20;
      this.csForms.BorderStyle3D = Border3DStyle.Raised;
      this.csForms.ControlToHide = (Control) this.gcForms;
      this.csForms.Cursor = Cursors.Default;
      this.csForms.ExpandParentForm = false;
      this.csForms.Location = new Point(196, 0);
      this.csForms.Name = "splitter1";
      this.csForms.TabIndex = 16;
      this.csForms.TabStop = false;
      this.csForms.UseAnimations = false;
      this.csForms.VisualStyle = VisualStyles.Encompass;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.BackColor = Color.WhiteSmoke;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(814, 516);
      this.Controls.Add((Control) this.pnlMiddle);
      this.Controls.Add((Control) this.pnlBottom);
      this.Controls.Add((Control) this.csAudit);
      this.Controls.Add((Control) this.pnlTop);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.KeyPreview = true;
      this.MinimizeBox = false;
      this.Name = nameof (ThinThickEditorDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Loan Audit Editor";
      this.FormClosing += new FormClosingEventHandler(this.ThinThickEditorDialog_FormClosing);
      this.Shown += new EventHandler(this.ThinThickEditorDialog_Shown);
      this.gcForms.ResumeLayout(false);
      this.pnlAlphaOrder.ResumeLayout(false);
      this.pnlAlphaOrder.PerformLayout();
      this.tcFormsTools.ResumeLayout(false);
      this.tabForms.ResumeLayout(false);
      this.tabForms.PerformLayout();
      this.tabTools.ResumeLayout(false);
      this.pnlTop.ResumeLayout(false);
      this.tcMain.ResumeLayout(false);
      this.tabDataResults.ResumeLayout(false);
      this.tabComplianceResults.ResumeLayout(false);
      this.tabLoanFormResults.ResumeLayout(false);
      this.pnlBottom.ResumeLayout(false);
      this.pnlMiddle.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
