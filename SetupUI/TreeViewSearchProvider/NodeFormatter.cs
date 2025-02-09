// Decompiled with JetBrains decompiler
// Type: TreeViewSearchProvider.NodeFormatter
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

#nullable disable
namespace TreeViewSearchProvider
{
  internal class NodeFormatter
  {
    private NodeFormatSettings _nodeFormats;
    private StateStore<TreeNode, ColorFormatSettings> _ncfsState = new StateStore<TreeNode, ColorFormatSettings>();
    private TreeNode _lastFormattedNode;
    private CancellationToken _ct;
    private int _clearTrailDelay;

    internal NodeFormatSettings NodeFormatSettings
    {
      get => this._nodeFormats;
      set => this._nodeFormats = value;
    }

    internal NodeFormatter()
      : this(NodeFormatSettings.DefaultSettings)
    {
    }

    internal NodeFormatter(NodeFormatSettings nodeFormatSettings)
    {
      this._nodeFormats = nodeFormatSettings;
      this._nodeFormats.Blink.BlinkSettingsChanged += new EventHandler(this.blinkValeChanged);
      this._nodeFormats.ClearTrail.ClearTrailSettingsChanged += new EventHandler(this.clearTrailValueChanged);
      this._nodeFormats.HighlightValueChanged += new EventHandler(this.highlightValueChanged);
    }

    internal void Format(TreeNode node)
    {
      this.clearTrail();
      this.format(node);
      this.show(node);
      this.blink(node);
    }

    internal void Format(List<TreeNode> nodes)
    {
      foreach (TreeNode node in nodes)
        this.Format(node);
    }

    internal void Reset()
    {
      if (this._ncfsState.Count == 0)
        return;
      this.clearFormat();
    }

    private void format(TreeNode node)
    {
      this.saveState(node);
      this.applyFormat(node, this._nodeFormats.Color);
      this._lastFormattedNode = node;
    }

    private void show(TreeNode node) => node.EnsureVisible();

    private async void blink(TreeNode node)
    {
      if (!this._nodeFormats.Blink.CanBlink)
        return;
      await this.startBlink(node, this._ct);
    }

    private void clearTrail()
    {
      if (!this._nodeFormats.ClearTrail.CanClear || this._ncfsState.Count <= 0)
        return;
      TreeNode node = this._lastFormattedNode;
      ColorFormatSettings state = this._ncfsState[this._lastFormattedNode];
      if (this._clearTrailDelay > 0)
      {
        Task.Run((Func<Task>) (async () =>
        {
          await Task.Delay(this._clearTrailDelay);
          this.applyFormat(node, state);
          this._ncfsState.Remove(node);
        }));
      }
      else
      {
        this.applyFormat(node, state);
        this._ncfsState.Remove(node);
      }
    }

    private void clearFormat()
    {
      this.cancelToken();
      foreach (KeyValuePair<TreeNode, ColorFormatSettings> keyValuePair in this._ncfsState.StateObject)
        this.applyFormat(keyValuePair.Key, keyValuePair.Value);
      this._ncfsState.Clear();
      this.generateToken();
    }

    private void blinkValeChanged()
    {
      this.cancelToken();
      this.generateToken();
      this.setClearTrailDelay();
    }

    private void clearTrailValueChanged()
    {
      if (!this._nodeFormats.ClearTrail.CanClear)
        return;
      if (this._ncfsState.Count > 0)
      {
        this.clearFormat();
        this.Format(this._lastFormattedNode);
      }
      this.setClearTrailDelay();
    }

    private void highlightValueChanged()
    {
      this.Reset();
      if (this._nodeFormats.Highlighting == NodeFormatSettings.Highlight.All)
        this._nodeFormats.ClearTrail.CanClear = false;
      else
        this._nodeFormats.ClearTrail.CanClear = true;
    }

    private void saveState(TreeNode node)
    {
      this._ncfsState.Add(node, ColorFormatSettings.GetNewSetting(node.ForeColor, node.BackColor));
    }

    private void applyFormat(TreeNode node, ColorFormatSettings cfs)
    {
      node.BackColor = cfs.BackColor;
      node.ForeColor = cfs.ForeColor;
    }

    private async Task startBlink(TreeNode node, CancellationToken token)
    {
      for (int i = 0; i < this._nodeFormats.Blink.Repeat * 2; ++i)
      {
        await Task.Delay(this._nodeFormats.Blink.SpeedInMilliseconds);
        if (token.IsCancellationRequested)
          break;
        node.BackColor = node.BackColor == this._nodeFormats.Color.BackColor ? this._nodeFormats.Color.ForeColor : this._nodeFormats.Color.BackColor;
        node.ForeColor = node.ForeColor == this._nodeFormats.Color.ForeColor ? this._nodeFormats.Color.BackColor : this._nodeFormats.Color.ForeColor;
      }
    }

    private void setClearTrailDelay()
    {
      this._clearTrailDelay = this._nodeFormats.Blink.CanBlink ? this._nodeFormats.Blink.Repeat * this._nodeFormats.Blink.SpeedInMilliseconds * 2 + this._nodeFormats.ClearTrail.DelayInMilliseconds : this._nodeFormats.ClearTrail.DelayInMilliseconds;
    }

    private void generateToken()
    {
      if (!this._nodeFormats.Blink.CanBlink)
        return;
      this._ct = CancellationHelper.Token;
    }

    private void cancelToken() => CancellationHelper.CancelRequest();
  }
}
