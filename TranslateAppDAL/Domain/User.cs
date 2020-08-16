using System;
using System.ComponentModel.DataAnnotations;

namespace TranslateAppDAL.Domain
{
   public class User
   {
      [Required]
      public Guid Id { get; set; }

      [Required]
      [EmailAddress]
      [Display(Name = "E-mail")]
      public string Email { get; set; }

      [Required]
      [Display(Name = "Пароль")]
      [DataType(DataType.Password)]
      public string Password { get; set; }

      public int? RoleId { get; set; }

      public virtual Role Role { get; set; }
   }
}
