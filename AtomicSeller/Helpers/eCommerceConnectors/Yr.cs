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
using YrAPI.Models;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace AtomicSeller.Helpers.eCommerceConnectors
{
    public class Yr
    {

        
        private static String API_BASE_URL = "http://server.a1ws.es/api_transport/public/api";
        private static String Token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiJ9.eyJhdWQiOiIzIiwianRpIjoiMzc1NWZlYmFmY2EyNDhjZjQ3MDI5MzNjZmIwODNjMjYyNjk3OWUwYTgzNGZmZWRmZGZmYjE2NDMyNWEzOGJhYjI1Y2MzOGM3NjIxZGZiODYiLCJpYXQiOjE2MjM0MjUxNDUsIm5iZiI6MTYyMzQyNTE0NSwiZXhwIjoxNjU0OTYxMTQ1LCJzdWIiOiIzIiwic2NvcGVzIjpbXX0.Z9JTUmLCcfhZc-30JkonHJAqzqy14v98HbnyWT4qy3lMp-KCXaV0pvCZTVmC42NbrG5mPw3uxxZeXpx_nzsD1kB4NB701B-e9t9DL-6qUZ1TUHW_vcnsSFUSizueUM-Xoqw8qlsZLqn3Is7tBJZzRo0mnw89ep08aUNFKENBCfrutt2al1ZwYLun3C-GfuhvDkLppsVck0VeduuBPFz3LU2eOoRdny-6OnAxX_DTvbAjNaYeOYABwrUCuH7jiZtN_zjg-nFNrabGFxcNOhhMA-BmKTi017y3CCY_2cPcJD57bmTME9fFPMsjG_NjkxhKQ-KE2WQI45bU38qQt2kettDqctzvyw52SBQwG2mX3KZXOxvTRVIqpXIyuJM2NikKUHEicH4bbEfTHw023-FVQVlIRBdUY-jtuNoxZZhL4EjsGIMga-2ZNp8O1lUNOuZTJNJBgicyH3PyYcEIzfvyXrBuoavrh2a_YJSagxMUxtK9RVWIoR_YvitM5tiD6QjGE1f83ca67aF_QdjXKXD_hsce_Mcq2BPdXCQUFRtcAUmB_kGR5i9GymnxoIMe6iusgiT6PhmOjsl432q3tX1pjb-fUhM6xuKYOnFpQ3qnwagI5FRfDPAIh3SxgBqf683RCNwvG2XLxMKEWuUC0GS0Ag6oYIFUl2IqhYtx72B9PxA";

        public static string ErrorMessage = "";

        private string SendPostHttpRequest(String url, String jsonParam)
        {
            const SslProtocols _Tls12 = (SslProtocols)0x00000C00;
            const SecurityProtocolType Tls12 = (SecurityProtocolType)_Tls12;
            ServicePointManager.SecurityProtocol = Tls12;

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            httpWebRequest.Accept = "application/json";
            httpWebRequest.Headers.Add("Authorization", "Bearer " + Token);
                

            using (StreamWriter writer = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                writer.WriteLine(jsonParam);
            }

            var result = "";
            try
            {
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }
            }
            catch (WebException wex)
            {
                using (WebResponse response = wex.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)response;
                    //result = "Error code: "+ httpResponse.StatusCode + " ";
                    using (Stream data = response.GetResponseStream())
                    using (var reader = new StreamReader(data))
                    {
                        result += reader.ReadToEnd();
                    }
                }

            }
            catch (Exception Ex)
            {
            }

            return result;
        }

        public QuoteResponse Yr_Transport_quote()
        {
            String Quote_API_URL = API_BASE_URL + "/quote";

            //set Request
            string jsonParam = string.Empty;
            QuoteRequest request = new QuoteRequest();

            request.account__country_code = "FR";
            request.account__number = "";
            request.date_pickup = "2021-06-21";
            request.declared_value = "10";
            request.transport_code = "DHL";
            countryInfo _frominfo = new countryInfo();
            _frominfo.city = "Lyon";
            _frominfo.country_code = "FR";
            _frominfo.postal_code = "59115";
            _frominfo.state_code = "";
            request.from = _frominfo;
            countryInfo _toinfo = new countryInfo();
            _toinfo.city = "Montaigu";
            _toinfo.country_code = "FR";
            _toinfo.postal_code = "78000";
            _toinfo.state_code = "";
            request.to = _toinfo;
            request.type = "p";
            List<piece> _lstpiece = new List<piece>();
            piece _piece = new piece();
            _piece.weight = (float)1.5;
            _piece.height = 10;
            _piece.depth = 10;
            _piece.width = 10;
            _lstpiece.Add(_piece);
            request.piece = _lstpiece;

            jsonParam = JsonConvert.SerializeObject(request).ToString();

            jsonParam = jsonParam.Replace("__", ".");

            //Set Response 
            QuoteResponse _QuoteResponse = new QuoteResponse();
            ResponseHeader _QuoteResponseHeader = new ResponseHeader();
            QuoteResponseData _Quotedata = new QuoteResponseData();

            try
            {
                string QuoteResult = new Yr().SendPostHttpRequest(Quote_API_URL, jsonParam);

                _Quotedata = JsonConvert.DeserializeObject<QuoteResponseData>(QuoteResult);

                _QuoteResponseHeader.LanguageCode = "En";
                _QuoteResponseHeader.RequestStatus = "Ok";
                _QuoteResponseHeader.ReturnCode = "AS0000";
                _QuoteResponseHeader.ReturnMessage = "";


            }
            catch (Exception ex)
            {
                _QuoteResponseHeader.LanguageCode = "En";
                _QuoteResponseHeader.RequestStatus = "Error";
                _QuoteResponseHeader.ReturnCode = "WZ0";
                _QuoteResponseHeader.ReturnMessage = ex.Message;
            }


            _QuoteResponse._header = _QuoteResponseHeader;
            _QuoteResponse._data = _Quotedata;

            return _QuoteResponse;
        }

        public ShipmentResponse Yr_Transport_shipment()
        {
            String Shipment_API_URL = API_BASE_URL + "/shipment";

            //set Request
            string jsonParam = string.Empty;
            ShipmentRequest request = new ShipmentRequest();
            request.shipper_id = "atomicseller";
            request.account__number = "";
            request.account__country_code = "FR";
            request.product_code = "DHL11";
            companyInfo _frominfo = new companyInfo();
            _frominfo.company = "VF Solutions";
            _frominfo.country_code = "FR";
            _frominfo.city = "Lyon";
            _frominfo.postal_code = "59115";
            _frominfo.state_code = "";
            contactInfo _fromcontact = new contactInfo();
            _fromcontact.name = "STOCK LOGISTIC";
            _fromcontact.phone = "0320200903";
            _fromcontact.email = "vfsols@atomicseller.fr";
            _frominfo.contact = _fromcontact;
            List<address> _lstfromaddress = new List<address>();
            address _fromaddress = new address();
            _fromaddress.line = "16 Rue du Trieu Quesnoy";
            _lstfromaddress.Add(_fromaddress);
            _frominfo.address = _lstfromaddress;
            request.from = _frominfo;

            companyInfo _toinfo = new companyInfo();
            _toinfo.company = "GEMMA";
            _toinfo.country_code = "FR";
            _toinfo.city = "Montaigu";
            _toinfo.postal_code = "78000";
            _toinfo.state_code = "";
            contactInfo _tocontact = new contactInfo();
            _tocontact.name = "GLASS";
            _tocontact.phone = "3378855299339";
            _tocontact.email = "test@atomicseller.com";
            _toinfo.contact = _tocontact;
            List<address> _lsttoaddress = new List<address>();
            address _toaddress1 = new address();
            _toaddress1.line = "Rue Henri Dunant";
            _lsttoaddress.Add(_toaddress1);
            address _toaddress2 = new address();
            _toaddress2.line = "Etage 3";
            _lsttoaddress.Add(_toaddress2);
            address _toaddress3 = new address();
            _toaddress3.line = "Batiment 4";
            _lsttoaddress.Add(_toaddress3);
            _toinfo.address = _lsttoaddress;
            request.to = _toinfo;

            pickup _pickup = new pickup();
            _pickup.date = "2021-06-21";
            _pickup.ready_time = "10:00";
            _pickup.close_time = "16:00";
            _pickup.program = "";
            request.pickup = _pickup;

            request.declared_value = "10";
            request.contents = "tracking_content";
            request.type = "P";
            List<piece> _lstPiece = new List<piece>();
            piece _piece = new piece();
            _piece.weight = (float)1.5;
            _piece.height = 10;
            _piece.depth = 10;
            _piece.width = 10;
            _lstPiece.Add(_piece);
            request.piece = _lstPiece;

            invoice _invoice = new invoice();
            _invoice.number = "123";
            _invoice.date = "2021-06-21";
            _invoice.export_reason = "";
            List<item> _lstitems = new List<item>();
            item _item = new item();
            _item.quantity = "1";
            _item.description = "invoice_description";
            _item.value = "125.5";
            _item.weight = "1.5";
            _item.eccn = "";
            _item.country_code = "FR";
            _lstitems.Add(_item);
            _invoice.items = _lstitems;
            request.invoice = _invoice;

            jsonParam = JsonConvert.SerializeObject(request).ToString();

            jsonParam = jsonParam.Replace("__",".");


            ShipmentResponse _ShipmentResponse = new ShipmentResponse();
            ResponseHeader _ShipmentResponseHeader = new ResponseHeader();
            ShipmentResponseData _Shipmentdata = new ShipmentResponseData();

            try
            {
                /*********Get ProductList***********/
                string ShipmentResult = new Yr().SendPostHttpRequest(Shipment_API_URL, jsonParam);
                /***********************************/

                _Shipmentdata = JsonConvert.DeserializeObject<ShipmentResponseData>(ShipmentResult);

                _ShipmentResponseHeader.LanguageCode = "En";
                _ShipmentResponseHeader.RequestStatus = "Ok";
                _ShipmentResponseHeader.ReturnCode = "AS0000";
                _ShipmentResponseHeader.ReturnMessage = "";
            }
            catch (Exception ex)
            {
                _ShipmentResponseHeader.LanguageCode = "En";
                _ShipmentResponseHeader.RequestStatus = "Error";
                _ShipmentResponseHeader.ReturnCode = "WZ0";
                _ShipmentResponseHeader.ReturnMessage = ex.Message;
            }

            _ShipmentResponse._header = _ShipmentResponseHeader;
            _ShipmentResponse._data = _Shipmentdata;
            return _ShipmentResponse;
        }


        public TrackingResponse Yr_Transport_tracking()
        {
            String Tracking_API_URL = API_BASE_URL + "/tracking";

            //set Request
            string jsonParam = string.Empty;
            TrackingRequest request = new TrackingRequest();
            request.tracking_number = "4114596485";
            request.transport_code = "DHL";
            request.language = "FR";

            jsonParam = JsonConvert.SerializeObject(request).ToString();

            jsonParam = jsonParam.Replace("__",".");

            //Set Response 
            TrackingResponse _TrackingResponse = new TrackingResponse();
            ResponseHeader _TrackingResponseHeader = new ResponseHeader();
            TrackingResponseData _Trackingdata = new TrackingResponseData();

            try
            {
                /*********Get ProductList***********/
                string TrackingResult = new Yr().SendPostHttpRequest(Tracking_API_URL, jsonParam);
                /***********************************/

                _Trackingdata = JsonConvert.DeserializeObject<TrackingResponseData>(TrackingResult);

                _TrackingResponseHeader.LanguageCode = "En";
                _TrackingResponseHeader.RequestStatus = "Ok";
                _TrackingResponseHeader.ReturnCode = "AS0000";
                _TrackingResponseHeader.ReturnMessage = "";
            }
            catch (Exception ex)
            {
                _TrackingResponseHeader.LanguageCode = "En";
                _TrackingResponseHeader.RequestStatus = "Error";
                _TrackingResponseHeader.ReturnCode = "WZ0";
                _TrackingResponseHeader.ReturnMessage = ex.Message;
            }

            _TrackingResponse._header = _TrackingResponseHeader;
            _TrackingResponse._data = _Trackingdata;
            return _TrackingResponse;
        }

        public PodResponse Yr_Transport_proof_of_delivery()
        {
            String Pod_API_URL = API_BASE_URL + "/pod";

            //set Request
            string jsonParam = string.Empty;
            PodRequest request = new PodRequest();
            request.language = "FR";
            request.tracking_number = "4114596485";
            request.transport_code = "DHL";

            jsonParam = JsonConvert.SerializeObject(request).ToString();

            jsonParam = jsonParam.Replace("__", ".");

            //Set Response 
            PodResponse _PodResponse = new PodResponse();
            ResponseHeader _PodResponseHeader = new ResponseHeader();
            PodResponseData _Poddata = new PodResponseData();

            try
            {
                /*********Get ProductList***********/
                string PodResult = new Yr().SendPostHttpRequest(Pod_API_URL, jsonParam);
                /***********************************/

                _Poddata = JsonConvert.DeserializeObject<PodResponseData>(PodResult);

                _PodResponseHeader.LanguageCode = "En";
                _PodResponseHeader.RequestStatus = "Ok";
                _PodResponseHeader.ReturnCode = "AS0000";
                _PodResponseHeader.ReturnMessage = "";
            }
            catch (Exception ex)
            {
                _PodResponseHeader.LanguageCode = "En";
                _PodResponseHeader.RequestStatus = "Error";
                _PodResponseHeader.ReturnCode = "WZ0";
                _PodResponseHeader.ReturnMessage = ex.Message;
            }

            _PodResponse._header = _PodResponseHeader;
            _PodResponse._data = _Poddata;
            return _PodResponse;
        }

    }
}