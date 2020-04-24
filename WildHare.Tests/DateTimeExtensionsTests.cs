using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using WildHare.Extensions;
using WildHare.Tests.Models;

namespace WildHare.Tests
{
    [TestFixture]
    public class DateTimeExtensionsTests
    {
        [Test]
        public void DateTime_Next_DayOfWeek()
        {
            var date = DateTime.Parse("3/24/2020 12:15:07 AM");
            var nextSunday = date.NextDayOfWeek(DayOfWeek.Sunday).Date;         

            Assert.AreEqual(DateTime.Parse("3/29/2020"), nextSunday);
        }

        [Test]
        public void DateTime_Next_DayOfWeek_When_On_That_Day_And_IncludeCurrentDate_Is_true()
        {
            var date = DateTime.Parse("3/29/2020 12:15:07 AM");
            var nextSunday = date.NextDayOfWeek(DayOfWeek.Sunday, true).Date;

            Assert.AreEqual(DateTime.Parse("3/29/2020"), nextSunday);
        }

        [Test]
        public void DateTime_Next_DayOfWeek_When_On_That_Day_And_IncludeCurrentDate_Is_false()
        {
            var date = DateTime.Parse("3/29/2020 12:15:07 AM");
            var nextSunday = date.NextDayOfWeek(DayOfWeek.Sunday).Date;

            Assert.AreEqual(DateTime.Parse("4/05/2020"), nextSunday);
        }

        [Test]
        public void TodayOrBefore_Test()
        {
            var yesterday   = DateTime.Today.AddDays(-1);
            var today       = DateTime.Today;
            var tomorrow    = DateTime.Today.AddDays(1);
            var now         = DateTime.Now;

            Assert.IsTrue(yesterday.TodayOrBefore());
            Assert.IsTrue(today.TodayOrBefore());
            Assert.IsTrue(now.TodayOrBefore());
            Assert.IsFalse(tomorrow.TodayOrBefore());
        }
    }
}
