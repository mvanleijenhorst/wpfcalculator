using AutoFixture;
using FluentAssertions;
using System.Globalization;
using WpfCalculator.ViewModels;
using Xunit;

namespace WpfCalculator.Tests
{
    /// <summary>
    /// These tests are functional tests. No real unit tests and doesn't cover everthing.
    /// The example let you only see that it no easy to test the UI without any test framework.
    /// </summary>
    public class CalculatorViewModelTest
    {
        private readonly static CultureInfo _culture = CultureInfo.GetCultureInfo("nl-NL");
        private readonly Fixture _fixture;

        public CalculatorViewModelTest()
        { 
            _fixture = new Fixture();
        }

        [Fact]
        public void OperatorAddShouldAdd2Numbers()
        {
            // we use fixture to create a random number - 1001 and add 1000 to prevent that the decimal will be out of range.
            var firstNumber = _fixture.Create < decimal>() - 1001;
            var secondNumber = 1000;
            var result = firstNumber + secondNumber;

            // Arrange & Act
            CalculatorViewModel sut = new();
            sut.DisplayNumber = firstNumber.ToString(_culture);
            sut.OperatorCommand.Execute("+");
            sut.DisplayNumber = secondNumber.ToString(_culture);
            sut.OperatorCommand.Execute("=");

            // Assert
            sut.DisplayNumber.Should().Be(result.ToString(_culture));
        }

        [Fact]
        public void OperatorSubstractShouldSubtract2Numbers()
        {
            var firstNumber = _fixture.Create<decimal>();
            var secondNumber = _fixture.Create<decimal>();

            // Arrange & Act
            CalculatorViewModel sut = new();
            sut.DisplayNumber = firstNumber.ToString(_culture);
            sut.OperatorCommand.Execute("-");
            sut.DisplayNumber = secondNumber.ToString(_culture);
            sut.OperatorCommand.Execute("=");

            // Assert
            sut.DisplayNumber.Should().Be($"{firstNumber - secondNumber}");
        }

        [Fact]
        public void OperatorMultiplyShouldMuliply2Numbers()
        {
            // we use fixture to create a random number then divide by 1000 and
            // multiply by 10 to prevent that the decimal will be out of range.
            var firstNumber = _fixture.Create<decimal>() / 1000;
            var secondNumber = 10;

            // Arrange & Act
            CalculatorViewModel sut = new();
            sut.DisplayNumber = firstNumber.ToString(_culture);
            sut.OperatorCommand.Execute("*");
            sut.DisplayNumber = secondNumber.ToString(_culture);
            sut.OperatorCommand.Execute("=");

            // Assert
            sut.DisplayNumber.Should().Be($"{firstNumber * secondNumber}");
        }

        [Fact]
        public void OperatorDivideShouldDivide2Numbers()
        {
            var firstNumber = _fixture.Create<decimal>();
            var secondNumber = _fixture.Create<decimal>();

            // prevent divide by zero.
            if (secondNumber == 0)
            {
                secondNumber = 1;
            }

            // Arrange & Act
            CalculatorViewModel sut = new();
            sut.DisplayNumber = firstNumber.ToString(_culture);
            sut.OperatorCommand.Execute("/");
            sut.DisplayNumber = secondNumber.ToString(_culture);
            sut.OperatorCommand.Execute("=");

            // Assert
            sut.DisplayNumber.Should().Be($"{firstNumber / secondNumber}");
        }

        [Fact]
        public void OperatorDivideByZeroShouldShowError()
        {
            var firstNumber = _fixture.Create<decimal>();
            var secondNumber = 0;

            // Arrange & Act
            CalculatorViewModel sut = new();
            sut.DisplayNumber = firstNumber.ToString(_culture);
            sut.OperatorCommand.Execute("/");
            sut.DisplayNumber = secondNumber.ToString(_culture);
            sut.OperatorCommand.Execute("=");

            // Assert
            sut.DisplayNumber.Should().Be($"Attempted to divide by zero.");
        }


    }
}