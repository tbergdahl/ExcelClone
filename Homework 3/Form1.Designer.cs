namespace Homework_3
{
    partial class Form1
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
            textBox1 = new TextBox();
            toolStripMenuItem1 = new ToolStripMenuItem();
            loadFirst50FibonacciNumbersToolStripMenuItem = new ToolStripMenuItem();
            loadFirst100FibonacciNumbersToolStripMenuItem = new ToolStripMenuItem();
            loadFromFileToolStripMenuItem = new ToolStripMenuItem();
            saveToFileToolStripMenuItem1 = new ToolStripMenuItem();
            Options = new MenuStrip();
            openFileDialog1 = new OpenFileDialog();
            saveFileDialog1 = new SaveFileDialog();
            Options.SuspendLayout();
            SuspendLayout();
            // 
            // textBox1
            // 
            textBox1.Location = new Point(0, 31);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.ScrollBars = ScrollBars.Both;
            textBox1.Size = new Size(800, 453);
            textBox1.TabIndex = 0;
            textBox1.TextChanged += textBox1_TextChanged;
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.DropDownItems.AddRange(new ToolStripItem[] { loadFirst50FibonacciNumbersToolStripMenuItem, loadFirst100FibonacciNumbersToolStripMenuItem, loadFromFileToolStripMenuItem, saveToFileToolStripMenuItem1 });
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new Size(75, 24);
            toolStripMenuItem1.Text = "Options";
            toolStripMenuItem1.Click += toolStripMenuItem1_Click;
            // 
            // loadFirst50FibonacciNumbersToolStripMenuItem
            // 
            loadFirst50FibonacciNumbersToolStripMenuItem.Name = "loadFirst50FibonacciNumbersToolStripMenuItem";
            loadFirst50FibonacciNumbersToolStripMenuItem.Size = new Size(315, 26);
            loadFirst50FibonacciNumbersToolStripMenuItem.Text = "Load First 50 Fibonacci Numbers";
            loadFirst50FibonacciNumbersToolStripMenuItem.Click += loadFirst50FibonacciNumbersToolStripMenuItem_Click;
            // 
            // loadFirst100FibonacciNumbersToolStripMenuItem
            // 
            loadFirst100FibonacciNumbersToolStripMenuItem.Name = "loadFirst100FibonacciNumbersToolStripMenuItem";
            loadFirst100FibonacciNumbersToolStripMenuItem.Size = new Size(315, 26);
            loadFirst100FibonacciNumbersToolStripMenuItem.Text = "Load First 100 Fibonacci Numbers";
            loadFirst100FibonacciNumbersToolStripMenuItem.Click += loadFirst100FibonacciNumbersToolStripMenuItem_Click;
            // 
            // loadFromFileToolStripMenuItem
            // 
            loadFromFileToolStripMenuItem.Name = "loadFromFileToolStripMenuItem";
            loadFromFileToolStripMenuItem.Size = new Size(315, 26);
            loadFromFileToolStripMenuItem.Text = "Load From File";
            loadFromFileToolStripMenuItem.Click += loadFromFileToolStripMenuItem_Click;
            // 
            // saveToFileToolStripMenuItem1
            // 
            saveToFileToolStripMenuItem1.Name = "saveToFileToolStripMenuItem1";
            saveToFileToolStripMenuItem1.Size = new Size(315, 26);
            saveToFileToolStripMenuItem1.Text = "Save to File";
            saveToFileToolStripMenuItem1.Click += saveToFileToolStripMenuItem1_Click;
            // 
            // Options
            // 
            Options.ImageScalingSize = new Size(20, 20);
            Options.Items.AddRange(new ToolStripItem[] { toolStripMenuItem1 });
            Options.Location = new Point(0, 0);
            Options.Name = "Options";
            Options.Size = new Size(800, 28);
            Options.TabIndex = 1;
            Options.Text = "menuStrip1";
            Options.ItemClicked += menuStrip1_ItemClicked;
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(textBox1);
            Controls.Add(Options);
            MainMenuStrip = Options;
            Name = "Form1";
            Text = "tbergdahl_HW3";
            Load += Form1_Load;
            Options.ResumeLayout(false);
            Options.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox textBox1;
        private ToolStripMenuItem toolStripMenuItem1;
        private ToolStripMenuItem loadFirst50FibonacciNumbersToolStripMenuItem;
        private ToolStripMenuItem loadFirst100FibonacciNumbersToolStripMenuItem;
        private ToolStripMenuItem loadFromFileToolStripMenuItem;
        private ToolStripMenuItem saveToFileToolStripMenuItem1;
        private MenuStrip Options;
        private OpenFileDialog openFileDialog1;
        private SaveFileDialog saveFileDialog1;
    }
}