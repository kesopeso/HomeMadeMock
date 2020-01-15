using System;

namespace HomeMadeMock
{
    class Program
    {
        static void Main(string[] args)
        {
            var mock = Mock<ITester>.GetMock();

            // comment out the next two following lines to run mocks without setup
            mock.Setup((tester) => tester.TestingVoid());
            System.Console.WriteLine("TestingVoid() setup called.");

            mock.Setup((tester) => tester.TestingInt()).Returns(() => 10);
            System.Console.WriteLine("TestingInt() setup called.");

            mock.Object.TestingVoid();
            System.Console.WriteLine("TestingVoid() mock ran successfully.");

            var someInt = mock.Object.TestingInt();
            System.Console.WriteLine($"TestingInt() mock ran successfully. Result: {someInt}");
        }
    }
}
