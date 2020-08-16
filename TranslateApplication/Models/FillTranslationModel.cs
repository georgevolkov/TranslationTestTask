using System.ComponentModel.DataAnnotations;

namespace TranslateApplication.Models
{
   public class FillTranslationModel
   {
      [Required]
      public int Id { get; set; }

      [Required(ErrorMessage = "Не указано исходное слово")]
      public string SourceWord { get; set; }
      [Required(ErrorMessage = "Не указан язык исходного слова")]
      public int SourceLanguageId { get; set; }
      [Required(ErrorMessage = "Не указано слово перевода")]
      public string Translation { get; set; }
      [Required(ErrorMessage = "Не указан язык для слова перевода")]
      public int TranslationLanguageId { get; set; }

      public bool IsEdit { get; set; }
   }
}
