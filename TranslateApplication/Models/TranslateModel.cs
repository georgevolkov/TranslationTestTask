using TranslateApplication.Models.Enums;

namespace TranslateApplication.Models
{
   public class TranslateModel
   {
      public TranslateModel()
      {
         SourceWord = string.Empty;
         DestinationWord = string.Empty;
      }

      public Languages SourceLanguage { get; set; }
      public string SourceWord { get; set; }
      public string DestinationWord { get; set; }

   }
}
