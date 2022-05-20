using WpfCalculator.Common.MVVM;

namespace WpfCalculator.ViewModels
{
    /// <summary>
    /// View model for the MainView.
    /// </summary>
    public class MainViewModel : ViewModel
    {
        private CalculatorViewModel _calculatorViewModel = new() ;

        /// <summary>
        /// Calculate view model.
        /// </summary>
        public CalculatorViewModel CalculatorViewModel
        {
            get { return _calculatorViewModel; }
            set { SetField(ref _calculatorViewModel, _calculatorViewModel, nameof(CalculatorViewModel)); }
        }
    }
}
