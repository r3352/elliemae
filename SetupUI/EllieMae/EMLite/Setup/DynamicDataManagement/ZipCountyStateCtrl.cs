// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.DynamicDataManagement.ZipCountyStateCtrl
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Properties;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.DynamicDataManagement
{
  public class ZipCountyStateCtrl : UserControl
  {
    private List<ZipCountyState> srcList = new List<ZipCountyState>();
    private List<ZipCountyState> destList = new List<ZipCountyState>();
    private List<string> states = new List<string>();
    private const string DELIMITER = ",";
    private HashSet<string> zipCollection = new HashSet<string>();
    private List<string> countyCollection = new List<string>();
    private string valuesExisting = string.Empty;
    public bool IsCounty;
    protected Sessions.Session session;
    private ZipCountyStateCtrl.FormDataContext frmDataContext;
    private Dictionary<string, List<ZipCountyState>> selectedGroupZcs = new Dictionary<string, List<ZipCountyState>>();
    private int selectedStateIndex;
    private IContainer components;
    private GroupBox SourceGB;
    private GroupBox DestinationGB;
    private Label lblSource;
    private Label lblDestination;
    private GridView gvSource;
    private GridView gvDestination;
    private StandardIconButton stdAdd;
    private StandardIconButton stdRemove;
    private Label label1;
    private ComboBox cmbStates;
    private Panel pnlControls;
    private Label lblCounty;
    private ComboBox cmbCounty;

    public ZipCountyStateCtrl(bool isCounty, Sessions.Session ses)
    {
      this.IsCounty = isCounty;
      this.InitializeComponent();
      this.configureComboLayout();
      this.configureGridLayout();
      this.session = ses;
      this.frmDataContext = new ZipCountyStateCtrl.FormDataContext(this.session);
    }

    private void configureComboLayout()
    {
      this.cmbCounty.SelectedIndexChanged -= new EventHandler(this.cmbCounty_SelectedIndexChanged);
      if (this.IsCounty)
      {
        this.lblCounty.Visible = false;
        this.cmbCounty.Visible = false;
        this.pnlControls.Location = new Point(0, 34);
      }
      else
      {
        this.lblCounty.Visible = true;
        this.cmbCounty.Visible = true;
        this.pnlControls.Location = new Point(0, 60);
        this.cmbCounty.SelectedIndexChanged += new EventHandler(this.cmbCounty_SelectedIndexChanged);
      }
    }

    private void configureGridLayout()
    {
      if (this.IsCounty)
      {
        this.gvSource.Columns.Clear();
        this.gvSource.Columns.Add("County", this.gvSource.Width, ContentAlignment.MiddleLeft);
        this.gvDestination.Columns.Clear();
        this.gvDestination.Columns.Add("County", this.gvDestination.Width - 20, ContentAlignment.MiddleLeft);
      }
      else
      {
        this.gvSource.Columns.Clear();
        this.gvSource.Columns.Add("County", 75, ContentAlignment.MiddleLeft);
        this.gvSource.Columns.Add("City", 75, ContentAlignment.MiddleLeft);
        this.gvSource.Columns.Add("Zip", 61, ContentAlignment.MiddleLeft);
        this.gvDestination.Columns.Clear();
        this.gvDestination.Columns.Add("Zip", this.gvDestination.Width - 20, ContentAlignment.MiddleLeft);
      }
    }

    private void populateSourceGrid()
    {
      if (this.frmDataContext.InTransitMode == ZipCountyStateCtrl.EnumInTransitMode.Default)
        return;
      if (this.frmDataContext.InTransitMode == ZipCountyStateCtrl.EnumInTransitMode.Initialize || this.frmDataContext.InTransitItems == null)
      {
        this.gvSource.Items.Clear();
        this.gvSource.ClearSort();
        foreach (ZipCountyState src in this.srcList)
        {
          if (src == null)
            break;
          GVItem gvItem = new GVItem();
          if (this.IsCounty)
          {
            gvItem.SubItems.Add((object) src.County);
          }
          else
          {
            gvItem.SubItems.Add((object) src.County);
            gvItem.SubItems.Add((object) src.City);
            gvItem.SubItems.Add((object) src.Zip);
          }
          gvItem.Tag = (object) src;
          this.gvSource.Items.Add(gvItem);
        }
      }
      else if (this.frmDataContext.InTransitMode == ZipCountyStateCtrl.EnumInTransitMode.Add)
      {
        foreach (GVItem gvItem in this.gvSource.Items.ToList<GVItem>().Where<GVItem>((Func<GVItem, bool>) (t =>
        {
          ZipCountyState tag = t.Tag as ZipCountyState;
          return this.frmDataContext.InTransitItems.Any<ZipCountyState>((Func<ZipCountyState, bool>) (a => a == tag));
        })).Select<GVItem, GVItem>((Func<GVItem, GVItem>) (t => t)).ToList<GVItem>())
          this.gvSource.Items.Remove(gvItem);
      }
      else
      {
        if (this.frmDataContext.InTransitMode != ZipCountyStateCtrl.EnumInTransitMode.Remove)
          return;
        int index = -1;
        foreach (ZipCountyState inTransitItem in this.frmDataContext.InTransitItems)
        {
          GVItem gvItem = new GVItem();
          if (this.IsCounty)
          {
            gvItem.SubItems.Add((object) inTransitItem.County);
          }
          else
          {
            gvItem.SubItems.Add((object) inTransitItem.County);
            gvItem.SubItems.Add((object) inTransitItem.City);
            gvItem.SubItems.Add((object) inTransitItem.Zip);
          }
          gvItem.Tag = (object) inTransitItem;
          index = this.gvSource.Items.Add(gvItem);
        }
        if (index <= -1)
          return;
        this.gvSource.EnsureVisible(index);
      }
    }

    private void populateDestinationGrid()
    {
      if (this.frmDataContext.InTransitMode != ZipCountyStateCtrl.EnumInTransitMode.Default)
      {
        this.gvDestination.Items.Clear();
        foreach (ZipCountyState zipCountyState in this.destList.GroupBy<ZipCountyState, string>(!this.IsCounty ? (Func<ZipCountyState, string>) (t => t.Zip) : (Func<ZipCountyState, string>) (t => t.County)).Select<IGrouping<string, ZipCountyState>, ZipCountyState>((Func<IGrouping<string, ZipCountyState>, ZipCountyState>) (g => g.First<ZipCountyState>())).ToList<ZipCountyState>())
        {
          if (zipCountyState == null)
            return;
          GVItem gvItem = new GVItem();
          if (this.IsCounty)
            gvItem.SubItems.Add((object) zipCountyState.County);
          else
            gvItem.SubItems.Add((object) zipCountyState.Zip);
          gvItem.Tag = (object) zipCountyState;
          this.gvDestination.Items.Add(gvItem);
        }
      }
      if (this.frmDataContext.InTransitMode != ZipCountyStateCtrl.EnumInTransitMode.ExistingValue || this.gvDestination.Items.Count <= 0)
        return;
      this.gvDestination.Items[0].Selected = true;
    }

    private void populateListBasedOnCollection()
    {
      if (this.cmbStates.Items.Count == 0)
        this.populateStatesControl();
      this.stdAdd.Enabled = this.stdRemove.Enabled = true;
      this.populateSourceGrid();
      this.populateDestinationGrid();
      this.gvDestination.Refresh();
      this.gvSource.Refresh();
      this.lblDestination.Text = string.Format("Selected ({0})", (object) this.gvDestination.Items.Count);
      this.GetSelectedItem();
      if (!this.gvSource.Items.Any<GVItem>())
        this.stdAdd.Enabled = false;
      if (this.gvDestination.Items.Any<GVItem>())
        return;
      this.stdRemove.Enabled = false;
    }

    private string GetSelectedItem()
    {
      this.zipCollection.Clear();
      string selectedItem = string.Empty;
      if (!this.destList.Any<ZipCountyState>())
        return selectedItem;
      foreach (ZipCountyState dest in this.destList)
      {
        string str = this.IsCounty ? dest.County : dest.Zip;
        if (!this.zipCollection.Contains(str))
        {
          selectedItem = selectedItem + str + ",";
          this.zipCollection.Add(str);
        }
      }
      return selectedItem.Substring(0, selectedItem.Length - 1);
    }

    private void ZipCountyStateCtrl_Load(object sender, EventArgs e)
    {
      this.frmDataContext.SetContext(ZipCountyStateCtrl.EnumInTransitMode.Default);
      this.populateListBasedOnCollection();
    }

    public List<ZipCountyState> SourceList
    {
      get => this.srcList;
      set
      {
        this.srcList.Clear();
        this.srcList.AddRange((IEnumerable<ZipCountyState>) value);
      }
    }

    public List<ZipCountyState> DestinationList
    {
      get => this.destList;
      set => this.destList.AddRange((IEnumerable<ZipCountyState>) value);
    }

    public List<string> States
    {
      get => this.states;
      set
      {
        this.states.AddRange((IEnumerable<string>) value);
        this.populateStatesControl(this.states);
      }
    }

    public string ValuesExisting
    {
      get => this.valuesExisting;
      set
      {
        this.valuesExisting = value;
        if (string.IsNullOrEmpty(value))
          return;
        this.populateDestListWithExisting(value);
        this.handleStateComboWithExisting();
        this.handleCountyComboWithExisting();
        this.frmDataContext.SetContext(ZipCountyStateCtrl.EnumInTransitMode.ExistingValue);
        this.populateListBasedOnCollection();
      }
    }

    public List<string> ZipCollection
    {
      get => this.zipCollection.ToList<string>();
      set
      {
        this.zipCollection.Clear();
        if (value == null)
          return;
        foreach (string str in value)
        {
          if (!this.zipCollection.Contains(str))
            this.zipCollection.Add(str);
        }
      }
    }

    public List<string> CountyCollection
    {
      get => this.countyCollection;
      set
      {
        this.countyCollection.Clear();
        if (value == null)
          return;
        this.countyCollection.AddRange((IEnumerable<string>) value);
      }
    }

    public int SelectedStateIndex
    {
      get => this.selectedStateIndex;
      set
      {
        this.selectedStateIndex = value;
        this.populateStatesControl();
        this.cmbStates.SelectedIndex = value < 0 ? 0 : value;
        this.populateListBasedOnState(value);
      }
    }

    private void populateStatesControl(List<string> externalStates = null)
    {
      if (externalStates != null)
      {
        this.cmbStates.Items.Clear();
        for (int index = 0; index < externalStates.Count; ++index)
        {
          string externalState = externalStates[index];
          this.cmbStates.Items.Add((object) new ZipCountyStateCtrl.StateComboItem()
          {
            Code = (index + 1),
            Abbr = externalState,
            Name = ""
          });
        }
      }
      else
      {
        if (this.cmbStates.Items.Count != 0)
          return;
        this.cmbStates.Items.Add((object) new ZipCountyStateCtrl.StateComboItem()
        {
          Code = -1
        });
        foreach (USPS.State state in USPS.States)
          this.cmbStates.Items.Add((object) new ZipCountyStateCtrl.StateComboItem()
          {
            Code = (int) state.Code,
            Name = state.Name,
            Abbr = state.Abbrev
          });
        this.cmbStates.SelectedIndex = 0;
      }
    }

    private void populateCountyControl()
    {
      int selectedIndex = this.cmbStates.SelectedIndex;
      this.cmbCounty.Items.Clear();
      if (selectedIndex > 0)
      {
        this.frmDataContext.RetrieveZipCountyByState(((ZipCountyStateCtrl.StateComboItem) this.cmbStates.Items[selectedIndex]).Abbr);
        List<ZipCountyState> list = this.frmDataContext.ListZipCountyState.GroupBy<ZipCountyState, string>((Func<ZipCountyState, string>) (z => z.County)).Select<IGrouping<string, ZipCountyState>, ZipCountyState>((Func<IGrouping<string, ZipCountyState>, ZipCountyState>) (g => g.First<ZipCountyState>())).OrderBy<ZipCountyState, string>((Func<ZipCountyState, string>) (g => g.County)).ToList<ZipCountyState>();
        this.cmbCounty.Items.Add((object) "Please select");
        foreach (ZipCountyState zipCountyState in list)
          this.cmbCounty.Items.Add((object) zipCountyState.County);
        this.cmbCounty.SelectedIndex = 0;
        this.cmbCounty.Enabled = true;
      }
      else
      {
        this.cmbCounty.Items.Add((object) "Please select a state");
        this.cmbCounty.Enabled = false;
      }
      this.cmbCounty.Refresh();
    }

    private bool compareByZip(ZipCountyState z1, ZipCountyState z2) => z1.Zip.Equals(z2.Zip);

    private bool compareByCounty(ZipCountyState z1, ZipCountyState z2)
    {
      return z1.County.Equals(z2.County);
    }

    private HashSet<ZipCountyState> removeSourceBasedByGroup(
      string stateAbbr,
      Func<ZipCountyState, ZipCountyState, bool> comparer,
      string[] zip,
      ZipCountyState[] selectedZcs = null)
    {
      if (!this.selectedGroupZcs.ContainsKey(stateAbbr))
        this.selectedGroupZcs.Add(stateAbbr, new List<ZipCountyState>());
      HashSet<ZipCountyState> zipCountyStateSet = new HashSet<ZipCountyState>();
      if (selectedZcs != null)
      {
        foreach (ZipCountyState selectedZc in selectedZcs)
        {
          ZipCountyState z = selectedZc;
          if (!zipCountyStateSet.Contains(z))
          {
            this.selectedGroupZcs[stateAbbr].Add(z);
            zipCountyStateSet.Add(z);
            this.srcList.Remove(z);
          }
          foreach (ZipCountyState zipCountyState in this.srcList.Where<ZipCountyState>((Func<ZipCountyState, bool>) (i => comparer(i, z))).ToList<ZipCountyState>())
          {
            if (!zipCountyStateSet.Contains(zipCountyState))
            {
              this.selectedGroupZcs[stateAbbr].Add(zipCountyState);
              zipCountyStateSet.Add(zipCountyState);
              this.srcList.Remove(zipCountyState);
            }
          }
        }
      }
      return zipCountyStateSet;
    }

    private List<ZipCountyState> reinstateSourceByGroup(List<ZipCountyState> selectedZcs)
    {
      string selStAbbr = this.getSelectedState();
      List<ZipCountyState> zipCountyStateList = new List<ZipCountyState>();
      if (this.IsCounty)
      {
        foreach (ZipCountyState zipCountyState in selectedZcs.GroupBy(z => new
        {
          State = z.State,
          County = z.County
        }).Select<IGrouping<\u003C\u003Ef__AnonymousType4<string, string>, ZipCountyState>, ZipCountyState>(grp => grp.First<ZipCountyState>()).ToList<ZipCountyState>())
        {
          ZipCountyState z = zipCountyState;
          if (string.IsNullOrEmpty(z.State))
          {
            if (!string.IsNullOrEmpty(selStAbbr))
            {
              List<ZipCountyState> list = this.frmDataContext.ListZipCountyState.Where<ZipCountyState>((Func<ZipCountyState, bool>) (t => t.County.Equals(z.County) && t.State.Equals(selStAbbr))).GroupBy(f => new
              {
                County = f.County
              }).Select<IGrouping<\u003C\u003Ef__AnonymousType5<string>, ZipCountyState>, ZipCountyState>(grp => grp.First<ZipCountyState>()).ToList<ZipCountyState>();
              zipCountyStateList.AddRange((IEnumerable<ZipCountyState>) list);
            }
          }
          else if (z.State.Equals(selStAbbr))
          {
            List<ZipCountyState> list = this.frmDataContext.ListZipCountyState.Where<ZipCountyState>((Func<ZipCountyState, bool>) (f => f.State.Equals(selStAbbr) && f.County.Equals(z.County))).GroupBy(f => new
            {
              County = f.County
            }).Select<IGrouping<\u003C\u003Ef__AnonymousType5<string>, ZipCountyState>, ZipCountyState>(grp => grp.First<ZipCountyState>()).ToList<ZipCountyState>();
            this.srcList.AddRange((IEnumerable<ZipCountyState>) list);
            zipCountyStateList.AddRange((IEnumerable<ZipCountyState>) list);
          }
        }
      }
      else
      {
        string selCounty = this.getSelectedCounty();
        foreach (ZipCountyState zipCountyState1 in selectedZcs.GroupBy(z => new
        {
          Zip = z.Zip
        }).Select<IGrouping<\u003C\u003Ef__AnonymousType6<string>, ZipCountyState>, ZipCountyState>(grp => grp.First<ZipCountyState>()).ToList<ZipCountyState>())
        {
          ZipCountyState z = zipCountyState1;
          foreach (ZipCountyState zipCountyState2 in this.frmDataContext.ListZipCountyState.Where<ZipCountyState>((Func<ZipCountyState, bool>) (f =>
          {
            if (!f.State.Equals(selStAbbr) || !f.Zip.Equals(z.Zip))
              return false;
            return string.IsNullOrEmpty(selCounty) || f.County.Equals(selCounty);
          })).ToList<ZipCountyState>())
          {
            this.srcList.Add(zipCountyState2);
            zipCountyStateList.Add(zipCountyState2);
          }
        }
      }
      return zipCountyStateList;
    }

    private void populateDestListWithExisting(string values)
    {
      string[] strArray = values.Split(',');
      HashSet<string> stringSet = new HashSet<string>();
      foreach (string str in strArray)
      {
        string val = str;
        if (!stringSet.Contains(val))
        {
          stringSet.Add(val);
          ZipCountyState zipCountyState = (ZipCountyState) null;
          if (this.IsCounty)
          {
            zipCountyState = new ZipCountyState()
            {
              County = val
            };
          }
          else
          {
            ZipCodeInfo zipInfoAt = ZipCodeUtils.GetZipInfoAt(val);
            if (zipInfoAt == null)
            {
              List<ZipCodeInfo> list = ((IEnumerable<ZipcodeInfoUserDefined>) this.session.ConfigurationManager.GetZipcodeUserDefined((string) null)).Where<ZipcodeInfoUserDefined>((Func<ZipcodeInfoUserDefined, bool>) (cz => cz.Zipcode.Equals(val))).Select<ZipcodeInfoUserDefined, ZipCodeInfo>((Func<ZipcodeInfoUserDefined, ZipCodeInfo>) (cz => cz.ZipInfo)).ToList<ZipCodeInfo>();
              if (list.Count > 0)
                zipInfoAt = list[0];
            }
            if (zipInfoAt != null)
              zipCountyState = new ZipCountyState()
              {
                State = zipInfoAt.State,
                Zip = val,
                City = zipInfoAt.City,
                County = zipInfoAt.County
              };
          }
          if (zipCountyState != null)
            this.destList.Add(zipCountyState);
        }
      }
    }

    private void handleStateComboWithExisting()
    {
      if (this.destList.Count <= 0)
        return;
      ZipCountyState zcs = this.destList[0];
      string key = string.Empty;
      if (this.IsCounty)
      {
        foreach (USPS.State state in USPS.States)
        {
          this.frmDataContext.RetrieveZipCountyByState(state.Abbrev);
          ZipCountyState zipCountyState = this.frmDataContext.ListZipCountyState.Where<ZipCountyState>((Func<ZipCountyState, bool>) (t => t.County.Equals(zcs.County))).FirstOrDefault<ZipCountyState>();
          if (zipCountyState != null)
          {
            key = zipCountyState.State;
            break;
          }
        }
      }
      else
        key = zcs.State;
      if (string.IsNullOrEmpty(key))
        return;
      this.cmbStates.SelectedIndex = (int) USPS.StateCodes[(object) key];
    }

    private void handleCountyComboWithExisting()
    {
      if (this.destList.Count <= 0)
        return;
      this.cmbCounty.SelectedItem = (object) this.destList[0].County;
    }

    private string getSelectedState()
    {
      int selectedIndex = this.cmbStates.SelectedIndex;
      return selectedIndex > 0 ? ((ZipCountyStateCtrl.StateComboItem) this.cmbStates.Items[selectedIndex]).Abbr : string.Empty;
    }

    private string getSelectedCounty()
    {
      int selectedIndex = this.cmbCounty.SelectedIndex;
      return selectedIndex > 0 ? (string) this.cmbCounty.Items[selectedIndex] : string.Empty;
    }

    private void stdAdd_Click(object sender, EventArgs e)
    {
      if (this.gvSource.SelectedItems.Count <= 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "No item Selected", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        List<string> stringList = new List<string>();
        List<ZipCountyState> zipCountyStateList = new List<ZipCountyState>();
        foreach (GVItem selectedItem in this.gvSource.SelectedItems)
        {
          ZipCountyState tag = (ZipCountyState) selectedItem.Tag;
          this.destList.Add(tag);
          stringList.Add(tag.Zip);
          zipCountyStateList.Add(tag);
        }
        ZipCountyStateCtrl.StateComboItem selectedItem1 = this.cmbStates.SelectedItem as ZipCountyStateCtrl.StateComboItem;
        HashSet<ZipCountyState> source = (HashSet<ZipCountyState>) null;
        if (selectedItem1 != null)
          source = !this.IsCounty ? this.removeSourceBasedByGroup(selectedItem1.Abbr, new Func<ZipCountyState, ZipCountyState, bool>(this.compareByZip), stringList.ToArray(), zipCountyStateList.ToArray()) : this.removeSourceBasedByGroup(selectedItem1.Abbr, new Func<ZipCountyState, ZipCountyState, bool>(this.compareByCounty), stringList.ToArray(), zipCountyStateList.ToArray());
        this.frmDataContext.SetContext(ZipCountyStateCtrl.EnumInTransitMode.Add, source.ToList<ZipCountyState>());
        this.populateListBasedOnCollection();
      }
    }

    private void standardIconButton1_Click(object sender, EventArgs e)
    {
      if (this.gvDestination.SelectedItems.Count <= 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "No item Selected", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        List<ZipCountyState> selectedZcs = new List<ZipCountyState>();
        foreach (GVItem selectedItem in this.gvDestination.SelectedItems)
        {
          ZipCountyState zcs = (ZipCountyState) selectedItem.Tag;
          selectedZcs.Add(zcs);
          foreach (ZipCountyState zipCountyState in this.destList.Where<ZipCountyState>(!this.IsCounty ? (Func<ZipCountyState, bool>) (z => z.Zip.Equals(zcs.Zip)) : (Func<ZipCountyState, bool>) (z => z.County.Equals(zcs.County))).ToList<ZipCountyState>())
            this.destList.Remove(zipCountyState);
        }
        this.frmDataContext.SetContext(ZipCountyStateCtrl.EnumInTransitMode.Remove, this.reinstateSourceByGroup(selectedZcs));
        this.populateListBasedOnCollection();
      }
    }

    private void cmbStates_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.selectedStateIndex = this.cmbStates.SelectedIndex;
      this.populateCountyControl();
      this.populateListBasedOnState(this.selectedStateIndex);
      this.frmDataContext.SetContext(ZipCountyStateCtrl.EnumInTransitMode.Initialize);
      this.populateListBasedOnCollection();
    }

    private void cmbCounty_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.populateListBasedOnCounty();
      this.frmDataContext.SetContext(ZipCountyStateCtrl.EnumInTransitMode.Initialize);
      this.populateListBasedOnCollection();
    }

    private void populateListBasedOnState(string stateAbbr)
    {
      List<ZipCountyState> zipCountyStateList = new List<ZipCountyState>();
      if (!string.IsNullOrEmpty(stateAbbr))
      {
        try
        {
          string selCounty = this.IsCounty ? string.Empty : this.getSelectedCounty();
          this.frmDataContext.RetrieveZipCountyByState(stateAbbr);
          Func<ZipCountyState, ZipCountyState, bool> comparer = !this.IsCounty ? new Func<ZipCountyState, ZipCountyState, bool>(this.compareByZip) : new Func<ZipCountyState, ZipCountyState, bool>(this.compareByCounty);
          List<ZipCountyState> list = this.frmDataContext.ListZipCountyState.Where<ZipCountyState>((Func<ZipCountyState, bool>) (x =>
          {
            if (this.destList.Any<ZipCountyState>((Func<ZipCountyState, bool>) (x2 => comparer(x, x2))))
              return false;
            return string.IsNullOrEmpty(selCounty) || x.County.Equals(selCounty);
          })).ToList<ZipCountyState>();
          if (this.IsCounty)
            list = list.GroupBy(x => new
            {
              County = x.County
            }).Select<IGrouping<\u003C\u003Ef__AnonymousType5<string>, ZipCountyState>, ZipCountyState>(grp => grp.First<ZipCountyState>()).OrderBy<ZipCountyState, string>((Func<ZipCountyState, string>) (g => g.County)).ToList<ZipCountyState>();
          foreach (ZipCountyState zipCountyState in list)
            zipCountyStateList.Add(zipCountyState);
        }
        catch (Exception ex)
        {
          return;
        }
      }
      this.SourceList = zipCountyStateList;
    }

    private void populateListBasedOnState(int selStateIndex)
    {
      if (selStateIndex > 0)
      {
        try
        {
          this.populateListBasedOnState(((ZipCountyStateCtrl.StateComboItem) this.cmbStates.Items[selStateIndex]).Abbr);
        }
        catch (Exception ex)
        {
        }
      }
      else
        this.SourceList = new List<ZipCountyState>();
    }

    private void populateListBasedOnCounty()
    {
      string selCounty = this.getSelectedCounty();
      string selectedState = this.getSelectedState();
      List<ZipCountyState> source = new List<ZipCountyState>();
      if (!string.IsNullOrEmpty(selectedState))
      {
        this.frmDataContext.RetrieveZipCountyByState(selectedState);
        if (!string.IsNullOrEmpty(selCounty))
        {
          foreach (ZipCountyState zipCountyState in this.frmDataContext.ListZipCountyState.Where<ZipCountyState>((Func<ZipCountyState, bool>) (z => z.County.Equals(selCounty))).GroupBy(z => new
          {
            County = z.County,
            City = z.City,
            Zip = z.Zip
          }).Select<IGrouping<\u003C\u003Ef__AnonymousType7<string, string, string>, ZipCountyState>, ZipCountyState>(grp => grp.First<ZipCountyState>()).ToList<ZipCountyState>())
            source.Add(zipCountyState);
        }
        else
        {
          foreach (ZipCountyState zipCountyState in this.frmDataContext.ListZipCountyState)
            source.Add(zipCountyState);
        }
      }
      this.SourceList = source.Where<ZipCountyState>((Func<ZipCountyState, bool>) (x =>
      {
        if (this.destList.Any<ZipCountyState>((Func<ZipCountyState, bool>) (x2 => x2.Zip.Equals(x.Zip))))
          return false;
        return string.IsNullOrEmpty(selCounty) || x.County.Equals(selCounty);
      })).ToList<ZipCountyState>();
    }

    private void gvSource_DoubleClick(object sender, EventArgs e) => this.stdAdd_Click(sender, e);

    private void gvDestination_DoubleClick(object sender, EventArgs e)
    {
      this.standardIconButton1_Click(sender, e);
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
      this.SourceGB = new GroupBox();
      this.gvSource = new GridView();
      this.DestinationGB = new GroupBox();
      this.gvDestination = new GridView();
      this.lblSource = new Label();
      this.lblDestination = new Label();
      this.stdRemove = new StandardIconButton();
      this.stdAdd = new StandardIconButton();
      this.label1 = new Label();
      this.cmbStates = new ComboBox();
      this.pnlControls = new Panel();
      this.lblCounty = new Label();
      this.cmbCounty = new ComboBox();
      this.SourceGB.SuspendLayout();
      this.DestinationGB.SuspendLayout();
      ((ISupportInitialize) this.stdRemove).BeginInit();
      ((ISupportInitialize) this.stdAdd).BeginInit();
      this.pnlControls.SuspendLayout();
      this.SuspendLayout();
      this.SourceGB.Controls.Add((Control) this.gvSource);
      this.SourceGB.Location = new Point(3, 19);
      this.SourceGB.Name = "SourceGB";
      this.SourceGB.Size = new Size(226, 194);
      this.SourceGB.TabIndex = 2;
      this.SourceGB.TabStop = false;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "County";
      gvColumn1.Text = "County";
      gvColumn1.Width = 75;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "City";
      gvColumn2.Text = "City";
      gvColumn2.Width = 75;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Zip";
      gvColumn3.SpringToFit = true;
      gvColumn3.Text = "Zip";
      gvColumn3.Width = 61;
      this.gvSource.Columns.AddRange(new GVColumn[3]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3
      });
      this.gvSource.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvSource.Location = new Point(7, 13);
      this.gvSource.Name = "gvSource";
      this.gvSource.Size = new Size(213, 173);
      this.gvSource.TabIndex = 0;
      this.gvSource.DoubleClick += new EventHandler(this.gvSource_DoubleClick);
      this.DestinationGB.Controls.Add((Control) this.gvDestination);
      this.DestinationGB.Location = new Point(265, 19);
      this.DestinationGB.Name = "DestinationGB";
      this.DestinationGB.Size = new Size(226, 195);
      this.DestinationGB.TabIndex = 3;
      this.DestinationGB.TabStop = false;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Zip";
      gvColumn4.Text = "Zip";
      gvColumn4.Width = 50;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "City";
      gvColumn5.Text = "City";
      gvColumn5.Width = 75;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "County";
      gvColumn6.SpringToFit = true;
      gvColumn6.Text = "County";
      gvColumn6.Width = 86;
      this.gvDestination.Columns.AddRange(new GVColumn[3]
      {
        gvColumn4,
        gvColumn5,
        gvColumn6
      });
      this.gvDestination.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvDestination.Location = new Point(7, 13);
      this.gvDestination.Name = "gvDestination";
      this.gvDestination.Size = new Size(213, 174);
      this.gvDestination.TabIndex = 0;
      this.gvDestination.DoubleClick += new EventHandler(this.gvDestination_DoubleClick);
      this.lblSource.AutoSize = true;
      this.lblSource.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblSource.Location = new Point(-1, 5);
      this.lblSource.Name = "lblSource";
      this.lblSource.Size = new Size(87, 13);
      this.lblSource.TabIndex = 1;
      this.lblSource.Text = "Search Result";
      this.lblDestination.AutoSize = true;
      this.lblDestination.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblDestination.Location = new Point(263, 5);
      this.lblDestination.Name = "lblDestination";
      this.lblDestination.Size = new Size(61, 13);
      this.lblDestination.TabIndex = 2;
      this.lblDestination.Text = "Selected ";
      this.stdRemove.BackColor = Color.Transparent;
      this.stdRemove.Location = new Point(234, 128);
      this.stdRemove.MouseDownImage = (Image) null;
      this.stdRemove.Name = "stdRemove";
      this.stdRemove.Size = new Size(25, 25);
      this.stdRemove.SizeMode = PictureBoxSizeMode.AutoSize;
      this.stdRemove.StandardButtonType = StandardIconButton.ButtonType.LeftArrowButton;
      this.stdRemove.TabIndex = 4;
      this.stdRemove.TabStop = false;
      this.stdRemove.Click += new EventHandler(this.standardIconButton1_Click);
      this.stdAdd.BackColor = Color.Transparent;
      this.stdAdd.InitialImage = (Image) Resources.arrow_forward;
      this.stdAdd.Location = new Point(234, 98);
      this.stdAdd.MouseDownImage = (Image) null;
      this.stdAdd.Name = "stdAdd";
      this.stdAdd.Size = new Size(25, 25);
      this.stdAdd.SizeMode = PictureBoxSizeMode.AutoSize;
      this.stdAdd.StandardButtonType = StandardIconButton.ButtonType.RightArrowButton;
      this.stdAdd.TabIndex = 3;
      this.stdAdd.TabStop = false;
      this.stdAdd.WaitOnLoad = true;
      this.stdAdd.Click += new EventHandler(this.stdAdd_Click);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(-1, 5);
      this.label1.Name = "label1";
      this.label1.Size = new Size(74, 13);
      this.label1.TabIndex = 5;
      this.label1.Text = "Select a State";
      this.cmbStates.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbStates.FormattingEnabled = true;
      this.cmbStates.Location = new Point(106, 1);
      this.cmbStates.Name = "cmbStates";
      this.cmbStates.Size = new Size(226, 21);
      this.cmbStates.TabIndex = 0;
      this.cmbStates.SelectedIndexChanged += new EventHandler(this.cmbStates_SelectedIndexChanged);
      this.pnlControls.Controls.Add((Control) this.lblDestination);
      this.pnlControls.Controls.Add((Control) this.SourceGB);
      this.pnlControls.Controls.Add((Control) this.DestinationGB);
      this.pnlControls.Controls.Add((Control) this.lblSource);
      this.pnlControls.Controls.Add((Control) this.stdRemove);
      this.pnlControls.Controls.Add((Control) this.stdAdd);
      this.pnlControls.Location = new Point(0, 60);
      this.pnlControls.Name = "pnlControls";
      this.pnlControls.Size = new Size(500, 230);
      this.pnlControls.TabIndex = 4;
      this.lblCounty.AutoSize = true;
      this.lblCounty.Location = new Point(-1, 34);
      this.lblCounty.Name = "lblCounty";
      this.lblCounty.Size = new Size(40, 13);
      this.lblCounty.TabIndex = 7;
      this.lblCounty.Text = "County";
      this.cmbCounty.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbCounty.FormattingEnabled = true;
      this.cmbCounty.Location = new Point(106, 31);
      this.cmbCounty.Name = "cmbCounty";
      this.cmbCounty.Size = new Size(226, 21);
      this.cmbCounty.TabIndex = 1;
      this.cmbCounty.SelectedIndexChanged += new EventHandler(this.cmbCounty_SelectedIndexChanged);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.cmbCounty);
      this.Controls.Add((Control) this.lblCounty);
      this.Controls.Add((Control) this.pnlControls);
      this.Controls.Add((Control) this.cmbStates);
      this.Controls.Add((Control) this.label1);
      this.Name = nameof (ZipCountyStateCtrl);
      this.Size = new Size(500, 300);
      this.Load += new EventHandler(this.ZipCountyStateCtrl_Load);
      this.SourceGB.ResumeLayout(false);
      this.DestinationGB.ResumeLayout(false);
      ((ISupportInitialize) this.stdRemove).EndInit();
      ((ISupportInitialize) this.stdAdd).EndInit();
      this.pnlControls.ResumeLayout(false);
      this.pnlControls.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private class FormDataContext
    {
      private string currentStateAbbr;
      private Sessions.Session session;

      public List<ZipCountyState> ListZipCountyState { get; set; }

      public List<ZipCountyState> InTransitItems { get; set; }

      public ZipCountyStateCtrl.EnumInTransitMode InTransitMode { get; set; }

      public FormDataContext(Sessions.Session ses)
      {
        this.ListZipCountyState = new List<ZipCountyState>();
        this.session = ses;
      }

      public void RetrieveZipCountyByState(string stateAbbr)
      {
        try
        {
          if (string.IsNullOrEmpty(stateAbbr))
            return;
          if (this.currentStateAbbr != stateAbbr)
          {
            this.currentStateAbbr = stateAbbr;
            this.getMultipleStateInfo(stateAbbr);
          }
          else
          {
            if (string.IsNullOrEmpty(this.currentStateAbbr) || this.ListZipCountyState.Count != 0)
              return;
            this.getMultipleStateInfo(this.currentStateAbbr);
          }
        }
        catch (Exception ex)
        {
        }
      }

      public void SetContext(
        ZipCountyStateCtrl.EnumInTransitMode transitMode,
        List<ZipCountyState> inTransitData = null)
      {
        this.InTransitMode = transitMode;
        this.InTransitItems = inTransitData;
      }

      private void getMultipleStateInfo(string st)
      {
        this.ListZipCountyState.Clear();
        foreach (StateInfo stateInfo in ZipCodeUtils.GetMultipleStateInfoAt(st))
          this.ListZipCountyState.Add(new ZipCountyState()
          {
            State = st,
            County = stateInfo.County,
            Zip = stateInfo.Zipcode,
            City = stateInfo.City
          });
        foreach (ZipcodeInfoUserDefined zipcodeInfoUserDefined in ((IEnumerable<ZipcodeInfoUserDefined>) this.session.ConfigurationManager.GetZipcodeUserDefined((string) null)).Where<ZipcodeInfoUserDefined>((Func<ZipcodeInfoUserDefined, bool>) (cz => cz.ZipInfo.State.Equals(st))).ToList<ZipcodeInfoUserDefined>())
          this.ListZipCountyState.Add(new ZipCountyState()
          {
            State = zipcodeInfoUserDefined.ZipInfo.State,
            County = zipcodeInfoUserDefined.ZipInfo.County,
            Zip = zipcodeInfoUserDefined.Zipcode,
            City = zipcodeInfoUserDefined.ZipInfo.City
          });
      }
    }

    private class StateComboItem
    {
      public int Code { get; set; }

      public string Name { get; set; }

      public string Abbr { get; set; }

      public override string ToString()
      {
        return this.Code == -1 ? "Please select" : this.Abbr + " - " + this.Name;
      }
    }

    private enum EnumInTransitMode
    {
      Default,
      Initialize,
      Add,
      Remove,
      ExistingValue,
    }
  }
}
