// <copyright file="Form1.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
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
        private readonly Spreadsheet sheet;
        string oldStringTextBox;

        /// <summary>
        /// Initializes a new instance of the <see cref="Form1"/> class.
        /// </summary>
        public Form1()
        {
            this.InitializeComponent();
            this.dataGridView1.Columns.Clear();
            this.InitializeDataGrid();
            this.sheet = new Spreadsheet(51, 27);
            this.sheet.CellPropertyChanged += this.Spreadsheet_PropertyChanged;
        }

        /// <summary>
        /// Creates columns with names A - Z, and rows 1 - 50.
        /// </summary>
        private void InitializeDataGrid()
        {
            string alp = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            foreach (char ch in alp)
            {
#pragma warning disable IDE0017 // Simplify object initialization
                DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn();
#pragma warning restore IDE0017 // Simplify object initialization - Had to ignore because I need to use column.
                column.HeaderText = ch.ToString();
                this.dataGridView1.Columns.Add(column);
            }

            for (int i = 1; i <= 50; i++)
            {
                this.dataGridView1.Rows.Add();
                this.dataGridView1.Rows[i - 1].HeaderCell.Value = i.ToString();
            }
        }

        /// <summary>
        /// Method that when notified of a spreadsheet change, updates the corresponding index on the form.
        /// </summary>
        /// <param name="sender"> What object sent the notification. </param>
        /// <param name="e"> Event arguments. </param>
        private void Spreadsheet_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            string? indices = e?.PropertyName;

            if (indices != null)
            {
                string[] indexParts = indices.Split(' ');

                if (indexParts.Length == 2)
                {
                    if (int.TryParse(indexParts[0], out int rowIndex) && int.TryParse(indexParts[1], out int columnIndex))
                    {
                        var dataGridViewRow = this.dataGridView1.Rows[rowIndex - 1];
                        var dataGridViewCell = dataGridViewRow?.Cells[columnIndex - 1];
                        var spreadsheetCell = this.sheet.GetCell(rowIndex, columnIndex);

                        if (dataGridViewCell != null && spreadsheetCell != null)
                        {
                            dataGridViewCell.Value = spreadsheetCell.Value;
                        }
                        else
                        {
                            return;
                        }
                    }
                }
            }
        }

        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            DataGridViewCell current = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
            if (current != null)
            {
                current.Value = this.sheet.GetCell(current.RowIndex + 1, current.ColumnIndex + 1).Text;
            }
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewCell current = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
            if (current != null)
            {
                this.sheet.GetCell(current.RowIndex + 1, current.ColumnIndex + 1).Text = current.Value.ToString();
                this.sheet.GetCell(current.RowIndex + 1, current.ColumnIndex + 1).SendNotification();
            }
        }

        /// <summary>
        /// When button clicked, preform demo for HW4.
        /// </summary>
        /// <param name="sender"> What object sent notification. </param>
        /// <param name="e"> Event arguments. </param>
        private void Button1_Click(object sender, EventArgs e)
        {
            this.sheet.Demo();
        }
    }
}