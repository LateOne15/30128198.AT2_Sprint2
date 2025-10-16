using Microsoft.VisualStudio.TestTools.UnitTesting;
using Astronomical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astronomical.Tests
{
    [TestClass()]
    public class StatsMathTests
    {
        [TestMethod()]
        public void AverageMeanTest()
        {
            // sorted version: 10, 16, 16, 18, 24, 29, 32, 38, 44, 45, 45, 45, 45, 47, 51, 56, 61, 68, 73, 73, 73, 77, 88, 89
            int[] arrayIn = { 10, 89, 16, 45, 24, 61, 88, 73, 16, 73, 51, 44, 32, 47, 45, 68, 45, 73, 29, 45, 77, 56, 18, 38 };
            List<int> input = arrayIn.ToList();
            string expected = "48.46";
            double result;

            result = StatsMath.AverageMean(input);

            Assert.AreEqual(expected, result.ToString("F"));
        }

        [TestMethod()]
        public void AverageModeTest1()
        {
            int[] arrayIn = { 10, 89, 16, 45, 24, 61, 88, 73, 16, 73, 51, 44, 32, 47, 45, 68, 45, 73, 29, 45, 77, 56, 18, 38 };
            List<int> input = arrayIn.ToList();
            List<int> expected = new List<int>();
            expected.Add(45);

            input.Sort();
            List<int> result = StatsMath.AverageMode(input);

            if (result.Count != 1)
            {
                Assert.Fail();
            }
            Assert.AreEqual(result[0], expected[0]);
        }

        [TestMethod()]
        public void AverageModeTest2()
        {
            int[] arrayIn = { 10, 89, 16, 10, 24, 61, 88, 73, 16, 73, 51, 44, 32, 47, 45, 68, 45, 73, 29, 45, 77, 56, 18, 38 };
            List<int> input = arrayIn.ToList();
            List<int> expected = new List<int>();
            expected.Add(45);
            expected.Add(73);

            input.Sort();
            List<int> result = StatsMath.AverageMode(input);

            if (result.Count != 2)
            {
                Assert.Fail();
            }
            int j = 0;
            foreach (int i in result)
            {
                if (i != expected[j])
                {
                    Assert.Fail();
                }
                j++;
            }
        }

        [TestMethod()]
        public void AverageModeTest3()
        {
            int[] arrayIn = { 10, 89, 16, 16, 24, 61, 88, 73, 16, 73, 51, 44, 32, 47, 45, 68, 45, 73, 29, 45, 77, 56, 18, 38 };
            List<int> input = arrayIn.ToList();
            List<int> expected = new List<int>();
            expected.Add(16);
            expected.Add(45);
            expected.Add(73);

            input.Sort();
            List<int> result = StatsMath.AverageMode(input);

            if (result.Count != 3)
            {
                Assert.Fail();
            }
            int j = 0;
            foreach (int i in result)
            {
                if (i != expected[j])
                {
                    Assert.Fail();
                }
                j++;
            }
        }

        [TestMethod()]
        public void AverageRangeTest()
        {
            int[] arrayIn = { 10, 89, 16, 16, 24, 61, 88, 73, 16, 73, 51, 44, 32, 47, 45, 68, 45, 73, 29, 45, 77, 56, 18, 38 };
            List<int> input = arrayIn.ToList();

            double expected = 79.00;
            double result;

            input.Sort();
            result = StatsMath.AverageRange(input);

            Assert.AreEqual(expected, result);
        }

        [TestMethod()]
        public void AverageMidExtremeTest()
        {
            int[] arrayIn = { 10, 89, 16, 16, 24, 61, 88, 73, 16, 73, 51, 44, 32, 47, 45, 68, 45, 73, 29, 45, 77, 56, 18, 38 };
            List<int> input = arrayIn.ToList();
            double expected = 49.50;
            double result;

            input.Sort();
            result = StatsMath.AverageMidExtreme(input);

            Assert.AreEqual(expected, result);
        }
    }
}