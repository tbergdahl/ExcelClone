using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spreadsheet_Engine
{
    using Spreadsheet_Engine;
    public class ChangeCellBackgroundColorCommand : UndoRedoCommand
    {
        List<Spreadsheet.SpreadsheetCell> cells; // keep track of all cells changed in a command instead of just one so
        //one call to redo changes all of them at once
        public string description = "Change Cell(s) Background Color";
        private uint newBG, oldBG;

        public ChangeCellBackgroundColorCommand(uint nBackground)
        {
            newBG = nBackground;
            cells = new List<Spreadsheet.SpreadsheetCell>();
        }

        public void AddChangedCell(Spreadsheet.SpreadsheetCell cell)
        {
            cells.Add(cell);
        }

        public void Execute()
        {
            foreach (Spreadsheet.SpreadsheetCell cell in cells)
            {
                oldBG = cell.BGColor;
                cell.SetBGColor(newBG);
            }
        }

        public void Undo()
        {
            foreach (Spreadsheet.SpreadsheetCell cell in cells)
            {
                cell.SetBGColor(oldBG);
            }
        }

        public string GetDescription()
        {
            return description;
        }

    }
}
