// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.FieldHelpDialog
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Text;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class FieldHelpDialog : Form
  {
    private static FieldHelpDialog helpDialog;
    private const string instructionText = "Use the HelpPad to capture and view field-level help text. Press the Ctrl key, and then click in a field on an input form.";
    private int nextPrintItem;
    private bool displayingIntroItem;
    private IContainer components;
    private ControlListBox lstHelp;
    private SaveFileDialog sfdSave;
    private PrintDialog pdlgPrint;
    private PrintDocument pdPrintDoc;
    private PrintPreviewDialog ppvPreview;
    private GradientPanel gradientPanel1;
    private FlowLayoutPanel flowLayoutPanel1;
    private StandardIconButton btnSave;
    private StandardIconButton btnPrint;
    private StandardIconButton btnCopy;
    private Label label2;
    private ToolTip toolTip1;

    public FieldHelpDialog() => this.InitializeComponent();

    public void AddItem(string text)
    {
      try
      {
        FieldHelpItem fieldHelpItem = new FieldHelpItem(text);
        fieldHelpItem.Tag = (object) text;
        if (this.displayingIntroItem)
        {
          this.lstHelp.ListControls.Clear();
          this.displayingIntroItem = false;
        }
        this.lstHelp.ListControls.Add((Control) fieldHelpItem);
        this.lstHelp.EnsureVisible(this.lstHelp.ListControls.Count - 1);
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) null, "Error: " + (object) ex);
      }
    }

    public void ShowInstructions()
    {
      if (this.lstHelp.ListControls.Count != 0)
        return;
      FieldHelpItem fieldHelpItem = new FieldHelpItem("Use the HelpPad to capture and view field-level help text. Press the Ctrl key, and then click in a field on an input form.");
      fieldHelpItem.Tag = (object) "Use the HelpPad to capture and view field-level help text. Press the Ctrl key, and then click in a field on an input form.";
      this.lstHelp.ListControls.Add((Control) fieldHelpItem);
      this.lstHelp.EnsureVisible(this.lstHelp.ListControls.Count - 1);
      this.displayingIntroItem = true;
    }

    public static void Open() => FieldHelpDialog.openHelpDialog().ShowInstructions();

    public static void ShowHelp(string text) => FieldHelpDialog.openHelpDialog().AddItem(text);

    public static void Close()
    {
      try
      {
        if (FieldHelpDialog.helpDialog == null)
          return;
        FieldHelpDialog.helpDialog.Dispose();
        FieldHelpDialog.helpDialog = (FieldHelpDialog) null;
      }
      catch
      {
      }
    }

    private static FieldHelpDialog openHelpDialog()
    {
      if (FieldHelpDialog.helpDialog != null && !FieldHelpDialog.helpDialog.IsDisposed)
        return FieldHelpDialog.helpDialog;
      Form mainForm = Session.MainForm;
      try
      {
        FieldHelpDialog.helpDialog = new FieldHelpDialog();
        FieldHelpDialog.helpDialog.FormClosing += new FormClosingEventHandler(FieldHelpDialog.helpDialog_FormClosing);
        FieldHelpDialog.helpDialog.Left = Math.Max(0, mainForm.Right - FieldHelpDialog.helpDialog.Width - 5 * SystemInformation.Border3DSize.Width - SystemInformation.VerticalScrollBarWidth);
        FieldHelpDialog.helpDialog.Top = Math.Max(0, mainForm.PointToScreen(new Point(0, 0)).Y + 125);
        FieldHelpDialog.helpDialog.Show((IWin32Window) Session.MainScreen);
        return FieldHelpDialog.helpDialog;
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.ToString());
        return (FieldHelpDialog) null;
      }
    }

    private static void helpDialog_FormClosing(object sender, FormClosingEventArgs e)
    {
      FieldHelpDialog.helpDialog = (FieldHelpDialog) null;
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      if (this.sfdSave.ShowDialog((IWin32Window) this) != DialogResult.OK)
        return;
      this.saveHelpToFile(this.sfdSave.FileName);
    }

    private void saveHelpToFile(string filename)
    {
      try
      {
        using (StreamWriter streamWriter = new StreamWriter(filename, false, Encoding.ASCII))
          streamWriter.Write(this.getConcatentatedHelpText());
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "An error occurred while attempting to save the help text: " + (object) ex);
      }
    }

    private void btnCopy_Click(object sender, EventArgs e)
    {
      Clipboard.SetText(this.getConcatentatedHelpText());
    }

    private string getConcatentatedHelpText()
    {
      StringBuilder stringBuilder = new StringBuilder();
      for (int index = 0; index < this.lstHelp.ListControls.Count; ++index)
        stringBuilder.AppendLine(string.Concat(this.lstHelp.GetControlAt(index).Tag));
      return stringBuilder.ToString();
    }

    private void btnPrint_Click(object sender, EventArgs e)
    {
      this.pdlgPrint.Document = this.pdPrintDoc;
      if (this.pdlgPrint.ShowDialog((IWin32Window) this) != DialogResult.OK)
        return;
      try
      {
        this.pdPrintDoc.Print();
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Error attempting to print field help: " + (object) ex);
      }
    }

    private void pdPrintDoc_PrintPage(object sender, PrintPageEventArgs e)
    {
      using (Font font1 = new Font("Times Roman", 10f, FontStyle.Regular))
      {
        int num1 = 0;
        int num2 = e.MarginBounds.Height / font1.Height;
        StringFormat stringFormat1 = new StringFormat();
        e.HasMorePages = false;
        for (; this.nextPrintItem < this.lstHelp.ListControls.Count; ++this.nextPrintItem)
        {
          string str = string.Concat(this.lstHelp.GetControlAt(this.nextPrintItem).Tag);
          int num3 = 0;
          int num4 = 0;
          Graphics graphics1 = e.Graphics;
          string text = str;
          Font font2 = font1;
          Rectangle marginBounds = e.MarginBounds;
          double width1 = (double) marginBounds.Width;
          marginBounds = e.MarginBounds;
          double height1 = (double) marginBounds.Height;
          SizeF layoutArea = new SizeF((float) width1, (float) height1);
          StringFormat stringFormat2 = new StringFormat();
          ref int local1 = ref num3;
          ref int local2 = ref num4;
          graphics1.MeasureString(text, font2, layoutArea, stringFormat2, out local1, out local2);
          if (num1 + num4 > num2)
          {
            e.HasMorePages = true;
            break;
          }
          Graphics graphics2 = e.Graphics;
          string s = str;
          Font font3 = font1;
          Brush black1 = Brushes.Black;
          marginBounds = e.MarginBounds;
          double left1 = (double) marginBounds.Left;
          marginBounds = e.MarginBounds;
          double y1 = (double) (marginBounds.Top + num1 * font1.Height);
          marginBounds = e.MarginBounds;
          double width2 = (double) marginBounds.Width;
          marginBounds = e.MarginBounds;
          double height2 = (double) marginBounds.Height;
          RectangleF layoutRectangle1 = new RectangleF((float) left1, (float) y1, (float) width2, (float) height2);
          graphics2.DrawString(s, font3, black1, layoutRectangle1);
          num1 += num4;
          if (num1 < num2)
          {
            Graphics graphics3 = e.Graphics;
            string newLine = Environment.NewLine;
            Font font4 = font1;
            Brush black2 = Brushes.Black;
            marginBounds = e.MarginBounds;
            double left2 = (double) marginBounds.Left;
            marginBounds = e.MarginBounds;
            double y2 = (double) (marginBounds.Top + num1 * font1.Height);
            marginBounds = e.MarginBounds;
            double width3 = (double) marginBounds.Width;
            marginBounds = e.MarginBounds;
            double height3 = (double) marginBounds.Height;
            RectangleF layoutRectangle2 = new RectangleF((float) left2, (float) y2, (float) width3, (float) height3);
            graphics3.DrawString(newLine, font4, black2, layoutRectangle2);
            ++num1;
          }
        }
      }
    }

    private void pdPrintDoc_BeginPrint(object sender, PrintEventArgs e) => this.nextPrintItem = 0;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FieldHelpDialog));
      this.lstHelp = new ControlListBox();
      this.sfdSave = new SaveFileDialog();
      this.pdlgPrint = new PrintDialog();
      this.pdPrintDoc = new PrintDocument();
      this.ppvPreview = new PrintPreviewDialog();
      this.gradientPanel1 = new GradientPanel();
      this.flowLayoutPanel1 = new FlowLayoutPanel();
      this.btnSave = new StandardIconButton();
      this.btnPrint = new StandardIconButton();
      this.btnCopy = new StandardIconButton();
      this.label2 = new Label();
      this.toolTip1 = new ToolTip(this.components);
      this.gradientPanel1.SuspendLayout();
      this.flowLayoutPanel1.SuspendLayout();
      ((ISupportInitialize) this.btnSave).BeginInit();
      ((ISupportInitialize) this.btnPrint).BeginInit();
      ((ISupportInitialize) this.btnCopy).BeginInit();
      this.SuspendLayout();
      this.lstHelp.BackColor = Color.WhiteSmoke;
      this.lstHelp.Dock = DockStyle.Fill;
      this.lstHelp.Footer = (Control) null;
      this.lstHelp.Header = (Control) null;
      this.lstHelp.IntegralScrolling = false;
      this.lstHelp.Location = new Point(0, 26);
      this.lstHelp.Name = "lstHelp";
      this.lstHelp.Size = new Size(342, 422);
      this.lstHelp.TabIndex = 0;
      this.lstHelp.TopIndex = 0;
      this.sfdSave.DefaultExt = "txt";
      this.sfdSave.FileName = "FieldHelp.txt";
      this.sfdSave.Filter = "Text Files|*.txt|All Files|*.*";
      this.sfdSave.Title = "Save Help Text to File";
      this.pdlgPrint.AllowPrintToFile = false;
      this.pdlgPrint.UseEXDialog = true;
      this.pdPrintDoc.DocumentName = "Encompass Field Help";
      this.pdPrintDoc.PrintPage += new PrintPageEventHandler(this.pdPrintDoc_PrintPage);
      this.pdPrintDoc.BeginPrint += new PrintEventHandler(this.pdPrintDoc_BeginPrint);
      this.ppvPreview.AutoScrollMargin = new Size(0, 0);
      this.ppvPreview.AutoScrollMinSize = new Size(0, 0);
      this.ppvPreview.ClientSize = new Size(400, 300);
      this.ppvPreview.Document = this.pdPrintDoc;
      this.ppvPreview.Enabled = true;
      this.ppvPreview.Icon = (Icon) componentResourceManager.GetObject("ppvPreview.Icon");
      this.ppvPreview.Name = "ppvPreview";
      this.ppvPreview.UseAntiAlias = true;
      this.ppvPreview.Visible = false;
      this.gradientPanel1.Controls.Add((Control) this.flowLayoutPanel1);
      this.gradientPanel1.Controls.Add((Control) this.label2);
      this.gradientPanel1.Dock = DockStyle.Top;
      this.gradientPanel1.Location = new Point(0, 0);
      this.gradientPanel1.Name = "gradientPanel1";
      this.gradientPanel1.Size = new Size(342, 26);
      this.gradientPanel1.Style = GradientPanel.PanelStyle.TableHeader;
      this.gradientPanel1.TabIndex = 2;
      this.flowLayoutPanel1.BackColor = Color.Transparent;
      this.flowLayoutPanel1.Controls.Add((Control) this.btnSave);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnPrint);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnCopy);
      this.flowLayoutPanel1.FlowDirection = FlowDirection.RightToLeft;
      this.flowLayoutPanel1.Location = new Point(258, 2);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      this.flowLayoutPanel1.Size = new Size(82, 22);
      this.flowLayoutPanel1.TabIndex = 2;
      this.btnSave.BackColor = Color.Transparent;
      this.btnSave.Location = new Point(63, 3);
      this.btnSave.Margin = new Padding(2, 3, 3, 3);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(16, 16);
      this.btnSave.StandardButtonType = StandardIconButton.ButtonType.SaveButton;
      this.btnSave.TabIndex = 0;
      this.btnSave.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnSave, "Save");
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.btnPrint.BackColor = Color.Transparent;
      this.btnPrint.Location = new Point(42, 3);
      this.btnPrint.Margin = new Padding(2, 3, 3, 3);
      this.btnPrint.Name = "btnPrint";
      this.btnPrint.Size = new Size(16, 16);
      this.btnPrint.StandardButtonType = StandardIconButton.ButtonType.PrintButton;
      this.btnPrint.TabIndex = 1;
      this.btnPrint.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnPrint, "Print");
      this.btnPrint.Click += new EventHandler(this.btnPrint_Click);
      this.btnCopy.BackColor = Color.Transparent;
      this.btnCopy.Location = new Point(21, 3);
      this.btnCopy.Margin = new Padding(2, 3, 3, 3);
      this.btnCopy.Name = "btnCopy";
      this.btnCopy.Size = new Size(16, 16);
      this.btnCopy.StandardButtonType = StandardIconButton.ButtonType.CopyButton;
      this.btnCopy.TabIndex = 2;
      this.btnCopy.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnCopy, "Copy");
      this.btnCopy.Click += new EventHandler(this.btnCopy_Click);
      this.label2.AutoSize = true;
      this.label2.BackColor = Color.Transparent;
      this.label2.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label2.ForeColor = Color.Black;
      this.label2.Location = new Point(7, 6);
      this.label2.Name = "label2";
      this.label2.Size = new Size(60, 14);
      this.label2.TabIndex = 1;
      this.label2.Text = "Field Help";
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(342, 448);
      this.Controls.Add((Control) this.lstHelp);
      this.Controls.Add((Control) this.gradientPanel1);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
      this.Name = nameof (FieldHelpDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.Manual;
      this.Text = "Encompass HelpPad";
      this.TopMost = true;
      this.gradientPanel1.ResumeLayout(false);
      this.gradientPanel1.PerformLayout();
      this.flowLayoutPanel1.ResumeLayout(false);
      ((ISupportInitialize) this.btnSave).EndInit();
      ((ISupportInitialize) this.btnPrint).EndInit();
      ((ISupportInitialize) this.btnCopy).EndInit();
      this.ResumeLayout(false);
    }
  }
}
