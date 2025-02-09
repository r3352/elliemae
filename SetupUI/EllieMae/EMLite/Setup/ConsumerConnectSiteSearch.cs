// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ConsumerConnectSiteSearch
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.LoanUtils.ConsumerConnect;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class ConsumerConnectSiteSearch : Form
  {
    private Sessions.Session session;
    private IContainer components;
    private int currentOffsetIntoUrls;
    private SiteAccessor siteAccessor;
    private bool formCanceled = true;
    private static int itemsPerPage = 25;
    private static int itemsPerSearchBatch = 100;
    private Dictionary<int, List<EllieMae.EMLite.LoanUtils.ConsumerConnect.Site>> batchSearchResults = new Dictionary<int, List<EllieMae.EMLite.LoanUtils.ConsumerConnect.Site>>();
    private static string[] prefixesToRemove = new string[2]
    {
      "http://",
      "https://"
    };
    private GroupContainer gcUrls;
    private Label label2;
    private GridView urlsView;
    private Button cancelButton;
    private Panel panel1;
    private Button btnClear;
    private Button btnSearch;
    private TextBox searchFilterBox;
    private Label lblObject;
    private Button selectButton;
    private PageListNavigator navigator;
    private Panel panel2;
    private Label label1;
    private GroupContainer groupContainer1;
    private Label label3;

    public ConsumerConnectSiteSearch(Sessions.Session session, string query)
    {
      this.session = session;
      this.InitializeComponent();
      this.Text = "Consumer Connect Site URL";
      this.urlsView.AllowMultiselect = false;
      this.urlsView.ItemDoubleClick += new GVItemEventHandler(this.urlsView_ItemDoubleClick);
      this.urlsView.SelectedIndexChanged += new EventHandler(this.urlsView_SelectedIndexChanged);
      this.siteAccessor = new SiteAccessor();
      this.searchFilterBox.Text = query.Trim();
      this.searchFilterBox.KeyPress += new KeyPressEventHandler(this.searchFilterBox_KeyPress);
      this.navigator.ItemsPerPage = ConsumerConnectSiteSearch.itemsPerPage;
      this.navigator.PageChangedEvent += new PageListNavigator.PageChangedEventHandler(this.eventPageChanged);
      this.btnSearch_Click((object) null, (EventArgs) null);
    }

    public int getNumQueryResults() => this.navigator.NumberOfItems;

    public EllieMae.EMLite.LoanUtils.ConsumerConnect.Site getSelectedSite()
    {
      return this.navigator.NumberOfItems == 1 ? this.batchSearchResults[0][0] : (EllieMae.EMLite.LoanUtils.ConsumerConnect.Site) this.urlsView.SelectedItems[0].Tag;
    }

    public bool userClickedCancel() => this.formCanceled;

    private void populatePage(List<EllieMae.EMLite.LoanUtils.ConsumerConnect.Site> sites)
    {
      this.urlsView.Items.Clear();
      foreach (EllieMae.EMLite.LoanUtils.ConsumerConnect.Site site in sites)
        this.urlsView.Items.Add(new GVItem()
        {
          SubItems = {
            (object) CCSiteControl.getCCSiteURL(site)
          },
          Tag = (object) site
        });
    }

    private string getFormattedSearchQuery()
    {
      string s = this.stripLeadingAndTrailingSpecialCharacters(this.searchFilterBox.Text.Trim());
      foreach (string str in ConsumerConnectSiteSearch.prefixesToRemove)
      {
        if (s.IndexOf(str) == 0)
        {
          s = s.Substring(str.Length);
          break;
        }
      }
      return this.stripLeadingAndTrailingSpecialCharacters(s);
    }

    private string stripLeadingAndTrailingSpecialCharacters(string s)
    {
      int startIndex = 0;
      int num = 0;
      string str = s;
      for (int index = 0; index < str.Length && !char.IsLetterOrDigit(str[index]); ++index)
        ++startIndex;
      foreach (char c in s.Reverse<char>())
      {
        if (!char.IsLetterOrDigit(c))
          ++num;
        else
          break;
      }
      return s.Substring(startIndex, Math.Max(0, s.Length - startIndex - num));
    }

    private static int getBatchIndexForOffset(int offset)
    {
      return offset / ConsumerConnectSiteSearch.itemsPerSearchBatch;
    }

    private bool getBatchForSearchAtOffset(int offset, bool currentlyInSearch)
    {
      try
      {
        string formattedSearchQuery = this.getFormattedSearchQuery();
        int batchIndexForOffset = ConsumerConnectSiteSearch.getBatchIndexForOffset(offset);
        int startOffset = batchIndexForOffset * ConsumerConnectSiteSearch.itemsPerSearchBatch;
        CMSPaginatedResponse ccSites = this.siteAccessor.GetCCSites(this.session.SessionObjects, formattedSearchQuery, startOffset);
        if (ccSites != null)
        {
          if (!currentlyInSearch && this.navigator.NumberOfItems != ccSites.totalSiteCount)
            return true;
          this.batchSearchResults[batchIndexForOffset] = ccSites.sites;
          this.navigator.NumberOfItems = ccSites.totalSiteCount;
        }
        else
        {
          this.batchSearchResults[batchIndexForOffset] = new List<EllieMae.EMLite.LoanUtils.ConsumerConnect.Site>();
          this.navigator.NumberOfItems = 0;
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Error occured while retrieving Consumer Connect Sites: " + ex.Message, "Encompass", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.cancelButton_Click((object) null, (EventArgs) null);
      }
      return false;
    }

    private void btnClear_Click(object sender, EventArgs e)
    {
      this.searchFilterBox.Text = "";
      this.urlsView.Items.Clear();
      this.batchSearchResults.Clear();
      this.navigator.NumberOfItems = 0;
    }

    private bool populateAtOffset(int offset, bool currentlyInSearch)
    {
      int batchIndexForOffset = ConsumerConnectSiteSearch.getBatchIndexForOffset(offset);
      if (currentlyInSearch)
        this.getBatchForSearchAtOffset(offset, currentlyInSearch);
      else if (offset < this.navigator.NumberOfItems && !this.batchSearchResults.ContainsKey(batchIndexForOffset) && this.getBatchForSearchAtOffset(offset, currentlyInSearch))
        return true;
      if (this.batchSearchResults.ContainsKey(batchIndexForOffset))
      {
        int index = offset % ConsumerConnectSiteSearch.itemsPerSearchBatch;
        int count1 = this.batchSearchResults[batchIndexForOffset].Count;
        if (count1 > 0)
        {
          int count2 = Math.Min(count1 - index, ConsumerConnectSiteSearch.itemsPerPage);
          this.populatePage(this.batchSearchResults[batchIndexForOffset].GetRange(index, count2));
        }
        else
          this.urlsView.Items.Clear();
      }
      return false;
    }

    private void eventPageChanged(object sender, PageChangedEventArgs e)
    {
      bool flag = false;
      this.navigator.PageChangedEvent -= new PageListNavigator.PageChangedEventHandler(this.eventPageChanged);
      if (e.ItemIndex != -1)
      {
        this.currentOffsetIntoUrls = e.ItemIndex;
        flag = this.populateAtOffset(this.currentOffsetIntoUrls, false);
      }
      this.navigator.PageChangedEvent += new PageListNavigator.PageChangedEventHandler(this.eventPageChanged);
      if (!flag)
        return;
      this.btnSearch_Click((object) null, (EventArgs) null);
    }

    private void btnSearch_Click(object sender, EventArgs e)
    {
      this.btnSearch.Enabled = false;
      this.navigator.PageChangedEvent -= new PageListNavigator.PageChangedEventHandler(this.eventPageChanged);
      Cursor.Current = Cursors.WaitCursor;
      try
      {
        this.navigator.ClearSelection();
        this.urlsView.ClearSort();
        this.batchSearchResults.Clear();
        this.currentOffsetIntoUrls = 0;
        this.populateAtOffset(this.currentOffsetIntoUrls, true);
      }
      finally
      {
        this.btnSearch.Enabled = true;
        this.navigator.PageChangedEvent += new PageListNavigator.PageChangedEventHandler(this.eventPageChanged);
        Cursor.Current = Cursors.Default;
      }
    }

    private void searchFilterBox_TextChanged(object sender, EventArgs e)
    {
      this.searchFilterBox.TextChanged -= new EventHandler(this.searchFilterBox_TextChanged);
      this.searchFilterBox.Text = this.searchFilterBox.Text.Trim();
      this.searchFilterBox.TextChanged += new EventHandler(this.searchFilterBox_TextChanged);
    }

    private void selectButton_Click(object sender, EventArgs e)
    {
      this.formCanceled = false;
      this.Close();
    }

    private void urlsView_ItemDoubleClick(object source, GVItemEventArgs e)
    {
      this.selectButton_Click((object) null, (EventArgs) null);
    }

    private void cancelButton_Click(object sender, EventArgs e)
    {
      this.formCanceled = true;
      this.Close();
    }

    private void urlsView_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.selectButton.Enabled = this.urlsView.SelectedItems != null && this.urlsView.SelectedItems.Count == 1;
    }

    private void searchFilterBox_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar != '\r')
        return;
      this.btnSearch_Click((object) null, (EventArgs) null);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      GVColumn gvColumn = new GVColumn();
      this.gcUrls = new GroupContainer();
      this.label2 = new Label();
      this.urlsView = new GridView();
      this.cancelButton = new Button();
      this.panel1 = new Panel();
      this.btnClear = new Button();
      this.btnSearch = new Button();
      this.searchFilterBox = new TextBox();
      this.lblObject = new Label();
      this.selectButton = new Button();
      this.navigator = new PageListNavigator();
      this.panel2 = new Panel();
      this.label1 = new Label();
      this.groupContainer1 = new GroupContainer();
      this.label3 = new Label();
      this.gcUrls.SuspendLayout();
      this.panel1.SuspendLayout();
      this.panel2.SuspendLayout();
      this.groupContainer1.SuspendLayout();
      this.SuspendLayout();
      this.gcUrls.BackgroundImageLayout = ImageLayout.None;
      this.gcUrls.Borders = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.gcUrls.Controls.Add((Control) this.label2);
      this.gcUrls.Controls.Add((Control) this.urlsView);
      this.gcUrls.Controls.Add((Control) this.cancelButton);
      this.gcUrls.Controls.Add((Control) this.panel1);
      this.gcUrls.Controls.Add((Control) this.selectButton);
      this.gcUrls.Controls.Add((Control) this.navigator);
      this.gcUrls.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.gcUrls.HeaderForeColor = SystemColors.ControlText;
      this.gcUrls.Location = new Point(1, 51);
      this.gcUrls.Name = "gcUrls";
      this.gcUrls.Size = new Size(850, 603);
      this.gcUrls.TabIndex = 12;
      this.label2.AutoSize = true;
      this.label2.BackColor = Color.Transparent;
      this.label2.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label2.Location = new Point(5, 8);
      this.label2.Name = "label2";
      this.label2.Size = new Size(65, 14);
      this.label2.TabIndex = 30;
      this.label2.Text = "Select URL";
      this.urlsView.BorderStyle = BorderStyle.None;
      gvColumn.ImageIndex = -1;
      gvColumn.Name = "Column1";
      gvColumn.SortMethod = GVSortMethod.None;
      gvColumn.Tag = (object) "URL";
      gvColumn.Text = "URL";
      gvColumn.Width = 830;
      this.urlsView.Columns.AddRange(new GVColumn[1]
      {
        gvColumn
      });
      this.urlsView.Cursor = Cursors.Default;
      this.urlsView.Dock = DockStyle.Bottom;
      this.urlsView.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.urlsView.Location = new Point(1, 95);
      this.urlsView.Name = "urlsView";
      this.urlsView.Size = new Size(848, 479);
      this.urlsView.SortOption = GVSortOption.None;
      this.urlsView.TabIndex = 0;
      this.cancelButton.BackColor = SystemColors.Control;
      this.cancelButton.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.cancelButton.Location = new Point(753, 578);
      this.cancelButton.Name = "cancelButton";
      this.cancelButton.Size = new Size(78, 21);
      this.cancelButton.TabIndex = 23;
      this.cancelButton.Text = "Cancel";
      this.cancelButton.UseVisualStyleBackColor = true;
      this.cancelButton.Click += new EventHandler(this.cancelButton_Click);
      this.panel1.Controls.Add((Control) this.btnClear);
      this.panel1.Controls.Add((Control) this.btnSearch);
      this.panel1.Controls.Add((Control) this.searchFilterBox);
      this.panel1.Controls.Add((Control) this.lblObject);
      this.panel1.Dock = DockStyle.Fill;
      this.panel1.Location = new Point(1, 25);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(848, 549);
      this.panel1.TabIndex = 22;
      this.btnClear.BackColor = SystemColors.Control;
      this.btnClear.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.btnClear.Location = new Point(467, 25);
      this.btnClear.Name = "btnClear";
      this.btnClear.Size = new Size(78, 21);
      this.btnClear.TabIndex = 21;
      this.btnClear.Text = "Clear";
      this.btnClear.UseVisualStyleBackColor = true;
      this.btnClear.Click += new EventHandler(this.btnClear_Click);
      this.btnSearch.BackColor = SystemColors.Control;
      this.btnSearch.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.btnSearch.Location = new Point(383, 26);
      this.btnSearch.Name = "btnSearch";
      this.btnSearch.Size = new Size(78, 21);
      this.btnSearch.TabIndex = 18;
      this.btnSearch.Text = "Search";
      this.btnSearch.UseVisualStyleBackColor = true;
      this.btnSearch.Click += new EventHandler(this.btnSearch_Click);
      this.searchFilterBox.Font = new Font("Arial", 8.25f);
      this.searchFilterBox.Location = new Point(7, 26);
      this.searchFilterBox.Name = "searchFilterBox";
      this.searchFilterBox.Size = new Size(353, 20);
      this.searchFilterBox.TabIndex = 17;
      this.searchFilterBox.TextChanged += new EventHandler(this.searchFilterBox_TextChanged);
      this.lblObject.AutoSize = true;
      this.lblObject.Font = new Font("Arial", 8.25f);
      this.lblObject.Location = new Point(4, 8);
      this.lblObject.Name = "lblObject";
      this.lblObject.Size = new Size(111, 14);
      this.lblObject.TabIndex = 16;
      this.lblObject.Text = "Enter a text to search";
      this.selectButton.BackColor = SystemColors.Control;
      this.selectButton.Enabled = false;
      this.selectButton.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.selectButton.Location = new Point(669, 578);
      this.selectButton.Name = "selectButton";
      this.selectButton.Size = new Size(78, 21);
      this.selectButton.TabIndex = 22;
      this.selectButton.Text = "Select";
      this.selectButton.UseVisualStyleBackColor = true;
      this.selectButton.Click += new EventHandler(this.selectButton_Click);
      this.navigator.BackColor = Color.Transparent;
      this.navigator.Dock = DockStyle.Bottom;
      this.navigator.Font = new Font("Arial", 8f);
      this.navigator.Location = new Point(1, 574);
      this.navigator.Name = "navigator";
      this.navigator.NumberOfItems = 0;
      this.navigator.Size = new Size(848, 28);
      this.navigator.TabIndex = 5;
      this.panel2.Controls.Add((Control) this.label1);
      this.panel2.ImeMode = ImeMode.On;
      this.panel2.Location = new Point(1, 26);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(850, 27);
      this.panel2.TabIndex = 23;
      this.label1.AutoSize = true;
      this.label1.Font = new Font("Arial", 8.25f);
      this.label1.Location = new Point(4, 8);
      this.label1.Name = "label1";
      this.label1.Size = new Size(231, 14);
      this.label1.TabIndex = 16;
      this.label1.Text = "Select a URL from the list or perform a search.";
      this.groupContainer1.Controls.Add((Control) this.label3);
      this.groupContainer1.Controls.Add((Control) this.panel2);
      this.groupContainer1.Controls.Add((Control) this.gcUrls);
      this.groupContainer1.Dock = DockStyle.Fill;
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(0, 0);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(852, 654);
      this.groupContainer1.TabIndex = 13;
      this.label3.AutoSize = true;
      this.label3.BackColor = Color.Transparent;
      this.label3.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label3.Location = new Point(6, 9);
      this.label3.Name = "label3";
      this.label3.Size = new Size(163, 14);
      this.label3.TabIndex = 31;
      this.label3.Text = "Consumer Connect Site URL";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(852, 654);
      this.Controls.Add((Control) this.groupContainer1);
      this.FormBorderStyle = FormBorderStyle.FixedSingle;
      this.Name = nameof (ConsumerConnectSiteSearch);
      this.gcUrls.ResumeLayout(false);
      this.gcUrls.PerformLayout();
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.panel2.ResumeLayout(false);
      this.panel2.PerformLayout();
      this.groupContainer1.ResumeLayout(false);
      this.groupContainer1.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
