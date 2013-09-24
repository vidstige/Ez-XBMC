using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using EzXBMC.Properties;
using System.Threading;
using System.Windows.Threading;

namespace EzXBMC
{
    public class MainViewModel: ViewModel
    {
        private readonly List<string> _log = new List<string>();
        private FileSystemWatcher _sourceWatcher;

        private readonly Thread _moveFilesThread;
        private Dispatcher _bgDispatcher;
        private ManualResetEvent _event = new ManualResetEvent(false);

        public MainViewModel()
        {
            _moveFilesThread = new Thread(FileMover);
            _moveFilesThread.IsBackground = true;
            _moveFilesThread.Start(null);
            _event.WaitOne();
        }

        internal void Browse()
        {
        }

        private void FileMover(object parameter)
        {
            _bgDispatcher = Dispatcher.CurrentDispatcher;
            _event.Set();
            Dispatcher.Run();
        }

        public string SourceFolder
        {
            get { return Settings.Default.SourceFolder; }
            set
            {
                Settings.Default.SourceFolder = value;
                Settings.Default.Save();

                RaisePropertyChanged("SourceFolder");
                MoveFiles();

                WatchSource();
            }
        }

        private void WatchSource()
        {
            if (Directory.Exists(Settings.Default.SourceFolder))
            {
                if (_sourceWatcher != null)
                {
                    _sourceWatcher.Changed -= SourceChanged;
                }

                _sourceWatcher = new FileSystemWatcher(Settings.Default.SourceFolder);
                _sourceWatcher.Error += _sourceWatcher_Error;
                //_sourceWatcher.Filter = "*.*";
                //_sourceWatcher.Changed += SourceChanged;
                _sourceWatcher.Created += SourceChanged;
                _sourceWatcher.EnableRaisingEvents = true;
            }
        }

        private void _sourceWatcher_Error(object sender, ErrorEventArgs e)
        {
        }

        private void SourceChanged(object sender, FileSystemEventArgs e)
        {
            Log(e.ChangeType.ToString());
            MoveFiles();
        }

        public string TargetFolder
        {
            get { return Settings.Default.TargetFolder; }
            set
            {
                Settings.Default.TargetFolder = value;
                Settings.Default.Save();
                RaisePropertyChanged("TargetFolder");
            }
        }        

        public string LogRows
        {
            get { return string.Join("\n", _log); }
        }

        private void Log(string s)
        {
            _log.Add(s);
            RaisePropertyChanged("LogRows");
        }

        internal void WindowLoaded()
        {
            MoveFiles();
            WatchSource();
        }

        private void MoveFiles()
        {
            if (!Directory.Exists(TargetFolder)) return;

            foreach (var path in Directory.EnumerateFiles(SourceFolder))
            {
                var f = Path.GetFileName(path);
                var match = Regex.Match(f, "(.*)\\.S([0-9][0-9])E([0-9][0-9])");
                if (match.Success)
                {
                    var name = match.Groups[1].ToString().Replace('.', ' ');
                    var season = match.Groups[2].ToString();
                    var episode = match.Groups[3].ToString();
                    var extension = Path.GetExtension(f);

                    var targetFilename = string.Format("{0} - S{1}E{2}{3}", name, season, episode, extension);

                    Log(string.Format("Moving {0} to {1}", f, targetFilename));

                    var dir = Path.Combine(TargetFolder, name);
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }
                    
                    var targetPath = Path.Combine(dir, targetFilename);
                    File.Move(path, targetPath);
                }
            }
        }
    }
}
