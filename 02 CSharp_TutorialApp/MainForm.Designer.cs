using System;
using System.Windows.Forms;

namespace CSharp20Tutorial
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private SplitContainer splitContainer1;
        private TreeView treeViewChapters;
        private WebBrowser webBrowserOutput;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStripStatusLabel1;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.splitContainer1 = new SplitContainer();
            this.treeViewChapters = new TreeView();
            this.webBrowserOutput = new WebBrowser();
            this.statusStrip1 = new StatusStrip();
            this.toolStripStatusLabel1 = new ToolStripStatusLabel();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.treeViewChapters);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.webBrowserOutput);
            this.splitContainer1.Size = new System.Drawing.Size(984, 662);
            this.splitContainer1.SplitterDistance = 220;
            this.splitContainer1.TabIndex = 0;
            // 
            // treeViewChapters
            // 
            this.treeViewChapters.Dock = DockStyle.Fill;
            this.treeViewChapters.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.treeViewChapters.Location = new System.Drawing.Point(0, 0);
            this.treeViewChapters.Name = "treeViewChapters";
            this.treeViewChapters.Size = new System.Drawing.Size(220, 662);
            this.treeViewChapters.TabIndex = 0;
            this.treeViewChapters.AfterSelect += new TreeViewEventHandler(this.treeViewChapters_AfterSelect);
            // 
            // webBrowserOutput
            // 
            this.webBrowserOutput.Dock = DockStyle.Fill;
            this.webBrowserOutput.IsWebBrowserContextMenuEnabled = true;
            this.webBrowserOutput.Location = new System.Drawing.Point(0, 0);
            this.webBrowserOutput.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowserOutput.Name = "webBrowserOutput";
            this.webBrowserOutput.ScriptErrorsSuppressed = true;
            this.webBrowserOutput.Size = new System.Drawing.Size(760, 662);
            this.webBrowserOutput.TabIndex = 0;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 640);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(984, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(192, 17);
            this.toolStripStatusLabel1.Text = "C# 2.0 入门教程 | WinForms桌面版";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 662);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.splitContainer1);
            this.Name = "MainForm";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "C# 2.0 入门教程 - 桌面版";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
