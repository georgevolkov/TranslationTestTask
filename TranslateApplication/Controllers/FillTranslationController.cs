using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TranslateAppDAL;
using TranslateAppDAL.Domain;
using TranslateAppDAL.Repositories.Interfaces;
using TranslateApplication.Models;
using TranslateApplication.Models.Enums;

namespace TranslateApplication.Controllers
{
   public class FillTranslationController : Controller
   {
      private readonly TranslateDbContext _dbContext;
      private readonly IMapper _mapper;
      private readonly ITranslationRepository _repository;
      private const int RecordsCount = 20;

      public FillTranslationController(TranslateDbContext dbContext, IMapper mapper, ITranslationRepository repository)
      {
         _dbContext = dbContext;
         _mapper = mapper;
         _repository = repository;
      }

      [Authorize(Roles = "admin")]
      public IActionResult Index()
      {
         return View(new FillTranslationModel());
      }

      [HttpPost]
      [Authorize(Roles = "admin")]
      public async Task<IActionResult> Index(FillTranslationModel model)
      {
         if(ModelState.IsValid)
         {
            if (model.SourceLanguageId == model.TranslationLanguageId)
            {
               ModelState.AddModelError("", "Исходный язык и язык для перевода должны отличаться");
               return View();
            }

            if (!model.IsEdit)
               await CreateTranslation(model);
            else
               await UpdateTranslation(model);

            ModelState.Clear();
         }
         else
         {
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            foreach (var error in errors)
            {
               ModelState.AddModelError("", error.ErrorMessage);
            }

            return View();
         }


         return RedirectToAction("GetTranslations", "FillTranslation");
      }

      private async Task UpdateTranslation(FillTranslationModel model)
      {
         var mappedTranslation = _mapper.Map<Translations>(model);
         await _repository.UpdateTranslation(mappedTranslation);
      }

      private async Task CreateTranslation(FillTranslationModel model)
      {
         var mappedTranslation = _mapper.Map<Translations>(model);
         await _repository.CreateTranslation(mappedTranslation);
      }

      [Authorize(Roles = "admin")]
      public async Task<IActionResult> Edit(int id)
      {
         var model = await _dbContext.Translations.FirstOrDefaultAsync(x => x.Id == id);
         var mappedTranslation = _mapper.Map<FillTranslationModel>(model);
         mappedTranslation.IsEdit = true;
         return View("Index", mappedTranslation);
      }

      [Authorize(Roles = "admin")]
      public async Task<IActionResult> Delete(int id)
      {
         await _repository.DeleteTranslation(id);
         return RedirectToAction("GetTranslations");
      }

      [Authorize(Roles = "admin")]
      public async Task<IActionResult> GetTranslations()
      {
         var translations = await _repository.GetAllTranslations().OrderBy(x => x.SourceWord).ToListAsync();
         var list = translations.Where(x => x.SourceLanguageId == (int)Languages.English).Take(RecordsCount)
            .Select(translation => _mapper.Map<FillTranslationModel>(translation)).ToList();

         return View(list);
      }

      [HttpPost]
      [Authorize(Roles = "admin")]
      public IActionResult GetData(int currentIndex, int elementsNumber)
      {
         var translations = _repository.GetAllTranslations().OrderBy(x => x.SourceWord).Where(x => x.SourceLanguageId == (int)Languages.English)
            .Skip(currentIndex * elementsNumber).Take(elementsNumber).ToList();
         var list = JsonConvert.SerializeObject(translations,
            Formatting.None,
            new JsonSerializerSettings
            {
               ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

         return Content(list, "application/json");
      }
   }
}
