using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MiniSchematicEditor.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MiniSchematicEditor.ViewModels
{
    public class AddBlockViewModel : ObservableValidator
    {
        private readonly IEnumerable<string> _existingNames;

        public AddBlockViewModel(IEnumerable<string> existingNames) 
        {
            _existingNames = existingNames;

            Ok = new RelayCommand(OkCommand);
            Cancel = new RelayCommand(CancelCommand);
            ValidateAllProperties();
        }

        public Action<bool> CloseAction { get; set; }
        public ICommand Ok { get; set; }
        public ICommand Cancel { get; set; }
        public IEnumerable<BlockType> AllBlockTypes => Enum.GetValues<BlockType>();


        [Required(ErrorMessage = "Имя блока не должно быть пустым.")]
        [CustomValidation(typeof(AddBlockViewModel), nameof(ValidateUniqueName))]
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value, true);
        }
        private string _name = string.Empty;

        public BlockType SelectedType
        {
            get => _selectedType;
            set => SetProperty(ref _selectedType, value, nameof(SelectedType));
        }
        private BlockType _selectedType;

        [Range(0, 1500, ErrorMessage = "Координата X должна быть в диапазоне от 0 до 1500")]
        public double X
        {
            get => _x;
            set => SetProperty(ref _x, value, true);
        }
        private double _x;

        [Range(0, 850, ErrorMessage = "Координата Y должна быть в диапазоне от 0 до 850")]
        public double Y
        {
            get => _y;
            set => SetProperty(ref _y, value, true);
        }
        private double _y;

        /// <summary>
        /// Проверка дубликатов
        /// </summary>
        public static ValidationResult? ValidateUniqueName(string name, ValidationContext context)
        {
            var instance = (AddBlockViewModel)context.ObjectInstance;
            if (instance._existingNames != null && instance._existingNames.Contains(name, StringComparer.OrdinalIgnoreCase))
            {
                return new ValidationResult("Блок с таким именем уже существует в проекте.");
            }
            return ValidationResult.Success;
        }

        private void OkCommand()
        {
            ValidateAllProperties();
            if (HasErrors)
                return;

            CloseAction?.Invoke(true);
        }

        private void CancelCommand()
        {
            CloseAction?.Invoke(false);
        }
    }
}
