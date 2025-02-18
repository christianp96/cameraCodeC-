﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CameraEmguCV
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class DialDefinition : Window
    {
        MainWindow parent;
        public DialDefinition(Emgu.CV.Mat dial)
        {
            InitializeComponent();
            this.parent = (MainWindow)Application.Current.MainWindow;
            if (dial != null)
                dialImage.Source = Utils.ToBitmapSource(dial);
            else
                dialImage.Source = null;
        }

        private void Btn_Accept_Click(object sender, RoutedEventArgs e)
        {
            if (CadranName.Text == string.Empty || CadranType.SelectedValue == null)
                MessageBox.Show("You have to choose a name and a type for the dial! ", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            else
                this.Close();
        }

        

      
    }
}
