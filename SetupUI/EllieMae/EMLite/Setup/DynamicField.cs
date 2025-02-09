// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.DynamicField
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Properties;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  internal class DynamicField
  {
    private TextBox newBox;
    private ComboBox newComboBox;
    private Label newLabel;
    private StandardIconButton removedbutton;
    private PictureBox newWarningIcon;

    public DynamicField(
      TextBox textBox,
      Label label,
      StandardIconButton removedbutton,
      PictureBox newWarningIcon)
    {
      this.newBox = textBox;
      this.newLabel = label;
      this.removedbutton = removedbutton;
      this.newWarningIcon = newWarningIcon;
      this.newComboBox = (ComboBox) null;
    }

    public DynamicField(
      ComboBox comboBox,
      Label label,
      StandardIconButton removedbutton,
      PictureBox newWarningIcon)
    {
      this.newComboBox = comboBox;
      this.newLabel = label;
      this.removedbutton = removedbutton;
      this.newWarningIcon = newWarningIcon;
      this.newBox = (TextBox) null;
    }

    public DynamicField(int width, int height, bool useDropdown)
    {
      if (useDropdown)
      {
        this.newComboBox = new ComboBox();
        this.newComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        this.newComboBox.FormattingEnabled = true;
        this.newComboBox.Location = new Point(93, 71);
        this.newComboBox.Name = Guid.NewGuid().ToString();
        this.newComboBox.Size = new Size(width, 21);
      }
      else
      {
        this.newBox = new TextBox();
        this.newBox.Name = Guid.NewGuid().ToString();
        this.newBox.Size = new Size(width, height);
      }
      this.newLabel = new Label();
      this.newLabel.Name = Guid.NewGuid().ToString();
      this.newLabel.Text = "Site URL";
      this.newLabel.AutoSize = true;
      this.removedbutton = new StandardIconButton();
      this.removedbutton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.removedbutton.BackColor = Color.Transparent;
      this.removedbutton.MouseDownImage = (Image) Resources.delete_over;
      StandardIconButton removedbutton = this.removedbutton;
      Guid guid = Guid.NewGuid();
      string str1 = guid.ToString();
      removedbutton.Name = str1;
      this.removedbutton.Size = new Size(16, 16);
      this.removedbutton.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.newWarningIcon = new PictureBox();
      this.newWarningIcon.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.newWarningIcon.Image = (Image) Resources.alert;
      PictureBox newWarningIcon = this.newWarningIcon;
      guid = Guid.NewGuid();
      string str2 = guid.ToString();
      newWarningIcon.Name = str2;
      this.newWarningIcon.Size = new Size(16, 16);
      this.newWarningIcon.SizeMode = PictureBoxSizeMode.AutoSize;
    }

    public void Add(UserControl c, int toTop, int toLeft)
    {
      this.relocated(toTop, toLeft);
      c.Controls.Add((Control) this.newLabel);
      if (this.newBox != null)
        c.Controls.Add((Control) this.newBox);
      if (this.newComboBox != null)
        c.Controls.Add((Control) this.newComboBox);
      c.Controls.Add((Control) this.removedbutton);
      c.Controls.Add((Control) this.newWarningIcon);
    }

    public void Remove(UserControl c)
    {
      if (c.Contains((Control) this.newLabel))
        c.Controls.Remove((Control) this.newLabel);
      if (this.newBox != null && c.Contains((Control) this.newBox))
        c.Controls.Remove((Control) this.newBox);
      else if (this.newComboBox != null && c.Contains((Control) this.newComboBox))
        c.Controls.Remove((Control) this.newComboBox);
      if (c.Contains((Control) this.removedbutton))
        c.Controls.Remove((Control) this.removedbutton);
      if (!c.Contains((Control) this.newWarningIcon))
        return;
      c.Controls.Remove((Control) this.newWarningIcon);
    }

    public void Relocate(int toTop) => this.relocated(toTop, this.newBox.Left);

    private void relocated(int toTop, int toLeft)
    {
      if (this.newBox != null)
      {
        this.newBox.Left = toLeft;
        this.newBox.Top = toTop;
      }
      else
      {
        this.newComboBox.Left = toLeft;
        this.newComboBox.Top = toTop;
      }
      this.newLabel.Left = 17;
      this.newLabel.Top = toTop + 3;
      this.removedbutton.Top = this.newWarningIcon.Top = toTop + 3;
      this.removedbutton.Left = (this.newBox != null ? this.newBox.Left + this.newBox.Width : this.newComboBox.Left + this.newComboBox.Width) + 3;
      this.newWarningIcon.Left = this.removedbutton.Left + this.removedbutton.Width + 3;
    }

    public TextBox NewBox => this.newBox;

    public ComboBox NewComboBox => this.newComboBox;

    public Label NewLabel => this.newLabel;

    public StandardIconButton Removedbutton => this.removedbutton;

    public PictureBox NewWarningIcon => this.newWarningIcon;

    public void InitControlValue(List<string> availableList, string currentlySelectedValue)
    {
      if (this.newBox != null)
      {
        this.newBox.Text = currentlySelectedValue;
      }
      else
      {
        this.newComboBox.Items.AddRange((object[]) availableList.ToArray());
        this.newComboBox.Text = currentlySelectedValue;
      }
    }

    public string Value => this.newBox != null ? this.newBox.Text : this.newComboBox.Text;
  }
}
