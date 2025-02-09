// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.AsmResolver.InstallationUrlBrowserControl
// Assembly: EllieMae.Encompass.AsmResolver, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: DF61C82F-0411-4587-80AB-293D90FB58E7
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\EllieMae.Encompass.AsmResolver.dll

using EllieMae.Encompass.AsmResolver.Utils;
using Microsoft.Web.Administration;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.DirectoryServices;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.Encompass.AsmResolver
{
  public class InstallationUrlBrowserControl : UserControl
  {
    private InstallationUrlBrowserControl.Type browserType;
    private bool isIIS7;
    private IContainer components;
    private TextBox txtBoxURL;
    private Label lblPath;
    private TreeView tvDirectory;
    private ImageList imgLst;
    private Panel pnlURL;
    private Panel pnlSSL;
    private CheckBox chkBoxSSL;

    public InstallationUrlBrowserControl(
      InstallationUrlBrowserControl.Type type,
      string installationUrl)
    {
      this.InitializeComponent();
      this.browserType = type;
      this.resetPage(this.browserType);
      if ((type != InstallationUrlBrowserControl.Type.FileSystem || BasicUtils.IsHttpOrHttps(installationUrl)) && (type != InstallationUrlBrowserControl.Type.LocalIIS || !BasicUtils.IsHttpOrHttps(installationUrl)))
        return;
      this.txtBoxURL.Text = installationUrl;
    }

    public string InstallationURL => this.txtBoxURL.Text;

    private void resetPage(InstallationUrlBrowserControl.Type browserType)
    {
      switch (this.browserType)
      {
        case InstallationUrlBrowserControl.Type.FileSystem:
          this.pnlSSL.Visible = false;
          this.lblPath.Text = "Folder:";
          this.initialFileSystem();
          break;
        case InstallationUrlBrowserControl.Type.LocalIIS:
          this.pnlSSL.Visible = true;
          this.lblPath.Text = "Web site:";
          this.initialLocalIIS();
          break;
      }
    }

    private void initialFileSystem()
    {
      TreeNode systemSpecialFolder1 = this.getFileSystemSpecialFolder(Environment.SpecialFolder.Desktop);
      TreeNode systemSpecialFolder2 = this.getFileSystemSpecialFolder(Environment.SpecialFolder.MyComputer);
      this.insertFileSystemChildNode(systemSpecialFolder1);
      systemSpecialFolder1.Nodes.Insert(0, systemSpecialFolder2);
      this.tvDirectory.Nodes.Add(systemSpecialFolder1);
    }

    private TreeNode getFileSystemSpecialFolder(Environment.SpecialFolder folder)
    {
      TreeNode systemSpecialFolder = (TreeNode) null;
      switch (folder)
      {
        case Environment.SpecialFolder.Desktop:
          systemSpecialFolder = new TreeNode("Desktop");
          systemSpecialFolder.Tag = (object) new InstallationUrlBrowserControl.SiteInfo(Environment.GetFolderPath(folder));
          systemSpecialFolder.StateImageIndex = 0;
          break;
        case Environment.SpecialFolder.MyComputer:
          systemSpecialFolder = new TreeNode("My Computer");
          systemSpecialFolder.StateImageIndex = 1;
          foreach (string logicalDrive in Environment.GetLogicalDrives())
          {
            DriveInfo driveInfo = new DriveInfo(logicalDrive);
            TreeNode node = new TreeNode(driveInfo.Name);
            switch (driveInfo.DriveType)
            {
              case DriveType.Removable:
              case DriveType.Fixed:
                node.StateImageIndex = 2;
                break;
              case DriveType.Network:
                node.StateImageIndex = 4;
                break;
              case DriveType.CDRom:
                node.StateImageIndex = 3;
                break;
            }
            node.Tag = (object) new InstallationUrlBrowserControl.SiteInfo(logicalDrive);
            systemSpecialFolder.Nodes.Add(node);
            node.Nodes.Add("empty");
          }
          break;
      }
      return systemSpecialFolder;
    }

    private void insertFileSystemChildNode(TreeNode node)
    {
      try
      {
        string[] directories = Directory.GetDirectories(((InstallationUrlBrowserControl.SiteInfo) node.Tag).Path);
        if (directories == null || directories.Length == 0)
          return;
        foreach (string path in directories)
        {
          string[] strArray = path.Split('\\');
          node.Nodes.Add(new TreeNode(strArray[strArray.Length - 1])
          {
            Tag = (object) new InstallationUrlBrowserControl.SiteInfo(path),
            StateImageIndex = 6,
            Nodes = {
              "empty"
            }
          });
        }
      }
      catch
      {
      }
    }

    private void initialLocalIIS()
    {
      if (this.tvDirectory.Nodes.Count != 0)
        return;
      TreeNode treeNode = new TreeNode("Local Web Servers");
      treeNode.StateImageIndex = 7;
      treeNode.Tag = (object) new InstallationUrlBrowserControl.SiteInfo("IIS://localhost/W3SVC");
      treeNode.ExpandAll();
      this.fillIISWebSites(treeNode);
      this.tvDirectory.Nodes.Add(treeNode);
    }

    private void fillIISWebSites(TreeNode parentNode)
    {
      RegistryKey registryKey;
      try
      {
        registryKey = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\InetStp");
        if (registryKey == null)
          return;
      }
      catch (Exception ex)
      {
        int num = (int) AssemblyResolver.DisplayOrLogMessage(EventLogEntryType.Error, "There is no IIS server installed in this machine.");
        return;
      }
      using (registryKey)
      {
        if ((registryKey.GetValue("MajorVersion")?.ToString() ?? "") == "7")
          this.isIIS7 = true;
      }
      if (!this.isIIS7)
      {
        DirectoryEntry directoryEntry = new DirectoryEntry(((InstallationUrlBrowserControl.SiteInfo) parentNode.Tag).Path);
        if (directoryEntry == null)
          return;
        string text = "";
        try
        {
          foreach (DirectoryEntry child in directoryEntry.Children)
          {
            if (!(child.SchemaClassName != "IIsWebServer"))
            {
              if (child.Properties.Contains("ServerComment"))
                text = child.Properties["ServerComment"][0]?.ToString() ?? "";
              PropertyValueCollection property = child.Properties["ServerBindings"];
              string str = "";
              foreach (object obj in (CollectionBase) property)
              {
                string[] strArray = obj.ToString().Split(':');
                if (strArray != null && strArray.Length > 2)
                  str = !(str == "") ? str + "; Port: " + strArray[1] : " (Port:" + strArray[1];
              }
              if (str != "")
                str += ")";
              text += str;
              TreeNode treeNode = new TreeNode(text);
              treeNode.StateImageIndex = 8;
              treeNode.Tag = (object) new InstallationUrlBrowserControl.SiteInfo(child.Path + "\\Root")
              {
                Tag = (object) "Web Site"
              };
              parentNode.Nodes.Add(treeNode);
              this.fillIISVirtualDirectory(treeNode);
              this.fillIISFileSystem(treeNode);
            }
          }
        }
        catch (Exception ex)
        {
        }
      }
      else
        this.fillIIS7WebSites(parentNode);
    }

    private void fillIISVirtualDirectory(TreeNode parentNode)
    {
      if (!this.isIIS7)
      {
        DirectoryEntry directoryEntry = new DirectoryEntry(((InstallationUrlBrowserControl.SiteInfo) parentNode.Tag).Path);
        try
        {
          foreach (DirectoryEntry child in directoryEntry.Children)
          {
            if (child.SchemaClassName.ToLower() == "iiswebvirtualdir")
            {
              TreeNode node = new TreeNode(child.Name);
              node.Tag = (object) new InstallationUrlBrowserControl.SiteInfo(child.Properties["Path"][0]?.ToString() ?? "");
              node.StateImageIndex = 9;
              parentNode.Nodes.Add(node);
              node.Nodes.Add("empty");
            }
          }
        }
        catch (Exception ex)
        {
        }
        finally
        {
          directoryEntry.Close();
        }
      }
      else
        this.fillIIS7VirtualDirectory(parentNode);
    }

    private void fillIIS7WebSites(TreeNode parentNode)
    {
      foreach (Microsoft.Web.Administration.Site site in (ConfigurationElementCollectionBase<Microsoft.Web.Administration.Site>) new ServerManager().Sites)
      {
        TreeNode treeNode = new TreeNode(site.Name);
        InstallationUrlBrowserControl.SiteInfo siteInfo = new InstallationUrlBrowserControl.SiteInfo("");
        siteInfo.Tag = (object) site.Applications;
        string str1 = "";
        IEnumerator<Microsoft.Web.Administration.Binding> enumerator = site.Bindings.GetEnumerator();
        while (enumerator.MoveNext())
        {
          Microsoft.Web.Administration.Binding current = enumerator.Current;
          KeyValuePair<string, string> binding;
          ref KeyValuePair<string, string> local = ref binding;
          int port = current.EndPoint.Port;
          string key = port.ToString();
          string protocol1 = current.Protocol;
          local = new KeyValuePair<string, string>(key, protocol1);
          siteInfo.AddBinding(binding);
          if (str1 == "")
          {
            string protocol2 = current.Protocol;
            port = current.EndPoint.Port;
            string str2 = port.ToString();
            str1 = " (" + protocol2 + "://*:" + str2;
          }
          else
          {
            string[] strArray = new string[5]
            {
              str1,
              "; ",
              current.Protocol,
              "://*:",
              null
            };
            port = current.EndPoint.Port;
            strArray[4] = port.ToString();
            str1 = string.Concat(strArray);
          }
        }
        if (str1 != "")
          str1 += ")";
        treeNode.Text += str1;
        treeNode.Tag = (object) siteInfo;
        treeNode.StateImageIndex = 9;
        this.fillIISVirtualDirectory(treeNode);
        parentNode.Nodes.Add(treeNode);
      }
    }

    private void fillIIS7VirtualDirectory(TreeNode parentNode)
    {
      InstallationUrlBrowserControl.SiteInfo tag = (InstallationUrlBrowserControl.SiteInfo) parentNode.Tag;
      foreach (VirtualDirectory virtualDirectory in (ConfigurationElementCollectionBase<VirtualDirectory>) ((ConfigurationElementCollectionBase<Microsoft.Web.Administration.Application>) tag.Tag)[0].VirtualDirectories)
      {
        string path = virtualDirectory.PhysicalPath;
        if (path.IndexOf("%SystemDrive%") > -1)
          path = path.Replace("%SystemDrive%", Environment.GetEnvironmentVariable("%SystemDrive%"));
        if (virtualDirectory.Path == "/")
        {
          tag.Path = path;
          parentNode.Tag = (object) tag;
          this.fillIISFileSystem(parentNode);
        }
        else
        {
          TreeNode treeNode = new TreeNode(virtualDirectory.Path.Replace("/", ""));
          treeNode.Tag = (object) new InstallationUrlBrowserControl.SiteInfo(path)
          {
            BindingInfo = tag.BindingInfo
          };
          treeNode.StateImageIndex = 9;
          this.fillIISFileSystem(treeNode);
          parentNode.Nodes.Add(treeNode);
        }
      }
    }

    private void fillIISFileSystem(TreeNode parentNode)
    {
      string[] strArray1 = this.isIIS7 ? Directory.GetDirectories(((InstallationUrlBrowserControl.SiteInfo) parentNode.Tag).Path) : Directory.GetDirectories(Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\InetStp").GetValue("PathWWWRoot").ToString());
      if (strArray1 == null || strArray1.Length == 0)
        return;
      foreach (string path in strArray1)
      {
        string[] strArray2 = path.Split('\\');
        TreeNode node = new TreeNode(strArray2[strArray2.Length - 1]);
        if (!this.isIIS7)
          node.Tag = (object) new InstallationUrlBrowserControl.SiteInfo(path);
        else
          node.Tag = (object) new InstallationUrlBrowserControl.SiteInfo(path)
          {
            BindingInfo = ((InstallationUrlBrowserControl.SiteInfo) parentNode.Tag).BindingInfo
          };
        node.StateImageIndex = 6;
        parentNode.Nodes.Add(node);
        node.Nodes.Add("empty");
      }
    }

    private void tvDirectory_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
    {
      if (e.Node.Text.StartsWith("Desktop") || e.Node.Text.StartsWith("My Computer") || e.Node.Text.StartsWith("Local Web Servers") || e.Node.Text.StartsWith("Default Web Site") || ((InstallationUrlBrowserControl.SiteInfo) e.Node.Tag).Tag != null)
        return;
      this.tvDirectory.BeginUpdate();
      e.Node.Nodes.Clear();
      this.insertFileSystemChildNode(e.Node);
      this.tvDirectory.EndUpdate();
      if (this.browserType == InstallationUrlBrowserControl.Type.FileSystem)
      {
        this.txtBoxURL.Text = ((InstallationUrlBrowserControl.SiteInfo) e.Node.Tag).Path;
      }
      else
      {
        string str = e.Node.FullPath;
        int startIndex = str.IndexOf("\\", 18);
        if (startIndex > -1)
          str = str.Substring(startIndex);
        this.txtBoxURL.Text = ((this.chkBoxSSL.Checked ? ResolverConsts.HttpsPrefix : ResolverConsts.HttpPrefix) + "localhost" + str).Replace("\\", "/");
      }
    }

    private void tvDirectory_AfterCollapse(object sender, TreeViewEventArgs e)
    {
      this.tvDirectory.SelectedNode = e.Node;
      TreeNode node = e.Node;
      Rectangle bounds = e.Node.Bounds;
      int x = bounds.X;
      bounds = e.Node.Bounds;
      int y = bounds.Y;
      TreeNodeMouseClickEventArgs e1 = new TreeNodeMouseClickEventArgs(node, MouseButtons.Left, 1, x, y);
      this.tvDirectory_NodeMouseClick(sender, e1);
    }

    private void tvDirectory_AfterExpand(object sender, TreeViewEventArgs e)
    {
      this.tvDirectory.SelectedNode = e.Node;
      TreeNode node = e.Node;
      Rectangle bounds = e.Node.Bounds;
      int x = bounds.X;
      bounds = e.Node.Bounds;
      int y = bounds.Y;
      TreeNodeMouseClickEventArgs e1 = new TreeNodeMouseClickEventArgs(node, MouseButtons.Left, 1, x, y);
      this.tvDirectory_NodeMouseClick(sender, e1);
    }

    private void chkBoxSSL_CheckedChanged(object sender, EventArgs e)
    {
      if (this.chkBoxSSL.Checked)
      {
        if (!this.txtBoxURL.Text.ToLower().StartsWith(ResolverConsts.HttpPrefix))
          return;
        this.txtBoxURL.Text = this.txtBoxURL.Text.Insert(4, "s");
      }
      else
      {
        if (!this.txtBoxURL.Text.ToLower().StartsWith(ResolverConsts.HttpsPrefix))
          return;
        this.txtBoxURL.Text = this.txtBoxURL.Text.Remove(4, 1);
      }
    }

    private void txtBoxURL_TextChanged(object sender, EventArgs e)
    {
      if (this.txtBoxURL.Text.ToLower().StartsWith(ResolverConsts.HttpPrefix))
      {
        this.chkBoxSSL.Checked = false;
      }
      else
      {
        if (!this.txtBoxURL.Text.ToLower().StartsWith(ResolverConsts.HttpsPrefix))
          return;
        this.chkBoxSSL.Checked = true;
      }
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (InstallationUrlBrowserControl));
      this.tvDirectory = new TreeView();
      this.imgLst = new ImageList(this.components);
      this.pnlURL = new Panel();
      this.txtBoxURL = new TextBox();
      this.lblPath = new Label();
      this.pnlSSL = new Panel();
      this.chkBoxSSL = new CheckBox();
      this.pnlURL.SuspendLayout();
      this.pnlSSL.SuspendLayout();
      this.SuspendLayout();
      this.tvDirectory.Dock = DockStyle.Fill;
      this.tvDirectory.HideSelection = false;
      this.tvDirectory.Location = new Point(0, 0);
      this.tvDirectory.Name = "tvDirectory";
      this.tvDirectory.Size = new Size(505, 237);
      this.tvDirectory.StateImageList = this.imgLst;
      this.tvDirectory.TabIndex = 0;
      this.tvDirectory.AfterCollapse += new TreeViewEventHandler(this.tvDirectory_AfterCollapse);
      this.tvDirectory.NodeMouseClick += new TreeNodeMouseClickEventHandler(this.tvDirectory_NodeMouseClick);
      this.tvDirectory.AfterExpand += new TreeViewEventHandler(this.tvDirectory_AfterExpand);
      this.imgLst.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imgLst.ImageStream");
      this.imgLst.TransparentColor = System.Drawing.Color.Transparent;
      this.imgLst.Images.SetKeyName(0, "");
      this.imgLst.Images.SetKeyName(1, "");
      this.imgLst.Images.SetKeyName(2, "localDisk.bmp");
      this.imgLst.Images.SetKeyName(3, "CDRom.bmp");
      this.imgLst.Images.SetKeyName(4, "networkDrive.bmp");
      this.imgLst.Images.SetKeyName(5, "");
      this.imgLst.Images.SetKeyName(6, "");
      this.imgLst.Images.SetKeyName(7, "localComputer.bmp");
      this.imgLst.Images.SetKeyName(8, "defaultWeb.bmp");
      this.imgLst.Images.SetKeyName(9, "virtualDir.bmp");
      this.pnlURL.Controls.Add((Control) this.txtBoxURL);
      this.pnlURL.Controls.Add((Control) this.lblPath);
      this.pnlURL.Dock = DockStyle.Bottom;
      this.pnlURL.Location = new Point(0, 237);
      this.pnlURL.Name = "pnlURL";
      this.pnlURL.Size = new Size(505, 29);
      this.pnlURL.TabIndex = 1;
      this.txtBoxURL.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.txtBoxURL.Location = new Point(57, 6);
      this.txtBoxURL.Name = "txtBoxURL";
      this.txtBoxURL.Size = new Size(448, 20);
      this.txtBoxURL.TabIndex = 1;
      this.txtBoxURL.TextChanged += new EventHandler(this.txtBoxURL_TextChanged);
      this.lblPath.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.lblPath.AutoSize = true;
      this.lblPath.Location = new Point(-1, 10);
      this.lblPath.Name = "lblPath";
      this.lblPath.Size = new Size(39, 13);
      this.lblPath.TabIndex = 0;
      this.lblPath.Text = "Folder:";
      this.pnlSSL.Controls.Add((Control) this.chkBoxSSL);
      this.pnlSSL.Dock = DockStyle.Bottom;
      this.pnlSSL.Location = new Point(0, 266);
      this.pnlSSL.Name = "pnlSSL";
      this.pnlSSL.Size = new Size(505, 20);
      this.pnlSSL.TabIndex = 2;
      this.chkBoxSSL.AutoSize = true;
      this.chkBoxSSL.Location = new Point(3, 2);
      this.chkBoxSSL.Name = "chkBoxSSL";
      this.chkBoxSSL.Size = new Size(153, 17);
      this.chkBoxSSL.TabIndex = 0;
      this.chkBoxSSL.Text = "Use Secure Sockets Layer";
      this.chkBoxSSL.UseVisualStyleBackColor = true;
      this.chkBoxSSL.CheckedChanged += new EventHandler(this.chkBoxSSL_CheckedChanged);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.tvDirectory);
      this.Controls.Add((Control) this.pnlURL);
      this.Controls.Add((Control) this.pnlSSL);
      this.Name = "InstallationUrlBrowserDialog";
      this.Size = new Size(505, 286);
      this.pnlURL.ResumeLayout(false);
      this.pnlURL.PerformLayout();
      this.pnlSSL.ResumeLayout(false);
      this.pnlSSL.PerformLayout();
      this.ResumeLayout(false);
    }

    private class SiteInfo
    {
      internal string Path = "";
      internal object Tag;
      private List<KeyValuePair<string, string>> bindingInfo = new List<KeyValuePair<string, string>>();

      internal SiteInfo(string path) => this.Path = path;

      internal void AddBinding(KeyValuePair<string, string> binding)
      {
        this.bindingInfo.Add(binding);
      }

      internal KeyValuePair<string, string>[] BindingInfo
      {
        get => this.bindingInfo.ToArray();
        set
        {
          this.bindingInfo.Clear();
          this.bindingInfo.AddRange((IEnumerable<KeyValuePair<string, string>>) value);
        }
      }

      internal string GetPortNumber(string protocol)
      {
        foreach (KeyValuePair<string, string> keyValuePair in this.bindingInfo.ToArray())
        {
          if (keyValuePair.Value == protocol)
            return keyValuePair.Key;
        }
        return "";
      }
    }

    public enum Type
    {
      FileSystem,
      LocalIIS,
    }
  }
}
