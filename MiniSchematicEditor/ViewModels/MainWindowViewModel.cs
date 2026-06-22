using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MiniSchematicEditor.Models;
using MiniSchematicEditor.Services;
using MiniSchematicEditor.Services.Interfaces;
using MiniSchematicEditor.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MiniSchematicEditor.ViewModels
{
    public class MainWindowViewModel : ObservableObject
    {
        private bool isProject = false;
        private readonly IJsonService _jsonService;
        private readonly IFileService _fileService;
        private readonly IMessageService _messageService;

        public MainWindowViewModel()
        {
            _jsonService = new JsonService();
            _fileService = new FileService();
            _messageService = new MessageService();

            CreateNewProject = new RelayCommand(CreateNewProjectCommand);
            OpenProject = new RelayCommand(OpenProjectCommand);
            SaveProject = new RelayCommand(SaveProjectCommand);
            AddBlock = new RelayCommand(AddBlockCommand);
            RemoveBlock = new RelayCommand(RemoveBlockCommand);
            SelectBlock = new RelayCommand<BlockModel>(SelectBlockCommand);
        }

        public ICommand CreateNewProject { get; set; }
        public ICommand OpenProject { get; set; }
        public ICommand SaveProject { get; set; }
        public ICommand AddBlock { get; set; }
        public ICommand RemoveBlock { get; set; }
        public ICommand SelectBlock { get; set; }

        // Коллекция блоков
        public ObservableCollection<BlockModel> Blocks
        {
            get => _blocks;
            set => SetProperty(ref _blocks, value, nameof(Blocks));
        }
        private ObservableCollection<BlockModel> _blocks = new();

        // Выбранный блок
        public BlockModel? SelectedBlock
        {
            get => _selectedBlock;
            set => SetProperty(ref _selectedBlock, value, nameof(SelectedBlock));
        }
        private BlockModel? _selectedBlock;

        // Имя проекта
        public string ProjectName
        {
            get => _projectName;
            set => SetProperty(ref _projectName, value, nameof(ProjectName));
        }
        private string _projectName = string.Empty;

        // Список всех типов блок
        public IEnumerable<BlockType> AllBlockTypes => Enum.GetValues<BlockType>();


        /// <summary>
        /// Создание нового проекта
        /// </summary>
        private void CreateNewProjectCommand()
        {
            if (isProject == true)
            {
                bool? dialogResult = _messageService.ShowQuestionYesNoCancel(
                    "Текущий проект открыт. Сохранить изменения перед созданием нового проекта?",
                    "Внимание"
                );

                if (dialogResult == true)
                    SaveProjectCommand();
                else if (dialogResult == null)
                    return;
            }

            InputDialog inputDialog = new InputDialog();

            if(inputDialog.ShowDialog() == true)
            {
                if(inputDialog.DataContext is InputDialogViewModel viewModel)
                {
                    Blocks.Clear();
                    SelectedBlock = null;
                    ProjectName = viewModel.ProjectName;
                }
            }
            else
                return;

            isProject = true;
        }

        /// <summary>
        /// Открытие проекта из JSON-файла
        /// </summary>
        private void OpenProjectCommand()
        {
            if (isProject == true)
            {
                bool? dialogResult = _messageService.ShowQuestionYesNoCancel(
                    "Текущий проект открыт. Сохранить изменения перед созданием нового проекта?",
                    "Внимание"
                );

                if (dialogResult == true)
                    SaveProjectCommand();
                else if (dialogResult == null)
                    return;
            }

            string? filePath = _fileService.OpenFileDialog();
            if (string.IsNullOrWhiteSpace(filePath))
                return;

            ProjectData? projectData = _jsonService.LoadProject(filePath);
            if (projectData == null)
            {
                _messageService.ShowError(
                    "Не удалось открыть проект. Файл поврежден, пуст или имеет неверный формат JSON.",
                    "Ошибка загрузки"
                );

                return;
            }

            Blocks.Clear();
            SelectedBlock = null!;
            ProjectName = projectData.ProjectName;

            foreach (var blockData in projectData.Blocks)
            {
                if (!Enum.TryParse(blockData.Type, out BlockType blockType))
                    blockType = BlockType.Square;

                BlockModel loadedBlock = new BlockModel
                {
                    UId = blockData.UId,
                    Name = blockData.Name,
                    Type = blockType,
                    X = blockData.X,
                    Y = blockData.Y,
                    IsActive = blockData.IsActive
                };

                loadedBlock.IsNameUniqueChecker = IsBlockNameUnique;

                Blocks.Add(loadedBlock);
            }

            isProject = true;
        }

        /// <summary>
        /// Сохранение проекта в JSON-файл
        /// </summary>
        private void SaveProjectCommand()
        {
            if (isProject == false)
                return;

            bool hasInvalidBlocks = Blocks.Any(b => b.HasErrors);

            if (hasInvalidBlocks)
            {
                _messageService.ShowError(
                    "Невозможно сохранить проект! Исправьте ошибки валидации в блоках (поля, подсвеченные красным).",
                    "Ошибка валидации"
                );
                return;
            }

            string? filePath = _fileService.SaveFileDialog(ProjectName);

            if (string.IsNullOrWhiteSpace(filePath)) 
                return;

            ProjectData projectData = new ProjectData
            {
                ProjectName = this.ProjectName,
                Blocks = this.Blocks.Select(x => new BlockData
                {
                    UId = x.UId,
                    Name = x.Name,
                    Type = x.Type.ToString(),
                    X = x.X,
                    Y = x.Y,
                    IsActive = x.IsActive
                }).ToList()
            };

            bool success = _jsonService.SaveProject(filePath, projectData);

            if (!success)
                _messageService.ShowError(
                   "Не удалось сохранить проект. Возможно, у приложения нет прав на запись в эту папку или файл занят другим процессом.",
                   "Ошибка сохранения"
                );
        }

        /// <summary>
        /// Добавление блоков на рабочую плоскость
        /// </summary>
        private void AddBlockCommand()
        {
            if (isProject == false)
                return;

            var currentNames = Blocks.Select(x => x.Name).ToList();

            AddBlockDialog addBlockDialog = new AddBlockDialog();
            var viewModel = new AddBlockViewModel(currentNames);
            addBlockDialog.DataContext = viewModel;

            viewModel.CloseAction = (result) =>
            {
                addBlockDialog.DialogResult = result;
                addBlockDialog.Close();
            };

            if (addBlockDialog.ShowDialog() == true)
            {
                BlockModel newBlock = new BlockModel
                {
                    Name = viewModel.Name,
                    Type = viewModel.SelectedType,
                    X = viewModel.X,
                    Y = viewModel.Y,
                    IsActive = true
                };

                newBlock.IsNameUniqueChecker = IsBlockNameUnique;
                Blocks.Add(newBlock);
                SelectedBlock = newBlock;
            }
        }

        /// <summary>
        /// Удаление блока
        /// </summary>
        private void RemoveBlockCommand()
        {
            if (isProject == false || SelectedBlock == null)
                return;

            Blocks.Remove(SelectedBlock);
            SelectedBlock = null;
        }

        /// <summary>
        /// Установка выбранного блока на рабочей зоне
        /// </summary>
        public void SelectBlockCommand(BlockModel? block) 
            => SelectedBlock = block!;

        /// <summary>
        /// Проверка уникальности имени
        /// </summary>
        private bool IsBlockNameUnique(string id, string name) 
            => !Blocks.Any(b => b.UId != id && string.Equals(b.Name, name, StringComparison.OrdinalIgnoreCase));
    }
}
