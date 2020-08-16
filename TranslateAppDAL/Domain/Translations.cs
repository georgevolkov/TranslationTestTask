using System.ComponentModel.DataAnnotations;

namespace TranslateAppDAL.Domain
{
   public class Translations
   {

      [Required]
      public int Id { get; set; }

      [Required]
      public string SourceWord { get; set; }

      [Required]
      public int SourceLanguageId { get; set; }

      public string Translation { get; set; }

      public int TranslationLanguageId { get; set; }
   }
}
