using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TranslateApplication.Models.Translation
{
   public class LanguageListReceiver
   {
      public static IEnumerable<SelectListItem> GetSelectList(Type type)
      {
         var result = new List<SelectListItem>();
         foreach (var enumValue in type.GetEnumValues())
         {
            result.Add(new SelectListItem(enumValue.ToString(), ((int)enumValue).ToString()));
         }

         return result;
      }
   }
}
