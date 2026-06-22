using MiniSchematicEditor.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MiniSchematicEditor.Services
{
    public class MessageService : IMessageService
    {
        public void ShowError(string message, string title = "Ошибка")
            => MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);

        public void ShowInfo(string message, string title = "Информация")
            => MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);

        public bool? ShowQuestionYesNoCancel(string message, string title = "Вопрос")
        {
            MessageBoxResult result = MessageBox.Show(
                message,
                title,
                MessageBoxButton.YesNoCancel,
                MessageBoxImage.Warning
            );

            switch (result)
            {
                case MessageBoxResult.Yes: 
                    return true;
                case MessageBoxResult.No:
                    return false;
                case MessageBoxResult.Cancel:
                    return null;
                default:
                    return null;
            }
        }
    }
}
