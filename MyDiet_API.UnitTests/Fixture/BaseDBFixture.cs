using System;
using System.Collections.Generic;

namespace MyDiet_API.UnitTests.Fixture
{
    public class BaseDBFixture<T> : IDisposable where T : class
    {
        public List<T> Items { get; private set; }

        public BaseDBFixture()
        {
            Items = new List<T>();
        }

        public void Dispose()
        {
            Items.Clear();
        }
    }
}
