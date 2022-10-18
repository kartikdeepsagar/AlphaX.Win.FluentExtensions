using System;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace AlphaX.Win.FluentExtensions
{
    public static class ControlExtensions
    {
        /// <summary>
        /// Removes an event handler from a control's event.
        /// </summary>
        /// <param name="control"></param>
        /// <param name="eventName"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public static void RemoveEventHandler(this Control control, string eventName)
        {
            FieldInfo eventField = typeof(Control).GetField(ToEventFieldName(eventName),
                BindingFlags.Static | BindingFlags.NonPublic);

            if (eventField == null)
                throw new InvalidOperationException($"Event not found with name {eventName}");

            EventHandlerList list = control.GetEventHandlerList();
            object eventObject = eventField.GetValue(control);
            list.RemoveHandler(eventObject, list[eventObject]);
        }

        /// <summary>
        /// Gets whether the event has any event handler or not.
        /// </summary>
        /// <param name="control"></param>
        /// <param name="eventName"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static bool HasEventHandler(this Control control, string eventName)
        {
            FieldInfo eventField = typeof(Control).GetField(ToEventFieldName(eventName),
                BindingFlags.Static | BindingFlags.NonPublic);

            if (eventField == null)
                throw new InvalidOperationException($"Event not found with name {eventName}");

            EventHandlerList list = control.GetEventHandlerList();
            object eventObject = eventField.GetValue(control);
            var handler = list[eventObject];
            return handler != null;
        }

        /// <summary>
        /// Gets the framework specific field name.
        /// </summary>
        /// <param name="eventName"></param>
        /// <returns></returns>
        private static string ToEventFieldName(string eventName)
        {
#if NET6_0 || NETCOREAPP3_1
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("s_");
            stringBuilder.Append(eventName.Substring(0, 1).ToLower());
            stringBuilder.Append(eventName.Substring(1));
            stringBuilder.Append("Event");
            return stringBuilder.ToString();
#endif

#if NETFRAMEWORK
            return $"Event{eventName}";
#endif
        }

        /// <summary>
        /// Gets the event handler list of a control.
        /// </summary>
        /// <param name="control"></param>
        /// <returns></returns>
        private static EventHandlerList GetEventHandlerList(this Control control) 
        {
            PropertyInfo propertyInfo = control.GetType().GetProperty("Events", BindingFlags.NonPublic | BindingFlags.Instance);
            EventHandlerList list = (EventHandlerList)propertyInfo.GetValue(control, null);
            return list;
        }
    }
}