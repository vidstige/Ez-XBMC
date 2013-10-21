using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace EzXBMC.Controls
{
    /// <summary>
    /// Interaction logic for FolderSelector.xaml
    /// </summary>
    public partial class FolderSelector : UserControl
    {
        public FolderSelector()
        {
            InitializeComponent();
        }

        private void MyBorderedButton_DragEnter(object sender, DragEventArgs e)
        {
            Console.WriteLine("Drag Enter");
        }
        
        private void MyBorderedButton_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] fileList = (string[])e.Data.GetData(DataFormats.FileDrop, false);
                var dir = fileList.FirstOrDefault(f => Directory.Exists(f));
                if (dir != null)
                {
                    Select(dir);
                }
            }
        }

        private void Select(string directoryPath)
        {
            _text.Text = directoryPath;
        }
    }
}
