using MiniSchematicEditor.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MiniSchematicEditor.Views
{
    /// <summary>
    /// Логика взаимодействия для AddBlockDialog.xaml
    /// </summary>
    public partial class AddBlockDialog : Window
    {
        public AddBlockDialog()
        {
            InitializeComponent();
        }

        private void XCoordinate_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (Regex.IsMatch(e.Text, "[^0-9]"))
            {
                e.Handled = true;
                return;
            }

            if (sender is TextBox textBox)
            {
                string currentText = textBox.Text;
                int selectionStart = textBox.SelectionStart;
                int selectionLength = textBox.SelectionLength;

                string futureText = currentText.Remove(selectionStart, selectionLength)
                                               .Insert(selectionStart, e.Text);
                if (double.TryParse(futureText, out double value) && value > 1500)
                {
                    e.Handled = true;
                }
            }
        }
        private void YCoordinate_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (Regex.IsMatch(e.Text, "[^0-9]"))
            {
                e.Handled = true;
                return;
            }

            if (sender is TextBox textBox)
            {
                string currentText = textBox.Text;
                int selectionStart = textBox.SelectionStart;
                int selectionLength = textBox.SelectionLength;

                string futureText = currentText.Remove(selectionStart, selectionLength)
                                               .Insert(selectionStart, e.Text);

                if (double.TryParse(futureText, out double value) && value > 850)
                {
                    e.Handled = true;
                }
            }
        }
    }
}
