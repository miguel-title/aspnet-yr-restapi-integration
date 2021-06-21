using AtomicSeller.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Authentication;
using System.Web;
using AtomicSeller.ViewModels;
using UPSAPI.Models;
using Newtonsoft.Json.Linq;

namespace AtomicSeller.Helpers.eCommerceConnectors
{
    public class UPS
    {

        
        private static String API_BASE_URL = "https://onlinetools.ups.com/ship/v1/shipments";
        private static String m_strUserName = "atomic702";
        private static String m_strUserPassword = "J27sqh%%%%";
        private static String m_strLicNumber = "BD86234A2055EBD2";


        private string SendPostHttpRequest(String url, String jsonParam)
        {
            const SslProtocols _Tls12 = (SslProtocols)0x00000C00;
            const SecurityProtocolType Tls12 = (SecurityProtocolType)_Tls12;
            ServicePointManager.SecurityProtocol = Tls12;

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            httpWebRequest.Accept = "application/json";
            httpWebRequest.Headers.Add("username", m_strUserName);
            httpWebRequest.Headers.Add("password", m_strUserPassword);
            httpWebRequest.Headers.Add("AccessLicenseNumber", m_strLicNumber);

            using (StreamWriter writer = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                writer.WriteLine(jsonParam);
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            var result = "";
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();
            }
            return result;
        }

        public CreateShipmentResponse UPS_CreateShipment(String StrShipperNumber)
        {
            String CreateShipment_API_URL = API_BASE_URL + "?additionaladdressvalidation=city";

            //set Request
            string jsonParam = string.Empty;
            CreateShipmentRequest request = new CreateShipmentRequest();
            cShipmentRequest cRequest = new cShipmentRequest();

            cShipment _shipment = new cShipment();
            _shipment.Description = "1206 PTR";
            _shipment.ItemizedChargesRequestedIndicator = "";
            _shipment.RatingMethodRequestedIndicator = "";
            _shipment.TaxInformationIndicator = "";

            cShipper _Shipper = new cShipper();
            _Shipper.AttentionName = "AttentionName";
            _Shipper.Name = "ShipperName";
            _Shipper.TaxIdentificationNumber = "TaxID";
            _Shipper.ShipperNumber = StrShipperNumber;
            cPhone _ShipperPhone = new cPhone();
            _ShipperPhone.Number = "123456";
            cAddress _ShipperAddress = new cAddress();
            _ShipperAddress.AddressLine = "Shipper AddressLine";
            _ShipperAddress.City = "Roswell";
            _ShipperAddress.CountryCode = "US";
            _ShipperAddress.StateProvinceCode = "GA";
            _ShipperAddress.PostalCode = "30075";
            _Shipper.Phone = _ShipperPhone;
            _Shipper.Address = _ShipperAddress;
            _shipment.Shipper = _Shipper;

            cShipTo ShipTo = new cShipTo();
            ShipTo.AttentionName = "AttentionName";
            ShipTo.Name = "ShipperName";
            ShipTo.TaxIdentificationNumber = "TaxID";
            ShipTo.FaxNumber = "Your Shipper Number";
            cPhone ShipToPhone = new cPhone();
            ShipToPhone.Number = "123456";
            cAddress ShipToAddress = new cAddress();
            ShipToAddress.AddressLine = "ShipTo AddressLine";
            ShipToAddress.City = "Roswell";
            ShipToAddress.CountryCode = "US";
            ShipToAddress.StateProvinceCode = "GA";
            ShipToAddress.PostalCode = "30076";
            ShipTo.Phone = ShipToPhone;
            ShipTo.Address = ShipToAddress;
            _shipment.ShipTo = ShipTo;

            cShipFrom ShipFrom = new cShipFrom();
            ShipFrom.AttentionName = "AttentionName";
            ShipFrom.Name = "ShipperName";
            ShipFrom.TaxIdentificationNumber = "TaxID";
            ShipFrom.FaxNumber = "Your Shipper Number";
            cPhone ShipFromPhone = new cPhone();
            ShipFromPhone.Number = "123456";
            cAddress ShipFromAddress = new cAddress();
            ShipFromAddress.AddressLine = "ShipTo AddressLine";
            ShipFromAddress.City = "Roswell";
            ShipFromAddress.CountryCode = "US";
            ShipFromAddress.StateProvinceCode = "GA";
            ShipFromAddress.PostalCode = "30076";
            ShipFrom.Phone = ShipFromPhone;
            ShipFrom.Address = ShipFromAddress;
            _shipment.ShipFrom = ShipFrom;

            cPaymentInformation _PaymentInformation = new cPaymentInformation();
            cShipmentCharge _ShipmentCharge = new cShipmentCharge();
            cBillShipper _BillShipper = new cBillShipper();
            _BillShipper.AccountNumber = "AccountNumber";
            _ShipmentCharge.Type = "01";
            _ShipmentCharge.BillShipper = _BillShipper;
            _PaymentInformation.ShipmentCharge = _ShipmentCharge;
            _shipment.PaymentInformation = _PaymentInformation;

            cService _service = new cService();
            _service.Code = "01";
            _service.Description = "Expedited";
            _shipment.Service = _service;

            List<cPackage> lstPackage = new List<cPackage>();
            for (int i = 0; i < 2; i++)
            {
                cPackage _Package = new cPackage();
                _Package.Description = "International Goods";
                _Package.PackageServiceOptions = "";
                cPackaging _Packaging = new cPackaging();
                _Packaging.Code = "02";
                cPackageWeight _PackageWeight = new cPackageWeight();
                _PackageWeight.Weight = "70";
                cUnitOfMeasurement _UnitOfMeasurement = new cUnitOfMeasurement();
                _UnitOfMeasurement.Code = "LBS";
                _PackageWeight.UnitOfMeasurement = _UnitOfMeasurement;
                _Package.PackageWeight = _PackageWeight;
                _Package.Packaging = _Packaging;
                lstPackage.Add(_Package);
            }
            _shipment.Package = lstPackage;

            cShipmentRatingOptions _ShipmentRatingOptions = new cShipmentRatingOptions();
            _ShipmentRatingOptions.NegotiatedRatesIndicator = "";
            _shipment.ShipmentRatingOptions = _ShipmentRatingOptions;

            cLabelSpecification _LabelSpecification = new cLabelSpecification();
            cLabelImageFormat _LabelImageFormat = new cLabelImageFormat();
            _LabelImageFormat.Code = "GIF";
            _LabelSpecification.LabelImageFormat = _LabelImageFormat;

            cRequest.Shipment = _shipment;
            cRequest.LabelSpecification = _LabelSpecification;
            request.ShipmentRequest = cRequest;
            jsonParam = JsonConvert.SerializeObject(request).ToString();

            //Set Response 
            CreateShipmentResponse _GetShipmentResponse = new CreateShipmentResponse();

            ResponseHeader _GetShipmentResponseHeader = new ResponseHeader();
            _GetShipmentResponseHeader.LanguageCode = "En";
            _GetShipmentResponseHeader.RequestStatus = "Ok";
            _GetShipmentResponseHeader.ReturnCode = "AS0000";
            _GetShipmentResponseHeader.ReturnMessage = "";

            _GetShipmentResponse.Header = _GetShipmentResponseHeader;
            _GetShipmentResponse.Response = new ShipmentResponse();



            try
            {
                /*********Get ProductList***********/
                string ShipmentResult = new UPS().SendPostHttpRequest(CreateShipment_API_URL, jsonParam);
                /***********************************/

                CreateShipmentResponse Shipment = JsonConvert.DeserializeObject<CreateShipmentResponse>(ShipmentResult);

            }
            catch (Exception ex)
            {
                _GetShipmentResponseHeader.LanguageCode = "En";
                _GetShipmentResponseHeader.RequestStatus = "Error";
                _GetShipmentResponseHeader.ReturnCode = "WZ0"; ;
                _GetShipmentResponseHeader.ReturnMessage = ex.Message; ;
                _GetShipmentResponse.Header = _GetShipmentResponseHeader;

            }

            return _GetShipmentResponse;
        }

        public static string ErrorMessage = "";

        public GetLabelResponse UPS_GetLabel(String strTrackingNumber)
        {                   
            String GETLABEL_API_URL = API_BASE_URL + "/labels";

            string jsonParam = string.Empty;
            GetLabelRequest request = new GetLabelRequest();
            cLabelRecoveryRequest cRequest = new cLabelRecoveryRequest();

            cRequest.TrackingNumber = strTrackingNumber;
            cLabelSpecification _LabelSpecification = new cLabelSpecification();
            _LabelSpecification.HTTPUserAgent = "";
            cLabelImageFormat _LabelImageFormat = new cLabelImageFormat();
            _LabelImageFormat.Code = "ZPL";
            _LabelSpecification.LabelImageFormat = _LabelImageFormat;
            cLabelStockSize _LabelStockSize = new cLabelStockSize();
            _LabelStockSize.Height = "6";
            _LabelStockSize.Width = "4";
            _LabelSpecification.LabelStockSize = _LabelStockSize;
            cRequest.LabelSpecification = _LabelSpecification;

            cTranslate _Translate = new cTranslate();
            _Translate.LanguageCode = "eng";
            _Translate.Code = "01";
            _Translate.DialectCode = "US";
            cRequest.Translate = _Translate;

            cLabelDelivery _LabelDelivery = new cLabelDelivery();
            _LabelDelivery.LabelLinkIndicator = "";
            _LabelDelivery.ResendEMailIndicator = "";
            cEMailMessage _EMailMessage = new cEMailMessage();
            _EMailMessage.EMailAddress = "";
            _LabelDelivery.EMailMessage = _EMailMessage;
            cRequest.LabelDelivery = _LabelDelivery;

            request.LabelRecoveryRequest = cRequest;

            jsonParam = JsonConvert.SerializeObject(request).ToString();

            //Set Response of Get Label
            GetLabelResponse _GetProductsResponse = new GetLabelResponse();

            ResponseHeader _GetLabelResponseHeader = new ResponseHeader();
            _GetLabelResponseHeader.LanguageCode = "En";
            _GetLabelResponseHeader.RequestStatus = "Ok";
            _GetLabelResponseHeader.ReturnCode = "AS0000";
            _GetLabelResponseHeader.ReturnMessage = "";

            _GetProductsResponse.Header = _GetLabelResponseHeader;
            _GetProductsResponse.Response = new cLabelResponse();

            try
            {
                /*********Get ProductList***********/
                string GetLabelResult = new UPS().SendPostHttpRequest(GETLABEL_API_URL, jsonParam);
                /***********************************/

                GetLabelResponse _skuList = JsonConvert.DeserializeObject<GetLabelResponse>(GetLabelResult);

            }
            catch (Exception ex)
            {
                _GetLabelResponseHeader.LanguageCode = "En";
                _GetLabelResponseHeader.RequestStatus = "Error";
                _GetLabelResponseHeader.ReturnCode = "WZ0";;
                _GetLabelResponseHeader.ReturnMessage = ex.Message;;
                _GetProductsResponse.Header = _GetLabelResponseHeader;

            }

            return _GetProductsResponse;
        }

    }
}