using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiniSchematicEditor.ViewModels;

namespace MiniSchematicEditor.Services.Interfaces
{
    public interface IMessageService
    {
        void ShowError(string message, string title = "Ошибка");
        void ShowInfo(string message, string title = "Информация");
        bool? ShowQuestionYesNoCancel(string message, string title = "Вопрос");
        bool ShowAddBlockDialog(List<string> currentNames, out AddBlockViewModel viewModel);
        string ShowInputDialog();
    }
}
