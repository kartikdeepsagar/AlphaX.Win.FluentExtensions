using NUnit.Framework;
using System;
using System.Windows.Forms;

namespace AlphaX.Win.FluentExtensions.Tests
{
    public class ControlExtensionsTests
    {
        [Test]
        public void RemoveEventHandlerTest_Success()
        {
            Button control = new Button();
            control.Click += (s, e) => { };
            control.RemoveEventHandler("Click");
            Assert.IsFalse(control.HasEventHandler("Click"));
        }

        [Test]
        public void RemoveEventHandlerTest_Failure()
        {
            Button control = new Button();
            Assert.Throws(typeof(InvalidOperationException), () => control.RemoveEventHandler("Clicsk"));
        }

        [Test]
        public void HasEventHandlerTest_Success()
        {
            Button control = new Button();
            control.Click += (s, e) => { };
            var result = control.HasEventHandler("Click");
            Assert.IsTrue(result);
        }

        [Test]
        public void HasEventHandlerTest_Failure()
        {
            Button control = new Button();
            Assert.Throws(typeof(InvalidOperationException), () => control.HasEventHandler("Clicsk"));
        }
    }
}