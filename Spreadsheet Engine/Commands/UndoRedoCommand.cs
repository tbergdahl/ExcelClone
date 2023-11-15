using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spreadsheet_Engine
{
    public interface UndoRedoCommand
    {
        void Execute();
        void Undo();

        string GetDescription();
    }
}
