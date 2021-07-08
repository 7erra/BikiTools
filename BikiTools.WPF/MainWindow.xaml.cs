using AdonisUI.Controls;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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

        private void TxtInput_PreviewDragOver(object sender, DragEventArgs e)
        {
            string file = ((string[])e.Data.GetData(DataFormats.FileDrop))[0];
            if (Path.GetExtension(file) == ".sqf")
            {
                e.Handled = true;
            };
        }

        private void TxtInput_Drop(object sender, DragEventArgs e)
        {
            string file = ((string[])e.Data.GetData(DataFormats.FileDrop))[0];
            TxtInput.Text = $"{File.ReadAllText(file)}";
        }

        private void TxtInput_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            tb.Select(tb.SelectionStart, tb.SelectedText.Trim().Length);
        }
    }
}
