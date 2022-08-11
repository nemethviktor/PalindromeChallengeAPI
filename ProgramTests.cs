using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Program;

namespace Tests
{
    [TestClass()]
    public class ProgramTests
    {
        string stringToPass = "";
        string expected = "";
        string result = "";

        [TestMethod()]
        public void Test_AllLowerCase()
        {
            findPalindrome FindPalindrome = new();
            stringToPass = "abbaabba";
            expected = "Text: abbaabba, Index: 0, Length: 8" + Environment.NewLine;
            string result = findPalindrome.findPalindromeString(stringToPass);
            Assert.AreEqual(expected, result);
        }
        [TestMethod()]
        public void Test_MixedCase()
        {
            findPalindrome FindPalindrome = new();
            stringToPass = "abBA";
            expected = "No palindromes found.";
            string result = findPalindrome.findPalindromeString(stringToPass);
            Assert.AreEqual(expected, result);
        }
        [TestMethod()]
        public void Test_SpecialCharacters1()
        {
            findPalindrome FindPalindrome = new();
            stringToPass = "abba!!abba";
            expected = "Text: abba!!abba, Index: 0, Length: 10" + Environment.NewLine;
            string result = findPalindrome.findPalindromeString(stringToPass);
            Assert.AreEqual(expected, result);
        }
        [TestMethod()]
        public void Test_SpecialCharacters2()
        {
            findPalindrome FindPalindrome = new();
            stringToPass = "abba##abba";
            expected = "Text: abba##abba, Index: 0, Length: 10" + Environment.NewLine;
            string result = findPalindrome.findPalindromeString(stringToPass);
            Assert.AreEqual(expected, result);
        }
        [TestMethod()]
        public void Test_SpecialCharacters3()
        {
            findPalindrome FindPalindrome = new();
            stringToPass = "丽丽丽丽";
            expected = "Text: 丽丽丽丽, Index: 0, Length: 4" + Environment.NewLine;
            string result = findPalindrome.findPalindromeString(stringToPass);
            Assert.AreEqual(expected, result);
        }
        [TestMethod()]
        public void Test_sqrrqabccbatudefggfedvwhijkllkjihxymnnmzpop()
        {
            findPalindrome FindPalindrome = new();
            stringToPass = "sqrrqabccbatudefggfedvwhijkllkjihxymnnmzpop";
            expected = "Text: hijkllkjih, Index: 23, Length: 10" + Environment.NewLine +
                "Text: defggfed, Index: 13, Length: 8" + Environment.NewLine +
                "Text: abccba, Index: 5, Length: 6" + Environment.NewLine;
            string result = findPalindrome.findPalindromeString(stringToPass);
            Assert.AreEqual(expected, result);
        }
    }
}