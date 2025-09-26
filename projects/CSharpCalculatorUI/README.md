# CSharpCalculatorUI

A simple **WinForms-based calculator** written in C# that supports basic arithmetic operations as well as **floating-point numbers** and full expression evaluation.

## Features

* Add, subtract, multiply, and divide numbers
* Handle **floating-point numbers** with decimals
* Evaluate full expressions like `12.5 + 3.2 * 4`
* Prevent invalid operations like division by zero or trailing operators
* Clear output and start a new calculation easily

## How It Works

1. The calculator allows the user to input numbers and operators using buttons on the form.
2. Expressions are constructed in the **text box** as the user clicks buttons.
3. When the **equals** button is pressed:

   * The expression is validated to ensure it does not end with an operator.
   * The program uses `DataTable.Compute` to evaluate the full expression.
   * The result is converted to a `float` for floating-point precision and displayed in the textbox.
4. The **decimal button** ensures that each number can only have one decimal point.

### Example

A user can input:

```
12.5 + 3 * 2.1
```

The calculator will correctly evaluate and display:

```
18.8
```

## Getting Started

### Prerequisites

* [Visual Studio](https://visualstudio.microsoft.com/) or any C# IDE that supports WinForms
* [.NET Framework](https://dotnet.microsoft.com/download/dotnet-framework) installed

### Running the Application

1. Open the solution in Visual Studio.
2. Build the project (`Ctrl+Shift+B`).
3. Run the application (`F5`) and the calculator form will appear.

## Usage

* Click number buttons (`0-9`) to input numbers.
* Click operator buttons (`+ - * /`) to perform operations.
* Use the decimal point button `.` to enter floating-point numbers.
* Press `=` to evaluate the expression.
* Press `C` to clear the current calculation.

## License

This project is licensed under the MIT License in its parent directory.
