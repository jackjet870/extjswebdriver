﻿using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Linq;
using System.Threading;

namespace ExtjsWd.Elements
{
    public static class BaseContainerComponentExtentions
    {
        public static T ShouldDisplaySuccessNotification<T>(this T callee, string text) where T : BaseContainerComponent
        {
            callee.WaitUntil(x => callee.NotificationSuccess.Text.Contains(text));
            return callee;
        }

        public static T ShouldDisplayWarningNotification<T>(this T callee, string text) where T : BaseContainerComponent
        {
            callee.WaitUntil(x => callee.NotificationWarning.Text.Contains(text));
            return callee;
        }

        public static T ShouldHaveErrorTip<T>(this T callee, string containingText) where T : BaseContainerComponent
        {
            var errors = callee.NotificationErrorsAsText();
            var filtered = errors.Where(error => error.Contains(containingText));
            Assert.IsTrue(filtered.Any(), "Expected error tip with text containing '" + containingText + "' to be present! - displayed instead: [" + string.Join(",", errors) + "]");
            return callee;
        }

        public static T ShouldHaveNoErrorTip<T>(this T callee, string containingText) where T : BaseContainerComponent
        {
            var errors = callee.NotificationErrorsAsText();
            var filtered = errors.Where(error => error.Contains(containingText));
            Assert.IsFalse(filtered.Any(), "Expected no error tip with text containing '" + containingText + "' to be present! - displayed instead: [" + string.Join(",", errors) + "]");
            return callee;
        }

        public static T ShouldHaveNoErrorTip<T>(this T callee) where T : BaseContainerComponent
        {
            if (callee.NotificationError != null)
            {
                Console.WriteLine("-------------------------------------------------------------------------------------------------");
                Console.WriteLine(callee.NotificationError.Text);
            }
            Assert.IsNull(callee.NotificationError, "Expected no error notification to show!");
            return callee;
        }

        public static T ShouldHaveNoWarningNotification<T>(this T callee, string containingText) where T : BaseContainerComponent
        {
            var warnings = callee.NotificationWarningsAsText();
            var filtered = warnings.Where(warn => warn.Contains(containingText));
            Assert.IsFalse(filtered.Any(),
                "Expected no warning notification with text containing '" + containingText +
                "' to be present! - displayed instead: [" + string.Join(",", warnings) + "]");
            return callee;
        }

        public static T WaitForSomeMiliTime<T>(this T callingObject, int timeOutInMiliSeconds)
        {
            Thread.Sleep(timeOutInMiliSeconds);
            return callingObject;
        }

        public static T WaitForSomeTime<T>(this T callingObject, int timeOutInSeconds)
        {
            Thread.Sleep(timeOutInSeconds * 1000);
            return callingObject;
        }

        public static T WaitUntilAjaxLoadingDone<T>(this T callee) where T : BaseContainerComponent
        {
            callee.WaitUntil(30, x => callee.AjaxRequestsBusy == 0);
            return callee;
        }

        public static T WaitUntilExtLoadingDone<T>(this T callee) where T : BaseContainerComponent
        {
            callee.Wait(30).UntilNotDisplayed(By.CssSelector(".x-mask-msg"));
            return callee;
        }
    }
}