// <copyright file="Form1.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
namespace Spreadsheet_Trenton_Bergdahl
{
    using System.ComponentModel;
    using System.Data;
    using System.Numerics;
    using System.Runtime.CompilerServices;
    using Spreadsheet_Engine;
    using static Spreadsheet_Engine.Spreadsheet;

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
            this.sheet = new Spreadsheet(51, 27);
            this.sheet.CellPropertyChanged += this.Spreadsheet_PropertyChanged;
            this.sheet.CellBackgroundColorChanged += this.Spreadsheet_BackgroundColorChanged;
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

        private void Spreadsheet_BackgroundColorChanged(object? sender, PropertyChangedEventArgs e)
        {
            string? indices = e?.PropertyName;
            if (this.sheet.GetCell != null)
            {
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
                                byte alpha = (byte)((spreadsheetCell.BGColor & 0xFF000000) >> 24);
                                byte red = (byte)((spreadsheetCell.BGColor & 0x00FF0000) >> 16);
                                byte green = (byte)((spreadsheetCell.BGColor & 0x0000FF00) >> 8);
                                byte blue = (byte)(spreadsheetCell.BGColor & 0x000000FF);
                                Color cellColor = Color.FromArgb(alpha, red, green, blue);
                                dataGridViewCell.Style.BackColor = cellColor;
                            }
                            else
                            {
                                return;
                            }
                        }
                    }
                }
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
            if (this.sheet.GetCell != null)
            {
                if (indices != null)
                {
                    string[] indexParts = indices.Split(' ');

                    if (indexParts.Length == 2)
                    {
                        if (int.TryParse(indexParts[0], out int rowIndex) && int.TryParse(indexParts[1], out int columnIndex))
                        {
                            var dataGridViewRow = this.dataGridView1.Rows[rowIndex - 1];
                            var dataGridViewCell = dataGridViewRow?.Cells[columnIndex - 1];
                            SpreadsheetCell? spreadsheetCell = this.sheet.GetCell(rowIndex, columnIndex);

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
        }

        private void DataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            DataGridViewCell current = this.dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
            if (current != null)
            {
                if (this.sheet.GetCell != null)
                {
                    SpreadsheetCell? cell = this.sheet.GetCell(current.RowIndex + 1, current.ColumnIndex + 1);
                    if (cell != null)
                    {
                        if (cell.Text != null)
                        {
                            current.Value = cell.Text;
                        }
                    }
                }
            }
        }

        private void UpdateUndoRedoMenuItemsText()
        {
            if (this.sheet.UndoStackEmpty())
            {
                this.undoToolStripMenuItem.Visible = false;
            }
            else
            {
                this.undoToolStripMenuItem.Text = "Undo " + this.sheet.GetNextUndoCommandName();
                this.undoToolStripMenuItem.Visible = true;
            }

            if (this.sheet.RedoStackEmpty())
            {
                this.redoToolStripMenuItem.Visible = false;
            }
            else
            {
                this.redoToolStripMenuItem.Text = "Redo " + this.sheet.GetNextRedoCommandName();
                this.redoToolStripMenuItem.Visible = true;
            }
        }

        private void DataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewCell current = this.dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
            if (current != null && current.Value != null)
            {
                if (this.sheet.GetCell != null)
                {
                    SpreadsheetCell? cell = this.sheet.GetCell(current.RowIndex + 1, current.ColumnIndex + 1);
                    if (cell != null)
                    {
                        if (cell.Text == current.Value.ToString()) // if they just viewed the formula (didn't change it)
                        {
                            current.Value = cell.Value;
                        }
                        else
                        {
                            try
                            {
                                ChangeCellTextCommand command = new ChangeCellTextCommand(cell, current.Value.ToString());
                                command.Execute();
                                this.sheet.AddCommand(command);
                                this.UpdateUndoRedoMenuItemsText();
                            }
                            catch (InvalidExpressionException ex)
                            {
                                cell.Text = string.Empty;
                                MessageBox.Show("An Error Occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            catch (ArgumentOutOfRangeException ex)
                            {
                                cell.Text = string.Empty;
                                MessageBox.Show("An Error Occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            catch (DivideByZeroException ex)
                            {
                                cell.Text = string.Empty;
                                MessageBox.Show("An Error Occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
        }

        private void ChangeSelectedCellColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using ColorDialog colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                Color selectedColor = colorDialog.Color;
                byte alpha = selectedColor.A;
                byte red = selectedColor.R;
                byte green = selectedColor.G;
                byte blue = selectedColor.B;

                uint bgColor = (uint)((alpha << 24) | (red << 16) | (green << 8) | blue);

                ChangeCellBackgroundColorCommand command = new ChangeCellBackgroundColorCommand(bgColor);
                foreach (DataGridViewCell viewCell in this.dataGridView1.SelectedCells)
                {
                    if (this.sheet != null && this.sheet.GetCell != null)
                    {
                        SpreadsheetCell? cell = this.sheet.GetCell(viewCell.RowIndex + 1, viewCell.ColumnIndex + 1);
                        if (cell != null)
                        {
                            command.AddChangedCell(cell); // see ChangeCellBackgroundColor.cs
                        }
                    }
                }

                command.Execute();
                this.sheet.AddCommand(command);
                this.UpdateUndoRedoMenuItemsText();
            }
        }

        private void UndoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.sheet.Undo();
            this.UpdateUndoRedoMenuItemsText();
        }

        private void RedoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.sheet.Redo();
            this.UpdateUndoRedoMenuItemsText();
        }

        private void SaveSheetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "XML Files (*.xml)|*.xml";
            saveFileDialog.Title = "Save Spreadsheet";
            saveFileDialog.ShowDialog();

            if (saveFileDialog.FileName != string.Empty)
            {
                string filePath = saveFileDialog.FileName;
                this.sheet.Save(filePath);
            }
        }

        private void LoadSheetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "XML Files (*.xml)|*.xml";
            openFileDialog.Title = "Load Spreadsheet";
            openFileDialog.ShowDialog();

            if (openFileDialog.FileName != string.Empty)
            {
                this.sheet = new Spreadsheet(51, 27);
                this.sheet.CellPropertyChanged += this.Spreadsheet_PropertyChanged;
                this.sheet.CellBackgroundColorChanged += this.Spreadsheet_BackgroundColorChanged;

                foreach (DataGridViewRow row in dataGridView1.Rows) // reset view
                {
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        cell.Value = null;
                        cell.Style.BackColor = Color.White;
                    }
                }

                string filePath = openFileDialog.FileName;
                try
                {
                    this.sheet.Load(filePath);
                }
                catch (DataException ex)
                {
                    MessageBox.Show("An Error Occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}