using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DDA.Web.Helpers
{
    public class UTCDateTimeModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            // Check if the DateTime property being parsed is not null or "" (for JSONO
            if (value.AttemptedValue != null && value.AttemptedValue != "")
            {
                // Parse the datetime
                var dt = DateTime.ParseExact(value.AttemptedValue, new string[] { "MM/dd/yyyy", "dd/MM/yyyy", "dd/MM/yy" }, CultureInfo.InvariantCulture, DateTimeStyles.None);
                return dt;
            }
            else
            {
                return null;
            }
        }
    }
}