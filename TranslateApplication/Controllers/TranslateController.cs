using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TranslateAppDAL;
using TranslateApplication.Models;
using TranslateApplication.Models.Translation;

namespace TranslateApplication.Controllers
{
   public class TranslateController : Controller
   {
      private readonly TranslateWordProcessor _translateWordProcessor;
      private readonly TranslateDbContext _dbContext;
      private readonly IMapper _mapper;

      public TranslateController(TranslateWordProcessor translateWordProcessor,
         TranslateDbContext dbContext,
         IMapper mapper)
      {
         _translateWordProcessor = translateWordProcessor;
         _dbContext = dbContext;
         _mapper = mapper;
      }

      [Authorize(Roles = "admin, user")]
      public IActionResult Translate(string searchString, int sourceLanguage)
      {
         if (searchString == null)
            searchString = string.Empty;
         ViewBag.SearchString = searchString;
         var data = _dbContext.Translations
            .Where(x => x.SourceWord.StartsWith(searchString, StringComparison.OrdinalIgnoreCase) && x.SourceLanguageId == sourceLanguage).ToList();
         return View(_mapper.Map<List<TranslateModel>>(data));
      }
   }
}
