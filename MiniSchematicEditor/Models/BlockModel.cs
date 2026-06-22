using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniSchematicEditor.Models
{
    /// <summary>
    /// Класс блока для ViewModel
    /// </summary>
    public class BlockModel : ObservableValidator
    {
        public string UId { get; set; } = Guid.NewGuid().ToString();


        [Required(ErrorMessage = "Имя блока не должно быть пустым")]
        [CustomValidation(typeof(BlockModel), nameof(ValidateUniqueName))]
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value, true);
        }
        private string _name = string.Empty;


        public BlockType Type
        {
            get => _type;
            set => SetProperty(ref _type, value, nameof(Type));
        }
        private BlockType _type = BlockType.Square;


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


        public bool IsActive
        {
            get => _isActive;
            set
            {
                if(SetProperty(ref _isActive, value, nameof(IsActive)))
                    OnPropertyChanged(nameof(StatusText));
            }
        }
        private bool _isActive = true;


        public string StatusText => IsActive ? "Active" : "Disabled";


        public Func<string, string, bool>? IsNameUniqueChecker { get; set; }

        /// <summary>
        /// Валидация дубликатов имён
        /// </summary>
        public static ValidationResult? ValidateUniqueName(string name, ValidationContext context)
        {
            var instance = (BlockModel)context.ObjectInstance;

            if (instance.IsNameUniqueChecker != null && !instance.IsNameUniqueChecker(instance.UId, name))
            {
                return new ValidationResult("Блок с таким именем уже существует");
            }
            return ValidationResult.Success;
        }
    }
}
