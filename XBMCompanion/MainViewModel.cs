using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace XBMCompanion
{
    public class MainViewModel: ViewModel
    {
        private readonly List<string> _log = new List<string>();
        private string _sourceFolder;
        private string _targetFolder;
        private FileSystemWatcher _sourceWatcher;

        internal void Browse()
        {            
        }

        public string SourceFolder
        {
            get { return _sourceFolder; }
            set
            {
                _sourceFolder = value;
                RaisePropertyChanged("SourceFolder");
                MoveFiles();

                if (Directory.Exists(_sourceFolder))
                {
                    if (_sourceWatcher != null)
                    {
                        _sourceWatcher.Changed -= SourceChanged;
                    }

                    _sourceWatcher = new FileSystemWatcher(_sourceFolder);
                    _sourceWatcher.NotifyFilter = NotifyFilters.LastWrite;
                    _sourceWatcher.Changed += SourceChanged;
                    _sourceWatcher.EnableRaisingEvents = true;
                }
            }
        }

        private void SourceChanged(object sender, FileSystemEventArgs e)
        {
            MoveFiles();
        }

        public string TargetFolder
        {
            get { return _targetFolder; }
            set
            {
                _targetFolder = value;
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
        }

        private void MoveFiles()
        {
            if (!Directory.Exists(_targetFolder)) return;

            foreach (var f in Directory.EnumerateFiles(_sourceFolder))
            //foreach (var f in new[] { "Breaking.Bad.S05E10.HDTV.x264-ASAP.avi", "Breaking.Bad.S05E11.HDTV.x264-ASAP.avi", "Breaking.Bad.S05E12.HDTV.x264-EVOLVE.avi", "anotherfile.txt" })
            {
                var match = Regex.Match(f, "(.*)\\.S([0-9][0-9])E([0-9][0-9])");
                if (match.Success)
                {
                    var name = match.Groups[1].ToString().Replace('.', ' ');
                    var season = match.Groups[2].ToString();
                    var episode = match.Groups[3].ToString();
                    var extension = Path.GetExtension(f);

                    var targetFilename = string.Format("{0} - S{1}E{2}.{3}", name, season, episode, extension);

                    Log(string.Format("Moving {0} to {1}", f, targetFilename));

                    var dir = Path.Combine(_targetFolder, name);
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }
                    
                    var targetPath = Path.Combine(dir, targetFilename);
                    File.Move(f, targetPath);
                }
            }
        }
    }
}
