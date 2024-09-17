using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spreadsheet_Engine
{
    public class ChangeCellTextCommand : UndoRedoCommand
    {
        readonly  Spreadsheet.SpreadsheetCell cell;
        public readonly string description = "Change Cell Text";
        private string? newText, oldText;

        public ChangeCellTextCommand(Spreadsheet.SpreadsheetCell cell, string? nText)
        {
            this.cell = cell;
            newText = nText;
        }

        public void Execute()
        {
            oldText = cell.Text;
            cell.SetText(newText);
            cell.SetEvaluate(true);
        }

        public void Undo()
        {
            cell.SetText(oldText);
            cell.SetEvaluate(false); // this is to prevent the cell from reevaluating a tree whos cells may
                                     // have been undone and no longer have a value
        }

        public string GetDescription()
        {
            return description;
        }

    }
}
