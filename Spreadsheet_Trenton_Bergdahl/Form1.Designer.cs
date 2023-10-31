﻿
namespace Spreadsheet_Trenton_Bergdahl
{
    using Spreadsheet_Engine;
    using System.ComponentModel;

    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            sheet.CellPropertyChanged -= Spreadsheet_PropertyChanged;
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
            dataGridView1 = new DataGridView();
            A = new DataGridViewTextBoxColumn();
            B = new DataGridViewTextBoxColumn();
            D = new DataGridViewTextBoxColumn();
            button1 = new Button();
            ((ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { A, B, D });
            dataGridView1.Location = new Point(2, -1);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 60;
            dataGridView1.RowTemplate.Height = 29;
            dataGridView1.Size = new Size(798, 451);
            dataGridView1.TabIndex = 0;
            dataGridView1.CellBeginEdit += dataGridView1_CellBeginEdit;
            dataGridView1.CellEndEdit += dataGridView1_CellEndEdit;
            // 
            // A
            // 
            A.HeaderText = "Column1";
            A.MinimumWidth = 6;
            A.Name = "A";
            A.Width = 125;
            // 
            // B
            // 
            B.HeaderText = "Column1";
            B.MinimumWidth = 6;
            B.Name = "B";
            B.Width = 125;
            // 
            // D
            // 
            D.HeaderText = "Column1";
            D.MinimumWidth = 6;
            D.Name = "D";
            D.Width = 125;
            // 
            // button1
            // 
            button1.Location = new Point(23, 53);
            button1.Name = "button1";
            button1.Size = new Size(97, 47);
            button1.TabIndex = 1;
            button1.Text = "button1";
            button1.UseVisualStyleBackColor = true;
            button1.Click += Button1_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(button1);
            Controls.Add(dataGridView1);
            Name = "Form1";
            Text = "Form1";
            ((ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dataGridView1;
        private DataGridViewTextBoxColumn A;
        private DataGridViewTextBoxColumn B;
        private DataGridViewTextBoxColumn D;
        private Button button1;
    }

}