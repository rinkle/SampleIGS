using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;

namespace IGS.Web.Controllers
{
    public class BaseController : Controller
    {
        private readonly int PAGE_SIZE = 10;

        public int PageSize => PAGE_SIZE;

        #region Notifications
        protected void AddNotification(NotifyType type, string message, bool persistForNextRequest = true)
        {
            string key = type.ToString();

            if (persistForNextRequest)
            {
                var messages = TempData.Get<List<string>>(key) ?? new List<string>();
                messages.Add(message);
                TempData.Put(key, messages);
            }
            else
            {
                var messages = ViewData[key] as List<string> ?? new List<string>();
                messages.Add(message);
                ViewData[key] = messages;
            }
        }

        protected void SuccessNotification(string message, bool persistForNextRequest = true) =>
            AddNotification(NotifyType.Success, message, persistForNextRequest);

        protected void InfoNotification(string message, bool persistForNextRequest = true) =>
            AddNotification(NotifyType.Info, message, persistForNextRequest);

        protected void ErrorNotification(string message, bool persistForNextRequest = true) =>
            AddNotification(NotifyType.Error, message, persistForNextRequest);

        protected void ErrorNotification(ModelStateDictionary modelState, bool persistForNextRequest = true)
        {
            foreach (var state in modelState.Values)
            {
                foreach (var error in state.Errors)
                {
                    AddNotification(NotifyType.Error, error.ErrorMessage, persistForNextRequest);
                }
            }
        }
        #endregion

        public enum NotifyType
        {
            Success,
            Error,
            Info
        }
    }
}
