using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;


namespace SubsConverter.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        [ObservableProperty]
        private ObservableCollection<string> files = new();

        [ObservableProperty]
        private string status = "Επιλέξτε αρχείο/αρχεία υποτίτλων...";

        [RelayCommand]
        public async Task OpenFilesAsync()
        {
            var topLevel = App.MainWindow;

            var options = new FilePickerOpenOptions
            {
                Title = "Άνοιγμα Αρχείου/Αρχείων Υποτίτλων",
                AllowMultiple = true,
                FileTypeFilter = new[]
                {
                    new FilePickerFileType("Αρχεία .srt") { Patterns = new[] { "*.srt" } },
                    new FilePickerFileType("Text Files") { Patterns = new[] { "*.txt" } }
                }
            };

            var result = await topLevel.StorageProvider.OpenFilePickerAsync(options);

            if (result.Any())
            {
                Files.Clear();
                foreach (var file in result)
                {
                    if (Path.GetExtension(file.TryGetLocalPath()) == ".srt")
                    {
                        Files.Add(file.Path.LocalPath);
                    }
                }
                Status = $"Επίλέχθηκαν {Files.Count} αρχεία.";
                foreach (var file in Files)
                {
                    Status += $"\n{file}";
                }
            }
        }

        [RelayCommand]
        public async Task ConvertFilesAsync()
        {
            if (Files.Count == 0)
            {
                Status = "Δεν επιλέχθηκαν αρχεία!";
                return;
            }

            Status = "";

            foreach (var file in Files)
            {
                try
                {
                    var content = await File.ReadAllTextAsync(file, Encoding.Default);
                    var utf8File = Path.ChangeExtension(file, ".srt");
                    await File.WriteAllTextAsync(utf8File, content, Encoding.UTF8);
                    Status += $"Μετατράπηκε με επιτυχία: {file}\n";
                }
                catch (Exception ex)
                {
                    Status = $"Σφάλμα στη μετατροπή{file}: {ex.Message}\n";
                }
            }
        }

        [RelayCommand]
        public void AddFilesFromDragAndDrop(IEnumerable<string> droppedFiles)
        {
            foreach (var droppedFile in droppedFiles)
            {
                if (Path.GetExtension(droppedFile) == ".srt")
                {
                    Files.Add(droppedFile);
                }
            }

            Status = $"Επιλέχθηκαν {Files.Count} αρχεία.";

            foreach (var file in Files)
            {
                Status += $"\n{file}";
            }

        }

        [RelayCommand]
        public void ClearFiles()
        {
            Files.Clear();
            Status = "Επιλέξτε αρχείο/αρχεία υποτίτλων...";
        }
    }
}
