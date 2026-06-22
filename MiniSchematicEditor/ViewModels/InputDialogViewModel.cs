using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MiniSchematicEditor.ViewModels
{
    public class InputDialogViewModel : ObservableValidator
    {
        public InputDialogViewModel()
        {
            Ok = new RelayCommand(OkCommand);
            Cancel = new RelayCommand(CancelCommand);
        }

        public Action<bool> CloseAction { get; set; }
        public ICommand Ok { get; set; }
        public ICommand Cancel { get; set; }


        [Required(ErrorMessage = "Имя проекта не должно быть пустым.")]
        public string ProjectName
        {
            get => _projectName;
            set => SetProperty(ref _projectName, value, true);
        }
        private string _projectName = "Новый проект";

        private void OkCommand()
        {
            ValidateAllProperties();
            if (HasErrors)
                return;

            CloseAction?.Invoke(true);
        }

        private void CancelCommand() 
            => CloseAction?.Invoke(false);

    }
}
