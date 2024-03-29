﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ShlasoufLauncherCore.Services
{
    public class FileDialogHelperService : IFileDialogHelperService
    {

        public string OpenFileBrowser(string path)
        {
            OpenFileDialog folderBrowser = new OpenFileDialog();
            // Set validate names and check file exists to false otherwise windows will
            // not let you select "Folder Selection."
            folderBrowser.InitialDirectory = path;
            folderBrowser.ValidateNames = false;
            folderBrowser.CheckFileExists = false;
            folderBrowser.CheckPathExists = false;
            // Always default to Folder Selection.
            folderBrowser.FileName = "Sélectionner un dossier";

            string finalPath = path;
            if (folderBrowser.ShowDialog() == true)
            {
                finalPath = Path.GetDirectoryName(folderBrowser.FileName);
            }
            return finalPath;
        }
    }
}
