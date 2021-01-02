using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using WildHare.Extensions;
using WildHare.Tests.Models;

namespace WildHare.Tests
{
    [TestFixture]
    public class DateTimeExtensionsTests
    {
        [Test]
        public void NextDayOfWeek_Test()
        {
            var date = DateTime.Parse("3/24/2020 12:15:07 AM");
            var nextSunday = date.NextDayOfWeek(DayOfWeek.Sunday).Date;         

            Assert.AreEqual(DateTime.Parse("3/29/2020"), nextSunday);
        }

        [Test]
        public void NextDayOfWeek_When_On_That_Day_And_IncludeCurrentDate_Is_true()
        {
            var date = DateTime.Parse("3/29/2020 12:15:07 AM");
            var nextSunday = date.NextDayOfWeek(DayOfWeek.Sunday, true).Date;

            Assert.AreEqual(DateTime.Parse("3/29/2020"), nextSunday);
        }

        [Test]
        public void NextDayOfWeek_When_On_That_Day_And_IncludeCurrentDate_Is_false()
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

        [Test]
        public void MonthName_Test()
        {
            // Test will when CurrentCulture is en-US (or similar culture)
            string currentCulture = CultureInfo.CurrentCulture.DisplayName; // en-US

            int monthOfJulyInt = 7;
            var date = new DateTime(2020, monthOfJulyInt, 1);

            Assert.AreEqual("July", monthOfJulyInt.MonthName());
            Assert.AreEqual("July", date.MonthName());
        }

        [Test]
        public void ShortMonthName_Test()
        {
            // Test will when CurrentCulture is en-US (or similar culture)
            string currentCulture = CultureInfo.CurrentCulture.DisplayName; // en-US

            int monthOfJulyInt = 7;
            var date = new DateTime(2020, monthOfJulyInt, 1);

            Assert.AreEqual("Jul", monthOfJulyInt.ShortMonthName());
            Assert.AreEqual("Jul", date.ShortMonthName());
        }

        [Test]
        public void YearMonth_Test()
        {
            var december_1_2020 = new DateTime(2020, 12, 1);

            Assert.AreEqual("202012", december_1_2020.YearMonth());

            var april_5_2020 = new DateTime(2020, 4, 5);

            Assert.AreEqual("202004", april_5_2020.YearMonth());
        }

        [Test]
        public void YearMonthDay_Test()
        {
            var december_1_2020 = new DateTime(2020, 12, 1);

            Assert.AreEqual("20201201", december_1_2020.YearMonthDay());

            var april_5_2020 = new DateTime(2020, 4, 5);

            Assert.AreEqual("20200405", april_5_2020.YearMonthDay());
        }
    }
}
