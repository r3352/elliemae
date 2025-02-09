// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.CDOHelpStepDialog
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ContactUI
{
  public class CDOHelpStepDialog : Form
  {
    private RichTextBox rtbSteps;
    private Button btnOK;
    private System.ComponentModel.Container components;

    public CDOHelpStepDialog() => this.InitializeComponent();

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.rtbSteps = new RichTextBox();
      this.btnOK = new Button();
      this.SuspendLayout();
      this.rtbSteps.BackColor = SystemColors.Control;
      this.rtbSteps.BorderStyle = BorderStyle.None;
      this.rtbSteps.Location = new Point(16, 8);
      this.rtbSteps.Name = "rtbSteps";
      this.rtbSteps.ReadOnly = true;
      this.rtbSteps.Size = new Size(464, 360);
      this.rtbSteps.TabIndex = 0;
      this.rtbSteps.Text = "";
      this.btnOK.Anchor = AnchorStyles.Bottom;
      this.btnOK.DialogResult = DialogResult.OK;
      this.btnOK.Location = new Point(208, 376);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(72, 24);
      this.btnOK.TabIndex = 2;
      this.btnOK.Text = "OK";
      this.AutoScaleBaseSize = new Size(5, 13);
      this.CancelButton = (IButtonControl) this.btnOK;
      this.ClientSize = new Size(490, 407);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.rtbSteps);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (CDOHelpStepDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Encompass";
      this.Load += new EventHandler(this.CDOHelpStepDialog_Load);
      this.ResumeLayout(false);
    }

    private void CDOHelpStepDialog_Load(object sender, EventArgs e)
    {
      this.rtbSteps.Rtf = "{\\rtf1\\ansi\\ansicpg1252\\uc1 \\deff0\\deflang1033\\deflangfe1033{\\fonttbl{\\f0\\froman\\fcharset0\\fprq2{\\*\\panose 02020603050405020304}Times New Roman;}{\\f2\\fmodern\\fcharset0\\fprq1{\\*\\panose 02070309020205020404}Courier New;}\r\n{\\f3\\froman\\fcharset2\\fprq2{\\*\\panose 05050102010706020507}Symbol;}{\\f14\\fnil\\fcharset2\\fprq2{\\*\\panose 05000000000000000000}Wingdings;}{\\f64\\fswiss\\fcharset0\\fprq2{\\*\\panose 020b0604020202020204}Microsoft Sans Serif;}\r\n{\\f107\\froman\\fcharset238\\fprq2 Times New Roman CE;}{\\f108\\froman\\fcharset204\\fprq2 Times New Roman Cyr;}{\\f110\\froman\\fcharset161\\fprq2 Times New Roman Greek;}{\\f111\\froman\\fcharset162\\fprq2 Times New Roman Tur;}\r\n{\\f112\\froman\\fcharset177\\fprq2 Times New Roman (Hebrew);}{\\f113\\froman\\fcharset178\\fprq2 Times New Roman (Arabic);}{\\f114\\froman\\fcharset186\\fprq2 Times New Roman Baltic;}{\\f123\\fmodern\\fcharset238\\fprq1 Courier New CE;}\r\n{\\f124\\fmodern\\fcharset204\\fprq1 Courier New Cyr;}{\\f126\\fmodern\\fcharset161\\fprq1 Courier New Greek;}{\\f127\\fmodern\\fcharset162\\fprq1 Courier New Tur;}{\\f128\\fmodern\\fcharset177\\fprq1 Courier New (Hebrew);}\r\n{\\f129\\fmodern\\fcharset178\\fprq1 Courier New (Arabic);}{\\f130\\fmodern\\fcharset186\\fprq1 Courier New Baltic;}{\\f619\\fswiss\\fcharset238\\fprq2 Microsoft Sans Serif CE;}{\\f620\\fswiss\\fcharset204\\fprq2 Microsoft Sans Serif Cyr;}\r\n{\\f622\\fswiss\\fcharset161\\fprq2 Microsoft Sans Serif Greek;}{\\f623\\fswiss\\fcharset162\\fprq2 Microsoft Sans Serif Tur;}{\\f624\\fswiss\\fcharset177\\fprq2 Microsoft Sans Serif (Hebrew);}{\\f625\\fswiss\\fcharset178\\fprq2 Microsoft Sans Serif (Arabic);}\r\n{\\f626\\fswiss\\fcharset186\\fprq2 Microsoft Sans Serif Baltic;}}{\\colortbl;\\red0\\green0\\blue0;\\red0\\green0\\blue255;\\red0\\green255\\blue255;\\red0\\green255\\blue0;\\red255\\green0\\blue255;\\red255\\green0\\blue0;\\red255\\green255\\blue0;\\red255\\green255\\blue255;\r\n\\red0\\green0\\blue128;\\red0\\green128\\blue128;\\red0\\green128\\blue0;\\red128\\green0\\blue128;\\red128\\green0\\blue0;\\red128\\green128\\blue0;\\red128\\green128\\blue128;\\red192\\green192\\blue192;}{\\stylesheet{\r\n\\ql \\li0\\ri0\\widctlpar\\aspalpha\\aspnum\\faauto\\adjustright\\rin0\\lin0\\itap0 \\fs24\\lang1033\\langfe1033\\cgrid\\langnp1033\\langfenp1033 \\snext0 Normal;}{\\*\\cs10 \\additive Default Paragraph Font;}{\r\n\\s15\\qc \\li0\\ri0\\widctlpar\\aspalpha\\aspnum\\faauto\\adjustright\\rin0\\lin0\\itap0 \\b\\fs22\\lang1033\\langfe1033\\cgrid\\langnp1033\\langfenp1033 \\sbasedon0 \\snext15 Title;}}{\\*\\listtable{\\list\\listtemplateid308603950\\listhybrid{\\listlevel\\levelnfc23\\levelnfcn23\r\n\\leveljc0\\leveljcn0\\levelfollow0\\levelstartat1\\levelspace360\\levelindent0{\\leveltext\\leveltemplateid67698689\\'01\\u-3913 ?;}{\\levelnumbers;}\\f3\\chbrdr\\brdrnone\\brdrcf1 \\chshdng0\\chcfpat1\\chcbpat1\\fbias0 \\fi-360\\li1080\\jclisttab\\tx1080 }{\\listlevel\r\n\\levelnfc23\\levelnfcn23\\leveljc0\\leveljcn0\\levelfollow0\\levelstartat1\\levelspace360\\levelindent0{\\leveltext\\leveltemplateid67698691\\'01o;}{\\levelnumbers;}\\f2\\chbrdr\\brdrnone\\brdrcf1 \\chshdng0\\chcfpat1\\chcbpat1\\fbias0 \\fi-360\\li1800\\jclisttab\\tx1800 }\r\n{\\listlevel\\levelnfc23\\levelnfcn23\\leveljc0\\leveljcn0\\levelfollow0\\levelstartat1\\levelspace360\\levelindent0{\\leveltext\\leveltemplateid67698693\\'01\\u-3929 ?;}{\\levelnumbers;}\\f14\\chbrdr\\brdrnone\\brdrcf1 \\chshdng0\\chcfpat1\\chcbpat1\\fbias0 \\fi-360\\li2520\r\n\\jclisttab\\tx2520 }{\\listlevel\\levelnfc23\\levelnfcn23\\leveljc0\\leveljcn0\\levelfollow0\\levelstartat1\\levelspace360\\levelindent0{\\leveltext\\leveltemplateid67698689\\'01\\u-3913 ?;}{\\levelnumbers;}\\f3\\chbrdr\\brdrnone\\brdrcf1 \\chshdng0\\chcfpat1\\chcbpat1\\fbias0 \r\n\\fi-360\\li3240\\jclisttab\\tx3240 }{\\listlevel\\levelnfc23\\levelnfcn23\\leveljc0\\leveljcn0\\levelfollow0\\levelstartat1\\levelspace360\\levelindent0{\\leveltext\\leveltemplateid67698691\\'01o;}{\\levelnumbers;}\\f2\\chbrdr\\brdrnone\\brdrcf1 \r\n\\chshdng0\\chcfpat1\\chcbpat1\\fbias0 \\fi-360\\li3960\\jclisttab\\tx3960 }{\\listlevel\\levelnfc23\\levelnfcn23\\leveljc0\\leveljcn0\\levelfollow0\\levelstartat1\\levelspace360\\levelindent0{\\leveltext\\leveltemplateid67698693\\'01\\u-3929 ?;}{\\levelnumbers;}\\f14\\chbrdr\r\n\\brdrnone\\brdrcf1 \\chshdng0\\chcfpat1\\chcbpat1\\fbias0 \\fi-360\\li4680\\jclisttab\\tx4680 }{\\listlevel\\levelnfc23\\levelnfcn23\\leveljc0\\leveljcn0\\levelfollow0\\levelstartat1\\levelspace360\\levelindent0{\\leveltext\\leveltemplateid67698689\r\n\\'01\\u-3913 ?;}{\\levelnumbers;}\\f3\\chbrdr\\brdrnone\\brdrcf1 \\chshdng0\\chcfpat1\\chcbpat1\\fbias0 \\fi-360\\li5400\\jclisttab\\tx5400 }{\\listlevel\\levelnfc23\\levelnfcn23\\leveljc0\\leveljcn0\\levelfollow0\\levelstartat1\\levelspace360\\levelindent0{\\leveltext\r\n\\leveltemplateid67698691\\'01o;}{\\levelnumbers;}\\f2\\chbrdr\\brdrnone\\brdrcf1 \\chshdng0\\chcfpat1\\chcbpat1\\fbias0 \\fi-360\\li6120\\jclisttab\\tx6120 }{\\listlevel\\levelnfc23\\levelnfcn23\\leveljc0\\leveljcn0\\levelfollow0\\levelstartat1\\levelspace360\\levelindent0\r\n{\\leveltext\\leveltemplateid67698693\\'01\\u-3929 ?;}{\\levelnumbers;}\\f14\\chbrdr\\brdrnone\\brdrcf1 \\chshdng0\\chcfpat1\\chcbpat1\\fbias0 \\fi-360\\li6840\\jclisttab\\tx6840 }{\\listname ;}\\listid379979996}}{\\*\\listoverridetable{\\listoverride\\listid379979996\r\n\\listoverridecount0\\ls1}}{\\info{\\title Steps for Installing Collaboration Data Objects}{\\author Amcdona}{\\operator Jli}{\\creatim\\yr2003\\mo11\\dy12\\hr14}{\\revtim\\yr2003\\mo11\\dy12\\hr14\\min34}{\\printim\\yr2003\\mo11\\dy12\\hr13\\min23}{\\version3}{\\edmins43}\r\n{\\nofpages1}{\\nofwords0}{\\nofchars0}{\\*\\company Ellie Mae}{\\nofcharsws0}{\\vern8269}}\\widowctrl\\ftnbj\\aenddoc\\noxlattoyen\\expshrtn\\noultrlspc\\dntblnsbdb\\nospaceforul\\hyphcaps0\\formshade\\horzdoc\\dgmargin\\dghspace180\\dgvspace180\\dghorigin1800\\dgvorigin1440\r\n\\dghshow1\\dgvshow1\\jexpand\\viewkind1\\viewscale100\\pgbrdrhead\\pgbrdrfoot\\splytwnine\\ftnlytwnine\\htmautsp\\nolnhtadjtbl\\useltbaln\\alntblind\\lytcalctblwd\\lyttblrtgr\\lnbrkrule \\fet0\\sectd \\linex0\\endnhere\\sectlinegrid360\\sectdefaultcl {\\*\\pnseclvl1\r\n\\pnucrm\\pnstart1\\pnindent720\\pnhang{\\pntxta .}}{\\*\\pnseclvl2\\pnucltr\\pnstart1\\pnindent720\\pnhang{\\pntxta .}}{\\*\\pnseclvl3\\pndec\\pnstart1\\pnindent720\\pnhang{\\pntxta .}}{\\*\\pnseclvl4\\pnlcltr\\pnstart1\\pnindent720\\pnhang{\\pntxta )}}{\\*\\pnseclvl5\r\n\\pndec\\pnstart1\\pnindent720\\pnhang{\\pntxtb (}{\\pntxta )}}{\\*\\pnseclvl6\\pnlcltr\\pnstart1\\pnindent720\\pnhang{\\pntxtb (}{\\pntxta )}}{\\*\\pnseclvl7\\pnlcrm\\pnstart1\\pnindent720\\pnhang{\\pntxtb (}{\\pntxta )}}{\\*\\pnseclvl8\\pnlcltr\\pnstart1\\pnindent720\\pnhang\r\n{\\pntxtb (}{\\pntxta )}}{\\*\\pnseclvl9\\pnlcrm\\pnstart1\\pnindent720\\pnhang{\\pntxtb (}{\\pntxta )}}\\pard\\plain \\s15\\qc \\li0\\ri0\\widctlpar\\aspalpha\\aspnum\\faauto\\adjustright\\rin0\\lin0\\itap0 \\b\\fs22\\lang1033\\langfe1033\\cgrid\\langnp1033\\langfenp1033 {\\f64 \r\nSteps for Installing Collaboration Data Objects\r\n\\par }\\pard\\plain \\ql \\li0\\ri0\\widctlpar\\aspalpha\\aspnum\\faauto\\adjustright\\rin0\\lin0\\itap0 \\fs24\\lang1033\\langfe1033\\cgrid\\langnp1033\\langfenp1033 {\\f64\\fs20 \r\n\\par }\\pard \\ql \\li0\\ri0\\sa80\\widctlpar\\tx360\\aspalpha\\aspnum\\faauto\\adjustright\\rin0\\lin0\\itap0 {\\f64\\fs18 1.\\tab On the Windows taskbar, click }{\\b\\f64\\fs18 Start}{\\f64\\fs18 , Point to }{\\b\\f64\\fs18 Settings}{\\f64\\fs18 , and then click }{\\b\\f64\\fs18 \r\nControl Panel}{\\f64\\fs18 .\r\n\\par 2.\\tab On the Control Panel, double-click }{\\b\\f64\\fs18 Add or Remove Programs}{\\f64\\fs18 .\r\n\\par 3.\\tab From the list of programs, select your Microsoft Office application, and then click }{\\b\\f64\\fs18 Change}{\\f64\\fs18 .\r\n\\par {\\listtext\\pard\\plain\\f3\\fs18 \\loch\\af3\\dbch\\af0\\hich\\f3 \\'b7\\tab}}\\pard \\ql \\fi-360\\li720\\ri0\\sa80\\widctlpar\\tx720\\aspalpha\\aspnum\\faauto\\ls1\\adjustright\\rin0\\lin720\\itap0 {\\f64\\fs18 If you are using the Windows 98 operating system, click }{\\b\\f64\\fs18 \r\nAdd/Remove}{\\f64\\fs18 .\r\n\\par }\\pard \\ql \\li0\\ri0\\sa80\\widctlpar\\tx360\\aspalpha\\aspnum\\faauto\\adjustright\\rin0\\lin0\\itap0 {\\f64\\fs18 4.\\tab When the update wizard opens, select the }{\\b\\f64\\fs18 Add or Remove}{\\f64\\fs18  option and then click }{\\b\\f64\\fs18 Next}{\\f64\\fs18 .\r\n\\par {\\listtext\\pard\\plain\\f3\\fs18 \\loch\\af3\\dbch\\af0\\hich\\f3 \\'b7\\tab}}\\pard \\ql \\fi-360\\li720\\ri0\\sa80\\widctlpar\\tx360\\jclisttab\\tx720\\aspalpha\\aspnum\\faauto\\ls1\\adjustright\\rin0\\lin720\\itap0 {\\f64\\fs18 \r\nIf you are using Microsoft Office 2000, you do not need to click }{\\b\\f64\\fs18 Next}{\\f64\\fs18 .\r\n\\par }\\pard \\ql \\li0\\ri0\\sa80\\widctlpar\\tx360\\aspalpha\\aspnum\\faauto\\adjustright\\rin0\\lin0\\itap0 {\\f64\\fs18 5.\\tab Click to expand the }{\\b\\f64\\fs18 Microsoft Outlook for Windows}{\\f64\\fs18  option.\r\n\\par }\\pard \\ql \\fi-360\\li360\\ri0\\sa80\\widctlpar\\tx360\\aspalpha\\aspnum\\faauto\\adjustright\\rin0\\lin360\\itap0 {\\f64\\fs18 6.\\tab Click }{\\b\\f64\\fs18 Collaboration Data Objects}{\\f64\\fs18 , select }{\\b\\f64\\fs18 Run from My Computer}{\\f64\\fs18 , and then click }{\r\n\\b\\f64\\fs18 Update }{\\f64\\fs18 (or }{\\b\\f64\\fs18 Update Now}{\\f64\\fs18 ).\r\n\\par {\\listtext\\pard\\plain\\f3\\fs18 \\loch\\af3\\dbch\\af0\\hich\\f3 \\'b7\\tab}}\\pard \\ql \\fi-360\\li720\\ri0\\sa80\\widctlpar\\tx720\\aspalpha\\aspnum\\faauto\\ls1\\adjustright\\rin0\\lin720\\itap0 {\\f64\\fs18 \r\nYou may be prompted to insert your Microsoft Office installation CD to complete the update.\r\n\\par }\\pard \\ql \\li0\\ri0\\sa80\\widctlpar\\tx360\\aspalpha\\aspnum\\faauto\\adjustright\\rin0\\lin0\\itap0 {\\f64\\fs18 7.\\tab When the update is completed, click }{\\b\\f64\\fs18 OK }{\\f64\\fs18 on the successful completion message.\r\n\\par 8. \\tab Close the Control Panel.}{\\fs20 \r\n\\par }}";
    }
  }
}
