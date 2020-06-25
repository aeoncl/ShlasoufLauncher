using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ShlasoufLauncherCore.Views.UserControls
{
    /// <summary>
    /// Logique d'interaction pour SetupBrowseUserControl.xaml
    /// </summary>
    public partial class SetupBrowseUserControl : UserControl
    {
        public SetupBrowseUserControl()
        {
            InitializeComponent();
        }

        private void TextBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var textBox = (sender as TextBox);
            if (textBox != null)
                textBox.SelectAll();
        }
    }
}
