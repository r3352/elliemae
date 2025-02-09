// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.ObjectTypeDropdownBox
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class ObjectTypeDropdownBox : UserControl
  {
    private IContainer components;
    private ComboBox cboAutoHeight;
    private ComboBoxEx cboObjectTypes;

    public event EventHandler SelectedIndexChanged;

    public ObjectTypeDropdownBox()
    {
      this.InitializeComponent();
      this.cboObjectTypes.Items.Add((object) string.Empty);
      this.cboObjectTypes.Items.Add((object) new ObjectTypeLabel(ObjectTypeEnum.FileAttachment));
      this.cboObjectTypes.Items.Add((object) new ObjectTypeLabel(ObjectTypeEnum.ImageAttachment));
      this.cboObjectTypes.Items.Add((object) new ObjectTypeLabel(ObjectTypeEnum.Document));
      this.cboObjectTypes.Items.Add((object) new ObjectTypeLabel(ObjectTypeEnum.Condition));
      this.cboObjectTypes.Items.Add((object) new ObjectTypeLabel(ObjectTypeEnum.PageImage));
    }

    public string ObjectType
    {
      get
      {
        return this.cboObjectTypes.SelectedIndex < 1 ? (string) null : ((ObjectTypeLabel) this.cboObjectTypes.SelectedItem).ToString();
      }
      set
      {
        if ((value ?? "") == "")
        {
          this.cboObjectTypes.SelectedIndex = 0;
        }
        else
        {
          foreach (object obj in this.cboObjectTypes.Items)
          {
            if (obj is ObjectTypeLabel && ((ObjectTypeLabel) obj).ToString().Equals(value))
            {
              this.cboObjectTypes.SelectedItem = obj;
              break;
            }
          }
        }
      }
    }

    private void cboObjectTypes_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.OnSelectedIndexChanged(e);
    }

    protected void OnSelectedIndexChanged(EventArgs e)
    {
      if (this.SelectedIndexChanged == null)
        return;
      this.SelectedIndexChanged((object) this, e);
    }

    private void ObjectTypeDropdownBox_Load(object sender, EventArgs e)
    {
      this.Height = this.cboAutoHeight.Height;
      this.cboAutoHeight.Visible = false;
    }

    public override Size GetPreferredSize(Size proposedSize)
    {
      return new Size(proposedSize.Width, this.cboAutoHeight.Height);
    }

    protected override void OnResize(EventArgs e)
    {
      if (this.Height != this.cboAutoHeight.Height)
        this.Height = this.cboAutoHeight.Height;
      this.cboObjectTypes.ItemHeight = this.Height - 6;
      base.OnResize(e);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.cboAutoHeight = new ComboBox();
      this.cboObjectTypes = new ComboBoxEx();
      this.SuspendLayout();
      this.cboAutoHeight.FormattingEnabled = true;
      this.cboAutoHeight.Location = new Point(98, 84);
      this.cboAutoHeight.Name = "cboAutoHeight";
      this.cboAutoHeight.Size = new Size(10, 22);
      this.cboAutoHeight.TabIndex = 1;
      this.cboObjectTypes.Dock = DockStyle.Top;
      this.cboObjectTypes.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboObjectTypes.FormattingEnabled = true;
      this.cboObjectTypes.ItemHeight = 16;
      this.cboObjectTypes.Location = new Point(0, 0);
      this.cboObjectTypes.Name = "cboObjectTypes";
      this.cboObjectTypes.SelectedBGColor = SystemColors.Highlight;
      this.cboObjectTypes.Size = new Size(146, 22);
      this.cboObjectTypes.TabIndex = 0;
      this.cboObjectTypes.SelectedIndexChanged += new EventHandler(this.cboObjectTypes_SelectedIndexChanged);
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.cboAutoHeight);
      this.Controls.Add((Control) this.cboObjectTypes);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (ObjectTypeDropdownBox);
      this.Size = new Size(146, 27);
      this.Load += new EventHandler(this.ObjectTypeDropdownBox_Load);
      this.ResumeLayout(false);
    }
  }
}
