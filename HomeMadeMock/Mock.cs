using System.Collections.Generic;
using System;
using System.Dynamic;
using ImpromptuInterface;

namespace HomeMadeMock
{
    public class Mock<T> : DynamicObject where T : class
    {
        private bool _isInSetup;

        private Dictionary<string, object> _mappings;

        private string _currentMethodName;

        public T Object => this.ActLike<T>();

        public Mock()
        {
            _isInSetup = false;
            _mappings = new Dictionary<string, object>();
        }

        public static Mock<T> GetMock()
        {
            var mock = new Mock<T>();
            return mock;
        }

        public void SetMapping(string memberName, object value) {
            _mappings[memberName] = value;
        }

        public void Setup(Action<T> callback)
        {
            _isInSetup = true;
            callback(Object);
            _isInSetup = false;
        }

        public MockReturn<T, TMethodResult> Setup<TMethodResult>(Func<T, TMethodResult> callback)
        {
            Setup((o) => { callback(o); }); // call "base" function
            var result = new MockReturn<T, TMethodResult>(this, _currentMethodName);
            return result;
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            var methodInfo = typeof(T).GetMethod(binder.Name);

            if (_isInSetup)
            {
                _currentMethodName = methodInfo.Name;
            }

            var isVoid = methodInfo.ReturnType == typeof(void);
            if (isVoid)
            {
                base.TryInvokeMember(binder, args, out result);
            }
            else
            {
                result = _mappings.ContainsKey(methodInfo.Name) ?
                    _mappings[methodInfo.Name] : (methodInfo.ReturnType.IsValueType ?
                        Activator.CreateInstance(methodInfo.ReturnType) :
                        null);
            }

            return true;
        }
    }
}