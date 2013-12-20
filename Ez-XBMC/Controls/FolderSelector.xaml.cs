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

        // Dependency Property
        public static readonly DependencyProperty FolderProperty =
             DependencyProperty.Register("Folder", typeof(DirectoryInfo),
             typeof(FolderSelector), new PropertyMetadata(OnFolderChangedCallBack));

        // .NET Property wrapper
        public DirectoryInfo Folder
        {
            get { return (DirectoryInfo)GetValue(FolderProperty); }
            set { SetValue(FolderProperty, value); }
        }

        private static void OnFolderChangedCallBack(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var c = sender as FolderSelector;
            if (c != null)
            {
                c.OnFolderChanged();
            }
        }

        protected virtual void OnFolderChanged()
        {
            var folder = Folder;
            if (folder == null) _text.Text = "Drop folder here";
            else _text.Text = Folder.FullName;
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
            Folder = new DirectoryInfo(directoryPath);
        }
    }
}
