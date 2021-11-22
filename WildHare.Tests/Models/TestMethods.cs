using System;
using System.Collections.Generic;
using System.Text;

namespace WildHare.Tests.Models
{
    public class TestMethods
    {
        private int _first;
        private int _second;

        public TestMethods(int first, int second)
        {
            _first = first;
            _second = second;
        }
        
        public int Add() => _first + _second;

        public int Subtract() => _first - _second;

        public int Multiply(int first, int second) => first *_second;

        public int Divide(int first, int second) => first / second;

    }
}
