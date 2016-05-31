using MyOnlineClinic.Entity;
using MyOnlineClinic.RepositoryService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyOnlineClinic.Web.Models;
using CaptchaMvc.HtmlHelpers;
using PagedList;
using System.Text;
using System.Web.Script.Serialization;
using System.Net;
using MyOnlineClinic.Web.Helper;
using System.Configuration;
using MyOnlineClinic.Emailer;
using System.Security.Cryptography;
using System.Web.Security;

namespace MyOnlineClinic.Web.Controllers
{
    public class HomeController : BaseController
    {
        protected IFormsAuthentication _formsAuth;

        private readonly IStateService _stateService;
        private readonly ICityService _cityService;
        private readonly IClinicService _clinicService;
        IUserService _userService;
        private readonly IAdminUserService _adminUserService;
        private readonly IProfessionTypeService _professionType;
        private readonly ICommunication _Communication;
        protected readonly IProfessionTypeService _professiontypeservice;
        private readonly IDoctorService _doctorservice;
        private readonly IDoctorMembershipService _doctormembershipService;
        private readonly ICountryService _countryService;
        #region Comment
        ///// <summary>
        ///// news service object created
        ///// </summary>
        //protected INewsService _newsService;

        ///// <summary>
        ///// News photo service object is created
        ///// </summary>
        //protected INewsPhotoService _newsPhotoService;

        ///// <summary>
        ///// Banner service object is created
        ///// </summary>
        //protected IBannerService _bannerService;

        ///// <summary>
        ///// Page cms service object is created
        ///// </summary>
        //protected IPageCMSService _pageCmsService;

        ///// <summary>
        ///// Activities service object is created
        ///// </summary>
        //protected IActivitiesSectorService _activitiesSectorServices;

        ///// <summary>
        ///// Classified service object is created
        ///// </summary>
        //protected IClassifiedService _classifiedServices;

        ///// <summary>
        ///// state service object is created
        ///// </summary>
        //protected IStateService _stateService;

        ///// <summary>
        ///// Category service object is created
        ///// </summary>
        //protected ICategoryService _categoryService;

        ///// <summary>
        ///// Service repository service is created
        ///// </summary>
        //protected IServiceRepoService _serviceRepoService;

        ///// <summary>
        ///// Event service object created
        ///// </summary>
        //protected IEventService _eventService;

        ///// <summary>
        ///// Country service object created
        ///// </summary>
        //protected ICountryService _countryService;

        ///// <summary>
        ///// Provinence service object is created
        ///// </summary>
        //protected IProvinceService _provinceService;

        ///// <summary>
        ///// City service object is created
        ///// </summary>
        //protected ICityService _cityService;

        ///// <summary>
        ///// Subcategory service object is created
        ///// </summary>
        //protected ISubCategoryService _subcategoryService;

        ///// <summary>
        ///// Receipe service object is created
        ///// </summary>
        //protected IRecipesServices _recipesServices;

        ///// <summary>
        ///// Event comment service object is created
        ///// </summary>
        //protected IEventCommentsService _commentService;

        ///// <summary>
        ///// Provided service object is created
        ///// </summary>
        //protected IProvidedServicesService _providedServices;

        ///// <summary>
        ///// Faq service object is created
        ///// </summary>
        //protected IFAQSectionService _faqService;

        ///// <summary>
        ///// Empresas service object is created
        ///// </summary>
        //protected IRepositoryService<LookUpCompany> _companyService;

        ///// <summary>
        ///// Social media service object is created
        ///// </summary>
        //protected ISocialMediaService _socialMediaService;

        ///// <summary>
        ///// Mercado service object is created
        ///// </summary>
        //protected IMercadoService _mercadoService;

        ///// <summary>
        ///// Event subcategory service object is created
        ///// </summary>
        //private IEventSubCategoryService _eventSubCategory;

        ///// <summary>
        ///// Tipologia Service object is created
        ///// </summary>
        //protected ITipologiaService _tipologiaService;

        ///// <summary>
        ///// Natureza Service object is created
        ///// </summary>
        //protected INaturezaService _naturezaService;

        ///// <summary>
        ///// News category service object is created
        ///// </summary>
        //protected INewCategoryService _newsCategoryService;

        ///// <summary>
        ///// General comment service object is created
        ///// </summary>
        //protected IcommentService _generalCommentService;

        ///// <summary>
        ///// Sell condition service object created
        ///// </summary>
        //protected ISellConditionService _sellConditionService;

        ///// <summary>
        ///// Condition service object created
        ///// </summary>
        //protected IConditionService _conditionService;

        ///// <summary>
        ///// Business service object created
        ///// </summary>
        //protected IBusinessService _businessService;

        ///// <summary>
        ///// Make service object is created
        ///// </summary>
        //protected IMakeService _makeService;

        //// We have used the dependancy injection in this code and design patters so we are passing the object of interface
        //// to the controller constructor.

        ///// <summary>
        ///// Home controller constructor is created to pass objects of the services.
        ///// </summary>
        ///// <param name="newsService">newsService</param>
        ///// <param name="newsPhotoService">newsPhotoService</param>
        ///// <param name="bannerService">bannerService</param>
        ///// <param name="pageCmsService">pageCmsService</param>
        ///// <param name="activitiesSectorServices">activitiesSectorServices</param>
        ///// <param name="classifiedServices">classifiedServices</param>
        ///// <param name="categoryService">categoryService</param>
        ///// <param name="stateService">stateService</param>
        ///// <param name="serviceRepoService">serviceRepoService</param>
        ///// <param name="eventServices">eventServices</param>
        ///// <param name="countryService">countryService</param>
        ///// <param name="provinceService">provinceService</param>
        ///// <param name="cityService">cityService</param>
        ///// <param name="subcategoryService">subcategoryService</param>
        ///// <param name="recipesServices">recipesServices</param>
        ///// <param name="commentService">commentService</param>
        ///// <param name="providedServices">providedServices</param>
        ///// <param name="faqService">faqService</param>
        ///// <param name="companyService">companyService</param>
        ///// <param name="socialMediaService">providedServices</param>
        ///// <param name="mercadoService">mercadoService</param>
        ///// <param name="eventSubCategory">eventSubCategory</param>
        ///// <param name="newsCategoryService">newsCategoryService</param>
        ///// <param name="tipologiaService">tipologiaService</param>
        ///// <param name="naturezaService">naturezaService</param>
        ///// <param name="generalCommentService">generalCommentService</param>
        ///// <param name="sellConditionService">sellConditionService</param>
        ///// <param name="conditionService">conditionService</param>
        ///// <param name="businessService">businessService</param>
        ///// <param name="makeService">makeService</param>
        //public HomeController(INewsService newsService, INewsPhotoService newsPhotoService, IBannerService bannerService,
        //    IPageCMSService pageCmsService, IActivitiesSectorService activitiesSectorServices, IClassifiedService classifiedServices,
        //    ICategoryService categoryService, IStateService stateService, IServiceRepoService serviceRepoService, IEventService eventServices,
        //    ICountryService countryService, IProvinceService provinceService, ICityService cityService, ISubCategoryService subcategoryService,
        //    IRecipesServices recipesServices, IEventCommentsService commentService, IProvidedServicesService providedServices,
        //    IFAQSectionService faqService, IRepositoryService<LookUpCompany> companyService, ISocialMediaService socialMediaService,
        //    IMercadoService mercadoService, IEventSubCategoryService eventSubCategory, INewCategoryService newsCategoryService,
        //    ITipologiaService tipologiaService, INaturezaService naturezaService, IcommentService generalCommentService,
        //    ISellConditionService sellConditionService, IConditionService conditionService, IBusinessService businessService,
        //    IMakeService makeService
        //    )
        //{
        //    _newsService = newsService;
        //    _newsPhotoService = newsPhotoService;
        //    _bannerService = bannerService;
        //    _pageCmsService = pageCmsService;
        //    _activitiesSectorServices = activitiesSectorServices;
        //    _classifiedServices = classifiedServices;
        //    _categoryService = categoryService;
        //    _stateService = stateService;
        //    _serviceRepoService = serviceRepoService;
        //    _eventService = eventServices;
        //    _countryService = countryService;
        //    _provinceService = provinceService;
        //    _cityService = cityService;
        //    _subcategoryService = subcategoryService;
        //    _recipesServices = recipesServices;
        //    _commentService = commentService;
        //    _providedServices = providedServices;
        //    _faqService = faqService;
        //    _companyService = companyService;
        //    _socialMediaService = socialMediaService;
        //    _mercadoService = mercadoService;
        //    _eventSubCategory = eventSubCategory;
        //    _newsCategoryService = newsCategoryService;
        //    _tipologiaService = tipologiaService;
        //    _naturezaService = naturezaService;
        //    _generalCommentService = generalCommentService;
        //    _sellConditionService = sellConditionService;
        //    _conditionService = conditionService;
        //    _businessService = businessService;
        //    _makeService = makeService;
        //}

        /// <summary>
        /// Viajar Action to fetch the news related to Viajar subCategory 
        /// </summary>
        /// <param name="page">page</param>
        /// <param name="pageSize">pageSize</param>
        /// <param name="subCategory">subCategory</param>
        /// <returns>View</returns>
        //[Route("Viajar")]
        //public ActionResult Viajar(int? page, int? pageSize, string subCategory)
        //{
        //    //Assigning the subcategory name in the view bag
        //    ViewBag.SubCategory = subCategory;

        //    //News list object created
        //    var newsList = new List<Entity.News>();

        //    //Getting news category id
        //    var newsCategoryId = _newsCategoryService.GetNewCategoriesList().FirstOrDefault(x => x.LookUpNewsCategoryName == "Viajar").LookUpNewsCategoryId;

        //    //Getting all the news subcategory
        //    var subCategoryList = _newsCategoryService.GetNewsSubCategoriesListByCategoryId(newsCategoryId);

        //    //Assigning the subcategory list in view bag.
        //    ViewBag.SubCategories = subCategoryList;

        //    //Filtering news by subcategory
        //    if (!string.IsNullOrEmpty(subCategory))
        //    {
        //        newsList = _newsService.GetQueryableNews().FilterBySubCategory(subCategory).Where(x=>x.Approved && x.IsActive).OrderByDescending(x => x.DateFrom).ToList();
        //    }
        //    else
        //    { newsList = _newsService.GetQueryableNews().FilterByCategory("Viajar").Where(x=>x.IsActive && x.Approved).OrderByDescending(x => x.DateFrom).ToList(); }

        //    int pageNumber = 1;
        //    int newPageSize = pageSize ?? 10;
        //    return View(newsList.ToPagedList(pageNumber, newPageSize));
        //}

        //[Route("Viajar/{id}")]
        //public ActionResult ViajarDetails(string id)
        //{
        //    ViewBag.Msg = TempData["SuccessMsg"];
        //    NewsViewModel model = new NewsViewModel();
        //    string title = id;
        //    var news = _newsService.GetNewsByTitleUrl(id);
        //    if (news == null) return View(model);

        //    news.Viewed = news.Viewed + 1;
        //    _newsService.Update(news);

        //    model.Photo = news.NewsPhotos.FirstOrDefault();
        //    model.Detail = news.Detail;
        //    model.Title = news.Title;
        //    model.TitleUrl = news.TitleUrl;
        //    model.Tags = news.Tags;
        //    model.NewsId = news.NewsId;
        //    model.PhotosList = news.NewsPhotos.ToList();
        //    model.DateFrom = news.DateFrom;
        //    model.CategoryName = "Viajar";
        //    model.CommentsList = _generalCommentService.GetAllCommentsBySectionId(news.NewsId, "News");
        //    if (news.LoginId != Guid.Empty)
        //    {
        //        var loginId = new Guid(news.LoginId.ToString());
        //        var user = _userService.Find(loginId);
        //        if (user != null)
        //        {
        //            model.UserName = user.LoginName;
        //            model.Avatar = user.Avatar;
        //        }
        //    }
        //    var fregusia = _cityService.GetCityList().FirstOrDefault(x => x.LookUpCityId == news.FId);
        //    if (fregusia != null)
        //    {
        //        model.FregusiaName = fregusia.LookUpCityName;
        //    }
        //    var country = _countryService.GetActiveCountiesForFooter().ToList().FirstOrDefault(x => x.CountryId == news.LocationCountry);
        //    if (country != null)
        //    {
        //        model.CountryName = country.CountryName;
        //    }
        //    var distrito = _stateService.GetStateById(Convert.ToInt32(news.StateId));
        //    if (distrito != null)
        //    {
        //        model.DistritoName = distrito.LookUpStateName;
        //    }

        //    var concelho = _provinceService.GetProvinceList().FirstOrDefault(x => x.LookUpProvinceId == news.CId);
        //    if (concelho != null)
        //    {
        //        model.ConcelhoName = concelho.LookUpProvinceName;
        //    }
        //    return View("NewsDetails", model);
        //}


        /// <summary>
        /// Index To get home page contents
        /// </summary>
        /// <returns></returns>


        ///// <summary>
        ///// Faq to get faq contents
        ///// </summary>
        ///// <returns></returns>
        //public ActionResult Faq()
        //{
        //    var modelFaq = new FaqViewModel
        //    {
        //        FaqSectionList = _faqService.GetListFaqSection(),
        //        FaqSubSectionList = _faqService.GetSubSectionList()
        //    };
        //    return View(modelFaq);
        //}

        /// <summary>
        /// Search to implement google search
        /// </summary>
        /// <returns>View with results</returns>
        [Route("Pesquisa")]
        public ActionResult Search()
        {
            return View();
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        ///// <summary>
        ///// Getting home page news list
        ///// </summary>
        ///// <returns>Partial view</returns>
        //public ActionResult HomeNewsSlide()
        //{
        //    var list = _newsService.GetPhotosForSlider();
        //    return PartialView("PartialViewNewsSlide", list);
        //}

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        /// <summary>
        /// Getting active countries for footer section
        /// </summary>
        /// <returns></returns>
        //public ActionResult GetCountriesforFooter()
        //{
        //    var countries = _countryService.GetActiveCountiesForFooter();
        //    return PartialView("_Countries", countries);
        //}

        /// <summary>
        /// Getting featured companies
        /// </summary>
        /// <returns></returns>
        //public ActionResult GetFeaturedCompanies()
        //{
        //    if (TempData["country"] != null)
        //    {
        //        var companyList = _companyService.GetAll().Where(x => x.IsFeatured && x.IsActive).FilterbyCountry(TempData["country"].ToString()).Take(10).ToList();
        //        return View("_FeaturedCompanies", companyList);
        //    }
        //    else
        //    {
        //        var companyList = _companyService.GetAll().Where(x => x.IsFeatured && x.IsActive).Take(10).ToList();
        //        return View("_FeaturedCompanies", companyList);
        //    }

        //}

        ///// <summary>
        ///// Setting the mercado clicks
        ///// </summary>
        ///// <param name="Id">mercado id</param>
        ///// <returns></returns>
        //[HttpGet]
        //public JsonResult MercadoClick(string Id)
        //{
        //    if (!string.IsNullOrEmpty(Id))
        //    {
        //        var mercadoId = Convert.ToInt32(Id);
        //        var marcado = _mercadoService.GetMercadoList().FirstOrDefault(x => x.MercadoId == mercadoId);
        //        if (marcado != null)
        //        {
        //            marcado.Click = marcado.Click + 1;
        //            _mercadoService.Update(marcado);
        //        }
        //    }
        //    return Json("true", JsonRequestBehavior.AllowGet);
        //}

        //public ActionResult TopNews(string keyWord)
        //{
        //    ViewBag.Name = keyWord;
        //    var list = _newsService.GetNewsByKeywordForHome(keyWord).Where(x => x.Approved && x.IsActive).OrderByDescending(x => x.DateFrom).Take(1).ToList();
        //    return PartialView("_TopNews", list);
        //}

        //public ActionResult TopNewsForEmpressa(string keyWord)
        //{
        //    ViewBag.Name = keyWord;
        //    var list = _newsService.GetNewsByKeyword(keyWord).Take(1).ToList();
        //    return PartialView("TopNews", list);
        //}

        //public ActionResult Services()
        //{
        //    var list = _providedServices.GetProvidedServiceList().Where(x => x.IsActive == true);
        //    return PartialView("_Service", list);
        //}


        //public ActionResult SocialMedia()
        //{
        //    var list = _socialMediaService.GetSocialMedia().Where(x => x.IsActive == true);
        //    return PartialView("_socialMedia", list);
        //}

        //#region Classifieds
        //public ActionResult Classified(FilterParam filterParam)
        //{

        //    filterParam.page = filterParam.page == 0 ? 1 : filterParam.page;
        //    filterParam.pageSize = filterParam.pageSize == 0 ? 12 : filterParam.pageSize;
        //    IEnumerable<ClassifiedCreateModel> listNewsViewModel = null;
        //    var list = _classifiedServices.GetClassifiedsByKeyword(filterParam.q).OrderByDescending(x=>x.PostedOn);
        //    var query = _classifiedServices.GetQueryableClassifieds();
        //    if (!string.IsNullOrEmpty(filterParam.q))
        //    {
        //        query = query.FilterByKeyword(filterParam.q).OrderByDescending(x => x.PostedOn);
        //    }
        //    var facetResult = new FacetResult();


        //    if (!string.IsNullOrEmpty(filterParam.scat))
        //    {
        //        filterParam.cat = _subcategoryService.GetAll().FirstOrDefault(x => x.LookUpSubCategoryTitle == filterParam.scat).LookUpCategory.LookUpCategoryTitle;
        //        query = query.FilterBySubCategory(filterParam.scat).OrderByDescending(x => x.PostedOn); 
        //    }
        //    List<FilterObject> scatResult = null;
        //    if (!string.IsNullOrEmpty(filterParam.cat))
        //    {
        //        scatResult = list.GroupBy(g => g.SubCategory)
        //                                         .Select(g =>
        //                                             new FilterObject
        //                                             {
        //                                                 Id = g.Key.LookupSubCategoryId,
        //                                                 Title = g.Key.LookUpSubCategoryTitle,
        //                                                 Count = g.Count()
        //                                             }).ToList();
        //        query = query.FilterByCategory(filterParam.cat).OrderByDescending(x => x.PostedOn); 
        //    }
        //    facetResult.CategoryFilter = list.GroupBy(g => g.SubCategory.LookUpCategory)
        //                                    .Select(g => new FilterObject
        //                                    {
        //                                        Id = g.Key.LookUpCategoryId,
        //                                        Title = g.Key.LookUpCategoryTitle,
        //                                        Count = g.Count()
        //                                    }).ToList();
        //    if (scatResult != null)
        //    {
        //        facetResult.CategoryFilter.FirstOrDefault(x => x.Title == filterParam.cat).Children = scatResult;
        //    }



        //    if (!string.IsNullOrEmpty(filterParam.fregusia))
        //    {
        //        filterParam.concelho = _provinceService.GetProvinceList().FirstOrDefault(x => x.LookUpCities.Any(y => y.LookUpCityName == filterParam.fregusia)).LookUpProvinceName;
        //        query = query.FilterbyFergrusia(filterParam.fregusia);
        //    }


        //    List<FilterObject> cityResult = null;
        //    if (!string.IsNullOrEmpty(filterParam.concelho))
        //    {
        //        cityResult = list.Join(
        //                                 _cityService.GetCityList(),
        //                                 news => news.FId, city => city.LookUpCityId,
        //                                 (news, city) => new { News = news, City = city }
        //                               )
        //                               .GroupBy(g => g.City)
        //                                .Select(g => new FilterObject
        //                                {
        //                                    Id = g.Key.LookUpCityId,
        //                                    Title = g.Key.LookUpCityName,
        //                                    Count = g.Count()
        //                                }).ToList();
        //        filterParam.distrtito = _stateService.GetStateList().FirstOrDefault(x => x.Provinces.Any(y => y.LookUpProvinceName == filterParam.concelho)).LookUpStateName;
        //        query = query.FilterbyConcelho(filterParam.concelho);
        //    }


        //    List<FilterObject> provinceResult = null;
        //    if (!string.IsNullOrEmpty(filterParam.distrtito))
        //    {
        //        provinceResult = list.Join(_provinceService.GetProvinceList(),
        //           news => news.CId, concelho => concelho.LookUpProvinceId,
        //           (news, concelho) => new { News = news, Concelho = concelho })
        //           .GroupBy(g => g.Concelho)
        //           .Select(g => new FilterObject
        //           {
        //               Id = g.Key.LookUpProvinceId,
        //               Title = g.Key.LookUpProvinceName,
        //               Count = g.Count()
        //           }).ToList();
        //        if (cityResult != null)
        //        {
        //            provinceResult.FirstOrDefault(x => x.Title == filterParam.concelho).Children = cityResult;
        //        }
        //        query = query.FilterbyDistrito(filterParam.distrtito);
        //    }

        //    List<FilterObject> stateResult = null;
        //    if (!string.IsNullOrEmpty(filterParam.country))
        //    {
        //        stateResult = list.Join(_stateService.GetStateList(),
        //           news => news.LocationState, distrito => distrito.LookUpStateId,
        //           (news, distrito) => new { News = news, Distrito = distrito })
        //           .GroupBy(g => g.Distrito)
        //           .Select(g => new FilterObject
        //           {
        //               Id = g.Key.LookUpStateId,
        //               Title = g.Key.LookUpStateName,
        //               Count = g.Count()
        //           }).ToList();
        //        if (provinceResult != null)
        //        {
        //            stateResult.FirstOrDefault(x => x.Title == filterParam.distrtito).Children = provinceResult;
        //        }
        //        query = query.FilterbyCountry(filterParam.country);
        //    }


        //    List<FilterObject> countryResult = list.Join(_countryService.GetActiveCountiesForFooter().ToList(),
        //        news => news.LocationCountry, country => country.CountryId,
        //                        (news, country) => new { News = news, Country = country })
        //                        .GroupBy(g => g.Country)
        //                        .Select(g => new FilterObject
        //                        {
        //                            Id = g.Key.CountryId,
        //                            Title = g.Key.CountryName,
        //                            Count = g.Count()
        //                        }).ToList();
        //    if (stateResult != null && stateResult.Count() > 0)
        //    {
        //        countryResult.FirstOrDefault(x => x.Title == filterParam.country).Children = stateResult;
        //    }
        //    facetResult.LocationFilter = countryResult;

        //    listNewsViewModel = query.Select(x => new ClassifiedCreateModel
        //    {
        //        ClassifiedId = x.ClassifiedId,
        //        Title = x.Title,
        //        Summary = x.Summary,
        //        Description = x.Detail,
        //        ClassifiedPhoto = x.ClassifiedPhotos.FirstOrDefault(),
        //        SubCategoryId = x.SubCategory.LookupSubCategoryId,
        //        SlugUrl=x.SlugUrl
        //    }).ToList();

        //    var model = new FilteredClassifiedList();

        //    model.PageListItem = listNewsViewModel.ToPagedList(filterParam.page, filterParam.pageSize);
        //    model.Facet.filterParam = filterParam;
        //    model.Facet.FacetResult = facetResult;
        //    return View(model);


        //}

        //[Route("Classificados/{id}")]
        //[HttpGet]
        //public ActionResult GeneralClassifieds(string id)
        //{
        //    ViewBag.Msg = TempData["SuccessMsg"];
        //    ClassifiedDetailsViewModel model = new ClassifiedDetailsViewModel();

        //    //var classifiedId = !string.IsNullOrEmpty(id) ? Convert.ToInt32(id) : 0;

        //    var classified = _classifiedServices.GetClassifiedBySlug(id);
        //    var classifiedId = classified != null ? Convert.ToInt32(classified.ClassifiedId) : 0;
        //    if (classified != null)
        //    {
        //        classified.Views = classified.Views + 1;
        //        _classifiedServices.Update(classified);
        //        if (classified.LoginId != new Guid("00000000-0000-0000-0000-000000000000"))
        //        {
        //            var userDetails = _userService.Find(classified.LoginId);
        //            if (userDetails != null)
        //            {
        //                if (userDetails.Member != null)
        //                {
        //                    if (userDetails.IsCompanyUser())
        //                    {
        //                        model.RoleId = RoleType.COMPANYUSER;
        //                    }
        //                    else
        //                    {
        //                        model.RoleId = RoleType.INDIVIDUALUSER;
        //                    }
        //                    model.Member = userDetails.Member;
        //                }
        //            }
        //        }
        //        model.BusniessName = classified.Business.LookUpBusinessName;
        //        var distrito = _stateService.GetStateById(Convert.ToInt32(classified.FId));
        //        if (distrito != null)
        //        {
        //            model.Distrito = distrito.LookUpStateName;
        //        }

        //        var country = _countryService.GetActiveCountiesForFooter().ToList().FirstOrDefault(x => x.CountryId == classified.LocationCountry);
        //        if (country != null)
        //        {
        //            model.Country = country.CountryName;
        //        }

        //        var concelos = _provinceService.GetProvinceList().FirstOrDefault(x => x.LookUpProvinceId == classified.CId);
        //        if (concelos != null)
        //        {
        //            model.Concelho = concelos.LookUpProvinceName;
        //        }
        //        var fregusia = _cityService.GetCityList().FirstOrDefault(x => x.LookUpCityId == classified.FId);
        //        if (fregusia != null)
        //        {
        //            model.Fregusia = fregusia.LookUpCityName;
        //        }

        //        if (classified.ClassifiedPhotos != null)
        //        {
        //            model.ClassifiedPhotos = classified.ClassifiedPhotos;
        //        }
        //        model.Title = classified.Title;
        //        model.Summary = classified.Summary;
        //        model.Price = classified.Price;
        //        model.Description = classified.Detail;
        //        model.CreateDate = classified.PostedOn;
        //        model.Viewed = classified.Views;
        //        model.ClassifiedId = classified.ClassifiedId;
        //        model.SlugUrl = classified.SlugUrl;
        //        if (classified.SellConditionId != null)
        //        {
        //            var sellCondition = _sellConditionService.GetSellConditionById(classified.SellConditionId);
        //            if (sellCondition != null)
        //            {
        //                model.SellCondition = sellCondition.LookUpSellConditionName;
        //            }
        //        }

        //        if (classified.ConditionId != null)
        //        {
        //            var condition = _conditionService.GetConditionById(classified.ConditionId);
        //            if (condition != null)
        //            {
        //                model.Condition = condition.LookUpConditionName;
        //            }
        //        }

        //        var extra = classified.ExtraAutos.FirstOrDefault(x => x.ClassifiedId == classifiedId);
        //        if (extra != null)
        //        {

        //            var classifiedQuery = from extraAuto in entitiesDb.ExtraAutos
        //                                  join make in entitiesDb.LookUpMakes on extraAuto.MakeId equals make.LookUpMakeId into group1
        //                                  from makeGroup in group1.DefaultIfEmpty()
        //                                  join modelName in entitiesDb.LookUpModels on extraAuto.ModelId equals modelName.LookUpModelId into group2
        //                                  from g2 in group2.DefaultIfEmpty()
        //                                  join cumbston in entitiesDb.LookUpCombustions on extraAuto.CombustionId equals cumbston.LookUpCombustionId into group3
        //                                  from g3 in group3.DefaultIfEmpty()
        //                                  join body in entitiesDb.LookUpBodies on extraAuto.BodyId equals body.LookUpBodyId into group4
        //                                  from g4 in group4.DefaultIfEmpty()
        //                                  where extraAuto.ExtraAutoId == extra.ExtraAutoId
        //                                  select new
        //                                  {
        //                                      MakeName = makeGroup.MakeName,
        //                                      ModelName = g2.ModelName,
        //                                      Version = extraAuto.Version,
        //                                      Potentia = extraAuto.Potencia.ToString(),
        //                                      Ano = extraAuto.Potencia.ToString(),
        //                                      cylendars = extra.Cylinder,
        //                                      cumbston = g3.LookUpCombustionName,
        //                                      bodyName = g4.LookUpBodyName
        //                                  };
        //            if (classifiedQuery != null)
        //            {
        //                ExtraAutoDetails oExtraAutoDetails = classifiedQuery.Select(x => new ExtraAutoDetails
        //                {
        //                    Ano = x.Ano,
        //                    Version = x.Version,
        //                    ModelName = x.ModelName,
        //                    MakeName = x.MakeName,
        //                    Cylendra = x.cylendars.ToString(),
        //                    Cumstolevel = x.cumbston,
        //                    BodyName = x.bodyName


        //                }).FirstOrDefault();
        //                model.ExtraAutoDetails = oExtraAutoDetails;
        //            }

        //            model.ExtraAutos = classified.ExtraAutos.FirstOrDefault(x => x.ClassifiedId == classifiedId);
        //            model.ExtraReals = classified.ExtraReals.FirstOrDefault(x => x.ClassifiedId == classifiedId);
        //        }
        //    }

        //    return View("ClassifiedDetails", model);

        //}

        //[Route("Classificado/{id}")]
        //[HttpGet]
        //public ActionResult ClassifiedDetails(string id)
        //{
        //    ViewBag.Msg = TempData["SuccessMsg"];
        //    ClassifiedDetailsViewModel model = new ClassifiedDetailsViewModel();

        //    var classified = _classifiedServices.GetClassifiedBySlug(id);
        //    var classifiedId = classified != null ? Convert.ToInt32(classified.ClassifiedId) : 0;
        //    if (classified != null)
        //    {
        //        classified.Views = classified.Views + 1;
        //        _classifiedServices.Update(classified);
        //        if (classified.LoginId != new Guid("00000000-0000-0000-0000-000000000000"))
        //        {
        //            var userDetails = _userService.Find(classified.LoginId);
        //            if (userDetails != null)
        //            {

        //                if (userDetails.Member != null)
        //                {
        //                    model.Email = userDetails.Email;
        //                    if (userDetails.IsCompanyUser())
        //                    {
        //                        model.RoleId = RoleType.COMPANYUSER;
        //                    }
        //                    else
        //                    {
        //                        model.RoleId = RoleType.INDIVIDUALUSER;
        //                    }
        //                    model.Member = userDetails.Member;
        //                }
        //            }
        //        }
        //        model.BusniessName = classified.Business.LookUpBusinessName;
        //        var distrito = _stateService.GetStateById(Convert.ToInt32(classified.FId));
        //        if (distrito != null)
        //        {
        //            model.Distrito = distrito.LookUpStateName;
        //        }

        //        var country = _countryService.GetActiveCountiesForFooter().ToList().FirstOrDefault(x => x.CountryId == classified.LocationCountry);
        //        if (country != null)
        //        {
        //            model.Country = country.CountryName;
        //        }

        //        var concelos = _provinceService.GetProvinceList().FirstOrDefault(x => x.LookUpProvinceId == classified.CId);
        //        if (concelos != null)
        //        {
        //            model.Concelho = concelos.LookUpProvinceName;
        //        }
        //        var fregusia = _cityService.GetCityList().FirstOrDefault(x => x.LookUpCityId == classified.FId);
        //        if (fregusia != null)
        //        {
        //            model.Fregusia = fregusia.LookUpCityName;
        //        }

        //        if (classified.ClassifiedPhotos != null)
        //        {
        //            model.ClassifiedPhotos = classified.ClassifiedPhotos;
        //        }
        //        model.Title = classified.Title;
        //        model.Summary = classified.Summary;
        //        model.Price = classified.Price;
        //        model.Description = classified.Detail;
        //        model.CreateDate = classified.PostedOn;
        //        model.Viewed = classified.Views;
        //        model.ClassifiedId = classified.ClassifiedId;
        //        model.SlugUrl = classified.SlugUrl;
        //        if (classified.SellConditionId != null)
        //        {
        //            var sellCondition = _sellConditionService.GetSellConditionById(classified.SellConditionId);
        //            if (sellCondition != null)
        //            {
        //                model.SellCondition = sellCondition.LookUpSellConditionName;
        //            }
        //        }

        //        if (classified.ConditionId != null)
        //        {
        //            var condition = _conditionService.GetConditionById(classified.ConditionId);
        //            if (condition != null)
        //            {
        //                model.Condition = condition.LookUpConditionName;
        //            }
        //        }

        //        var extra = classified.ExtraAutos.FirstOrDefault(x => x.ClassifiedId == classifiedId);
        //        if (extra != null)
        //        {

        //            var classifiedQuery = from extraAuto in entitiesDb.ExtraAutos
        //                                  join make in entitiesDb.LookUpMakes on extraAuto.MakeId equals make.LookUpMakeId into group1
        //                                  from makeGroup in group1.DefaultIfEmpty()
        //                                  join modelName in entitiesDb.LookUpModels on extraAuto.ModelId equals modelName.LookUpModelId into group2
        //                                  from g2 in group2.DefaultIfEmpty()
        //                                  join cumbston in entitiesDb.LookUpCombustions on extraAuto.CombustionId equals cumbston.LookUpCombustionId into group3
        //                                  from g3 in group3.DefaultIfEmpty()
        //                                  join body in entitiesDb.LookUpBodies on extraAuto.BodyId equals body.LookUpBodyId into group4
        //                                  from g4 in group4.DefaultIfEmpty()
        //                                  where extraAuto.ExtraAutoId == extra.ExtraAutoId
        //                                  select new
        //                                  {
        //                                      MakeName = makeGroup.MakeName,
        //                                      ModelName = g2.ModelName,
        //                                      Version = extraAuto.Version,
        //                                      Potentia = extraAuto.Potencia.ToString(),
        //                                      Ano = extraAuto.Potencia.ToString(),
        //                                      cylendars = extra.Cylinder,
        //                                      cumbston = g3.LookUpCombustionName,
        //                                      bodyName = g4.LookUpBodyName
        //                                  };
        //            if (classifiedQuery != null)
        //            {
        //                ExtraAutoDetails oExtraAutoDetails = classifiedQuery.Select(x => new ExtraAutoDetails
        //                {
        //                    Ano = x.Ano,
        //                    Version = x.Version,
        //                    ModelName = x.ModelName,
        //                    MakeName = x.MakeName,
        //                    Cylendra = x.cylendars.ToString(),
        //                    Cumstolevel = x.cumbston,
        //                    BodyName = x.bodyName


        //                }).FirstOrDefault();
        //                model.ExtraAutoDetails = oExtraAutoDetails;
        //            }

        //            model.ExtraAutos = classified.ExtraAutos.FirstOrDefault(x => x.ClassifiedId == classifiedId);
        //            model.ExtraReals = classified.ExtraReals.FirstOrDefault(x => x.ClassifiedId == classifiedId);
        //        }
        //    }
        //    return View("GeneralClassifieds", model);
        //}
        //#endregion

        //public PartialViewResult NewsByCategory(string category, int? page, string subCategory)
        //{
        //    ViewBag.Name = category;
        //    ViewBag.SubCategory = subCategory;
        //    ViewBag.SubCategories = _newsService.GetSubCategoryList().Where(x => x.LookUpNewsCategory.LookUpNewsCategoryName == category).ToList();
        //    IEnumerable<NewsViewModel> listNewsViewModel = null;
        //    var list = _newsService.GetNewsByCategory(category).Where(x=>x.IsActive && x.Approved);
        //    if (!string.IsNullOrEmpty(subCategory))
        //    {
        //        list = _newsService.GetNewsBySubCategory(subCategory).Where(x=>x.IsActive && x.Approved);
        //    }

        //    if (list.ToList().Any())
        //    {
        //        listNewsViewModel = list.Select(x => new NewsViewModel
        //        {
        //            NewsId = x.NewsId,
        //            Title = x.Title,
        //            Summary = x.Summary,
        //            Detail = x.Detail,
        //            Photo = x.NewsPhotos.FirstOrDefault(),
        //            TitleUrl = x.TitleUrl,
        //            CategoryName = category
        //        }).AsEnumerable();
        //    }
        //    int pageSize = 10;
        //    int pageNumber = (page ?? 1);
        //    return PartialView("CategoryNewsPartial", listNewsViewModel.ToPagedList(pageNumber, pageSize));
        //}


        //public ActionResult Unauthorized()
        //{
        //    if (base.loginUserModel == null)
        //    {
        //        return RedirectToAction("Login", "Account", new { @area = "" });
        //    }
        //    switch (base.loginUserModel.UserRoleName)
        //    {
        //        case "Administrator":
        //            return RedirectToAction("Unauthorized", "Dashboard", new { @area = "Admin" });
        //        case "Empresa":
        //            return RedirectToAction("Unauthorized", "CompanyDashboard", new { @area = "Company" });
        //        case "Particular":
        //            return RedirectToAction("Unauthorized", "Dashboard", new { @area = "User" });
        //    }
        //    return View();
        //}

        //public ActionResult Condicoes()
        //{
        //    return View();
        //}



        //public ActionResult Privacidade()
        //{
        //    return View();
        //}
        //public ActionResult Acerca()
        //{
        //    return View();
        //}
        //public ActionResult Contactos()
        //{
        //    ViewBag.Msg = TempData["SuccessMsg"];
        //    return View();
        //}

        //public ActionResult Início()
        //{
        //    return RedirectToRoute(new
        //    {
        //        controller = "Home",
        //        action = "Index"
        //    });
        //}
        //public ActionResult PageDetails(string sector)
        //{
        //    PageCMSViewModel model = new PageCMSViewModel();
        //    var pageCms = _pageCmsService.GetPageBySector(sector);
        //    if (pageCms != null)
        //    {
        //        model.Detail = pageCms.Detail;
        //    }
        //    else { model.Detail = "Conteúdo n&atilde;o especificado!"; }
        //    return PartialView("PagePartial", model);
        //}

        //public ActionResult EmpresasEmail()
        //{
        //    CommentViewModel model = new CommentViewModel();
        //    return PartialView("EmpresasEmailPartial", model);

        //}
        //[HttpPost]
        //public ActionResult EmpresasEmail(CommentViewModel model)
        //{
        //    if (this.IsCaptchaValid("Captcha is not valid"))
        //    {
        //        if (!string.IsNullOrEmpty(model.EmpresasOwnerEmail))
        //        {
        //            Emailer.EmailService objEmail = new Emailer.EmailService();
        //            var fileUrl = Server.MapPath("~/EmailTemplates/UserComments.html");
        //            string html = System.IO.File.ReadAllText(fileUrl);
        //            html = html.Replace("@@SenderName", model.Name).Replace("@@Email", model.Email).Replace("@@Comments", model.Comments);
        //            objEmail.SendEmail("User send a query", html, model.EmpresasOwnerEmail, model.Email);
        //            TempData["SuccessMsg"] = "Recebemos a sua mensagem, Responderemos brevemente.";
        //            return RedirectToAction(model.RedirectingActionName, new { @id = model.Title });
        //        }
        //        else
        //        {
        //            TempData["SuccessMsg"] = "Nenhum endereço de e-mail é fornecido";
        //            return RedirectToAction(model.RedirectingActionName, new { @id = model.Title });
        //        }
        //    }

        //    //ViewBag.ErrMessage = "Error: captcha is not valid.";
        //    TempData["SuccessMsg"] = "Error: captcha is not valid.";
        //    return RedirectToAction(model.RedirectingActionName, new { @id = model.Title });
        //}


        //public ActionResult Comments()
        //{
        //    CommentViewModel model = new CommentViewModel();
        //    return PartialView("CommentsPartial", model);
        //}

        //[HttpPost]
        //public ActionResult Comments(CommentViewModel model)
        //{
        //    if (this.IsCaptchaValid("Captcha is not valid"))
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            var comments = new Comments();
        //            comments.Approved = false;
        //            comments.Name = model.Name;
        //            comments.Email = model.Email;
        //            comments.Section = model.Section;
        //            comments.UserComments = model.Comments;
        //            comments.UserIP = Request.UserHostAddress;
        //            comments.SectionId = model.SectionId;
        //            comments.SlugTitle = model.Title;
        //            comments.CategoryName = model.CategoryName;
        //            _generalCommentService.Add(comments);
        //            TempData["SuccessMsg"] = "Comentário enviado com sucesso.";
        //            return RedirectToAction(model.RedirectingActionName, new { @id = model.Title });
        //        }
        //        else
        //        {
        //            TempData["SuccessMsg"] = "Error: Some error occured";
        //            return RedirectToAction(model.RedirectingActionName, new { @id = model.Title });
        //        }

        //    }

        //    //ViewBag.ErrMessage = "Error: captcha is not valid.";
        //    TempData["SuccessMsg"] = "Error: captcha is not valid.";
        //    return RedirectToAction(model.RedirectingActionName, new { @id = model.Title });
        //}

        //public ActionResult Email()
        //{
        //    MailViewModel model = new MailViewModel();
        //    return PartialView("EmailPartial", model);
        //}

        //[HttpPost]
        //public ActionResult Email(MailViewModel model)
        //{
        //    if (this.IsCaptchaValid("Captcha is not valid"))
        //    {
        //        Emailer.EmailService objEmail = new Emailer.EmailService();
        //        var fileUrl = Server.MapPath("~/EmailTemplates/UserComments.html");
        //        string html = System.IO.File.ReadAllText(fileUrl);
        //        html = html.Replace("@@SenderName", model.Name).Replace("@@Phone", model.Telephone).Replace("@@Email", model.Email).Replace("@@Comments", model.Comments);
        //        objEmail.SendEmail("User send a query", html, "info@portugalbiz.pt", model.Email);
        //        TempData["SuccessMsg"] = "Recebemos a sua mensagem, Responderemos brevemente.";
        //        return RedirectToAction("Contactos");
        //    }

        //    //ViewBag.ErrMessage = "Error: captcha is not valid.";
        //    TempData["SuccessMsg"] = "Error: captcha is not valid.";
        //    return RedirectToAction("Contactos");

        //}



        //public ActionResult Empresas(string industry, string country, string distrito, int page = 1, int pageSize = 25)
        //{
        //    var query = entitiesDb.Companies.AsQueryable();
        //    if (!string.IsNullOrEmpty(industry))
        //    {

        //        if (!string.IsNullOrEmpty(industry))
        //        {
        //            TempData["industry"] = industry;
        //            query = query.FilterByIndustry(industry);
        //        }
        //        if (!string.IsNullOrEmpty(country))
        //        {
        //            TempData["country"] = country;
        //            ViewBag.Country = country;
        //            query = query.FilterbyCountry(country);
        //        }
        //        if (!string.IsNullOrEmpty(distrito))
        //        {
        //            TempData["distrito"] = distrito;
        //            ViewBag.distrito = distrito;
        //            query = query.FilterbyDistrito(distrito);
        //        }

        //        var model = query.OrderBy(x => x.LookUpCompanyName).ToPagedList(page, pageSize);
        //        return View("EmpresaList", model);
        //    }
        //    else
        //    {
        //        //var query1 = _activitiesSectorServices.GetIndustries();
        //        if (!string.IsNullOrEmpty(country))
        //        {
        //            TempData["country"] = country;
        //            ViewBag.Country = country;
        //            query = query.FilterbyCountry(country);
        //        }
        //        if (!string.IsNullOrEmpty(distrito))
        //        {
        //            TempData["distrito"] = distrito;
        //            ViewBag.distrito = distrito;
        //            query = query.FilterbyDistrito(distrito);
        //        }
        //        IEnumerable<CompanyInIndustry> model = query.Join(entitiesDb.LookUpIndustries, company => company.EmpresaActId, industries => industries.LookUpIndustryId,
        //        (company, industries) => new { Company = company, Industry = industries })
        //        .GroupBy(g => new { g.Industry })
        //        .Select(z =>
        //            new CompanyInIndustry
        //            {
        //                CompanyCount = z.Count(),
        //                IndustryId = z.Key.Industry.LookUpIndustryId,
        //                IndustryName = z.Key.Industry.LookUpIndustryName,
        //            }).ToList();
        //        return View(model);
        //    }
        //}

        //[NonAction]
        //private ActionResult TopPages(string id, string distrito)
        //{
        //    var newsQuery = _newsService.GetQueryableNews();
        //    var classifiedQuery = _classifiedServices.GetQueryableClassifieds();
        //    var eventQuery = _eventService.GetQueryableEvents();

        //    if (!string.IsNullOrEmpty(id))
        //    {
        //        newsQuery = newsQuery.FilterByCategory(id);
        //        classifiedQuery = classifiedQuery.FilterByCategory(id);
        //        eventQuery = eventQuery.FilterByCategory(id);
        //    }
        //    var model = new DistritoNewsViewModel { DistritoName = distrito };

        //    if (string.IsNullOrEmpty(distrito))
        //    {
        //        var state = _stateService.GetStateList().FirstOrDefault(x => x.LookUpStateName == distrito);
        //        if (state != null)
        //        {
        //            model.DistritoName = state.LookUpStateName;
        //            model.DistritoId = state.LookUpStateId;
        //            newsQuery = newsQuery.FilterbyDistrito(distrito);
        //            classifiedQuery = classifiedQuery.FilterbyDistrito(distrito);
        //            eventQuery = eventQuery.FilterbyDistrito(distrito);
        //        }
        //    }
        //    model.NewsList = newsQuery.OrderByDescending(x => x.DateFrom).Take(3).ToList();
        //    model.ClassifiedsList = classifiedQuery.Take(3).OrderByDescending(x => x.PostedOn).ToList();
        //    model.EventList = eventQuery.Take(3).OrderByDescending(x => x.DateFrom).ToList();
        //    return View("DistritoSection", model);

        //}

        //[Route("Casa")]
        //public ActionResult Casa(int? bedrooms, string minPrice, string maxPrice, string type, string keyword, int page = 1, int pageSize = 5)
        //{
        //    ViewBag.Tipologia = _tipologiaService.GetAll();
        //    ViewBag.Natureza = _naturezaService.GetAll();
        //    ViewBag.minPrice = minPrice;
        //    ViewBag.maxPrice = maxPrice;
        //    ViewBag.bedrooms = bedrooms;
        //    ViewBag.type = type;
        //    ViewBag.keyword = keyword;
        //    var classifiedQuery = _classifiedServices.GetQueryableClassifieds();
        //    if (!string.IsNullOrEmpty(keyword))
        //    {
        //        classifiedQuery = classifiedQuery.FilterByCategory("Imobiliário").FilterByKeyword(keyword).OrderByDescending(x => x.PostedOn);
        //    }
        //    if (bedrooms.HasValue)
        //    {
        //        var tipologiaId = Convert.ToInt32(bedrooms);
        //        classifiedQuery = classifiedQuery.FilterByCategory("Imobiliário").FilterByTipologia(tipologiaId).OrderByDescending(x => x.PostedOn);
        //    }
        //    if ((!string.IsNullOrEmpty(minPrice) && minPrice != "select") && (!string.IsNullOrEmpty(maxPrice) && maxPrice != "select"))
        //    {
        //        var minimumPrice = minPrice.Split('€');
        //        var maximumPrice = maxPrice.Split('€');
        //        decimal topPrice = Convert.ToDecimal(maximumPrice[1]);
        //        decimal lowPrice = Convert.ToDecimal(minimumPrice[1]);
        //        classifiedQuery = classifiedQuery.FilterByCategory("Imobiliário").FilterByPriceMinMax(topPrice, lowPrice).OrderByDescending(x => x.PostedOn);
        //    }
        //    if ((!string.IsNullOrEmpty(minPrice) && minPrice != "select") && maxPrice == "select")
        //    {
        //        var minimumPrice = minPrice.Split('€');
        //        decimal lowPrice = Convert.ToDecimal(minimumPrice[1]);
        //        classifiedQuery = classifiedQuery.FilterByCategory("Imobiliário").FilterByMinPrice(lowPrice).OrderByDescending(x => x.PostedOn);
        //    }
        //    if (minPrice == "select" && (!string.IsNullOrEmpty(maxPrice) && maxPrice != "select"))
        //    {
        //        var maximumPrice = maxPrice.Split('€');
        //        decimal topPrice = Convert.ToDecimal(maximumPrice[1]);
        //        classifiedQuery = classifiedQuery.FilterByCategory("Imobiliário").FilterByMaxPrice(topPrice).OrderByDescending(x => x.PostedOn);
        //    }

        //    var list = classifiedQuery.FilterByCategory("Imobiliário").OrderByDescending(x => x.PostedOn).ToList();
        //    return View(list.ToPagedList(page, pageSize));
        //}

        //[Route("Casa/{id}")]
        //public ActionResult CasaDetail(string id)
        //{
        //    int classifiedId = 0;
        //    int concelhoId = 0;
        //    int freguesiaId = 0;
        //    //var casaid = Convert.ToInt32(id);
        //    CasaViewModel model = new CasaViewModel();
        //    //_classifiedServices.GetClassifiedBySlug(id);
        //    var classified = _classifiedServices.GetClassifiedBySlug(id);
        //    if (classified.LoginId != new Guid("00000000-0000-0000-0000-000000000000"))
        //    {
        //        var userDetails = _userService.Find(classified.LoginId);
        //        if (userDetails != null)
        //        {
        //            if (userDetails.Member != null)
        //            {
        //                if (userDetails.IsCompanyUser())
        //                {
        //                    model.RoleId = RoleType.COMPANYUSER;
        //                }
        //                else
        //                {
        //                    model.RoleId = RoleType.INDIVIDUALUSER;
        //                }
        //                model.Member = userDetails.Member;
        //            }
        //        }
        //    }
        //    if (classified != null)
        //    {
        //        classifiedId = classified.ClassifiedId;
        //        model.SlugUrl = classified.SlugUrl;
        //    }
        //    var classifiedQuery = _classifiedServices.GetQueryableClassifieds().FirstOrDefault(x => x.ClassifiedId == classifiedId);
        //    if (classifiedQuery != null)
        //    {
        //        var view = classified.Views;
        //        view = view + 1;
        //        _classifiedServices.Update(classifiedQuery);

        //        model.Title = classifiedQuery.Title;
        //        model.Summary = classifiedQuery.Summary;
        //        model.Details = classifiedQuery.Detail;
        //        model.Id = classifiedQuery.ClassifiedId.ToString();

        //        if (classifiedQuery.ExtraReals.Count() > 0)
        //        {

        //            model.BedRooms = classifiedQuery.ExtraReals.FirstOrDefault().Tipologia.ToString();
        //            if (!string.IsNullOrEmpty(model.BedRooms))
        //            {
        //                var tiplogia = _tipologiaService.GetById(Convert.ToInt32(model.BedRooms));
        //                if (tiplogia != null)
        //                {
        //                    model.BedRooms = tiplogia.LookUpTipologiaName;
        //                }
        //            }
        //            var test1 = classifiedQuery.ExtraReals.Join(entitiesDb.LookUpNaturezas, extra => extra.Natureza, Nat => Nat.LookUpNaturezaId, (extra, Nat) => new { Nat = Nat, extra = extra });
        //            model.PropertyType = test1.FirstOrDefault().Nat.LookUpNaturezaName;
        //            model.LivingArea = classifiedQuery.ExtraReals.FirstOrDefault().Areabruta;
        //            model.UseFulArea = classifiedQuery.ExtraReals.FirstOrDefault().Areautil;
        //            model.CoordX = classifiedQuery.ExtraReals.FirstOrDefault().Coordx;
        //            model.CoordY = classifiedQuery.ExtraReals.FirstOrDefault().Coordy;
        //            model.Empreendimento = classifiedQuery.ExtraReals.FirstOrDefault().Empreendimento;
        //            model.Frecao = classifiedQuery.ExtraReals.FirstOrDefault().Fraccao;
        //            model.Reference = classifiedQuery.ExtraReals.FirstOrDefault().Referencia;
        //            model.Garagem = classifiedQuery.ExtraReals.FirstOrDefault().Garagem;
        //            model.AreaTerreNo = classifiedQuery.ExtraReals.FirstOrDefault().Areaterreno;
        //        }
        //        model.BusniessName = classified.Business.LookUpBusinessName;
        //        if (classified.SellConditionId != null)
        //        {
        //            var sellCondition = _sellConditionService.GetSellConditionById(classified.SellConditionId);
        //            if (sellCondition != null)
        //            {
        //                model.SellCondition = sellCondition.LookUpSellConditionName;
        //            }
        //        }

        //        if (classified.ConditionId != null)
        //        {
        //            var condition = _conditionService.GetConditionById(classified.ConditionId);
        //            if (condition != null)
        //            {
        //                model.Condition = condition.LookUpConditionName;
        //            }
        //        }
        //        if (classifiedQuery.CId != null)
        //        {
        //            concelhoId = Convert.ToInt32(classifiedQuery.CId);
        //        }
        //        if (classifiedQuery.FId != null)
        //        {
        //            freguesiaId = Convert.ToInt32(classifiedQuery.FId);
        //        }
        //        model.Country = _countryService.GetCountryById(classifiedQuery.LocationCountry) != null ? _countryService.GetCountryById(classifiedQuery.LocationCountry).CountryName : string.Empty;
        //        model.Distrito = _stateService.GetStateById(classifiedQuery.LocationState) != null ? _stateService.GetStateById(classifiedQuery.LocationState).LookUpStateName : string.Empty;
        //        model.Concelho = _provinceService.GetConelhoById(concelhoId) != null ? _provinceService.GetConelhoById(concelhoId).LookUpProvinceName : string.Empty;
        //        model.Freguesia = _cityService.GetCityById(freguesiaId) != null ? _cityService.GetCityById(freguesiaId).LookUpCityName : string.Empty;

        //        model.Price = classifiedQuery.Price;
        //        model.Views = classifiedQuery.Views.ToString();
        //        model.PublisedDate = classifiedQuery.PostedOn;

        //        model.PhotosList = classifiedQuery.ClassifiedPhotos;

        //    }
        //    return View(model);
        //}

        //public ActionResult GetCasaDistrito(string classifiedId)
        //{
        //    string returnVal = string.Empty;
        //    var id = Convert.ToInt32(classifiedId);
        //    var classifiedQuery = _classifiedServices.GetQueryableClassifieds().FirstOrDefault(x => x.ClassifiedId == id);
        //    if (classifiedQuery != null)
        //    {
        //        var distritoId = Convert.ToInt32(classifiedQuery.FId ?? 0);
        //        var distrito = _cityService.GetCityById(Convert.ToInt32(distritoId));
        //        if (distrito != null)
        //        {
        //            returnVal = distrito.LookUpCityName;
        //        }
        //    }
        //    return Json(returnVal, JsonRequestBehavior.AllowGet);
        //}
        //public ActionResult News(FilterParam filterParam)
        //{
        //    filterParam.page = filterParam.page == 0 ? 1 : filterParam.page;
        //    filterParam.pageSize = filterParam.pageSize == 0 ? 10 : filterParam.pageSize;
        //    IEnumerable<NewsViewModel> listNewsViewModel = null;
        //    var list = _newsService.GetNewsByKeyword(filterParam.q);
        //    var query = _newsService.GetQueryableNews();
        //    if (!string.IsNullOrEmpty(filterParam.q))
        //    {
        //        query = query.FilterByKeyword(filterParam.q);
        //    }
        //    var facetResult = new FacetResult();


        //    if (!string.IsNullOrEmpty(filterParam.scat))
        //    {
        //        filterParam.cat = _newsService.GetSubCategoryList().FirstOrDefault(x => x.LookUpNewsSubCategoryName == filterParam.scat).LookUpNewsCategory.LookUpNewsCategoryName;
        //        query = query.FilterBySubCategory(filterParam.scat);
        //    }
        //    List<FilterObject> scatResult = null;
        //    if (!string.IsNullOrEmpty(filterParam.cat))
        //    {
        //        scatResult = list.GroupBy(g => g.NewsSubCategory)
        //                                         .Select(g =>
        //                                             new FilterObject
        //                                             {
        //                                                 Id = g.Key.LookUpNewsCategoryId,
        //                                                 Title = g.Key.LookUpNewsSubCategoryName,
        //                                                 Count = g.Count()
        //                                             }).ToList();
        //        query = query.FilterByCategory(filterParam.cat);
        //    }
        //    facetResult.CategoryFilter = list.GroupBy(g => g.NewsSubCategory.LookUpNewsCategory)
        //                                    .Select(g => new FilterObject
        //                                    {
        //                                        Id = g.Key.LookUpNewsCategoryId,
        //                                        Title = g.Key.LookUpNewsCategoryName,
        //                                        Count = g.Count()
        //                                    }).ToList();
        //    if (scatResult != null)
        //    {
        //        facetResult.CategoryFilter.FirstOrDefault(x => x.Title == filterParam.cat).Children = scatResult;
        //    }



        //    if (!string.IsNullOrEmpty(filterParam.fregusia))
        //    {
        //        filterParam.concelho = _provinceService.GetProvinceList().FirstOrDefault(x => x.LookUpCities.Any(y => y.LookUpCityName == filterParam.fregusia)).LookUpProvinceName;
        //        query = query.FilterbyFergrusia(filterParam.fregusia);
        //    }


        //    List<FilterObject> cityResult = null;
        //    if (!string.IsNullOrEmpty(filterParam.concelho))
        //    {
        //        cityResult = list.Join(
        //                                 _cityService.GetCityList(),
        //                                 news => news.FId, city => city.LookUpCityId,
        //                                 (news, city) => new { News = news, City = city }
        //                               )
        //                               .GroupBy(g => g.City)
        //                                .Select(g => new FilterObject
        //                                {
        //                                    Id = g.Key.LookUpCityId,
        //                                    Title = g.Key.LookUpCityName,
        //                                    Count = g.Count()
        //                                }).ToList();
        //        filterParam.distrtito = _stateService.GetStateList().FirstOrDefault(x => x.Provinces.Any(y => y.LookUpProvinceName == filterParam.concelho)).LookUpStateName;
        //        query = query.FilterbyConcelho(filterParam.concelho);
        //    }


        //    List<FilterObject> provinceResult = null;
        //    if (!string.IsNullOrEmpty(filterParam.distrtito))
        //    {
        //        provinceResult = list.Join(_provinceService.GetProvinceList(),
        //           news => news.CId, concelho => concelho.LookUpProvinceId,
        //           (news, concelho) => new { News = news, Concelho = concelho })
        //           .GroupBy(g => g.Concelho)
        //           .Select(g => new FilterObject
        //           {
        //               Id = g.Key.LookUpProvinceId,
        //               Title = g.Key.LookUpProvinceName,
        //               Count = g.Count()
        //           }).ToList();
        //        if (cityResult != null)
        //        {
        //            provinceResult.FirstOrDefault(x => x.Title == filterParam.concelho).Children = cityResult;
        //        }
        //        query = query.FilterbyDistrito(filterParam.distrtito);
        //    }

        //    List<FilterObject> stateResult = null;
        //    if (!string.IsNullOrEmpty(filterParam.country))
        //    {
        //        stateResult = list.Join(_stateService.GetStateList(),
        //           news => news.StateId, distrito => distrito.LookUpStateId,
        //           (news, distrito) => new { News = news, Distrito = distrito })
        //           .GroupBy(g => g.Distrito)
        //           .Select(g => new FilterObject
        //           {
        //               Id = g.Key.LookUpStateId,
        //               Title = g.Key.LookUpStateName,
        //               Count = g.Count()
        //           }).ToList();
        //        if (provinceResult != null && stateResult.Any())
        //        {
        //            stateResult.FirstOrDefault(x => x.Title == filterParam.distrtito).Children = provinceResult;
        //        }
        //        query = query.FilterbyCountry(filterParam.country);
        //    }


        //    List<FilterObject> countryResult = list.Join(_countryService.GetActiveCountiesForFooter().ToList(),
        //        news => news.LocationCountry, country => country.CountryId,
        //                        (news, country) => new { News = news, Country = country })
        //                        .GroupBy(g => g.Country)
        //                        .Select(g => new FilterObject
        //                        {
        //                            Id = g.Key.CountryId,
        //                            Title = g.Key.CountryName,
        //                            Count = g.Count()
        //                        }).ToList();
        //    if (stateResult != null && stateResult.Any())
        //    {
        //        countryResult.FirstOrDefault(x => x.Title == filterParam.country).Children = stateResult;
        //    }
        //    facetResult.LocationFilter = countryResult;


        //    listNewsViewModel = query.Select(x => new NewsViewModel
        //    {
        //        NewsId = x.NewsId,
        //        Title = x.Title,
        //        Summary = x.Summary,
        //        Detail = x.Detail,
        //        Photo = x.NewsPhotos.FirstOrDefault(),
        //        TitleUrl = x.TitleUrl
        //    }).ToList();

        //    var model = new FilteredNewList();
        //    model.PageListItem = listNewsViewModel.ToPagedList(filterParam.page, filterParam.pageSize);
        //    model.Facet.filterParam = filterParam;
        //    model.Facet.FacetResult = facetResult;
        //    return View(model);

        //}
        //public ActionResult Events(FilterParam filterParam)
        //{
        //    filterParam.page = filterParam.page == 0 ? 1 : filterParam.page;
        //    filterParam.pageSize = filterParam.pageSize == 0 ? 10 : filterParam.pageSize;

        //    IEnumerable<EventViewModel> listNewsViewModel = null;
        //    var list = _eventService.GetEventsByKeyword(filterParam.q);
        //    var query = _eventService.GetQueryableEvents();
        //    if (!string.IsNullOrEmpty(filterParam.q))
        //    {
        //        query = query.FilterByKeyword(filterParam.q);
        //    }
        //    var facetResult = new FacetResult();

        //    if (!string.IsNullOrEmpty(filterParam.cat))
        //    {
        //        query = query.FilterByCategory(filterParam.cat);
        //    }
        //    facetResult.CategoryFilter = list.GroupBy(g => g.EventCategory)
        //                                    .Select(g => new FilterObject
        //                                    {
        //                                        Id = g.Key.LookUpEZCategoryId,
        //                                        Title = g.Key.LookUpEZCategoryTitle,
        //                                        Count = g.Count()
        //                                    }).ToList();

        //    if (!string.IsNullOrEmpty(filterParam.fregusia))
        //    {
        //        filterParam.concelho = _provinceService.GetProvinceList().FirstOrDefault(x => x.LookUpCities.Any(y => y.LookUpCityName == filterParam.fregusia)).LookUpProvinceName;
        //        query = query.FilterbyFergrusia(filterParam.fregusia);
        //    }


        //    List<FilterObject> cityResult = null;
        //    if (!string.IsNullOrEmpty(filterParam.concelho))
        //    {
        //        cityResult = list.Join(
        //                                 _cityService.GetCityList(),
        //                                 news => news.FId, city => city.LookUpCityId,
        //                                 (news, city) => new { News = news, City = city }
        //                               )
        //                               .GroupBy(g => g.City)
        //                                .Select(g => new FilterObject
        //                                {
        //                                    Id = g.Key.LookUpCityId,
        //                                    Title = g.Key.LookUpCityName,
        //                                    Count = g.Count()
        //                                }).ToList();
        //        filterParam.distrtito = _stateService.GetStateList().FirstOrDefault(x => x.Provinces.Any(y => y.LookUpProvinceName == filterParam.concelho)).LookUpStateName;
        //        query = query.FilterbyConcelho(filterParam.concelho);
        //    }


        //    List<FilterObject> provinceResult = null;
        //    if (!string.IsNullOrEmpty(filterParam.distrtito))
        //    {
        //        provinceResult = list.Join(_provinceService.GetProvinceList(),
        //           news => news.CId, concelho => concelho.LookUpProvinceId,
        //           (news, concelho) => new { News = news, Concelho = concelho })
        //           .GroupBy(g => g.Concelho)
        //           .Select(g => new FilterObject
        //           {
        //               Id = g.Key.LookUpProvinceId,
        //               Title = g.Key.LookUpProvinceName,
        //               Count = g.Count()
        //           }).ToList();
        //        if (cityResult != null)
        //        {
        //            provinceResult.FirstOrDefault(x => x.Title == filterParam.concelho).Children = cityResult;
        //        }
        //        query = query.FilterbyDistrito(filterParam.distrtito);
        //    }

        //    List<FilterObject> stateResult = null;
        //    if (!string.IsNullOrEmpty(filterParam.country))
        //    {
        //        stateResult = list.Join(_stateService.GetStateList(),
        //           news => news.StateId, distrito => distrito.LookUpStateId,
        //           (news, distrito) => new { News = news, Distrito = distrito })
        //           .GroupBy(g => g.Distrito)
        //           .Select(g => new FilterObject
        //           {
        //               Id = g.Key.LookUpStateId,
        //               Title = g.Key.LookUpStateName,
        //               Count = g.Count()
        //           }).ToList();
        //        if (provinceResult != null)
        //        {
        //            var filterObject = stateResult.FirstOrDefault(x => x.Title == filterParam.distrtito);
        //            if (filterObject != null)
        //                filterObject.Children = provinceResult;
        //        }
        //        query = query.FilterbyCountry(filterParam.country);
        //    }


        //    List<FilterObject> countryResult = list.Join(_countryService.GetActiveCountiesForFooter().ToList(),
        //        news => news.LocationCountry, country => country.CountryId,
        //                        (news, country) => new { News = news, Country = country })
        //                        .GroupBy(g => g.Country)
        //                        .Select(g => new FilterObject
        //                        {
        //                            Id = g.Key.CountryId,
        //                            Title = g.Key.CountryName,
        //                            Count = g.Count()
        //                        }).ToList();
        //    if (stateResult != null && stateResult.Any())
        //    {
        //        var filterObject = countryResult.FirstOrDefault(x => x.Title == filterParam.country);
        //        if (filterObject != null)
        //            filterObject.Children = stateResult;
        //    }
        //    facetResult.LocationFilter = countryResult;


        //    listNewsViewModel = query.Select(x => new EventViewModel
        //    {
        //        EzEventsId = x.EzEventsId,
        //        Title = x.Title,
        //        Summary = x.Summary,
        //        Detail = x.Detail,
        //        EzPhoto = x.EventPhotos.FirstOrDefault(),
        //    }).ToList();

        //    var model = new FilteredEventsList
        //    {
        //        PageListItem = listNewsViewModel.ToPagedList(filterParam.page, filterParam.pageSize),
        //        Facet = { filterParam = filterParam, FacetResult = facetResult }
        //    };
        //    return View(model);
        //}

        //public ActionResult CarrosList(int? page, string category, string subCategory)
        //{
        //    var news = _newsService.GetNewsByCategory("Carros").Where(x=>x.Approved && x.IsActive).OrderByDescending(x => x.DateFrom).AsEnumerable();

        //    const int pageSize = 8;
        //    var pageNumber = (page ?? 1);
        //    return PartialView("_CarrosList", news.ToPagedList(pageNumber, pageSize));
        //}

        ////public ActionResult Carros(string distrito)
        ////{
        ////    return TopPages("Carros", distrito);
        ////}

        //[OutputCache(Duration = 900, VaryByParam = "*")]
        //public ActionResult Classificados()
        //{
        //    ClassifiedIndexViewModel model = new ClassifiedIndexViewModel();
        //    model.FeatureAds = _classifiedServices.GetAll().Where(x => x.isFeature).Take(4).ToList();
        //    model.AdsInCategory = _categoryService.GetAll()
        //        .OrderByDescending(x => x.LookUpCategoryTitle)
        //        .Select(x => new ClassifiedCategoryViewModel
        //        {
        //            CategoryId = x.LookUpCategoryId,
        //            CategoryName = x.LookUpCategoryTitle,
        //            AdsCount = x.LookupSubCategories.Sum(y => y.Classifieds.Count()),
        //            subcategory = x.LookupSubCategories.Select(s => new ClassifiedSubCategoryViewModel
        //            {
        //                SubCategoryName = s.LookUpSubCategoryTitle,
        //                SubCategoryId = s.LookupSubCategoryId,
        //                AdsCount = s.Classifieds.Count()
        //            }).Take(5).ToList()
        //        })
        //        .ToList();
        //    model.LatestAds = _classifiedServices.GetAll().OrderByDescending(x => x.PostedOn).Take(4).ToList();
        //    model.PopularAds = _classifiedServices.GetAll().OrderByDescending(x => x.Views).Take(4).ToList();
        //    model.RandomAds = new List<Classified>();
        //    int count = _classifiedServices.GetAll().Count(); // 1st round-trip
        //    for (var i = 0; i < 4; i++)
        //    {
        //        int index = new Random().Next(count);
        //        model.RandomAds.Add(_classifiedServices.GetAll().Skip(index).FirstOrDefault());
        //    }
        //    return View(model);
        //}


        //public ActionResult Mercado()
        //{
        //    var model = _mercadoService.GetMercadoList().Where(x => x.EndDate > DateTime.Now && x.StartDate <= DateTime.Now).ToList();
        //    model.Select(x => new MercadoViewModel
        //    {
        //        MercadoId = x.MercadoId,
        //        Title = x.Title,
        //        LinkUrl = x.LinkUrl,
        //        Icon = x.Icon,
        //        Price = x.Price,
        //        Description = x.Description
        //    }).ToList();
        //    return PartialView("_mercado", model);
        //}

        //public ActionResult RecentClassifieds()
        //{
        //    List<ClassifiedPartialViewModel> list = new List<ClassifiedPartialViewModel>();
        //    var model = _classifiedServices.GetQueryableClassifieds().OrderByDescending(x => x.PostedOn).Take(5);
        //    foreach (var item in model)
        //    {
        //        ClassifiedPartialViewModel pModel = new ClassifiedPartialViewModel();
        //        pModel.ClassifiedPhotos = item.ClassifiedPhotos.FirstOrDefault();
        //        pModel.Title = item.Title;
        //        pModel.Price = item.Price.ToString();
        //        pModel.Id = item.ClassifiedId;
        //        pModel.CategoryId = item.SubCategory.LookupSubCategoryId;
        //        pModel.SlugUrl = item.SlugUrl;
        //        if (item.LocationState > 0)
        //        {
        //            pModel.DistritoName = _stateService.GetStateById(item.LocationState).LookUpStateName;
        //        }
        //        list.Add(pModel);
        //    }
        //    return PartialView("_RecentClassifieds", list);
        //}

        //[OutputCache(Duration = 60, VaryByParam = "*")]
        //public ActionResult GetClassified(string category, bool carros = false)
        //{
        //    var model = _classifiedServices.GetClassifieds(category);
        //    if (!carros)
        //    {
        //        return PartialView("_Classifieds", model);
        //    }
        //    else { return PartialView("_CarrosPartial", model); }
        //}

        //[OutputCache(Duration = 60, VaryByParam = "*")]
        //public ActionResult GetCarrosNews(string category)
        //{
        //    ViewBag.SubCategories = _newsCategoryService.GetNewsSubCategoriesListByCategoryId(5).ToList();
        //    var model = _newsService.GetNewsByCategory(category).Take(5).OrderByDescending(x => x.DateFrom).ToList();
        //    return PartialView("_CarrosPartial", model);
        //}

        //[OutputCache(Duration = 60, VaryByParam = "*")]
        //public ActionResult GetClassifiedsCategories()
        //{
        //    var model = _classifiedServices.GetClassifiedsCategories();
        //    return PartialView("_ClassifiedCategory", model);
        //}

        //public ActionResult Banner(string page, string bannerType = "Home")
        //{
        //    var model = _bannerService.Find().Where(x => x.PagePosition == bannerType
        //        && x.PageId.ToLower() == page.ToLower()).Take(5).ToList();
        //    var bModel = new BannerModel();
        //    foreach (var item in model)
        //    {
        //        StringBuilder ban = new StringBuilder();
        //        var dimension = item.BannerSize.Split('x');
        //        if (item.BannerType == "Graphic")
        //        {
        //            return PartialView("_GraphicsBanner", model);
        //        }
        //        else if (item.BannerType.ToString() == "Text")
        //        {
        //            bModel.Url = item.Url.ToString();
        //            return PartialView("_BannerPartial", bModel);
        //        }
        //    }
        //    return PartialView("_GraphicsBanner", model);
        //}

        //public ActionResult Distrito()
        //{
        //    var model = _stateService.GetStateList();
        //    return PartialView("_Distrito", model.ToList());
        //    // return View();
        //}



        //#region common Json Method
        public JsonResult Getstate(int countyid)
        {
            var subcategory = _stateService.GetStateList().Where(x => x.CountryId == Convert.ToInt32(countyid)).ToList();
            for (int i = 0; i < subcategory.Count; i++)
            {
                subcategory[i].StateName = StringCipher.Decrypt(subcategory[i].StateName);
            }
                return Json(subcategory, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCity(int StateId)
        {
            var subcity = _cityService.GetCityList().Where(x => x.LookUpProvinceId == Convert.ToInt16(StateId)).ToList();
            for (int i = 0; i < subcity.Count;i++ )
            {
                subcity[i].LookUpCityName = StringCipher.Decrypt(subcity[i].LookUpCityName);
            }
                return Json(subcity, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetStateInCountry(string Origin, string Target, string Value)
        {
            var model = new List<SelectListItem>()
                            { new SelectListItem()
                                {
                                    Text="--Select State--"
                                }
                            };
            if (!string.IsNullOrEmpty(Value))
            {
                var subcategory = _stateService.GetStateList().Where(x => x.CountryId == Convert.ToInt16(Value)).ToList();
                for (int i = 0; i < subcategory.Count; i++)
                {
                    subcategory[i].StateName = StringCipher.Decrypt(subcategory[i].StateName);
                }
                model.AddRange(subcategory.Select(x =>
                new SelectListItem
                {
                    Text = x.StateName,
                    Value = x.Id.ToString()
                }));

            }
            return Json(model);
        }
        public JsonResult GetSubCategoryInProfession(string Origin, string Target, string Value)
        {
            var model = new List<SelectListItem>()
                            { new SelectListItem()
                                {
                                    Text="Select Sub Category"
                                }
                            };
            if (!string.IsNullOrEmpty(Value))
            {
                var subcategory = _professionType.GetProfessionSubTypesList().Where(x => x.Id == Convert.ToInt16(Value));
                model.AddRange(subcategory.Select(x =>
                new SelectListItem
                {
                    Text = x.ProfessionSub,
                    Value = x.SubId.ToString()
                }));
            }
            return Json(model);
        }
        public JsonResult GetCityInState(string Origin, string Target, string Value)
        {

            var model = new List<SelectListItem>()
                            { new SelectListItem()
                                {
                                    Text="--Select City--"
                                }
                            };
            if (!string.IsNullOrEmpty(Value))
            {
                var subcategory = _cityService.GetCityList().Where(x => x.LookUpProvinceId == Convert.ToInt16(Value)).ToList();
                for (int i = 0; i < subcategory.Count; i++)
                {
                    subcategory[i].LookUpCityName = StringCipher.Decrypt(subcategory[i].LookUpCityName);
                }
                model.AddRange(subcategory.Select(x =>
                new SelectListItem
                {
                    Text = x.LookUpCityName,
                    Value = x.LookUpCityId.ToString()
                }));

            }

            return Json(model);
        }
        public JsonResult GetClinicinOrganization(string Origin, string Target, string Value)
        {
            var model = new List<SelectListItem>()
                            { new SelectListItem()
                                {
                                    Text="--Select Clinic--"
                                }
                            };
            if (!string.IsNullOrEmpty(Value))
            {
                var subcategory = _clinicService.GetClinicList().Where(x => x.OrganizationId == Convert.ToInt16(Value));
                model.AddRange(subcategory.Select(x =>
                new SelectListItem
                {
                    Text = x.ClinicName,
                    Value = x.ClinicId.ToString()
                }));
                for (int i = 0; i < model.Count; i++)
                {
                    model[i].Text = StringCipher.Decrypt(model[i].Text);
                }
            }
            return Json(model);
        }

        //#endregion

        //#region Receipe
        //[Route("Receitas")]
        //public ActionResult Receitas(int? page, string category, string subcategory, int? Id)
        //{
        //    //IEnumerable<RecipeViewModel> lstRecipeViewModel = null;
        //    var recipeCat = _recipesServices.GetRecipeCategories().Where(x => x.IsActive == true);
        //    var recipeList = _recipesServices.GetListRecipes().Where(x => x.IsActive == true);
        //    var objCat = recipeCat.Where(x => x.CategoryName == category);

        //    if (objCat.Any() && objCat.FirstOrDefault() != null)
        //    {
        //        recipeList = recipeList.Where(x => x.CategoryId == objCat.FirstOrDefault().CategoryId);
        //    }
        //    //if (recipeList.Any())
        //    //{  
        //    //    //lstRecipeViewModel = recipeList.Select(x => new RecipeViewModel
        //    //    //{
        //    //    //    RecipeId = x.RecipeId,
        //    //    //    Title = x.Title,
        //    //    //    Tags = x.Tags,
        //    //    //    CategoryId = x.CategoryId,
        //    //    //    CategoryName = x.RecipeCategory.CategoryName,
        //    //    //    EmpoyeeId = x.EmpoyeeId,
        //    //    //    ImageName = x.RecipePhotos.FirstOrDefault(),
        //    //    //    Ingredientes = x.Ingredientes,
        //    //    //    IsActive = x.IsActive,
        //    //    //    Preparation = x.Preparation
        //    //    //});
        //    //}
        //    //else
        //    //{

        //    //    lstRecipeViewModel = newRecipeList.Select(x => new RecipeViewModel
        //    //    {
        //    //        RecipeId = x.RecipeId,
        //    //        Title = x.Title,
        //    //        Tags = x.Tags,
        //    //        CategoryId = x.CategoryId,
        //    //        CategoryName = x.RecipeCategory.CategoryName,
        //    //        EmpoyeeId = x.EmpoyeeId,
        //    //        ImageName = x.RecipePhotos.FirstOrDefault(),
        //    //        Ingredientes = x.Ingredientes,
        //    //        IsActive = x.IsActive,
        //    //        Preparation = x.Preparation
        //    //    });
        //    //}

        //    ViewBag.lstRecipesCategory = recipeCat;
        //    ViewBag.Name = category;

        //    if (!string.IsNullOrEmpty(subcategory))
        //    {
        //        ViewBag.SubCategory = subcategory.Replace("_", " ");
        //    }
        //    const int pageSize = 12;
        //    var pageNumber = (page ?? 1);
        //    return View(recipeList.ToPagedList(pageNumber, pageSize));
        //}

        //[Route("Receitas/{id}")]
        //public ActionResult ReceitasDetails(string id)
        //{
        //    ViewBag.Msg = TempData["SuccessMsg"];
        //    RecipeViewModel recipeViewModel = new RecipeViewModel();
        //    var recipeCat = _recipesServices.GetRecipeCategories().Where(x => x.IsActive == true);

        //    if (string.IsNullOrEmpty(id)) return View(recipeViewModel);
        //    var receip = _recipesServices.GetReceipeBySlugTitle(id);
        //    if (receip == null) return View(recipeViewModel);
        //    recipeViewModel.Title = receip.Title;
        //    recipeViewModel.CategoryList = recipeCat.ToList();
        //    recipeViewModel.ImageName = receip.RecipePhotos.FirstOrDefault();
        //    recipeViewModel.Ingredientes = receip.Ingredientes;
        //    recipeViewModel.Preparation = receip.Preparation;
        //    recipeViewModel.Tags = receip.Tags;
        //    recipeViewModel.EmpoyeeId = receip.EmpoyeeId;
        //    recipeViewModel.RecipeId = receip.RecipeId;
        //    recipeViewModel.SlugTitle = receip.SlugTitle;
        //    recipeViewModel.CommentsList = _generalCommentService.GetAllCommentsBySectionId(receip.RecipeId, "Receitas");

        //    if (receip.LoginId != Guid.Empty)
        //    {
        //        var loginId = new Guid(receip.LoginId.ToString());
        //        var user = _userService.Find(loginId);
        //        if (user != null)
        //        {
        //            if (user.Member != null)
        //            {
        //                recipeViewModel.UserName = user.Member.MemberName;
        //            }
        //            recipeViewModel.Avatar = user.Avatar;
        //        }
        //    }
        //    return View(recipeViewModel);
        //}
        //#endregion

        //#region News
        //[OutputCache(Duration = 900, VaryByParam = "*")]
        //public ActionResult Humor(int? page, string subCategory, string category, int? Id)
        //{
        //    IEnumerable<NewsViewModel> listNewsViewModel = null;
        //    var list = _newsService.GetNewsByKeyword(category);
        //    var news_category = _newsService.GetCategoryList().Where(x => x.LookUpNewsCategoryId == 6);
        //    if (news_category.Any() && string.IsNullOrEmpty(category))
        //    {
        //        var all_sub_cat = news_category.FirstOrDefault().LookUpNewsSubCategories.Select(z => z.LookUpNewsSubCategoryId).ToArray();
        //        list = list.Where(x => all_sub_cat.Contains(x.LookUpNewsSubCategoryId));
        //    }
        //    ViewBag.Name = category;
        //    ViewBag.SubCategory = subCategory;

        //    if (!string.IsNullOrEmpty(subCategory))
        //    {
        //        list = _newsService.GetNewsBySubCategory(subCategory);
        //    }

        //    if (list.Any())
        //    {
        //        listNewsViewModel = list.Select(x => new NewsViewModel
        //        {
        //            NewsId = x.NewsId,
        //            Title = x.Title,
        //            Summary = x.Summary,
        //            Detail = x.Detail,
        //            Photo = x.NewsPhotos.FirstOrDefault(),
        //            LookUpNewsSubCategoryId = x.LookUpNewsSubCategoryId,
        //            DateFrom = x.DateFrom,
        //            SubCategoryName = x.NewsSubCategory.LookUpNewsSubCategoryName,
        //            TitleUrl = x.TitleUrl
        //        });
        //    }
        //    const int pageSize = 15;
        //    var pageNumber = (page ?? 1);

        //    if (Id == null || !(Id > 0)) return View(listNewsViewModel.ToPagedList(pageNumber, pageSize));
        //    ViewBag.NewsId = Convert.ToInt32(Id);
        //    _newsService.UpdateNewsView(Convert.ToInt32(Id));
        //    if (listNewsViewModel != null)
        //        listNewsViewModel = listNewsViewModel.Where(x => x.NewsId == Convert.ToInt32(Id));

        //    return View(listNewsViewModel.ToPagedList(pageNumber, pageSize));
        //}

        //public ActionResult GetHumor(string position, string take, bool sponsered)
        //{
        //    int takeNews = 4;
        //    if (!string.IsNullOrEmpty(take))
        //    {
        //        takeNews = Convert.ToInt32(take);
        //    }
        //    IEnumerable<NewsViewModel> listNewsViewModel = null;
        //    var list = _newsService.GetNewsList().OrderByDescending(x => x.DateFrom).Take(takeNews);
        //    if (sponsered)
        //    {
        //        list = _newsService.GetSponseredNewsList().OrderByDescending(x => x.DateFrom).OrderBy(r => Guid.NewGuid()).Take(takeNews);
        //    }
        //    if (list.Any())
        //    {
        //        listNewsViewModel = list.Select(x => new NewsViewModel
        //        {
        //            NewsId = x.NewsId,
        //            Title = x.Title,
        //            Summary = x.Summary,
        //            Detail = x.Detail,
        //            Photo = x.NewsPhotos.FirstOrDefault(),
        //            LookUpNewsSubCategoryId = x.LookUpNewsSubCategoryId,
        //            DateFrom = x.DateFrom,
        //            SubCategoryName = x.NewsSubCategory.LookUpNewsSubCategoryName,
        //            Position = position,
        //            TitleUrl = x.TitleUrl,
        //            CategoryName=x.NewsSubCategory.LookUpNewsCategory.LookUpNewsCategoryName
        //        });
        //    }
        //    return PartialView("_Humor", listNewsViewModel);
        //}

        //[Route("News/{id}")]
        //public ActionResult categoryNews(string id)
        //{
        //    ViewBag.Msg = TempData["SuccessMsg"];
        //    NewsViewModel model = new NewsViewModel();
        //    string title = id;
        //    var news = _newsService.GetNewsByTitleUrl(id);
        //    if (news == null) return View(model);

        //    news.Viewed = news.Viewed + 1;
        //    _newsService.Update(news);

        //    model.Photo = news.NewsPhotos.FirstOrDefault();
        //    model.Detail = news.Detail;
        //    model.Title = news.Title;
        //    model.TitleUrl = news.TitleUrl;
        //    model.Tags = news.Tags;
        //    model.NewsId = news.NewsId;
        //    model.PhotosList = news.NewsPhotos.ToList();
        //    model.DateFrom = news.DateFrom;
        //    model.CategoryName = news.NewsSubCategory.LookUpNewsCategory.LookUpNewsCategoryName;
        //    model.CommentsList = _generalCommentService.GetAllCommentsBySectionId(news.NewsId, "News");
        //    if (news.LoginId != Guid.Empty)
        //    {
        //        var loginId = new Guid(news.LoginId.ToString());
        //        var user = _userService.Find(loginId);
        //        if (user != null)
        //        {
        //            model.UserName = user.LoginName;
        //            model.Avatar = user.Avatar;
        //        }
        //    }
        //    var fregusia = _cityService.GetCityList().FirstOrDefault(x => x.LookUpCityId == news.FId);
        //    if (fregusia != null)
        //    {
        //        model.FregusiaName = fregusia.LookUpCityName;
        //    }
        //    var country = _countryService.GetActiveCountiesForFooter().ToList().FirstOrDefault(x => x.CountryId == news.LocationCountry);
        //    if (country != null)
        //    {
        //        model.CountryName = country.CountryName;
        //    }
        //    var distrito = _stateService.GetStateById(Convert.ToInt32(news.StateId));
        //    if (distrito != null)
        //    {
        //        model.DistritoName = distrito.LookUpStateName;
        //    }

        //    var concelho = _provinceService.GetProvinceList().FirstOrDefault(x => x.LookUpProvinceId == news.CId);
        //    if (concelho != null)
        //    {
        //        model.ConcelhoName = concelho.LookUpProvinceName;
        //    }
        //    return View("NewsDetails", model);
        //}

        //[Route("Saúde")] 
        //public ActionResult Saúde(int? page, string subCategory, string category = "Saúde")
        //{
        //    ViewBag.Name = category;
        //    ViewBag.SubCategory = subCategory;
        //    ViewBag.SubCategories = _newsService.GetSubCategoryList().Where(x => x.LookUpNewsCategory.LookUpNewsCategoryName == category).ToList();
        //    IEnumerable<NewsViewModel> listNewsViewModel = null;
        //    var list = _newsService.GetNewsByCategory(category);
        //    if (!string.IsNullOrEmpty(subCategory))
        //    {
        //        list = _newsService.GetNewsBySubCategory(subCategory);
        //    }

        //    if (list.Any())
        //    {
        //        listNewsViewModel = list.Select(x => new NewsViewModel
        //        {
        //            NewsId = x.NewsId,
        //            Title = x.Title,
        //            Summary = x.Summary,
        //            Detail = x.Detail,
        //            Photo = x.NewsPhotos.FirstOrDefault(),
        //            TitleUrl = x.TitleUrl,
        //            CategoryName=category
        //        }).OrderByDescending(x => x.DateFrom).ToList(); ;
        //    }
        //    int pageSize = 10;
        //    int pageNumber = (page ?? 1);
        //    return View(listNewsViewModel.ToPagedList(pageNumber, pageSize));
        //}

        //[Route("Saúde/{id}")]
        //public ActionResult SaúdeDetails(string id)
        //{
        //    ViewBag.Msg = TempData["SuccessMsg"];
        //    NewsViewModel model = new NewsViewModel();
        //    string title = id;
        //    var news = _newsService.GetNewsByTitleUrl(id);
        //    if (news == null) return View(model);

        //    news.Viewed = news.Viewed + 1;
        //    _newsService.Update(news);

        //    model.Photo = news.NewsPhotos.FirstOrDefault();
        //    model.Detail = news.Detail;
        //    model.Title = news.Title;
        //    model.TitleUrl = news.TitleUrl;
        //    model.Tags = news.Tags;
        //    model.NewsId = news.NewsId;
        //    model.PhotosList = news.NewsPhotos.ToList();
        //    model.DateFrom = news.DateFrom;
        //    model.CategoryName = "Saúde";
        //    model.CommentsList = _generalCommentService.GetAllCommentsBySectionId(news.NewsId, "News");
        //    if (news.LoginId != Guid.Empty)
        //    {
        //        var loginId = new Guid(news.LoginId.ToString());
        //        var user = _userService.Find(loginId);
        //        if (user != null)
        //        {
        //            model.UserName = user.LoginName;
        //            model.Avatar = user.Avatar;
        //        }
        //    }
        //    var fregusia = _cityService.GetCityList().FirstOrDefault(x => x.LookUpCityId == news.FId);
        //    if (fregusia != null)
        //    {
        //        model.FregusiaName = fregusia.LookUpCityName;
        //    }
        //    var country = _countryService.GetActiveCountiesForFooter().ToList().FirstOrDefault(x => x.CountryId == news.LocationCountry);
        //    if (country != null)
        //    {
        //        model.CountryName = country.CountryName;
        //    }
        //    var distrito = _stateService.GetStateById(Convert.ToInt32(news.StateId));
        //    if (distrito != null)
        //    {
        //        model.DistritoName = distrito.LookUpStateName;
        //    }

        //    var concelho = _provinceService.GetProvinceList().FirstOrDefault(x => x.LookUpProvinceId == news.CId);
        //    if (concelho != null)
        //    {
        //        model.ConcelhoName = concelho.LookUpProvinceName;
        //    }
        //    return View("NewsDetails", model);
        //}


        //[Route("Tecnologia")] 
        //public ActionResult Tecnologia(int? page, string subCategory, string category = "Tecnologia")
        //{
        //    ViewBag.Name = category;
        //    ViewBag.SubCategory = subCategory;
        //    ViewBag.SubCategories = _newsService.GetSubCategoryList().Where(x => x.LookUpNewsCategory.LookUpNewsCategoryName == category).ToList();
        //    IEnumerable<NewsViewModel> listNewsViewModel = null;
        //    var list = _newsService.GetNewsByCategory(category);
        //    if (!string.IsNullOrEmpty(subCategory))
        //    {
        //        list = _newsService.GetNewsBySubCategory(subCategory).OrderBy(x => Guid.NewGuid()).ToList();
        //    }

        //    if (list.Any())
        //    {
        //        listNewsViewModel = list.Select(x => new NewsViewModel
        //        {
        //            NewsId = x.NewsId,
        //            Title = x.Title,
        //            Summary = x.Summary,
        //            Detail = x.Detail,
        //            Photo = x.NewsPhotos.FirstOrDefault(),
        //            TitleUrl = x.TitleUrl,
        //            CategoryName=category
        //        }).OrderByDescending(x => x.DateFrom).ToList();
        //    }
        //    int pageSize = 10;
        //    int pageNumber = (page ?? 1);
        //    return View(listNewsViewModel.ToPagedList(pageNumber, pageSize));
        //}



        //[Route("Tecnologia/{id}")]
        //public ActionResult TecnologiaDetails(string id)
        //{
        //    ViewBag.Msg = TempData["SuccessMsg"];
        //    NewsViewModel model = new NewsViewModel();
        //    string title = id;
        //    var news = _newsService.GetNewsByTitleUrl(id);
        //    if (news == null) return View(model);

        //    news.Viewed = news.Viewed + 1;
        //    _newsService.Update(news);

        //    model.Photo = news.NewsPhotos.FirstOrDefault();
        //    model.Detail = news.Detail;
        //    model.Title = news.Title;
        //    model.TitleUrl = news.TitleUrl;
        //    model.Tags = news.Tags;
        //    model.NewsId = news.NewsId;
        //    model.PhotosList = news.NewsPhotos.ToList();
        //    model.DateFrom = news.DateFrom;
        //    model.CategoryName = "Tecnologia";
        //    model.CommentsList = _generalCommentService.GetAllCommentsBySectionId(news.NewsId, "News");
        //    if (news.LoginId != Guid.Empty)
        //    {
        //        var loginId = new Guid(news.LoginId.ToString());
        //        var user = _userService.Find(loginId);
        //        if (user != null)
        //        {
        //            model.UserName = user.LoginName;
        //            model.Avatar = user.Avatar;
        //        }
        //    }
        //    var fregusia = _cityService.GetCityList().FirstOrDefault(x => x.LookUpCityId == news.FId);
        //    if (fregusia != null)
        //    {
        //        model.FregusiaName = fregusia.LookUpCityName;
        //    }
        //    var country = _countryService.GetActiveCountiesForFooter().ToList().FirstOrDefault(x => x.CountryId == news.LocationCountry);
        //    if (country != null)
        //    {
        //        model.CountryName = country.CountryName;
        //    }
        //    var distrito = _stateService.GetStateById(Convert.ToInt32(news.StateId));
        //    if (distrito != null)
        //    {
        //        model.DistritoName = distrito.LookUpStateName;
        //    }

        //    var concelho = _provinceService.GetProvinceList().FirstOrDefault(x => x.LookUpProvinceId == news.CId);
        //    if (concelho != null)
        //    {
        //        model.ConcelhoName = concelho.LookUpProvinceName;
        //    }
        //    return View("NewsDetails", model);
        //}

        ///// <summary>
        ///// Dinheiro used to show the Dinheiro category news 
        ///// </summary>
        ///// <param name="page">page number</param>
        ///// <param name="subCategory">subcategory name</param>
        ///// <param name="category">Category Name</param>
        ///// <returns>View with results</returns>
        //[Route("Dinheiro")] 
        //public ActionResult Dinheiro(int? page, string subCategory, string category = "Dinheiro")
        //{
        //    ViewBag.Name = category;
        //    ViewBag.SubCategory = subCategory;
        //    ViewBag.SubCategories = _newsService.GetSubCategoryList().Where(x => x.LookUpNewsCategory.LookUpNewsCategoryName == category && x.IsActive).ToList();
        //    IEnumerable<NewsViewModel> listNewsViewModel = null;
        //    var list = _newsService.GetNewsByCategory(category).Where(x => x.Approved && x.IsActive);
        //    if (!string.IsNullOrEmpty(subCategory))
        //    {
        //        list = _newsService.GetNewsBySubCategory(subCategory);
        //    }

        //    if (list.Any())
        //    {
        //        listNewsViewModel = list.Select(x => new NewsViewModel
        //        {
        //            NewsId = x.NewsId,
        //            Title = x.Title,
        //            Summary = x.Summary,
        //            Detail = x.Detail,
        //            Photo = x.NewsPhotos.FirstOrDefault(),
        //            TitleUrl = x.TitleUrl,
        //            CategoryName=category
        //        }).OrderByDescending(x => x.DateFrom).ToList();
        //    }
        //    int pageSize = 10;
        //    int pageNumber = (page ?? 1);
        //    return View(listNewsViewModel.ToPagedList(pageNumber, pageSize));
        //}

        ///// <summary>
        ///// DinheiroDetails used to show the details of dinheiro
        ///// </summary>
        ///// <param name="id">id</param>
        ///// <returns>view</returns>
        //[Route("Dinheiro/{id}")]
        //public ActionResult DinheiroDetails(string id)
        //{
        //    ViewBag.Msg = TempData["SuccessMsg"];
        //    NewsViewModel model = new NewsViewModel();
        //    string title = id;
        //    var news = _newsService.GetNewsByTitleUrl(id);
        //    if (news == null) return View(model);

        //    news.Viewed = news.Viewed + 1;
        //    _newsService.Update(news);

        //    model.Photo = news.NewsPhotos.FirstOrDefault();
        //    model.Detail = news.Detail;
        //    model.Title = news.Title;
        //    model.TitleUrl = news.TitleUrl;
        //    model.Tags = news.Tags;
        //    model.NewsId = news.NewsId;
        //    model.PhotosList = news.NewsPhotos.ToList();
        //    model.DateFrom = news.DateFrom;
        //    model.CategoryName = "Dinheiro";
        //    model.CommentsList = _generalCommentService.GetAllCommentsBySectionId(news.NewsId, "News");
        //    if (news.LoginId != Guid.Empty)
        //    {
        //        var loginId = new Guid(news.LoginId.ToString());
        //        var user = _userService.Find(loginId);
        //        if (user != null)
        //        {
        //            model.UserName = user.LoginName;
        //            model.Avatar = user.Avatar;
        //        }
        //    }
        //    var fregusia = _cityService.GetCityList().FirstOrDefault(x => x.LookUpCityId == news.FId);
        //    if (fregusia != null)
        //    {
        //        model.FregusiaName = fregusia.LookUpCityName;
        //    }
        //    var country = _countryService.GetActiveCountiesForFooter().ToList().FirstOrDefault(x => x.CountryId == news.LocationCountry);
        //    if (country != null)
        //    {
        //        model.CountryName = country.CountryName;
        //    }
        //    var distrito = _stateService.GetStateById(Convert.ToInt32(news.StateId));
        //    if (distrito != null)
        //    {
        //        model.DistritoName = distrito.LookUpStateName;
        //    }

        //    var concelho = _provinceService.GetProvinceList().FirstOrDefault(x => x.LookUpProvinceId == news.CId);
        //    if (concelho != null)
        //    {
        //        model.ConcelhoName = concelho.LookUpProvinceName;
        //    }
        //    return View("NewsDetails", model);
        //}


        ///// <summary>
        ///// Mundo used to show the details of Mundo
        ///// </summary>
        ///// <param name="id">id</param>
        ///// <returns>view</returns>
        //[Route("Mundo/{id}")]
        //public ActionResult Mundo(string id)
        //{
        //    ViewBag.Msg = TempData["SuccessMsg"];
        //    NewsViewModel model = new NewsViewModel();
        //    string title = id;
        //    var news = _newsService.GetNewsByTitleUrl(id);
        //    if (news == null) return View(model);

        //    news.Viewed = news.Viewed + 1;
        //    _newsService.Update(news);

        //    model.Photo = news.NewsPhotos.FirstOrDefault();
        //    model.Detail = news.Detail;
        //    model.Title = news.Title;
        //    model.TitleUrl = news.TitleUrl;
        //    model.Tags = news.Tags;
        //    model.NewsId = news.NewsId;
        //    model.PhotosList = news.NewsPhotos.ToList();
        //    model.DateFrom = news.DateFrom;
        //    model.CategoryName = "Mundo";
        //    model.CommentsList = _generalCommentService.GetAllCommentsBySectionId(news.NewsId, "News");
        //    if (news.LoginId != Guid.Empty)
        //    {
        //        var loginId = new Guid(news.LoginId.ToString());
        //        var user = _userService.Find(loginId);
        //        if (user != null)
        //        {
        //            model.UserName = user.LoginName;
        //            model.Avatar = user.Avatar;
        //        }
        //    }
        //    var fregusia = _cityService.GetCityList().FirstOrDefault(x => x.LookUpCityId == news.FId);
        //    if (fregusia != null)
        //    {
        //        model.FregusiaName = fregusia.LookUpCityName;
        //    }
        //    var country = _countryService.GetActiveCountiesForFooter().ToList().FirstOrDefault(x => x.CountryId == news.LocationCountry);
        //    if (country != null)
        //    {
        //        model.CountryName = country.CountryName;
        //    }
        //    var distrito = _stateService.GetStateById(Convert.ToInt32(news.StateId));
        //    if (distrito != null)
        //    {
        //        model.DistritoName = distrito.LookUpStateName;
        //    }

        //    var concelho = _provinceService.GetProvinceList().FirstOrDefault(x => x.LookUpProvinceId == news.CId);
        //    if (concelho != null)
        //    {
        //        model.ConcelhoName = concelho.LookUpProvinceName;
        //    }
        //    return View("NewsDetails", model);
        //}
        ///// <summary>
        ///// Atualidade used to show the details of Atualidade
        ///// </summary>
        ///// <param name="id">id</param>
        ///// <returns>view</returns>
        //[Route("Atualidade/{id}")]
        //public ActionResult Atualidade(string id)
        //{
        //    ViewBag.Msg = TempData["SuccessMsg"];
        //    NewsViewModel model = new NewsViewModel();
        //    string title = id;
        //    var news = _newsService.GetNewsByTitleUrl(id);
        //    if (news == null) return View(model);

        //    news.Viewed = news.Viewed + 1;
        //    _newsService.Update(news);

        //    model.Photo = news.NewsPhotos.FirstOrDefault();
        //    model.Detail = news.Detail;
        //    model.Title = news.Title;
        //    model.TitleUrl = news.TitleUrl;
        //    model.Tags = news.Tags;
        //    model.NewsId = news.NewsId;
        //    model.PhotosList = news.NewsPhotos.ToList();
        //    model.DateFrom = news.DateFrom;
        //    model.CategoryName = "Atualidade";
        //    model.CommentsList = _generalCommentService.GetAllCommentsBySectionId(news.NewsId, "News");
        //    if (news.LoginId != Guid.Empty)
        //    {
        //        var loginId = new Guid(news.LoginId.ToString());
        //        var user = _userService.Find(loginId);
        //        if (user != null)
        //        {
        //            model.UserName = user.LoginName;
        //            model.Avatar = user.Avatar;
        //        }
        //    }
        //    var fregusia = _cityService.GetCityList().FirstOrDefault(x => x.LookUpCityId == news.FId);
        //    if (fregusia != null)
        //    {
        //        model.FregusiaName = fregusia.LookUpCityName;
        //    }
        //    var country = _countryService.GetActiveCountiesForFooter().ToList().FirstOrDefault(x => x.CountryId == news.LocationCountry);
        //    if (country != null)
        //    {
        //        model.CountryName = country.CountryName;
        //    }
        //    var distrito = _stateService.GetStateById(Convert.ToInt32(news.StateId));
        //    if (distrito != null)
        //    {
        //        model.DistritoName = distrito.LookUpStateName;
        //    }

        //    var concelho = _provinceService.GetProvinceList().FirstOrDefault(x => x.LookUpProvinceId == news.CId);
        //    if (concelho != null)
        //    {
        //        model.ConcelhoName = concelho.LookUpProvinceName;
        //    }
        //    return View("NewsDetails", model);
        //}


        ///// <summary>
        ///// Entretenimento used to show the details of dinheiro
        ///// </summary>
        ///// <param name="id">id</param>
        ///// <returns>view</returns>
        //[Route("Entretenimento/{id}")]
        //public ActionResult Entretenimento(string id)
        //{
        //    ViewBag.Msg = TempData["SuccessMsg"];
        //    NewsViewModel model = new NewsViewModel();
        //    string title = id;
        //    var news = _newsService.GetNewsByTitleUrl(id);
        //    if (news == null) return View(model);

        //    news.Viewed = news.Viewed + 1;
        //    _newsService.Update(news);

        //    model.Photo = news.NewsPhotos.FirstOrDefault();
        //    model.Detail = news.Detail;
        //    model.Title = news.Title;
        //    model.TitleUrl = news.TitleUrl;
        //    model.Tags = news.Tags;
        //    model.NewsId = news.NewsId;
        //    model.PhotosList = news.NewsPhotos.ToList();
        //    model.DateFrom = news.DateFrom;
        //    model.CategoryName = "Entretenimento";
        //    model.CommentsList = _generalCommentService.GetAllCommentsBySectionId(news.NewsId, "News");
        //    if (news.LoginId != Guid.Empty)
        //    {
        //        var loginId = new Guid(news.LoginId.ToString());
        //        var user = _userService.Find(loginId);
        //        if (user != null)
        //        {
        //            model.UserName = user.LoginName;
        //            model.Avatar = user.Avatar;
        //        }
        //    }
        //    var fregusia = _cityService.GetCityList().FirstOrDefault(x => x.LookUpCityId == news.FId);
        //    if (fregusia != null)
        //    {
        //        model.FregusiaName = fregusia.LookUpCityName;
        //    }
        //    var country = _countryService.GetActiveCountiesForFooter().ToList().FirstOrDefault(x => x.CountryId == news.LocationCountry);
        //    if (country != null)
        //    {
        //        model.CountryName = country.CountryName;
        //    }
        //    var distrito = _stateService.GetStateById(Convert.ToInt32(news.StateId));
        //    if (distrito != null)
        //    {
        //        model.DistritoName = distrito.LookUpStateName;
        //    }

        //    var concelho = _provinceService.GetProvinceList().FirstOrDefault(x => x.LookUpProvinceId == news.CId);
        //    if (concelho != null)
        //    {
        //        model.ConcelhoName = concelho.LookUpProvinceName;
        //    }
        //    return View("NewsDetails", model);
        //}


        //public ActionResult GetSaude(string category, string subCategory)
        //{
        //    IEnumerable<NewsViewModel> listNewsViewModel = null;
        //    IEnumerable<News> listNews = null;
        //    if (!string.IsNullOrEmpty(subCategory))
        //    {
        //        listNews = _newsService.GetNewsBySubCategoryContains(subCategory).OrderBy(r => Guid.NewGuid());
        //    }
        //    else { listNews = _newsService.GetNewsByKeywordForHome(category).Where(x=>x.Approved && x.IsActive).OrderBy(r => Guid.NewGuid()); }
        //    if (listNews.Any())
        //    {
        //        listNewsViewModel = listNews.Select(x => new NewsViewModel
        //        {
        //            NewsId = x.NewsId,
        //            Title = x.Title,
        //            Summary = x.Summary,
        //            Detail = x.Detail,
        //            Photo = x.NewsPhotos.FirstOrDefault(),
        //            LookUpNewsSubCategoryId = x.LookUpNewsSubCategoryId,
        //            DateFrom = x.DateFrom,
        //            SubCategoryName = x.NewsSubCategory.LookUpNewsSubCategoryName,
        //            TitleUrl = x.TitleUrl,
        //            CategoryName = category
        //        }).Take(3).ToList();
        //    }
        //    return PartialView("_Saude", listNewsViewModel);
        //}
        //public ActionResult DistritoSection(int id)
        //{
        //    var model = new DistritoNewsViewModel { DistritoId = id };
        //    if (id > 0)
        //    {
        //        var distritoName = _stateService.GetStateList().FirstOrDefault(x => x.LookUpStateId == id);
        //        if (distritoName != null)
        //        {
        //            model.DistritoName = distritoName.LookUpStateName;
        //        }
        //    }

        //    var list = _newsService.GetDistritoNews(id).Take(6).ToList();
        //    model.NewsList = list.ToList();

        //    var classifiedList = _classifiedServices.GetClassifiedForDistrito(id).Take(6).ToList();
        //    model.ClassifiedsList = classifiedList;
        //    model.EventList = _eventService.GetEventListWithImages().Where(x => x.StateId == id).ToList();
        //    return View(model);
        //}

        //public ActionResult AllDistritoNews(int id)
        //{
        //    var model = new DistritoNewsViewModel();
        //    if (id > 0)
        //    {
        //        var distritoName = _stateService.GetStateList().FirstOrDefault(x => x.LookUpStateId == id);
        //        if (distritoName != null)
        //        {
        //            model.DistritoName = distritoName.LookUpStateName;
        //        }
        //    }

        //    var list = _newsService.GetDistritoNews(id).ToList();
        //    model.NewsList = list.ToList();
        //    return View(model);
        //}

        //public ActionResult NewsDetails(string id)
        //{
        //    ViewBag.Msg = TempData["SuccessMsg"];
        //    NewsViewModel model = new NewsViewModel();
        //    string title = id;
        //    var news = _newsService.GetNewsByTitleUrl(id);
        //    if (news == null) return View(model);

        //    news.Viewed = news.Viewed + 1;
        //    _newsService.Update(news);

        //    model.Photo = news.NewsPhotos.FirstOrDefault();
        //    model.Detail = news.Detail;
        //    model.Title = news.Title;
        //    model.TitleUrl = news.TitleUrl;
        //    model.Tags = news.Tags;
        //    model.NewsId = news.NewsId;
        //    model.PhotosList = news.NewsPhotos.ToList();
        //    model.DateFrom = news.DateFrom;
        //    model.CategoryName = news.NewsSubCategory.LookUpNewsCategory.LookUpNewsCategoryName;
        //    model.CommentsList = _generalCommentService.GetAllCommentsBySectionId(news.NewsId, "News");
        //    if (news.LoginId != Guid.Empty)
        //    {
        //        var loginId = new Guid(news.LoginId.ToString());
        //        var user = _userService.Find(loginId);
        //        if (user != null)
        //        {
        //            model.UserName = user.LoginName;
        //            model.Avatar = user.Avatar;
        //        }
        //    }
        //    var fregusia = _cityService.GetCityList().FirstOrDefault(x => x.LookUpCityId == news.FId);
        //    if (fregusia != null)
        //    {
        //        model.FregusiaName = fregusia.LookUpCityName;
        //    }
        //    var country = _countryService.GetActiveCountiesForFooter().ToList().FirstOrDefault(x => x.CountryId == news.LocationCountry);
        //    if (country != null)
        //    {
        //        model.CountryName = country.CountryName;
        //    }
        //    var distrito = _stateService.GetStateById(Convert.ToInt32(news.StateId));
        //    if (distrito != null)
        //    {
        //        model.DistritoName = distrito.LookUpStateName;
        //    }

        //    var concelho = _provinceService.GetProvinceList().FirstOrDefault(x => x.LookUpProvinceId == news.CId);
        //    if (concelho != null)
        //    {
        //        model.ConcelhoName = concelho.LookUpProvinceName;
        //    }
        //    return View(model);
        //}        

        //#endregion
        //[OutputCache(Duration = 60, VaryByParam = "*")]
        //public ActionResult GetReceipe()
        //{
        //    IEnumerable<RecipeViewModel> lstRecipeViewModel = null;
        //    var recipe_list = _recipesServices.GetListRecipes().OrderBy(r => Guid.NewGuid()).Where(x => x.IsActive == true);
        //    if (recipe_list.Any())
        //    {
        //        lstRecipeViewModel = recipe_list.Select(x => new RecipeViewModel
        //        {
        //            RecipeId = x.RecipeId,
        //            Title = x.Title,
        //            Tags = x.Tags,
        //            CategoryId = x.CategoryId,
        //            CategoryName = x.RecipeCategory.CategoryName,
        //            EmpoyeeId = x.EmpoyeeId,
        //            ImageName = x.RecipePhotos.FirstOrDefault(),
        //            Ingredientes = x.Ingredientes,
        //            IsActive = x.IsActive,
        //            Preparation = x.Preparation,
        //            SlugTitle=x.SlugTitle
        //        }).Take(4).ToList();
        //    }
        //    return PartialView("_Receitas", lstRecipeViewModel);
        //}
        //[OutputCache(Duration = 60, VaryByParam = "*")]
        //public ActionResult GetEvents()
        //{
        //    IEnumerable<EventViewModel> lstEventViewModel = null;
        //    var event_list = _eventService.GetEventListWithImages().OrderByDescending(x => x.DateFrom).Take(4).ToList();
        //    if (event_list.Any())
        //    {
        //        lstEventViewModel = event_list.Select(x => new EventViewModel
        //        {
        //            EzEventsId = x.EzEventsId,
        //            Title = x.Title,
        //            Tags = x.Tags,
        //            Category = x.Category,
        //            EzPhoto = x.EventPhotos.FirstOrDefault(),
        //            Detail = x.Detail,
        //            IsActive = x.IsActive,
        //            Summary = x.Summary,
        //            CategoryName = x.EventCategory.LookUpEZCategoryTitle,
        //            SlugTitle=x.SlugTitle
        //        });
        //    }
        //    return PartialView("_Eventos", lstEventViewModel);
        //}

        //public ActionResult GetComments()
        //{
        //    IEnumerable<CommentViewModel> lstComments = null;
        //    var commentsList = _generalCommentService.GetAllComments().Where(x => x.Section != "Event").OrderByDescending(x=>x.CreatedDate).Take(6).ToList();
        //    if (commentsList.Any())
        //    {
        //        lstComments = commentsList.Select(x => new CommentViewModel
        //        {
        //            CommentId = x.CommentsId,
        //            Name = x.Name,
        //            Comments = x.UserComments,
        //            Email = x.Email,
        //            Section = x.Section,
        //            SectionId = x.SectionId,
        //            Title = x.SlugTitle,
        //            CategoryName=x.CategoryName
        //        });
        //    }
        //    return PartialView("_Comments", lstComments);
        //}

        //public ActionResult CommentsDetails(int id)
        //{
        //    //CommentViewModel objMedel = new CommentViewModel();
        //    //var comments = _generalCommentService.GetCommentsById(id);
        //    //if (comments != null)
        //    //{
        //    //    objMedel.CommentId = comments.CommentsId;
        //    //    objMedel.Name = comments.Name;
        //    //    objMedel.Comments = comments.UserComments;
        //    //}
        //    //ViewBag.comment = objMedel;
        //    //IEnumerable<CommentViewModel> lstComments = null;
        //    //if (comments != null)
        //    //{

        //    //    //var commentsList = _generalCommentService.GetAllCommentsBySectionId(Convert.ToInt32(comments.SectionId), comments.Section);
        //    //    if (commentsList != null)
        //    //    {
        //    //        if (commentsList.Any())
        //    //        {
        //    //            lstComments = commentsList.Select(x => new CommentViewModel
        //    //            {
        //    //                CommentId = x.CommentsId,
        //    //                Name = x.Name,
        //    //                Comments = x.UserComments,
        //    //                Email = x.Email,
        //    //                Section = x.Section,
        //    //                SectionId = x.SectionId
        //    //            });
        //    //        }
        //    //    }
        //   // }
        //    return View();
        //}

        //public ActionResult EmpressaDetail(string id)
        //{
        //    ViewBag.Msg = TempData["SuccessMsg"];
        //    int empressaId = 0;
        //    if (!string.IsNullOrEmpty(id))
        //    {
        //        empressaId = Convert.ToInt32(id);
        //    }

        //    var model = new CompanyViewModel();
        //    string countryName = string.Empty;
        //    var comapny = _activitiesSectorServices.GetCompanyById(empressaId);
        //    if (comapny != null)
        //    {
        //        model.LookUpCompanyName = comapny.LookUpCompanyName;
        //        model.LookUpCompanyId = comapny.LookUpCompanyId;
        //        model.Morada = comapny.Morada;
        //        model.Capital = Convert.ToString(comapny.Capital);
        //        model.CodPostal = comapny.CodPostal;
        //        model.Descricao = comapny.Descricao;
        //        model.Denominacao = comapny.Denominacao;
        //        model.WebSite = comapny.Url;
        //        model.Fax = comapny.Fax;
        //        model.Telefone = comapny.Telefone;
        //        model.Localidade = comapny.Localidade;
        //        model.CompanyLogo = comapny.CompanyLogo;
        //        model.Email = comapny.Email;
        //        if (comapny.CountryId.HasValue)
        //        {
        //            int countryId = Convert.ToInt32(comapny.CountryId);
        //            var country = _countryService.GetCountryById(countryId);
        //            if (country != null)
        //            {
        //                countryName = country.CountryName;
        //            }
        //            model.CountryName = countryName;
        //        }
        //        var concelho = _provinceService.GetProvinceList().FirstOrDefault(x => x.LookUpProvinceId == (comapny.CId ?? 0));
        //        var distrito = _stateService.GetStateById(comapny.DId ?? 0);
        //        if (concelho != null)
        //        {
        //            model.ConcelhoName = concelho.LookUpProvinceName;
        //        }
        //        if (distrito != null)
        //        {
        //            model.DistritoName = distrito.LookUpStateName;
        //        }
        //        var fregusia = _cityService.GetCityList().FirstOrDefault(x => x.LookUpCityId == comapny.FId);
        //        if (fregusia != null)
        //        {
        //            model.FregusiaName = fregusia.LookUpCityName;
        //        }
        //    }
        //    return View(model);
        //}

        //[Route("Empregos")]
        //public ActionResult Empregos(int? page, int? pageSize, string keyword, string searchByClass, string city)
        //{  
        //    var listOfClassifieds = new List<Classified>();
        //    listOfClassifieds = _classifiedServices.GetQueryableClassifieds().FilterByCategory("Emprego").OrderByDescending(x => x.PostedOn).ToList();
        //    if (!string.IsNullOrEmpty(keyword))
        //    {
        //        listOfClassifieds = _classifiedServices.GetQueryableClassifieds().FilterByCategory("Emprego").FilterByKeyword(keyword).OrderByDescending(x => x.PostedOn).ToList();
        //    }
        //    if (!string.IsNullOrEmpty(searchByClass))
        //    {
        //        listOfClassifieds = _classifiedServices.GetQueryableClassifieds().FilterByCategory("Emprego").FilterByKeyword(keyword).OrderByDescending(x => x.PostedOn).ToList();
        //    }
        //    if (!string.IsNullOrEmpty(city))
        //    {
        //        listOfClassifieds = _classifiedServices.GetQueryableClassifieds().FilterByCategory("Emprego").FilterbyFergrusia(city).OrderByDescending(x => x.PostedOn).ToList();
        //    }
        //    var model= listOfClassifieds.Select(x => new ClassifiedsViewModel
        //    {
        //        Title = x.Title,
        //        Summary = x.Summary,
        //        Detail = x.Detail,
        //        ClassifiedId = x.ClassifiedId,
        //        ClassifiedPhotos = x.ClassifiedPhotos.ToList(),
        //        DistritoName = _stateService.GetStateById(x.LocationState)!=null?_stateService.GetStateById(x.LocationState).LookUpStateName:"",
        //        PostedOn = x.PostedOn,
        //        SlugUrl=x.SlugUrl,
        //        CountryName =  _countryService.GetCountryById(x.LocationCountry)!=null?_countryService.GetCountryById(x.LocationCountry).CountryName:string.Empty
        //    }).ToList();

        //    int pageSizeNew = Convert.ToInt32(pageSize ?? 5);
        //    int pageNumber = (page ?? 1);
        //    return View(model.ToPagedList(pageNumber, pageSizeNew));
        //}

        //[Route("Empregos/{id}")]
        //public ActionResult EmpregosDetails(string id)
        //{
        //    ViewBag.Msg = TempData["SuccessMsg"];
        //    ClassifiedDetailsViewModel model = new ClassifiedDetailsViewModel();

        //    //var classifiedId = !string.IsNullOrEmpty(id) ? Convert.ToInt32(id) : 0;
        //    //var classified = _classifiedServices.GetClassifiedDetails(classifiedId);
        //    var classified = _classifiedServices.GetClassifiedBySlug(id);
        //    if (classified != null)
        //    {
        //        //classified.Views = classified.Views + 1;
        //        //_classifiedServices.Update(classified);
        //        if (classified.LoginId != new Guid("00000000-0000-0000-0000-000000000000"))
        //        {
        //            var userDetails = _userService.Find(classified.LoginId);
        //            if (userDetails != null)
        //            {
        //                if (userDetails.Member != null)
        //                {
        //                    if (userDetails.IsCompanyUser())
        //                    {
        //                        model.RoleId = RoleType.COMPANYUSER;
        //                    }
        //                    else
        //                    {
        //                        model.RoleId = RoleType.INDIVIDUALUSER;
        //                    }
        //                    model.Member = userDetails.Member;
        //                }
        //            }
        //        }
        //        model.BusniessName = classified.Business.LookUpBusinessName;
        //        var distrito = _stateService.GetStateById(Convert.ToInt32(classified.FId));
        //        if (distrito != null)
        //        {
        //            model.Distrito = distrito.LookUpStateName;
        //        }

        //        var country = _countryService.GetActiveCountiesForFooter().ToList().FirstOrDefault(x => x.CountryId == classified.LocationCountry);
        //        if (country != null)
        //        {
        //            model.Country = country.CountryName;
        //        }

        //        var concelos = _provinceService.GetProvinceList().FirstOrDefault(x => x.LookUpProvinceId == classified.CId);
        //        if (concelos != null)
        //        {
        //            model.Concelho = concelos.LookUpProvinceName;
        //        }
        //        var fregusia = _cityService.GetCityList().FirstOrDefault(x => x.LookUpCityId == classified.FId);
        //        if (fregusia != null)
        //        {
        //            model.Fregusia = fregusia.LookUpCityName;
        //        }

        //        if (classified.ClassifiedPhotos != null)
        //        {
        //            model.ClassifiedPhotos = classified.ClassifiedPhotos;
        //        }
        //        model.Title = classified.Title;
        //        model.Summary = classified.Summary;
        //        model.Price = classified.Price;
        //        model.Description = classified.Detail;
        //        model.CreateDate = classified.PostedOn;
        //        model.Viewed = classified.Views;
        //        model.ClassifiedId = classified.ClassifiedId;
        //        model.SubCategoryName = classified.SubCategory.LookUpSubCategoryTitle;
        //        model.SlugUrl = classified.SlugUrl;
        //    }
        //    return View("EmpregosDetails", model);
        //}

        //[Route("Carros")]
        //public ActionResult Carros(int? page, string category, string subCategory)
        //{
        //    var subcategories = _newsCategoryService.GetNewsSubCategoriesListByCategoryId(5).ToList();
        //    ViewBag.SubCategories = _newsCategoryService.GetNewsSubCategoriesListByCategoryId(5).ToList();
        //    var news = _newsService.GetNewsByCategory("Carros").OrderByDescending(x => x.DateFrom).AsEnumerable();
        //    if (!string.IsNullOrEmpty(subCategory))
        //    {
        //        news = _newsService.GetNewsBySubCategory(subCategory).OrderByDescending(x => x.DateFrom).AsEnumerable();
        //    }
        //    const int pageSize = 8;
        //    var pageNumber = (page ?? 1);
        //    return View(news.ToPagedList(pageNumber, pageSize));
        //}

        //[Route("Carros/{id}")]
        //public ActionResult CarrosDetails(string id)
        //{
        //    ViewBag.Msg = TempData["SuccessMsg"];
        //    NewsViewModel model = new NewsViewModel();
        //    string title = id;
        //    var news = _newsService.GetNewsByTitleUrl(id);
        //    if (news == null) return View(model);

        //    news.Viewed = news.Viewed + 1;
        //    _newsService.Update(news);

        //    model.Photo = news.NewsPhotos.FirstOrDefault();
        //    model.Detail = news.Detail;
        //    model.Title = news.Title;
        //    model.TitleUrl = news.TitleUrl;
        //    model.Tags = news.Tags;
        //    model.NewsId = news.NewsId;
        //    model.PhotosList = news.NewsPhotos.ToList();
        //    model.DateFrom = news.DateFrom;
        //    model.CategoryName = "Carros";
        //    model.CommentsList = _generalCommentService.GetAllCommentsBySectionId(news.NewsId, "News");
        //    if (news.LoginId != Guid.Empty)
        //    {
        //        var loginId = new Guid(news.LoginId.ToString());
        //        var user = _userService.Find(loginId);
        //        if (user != null)
        //        {
        //            model.UserName = user.LoginName;
        //            model.Avatar = user.Avatar;
        //        }
        //    }
        //    var fregusia = _cityService.GetCityList().FirstOrDefault(x => x.LookUpCityId == news.FId);
        //    if (fregusia != null)
        //    {
        //        model.FregusiaName = fregusia.LookUpCityName;
        //    }
        //    var country = _countryService.GetActiveCountiesForFooter().ToList().FirstOrDefault(x => x.CountryId == news.LocationCountry);
        //    if (country != null)
        //    {
        //        model.CountryName = country.CountryName;
        //    }
        //    var distrito = _stateService.GetStateById(Convert.ToInt32(news.StateId));
        //    if (distrito != null)
        //    {
        //        model.DistritoName = distrito.LookUpStateName;
        //    }

        //    var concelho = _provinceService.GetProvinceList().FirstOrDefault(x => x.LookUpProvinceId == news.CId);
        //    if (concelho != null)
        //    {
        //        model.ConcelhoName = concelho.LookUpProvinceName;
        //    }
        //    return View("NewsDetails",model);
        //} 



        ////public ActionResult GetWeatherInformation(string cityName)
        ////{
        ////    //var city = string.Empty;
        ////    //var flagUrl = string.Empty;
        ////    //var description = string.Empty;
        ////    //var imgWeatherIcon = string.Empty;
        ////    //var tempMin = string.Empty;
        ////    //var tempMax = string.Empty;
        ////    //var tempDay = string.Empty;
        ////    //var tempNight = string.Empty;
        ////    //var humidity = string.Empty;
        ////    //var responseString = string.Empty;
        ////    //string appId = "<App Id>";
        ////    //string url = string.Format("http://api.openweathermap.org/data/2.5/forecast/daily?q={0}&units=metric&cnt=1&APPID={1}", cityName.Trim(), appId);
        ////    //using (WebClient client = new WebClient())
        ////    //{
        ////    //    string json = client.DownloadString(url);

        ////    //    WeatherModel weatherInfo = (new JavaScriptSerializer()).Deserialize<WeatherModel>(json);
        ////    //    city = weatherInfo.city.name + "," + weatherInfo.city.country;
        ////    //    flagUrl = string.Format("http://openweathermap.org/images/flags/{0}.png", weatherInfo.city.country.ToLower());
        ////    //    description = weatherInfo.list[0].weather[0].description;
        ////    //    imgWeatherIcon = string.Format("http://openweathermap.org/img/w/{0}.png", weatherInfo.list[0].weather[0].icon);
        ////    //    tempMin = string.Format("{0}°С", Math.Round(weatherInfo.list[0].temp.min, 1));
        ////    //    tempMax = string.Format("{0}°С", Math.Round(weatherInfo.list[0].temp.max, 1));
        ////    //    tempDay = string.Format("{0}°С", Math.Round(weatherInfo.list[0].temp.day, 1));
        ////    //    tempNight = string.Format("{0}°С", Math.Round(weatherInfo.list[0].temp.night, 1));
        ////    //    humidity = weatherInfo.list[0].humidity.ToString();
        ////    //}
        ////    //responseModel objResponse = new responseModel();
        ////    //objResponse.city = city;
        ////    //objResponse.flagUrl = flagUrl;
        ////    //objResponse.description = description;
        ////    //objResponse.imgWeatherIcon = imgWeatherIcon;
        ////    //objResponse.tempMin = tempMin;
        ////    //objResponse.tempMax = tempMax;
        ////    //objResponse.tempDay = tempDay;
        ////    //objResponse.tempNight = tempNight;
        ////    //return Json(objResponse, JsonRequestBehavior.AllowGet);
        ////}
        #endregion
        ILoginHelper _loginService;


        public ActionResult Index(string returnUrl)
        {
            return View();
        }

        public HomeController(ILoginHelper loginService, IUserService userService, IFormsAuthentication formsAuth, IStateService stateService, ICityService cityService,
            IClinicService clinicService, IAdminUserService adminUserService, IProfessionTypeService professionType, ICommunication Communication, IProfessionTypeService professiontypeservice, IDoctorMembershipService doctormembershipService,IDoctorService doctorservice,ICountryService countryService)
        {
            _loginService = loginService;
            _userService = userService;
            _formsAuth = formsAuth;
            _stateService = stateService;
            _cityService = cityService;
            _clinicService = clinicService;
            _adminUserService = adminUserService;
            _professionType = professionType;
            _Communication = Communication;
            _professiontypeservice = professiontypeservice;
            _doctormembershipService = doctormembershipService;
            _doctorservice = doctorservice;
            _countryService = countryService;
        }
        [AllowAnonymous]
        public ActionResult PatientDashboard()
        {
            Login model = _userService.Find(base.loginUserModel.LoginId);
            UserViewModel userModel = new UserViewModel(model);
            return View(userModel);

        }
        [AllowAnonymous]
        public ActionResult UpdatePatientDetails()
        {
            Login model = _userService.Find(base.loginUserModel.LoginId);
            UserViewModel userModel = new UserViewModel(model);
            return View(userModel);

        }
        [HttpPost]
        public ActionResult UpdatePatientDetails(UserViewModel model, HttpPostedFileBase ProfilePic)
        {
            _loginService.UpdateProfile(model, ProfilePic, base.loginUserModel.LoginId);
            return View(model);
        }
        public ActionResult Unauthorized()
        {
            return View();
        }

        //GET: /Account/RegisterDoctor---Create By dheeraj
        [AllowAnonymous]
        public ActionResult RegisterDoctor()
        {
            RegisterViewModel model = new RegisterViewModel();
            //model = BindDropDown(model);
            model.ProfessionTypes = _professiontypeservice.GetProfessionTypesList();
            model.ProfessionSubType = _professiontypeservice.GetProfessionSubTypesList();
            return View(model);
        }


        //POST: /Account/RegisterDoctor....Create by dheeraj
        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public ActionResult RegisterDoctor(RegisterViewModel model)
        {
            var userExists = _userService.CheckDuplicateUserName(model.UserName);
            if (userExists)
            {
                ModelState.AddModelError("UserName", "Username already exists.");
            }
            var userEmailExists = _userService.CheckDuplicateMail(model.Email);
            if (userEmailExists)
            {
                ModelState.AddModelError("Email", "User with this email id already exists.");
            }

            BaseMember profile = null;
            model.RoleId = RoleType.Doctor;
            profile = model.GetDoctor();
            var user = model.GetLogin(profile, model.Email);
            var random = RandomPassword.Generate(6, 8);
            user.LoginPassword = random;
            _userService.Add(user);
            string activationToken = user.ActivationToken.ToString();
            string link = ConfigurationManager.AppSettings["SitePath"] + "Account/VerifyEmail/" + activationToken;
            var fileUrl = Server.MapPath("~/EmailTemplates/RegistrationTemplate.html");
            string html = System.IO.File.ReadAllText(fileUrl);
            html = html.Replace("@@Password", random).Replace("@@ActivationLink", link);
            EmailService objEmail = new EmailService();
            objEmail.SendEmail("User Registration", html, model.Email, "tma@myonlineclinic.com.au");
            return RedirectToAction("Login");

        }

        public ActionResult RegisterPatient()
        {
            RegisterViewModel model = new RegisterViewModel();
            //model = BindDropDown(model);
            return View(model);
        }

        //POST: /Account/RegisterPatient....Create by dheeraj
        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public ActionResult RegisterPatient(RegisterViewModel model)
        {
            var userExists = _userService.CheckDuplicateUserName(model.UserName);
            if (userExists)
            {
                ModelState.AddModelError("UserName", "Username already exists.");
            }
            var userEmailExists = _userService.CheckDuplicateMail(model.Email);
            if (userEmailExists)
            {
                ModelState.AddModelError("Email", "User with this email id already exists.");
                ViewBag.ErrorMessage = "This email address already exists.";
                return View("~/Views/Home/Patient.cshtml");
                //\Views\Home\Patient.cshtml
            }
            else
            {
                //model = BindDropDown(model);
                //if (ModelState.IsValid)
                //{
                BaseMember profile = null;
                model.RoleId = RoleType.Patient;
                profile = model.GetPatient();
                var user = model.GetLogin(profile, model.Email);
                _userService.Add(user);
                string activationToken = user.ActivationToken.ToString();
                string link = ConfigurationManager.AppSettings["SitePath"] + "Account/VerifyEmail/" + activationToken;
                var fileUrl = Server.MapPath("~/EmailTemplates/PatientRegister.html");
                string html = System.IO.File.ReadAllText(fileUrl);
                html = html.Replace("@@ActivationLink", link);
                EmailService objEmail = new EmailService();
                objEmail.SendEmail("User Registration", html, model.Email, "tma@myonlineclinic.com.au");
                return RedirectToAction("Account/Login");
                // }
                //}
                //}
            }

            return View(model);
        }



        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.SuccessMsg = TempData["SuccessMsg"];
            try
            {
                HttpCookie authCookie = System.Web.HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
                if (authCookie != null)
                {
                    if (!string.IsNullOrEmpty(authCookie.Value))
                    {
                        FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
                        if (ticket.IsPersistent)
                        {
                            if (string.IsNullOrEmpty(returnUrl))
                                return RedirectToAction("Index", "Home");
                            else
                                return Redirect(returnUrl);
                        }
                    }
                }
            }
            catch (CryptographicException cex)
            {
                FormsAuthentication.SignOut();
            }

            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Home/Login
        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            try
            {
                if (ModelState.IsValid)
                {            
                    var email = StringCipher.Encrypt(model.Username);
                    if (_userService.IsUserLocked(email) == false)
                    {
                        string logo = string.Empty;
                        string organizationName = string.Empty;
                        var user = _userService.Find(email, model.Password);
                        if (user != null)
                        {
                            //if (user.LookUpRoleId == model.UserType)
                            //{
                            //if (!user.IsAdmin())
                            //{
                            if (!user.IsEmailVerified)
                            {
                                TempData["ErrorMessage"] = "Your email is not verified, Please verify your email.";
                                return View(model);
                            }
                            if (!user.IsApproved)
                            {
                                TempData["ErrorMessage"] = "Your account is in the approval process.";
                                return View(model);
                            }
                            //}

                            //if (user.IsOragnizatinAdmin())
                            //{
                            //    Organization org = new Organization();
                            //    if (user.Member != null)
                            //    {
                            //        org = ((OrganizationUsers)user.Member).organization;
                            //        logo = org.OrganizationLogo;
                            //        organizationName = org.OrganizationName;
                            //    }
                            //}
                            string avatar = string.Empty;
                            bool isAdmin = false;
                            avatar = user.Avatar;
                            string memberFullName = string.Empty;
                            memberFullName = user.Member != null ? user.Member.FirstName + " " + user.Member.SurName : user.LoginName;
                            if (user.LookUpRoleId == (int)RoleType.ADMIN)
                            {
                                isAdmin = true;
                                var staff = _adminUserService.GetAllAdminUser().Where(x => x.LoginId == user.LoginId && x.MemberId > 1).FirstOrDefault();
                                if (staff != null)
                                {
                                    isAdmin = false;
                                }
                            }
                            _formsAuth.SignIn(memberFullName, false, user.LoginRole.LookUpRoleName, user.LoginId.ToString(), avatar, logo, organizationName, isAdmin);

                            //if (user.IsAdmin())
                            //{
                            //    return RedirectToAction("index", "Dashboard", new { @area = "admin" });
                            //}
                            //else if (user.IsOragnizatinAdmin())
                            //{
                            //    return RedirectToAction("index", "Dashboard", new { @area = "Organizations" });
                            //}
                            if (user.IsDoctor())
                            {
                                //return RedirectToAction("index", "Doctor", new { @area = "Doctor" });
                               
                                    var doc = _doctorservice.GetDoctorByLoginId(user.LoginId).MemberId;

                                    var doctorMembership = _doctormembershipService.GetDoctorMembershipByDoctorId(doc);

                                    if (doctorMembership != null)
                                    {
                                        return RedirectToAction("index", "Dashboard", new { @area = "Doctor" });
                                    }
                                    else
                                    {
                                        return RedirectToAction("UpdateProfile", "Dashboard", new { @area = "Doctor" });
                                    }                            
                                //return Content("<script language='javascript' type='text/javascript'>alert('Correct Doctor');</script>");
                            }
                            else if (user.IsPatient())
                            {
                                return RedirectToAction("Index", "Dashboard", new { @area = "Patients" });
                            }
                            else if (user.IsClinicAdmin() || user.IsClinicUser())
                            {
                                return RedirectToAction("Index", "Dashboard", new { @area = "Clinic" });
                            }

                            //else if (user.IsIndividualUser())
                            //{
                            //    return RedirectToAction("index", "Dashboard", new { @area = "User" });
                            //}
                        }
                        else
                        {
                            TempData["ErrorMessage"] = "Invalid email or password, please try again.";
                            var count = _userService.FailureCount(model.Username);
                            if (count >= 3)
                            {
                                _userService.LockUser(model.Username);
                            }
                            //return new JavaScriptResult { Script = "myfunction();" };

                            //return Content("<script language='javascript' type='text/javascript'>alert('invalid email');</script>");

                            return RedirectToAction("Index", "Home");
                            //return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Your account has been locked please reset your password again to unlock your account.";
                        //return new JavaScriptResult { Script = "alert('Invalild faliure');" };
                        //return Content("<script language='javascript' type='text/javascript'>myfunction();</script>");
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            catch (CryptographicException cex)
            {
                FormsAuthentication.SignOut();
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public ActionResult Patient()
        {
            TempData["Type"] = "Patient";
            TempData.Peek("Type");
            return View();
        }
        public ActionResult Doctor()
        {
            

            TempData["Type"] = "Doctor";
            TempData.Peek("Type");
          
            return View();
        }
        [HttpGet]
        public ActionResult EquipmentEnquiry()
        {

            return View();


        }
        [HttpPost]
        public ActionResult EquipmentEnquiry(FormCollection collection, CommunicationViewModel model)
        {
            EquipmentInquery Equipment = new EquipmentInquery();
            Equipment.Email = model.Email;
            Equipment.UserName = model.UserName;
            Equipment.Message = model.Message;
            Equipment.EquipmentName = collection.Get("Equipment");
            if (TempData["Type"] == "Patient")
            {
                Equipment.UserType = "Patient";
            }
            else
            {
                Equipment.UserType = "Doctor";
            }
            _Communication.Add(Equipment);


            var fileUrl = Server.MapPath("~/EmailTemplates/Communication.html");
            string html = System.IO.File.ReadAllText(fileUrl);
            html = html.Replace("@UserName", model.UserName);
            html = html.Replace("@Email", model.Email);
            html = html.Replace("@Message", model.Message);
            EmailService objEmail = new EmailService();
            objEmail.SendEmail("Equipment Enquiry", html, "mocbookings@doctortoyou.com.au", "tma@myonlineclinic.com.au");
            return Redirect(Request.UrlReferrer.ToString());
        }
        [HttpPost]
        public ActionResult ContactUs(RegisterViewModel model)
        {
            ContectUs contact = new ContectUs();
            contact.Name = model.ContactUserName;
            contact.Email = model.ContactEmail;
            contact.Message = model.ContactMessage;
            //if (TempData["Type"] == "Patient")
            //{
            //    contact.UserType = "Patient";
            //}
            //else
            //{
            //    contact.UserType = "Doctor";
            //}

            _Communication.Add(contact);
            var fileUrl = Server.MapPath("~/EmailTemplates/Communication.html");
            string html = System.IO.File.ReadAllText(fileUrl);
            html = html.Replace("@UserName", model.ContactUserName);
            html = html.Replace("@Email", model.ContactEmail);
            html = html.Replace("@Message", model.ContactMessage);
            EmailService objEmail = new EmailService();
            objEmail.SendEmail("Contact us", html, "mocbookings@doctortoyou.com.au", "tma@myonlineclinic.com.au");
            return Redirect(Request.UrlReferrer.ToString());


        }

        public ActionResult ClinicalDashboard()
        {
            return View();
        }
        public JsonResult GetClinicDetails(int? id)
        {
            var cdetail = _clinicService.GetClinicList().Where(x => x.ClinicId == id).FirstOrDefault();
            cdetail.ClinicName = StringCipher.Decrypt(cdetail.ClinicName);
            cdetail.Address1 = StringCipher.Decrypt(cdetail.Address1);
            cdetail.Address2 = StringCipher.Decrypt(cdetail.Address2);
            cdetail.PostCode = StringCipher.Decrypt(cdetail.PostCode);
            cdetail.FaxNumber = _countryService.GetCountryList().Where(x => x.CountryId == Convert.ToInt32(cdetail.Country)).FirstOrDefault().CountryName;


            // cdetail.Country = _countryService.GetCountryList().Where(x => x.CountryId ==Convert.ToInt32(cdetail.Country));
            return Json(cdetail, JsonRequestBehavior.AllowGet);
        }
    }
}