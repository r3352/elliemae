// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.Viewers.PageAnnotationListControl
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.eFolder;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.eFolder.Properties;
using EllieMae.EMLite.eFolder.UI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.eFolder.Viewers
{
  public class PageAnnotationListControl : UserControl
  {
    private ImageAttachment[] attachmentList;
    private IContainer components;
    private GroupContainer gcAnnotations;
    private FlowLayoutPanel pnlToolbar;
    private StandardIconButton btnDelete;
    private StandardIconButton btnNext;
    private StandardIconButton btnPrevious;
    private GridView gvAnnotations;
    private ToolTip toolTip;

    public PageAnnotationListControl() => this.InitializeComponent();

    public void LoadAnnotations(ImageAttachment[] attachmentList)
    {
      this.attachmentList = attachmentList;
      this.applySecurity();
      this.gvAnnotations.Items.Clear();
      List<PageImage> pageImageList = new List<PageImage>();
      foreach (ImageAttachment attachment in attachmentList)
      {
        foreach (PageImage page in attachment.Pages)
          pageImageList.Add(page);
      }
      for (int index = 0; index < pageImageList.Count; ++index)
      {
        foreach (PageAnnotation annotation in pageImageList[index].Annotations)
        {
          if (eFolderUIHelper.IsAnnotationVisible(annotation))
          {
            GVItem gvItem = new GVItem();
            gvItem.Tag = (object) annotation;
            GVSubItem subItem = gvItem.SubItems[0];
            string text = annotation.Text;
            if (text.IndexOf(Environment.NewLine) > 0)
              text = text.Substring(0, text.IndexOf(Environment.NewLine)) + "...";
            if (annotation.Visibility == PageAnnotationVisibilityType.Personal)
              subItem.Value = (object) new ObjectWithImage(text, (Image) Resources.Annotation_Private, 4);
            if (annotation.Visibility == PageAnnotationVisibilityType.Internal)
              subItem.Value = (object) new ObjectWithImage(text, (Image) Resources.Annotation_Internal, 4);
            if (annotation.Visibility == PageAnnotationVisibilityType.Public)
              subItem.Value = (object) new ObjectWithImage(text, (Image) Resources.Annotation_Public, 4);
            subItem.SortValue = (object) annotation.Text;
            gvItem.SubItems[1].Value = (object) annotation.Date.ToString("MM/dd/yy hh:mm tt");
            gvItem.SubItems[2].Value = (object) annotation.AddedBy;
            gvItem.SubItems[3].Value = (object) ("Page " + Convert.ToString(index + 1) + " of " + pageImageList.Count.ToString());
            gvItem.SubItems[4].Value = (object) annotation.Visibility.ToString();
            this.gvAnnotations.Items.Add(gvItem);
          }
        }
      }
    }

    private void applySecurity()
    {
      this.btnDelete.Visible = new eFolderAccessRights(Session.LoanDataMgr, (LogRecordBase) Session.LoanDataMgr.FileAttachments.GetLinkedDocument(this.attachmentList[0].ID)).CanDeleteAnnotations;
    }

    public PageAnnotation GetSelectedAnnotation()
    {
      return this.gvAnnotations.SelectedItems.Count > 0 ? (PageAnnotation) this.gvAnnotations.SelectedItems[0].Tag : (PageAnnotation) null;
    }

    private void gvAnnotations_SelectedIndexChanged(object sender, EventArgs e)
    {
      int count = this.gvAnnotations.SelectedItems.Count;
      this.btnPrevious.Enabled = count > 0;
      this.btnNext.Enabled = count > 0;
      this.btnDelete.Enabled = count > 0;
    }

    private void gvAnnotations_SelectedIndexCommitted(object sender, EventArgs e)
    {
      this.OnSelectedIndexCommitted(EventArgs.Empty);
    }

    private void btnPrevious_Click(object sender, EventArgs e)
    {
      if (this.gvAnnotations.Items.Count < 1 || this.gvAnnotations.SelectedItems.Count < 1)
        return;
      int newIndex = this.gvAnnotations.Items.Count - 1;
      if (this.gvAnnotations.SelectedItems[0].DisplayIndex > 0)
        newIndex = this.gvAnnotations.SelectedItems[0].DisplayIndex - 1;
      this.selectAnnotationRowByIndex(newIndex);
    }

    private void btnNext_Click(object sender, EventArgs e)
    {
      if (this.gvAnnotations.Items.Count < 1 || this.gvAnnotations.SelectedItems.Count < 1)
        return;
      int newIndex = 0;
      if (this.gvAnnotations.SelectedItems[0].DisplayIndex < this.gvAnnotations.Items.Count - 1)
        newIndex = this.gvAnnotations.SelectedItems[0].DisplayIndex + 1;
      this.selectAnnotationRowByIndex(newIndex);
    }

    private void selectAnnotationRowByIndex(int newIndex)
    {
      this.gvAnnotations.VisibleItems[newIndex].Selected = true;
      this.gvAnnotations.EnsureVisible(newIndex);
      this.OnSelectedIndexCommitted(EventArgs.Empty);
    }

    private void btnDelete_Click(object sender, EventArgs e)
    {
      if (this.gvAnnotations.SelectedItems.Count < 1)
      {
        int num = (int) MessageBox.Show("Please select one or more annotations to delete.");
      }
      else
      {
        if (Utils.Dialog((IWin32Window) Form.ActiveForm, "Are you sure that you want to delete the selected annotation?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
          return;
        long ticks = DateTime.Now.Ticks;
        PageAnnotation tag = (PageAnnotation) this.gvAnnotations.SelectedItems[0].Tag;
        tag.Page.Annotations.Remove(tag);
        RemoteLogger.Write(TraceLevel.Info, "Annotated ImageAttachment: " + (object) TimeSpan.FromTicks(DateTime.Now.Ticks - ticks).TotalMilliseconds + " ms");
        this.LoadAnnotations(this.attachmentList);
        this.OnAnnotationsChanged(EventArgs.Empty);
      }
    }

    public event EventHandler AnnotationsChanged;

    public event EventHandler SelectedIndexCommitted;

    protected virtual void OnAnnotationsChanged(EventArgs e)
    {
      if (this.AnnotationsChanged == null)
        return;
      this.AnnotationsChanged((object) this, e);
    }

    protected virtual void OnSelectedIndexCommitted(EventArgs e)
    {
      if (this.SelectedIndexCommitted == null)
        return;
      this.SelectedIndexCommitted((object) this, e);
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
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      GVColumn gvColumn4 = new GVColumn();
      GVColumn gvColumn5 = new GVColumn();
      this.gcAnnotations = new GroupContainer();
      this.pnlToolbar = new FlowLayoutPanel();
      this.btnDelete = new StandardIconButton();
      this.btnNext = new StandardIconButton();
      this.btnPrevious = new StandardIconButton();
      this.gvAnnotations = new GridView();
      this.toolTip = new ToolTip(this.components);
      this.gcAnnotations.SuspendLayout();
      this.pnlToolbar.SuspendLayout();
      ((ISupportInitialize) this.btnDelete).BeginInit();
      ((ISupportInitialize) this.btnNext).BeginInit();
      ((ISupportInitialize) this.btnPrevious).BeginInit();
      this.SuspendLayout();
      this.gcAnnotations.Controls.Add((Control) this.pnlToolbar);
      this.gcAnnotations.Controls.Add((Control) this.gvAnnotations);
      this.gcAnnotations.Dock = DockStyle.Fill;
      this.gcAnnotations.HeaderForeColor = SystemColors.ControlText;
      this.gcAnnotations.Location = new Point(0, 0);
      this.gcAnnotations.Name = "gcAnnotations";
      this.gcAnnotations.Size = new Size(578, 215);
      this.gcAnnotations.TabIndex = 7;
      this.gcAnnotations.Text = "All Annotations";
      this.pnlToolbar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.pnlToolbar.BackColor = Color.Transparent;
      this.pnlToolbar.Controls.Add((Control) this.btnDelete);
      this.pnlToolbar.Controls.Add((Control) this.btnNext);
      this.pnlToolbar.Controls.Add((Control) this.btnPrevious);
      this.pnlToolbar.FlowDirection = FlowDirection.RightToLeft;
      this.pnlToolbar.Location = new Point(490, 2);
      this.pnlToolbar.Name = "pnlToolbar";
      this.pnlToolbar.Size = new Size(84, 22);
      this.pnlToolbar.TabIndex = 1;
      this.btnDelete.BackColor = Color.Transparent;
      this.btnDelete.Enabled = false;
      this.btnDelete.Location = new Point(68, 3);
      this.btnDelete.Margin = new Padding(4, 3, 0, 3);
      this.btnDelete.MouseDownImage = (Image) null;
      this.btnDelete.Name = "btnDelete";
      this.btnDelete.Size = new Size(16, 16);
      this.btnDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnDelete.TabIndex = 28;
      this.btnDelete.TabStop = false;
      this.toolTip.SetToolTip((Control) this.btnDelete, "Delete Annotation");
      this.btnDelete.Visible = false;
      this.btnDelete.Click += new EventHandler(this.btnDelete_Click);
      this.btnNext.BackColor = Color.Transparent;
      this.btnNext.Location = new Point(48, 3);
      this.btnNext.Margin = new Padding(4, 3, 0, 3);
      this.btnNext.MouseDownImage = (Image) null;
      this.btnNext.Name = "btnNext";
      this.btnNext.Size = new Size(16, 16);
      this.btnNext.StandardButtonType = StandardIconButton.ButtonType.DownArrowButton;
      this.btnNext.TabIndex = 27;
      this.btnNext.TabStop = false;
      this.toolTip.SetToolTip((Control) this.btnNext, "Next Annotation");
      this.btnNext.Click += new EventHandler(this.btnNext_Click);
      this.btnPrevious.BackColor = Color.Transparent;
      this.btnPrevious.Location = new Point(28, 3);
      this.btnPrevious.Margin = new Padding(4, 3, 0, 3);
      this.btnPrevious.MouseDownImage = (Image) null;
      this.btnPrevious.Name = "btnPrevious";
      this.btnPrevious.Size = new Size(16, 16);
      this.btnPrevious.StandardButtonType = StandardIconButton.ButtonType.UpArrowButton;
      this.btnPrevious.TabIndex = 26;
      this.btnPrevious.TabStop = false;
      this.toolTip.SetToolTip((Control) this.btnPrevious, "Previous Annotation");
      this.btnPrevious.Click += new EventHandler(this.btnPrevious_Click);
      this.gvAnnotations.AllowMultiselect = false;
      this.gvAnnotations.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "colAnnotation";
      gvColumn1.Text = "Annotation";
      gvColumn1.Width = 250;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "colDate";
      gvColumn2.SortMethod = GVSortMethod.DateTime;
      gvColumn2.Text = "Date";
      gvColumn2.Width = 105;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "colUser";
      gvColumn3.Text = "User";
      gvColumn3.Width = 100;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "colPage";
      gvColumn4.SortMethod = GVSortMethod.Numeric;
      gvColumn4.Text = "Location";
      gvColumn4.Width = 75;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "colVisibility";
      gvColumn5.Text = "Visibility";
      gvColumn5.Width = 75;
      this.gvAnnotations.Columns.AddRange(new GVColumn[5]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5
      });
      this.gvAnnotations.Dock = DockStyle.Fill;
      this.gvAnnotations.Location = new Point(1, 26);
      this.gvAnnotations.Name = "gvAnnotations";
      this.gvAnnotations.Size = new Size(576, 188);
      this.gvAnnotations.TabIndex = 2;
      this.gvAnnotations.TextTrimming = StringTrimming.EllipsisCharacter;
      this.gvAnnotations.UseCompatibleEditingBehavior = true;
      this.gvAnnotations.SelectedIndexChanged += new EventHandler(this.gvAnnotations_SelectedIndexChanged);
      this.gvAnnotations.SelectedIndexCommitted += new EventHandler(this.gvAnnotations_SelectedIndexCommitted);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.gcAnnotations);
      this.Name = nameof (PageAnnotationListControl);
      this.Size = new Size(578, 215);
      this.gcAnnotations.ResumeLayout(false);
      this.pnlToolbar.ResumeLayout(false);
      ((ISupportInitialize) this.btnDelete).EndInit();
      ((ISupportInitialize) this.btnNext).EndInit();
      ((ISupportInitialize) this.btnPrevious).EndInit();
      this.ResumeLayout(false);
    }
  }
}
