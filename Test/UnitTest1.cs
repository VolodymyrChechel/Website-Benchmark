using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebsiteBenchmark.Util;

namespace Test
{
    [TestClass]
    public class SitemapCreatorTest
    {
        private SitemapCreator _sitemapCreator;
        private string _url = "https://rozetka.com.ua/";

        public SitemapCreatorTest()
        {
            _sitemapCreator = new SitemapCreator(_url);
        }

        [TestMethod]
        public void TestMethod1()
        {
            
        }
    }
}
