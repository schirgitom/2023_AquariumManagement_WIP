using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.DBTest
{
    public class DBConnnectionTest : BaseUnitTests
    {
        [Test]
        public async Task TestDBConnection()
        { 
            UnitOfWork uow = new UnitOfWork();

            Assert.IsTrue(uow.Context.IsConnected);
        }
    }
}
