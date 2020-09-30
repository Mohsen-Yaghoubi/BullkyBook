using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BulkyBook.Models.ViewModels;
using BullkyBook.DataAccess.Repository.IRepository;
using BullkyBook.Models;
using BullkyBook.Utillities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BulkyBook.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> productList = _unitOfWork.Product.GetAll(includeProperties: "Category,CoverType");


            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claim != null)
            {
                var count = _unitOfWork.ShoppingCard
                    .GetAll(c => c.ApplicationUserId == claim.Value)
                    .ToList().Count();

                HttpContext.Session.SetInt32(SD.ssShoppingCart, count);
            }


            return View(productList);
        }

        public IActionResult Details(int id)
        {
            var productFromDb = _unitOfWork.Product.
                        GetFirstOrDefault(u => u.Id == id, includeProperties: "Category,CoverType");
            ShoppingCard cartObj = new ShoppingCard()
            {
                Product = productFromDb,
                ProductId = productFromDb.Id
            };
            return View(cartObj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Details(ShoppingCard CartObject)
        {
            CartObject.Id = 0;
            if (ModelState.IsValid)
            {
                //then we will add to cart
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                CartObject.ApplicationUserId = claim.Value;

                ShoppingCard cartFromDb = _unitOfWork.ShoppingCard.GetFirstOrDefault(
                    u => u.ApplicationUserId == CartObject.ApplicationUserId && u.ProductId == CartObject.ProductId
                    , includeProperties: "Product"
                    );

                if (cartFromDb == null)
                {
                    //no records exists in database for that product for that user
                    _unitOfWork.ShoppingCard.Add(CartObject);
                }
                else
                {
                    cartFromDb.Count += CartObject.Count;
                    //_unitOfWork.ShoppingCart.Update(cartFromDb);
                }
                _unitOfWork.Save();

                var count = _unitOfWork.ShoppingCard
                    .GetAll(c => c.ApplicationUserId == CartObject.ApplicationUserId)
                    .ToList().Count();

                HttpContext.Session.SetObject(SD.ssShoppingCart, CartObject);
                HttpContext.Session.SetInt32(SD.ssShoppingCart, count);

                return RedirectToAction(nameof(Index));
            }
            else
            {
                var productFromDb = _unitOfWork.Product.
                        GetFirstOrDefault(u => u.Id == CartObject.ProductId, includeProperties: "Category,CoverType");
                ShoppingCard cartObj = new ShoppingCard()
                {
                    Product = productFromDb,
                    ProductId = productFromDb.Id
                };
                return View(cartObj);
            }


        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
    //[Area("Customer")]
    //public class HomeController : Controller
    //{
    //    private readonly ILogger<HomeController> _logger;
    //    private readonly IUnitOfWork _unitOfWork;


    //    public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
    //    {
    //        _logger = logger;
    //        _unitOfWork = unitOfWork;
    //    }

    //    public IActionResult Index()
    //    {
    //        IEnumerable<Product> productList = _unitOfWork.Product.GetAll(includeProperties: "Category,CoverType");
    //        return View(productList);
    //    }
    //    ////////////////GET
    //    public IActionResult Details(int id)
    //    {
    //        var productFromDb = _unitOfWork.Product
    //             .GetFirstOrDefault(u => u.Id == id, includeProperties: "Category,CoverType");
    //        ShoppingCard cardObj = new ShoppingCard()
    //        {
    //            Product = productFromDb,
    //            ProductId = productFromDb.Id
    //        };
    //        return View(cardObj);
    //    }

    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    [Authorize]
    //    public IActionResult Details(ShoppingCard CartObject)
    //    {
    //        CartObject.Id = 0;
    //        if (ModelState.IsValid)
    //        {
    //            //we well add to cart
    //            var claimsIdentity = (ClaimsIdentity)User.Identity;
    //            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
    //            CartObject.ApplicationUserId = claim.Value;

    //            ShoppingCard cardFromDb = _unitOfWork.ShoppingCard.GetFirstOrDefault(
    //                u => u.ApplicationUserId == CartObject.ApplicationUserId && u.ProductId == CartObject.ProductId
    //                , includeProperties: "Product"
    //                );
    //            if (cardFromDb == null)
    //            {
    //                _unitOfWork.ShoppingCard.Add(CartObject);
    //            }
    //            else
    //            {
    //                CartObject.Count += cardFromDb.Count;
    //                _unitOfWork.ShoppingCard.Update(CartObject);
    //            }
    //            _unitOfWork.Save();
    //            return RedirectToAction(nameof(Index));
    //        }
    //        else
    //        {
    //            var productFromDb = _unitOfWork.Product
    //                 .GetFirstOrDefault(u => u.Id == CartObject.ProductId, includeProperties: "Category,CoverType");
    //            ShoppingCard cardObj = new ShoppingCard()
    //            {
    //                Product = productFromDb,
    //                ProductId = productFromDb.Id
    //            };
    //            return View(cardObj);
    //        }
    //    }


    //    public IActionResult Privacy()
    //    {
    //        return View();
    //    }

    //    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    //    public IActionResult Error()
    //    {
    //        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    //    }
    //}
}
