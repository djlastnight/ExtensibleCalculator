namespace CalculatorCoreTests
{
    using System;
    using System.Data;
    using CalculatorCore;
    using NUnit.Framework;

    [TestFixture]
    public class TestOperations
    {
        [Test]
        [TestCase(0.0f, 0.0f)]
        [TestCase(1.0f, 2.0f)]
        [TestCase(-20.2f, 33.15f)]
        [TestCase(float.MaxValue, 1.0f)]
        [TestCase(float.MaxValue, float.MaxValue)]
        [TestCase(float.MinValue, -1.0f)]
        public void TestSum(float a, float b)
        {
            double result = Operations.Sum(a, b);
            Assert.AreEqual(a + b, result);
        }

        [Test]
        [TestCase(0.0f, 0.0f)]
        [TestCase(1.0f, 2.0f)]
        [TestCase(-1230.2f, 433.15f)]
        [TestCase(float.MaxValue, 1.0f)]
        [TestCase(float.MaxValue, float.MaxValue)]
        [TestCase(float.MinValue, -1.0f)]
        public void TestSubtract(float a, float b)
        {
            double result = Operations.Subtract(a, b);
            Assert.AreEqual(a - b, result);
        }

        [Test]
        [TestCase(0.0f, 0.0f)]
        [TestCase(1.0f, 2.0f)]
        [TestCase(-23.2f, 55.187778f)]
        [TestCase(float.MaxValue, 1.0f)]
        [TestCase(float.MaxValue, float.MaxValue)]
        [TestCase(float.MinValue, -1.0f)]
        public void TestMultiply(float a, float b)
        {
            double result = Operations.Multiply(a, b);
            Assert.AreEqual(a * b, result);
        }

        [Test]
        [TestCase(0.0f, 0.0f)]
        [TestCase(12.0f, 0.0f)]
        [TestCase(1.0f, 33.0f)]
        [TestCase(-666.666f, 555.555123123f)]
        [TestCase(float.MaxValue, 1.0f)]
        [TestCase(float.MaxValue, float.MaxValue)]
        [TestCase(float.MinValue, -1.0f)]
        public void TestDivide(float a, float b)
        {
            double result = Operations.Divide(a, b);
            Assert.AreEqual(a / b, result);
        }

        [Test]
        [TestCase("1+1", 2.0)]
        [TestCase("1+12.3", 13.3)]
        [TestCase("(1-2)*33.3", -33.3)]
        [TestCase("(12*5)-61", -1.0)]
        [TestCase("1*-12.3", -12.3)]
        [TestCase("(12-3)*2 - 9*2 + 15 / 3 + 7", 12)]
        [TestCase("(((1 + 2) * 3 + 4) * 5) - 6", 59)]
        [TestCase("1 + 2*((3+4)*5) + 6", 77)]
        [TestCase("8 * ((1-3)/2 - (31-1)/15)", -24)]
        [TestCase("10/3", 3.3333333333333335)]
        [TestCase("sqrt(225)", 15)]
        [TestCase("sqrt(500)", 22.360679774997902)]
        [TestCase("10 * (sqrt(144) + 8)", 200)]
        [TestCase("2 * (sqrt(16) + sqrt(25))", 18)]
        [TestCase("100 - 2 * ((sqrt(4) + sqrt(9)) + 3)", 84)]
        [TestCase("sqrt(3.1415)", 1.77242771361768)]
        [TestCase("sqrt(sqrt(625))", 5)]
        [TestCase("sqrt(sqrt(2401)) + 1", 8)]
        [TestCase("6 * (reciproc(10) + 0.900) / 2", 3)]
        [TestCase("sqrt(reciproc(0.01))", 10)]
        [TestCase("sqrt(sqrt(sqrt(23)))", 1.47984414824496)]
        [TestCase("sqrt(sqrt(sqrt(sqrt(65536))))", 2)]
        [TestCase("sqrt((123 - 6) * 2)", 15.2970585407784)]
        [TestCase("sqrt(reciproc(sqrt(reciproc(100))))", 3.16227766016838)]
        [TestCase("sqrt((12 + 5))", 4.12310562561766)]
        [TestCase("sqrt(((12 + 5)))", 4.12310562561766)]
        [TestCase("reciproc(9) + 3 * 5 / 77 + sqrt(8)", 3.1343434306624958)]
        [TestCase("negate(12 + sqrt(3))", -13.7320508075689)]
        [TestCase("reciproc(negate(55 * (1 + sqrt(3)) - 41))", -0.0091522462457808498)]
        [TestCase("reciproc(negate(55 * (1 + sqrt(3)) - 41)) - 1", -1.0091522462457807)]
        [TestCase("2 ^ 3", 8)]
        [TestCase("2 ^ 3 ^ 1", 8)]
        [TestCase("2 ^ 3 ^ 2", 512)] /* Exponential is right associative */
        [TestCase("2 ^ (3 ^ 2)", 512)]
        [TestCase("(2 ^ 3) ^ 2", 64)]
        [TestCase("(2 ^ 3 ^ 2)", 512)]
        [TestCase("(2 ^ 10) / 2", 512)]
        [TestCase("2 ^ 10 / 2", 512)] /* Exponential has higher precedence */
        [TestCase("1 ^ negate(1)", 1)]
        [TestCase("2 ^ negate(1)", 0.5)]
        [TestCase("sind(90)", 1.0)]
        [TestCase("sind(9)", 0.156434465040231)]
        [TestCase("cosd(0)", 1.0)]
        [TestCase("tand(45)", 1.0)]
        [TestCase("1 + (2 ^ 3)", 9)]
        [TestCase("(1 + 2) ^ 3", 27)]
        [TestCase("1 + 2 ^ 3", 9)] /* Exponential must have higher precedence */
        [TestCase("sind(sqrt(122) + 15)", 0.439082583771293)]
        [TestCase("sind(sqrt(122.0) + 15 / 3)", 0.27639829937591698)]
        [TestCase("log(100.000)", 2)]
        [TestCase("(negate(sqrt(25.2) * 3.333 - 22/7.0) + 2.2) / -55.233 + sind(62)", 1.089140811908613)]
        [TestCase("(negate(sqrt(25.2) * 3.333 - 22 / 7.0) + 2.2) / -55.233 - 3 ^ sind(62 ^ 2.222)", -0.15150092965915213)]
        public void TestParse(string expression, double expectedResult)
        {
            Assert.AreEqual(expectedResult, Operations.Compute(expression));
        }

        [Test]
        public void TestParse_DivideByZero_ExpectsDivideByZeroException()
        {
            Assert.Throws(
                typeof(DivideByZeroException),
                () => Operations.Compute("1.123/0.0"));
        }

        [Test]
        public void TestParse_IllegalExpression_ExpectsEvaluateException()
        {
            Assert.Throws(
                typeof(EvaluateException),
                () => Operations.Compute("a"));
        }

        [Test]
        public void TestParse_IllegalTangens_ExpectsInvalidOperationException()
        {
            Assert.Throws(
                typeof(InvalidOperationException),
                () => Operations.Compute("tand(90)"));
        }

        [Test]
        public void TestParse_IllegalTangensExpression_ExpectsInvalidOperationException()
        {
            Assert.Throws(
                typeof(InvalidOperationException),
                () => Operations.Compute("12 + tand(90)"));
        }
    }
}