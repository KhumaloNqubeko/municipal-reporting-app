using System;
using System.Collections.Generic;
using System.Linq;

namespace Part3.Tests
{
    internal static class Check
    {
        internal static void True(bool condition, string message = "Assertion failed") { if (!condition) throw new Exception(message); }
        internal static void Equal<T>(T expected, T actual) { True(EqualityComparer<T>.Default.Equals(expected, actual), "Expected " + expected + ", got " + actual); }
        internal static void Sequence<T>(IEnumerable<T> expected, IEnumerable<T> actual) { True(expected.SequenceEqual(actual), "Sequences differ: " + string.Join(",", actual)); }
        internal static void Throws<T>(Action action) where T : Exception
        { try { action(); } catch (T) { return; } throw new Exception("Expected " + typeof(T).Name); }
    }
}
