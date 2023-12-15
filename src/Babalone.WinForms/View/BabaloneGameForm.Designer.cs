namespace EVAL.Babalone.View
{
    partial class BabaloneGameForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            menuStrip = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            loadToolStripMenuItem = new ToolStripMenuItem();
            saveToolStripMenuItem = new ToolStripMenuItem();
            saveAndQuitToolStripMenuItem = new ToolStripMenuItem();
            quitToolStripMenuItem = new ToolStripMenuItem();
            gameToolStripMenuItem = new ToolStripMenuItem();
            boardSizToolStripMenuItem = new ToolStripMenuItem();
            x3ToolStripMenuItem = new ToolStripMenuItem();
            x4ToolStripMenuItem = new ToolStripMenuItem();
            x6ToolStripMenuItem = new ToolStripMenuItem();
            newGameToolStripMenuItem = new ToolStripMenuItem();
            saveFileDialog = new SaveFileDialog();
            openFileDialog = new OpenFileDialog();
            BottomToolStripPanel = new ToolStripPanel();
            TopToolStripPanel = new ToolStripPanel();
            RightToolStripPanel = new ToolStripPanel();
            LeftToolStripPanel = new ToolStripPanel();
            toolStrip = new ToolStrip();
            playingTextToolStripLabel = new ToolStripLabel();
            playingToolStripLabel = new ToolStripLabel();
            turnToolStripLabel = new ToolStripLabel();
            turnTextToolStripLabel = new ToolStripLabel();
            menuStrip.SuspendLayout();
            toolStrip.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip
            // 
            menuStrip.ImageScalingSize = new Size(20, 20);
            menuStrip.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, gameToolStripMenuItem });
            menuStrip.Location = new Point(0, 0);
            menuStrip.Name = "menuStrip";
            menuStrip.Size = new Size(579, 24);
            menuStrip.TabIndex = 0;
            menuStrip.Text = "menuStrip";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { loadToolStripMenuItem, saveToolStripMenuItem, saveAndQuitToolStripMenuItem, quitToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(37, 20);
            fileToolStripMenuItem.Text = "File";
            // 
            // loadToolStripMenuItem
            // 
            loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            loadToolStripMenuItem.Size = new Size(147, 22);
            loadToolStripMenuItem.Text = "Load";
            loadToolStripMenuItem.Click += LoadGameFile;
            // 
            // saveToolStripMenuItem
            // 
            saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            saveToolStripMenuItem.Size = new Size(147, 22);
            saveToolStripMenuItem.Text = "Save";
            saveToolStripMenuItem.Click += SaveGameFile;
            // 
            // saveAndQuitToolStripMenuItem
            // 
            saveAndQuitToolStripMenuItem.Name = "saveAndQuitToolStripMenuItem";
            saveAndQuitToolStripMenuItem.Size = new Size(147, 22);
            saveAndQuitToolStripMenuItem.Text = "Save and Quit";
            saveAndQuitToolStripMenuItem.Click += SaveGameFileAndQuit;
            // 
            // quitToolStripMenuItem
            // 
            quitToolStripMenuItem.Name = "quitToolStripMenuItem";
            quitToolStripMenuItem.Size = new Size(147, 22);
            quitToolStripMenuItem.Text = "Quit";
            quitToolStripMenuItem.Click += CloseAfterConfirmation;
            // 
            // gameToolStripMenuItem
            // 
            gameToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { boardSizToolStripMenuItem, newGameToolStripMenuItem });
            gameToolStripMenuItem.Name = "gameToolStripMenuItem";
            gameToolStripMenuItem.Size = new Size(50, 20);
            gameToolStripMenuItem.Text = "Game";
            // 
            // boardSizToolStripMenuItem
            // 
            boardSizToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { x3ToolStripMenuItem, x4ToolStripMenuItem, x6ToolStripMenuItem });
            boardSizToolStripMenuItem.Name = "boardSizToolStripMenuItem";
            boardSizToolStripMenuItem.Size = new Size(180, 22);
            boardSizToolStripMenuItem.Text = "Board size";
            // 
            // x3ToolStripMenuItem
            // 
            x3ToolStripMenuItem.Name = "x3ToolStripMenuItem";
            x3ToolStripMenuItem.Size = new Size(180, 22);
            x3ToolStripMenuItem.Text = "3x3";
            x3ToolStripMenuItem.Click += NewGameSmall;
            // 
            // x4ToolStripMenuItem
            // 
            x4ToolStripMenuItem.Name = "x4ToolStripMenuItem";
            x4ToolStripMenuItem.Size = new Size(180, 22);
            x4ToolStripMenuItem.Text = "4x4";
            x4ToolStripMenuItem.Click += NewGameMedium;
            // 
            // x6ToolStripMenuItem
            // 
            x6ToolStripMenuItem.Name = "x6ToolStripMenuItem";
            x6ToolStripMenuItem.Size = new Size(180, 22);
            x6ToolStripMenuItem.Text = "6x6";
            x6ToolStripMenuItem.Click += NewGameLarge;
            // 
            // newGameToolStripMenuItem
            // 
            newGameToolStripMenuItem.Name = "newGameToolStripMenuItem";
            newGameToolStripMenuItem.Size = new Size(180, 22);
            newGameToolStripMenuItem.Text = "New game";
            newGameToolStripMenuItem.Click += RestartGame;
            // 
            // saveFileDialog
            // 
            saveFileDialog.DefaultExt = "baba";
            saveFileDialog.FileName = "BabaloneGame";
            saveFileDialog.Filter = "Babalone game (*.baba)|*.baba|All files|*.*";
            saveFileDialog.Title = "Save game";
            // 
            // openFileDialog
            // 
            openFileDialog.DefaultExt = "*.baba";
            openFileDialog.FileName = "BabaloneGame";
            openFileDialog.Filter = "Babalone game (*.baba)|*.baba|All files|*.*";
            // 
            // BottomToolStripPanel
            // 
            BottomToolStripPanel.Location = new Point(0, 0);
            BottomToolStripPanel.Name = "BottomToolStripPanel";
            BottomToolStripPanel.Orientation = Orientation.Horizontal;
            BottomToolStripPanel.RowMargin = new Padding(4, 0, 0, 0);
            BottomToolStripPanel.Size = new Size(0, 0);
            // 
            // TopToolStripPanel
            // 
            TopToolStripPanel.Location = new Point(0, 0);
            TopToolStripPanel.Name = "TopToolStripPanel";
            TopToolStripPanel.Orientation = Orientation.Horizontal;
            TopToolStripPanel.RowMargin = new Padding(4, 0, 0, 0);
            TopToolStripPanel.Size = new Size(0, 0);
            // 
            // RightToolStripPanel
            // 
            RightToolStripPanel.Location = new Point(0, 0);
            RightToolStripPanel.Name = "RightToolStripPanel";
            RightToolStripPanel.Orientation = Orientation.Horizontal;
            RightToolStripPanel.RowMargin = new Padding(4, 0, 0, 0);
            RightToolStripPanel.Size = new Size(0, 0);
            // 
            // LeftToolStripPanel
            // 
            LeftToolStripPanel.Location = new Point(0, 0);
            LeftToolStripPanel.Name = "LeftToolStripPanel";
            LeftToolStripPanel.Orientation = Orientation.Horizontal;
            LeftToolStripPanel.RowMargin = new Padding(4, 0, 0, 0);
            LeftToolStripPanel.Size = new Size(0, 0);
            // 
            // toolStrip
            // 
            toolStrip.Dock = DockStyle.Bottom;
            toolStrip.ImageScalingSize = new Size(20, 20);
            toolStrip.Items.AddRange(new ToolStripItem[] { playingTextToolStripLabel, playingToolStripLabel, turnToolStripLabel, turnTextToolStripLabel });
            toolStrip.Location = new Point(0, 355);
            toolStrip.Name = "toolStrip";
            toolStrip.Size = new Size(579, 25);
            toolStrip.TabIndex = 1;
            // 
            // playingTextToolStripLabel
            // 
            playingTextToolStripLabel.Name = "playingTextToolStripLabel";
            playingTextToolStripLabel.Size = new Size(49, 22);
            playingTextToolStripLabel.Text = "Playing:";
            // 
            // playingToolStripLabel
            // 
            playingToolStripLabel.Name = "playingToolStripLabel";
            playingToolStripLabel.Size = new Size(12, 22);
            playingToolStripLabel.Text = "-";
            // 
            // turnToolStripLabel
            // 
            turnToolStripLabel.Alignment = ToolStripItemAlignment.Right;
            turnToolStripLabel.Name = "turnToolStripLabel";
            turnToolStripLabel.Size = new Size(22, 22);
            turnToolStripLabel.Text = "-/-";
            // 
            // turnTextToolStripLabel
            // 
            turnTextToolStripLabel.Alignment = ToolStripItemAlignment.Right;
            turnTextToolStripLabel.Name = "turnTextToolStripLabel";
            turnTextToolStripLabel.Size = new Size(34, 22);
            turnTextToolStripLabel.Text = "Turn:";
            // 
            // BabaloneGameForm
            // 
            AutoScaleMode = AutoScaleMode.None;
            ClientSize = new Size(579, 380);
            Controls.Add(toolStrip);
            Controls.Add(menuStrip);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MainMenuStrip = menuStrip;
            MaximizeBox = false;
            Name = "BabaloneGameForm";
            ShowIcon = false;
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Kitolás The Video Game";
            menuStrip.ResumeLayout(false);
            menuStrip.PerformLayout();
            toolStrip.ResumeLayout(false);
            toolStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem loadToolStripMenuItem;
        private ToolStripMenuItem gameToolStripMenuItem;
        private ToolStripMenuItem boardSizToolStripMenuItem;
        private ToolStripMenuItem x3ToolStripMenuItem;
        private ToolStripMenuItem x4ToolStripMenuItem;
        private ToolStripMenuItem x6ToolStripMenuItem;
        private ToolStripMenuItem newGameToolStripMenuItem;
        private ToolStripMenuItem saveAndQuitToolStripMenuItem;
        private ToolStripMenuItem quitToolStripMenuItem;
        private SaveFileDialog saveFileDialog;
        private OpenFileDialog openFileDialog;
        private ToolStripPanel BottomToolStripPanel;
        private ToolStripPanel TopToolStripPanel;
        private ToolStripPanel RightToolStripPanel;
        private ToolStripPanel LeftToolStripPanel;
        private ToolStrip toolStrip;
        private ToolStripLabel playingTextToolStripLabel;
        private ToolStripLabel playingToolStripLabel;
        private ToolStripLabel turnToolStripLabel;
        private ToolStripLabel turnTextToolStripLabel;
    }
}