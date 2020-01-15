using System;

namespace HomeMadeMock
{
    public class MockReturn<TMock, TMethodResult> where TMock : class
    {
        private readonly Mock<TMock> _mock;

        private readonly string _memberName;

        public MockReturn(Mock<TMock> mock, string memberName)
        {
            _mock = mock;
            _memberName = memberName;
        }

        public void Returns(Func<TMethodResult> callback)
        {
            _mock.SetMapping(_memberName, callback());
        }
    }
}