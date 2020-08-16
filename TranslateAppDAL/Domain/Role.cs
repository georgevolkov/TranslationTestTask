using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TranslateAppDAL.Domain
{
   public class Role
   {
      [Required]
      public int Id { get; set; }

      [Required]
      [Display(Name = "Наименование роли")]
      public string Name { get; set; }

      public List<User> Users { get; set; }

      public Role()
      {
         Users = new List<User>();
      }
   }
}
