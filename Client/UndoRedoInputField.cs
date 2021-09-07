using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bomberman.Client
{
    #region Memento
    public class InputFieldValue
    {
        public string Message { get; set; } = "";

        public InputFieldValue(string message)
        {
            this.Message = message;
        }
    }

    #endregion

    #region Caretaker

    class Caretaker
    {
        private Stack<InputFieldValue> UndoStack = new Stack<InputFieldValue>();
        private Stack<InputFieldValue> RedoStack = new Stack<InputFieldValue>();

        public InputFieldValue GetUndoMemento()
        {
            if (IsUndoPossible)
            {
                RedoStack.Push(UndoStack.Pop());
                return UndoStack.Peek();
            }
            else
                return null;
        }

        public InputFieldValue GetRedoMemento()
        {
            if (IsRedoPossible)
            {
                InputFieldValue m = RedoStack.Pop();
                UndoStack.Push(m);
                return m;
            }
            else
                return null;
        }

        public void InsertMementoForUndoRedo(InputFieldValue memento)
        {
            if (memento != null)
            {
                UndoStack.Push(memento);
                RedoStack.Clear();
            }
        }

        public bool IsUndoPossible => UndoStack.Count >= 0;

        public bool IsRedoPossible => RedoStack.Count >= 2;
    }

    #endregion

    public class UndoRedoInputField
    {
        private Caretaker caretaker = new Caretaker();
        private InputFieldValue inputField = null;

        public UndoRedoInputField(InputFieldValue inputField)
        {
            this.inputField = inputField;
        }

        public void TryUndo()
        {
            InputFieldValue memento = caretaker.GetUndoMemento();
            if (memento != null)
            {
                inputField.Message = memento.Message;
            }

        }

        public void TryRedo()
        {
            InputFieldValue memento = caretaker.GetRedoMemento();
            if (memento != null)
            {
                inputField.Message = memento.Message;
            }
        }

        public void SetStateForUndoRedo(string value)
        {
            InputFieldValue memento = new InputFieldValue(value);
            caretaker.InsertMementoForUndoRedo(memento);
        }
    }
}
