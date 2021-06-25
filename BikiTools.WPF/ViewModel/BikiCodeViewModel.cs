using Microsoft.Toolkit.Mvvm.ComponentModel;
using BikiTools;

namespace BikiTools.ViewModel
{
    public class BikiCodeViewModel : ObservableObject
    {
        private string input;
        public string Input
        {
            get => input;
            set
            {
                SetProperty(ref input, value);
            }
        }

        private string output;
        public string Output
        {
            get => output;
            set => SetProperty(ref output, value);
        }
    }
}
