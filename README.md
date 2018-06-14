# djlastnight's Calculator

![alt text](https://raw.githubusercontent.com/djlastnight/ExtensibleCalculator/master/Calculator/Images/Screenshot.png)

The core provides experimental math expression parser.  

Usage example:  
```double result = CalculatorCore.Operations.Compute("(negate(sqrt(123.4) * 5.678 - 910 + 11.12) ^ 13.14) / -15.1617");```

You can easily add new custom math operators and functions to it.
They are defined at [Custom.cs](https://github.com/djlastnight/ExtensibleCalculator/blob/master/CalculatorCore/Custom.cs).  

Currently all the user defined ```operators``` has equal (but higher than the 'standard' operators like '+', '-', '*', '/') precedence.
They could be left (not tested) or right associative.

Currently the WPF app supports keyboard input for the 'standard' operators and the digits.  
[Try it now!](https://github.com/djlastnight/ExtensibleCalculator/releases)  

djlastnight,
2018
