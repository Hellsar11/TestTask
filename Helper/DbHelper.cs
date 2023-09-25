using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestTask.Models;

namespace TestTask.Helper
{
    public static class DbHelper
    {
        public static void UseContext(Action<TestTaskDBContext> action)
        {
            using(TestTaskDBContext context = new TestTaskDBContext())
            {
                action(context);
            }
        }

        public static T UseContext<T>(Func<TestTaskDBContext, T> function)
        {
            using (TestTaskDBContext context = new TestTaskDBContext())
            {
                return function(context);
            }
        }
    }
}
