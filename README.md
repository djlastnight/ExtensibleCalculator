# Support me on Patreon
<a href="https://www.patreon.com/djlastnight" style="font-size:50px">
  <img src="https://c5.patreon.com/external/logo/rebrandLogoIconMark@2x.png"
       height="40"
       style="vertical-align:top" />
  Click here to become a patron and get your reward!
    <img src="https://c5.patreon.com/external/logo/rebrandLogoIconMark@2x.png"
       height="40"
       style="vertical-align:top" />
</a>  

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
