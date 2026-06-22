using MiniSchematicEditor.ViewModels;
using System.ComponentModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MiniSchematicEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainWindowViewModel();
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