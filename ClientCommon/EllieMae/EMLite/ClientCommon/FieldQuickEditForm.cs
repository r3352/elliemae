// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientCommon.FieldQuickEditForm
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.BusinessRules;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ClientCommon
{
  public class FieldQuickEditForm : Form
  {
    private FieldQuickEditor editor;
    private Sessions.Session session;
    private IContainer components;
    private Button btnOK;
    private BorderPanel borderPanel1;
    private Panel panelMiddle;
    private Panel panelFields;

    public FieldQuickEditForm(Sessions.Session session, string[] requiredFields)
    {
      this.session = session;
      this.InitializeComponent();
      this.editor = new FieldQuickEditor(this.session);
      this.editor.RefreshFieldList(requiredFields, true);
      this.panelFields.Controls.Add((Control) this.editor);
      this.panelFields.Height = this.editor.MaxHeight + 10;
      if (this.panelFields.Height >= this.panelMiddle.Height)
        return;
      this.panelFields.Width = this.panelMiddle.Width - 2;
    }

    public FieldQuickEditForm(Sessions.Session session, LoanData loan, ArrayList requiredList)
    {
      this.session = session;
      this.InitializeComponent();
      Hashtable insensitiveHashtable = CollectionsUtil.CreateCaseInsensitiveHashtable();
      for (int index1 = 0; index1 < requiredList.Count; ++index1)
      {
        ArrayList required = (ArrayList) requiredList[index1];
        for (int index2 = 0; index2 < required.Count; ++index2)
        {
          if (!(required[index2] is PrintFormRuleValidators) && !insensitiveHashtable.ContainsKey((object) required[index2].ToString()))
            insensitiveHashtable.Add((object) required[index2].ToString(), (object) "");
        }
      }
      string[] requiredFields = new string[insensitiveHashtable.Count];
      insensitiveHashtable.Keys.CopyTo((Array) requiredFields, 0);
      this.editor = new FieldQuickEditor(this.session, loan, FieldQuickEditorMode.Other, false);
      this.editor.RefreshFieldList(requiredFields, true);
      this.panelFields.Controls.Add((Control) this.editor);
      this.panelFields.Height = this.editor.MaxHeight + 10;
      if (this.panelFields.Height >= this.panelMiddle.Height)
        return;
      this.panelFields.Width = this.panelMiddle.Width - 2;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.btnOK = new Button();
      this.borderPanel1 = new BorderPanel();
      this.panelMiddle = new Panel();
      this.panelFields = new Panel();
      this.borderPanel1.SuspendLayout();
      this.panelMiddle.SuspendLayout();
      this.SuspendLayout();
      this.btnOK.DialogResult = DialogResult.OK;
      this.btnOK.Location = new Point(497, 486);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 0;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.borderPanel1.AutoScroll = true;
      this.borderPanel1.BackColor = Color.WhiteSmoke;
      this.borderPanel1.Controls.Add((Control) this.panelMiddle);
      this.borderPanel1.Location = new Point(1, 1);
      this.borderPanel1.Name = "borderPanel1";
      this.borderPanel1.Size = new Size(583, 480);
      this.borderPanel1.TabIndex = 2;
      this.panelMiddle.AutoScroll = true;
      this.panelMiddle.Controls.Add((Control) this.panelFields);
      this.panelMiddle.Location = new Point(1, 1);
      this.panelMiddle.Name = "panelMiddle";
      this.panelMiddle.Size = new Size(581, 478);
      this.panelMiddle.TabIndex = 0;
      this.panelFields.Location = new Point(1, 1);
      this.panelFields.Name = "panelFields";
      this.panelFields.Size = new Size(563, 476);
      this.panelFields.TabIndex = 0;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(584, 522);
      this.Controls.Add((Control) this.borderPanel1);
      this.Controls.Add((Control) this.btnOK);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (FieldQuickEditForm);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Complete Fields";
      this.borderPanel1.ResumeLayout(false);
      this.panelMiddle.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
