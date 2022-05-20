using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfCalculator.Common.MVVM;
using WpfCalculator.Services;

namespace WpfCalculator.ViewModels
{
    /// <summary>
    /// View model for the calculator view.
    /// </summary>
    public class CalculatorViewModel : ViewModel
    {
        private static readonly CultureInfo _culture = CultureInfo.GetCultureInfo("nl-NL");

        private decimal _numberA;
        private decimal _numberB;
        private char? _operator;

        private string _displayNumber;
        private string _displaySum;

        private ICommand _numberCommand;
        private ICommand _pointCommand;
        private ICommand _operatorCommand;
        private ICommand _plusMinusCommand;
        private ICommand _deleteCommand;
        private ICommand _clearCommand;
        private ICommand _resetCommand;

        /// <summary>
        /// Constructor.
        /// </summary>
        public CalculatorViewModel()
        {
            _numberCommand = new RelayCommand(c => AddNumber(c), c => CanExecuteNumber());
            _pointCommand = new RelayCommand(c => AddPoint(), c => CanAddPoint());
            _operatorCommand = new RelayCommand(c => AddOperator(c), c => CanExecuteOperator());
            _plusMinusCommand = new RelayCommand(c => AddPlusMinus(), c => CanExecuteOperator());
            _deleteCommand = new RelayCommand(c => RemoveNumber(), c => CanRemoveNumber());
            _clearCommand = new RelayCommand(c => Clear(), c => CanClear());
            _resetCommand = new RelayCommand(c => Reset());

            _displaySum = string.Empty;
            _displayNumber = "0";
            State = CalculatorState.InputFirstNumber;
        }

        #region [Properties]

        /// <summary>
        /// Display number.
        /// </summary>
        public string DisplayNumber
        {
            get { return _displayNumber; }
            set { SetField(ref _displayNumber, value.Trim(), nameof(DisplayNumber)); }
        }

        /// <summary>
        /// Display sum.
        /// </summary>
        public string DisplaySum
        {
            get { return _displaySum; }
            set { SetField(ref _displaySum, value, nameof(DisplaySum)); }
        }

        /// <summary>
        /// Nummer command.
        /// </summary>
        public ICommand NumberCommand
        {
            get { return _numberCommand; }
            set { SetField(ref _numberCommand, value, nameof(NumberCommand)); }
        }

        /// <summary>
        /// Point command.
        /// </summary>
        public ICommand PointCommand
        {
            get { return _pointCommand; }
            set { SetField(ref _pointCommand, value, nameof(PointCommand)); }
        }

        /// <summary>
        /// Operator command.
        /// </summary>
        public ICommand OperatorCommand
        {
            get { return _operatorCommand; }
            set { SetField(ref _operatorCommand, value, nameof(OperatorCommand)); }
        }

        /// <summary>
        /// Plus minus command.
        /// </summary>
        public ICommand PlusMinusCommand
        {
            get { return _plusMinusCommand; }
            set { SetField(ref _plusMinusCommand, value, nameof(PlusMinusCommand)); }
        }

        /// <summary>
        /// Delete command.
        /// </summary>
        public ICommand DeleteCommand
        {
            get { return _deleteCommand; }
            set { SetField(ref _deleteCommand, value, nameof(DeleteCommand)); }
        }

        /// <summary>
        /// Clear command.
        /// </summary>
        public ICommand ClearCommand
        {
            get { return _clearCommand; }
            set { SetField(ref _clearCommand, value, nameof(ClearCommand)); }
        }

        /// <summary>
        /// Reset command.
        /// </summary>
        public ICommand ResetCommand
        {
            get { return _resetCommand; }
            set { SetField(ref _resetCommand, value, nameof(ResetCommand)); }
        }

        /// <summary>
        /// State, is made internal, can be used in unit test if needed to test te state machine of the calculator. 
        /// </summary>
        internal CalculatorState State { get; private set; }
        #endregion [Properties]

        /// <summary>
        /// Add number to the display if the param is valid.
        /// </summary>
        /// <param name="param">Textual number</param>
        public void AddNumber(object? param)
        {
            var text = param?.ToString() ?? string.Empty;

            switch (State)
            {
                case CalculatorState.InputFirstNumber:
                    if (DisplayNumber == "0")
                    {
                        DisplayNumber = text ?? "0";
                    }
                    else if (DisplayNumber == "-0")
                    {
                        DisplayNumber = "-" + text;
                    }
                    else
                    {
                        DisplayNumber += text;
                    }
                    break;
                case CalculatorState.InputSecondNumber:
                    if (DisplayNumber == "0")
                    {
                        DisplayNumber = text ?? "0";
                    }
                    else if (DisplayNumber == "-0")
                    {
                        DisplayNumber = "-" + text;
                    }
                    else
                    {
                        DisplayNumber += text;
                    }
                    break;
                case CalculatorState.Result:
                    State = CalculatorState.InputSecondNumber;
                    DisplayNumber = text;
                    break;
            }
        }

        /// <summary>
        /// Add point to the display. Based on dutch culture.
        /// </summary>
        private void AddPoint()
        {
            DisplayNumber += ",";
        }

        /// <summary>
        /// Add the operator to the sum and if possible calculate the sum.
        /// </summary>
        /// <param name="param">Operator</param>
        private void AddOperator(object? param)
        {
            string text = $"{param}" ?? string.Empty;
            if (text.Length == 1 && text != "=")
            {
                _operator = text.First();
            }

            if (_operator != null)
            {
                switch (State)
                {
                    case CalculatorState.InputFirstNumber:
                        bool canConvertNumberA = decimal.TryParse(DisplayNumber, NumberStyles.Number, _culture, out _numberA);
                        if (!canConvertNumberA)
                        {
                            DisplayNumber = "Invalid number";
                            State = CalculatorState.Error;
                            return;
                        }

                        State = CalculatorState.InputSecondNumber;
                        DisplayNumber = "0";
                        DisplaySum += $"{_numberA} {_operator} ";
                        break;
                    case CalculatorState.InputSecondNumber:
                        bool canConvertNumberB = decimal.TryParse(DisplayNumber, NumberStyles.Number, _culture, out _numberB);
                        if (!canConvertNumberB)
                        {
                            DisplayNumber = "Invalid number";
                            State = CalculatorState.Error;
                            return;
                        }

                        try
                        {
                            _numberA = CalculatorService.Calculate(_operator.Value, _numberA, _numberB);
                        }
                        catch (Exception ex)
                        {
                            DisplayNumber = ex.Message;
                            State = CalculatorState.Error;
                            return;
                        }

                        var operatorText = $" {_operator} ";
                        if (!DisplaySum.EndsWith(operatorText))
                        {
                            DisplaySum += operatorText;
                        }

                        DisplaySum += $" {_numberB}";
                        DisplayNumber = _numberA.ToString();

                        State = CalculatorState.Result;
                        break;
                    case CalculatorState.Result:
                        DisplaySum += $" {_operator} ";
                        DisplayNumber = "0";
                        State = CalculatorState.InputSecondNumber;
                        break;
                }
            }


        }

        /// <summary>
        /// Add plus/minus in front of the number.
        /// </summary>
        private void AddPlusMinus()
        {
            if (DisplayNumber.StartsWith("-"))
            {
                DisplayNumber = DisplayNumber[1..];
            }
            else
            {
                DisplayNumber = "-" + DisplayNumber;
            }
        }

        /// <summary>
        /// Reset to calculator and state machine.
        /// </summary>
        private void Reset()
        {
            DisplaySum = string.Empty;
            DisplayNumber = "0";

            State = CalculatorState.InputFirstNumber;
        }

        /// <summary>
        /// Clear the number/display.
        /// </summary>
        private void Clear()
        {
            DisplayNumber = "0";
        }

        /// <summary>
        /// Remove digit from display.
        /// </summary>
        private void RemoveNumber()
        {
            DisplayNumber = DisplayNumber[1..];
            if (DisplayNumber.Length == 0)
            {
                DisplayNumber = "0";
            }
        }

        /// <summary>
        /// Check if execute operator can be executed.
        /// </summary>
        /// <returns>True if can be executed</returns>
        private bool CanExecuteOperator()
        {
            return State != CalculatorState.Error;
        }

        /// <summary>
        /// Check if add point command can be executed.
        /// </summary>
        /// <returns>True if can be executed</returns>
        private bool CanAddPoint()
        {
            return !DisplayNumber.Contains(',') && State != CalculatorState.Error;
        }

        /// <summary>
        /// Check if number command can be executed.
        /// </summary>
        /// <returns>True if can be executed</returns>
        private bool CanExecuteNumber()
        {
            return State != CalculatorState.Error; 
        }

        /// <summary>
        /// Check if remove command can be executed.
        /// </summary>
        /// <returns>True if can be executed</returns>
        private bool CanRemoveNumber()
        {
            return State != CalculatorState.Error;
        }

        /// <summary>
        /// Check if clear command can be executed.
        /// </summary>
        /// <returns>True if can be executed</returns>
        private bool CanClear()
        {
            return State != CalculatorState.Error;
        }

    }
}
