using AdonisUI.Controls;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BikiTools.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : AdonisWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            TextBox otherTextBox = sender == TxtInput ? TxtOutput : TxtInput;
            otherTextBox.ScrollToVerticalOffset(e.VerticalOffset);
            otherTextBox.ScrollToHorizontalOffset(e.HorizontalOffset);
        }
    }
}
