using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bench.Observable;


namespace Bench.Test
{
    [Observable]
    public partial class HelloSGTest
    {
        public HelloSGTest()
        {
            Test = 2;
            Console.WriteLine("nothing");
        }

        int _Test;
    }
}
