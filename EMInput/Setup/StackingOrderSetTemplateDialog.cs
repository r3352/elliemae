// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.StackingOrderSetTemplateDialog
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.DocEngine;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class StackingOrderSetTemplateDialog : Form, IHelp
  {
    private const string className = "StackingOrderSetTemplateDialog";
    private static string sw = Tracing.SwOutsideLoan;
    private Label label4;
    private TextBox descTxt;
    private TextBox nameTxt;
    private Label label5;
    private Button cancelBtn;
    private Button saveBtn;
    private IContainer components;
    private StackingOrderSetTemplate orderTemplate;
    private EMHelpLink emHelpLink1;
    private CheckBox chkFilterDocuments;
    private Label label1;
    private Label label6;
    private Panel pnlBody;
    private Panel pnlBodyRight;
    private GroupContainer grpStackingOrder;
    private FlowLayoutPanel flowLayoutPanel1;
    private StandardIconButton btnRemoveDocument;
    private StandardIconButton btnMoveDocumentDown;
    private StandardIconButton btnMoveDocumentUp;
    private GridView gvStackingOrder;
    private Panel pnlBodyRightFooter;
    private Panel pnlArrows;
    private StandardIconButton btnAddNewDocument;
    private StandardIconButton btnRemoveNewDocument;
    private Panel pnlBodyLeft;
    private GroupContainer grpNewDocuments;
    private GridView gveFolderDocs;
    private Panel pnlBodyLeftFooter;
    private ToolTip toolTip1;
    private CheckBox chkAutoSelect;
    private GradientPanel pnlDocumentSource;
    private ComboBox cboDocumentSource;
    private Label label2;
    private GridView gvDocEngine;
    private DocumentTrackingSetup docTrackingSetup;
    private Sessions.Session session;
    private const string BADCHARS = "/:*?<>|.";

    public StackingOrderSetTemplateDialog(StackingOrderSetTemplate orderTemplate)
      : this(orderTemplate, Session.DefaultInstance)
    {
    }

    public StackingOrderSetTemplateDialog(
      StackingOrderSetTemplate orderTemplate,
      Sessions.Session session)
    {
      this.session = session;
      this.InitializeComponent();
      this.docTrackingSetup = this.session.ConfigurationManager.GetDocumentTrackingSetup();
      this.orderTemplate = orderTemplate;
      ArrayList requiredDocs = this.orderTemplate.RequiredDocs;
      ArrayList ndeDocGroups = this.orderTemplate.NDEDocGroups;
      this.gvStackingOrder.Items.Clear();
      foreach (string docName in this.orderTemplate.DocNames)
      {
        DocumentTemplate doc = this.docTrackingSetup.GetByName(docName) ?? new DocumentTemplate(docName);
        this.gvStackingOrder.Items.Add(!ndeDocGroups.Contains((object) docName) ? this.createGVItemForStackingOrder(doc, requiredDocs.Contains((object) docName)) : this.createGVItemForStackingOrder(new StackingElement(StackingElementType.DocumentType, docName), requiredDocs.Contains((object) docName)));
      }
      this.loadDocumentSources();
      this.nameTxt.Text = this.orderTemplate.TemplateName;
      this.descTxt.Text = this.orderTemplate.Description;
      this.chkFilterDocuments.Checked = this.orderTemplate.FilterDocuments;
      this.chkAutoSelect.Checked = this.orderTemplate.AutoSelectDocuments;
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
      GVColumn gvColumn6 = new GVColumn();
      this.label4 = new Label();
      this.descTxt = new TextBox();
      this.nameTxt = new TextBox();
      this.label5 = new Label();
      this.cancelBtn = new Button();
      this.saveBtn = new Button();
      this.chkFilterDocuments = new CheckBox();
      this.label1 = new Label();
      this.label6 = new Label();
      this.pnlBody = new Panel();
      this.pnlBodyRight = new Panel();
      this.grpStackingOrder = new GroupContainer();
      this.flowLayoutPanel1 = new FlowLayoutPanel();
      this.btnRemoveDocument = new StandardIconButton();
      this.btnMoveDocumentDown = new StandardIconButton();
      this.btnMoveDocumentUp = new StandardIconButton();
      this.gvStackingOrder = new GridView();
      this.pnlBodyRightFooter = new Panel();
      this.pnlArrows = new Panel();
      this.btnAddNewDocument = new StandardIconButton();
      this.btnRemoveNewDocument = new StandardIconButton();
      this.pnlBodyLeft = new Panel();
      this.grpNewDocuments = new GroupContainer();
      this.gvDocEngine = new GridView();
      this.pnlDocumentSource = new GradientPanel();
      this.cboDocumentSource = new ComboBox();
      this.label2 = new Label();
      this.gveFolderDocs = new GridView();
      this.pnlBodyLeftFooter = new Panel();
      this.toolTip1 = new ToolTip(this.components);
      this.emHelpLink1 = new EMHelpLink();
      this.chkAutoSelect = new CheckBox();
      this.pnlBody.SuspendLayout();
      this.pnlBodyRight.SuspendLayout();
      this.grpStackingOrder.SuspendLayout();
      this.flowLayoutPanel1.SuspendLayout();
      ((ISupportInitialize) this.btnRemoveDocument).BeginInit();
      ((ISupportInitialize) this.btnMoveDocumentDown).BeginInit();
      ((ISupportInitialize) this.btnMoveDocumentUp).BeginInit();
      this.pnlArrows.SuspendLayout();
      ((ISupportInitialize) this.btnAddNewDocument).BeginInit();
      ((ISupportInitialize) this.btnRemoveNewDocument).BeginInit();
      this.pnlBodyLeft.SuspendLayout();
      this.grpNewDocuments.SuspendLayout();
      this.pnlDocumentSource.SuspendLayout();
      this.SuspendLayout();
      this.label4.AutoSize = true;
      this.label4.Location = new Point(12, 83);
      this.label4.Name = "label4";
      this.label4.Size = new Size(61, 14);
      this.label4.TabIndex = 24;
      this.label4.Text = "Description";
      this.descTxt.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.descTxt.Location = new Point(103, 80);
      this.descTxt.Multiline = true;
      this.descTxt.Name = "descTxt";
      this.descTxt.Size = new Size(749, 48);
      this.descTxt.TabIndex = 0;
      this.nameTxt.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.nameTxt.Location = new Point(103, 48);
      this.nameTxt.MaxLength = 256;
      this.nameTxt.Name = "nameTxt";
      this.nameTxt.ReadOnly = true;
      this.nameTxt.Size = new Size(750, 20);
      this.nameTxt.TabIndex = 1;
      this.nameTxt.KeyPress += new KeyPressEventHandler(this.keypress);
      this.label5.AutoSize = true;
      this.label5.Location = new Point(12, 52);
      this.label5.Name = "label5";
      this.label5.Size = new Size(79, 14);
      this.label5.TabIndex = 23;
      this.label5.Text = "Template Name";
      this.cancelBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.cancelBtn.DialogResult = DialogResult.Cancel;
      this.cancelBtn.Location = new Point(778, 565);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(75, 24);
      this.cancelBtn.TabIndex = 8;
      this.cancelBtn.Text = "&Cancel";
      this.saveBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.saveBtn.Location = new Point(698, 565);
      this.saveBtn.Name = "saveBtn";
      this.saveBtn.Size = new Size(75, 24);
      this.saveBtn.TabIndex = 7;
      this.saveBtn.Text = "OK";
      this.saveBtn.Click += new EventHandler(this.saveBtn_Click);
      this.chkFilterDocuments.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.chkFilterDocuments.AutoSize = true;
      this.chkFilterDocuments.Location = new Point(12, 550);
      this.chkFilterDocuments.Name = "chkFilterDocuments";
      this.chkFilterDocuments.Size = new Size(304, 18);
      this.chkFilterDocuments.TabIndex = 27;
      this.chkFilterDocuments.Text = "Display only the documents included in the stacking order.";
      this.chkFilterDocuments.UseVisualStyleBackColor = true;
      this.label1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.label1.Location = new Point(10, 10);
      this.label1.Name = "label1";
      this.label1.Size = new Size(843, 18);
      this.label1.TabIndex = 28;
      this.label1.Text = "You can drag new documents and drop them into the stacking template on the right. When saved, the changes made to the document stacking template are immediate.";
      this.label6.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.label6.Location = new Point(10, 28);
      this.label6.Name = "label6";
      this.label6.Size = new Size(843, 18);
      this.label6.TabIndex = 29;
      this.label6.Text = "Select which documents are required when sending to a 3rd party.";
      this.pnlBody.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.pnlBody.Controls.Add((Control) this.pnlBodyRight);
      this.pnlBody.Controls.Add((Control) this.pnlArrows);
      this.pnlBody.Controls.Add((Control) this.pnlBodyLeft);
      this.pnlBody.Location = new Point(12, 135);
      this.pnlBody.Name = "pnlBody";
      this.pnlBody.Size = new Size(841, 409);
      this.pnlBody.TabIndex = 30;
      this.pnlBody.Resize += new EventHandler(this.pnlBody_Resize);
      this.pnlBodyRight.Controls.Add((Control) this.grpStackingOrder);
      this.pnlBodyRight.Controls.Add((Control) this.pnlBodyRightFooter);
      this.pnlBodyRight.Dock = DockStyle.Fill;
      this.pnlBodyRight.Location = new Point(450, 0);
      this.pnlBodyRight.Name = "pnlBodyRight";
      this.pnlBodyRight.Size = new Size(391, 409);
      this.pnlBodyRight.TabIndex = 15;
      this.grpStackingOrder.Controls.Add((Control) this.flowLayoutPanel1);
      this.grpStackingOrder.Controls.Add((Control) this.gvStackingOrder);
      this.grpStackingOrder.HeaderForeColor = SystemColors.ControlText;
      this.grpStackingOrder.Location = new Point(0, 0);
      this.grpStackingOrder.Name = "grpStackingOrder";
      this.grpStackingOrder.Size = new Size(391, 408);
      this.grpStackingOrder.TabIndex = 4;
      this.grpStackingOrder.Text = "Stacking Template";
      this.flowLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.flowLayoutPanel1.AutoSize = true;
      this.flowLayoutPanel1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
      this.flowLayoutPanel1.BackColor = Color.Transparent;
      this.flowLayoutPanel1.Controls.Add((Control) this.btnRemoveDocument);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnMoveDocumentDown);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnMoveDocumentUp);
      this.flowLayoutPanel1.FlowDirection = FlowDirection.RightToLeft;
      this.flowLayoutPanel1.Location = new Point(322, 2);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      this.flowLayoutPanel1.Size = new Size(63, 22);
      this.flowLayoutPanel1.TabIndex = 1;
      this.btnRemoveDocument.BackColor = Color.Transparent;
      this.btnRemoveDocument.Enabled = false;
      this.btnRemoveDocument.Location = new Point(47, 3);
      this.btnRemoveDocument.Margin = new Padding(5, 3, 0, 3);
      this.btnRemoveDocument.MouseDownImage = (Image) null;
      this.btnRemoveDocument.Name = "btnRemoveDocument";
      this.btnRemoveDocument.Size = new Size(16, 16);
      this.btnRemoveDocument.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnRemoveDocument.TabIndex = 2;
      this.btnRemoveDocument.TabStop = false;
      this.btnRemoveDocument.Click += new EventHandler(this.btnRemoveNewDocument_Click);
      this.btnMoveDocumentDown.BackColor = Color.Transparent;
      this.btnMoveDocumentDown.Enabled = false;
      this.btnMoveDocumentDown.Location = new Point(26, 3);
      this.btnMoveDocumentDown.Margin = new Padding(5, 3, 0, 3);
      this.btnMoveDocumentDown.MouseDownImage = (Image) null;
      this.btnMoveDocumentDown.Name = "btnMoveDocumentDown";
      this.btnMoveDocumentDown.Size = new Size(16, 16);
      this.btnMoveDocumentDown.StandardButtonType = StandardIconButton.ButtonType.DownArrowButton;
      this.btnMoveDocumentDown.TabIndex = 0;
      this.btnMoveDocumentDown.TabStop = false;
      this.btnMoveDocumentDown.Click += new EventHandler(this.btnMoveDocumentDown_Click);
      this.btnMoveDocumentUp.BackColor = Color.Transparent;
      this.btnMoveDocumentUp.Enabled = false;
      this.btnMoveDocumentUp.Location = new Point(5, 3);
      this.btnMoveDocumentUp.Margin = new Padding(5, 3, 0, 3);
      this.btnMoveDocumentUp.MouseDownImage = (Image) null;
      this.btnMoveDocumentUp.Name = "btnMoveDocumentUp";
      this.btnMoveDocumentUp.Size = new Size(16, 16);
      this.btnMoveDocumentUp.StandardButtonType = StandardIconButton.ButtonType.UpArrowButton;
      this.btnMoveDocumentUp.TabIndex = 1;
      this.btnMoveDocumentUp.TabStop = false;
      this.btnMoveDocumentUp.Click += new EventHandler(this.btnMoveDocumentUp_Click);
      this.gvStackingOrder.AllowDrop = true;
      this.gvStackingOrder.BorderStyle = BorderStyle.None;
      gvColumn1.CheckBoxes = true;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.SpringToFit = true;
      gvColumn1.Text = "Required";
      gvColumn1.Width = 89;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Name";
      gvColumn2.Width = 300;
      this.gvStackingOrder.Columns.AddRange(new GVColumn[2]
      {
        gvColumn1,
        gvColumn2
      });
      this.gvStackingOrder.Dock = DockStyle.Fill;
      this.gvStackingOrder.DropTarget = GVDropTarget.BetweenItems;
      this.gvStackingOrder.Location = new Point(1, 26);
      this.gvStackingOrder.Name = "gvStackingOrder";
      this.gvStackingOrder.Size = new Size(389, 381);
      this.gvStackingOrder.SortIconVisible = false;
      this.gvStackingOrder.SortOption = GVSortOption.None;
      this.gvStackingOrder.TabIndex = 0;
      this.gvStackingOrder.SelectedIndexChanged += new EventHandler(this.gvStackingOrder_SelectedIndexChanged);
      this.gvStackingOrder.ItemDrag += new GVItemEventHandler(this.gridView_ItemDrag);
      this.gvStackingOrder.DragDrop += new DragEventHandler(this.gridView_DragDrop);
      this.gvStackingOrder.DragOver += new DragEventHandler(this.gridView_DragOver);
      this.pnlBodyRightFooter.Dock = DockStyle.Bottom;
      this.pnlBodyRightFooter.Location = new Point(0, 399);
      this.pnlBodyRightFooter.Name = "pnlBodyRightFooter";
      this.pnlBodyRightFooter.Size = new Size(391, 10);
      this.pnlBodyRightFooter.TabIndex = 0;
      this.pnlArrows.Controls.Add((Control) this.btnAddNewDocument);
      this.pnlArrows.Controls.Add((Control) this.btnRemoveNewDocument);
      this.pnlArrows.Dock = DockStyle.Left;
      this.pnlArrows.Location = new Point(398, 0);
      this.pnlArrows.Name = "pnlArrows";
      this.pnlArrows.Size = new Size(52, 409);
      this.pnlArrows.TabIndex = 13;
      this.btnAddNewDocument.Anchor = AnchorStyles.None;
      this.btnAddNewDocument.BackColor = Color.Transparent;
      this.btnAddNewDocument.Enabled = false;
      this.btnAddNewDocument.Location = new Point(14, 176);
      this.btnAddNewDocument.MouseDownImage = (Image) null;
      this.btnAddNewDocument.Name = "btnAddNewDocument";
      this.btnAddNewDocument.Size = new Size(25, 25);
      this.btnAddNewDocument.SizeMode = PictureBoxSizeMode.AutoSize;
      this.btnAddNewDocument.StandardButtonType = StandardIconButton.ButtonType.RightArrowButton;
      this.btnAddNewDocument.TabIndex = 3;
      this.btnAddNewDocument.TabStop = false;
      this.btnAddNewDocument.Click += new EventHandler(this.btnAddNewDocument_Click);
      this.btnRemoveNewDocument.Anchor = AnchorStyles.None;
      this.btnRemoveNewDocument.BackColor = Color.Transparent;
      this.btnRemoveNewDocument.Enabled = false;
      this.btnRemoveNewDocument.Location = new Point(14, 207);
      this.btnRemoveNewDocument.MouseDownImage = (Image) null;
      this.btnRemoveNewDocument.Name = "btnRemoveNewDocument";
      this.btnRemoveNewDocument.Size = new Size(25, 25);
      this.btnRemoveNewDocument.SizeMode = PictureBoxSizeMode.AutoSize;
      this.btnRemoveNewDocument.StandardButtonType = StandardIconButton.ButtonType.LeftArrowButton;
      this.btnRemoveNewDocument.TabIndex = 2;
      this.btnRemoveNewDocument.TabStop = false;
      this.btnRemoveNewDocument.Visible = false;
      this.btnRemoveNewDocument.Click += new EventHandler(this.btnRemoveNewDocument_Click);
      this.pnlBodyLeft.Controls.Add((Control) this.grpNewDocuments);
      this.pnlBodyLeft.Controls.Add((Control) this.pnlBodyLeftFooter);
      this.pnlBodyLeft.Dock = DockStyle.Left;
      this.pnlBodyLeft.Location = new Point(0, 0);
      this.pnlBodyLeft.Name = "pnlBodyLeft";
      this.pnlBodyLeft.Size = new Size(398, 409);
      this.pnlBodyLeft.TabIndex = 14;
      this.grpNewDocuments.Controls.Add((Control) this.gvDocEngine);
      this.grpNewDocuments.Controls.Add((Control) this.gveFolderDocs);
      this.grpNewDocuments.Controls.Add((Control) this.pnlDocumentSource);
      this.grpNewDocuments.HeaderForeColor = SystemColors.ControlText;
      this.grpNewDocuments.Location = new Point(0, 0);
      this.grpNewDocuments.Name = "grpNewDocuments";
      this.grpNewDocuments.Size = new Size(398, 408);
      this.grpNewDocuments.TabIndex = 12;
      this.grpNewDocuments.Text = "New Documents";
      this.gvDocEngine.AllowDrop = true;
      this.gvDocEngine.BorderStyle = BorderStyle.None;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column1";
      gvColumn3.SpringToFit = true;
      gvColumn3.Text = "Name";
      gvColumn3.Width = 189;
      this.gvDocEngine.Columns.AddRange(new GVColumn[1]
      {
        gvColumn3
      });
      this.gvDocEngine.DropTarget = GVDropTarget.BetweenItems;
      this.gvDocEngine.Location = new Point(26, 109);
      this.gvDocEngine.Name = "gvDocEngine";
      this.gvDocEngine.Size = new Size(189, 284);
      this.gvDocEngine.TabIndex = 3;
      this.gvDocEngine.SelectedIndexChanged += new EventHandler(this.gvNewDocuments_SelectedIndexChanged);
      this.gvDocEngine.ItemDrag += new GVItemEventHandler(this.gridView_ItemDrag);
      this.gvDocEngine.DragDrop += new DragEventHandler(this.gridView_DragDrop);
      this.gvDocEngine.DragOver += new DragEventHandler(this.gridView_DragOver);
      this.pnlDocumentSource.Borders = AnchorStyles.Bottom;
      this.pnlDocumentSource.Controls.Add((Control) this.cboDocumentSource);
      this.pnlDocumentSource.Controls.Add((Control) this.label2);
      this.pnlDocumentSource.Dock = DockStyle.Top;
      this.pnlDocumentSource.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.pnlDocumentSource.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.pnlDocumentSource.Location = new Point(1, 26);
      this.pnlDocumentSource.Name = "pnlDocumentSource";
      this.pnlDocumentSource.Size = new Size(396, 25);
      this.pnlDocumentSource.Style = GradientPanel.PanelStyle.TableFooter;
      this.pnlDocumentSource.TabIndex = 2;
      this.cboDocumentSource.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cboDocumentSource.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboDocumentSource.FormattingEnabled = true;
      this.cboDocumentSource.Location = new Point(53, 1);
      this.cboDocumentSource.Name = "cboDocumentSource";
      this.cboDocumentSource.Size = new Size(336, 22);
      this.cboDocumentSource.TabIndex = 1;
      this.cboDocumentSource.SelectedIndexChanged += new EventHandler(this.cboDocumentSource_SelectedIndexChanged);
      this.label2.AutoSize = true;
      this.label2.BackColor = Color.Transparent;
      this.label2.Location = new Point(7, 6);
      this.label2.Name = "label2";
      this.label2.Size = new Size(42, 14);
      this.label2.TabIndex = 0;
      this.label2.Text = "Source";
      this.gveFolderDocs.AllowDrop = true;
      this.gveFolderDocs.BorderStyle = BorderStyle.None;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column1";
      gvColumn4.SpringToFit = true;
      gvColumn4.Text = "Name";
      gvColumn4.Width = 3;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Column2";
      gvColumn5.Text = "Type";
      gvColumn5.Width = 125;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "Column3";
      gvColumn6.Text = "Source";
      gvColumn6.Width = 150;
      this.gveFolderDocs.Columns.AddRange(new GVColumn[3]
      {
        gvColumn4,
        gvColumn5,
        gvColumn6
      });
      this.gveFolderDocs.DropTarget = GVDropTarget.BetweenItems;
      this.gveFolderDocs.Location = new Point(186, 67);
      this.gveFolderDocs.Name = "gveFolderDocs";
      this.gveFolderDocs.Size = new Size(189, 284);
      this.gveFolderDocs.TabIndex = 0;
      this.gveFolderDocs.SelectedIndexChanged += new EventHandler(this.gvNewDocuments_SelectedIndexChanged);
      this.gveFolderDocs.ItemDrag += new GVItemEventHandler(this.gridView_ItemDrag);
      this.gveFolderDocs.DragDrop += new DragEventHandler(this.gridView_DragDrop);
      this.gveFolderDocs.DragOver += new DragEventHandler(this.gridView_DragOver);
      this.pnlBodyLeftFooter.Dock = DockStyle.Bottom;
      this.pnlBodyLeftFooter.Location = new Point(0, 399);
      this.pnlBodyLeftFooter.Name = "pnlBodyLeftFooter";
      this.pnlBodyLeftFooter.Size = new Size(398, 10);
      this.pnlBodyLeftFooter.TabIndex = 16;
      this.emHelpLink1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.emHelpLink1.BackColor = Color.Transparent;
      this.emHelpLink1.Cursor = Cursors.Hand;
      this.emHelpLink1.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.emHelpLink1.HelpTag = nameof (StackingOrderSetTemplateDialog);
      this.emHelpLink1.Location = new Point(462, 568);
      this.emHelpLink1.Name = "emHelpLink1";
      this.emHelpLink1.Size = new Size(90, 16);
      this.emHelpLink1.TabIndex = 26;
      this.emHelpLink1.Visible = false;
      this.chkAutoSelect.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.chkAutoSelect.AutoSize = true;
      this.chkAutoSelect.Location = new Point(12, 574);
      this.chkAutoSelect.Name = "chkAutoSelect";
      this.chkAutoSelect.Size = new Size(356, 18);
      this.chkAutoSelect.TabIndex = 31;
      this.chkAutoSelect.Text = "Auto-select all documents included in the stacking for Print and Save";
      this.chkAutoSelect.UseVisualStyleBackColor = true;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.CancelButton = (IButtonControl) this.cancelBtn;
      this.ClientSize = new Size(865, 594);
      this.Controls.Add((Control) this.chkAutoSelect);
      this.Controls.Add((Control) this.pnlBody);
      this.Controls.Add((Control) this.label6);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.chkFilterDocuments);
      this.Controls.Add((Control) this.emHelpLink1);
      this.Controls.Add((Control) this.cancelBtn);
      this.Controls.Add((Control) this.saveBtn);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.descTxt);
      this.Controls.Add((Control) this.nameTxt);
      this.Controls.Add((Control) this.label5);
      this.Font = new Font("Arial", 8.25f);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (StackingOrderSetTemplateDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Update Document Stacking Template";
      this.KeyDown += new KeyEventHandler(this.Help_KeyDown);
      this.pnlBody.ResumeLayout(false);
      this.pnlBodyRight.ResumeLayout(false);
      this.grpStackingOrder.ResumeLayout(false);
      this.grpStackingOrder.PerformLayout();
      this.flowLayoutPanel1.ResumeLayout(false);
      ((ISupportInitialize) this.btnRemoveDocument).EndInit();
      ((ISupportInitialize) this.btnMoveDocumentDown).EndInit();
      ((ISupportInitialize) this.btnMoveDocumentUp).EndInit();
      this.pnlArrows.ResumeLayout(false);
      this.pnlArrows.PerformLayout();
      ((ISupportInitialize) this.btnAddNewDocument).EndInit();
      ((ISupportInitialize) this.btnRemoveNewDocument).EndInit();
      this.pnlBodyLeft.ResumeLayout(false);
      this.grpNewDocuments.ResumeLayout(false);
      this.pnlDocumentSource.ResumeLayout(false);
      this.pnlDocumentSource.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    public StackingOrderSetTemplate StackingOrderTemplate => this.orderTemplate;

    private void setToolTips()
    {
      Control[] controlArray = new Control[4]
      {
        (Control) this.btnAddNewDocument,
        (Control) this.btnRemoveNewDocument,
        (Control) this.btnMoveDocumentDown,
        (Control) this.btnMoveDocumentUp
      };
      foreach (Control control in controlArray)
        this.toolTip1.SetToolTip(control, this.toolTip1.GetToolTip(control) + Environment.NewLine + "You can also drag and drop.");
    }

    private void loadDocumentSources()
    {
      this.cboDocumentSource.Items.Add((object) "eDisclosures (Returned)");
      this.cboDocumentSource.Items.Add((object) "eDisclosures (Default)");
      this.cboDocumentSource.Items.Add((object) "Closing docs (Returned)");
      this.cboDocumentSource.Items.Add((object) "Closing docs (Default)");
      this.cboDocumentSource.Items.Add((object) "eFolder docs");
      this.cboDocumentSource.SelectedIndex = 0;
      if (this.gveFolderDocs.Items.Count != 0)
        return;
      this.cboDocumentSource.SelectedIndex = 4;
    }

    private void cboDocumentSource_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.refreshSourceDocumentList();
    }

    private void refreshSourceDocumentList()
    {
      if (this.cboDocumentSource.SelectedItem.ToString().ToUpper().Contains("RETURNED"))
        this.loadReturnedDocEngineStackingElements();
      else if (this.cboDocumentSource.SelectedItem.ToString().ToUpper().Contains("DEFAULT"))
        this.loadDefaultDocEngineStackingElements();
      else
        this.loadEFolderDocumentStackingElements();
    }

    private void loadReturnedDocEngineStackingElements()
    {
      DocumentOrderType orderType = DocumentOrderType.Opening;
      if (this.cboDocumentSource.SelectedItem.ToString().ToUpper().Contains("CLOSING"))
        orderType = DocumentOrderType.Closing;
      this.loadDocEngineElements(this.session.ConfigurationManager.GetKnownDocEngineStackingElements(orderType));
      this.gveFolderDocs.SelectedItems.Clear();
      this.gveFolderDocs.Visible = false;
      this.gvDocEngine.Dock = DockStyle.Fill;
      this.gvDocEngine.Visible = true;
      this.gvDocEngine.Sort(0, SortOrder.Ascending);
    }

    private void loadDefaultDocEngineStackingElements()
    {
      DocumentOrderType orderType = DocumentOrderType.Opening;
      if (this.cboDocumentSource.SelectedItem.ToString().ToUpper().Contains("CLOSING"))
        orderType = DocumentOrderType.Closing;
      this.loadDocEngineElements(DocEngineStackingOrder.GetDefaultStackingOrder(orderType).Elements.ToArray());
      this.gveFolderDocs.SelectedItems.Clear();
      this.gveFolderDocs.Visible = false;
      this.gvDocEngine.Dock = DockStyle.Fill;
      this.gvDocEngine.Visible = true;
      this.gvDocEngine.Sort(0, SortOrder.Ascending);
    }

    private void loadDocEngineElements(StackingElement[] elementsToLoad)
    {
      List<StackingElement> stackingElementList = new List<StackingElement>();
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvStackingOrder.Items)
        stackingElementList.Add((StackingElement) gvItem.Tag);
      this.gvDocEngine.Items.Clear();
      foreach (StackingElement element in elementsToLoad)
      {
        if (!stackingElementList.Contains(element))
          this.gvDocEngine.Items.Add(this.createGVItemForElement(element));
      }
      this.gvDocEngine.Sort(0, SortOrder.Ascending);
      this.updateDocumentCounts();
    }

    private void loadEFolderDocumentStackingElements()
    {
      List<StackingElement> stackingElementList = new List<StackingElement>();
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvStackingOrder.Items)
        stackingElementList.Add((StackingElement) gvItem.Tag);
      DocumentTrackingSetup documentTrackingSetup = this.session.ConfigurationManager.GetDocumentTrackingSetup();
      this.gveFolderDocs.Items.Clear();
      foreach (DocumentTemplate doc in documentTrackingSetup)
      {
        StackingElement stackingElement = new StackingElement(StackingElementType.Document, doc.Name);
        if (!stackingElementList.Contains(stackingElement))
          this.gveFolderDocs.Items.Add(this.createGVItemForDocumentTemplate(doc));
      }
      string name = string.Empty;
      for (int index = 1; index <= 5; ++index)
      {
        switch (index)
        {
          case 1:
            name = "VOD";
            break;
          case 2:
            name = "VOE";
            break;
          case 3:
            name = "VOL";
            break;
          case 4:
            name = "VOM";
            break;
          case 5:
            name = "VOR";
            break;
        }
        if (!this.orderTemplate.DocNames.Contains((object) name))
          this.gveFolderDocs.Items.Add(this.createGVItemForDocumentTemplate(new DocumentTemplate(name)));
      }
      this.updateDocumentCounts();
      this.gvDocEngine.SelectedItems.Clear();
      this.gvDocEngine.Visible = false;
      this.gveFolderDocs.Dock = DockStyle.Fill;
      this.gveFolderDocs.Visible = true;
      this.gveFolderDocs.Sort(0, SortOrder.Ascending);
    }

    private GVItem createGVItemForElement(StackingElement element)
    {
      return new GVItem()
      {
        SubItems = {
          [0] = {
            Value = (object) new StackingDisplayElement(element)
          }
        },
        Tag = (object) element
      };
    }

    private GVItem createGVItemForDocumentTemplate(DocumentTemplate doc)
    {
      return new GVItem()
      {
        SubItems = {
          [0] = {
            Value = (object) doc.Name
          },
          [1] = {
            Value = (object) doc.SourceType
          },
          [2] = {
            Value = (object) doc.Source
          }
        },
        Tag = (object) new StackingElement(StackingElementType.Document, doc.Name)
      };
    }

    private GVItem createGVItemForStackingOrder(DocumentTemplate doc, bool isRequired)
    {
      return new GVItem()
      {
        SubItems = {
          [0] = {
            Checked = isRequired
          },
          [1] = {
            Value = (object) doc.Name
          }
        },
        Tag = (object) new StackingElement(StackingElementType.Document, doc.Name)
      };
    }

    private GVItem createGVItemForStackingOrder(StackingElement element, bool isRequired)
    {
      return new GVItem()
      {
        SubItems = {
          [0] = {
            Checked = isRequired
          },
          [1] = {
            Value = (object) new StackingDisplayElement(element)
          }
        },
        Tag = (object) element
      };
    }

    private void updateDocumentCounts()
    {
      this.grpNewDocuments.Text = "New Documents (" + (object) this.getSourceDocumentGridView().Items.Count + ")";
      this.grpStackingOrder.Text = this.orderTemplate.TemplateName + " (" + (object) this.gvStackingOrder.Items.Count + ")";
    }

    private void gvStackingOrder_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.btnMoveDocumentUp.Enabled = this.gvStackingOrder.SelectedItems.Count > 0;
      this.btnMoveDocumentDown.Enabled = this.gvStackingOrder.SelectedItems.Count > 0;
      this.btnRemoveDocument.Enabled = this.gvStackingOrder.SelectedItems.Count > 0;
      this.btnRemoveNewDocument.Enabled = this.gvStackingOrder.SelectedItems.Count > 0;
      if (this.gvStackingOrder.SelectedItems.Count <= 0)
        return;
      this.gveFolderDocs.SelectedItems.Clear();
    }

    private void gvNewDocuments_SelectedIndexChanged(object sender, EventArgs e)
    {
      GridView documentGridView = this.getSourceDocumentGridView();
      this.btnAddNewDocument.Enabled = documentGridView.SelectedItems.Count > 0;
      if (documentGridView.SelectedItems.Count <= 0)
        return;
      this.gvStackingOrder.SelectedItems.Clear();
    }

    private void pnlBody_Resize(object sender, EventArgs e)
    {
      this.pnlBodyLeft.Width = Math.Max(0, (this.pnlBody.Width - this.pnlArrows.Width) / 2);
    }

    private void saveBtn_Click(object sender, EventArgs e)
    {
      this.orderTemplate.Description = this.descTxt.Text.Trim();
      this.orderTemplate.FilterDocuments = this.chkFilterDocuments.Checked;
      this.orderTemplate.AutoSelectDocuments = this.chkAutoSelect.Checked;
      ArrayList docNames = this.orderTemplate.DocNames;
      docNames.Clear();
      ArrayList requiredDocs = this.orderTemplate.RequiredDocs;
      requiredDocs.Clear();
      ArrayList ndeDocGroups = this.orderTemplate.NDEDocGroups;
      ndeDocGroups.Clear();
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvStackingOrder.Items)
      {
        StackingElement tag = (StackingElement) gvItem.Tag;
        docNames.Add((object) tag.Name);
        if (gvItem.SubItems[0].Checked)
          requiredDocs.Add((object) tag.Name);
        if (tag.Type == StackingElementType.DocumentType)
          ndeDocGroups.Add((object) tag.Name);
      }
      this.DialogResult = DialogResult.OK;
    }

    private void keypress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar.Equals((object) Keys.Delete) || char.IsControl(e.KeyChar))
        return;
      if ("/:*?<>|.".IndexOf(e.KeyChar) == -1)
      {
        char keyChar = e.KeyChar;
        if (!keyChar.Equals('\\'))
        {
          keyChar = e.KeyChar;
          if (!keyChar.Equals('"'))
          {
            e.Handled = false;
            return;
          }
        }
        e.Handled = true;
      }
      else
        e.Handled = true;
    }

    private void Help_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.F1)
        return;
      this.ShowHelp();
    }

    public void ShowHelp()
    {
      JedHelp.ShowHelp((Control) this, SystemSettings.HelpFile, nameof (StackingOrderSetTemplateDialog));
    }

    private void btnMoveDocumentUp_Click(object sender, EventArgs e)
    {
      for (int nItemIndex = 1; nItemIndex < this.gvStackingOrder.Items.Count; ++nItemIndex)
      {
        if (this.gvStackingOrder.Items[nItemIndex].Selected && !this.gvStackingOrder.Items[nItemIndex - 1].Selected)
        {
          GVItem gvItem = this.gvStackingOrder.Items[nItemIndex];
          this.gvStackingOrder.Items.RemoveAt(nItemIndex);
          this.gvStackingOrder.Items.Insert(nItemIndex - 1, gvItem);
          gvItem.Selected = true;
        }
      }
    }

    private void btnMoveDocumentDown_Click(object sender, EventArgs e)
    {
      for (int nItemIndex = this.gvStackingOrder.Items.Count - 2; nItemIndex >= 0; --nItemIndex)
      {
        if (this.gvStackingOrder.Items[nItemIndex].Selected && !this.gvStackingOrder.Items[nItemIndex + 1].Selected)
        {
          GVItem gvItem = this.gvStackingOrder.Items[nItemIndex];
          this.gvStackingOrder.Items.RemoveAt(nItemIndex);
          this.gvStackingOrder.Items.Insert(nItemIndex + 1, gvItem);
          gvItem.Selected = true;
        }
      }
    }

    private void gridView_ItemDrag(object source, GVItemEventArgs e)
    {
      int num = (int) this.DoDragDrop(source, DragDropEffects.Move);
    }

    private void gridView_DragOver(object sender, DragEventArgs e)
    {
      GridView data = (GridView) e.Data.GetData(typeof (GridView));
      if (data == this.gvDocEngine || data == this.gveFolderDocs)
        e.Effect = DragDropEffects.Move;
      else if (sender == data)
        e.Effect = DragDropEffects.Move;
      else
        e.Effect = DragDropEffects.None;
    }

    private void gridView_DragDrop(object sender, DragEventArgs e)
    {
      GVDragEventArgs gvDragEventArgs = (GVDragEventArgs) e;
      GridView data = (GridView) e.Data.GetData(typeof (GridView));
      GridView gridView = (GridView) sender;
      if (data != gridView)
        gridView.SelectedItems.Clear();
      int num = 0;
      for (int nItemIndex = 0; nItemIndex < gvDragEventArgs.TargetLocation.InsertIndex; ++nItemIndex)
      {
        if (!gridView.Items[nItemIndex].Selected)
          ++num;
      }
      List<GVItem> gvItemList = new List<GVItem>();
      for (int nItemIndex = data.Items.Count - 1; nItemIndex >= 0; --nItemIndex)
      {
        if (data.Items[nItemIndex].Selected)
        {
          gvItemList.Insert(0, data.Items[nItemIndex]);
          data.Items.RemoveAt(nItemIndex);
        }
      }
      for (int index = 0; index < gvItemList.Count; ++index)
      {
        GVItem gvItem = !(gridView.Name == this.gvStackingOrder.Name) ? this.createGVItemForDocumentTemplate((DocumentTemplate) gvItemList[index].Tag) : (data != gridView ? this.createGVItemForStackingOrder((StackingElement) gvItemList[index].Tag, false) : this.createGVItemForStackingOrder((StackingElement) gvItemList[index].Tag, gvItemList[index].SubItems[0].Checked));
        gridView.Items.Insert(num + index, gvItem);
        gvItem.Selected = true;
      }
      this.updateDocumentCounts();
    }

    private void btnAddNewDocument_Click(object sender, EventArgs e)
    {
      this.moveDocuments(this.getSourceDocumentGridView(), this.gvStackingOrder);
    }

    private void btnRemoveNewDocument_Click(object sender, EventArgs e)
    {
      this.moveDocuments(this.gvStackingOrder, this.getSourceDocumentGridView());
      this.gvDocEngine.Sort(0, SortOrder.Ascending);
    }

    private void moveDocuments(GridView gvSource, GridView gvTarget)
    {
      List<GVItem> gvItemList = new List<GVItem>();
      for (int nItemIndex = 0; nItemIndex < gvSource.Items.Count; ++nItemIndex)
      {
        if (gvSource.Items[nItemIndex].Selected)
          gvItemList.Add(gvSource.Items[nItemIndex]);
      }
      for (int index = 0; index < gvItemList.Count; ++index)
      {
        GVItem gvItem = gvItemList[index];
        gvSource.Items.Remove(gvItem);
        if (gvTarget.Name == this.gvStackingOrder.Name)
        {
          GVItem forStackingOrder = this.createGVItemForStackingOrder((StackingElement) gvItem.Tag, false);
          gvTarget.Items.Add(forStackingOrder);
        }
        else
          this.refreshSourceDocumentList();
      }
      this.updateDocumentCounts();
    }

    private GridView getSourceDocumentGridView()
    {
      return this.cboDocumentSource.SelectedItem.ToString() == "eFolder docs" ? this.gveFolderDocs : this.gvDocEngine;
    }
  }
}
