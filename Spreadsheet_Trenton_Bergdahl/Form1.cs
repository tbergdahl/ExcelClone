// <copyright file="Form1.cs" company="Trenton Bergdahl">
// Copyright (c) Trenton Bergdahl. All rights reserved.
// </copyright>
namespace Spreadsheet_Trenton_Bergdahl
{
    using System.ComponentModel;
    using System.Numerics;
    using Spreadsheet_Engine;
    using static Spreadsheet_Engine.Spreadsheet;

    /// <summary>
    /// Handles all Winforms Utility.
    /// </summary>
    public partial class Form1 : Form
    {
        private Spreadsheet sheet;
        public event PropertyChangedEventHandler? SpreadsheetPropertyChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="Form1"/> class.
        /// </summary>
        public Form1()
        {
            this.InitializeComponent();
            this.dataGridView1.Columns.Clear();
            this.InitializeDataGrid();
            sheet = new Spreadsheet(51, 27);
            sheet.CellPropertyChanged += Spreadsheet_PropertyChanged;
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
        { }

        private void Spreadsheet_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            string indices = e.PropertyName;

            string[] indexParts = indices.Split(' ');

            if (indexParts.Length == 2)
            {
                if (int.TryParse(indexParts[0], out int rowIndex) && int.TryParse(indexParts[1], out int columnIndex))
                {
                    dataGridView1.Rows[rowIndex - 1].Cells[columnIndex - 1].Value = sheet.GetCell(rowIndex, columnIndex).Value;
                }
            }



        }
        private void button1_Click(object sender, EventArgs e)
        {
            sheet.Demo();
        }
    }
}