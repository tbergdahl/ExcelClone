// <copyright file="Form1.cs" company="Trenton Bergdahl">
// Copyright (c) Trenton Bergdahl. All rights reserved.
// </copyright>
namespace Spreadsheet_Trenton_Bergdahl
{
    using System.ComponentModel;
    using System.Numerics;
    using Spreadsheet_Engine;
    /// <summary>
    /// Handles all Winforms Utility.
    /// </summary>
    public partial class Form1 : Form
    {
        private Spreadsheet sheet;


        /// <summary>
        /// Initializes a new instance of the <see cref="Form1"/> class.
        /// </summary>
        public Form1()
        {
            this.InitializeComponent();
            this.dataGridView1.Columns.Clear();
            this.InitializeDataGrid();
            sheet = new Spreadsheet(26, 50);
            sheet.CellPropertyChanged += Spreadsheet_CellChanged;
        }
        /// <summary>
        /// Creates columns with names A - Z, and rows 1 - 50.
        /// </summary>
        private void InitializeDataGrid()
        {
            string alp = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            foreach (char ch in alp)
            {
                DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn();
                column.HeaderText = ch.ToString();
                this.dataGridView1.Columns.Add(column);
            }

            for (int i = 1; i <= 50; i++)
            {
                this.dataGridView1.Rows.Add();
                this.dataGridView1.Rows[i - 1].HeaderCell.Value = i.ToString();
            }
        }

        private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {}

        private void Spreadsheet_CellChanged(object sender, DataGridViewCellEventArgs e)
        {

        }



    }
}