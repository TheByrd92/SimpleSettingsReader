using System.Collections.Generic;
using System.IO;

namespace SimpleSettingsReader
{
    /// <summary>
    /// Functions for file input output and orginization.
    /// </summary>
    public class Parser
    {
        private string directory = "";
        private string fileName = "";
        internal string[] allLines;

        public Parser(string directory, string fileName)
        {
            this.directory = directory;
            this.fileName = fileName;
            InitInformation();
        }

        public Parser(string fullPath)
        {
            int lastSlashIndex = fullPath.LastIndexOf(@"\");
            if (lastSlashIndex == -1)
            {
                lastSlashIndex = fullPath.LastIndexOf("/");
            }
            this.fileName = (lastSlashIndex < fullPath.Length) ? fullPath.Substring(lastSlashIndex + 1) : "";
            this.directory = (lastSlashIndex < fullPath.Length) ? fullPath.Substring(0, lastSlashIndex + 1) : "";
            InitInformation();
        }

        private void InitInformation()
        {
            ReadFullFile();
        }

        private bool IsFileExist()
        {
            return File.Exists(directory + fileName);
        }

        private void ReadFullFile()
        {
            if (IsFileExist())
            {
                allLines = File.ReadAllLines(directory + fileName);
            }
        }

        public void WriteNewSetting(string category, string name, string value)
        {
            for (int i = 0; i < allLines.Length; i++)
            {
                if (allLines[i].StartsWith("[" + category))
                {
                    for (int j = i + 1; j < allLines.Length; j++)
                    {
                        if(allLines[j].StartsWith(name))
                        {
                            return;
                        }
                        if(allLines[j].StartsWith("["))
                        {
                            i = j;
                            break;
                        }
                    }
                }
            }

            List<string> newLines = new List<string>();
            foreach (string line in allLines)
            {
                newLines.Add(line);
                if (line.Contains(category))
                {
                    newLines.Add(name + "=" + value);
                }
            }

            string toWrite = "";
            foreach (string line in newLines)
            {
                toWrite += line + "\n";
            }

            File.WriteAllText(directory + fileName, toWrite);
            ReadFullFile();
        }

        public void DeleteSetting(string category, string name)
        {
            List<string> newLines = new List<string>();
            for (int i = 0; i < allLines.Length; i++)
            {
                newLines.Add(allLines[i]);
                if (allLines[i].StartsWith("[" + category))
                {
                    for (int j = i + 1; j < allLines.Length; j++)
                    {
                        if (allLines[j].StartsWith(name))
                        {
                            continue;
                        }
                        newLines.Add(allLines[j]);
                        if (allLines[j].StartsWith("["))
                        {
                            i = j;
                            break;
                        }
                    }
                }
            }

            string toWrite = "";
            foreach (string line in newLines)
            {
                toWrite += line + "\n";
            }

            File.WriteAllText(directory + fileName, toWrite);
            ReadFullFile();
        }
    }
}
