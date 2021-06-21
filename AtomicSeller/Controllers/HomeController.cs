using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AtomicSeller.Helpers;
using AtomicSeller.ViewModels;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using AtomicSeller.Helpers.eCommerceConnectors;
using YrAPI.Models;

namespace AtomicSeller.Controllers
{
    public class HomeController : BaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Transport_quote()
        {
            new Yr().Yr_Transport_quote();

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult Transport_shipment()
        {
            new Yr().Yr_Transport_shipment();

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult Transport_tracking()
        {
            TrackingResponse _Result =  new Yr().Yr_Transport_tracking();

            if (string.IsNullOrEmpty(_Result._header.ReturnMessage))
                FlashMessage.Flash(TempData, new FlashMessage("", FlashMessageType.Success, "Ok", true));
            else
                FlashMessage.Flash(TempData, new FlashMessage(_Result._header.ReturnMessage, FlashMessageType.Warning, "Error", true));


            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult Transport_proof_of_delivery()
        {
            PodResponse _Result = new Yr().Yr_Transport_proof_of_delivery();

            if (string.IsNullOrEmpty(_Result._header.ReturnMessage))
                FlashMessage.Flash(TempData, new FlashMessage("", FlashMessageType.Success, "Ok", true));
            else
                FlashMessage.Flash(TempData, new FlashMessage(_Result._header.ReturnMessage, FlashMessageType.Warning, "Error", true));


            return RedirectToAction("Index", "Home");
        }
        
    }
}