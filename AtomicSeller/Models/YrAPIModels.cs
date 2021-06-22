using DocumentFormat.OpenXml.Drawing.Diagrams;
using iTextSharp.text.io;
using MarketplaceWebServiceOrders.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Web;

namespace YrAPI.Models
{
    //Quote Data Structur
    public class QuoteRequest
    {
        public string account__number { get; set; }
        public string account__country_code { get; set; }
        public string transport_code { get; set; }
        public countryInfo from { get; set; }
        public countryInfo to { get; set; }
        public string date_pickup { get; set; }
        public string declared_value { get; set; }
        public string type { get; set; }
        public List<piece> piece { get; set; }
    }

    public class countryInfo
    {
        public string country_code { get; set; }
        public string city { get; set; }
        public string postal_code { get; set; }
        public string state_code { get; set; }
    }

    public class piece
    {
        public float weight { get; set; }
        public float height { get; set; }
        public float depth { get; set; }
        public float width { get; set; }
    }

    public class QuoteResponse
    {
        public ResponseHeader _header { get; set; }
        public QuoteResponseData _data { get; set; }
    }

    public class QuoteResponseData
    {
        public string message_time { get; set; }
        public string message_reference { get; set; }
        public List<quotesuccess> success { get; set; }
    }

    public class quotesuccess
    {
        public string product_code { get; set; }
        public string product_name { get; set; }
        public string pickup_date { get; set; }
        public string delivery_date { get; set; }
        public List<rate> reat { get; set; }
    }

    public class rate
    {
        public string category { get; set; }
        public string name { get; set; }
        public string pickup { get; set; }
        public string transport { get; set; }
    }

    //Shipment Data Structure
    public class ShipmentRequest
    {
        public string shipper_id { get; set; }
        public string account__number { get; set; }
        public string account__country_code { get; set; }
        public string product_code { get; set; }
        public companyInfo from { get; set; }
        public companyInfo to { get; set; }
        public pickup pickup { get; set; }
        public string declared_value { get; set; }
        public string contents { get; set; }
        public string type { get; set; }
        public List<piece> piece { get; set; }
        public invoice invoice { get; set; }
    }

    public class invoice
    {
        public string number { get; set; }
        public string date { get; set; }
        public string export_reason { get; set; }
        public List<item> items { get; set; }
    }

    public class item
    {
        public string quantity { get; set; }
        public string description { get; set; }
        public string value { get; set; }
        public string weight { get; set; }
        public string eccn { get; set; }
        public string country_code { get; set; }
    }

    public class pickup
    {
        public string date { get; set; }
        public string ready_time { get; set; }
        public string close_time { get; set; }
        public string program { get; set; }
    }

    public class companyInfo
    {
        public string company { get; set; }
        public contactInfo contact { get; set; }
        public List<address> address { get; set; }
        public string country_code { get; set; }
        public string city { get; set; }
        public string postal_code { get; set; }
        public string state_code { get; set; }
    }

    public class address
    {
        public string line { get; set; }
    }

    public class contactInfo
    {
        public string name { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
    }

    public class ShipmentResponse
    {
        public ResponseHeader _header { get; set; }
        public ShipmentResponseData _data { get; set; }
    }


    //Tracking Data Structure
    public class TrackingRequest
    {
        public string transport_code { get; set; }
        public string tracking_number { get; set; }
        public string language { get; set; }
    }

    public class TrackingResponse
    {
        public ResponseHeader _header { get; set; }
        public TrackingResponseData _data { get; set; }
    }

    public class TrackingResponseData
    {
        public DateTime message_time { get; set; }
        public string message_reference { get; set; }
        public trackingsuccess success { get; set; }
        public Error error { get; set; }
    }

    public class Error
    {
        public string code { get; set; }
        public string message { get; set; }
    }
    public class trackingsuccess
    {
        public string transport_code { get; set; }
        public string tracking_number { get; set; }
        public destination destination { get; set; }
        public List<activity> activity { get; set; }
    }

    public class destination
    {
        public string description { get; set; }
        public string city { get; set; }
        public string postal_code { get; set; }
        public string country_code { get; set; }
    }

    public class activity
    {
        public string description { get; set; }
        public string status { get; set; }
        public string date { get; set; }
        public string time { get; set; }
    }

    //Pod Data Structure
    public class PodRequest
    {
        public string transport_code { get; set; }
        public string tracking_number { get; set; }
        public string language { get; set; }
    }

    public class PodResponse
    {
        public ResponseHeader _header { get; set; }
        public PodResponseData _data { get; set; }
    }

    public class PodResponseData
    {
        public string message_time { get; set; }
        public string message_reference { get; set; }
        public podsuccess success { get; set; }
    }

    public class podsuccess
    {
        public string tracking_number { get; set; }
        public string format { get; set; }
        public string pod { get; set; }
    }

    //Common Response Header

    public class ResponseHeader
    {
        public string RequestStatus { get; set; }

        public string ReturnCode { get; set; }

        public string ReturnMessage { get; set; }

        public string LanguageCode { get; set; }
    }



    public class ShipmentResponseData
    {
        public DateTime message_time { get; set; }
        public string message_reference { get; set; }
        public shipmentsuccess success { get; set; }
    }

    public class shipmentsuccess
    {
        public string tracking_number { get; set; }
        public string tracking_label_pdf { get; set; }
        public string reservation_number { get; set; }
    }




    /*
    public class Rootobject
    {
        public DateTime message_time { get; set; }
        public string message_reference { get; set; }
        public Success success { get; set; }
    }

    public class Success
    {
        public string tracking_number { get; set; }
        public string tracking_label_pdf { get; set; }
    }
    */
}