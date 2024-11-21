using Avalonia.Controls;
using Avalonia.Input;
using SubsConverter.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace SubsConverter.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitializeComponent();
            DataContext = new MainWindowViewModel();

            // Attach event handlers in code-behind for drag and drop
            AddHandler(DragDrop.DragOverEvent, OnDragOver);
            AddHandler(DragDrop.DropEvent, OnDrop);
        }

        // Event handler for drag-over event
        private void OnDragOver(object? sender, DragEventArgs e)
        {
            // Set the drag effect (e.g., copy) and mark the event as handled
            e.DragEffects = DragDropEffects.Copy;
            e.Handled = true;
        }


        // Event handler for drop event
        private void OnDrop(object? sender, DragEventArgs e)
        {
            // Check if the dropped data contains files
            var files = e.Data.GetFiles();

            if (files.Count() > 0)
            {
                List<string> fileList = new List<string>();
                foreach (var file in files)
                {
                    fileList.Add(file.Path.AbsolutePath.ToString());
                }
                var vm = (MainWindowViewModel)DataContext;
                vm.AddFilesFromDragAndDrop(fileList);
            }
            //vm.AddFilesFromDragAndDrop(files);
            e.Handled = true;
        }
    }
}