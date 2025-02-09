// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.ContactAdvancedSearchForm
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.ClientServer.Query;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ContactUI
{
  public class ContactAdvancedSearchForm : Form
  {
    private System.ComponentModel.Container components;
    protected string description = string.Empty;
    protected ArrayList criteria = new ArrayList();
    protected RelatedLoanMatchType loanMatchType;

    public ContactAdvancedSearchForm() => this.InitializeComponent();

    public string SearchDescription => this.description;

    public QueryCriterion[] QueryCriteria
    {
      get => (QueryCriterion[]) this.criteria.ToArray(typeof (QueryCriterion));
    }

    public RelatedLoanMatchType RelatedLoanMatchType => this.loanMatchType;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(432, 361);
      this.Name = nameof (ContactAdvancedSearchForm);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = nameof (ContactAdvancedSearchForm);
    }

    protected void initializeNumericField(
      ComboBox combo,
      TextBox text1,
      TextBox text2,
      OrdinalValueCriterion criterion)
    {
      if (text1.Text != "")
      {
        combo.SelectedIndex = 3;
        text2.Text = criterion.Value.ToString();
      }
      else if (text2.Text != "")
      {
        combo.SelectedIndex = 3;
        text1.Text = criterion.Value.ToString();
      }
      else
      {
        combo.SelectedIndex = criterion.MatchType == OrdinalMatchType.Equals ? 0 : (criterion.MatchType == OrdinalMatchType.GreaterThan ? 1 : 2);
        text1.Text = criterion.Value.ToString();
      }
    }

    protected void initializeEnumField(ComboBox combo, StringValueCriterion criterion)
    {
      for (int index = 0; index < combo.Items.Count; ++index)
      {
        if (criterion.Value == combo.Items[index].ToString())
        {
          combo.SelectedIndex = index;
          break;
        }
      }
    }

    protected void initializeDateField(
      ComboBox combo,
      DateTimePicker picker1,
      DateTimePicker picker2,
      DateValueCriterion criterion,
      string format)
    {
      DateTimePicker dateTimePicker = picker1;
      if (picker1.Tag != null)
      {
        combo.SelectedIndex = 3;
        dateTimePicker = picker2;
      }
      else if (picker2.Tag != null)
      {
        combo.SelectedIndex = 3;
        dateTimePicker = picker1;
      }
      else
        combo.SelectedIndex = criterion.MatchType == OrdinalMatchType.Equals ? 0 : (criterion.MatchType == OrdinalMatchType.LessThan ? 1 : 2);
      dateTimePicker.CustomFormat = format;
      dateTimePicker.Value = criterion.Value;
      dateTimePicker.Tag = (object) "";
    }
  }
}
