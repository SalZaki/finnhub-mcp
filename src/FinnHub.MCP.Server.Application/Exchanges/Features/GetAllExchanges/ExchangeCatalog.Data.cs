// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace FinnHub.MCP.Server.Application.Exchanges.Features.GetAllExchanges;

/// <content>
/// The verbatim catalog data backing <see cref="ExchangeCatalog"/>.
/// </content>
/// <remarks>
/// Generated from Finnhub's public "Supported Exchanges" reference sheet
/// (https://docs.google.com/spreadsheets/d/1I3pBxjfXB056-g_JYf_6o3Rns3BV2kMGG1nCatb91ls),
/// retrieved 2026-05-31. Columns are mapped 1:1 onto <see cref="Exchange"/> and values
/// are preserved as-published — including the raw <c>close_date</c> weekday encoding and
/// the absent reference URLs for venues Finnhub lists without one. Do not hand-edit;
/// re-export the sheet to refresh.
/// </remarks>
public sealed partial class ExchangeCatalog
{
    private static readonly IReadOnlyList<Exchange> s_all =
    [
        new Exchange
        {
            ExchangeCode = "AD",
            ExchangeName = "ABU DHABI SECURITIES EXCHANGE",
            MicCode = "XADS",
            TimeZone = "Asia/Dubai",
            PreMarketHours = null,
            TradingHours = "10:00-14:44",
            PostMarketHours = null,
            CloseDate = "7,0",
            CountryCode = "AE",
            CountryName = "UAE",
            Url = "https://www.tradinghours.com/markets/adx"
        },
        new Exchange
        {
            ExchangeCode = "AS",
            ExchangeName = "NYSE EURONEXT - EURONEXT AMSTERDAM",
            MicCode = "XAMS",
            TimeZone = "Europe/Amsterdam",
            PreMarketHours = null,
            TradingHours = "09:00-17:30",
            PostMarketHours = null,
            CloseDate = "7,0",
            CountryCode = "NL",
            CountryName = "Netherlands",
            Url = "https://www.tradinghours.com/exchanges/euronext"
        },
        new Exchange
        {
            ExchangeCode = "AT",
            ExchangeName = "ATHENS EXCHANGE S.A. CASH MARKET",
            MicCode = "ASEX",
            TimeZone = "Europe/Athens",
            PreMarketHours = null,
            TradingHours = "10:15-17:20",
            PostMarketHours = null,
            CloseDate = "7,0",
            CountryCode = "GR",
            CountryName = "Greece",
            Url = "https://www.tradinghours.com/exchanges/ase-athens"
        },
        new Exchange
        {
            ExchangeCode = "AX",
            ExchangeName = "ASX - ALL MARKETS",
            MicCode = "XASX",
            TimeZone = "Australia/Sydney",
            PreMarketHours = "07:00-10:00",
            TradingHours = "10:00-16:00",
            PostMarketHours = "16:00-18:50",
            CloseDate = "7,0",
            CountryCode = "AU",
            CountryName = "Australia",
            Url = "https://www.tradinghours.com/exchanges/asx"
        },
        new Exchange
        {
            ExchangeCode = "BA",
            ExchangeName = "BOLSA DE COMERCIO DE BUENOS AIRES",
            MicCode = "XBUE",
            TimeZone = "America/Argentina/Buenos_Aires",
            PreMarketHours = null,
            TradingHours = "10:30-17:15",
            PostMarketHours = null,
            CloseDate = "7,0",
            CountryCode = "AR",
            CountryName = "Argentina",
            Url = "https://www.tradinghours.com/exchanges/bcba"
        },
        new Exchange
        {
            ExchangeCode = "BC",
            ExchangeName = "BOLSA DE VALORES DE COLOMBIA",
            MicCode = "XBOG",
            TimeZone = "America/Cuiaba",
            PreMarketHours = null,
            TradingHours = "09:15-16:00",
            PostMarketHours = null,
            CloseDate = "7,0",
            CountryCode = "CO",
            CountryName = "Colombia",
            Url = "https://www.tradinghours.com/exchanges/bvc"
        },
        new Exchange
        {
            ExchangeCode = "BD",
            ExchangeName = "BUDAPEST STOCK EXCHANGE",
            MicCode = "XBUD",
            TimeZone = "Europe/Budapest",
            PreMarketHours = null,
            TradingHours = "08:15-17:20",
            PostMarketHours = null,
            CloseDate = "7,0",
            CountryCode = "HU",
            CountryName = "Hungary",
            Url = "https://www.tradinghours.com/exchanges/bse-budapest"
        },
        new Exchange
        {
            ExchangeCode = "BE",
            ExchangeName = "BOERSE BERLIN",
            MicCode = "XBER",
            TimeZone = "Europe/Berlin",
            PreMarketHours = null,
            TradingHours = "08:00-20:00",
            PostMarketHours = null,
            CloseDate = "7,0",
            CountryCode = "DE",
            CountryName = "Germany",
            Url = "https://www.tradinghours.com/exchanges/xber"
        },
        new Exchange
        {
            ExchangeCode = "BH",
            ExchangeName = "BAHRAIN BOURSE",
            MicCode = "XBAH",
            TimeZone = "Asia/Bahrain",
            PreMarketHours = null,
            TradingHours = "09:30-13:00",
            PostMarketHours = null,
            CloseDate = "6,7",
            CountryCode = "BH",
            CountryName = "Bahrain",
            Url = "https://www.tradinghours.com/markets/bhb"
        },
        new Exchange
        {
            ExchangeCode = "BK",
            ExchangeName = "STOCK EXCHANGE OF THAILAND",
            MicCode = "XBKK",
            TimeZone = "Asia/Bangkok",
            PreMarketHours = null,
            TradingHours = "09:30-17:00",
            PostMarketHours = null,
            CloseDate = "7,0",
            CountryCode = "TH",
            CountryName = "Thailand",
            Url = "https://www.tradinghours.com/exchanges/set"
        },
        new Exchange
        {
            ExchangeCode = "BO",
            ExchangeName = "BSE LTD",
            MicCode = "XBOM",
            TimeZone = "Asia/Kolkata",
            PreMarketHours = null,
            TradingHours = "09:00-16:00",
            PostMarketHours = null,
            CloseDate = "7,0",
            CountryCode = "IN",
            CountryName = "India",
            Url = "https://www.tradinghours.com/exchanges/bse-bombay"
        },
        new Exchange
        {
            ExchangeCode = "BR",
            ExchangeName = "NYSE EURONEXT - EURONEXT BRUSSELS",
            MicCode = "XBRU",
            TimeZone = "Europe/Brussels",
            PreMarketHours = null,
            TradingHours = "09:00-17:30",
            PostMarketHours = null,
            CloseDate = "7,0",
            CountryCode = "BE",
            CountryName = "Belgium",
            Url = "https://www.tradinghours.com/exchanges/euronext-brussels"
        },
        new Exchange
        {
            ExchangeCode = "CA",
            ExchangeName = "Egyptian Stock Exchange",
            MicCode = "XCAI",
            TimeZone = "Africa/Cairo",
            PreMarketHours = null,
            TradingHours = "10:00-14:30",
            PostMarketHours = null,
            CloseDate = "6,7",
            CountryCode = "EG",
            CountryName = "Egypt",
            Url = "https://www.tradinghours.com/markets/egx"
        },
        new Exchange
        {
            ExchangeCode = "CN",
            ExchangeName = "CANADIAN NATIONAL STOCK EXCHANGE",
            MicCode = "XCNQ",
            TimeZone = "America/New_York",
            PreMarketHours = "07:00-09:30",
            TradingHours = "09:30-16:00",
            PostMarketHours = "16:00-17:00",
            CloseDate = "7,0",
            CountryCode = "CA",
            CountryName = "Canada",
            Url = "https://www.tradinghours.com/exchanges/cnsx"
        },
        new Exchange
        {
            ExchangeCode = "CO",
            ExchangeName = "OMX NORDIC EXCHANGE COPENHAGEN A/S",
            MicCode = "XCSE,FNDK",
            TimeZone = "Europe/Copenhagen",
            PreMarketHours = null,
            TradingHours = "09:00-17:00",
            PostMarketHours = null,
            CloseDate = "7,0",
            CountryCode = "DK",
            CountryName = "Denmark",
            Url = "https://www.tradinghours.com/exchanges/omxc-copenhagen"
        },
        new Exchange
        {
            ExchangeCode = "CR",
            ExchangeName = "CARACAS STOCK EXCHANGE",
            MicCode = "BVCA",
            TimeZone = "America/Caracas",
            PreMarketHours = null,
            TradingHours = "08:30-13:30",
            PostMarketHours = null,
            CloseDate = "7,0",
            CountryCode = "VE",
            CountryName = "Venezuela",
            Url = "https://www.tradinghours.com/exchanges/bvcc"
        },
        new Exchange
        {
            ExchangeCode = "CS",
            ExchangeName = "CASABLANCA STOCK EXCHANGE",
            MicCode = "XCAS",
            TimeZone = "Africa/Casablanca",
            PreMarketHours = null,
            TradingHours = "09:30-15:20",
            PostMarketHours = null,
            CloseDate = "7,0",
            CountryCode = "MA",
            CountryName = "Morroco",
            Url = "https://www.tradinghours.com/markets/bc"
        },
        new Exchange
        {
            ExchangeCode = "DB",
            ExchangeName = "DUBAI FINANCIAL MARKET",
            MicCode = "XDFM",
            TimeZone = "Asia/Dubai",
            PreMarketHours = null,
            TradingHours = "09:30-14:00",
            PostMarketHours = null,
            CloseDate = "7,0",
            CountryCode = "AE",
            CountryName = "UAE",
            Url = "https://www.tradinghours.com/exchanges/dfm"
        },
        new Exchange
        {
            ExchangeCode = "DE",
            ExchangeName = "XETRA",
            MicCode = "XETR",
            TimeZone = "Europe/Berlin",
            PreMarketHours = null,
            TradingHours = "09:00-17:30",
            PostMarketHours = null,
            CloseDate = "7,0",
            CountryCode = "DE",
            CountryName = "Germany",
            Url = "https://www.tradinghours.com/exchanges/xetr"
        },
        new Exchange
        {
            ExchangeCode = "DU",
            ExchangeName = "BOERSE DUESSELDORF",
            MicCode = "XDUS",
            TimeZone = "Europe/Berlin",
            PreMarketHours = null,
            TradingHours = "08:00:20:00",
            PostMarketHours = null,
            CloseDate = "7,0",
            CountryCode = "DE",
            CountryName = "Germany",
            Url = "https://www.tradinghours.com/exchanges/xdus"
        },
        new Exchange
        {
            ExchangeCode = "F",
            ExchangeName = "DEUTSCHE BOERSE AG",
            MicCode = "XFRA",
            TimeZone = "Europe/Berlin",
            PreMarketHours = null,
            TradingHours = "08:00-20:00",
            PostMarketHours = null,
            CloseDate = "7,0",
            CountryCode = "DE",
            CountryName = "Germany",
            Url = "https://www.tradinghours.com/exchanges/fsx"
        },
        new Exchange
        {
            ExchangeCode = "HE",
            ExchangeName = "NASDAQ OMX HELSINKI LTD",
            MicCode = "XHEL,FNFI",
            TimeZone = "Europe/Helsinki",
            PreMarketHours = null,
            TradingHours = "10:00-18:30",
            PostMarketHours = null,
            CloseDate = "7,0",
            CountryCode = "FI",
            CountryName = "Finland",
            Url = "https://www.tradinghours.com/exchanges/omxh-helsinki"
        },
        new Exchange
        {
            ExchangeCode = "HK",
            ExchangeName = "HONG KONG EXCHANGES AND CLEARING LTD",
            MicCode = "XHKG",
            TimeZone = "Asia/Hong_Kong",
            PreMarketHours = null,
            TradingHours = "09:00-16:10",
            PostMarketHours = null,
            CloseDate = "7,0",
            CountryCode = "HK",
            CountryName = "Hong Kong",
            Url = "https://www.tradinghours.com/exchanges/hkex"
        },
        new Exchange
        {
            ExchangeCode = "HM",
            ExchangeName = "HANSEATISCHE WERTPAPIERBOERSE HAMBURG",
            MicCode = "XHAM",
            TimeZone = "Europe/Berlin",
            PreMarketHours = null,
            TradingHours = "08:00-20:00",
            PostMarketHours = null,
            CloseDate = "7,0",
            CountryCode = "DE",
            CountryName = "Germany",
            Url = "https://www.tradinghours.com/exchanges/xham"
        },
        new Exchange
        {
            ExchangeCode = "IC",
            ExchangeName = "NASDAQ OMX ICELAND",
            MicCode = "XICE,FNIS",
            TimeZone = "Atlantic/Reykjavik",
            PreMarketHours = null,
            TradingHours = "09:30-15:30",
            PostMarketHours = null,
            CloseDate = "7,0",
            CountryCode = "IS",
            CountryName = "Iceland",
            Url = "https://www.tradinghours.com/exchanges/xice"
        },
        new Exchange
        {
            ExchangeCode = "IR",
            ExchangeName = "IRISH STOCK EXCHANGE - ALL MARKET",
            MicCode = "XDUB",
            TimeZone = "Europe/Dublin",
            PreMarketHours = null,
            TradingHours = "09:00-17:30",
            PostMarketHours = null,
            CloseDate = "7,0",
            CountryCode = "IE",
            CountryName = "Ireland",
            Url = "https://www.tradinghours.com/exchanges/ise"
        },
        new Exchange
        {
            ExchangeCode = "IS",
            ExchangeName = "BORSA ISTANBUL",
            MicCode = "XIST",
            TimeZone = "Europe/Istanbul",
            PreMarketHours = null,
            TradingHours = "09:40-18:10",
            PostMarketHours = null,
            CloseDate = "7,0",
            CountryCode = "TR",
            CountryName = "Turkey",
            Url = "https://www.tradinghours.com/exchanges/bist"
        },
        new Exchange
        {
            ExchangeCode = "JK",
            ExchangeName = "INDONESIA STOCK EXCHANGE",
            MicCode = "XIDX",
            TimeZone = "Asia/Jakarta",
            PreMarketHours = null,
            TradingHours = "08:45-15:15",
            PostMarketHours = null,
            CloseDate = "7,0",
            CountryCode = "ID",
            CountryName = "Indonesia",
            Url = "https://www.tradinghours.com/exchanges/idx"
        },
        new Exchange
        {
            ExchangeCode = "JO",
            ExchangeName = "JOHANNESBURG STOCK EXCHANGE",
            MicCode = "XJSE",
            TimeZone = "Africa/Johannesburg",
            PreMarketHours = null,
            TradingHours = "09:00-17:00",
            PostMarketHours = null,
            CloseDate = "7,0",
            CountryCode = "ZA",
            CountryName = "South Africa",
            Url = "https://www.tradinghours.com/exchanges/jse"
        },
        new Exchange
        {
            ExchangeCode = "KL",
            ExchangeName = "BURSA MALAYSIA",
            MicCode = "XKLS",
            TimeZone = "Asia/Kuala_Lumpur",
            PreMarketHours = null,
            TradingHours = "08:30-17:00",
            PostMarketHours = null,
            CloseDate = "7,0",
            CountryCode = "MY",
            CountryName = "Malaysia",
            Url = "https://www.tradinghours.com/exchanges/myx"
        },
        new Exchange
        {
            ExchangeCode = "KQ",
            ExchangeName = "KOREA EXCHANGE (KOSDAQ)",
            MicCode = "XKOS",
            TimeZone = "Asia/Seoul",
            PreMarketHours = null,
            TradingHours = "09:00-15:30",
            PostMarketHours = null,
            CloseDate = "7,0",
            CountryCode = "KP",
            CountryName = "Korea",
            Url = "https://www.tradinghours.com/exchanges/kosdaq"
        },
        new Exchange
        {
            ExchangeCode = "KS",
            ExchangeName = "KOREA EXCHANGE (STOCK MARKET)",
            MicCode = "XKRX",
            TimeZone = "Asia/Seoul",
            PreMarketHours = null,
            TradingHours = "08:00-18:00",
            PostMarketHours = null,
            CloseDate = "7,0",
            CountryCode = "KP",
            CountryName = "Korea",
            Url = "https://www.tradinghours.com/exchanges/krx"
        },
        new Exchange
        {
            ExchangeCode = "KW",
            ExchangeName = "Kuwait Stock Exchange",
            MicCode = "XKUW",
            TimeZone = "Asia/Kuwait",
            PreMarketHours = null,
            TradingHours = "09:00-13:15",
            PostMarketHours = null,
            CloseDate = "7,0",
            CountryCode = "KW",
            CountryName = "Kuwait",
            Url = "https://www.tradinghours.com/markets/xkuw"
        },
        new Exchange
        {
            ExchangeCode = "L",
            ExchangeName = "LONDON STOCK EXCHANGE",
            MicCode = "XLON",
            TimeZone = "Europe/London",
            PreMarketHours = null,
            TradingHours = "08:00-16:30",
            PostMarketHours = null,
            CloseDate = "7,0",
            CountryCode = "GB",
            CountryName = "UK",
            Url = "https://www.tradinghours.com/exchanges/lse"
        },
        new Exchange
        {
            ExchangeCode = "LS",
            ExchangeName = "NYSE EURONEXT - EURONEXT LISBON",
            MicCode = "XLIS",
            TimeZone = "Europe/Lisbon",
            PreMarketHours = null,
            TradingHours = "09:00-17:30",
            PostMarketHours = null,
            CloseDate = "7,0",
            CountryCode = "PT",
            CountryName = "Portugal",
            Url = "https://www.tradinghours.com/exchanges/euronext-lisbon"
        },
        new Exchange
        {
            ExchangeCode = "MC",
            ExchangeName = "BOLSA DE MADRID",
            MicCode = "XMAD",
            TimeZone = "Europe/Madrid",
            PreMarketHours = null,
            TradingHours = "09:00-17:30",
            PostMarketHours = null,
            CloseDate = "7,0",
            CountryCode = "ES",
            CountryName = "Spain",
            Url = "https://www.tradinghours.com/exchanges/bme"
        },
        new Exchange
        {
            ExchangeCode = "ME",
            ExchangeName = "MOSCOW EXCHANGE",
            MicCode = "MISX",
            TimeZone = "Europe/Moscow",
            PreMarketHours = null,
            TradingHours = "09:30-19:00",
            PostMarketHours = null,
            CloseDate = "7,0",
            CountryCode = "RU",
            CountryName = "Russia",
            Url = "https://www.tradinghours.com/exchanges/moex"
        },
        new Exchange
        {
            ExchangeCode = "MI",
            ExchangeName = "Italian Stock Exchange",
            MicCode = "XMIL",
            TimeZone = "Europe/Rome",
            PreMarketHours = null,
            TradingHours = "08:00-17:42",
            PostMarketHours = null,
            CloseDate = "7,0",
            CountryCode = "IT",
            CountryName = "Italy",
            Url = "https://www.tradinghours.com/exchanges/mta"
        },
        new Exchange
        {
            ExchangeCode = "MT",
            ExchangeName = "MALTA STOCK EXCHANGE",
            MicCode = "XMAL",
            TimeZone = "Europe/Malta",
            PreMarketHours = null,
            TradingHours = "09:30-15:30",
            PostMarketHours = null,
            CloseDate = "7,0",
            CountryCode = "MT",
            CountryName = "Malta",
            Url = "https://www.tradinghours.com/markets/mse"
        },
        new Exchange
        {
            ExchangeCode = "MU",
            ExchangeName = "BOERSE MUENCHEN",
            MicCode = "XMUN",
            TimeZone = "Europe/Berlin",
            PreMarketHours = null,
            TradingHours = "08:00-20:00",
            PostMarketHours = null,
            CloseDate = "7,0",
            CountryCode = "DE",
            CountryName = "Germany",
            Url = "https://www.tradinghours.com/exchanges/xmun"
        },
        new Exchange
        {
            ExchangeCode = "MX",
            ExchangeName = "BOLSA MEXICANA DE VALORES (MEXICAN STOCK EXCHANGE)",
            MicCode = "XMEX",
            TimeZone = "America/Mexico_City",
            PreMarketHours = null,
            TradingHours = "08:00-15:10",
            PostMarketHours = null,
            CloseDate = "7,0",
            CountryCode = "MX",
            CountryName = "Mexico",
            Url = "https://www.tradinghours.com/exchanges/bmv"
        },
        new Exchange
        {
            ExchangeCode = "NE",
            ExchangeName = "AEQUITAS NEO EXCHANGE",
            MicCode = "NEOE",
            TimeZone = "America/Toronto",
            PreMarketHours = null,
            TradingHours = "09:30-16:00",
            PostMarketHours = null,
            CloseDate = "7,0",
            CountryCode = "CA",
            CountryName = "Canada",
            Url = "https://www.tradinghours.com/exchanges/neo"
        },
        new Exchange
        {
            ExchangeCode = "NL",
            ExchangeName = "Nigerian Stock Exchange",
            MicCode = "XNSA",
            TimeZone = "Africa/Lagos",
            PreMarketHours = null,
            TradingHours = "09:30-14:30",
            PostMarketHours = null,
            CloseDate = "7,0",
            CountryCode = "NG",
            CountryName = "Nigeria",
            Url = "https://www.tradinghours.com/markets/nse-nigeria"
        },
        new Exchange
        {
            ExchangeCode = "NS",
            ExchangeName = "NATIONAL STOCK EXCHANGE OF INDIA",
            MicCode = "XNSE",
            TimeZone = "Asia/Kolkata",
            PreMarketHours = null,
            TradingHours = "09:00-16:00",
            PostMarketHours = null,
            CloseDate = "7,0",
            CountryCode = "IN",
            CountryName = "India",
            Url = "https://www.tradinghours.com/exchanges/nse-india"
        },
        new Exchange
        {
            ExchangeCode = "NZ",
            ExchangeName = "NEW ZEALAND EXCHANGE LTD",
            MicCode = "XNZE",
            TimeZone = "Pacific/Auckland",
            PreMarketHours = null,
            TradingHours = "10:00-16:45",
            PostMarketHours = null,
            CloseDate = "7,0",
            CountryCode = "NZ",
            CountryName = "New Zealand",
            Url = "https://www.tradinghours.com/exchanges/nzx"
        },
        new Exchange
        {
            ExchangeCode = "OL",
            ExchangeName = "OSLO BORS ASA",
            MicCode = "XOSL",
            TimeZone = "Europe/Oslo",
            PreMarketHours = null,
            TradingHours = "08:15-17:30",
            PostMarketHours = null,
            CloseDate = "7,0",
            CountryCode = "NO",
            CountryName = "Norway",
            Url = "https://www.tradinghours.com/exchanges/ose"
        },
        new Exchange
        {
            ExchangeCode = "OM",
            ExchangeName = "Muscat Stock Exchange",
            MicCode = "XMUS",
            TimeZone = "Asia/Muscat",
            PreMarketHours = null,
            TradingHours = "10:00-14:00",
            PostMarketHours = null,
            CloseDate = "6,7",
            CountryCode = "OM",
            CountryName = "Oman",
            Url = null
        },
        new Exchange
        {
            ExchangeCode = "PA",
            ExchangeName = "NYSE EURONEXT - MARCHE LIBRE PARIS",
            MicCode = "XPAR",
            TimeZone = "Europe/Paris",
            PreMarketHours = null,
            TradingHours = "09:00-17:30",
            PostMarketHours = null,
            CloseDate = "7,0",
            CountryCode = "FR",
            CountryName = "France",
            Url = "https://www.tradinghours.com/exchanges/euronext-paris"
        },
        new Exchange
        {
            ExchangeCode = "PM",
            ExchangeName = "Philippine Stock Exchange",
            MicCode = "XPHS",
            TimeZone = "Asia/Manila",
            PreMarketHours = null,
            TradingHours = "09:30-13:00",
            PostMarketHours = null,
            CloseDate = "7,0",
            CountryCode = "PH",
            CountryName = "Philipppine",
            Url = "https://www.tradinghours.com/markets/pse"
        },
        new Exchange
        {
            ExchangeCode = "PR",
            ExchangeName = "PRAGUE STOCK EXCHANGE",
            MicCode = "XPRA",
            TimeZone = "Europe/Prague",
            PreMarketHours = null,
            TradingHours = "08:00-17:00",
            PostMarketHours = null,
            CloseDate = "7,0",
            CountryCode = "CZ",
            CountryName = "Czech",
            Url = "https://www.tradinghours.com/exchanges/xpra"
        },
        new Exchange
        {
            ExchangeCode = "QA",
            ExchangeName = "QATAR EXCHANGE",
            MicCode = "DSMD",
            TimeZone = "Asia/Qatar",
            PreMarketHours = null,
            TradingHours = "09:00-13:15",
            PostMarketHours = null,
            CloseDate = "7,0",
            CountryCode = "QA",
            CountryName = "Qatar",
            Url = "https://www.tradinghours.com/exchanges/qe"
        },
        new Exchange
        {
            ExchangeCode = "RO",
            ExchangeName = "BUCHAREST STOCK EXCHANGE",
            MicCode = "XBSE",
            TimeZone = "Europe/Bucharest",
            PreMarketHours = null,
            TradingHours = "10:00-17:45",
            PostMarketHours = null,
            CloseDate = "7,0",
            CountryCode = "RO",
            CountryName = "Romania",
            Url = "https://www.tradinghours.com/markets/bvb"
        },
        new Exchange
        {
            ExchangeCode = "RG",
            ExchangeName = "NASDAQ OMX RIGA",
            MicCode = "XRIS",
            TimeZone = "Europe/Riga",
            PreMarketHours = null,
            TradingHours = "09:00-16:30",
            PostMarketHours = null,
            CloseDate = "7,0",
            CountryCode = "LV",
            CountryName = "Latvia",
            Url = "https://www.tradinghours.com/exchanges/omxr-riga"
        },
        new Exchange
        {
            ExchangeCode = "SA",
            ExchangeName = "Brazil Bolsa - Sao Paolo",
            MicCode = "BVMF",
            TimeZone = "America/Sao_Paulo",
            PreMarketHours = null,
            TradingHours = "09:45-18:45",
            PostMarketHours = null,
            CloseDate = "7,0",
            CountryCode = "BR",
            CountryName = "Brazil",
            Url = "https://www.tradinghours.com/exchanges/bovespa"
        },
        new Exchange
        {
            ExchangeCode = "SG",
            ExchangeName = "BOERSE STUTTGART",
            MicCode = "XSTU",
            TimeZone = "Asia/Amman",
            PreMarketHours = null,
            TradingHours = "08:00-20:00",
            PostMarketHours = null,
            CloseDate = "7,0",
            CountryCode = "DE",
            CountryName = "Germany",
            Url = "https://www.tradinghours.com/exchanges/xstu"
        },
        new Exchange
        {
            ExchangeCode = "SI",
            ExchangeName = "SINGAPORE EXCHANGE",
            MicCode = "XSES",
            TimeZone = "Asia/Singapore",
            PreMarketHours = null,
            TradingHours = "08:30-17:16",
            PostMarketHours = null,
            CloseDate = "7,0",
            CountryCode = "SG",
            CountryName = "Singapore",
            Url = "https://www.tradinghours.com/exchanges/sgx"
        },
        new Exchange
        {
            ExchangeCode = "SN",
            ExchangeName = "SANTIAGO STOCK EXCHANGE",
            MicCode = "XSGO",
            TimeZone = "America/Santiago",
            PreMarketHours = null,
            TradingHours = "09:30-16:00",
            PostMarketHours = null,
            CloseDate = "7,0",
            CountryCode = "CL",
            CountryName = "Chile",
            Url = "https://www.tradinghours.com/exchanges/bvs"
        },
        new Exchange
        {
            ExchangeCode = "SR",
            ExchangeName = "SAUDI STOCK EXCHANGE",
            MicCode = "XSAU",
            TimeZone = "Asia/Riyadh",
            PreMarketHours = null,
            TradingHours = "10:00-15:10",
            PostMarketHours = null,
            CloseDate = "6,7",
            CountryCode = "SA",
            CountryName = "Saudi",
            Url = "https://www.tradinghours.com/exchanges/tadawul"
        },
        new Exchange
        {
            ExchangeCode = "SS",
            ExchangeName = "SHANGHAI STOCK EXCHANGE",
            MicCode = "XSHG",
            TimeZone = "Asia/Shanghai",
            PreMarketHours = null,
            TradingHours = "09:15-15:30",
            PostMarketHours = null,
            CloseDate = "7,0",
            CountryCode = "CN",
            CountryName = "China",
            Url = "https://www.tradinghours.com/exchanges/sse"
        },
        new Exchange
        {
            ExchangeCode = "ST",
            ExchangeName = "NASDAQ OMX NORDIC STOCKHOLM",
            MicCode = "XSTO,FNSE",
            TimeZone = "Europe/Stockholm",
            PreMarketHours = null,
            TradingHours = "09:00-17:30",
            PostMarketHours = "17:30-18:00",
            CloseDate = "7,0",
            CountryCode = "SE",
            CountryName = "Sweden",
            Url = "https://www.tradinghours.com/exchanges/xngm"
        },
        new Exchange
        {
            ExchangeCode = "SW",
            ExchangeName = "SWISS EXCHANGE",
            MicCode = "XSWX",
            TimeZone = "Europe/Zurich",
            PreMarketHours = null,
            TradingHours = "09:30-17:00",
            PostMarketHours = null,
            CloseDate = "7,0",
            CountryCode = "CH",
            CountryName = "Switzerland",
            Url = "https://www.tradinghours.com/exchanges/six"
        },
        new Exchange
        {
            ExchangeCode = "SZ",
            ExchangeName = "SHENZHEN STOCK EXCHANGE",
            MicCode = "XSHE",
            TimeZone = "Asia/Shanghai",
            PreMarketHours = null,
            TradingHours = "09:15-15:00",
            PostMarketHours = null,
            CloseDate = "7,0",
            CountryCode = "CN",
            CountryName = "China",
            Url = "https://www.tradinghours.com/exchanges/szse"
        },
        new Exchange
        {
            ExchangeCode = "T",
            ExchangeName = "TOKYO STOCK EXCHANGE-TOKYO PRO MARKET",
            MicCode = "XJPX",
            TimeZone = "Asia/Tokyo",
            PreMarketHours = null,
            TradingHours = "09:00-15:00",
            PostMarketHours = null,
            CloseDate = "7,0",
            CountryCode = "JP",
            CountryName = "Japan",
            Url = "https://www.tradinghours.com/exchanges/jpx"
        },
        new Exchange
        {
            ExchangeCode = "TA",
            ExchangeName = "TEL AVIV STOCK EXCHANGE",
            MicCode = "XTAE",
            TimeZone = "Asia/Jerusalem",
            PreMarketHours = null,
            TradingHours = "09:45-17:14",
            PostMarketHours = null,
            CloseDate = "6,7",
            CountryCode = "IL",
            CountryName = "Israel",
            Url = "https://www.tradinghours.com/exchanges/tase"
        },
        new Exchange
        {
            ExchangeCode = "TL",
            ExchangeName = "NASDAQ OMX TALLINN",
            MicCode = "XTAL",
            TimeZone = "Europe/Tallinn",
            PreMarketHours = null,
            TradingHours = "09:00-16:30",
            PostMarketHours = null,
            CloseDate = "7,0",
            CountryCode = "EE",
            CountryName = "Estonia",
            Url = "https://www.tradinghours.com/exchanges/omxt-tallinn"
        },
        new Exchange
        {
            ExchangeCode = "TO",
            ExchangeName = "TORONTO STOCK EXCHANGE",
            MicCode = "XTSE",
            TimeZone = "America/Toronto",
            PreMarketHours = "07:00-09:30",
            TradingHours = "09:30-16:00",
            PostMarketHours = "16:00-17:00",
            CloseDate = "7,0",
            CountryCode = "CA",
            CountryName = "Canada",
            Url = "https://www.tradinghours.com/exchanges/tsx"
        },
        new Exchange
        {
            ExchangeCode = "TW",
            ExchangeName = "TAIWAN STOCK EXCHANGE",
            MicCode = "XTAI",
            TimeZone = "Asia/Taipei",
            PreMarketHours = null,
            TradingHours = "09:00-17:00",
            PostMarketHours = null,
            CloseDate = "7,0",
            CountryCode = "TW",
            CountryName = "Taiwan",
            Url = "https://www.tradinghours.com/exchanges/twse"
        },
        new Exchange
        {
            ExchangeCode = "TWO",
            ExchangeName = "TPEx",
            MicCode = "ROCO",
            TimeZone = "Asia/Taipei",
            PreMarketHours = null,
            TradingHours = "09:00-17:00",
            PostMarketHours = null,
            CloseDate = "7,0",
            CountryCode = "TW",
            CountryName = "Taiwan",
            Url = null
        },
        new Exchange
        {
            ExchangeCode = "US",
            ExchangeName = "US exchanges (NYSE, Nasdaq)",
            MicCode = "XNYS,XASE,BATS,ARCX,XNMS,XNCM,XNGS,IEXG,XNAS, OTCM, OOTC",
            TimeZone = "America/New_York",
            PreMarketHours = "04:00-09:30",
            TradingHours = "09:30-16:00",
            PostMarketHours = "16:00-20:00",
            CloseDate = "7,0",
            CountryCode = "US",
            CountryName = "US",
            Url = "https://www.tradinghours.com/exchanges/nyse"
        },
        new Exchange
        {
            ExchangeCode = "V",
            ExchangeName = "TSX VENTURE EXCHANGE - NEX",
            MicCode = "XTSX",
            TimeZone = "America/Toronto",
            PreMarketHours = "07:00-09:30",
            TradingHours = "09:30-16:15",
            PostMarketHours = "16:00-17:00",
            CloseDate = "7,0",
            CountryCode = "CA",
            CountryName = "Canada",
            Url = "https://www.tradinghours.com/exchanges/xtsx"
        },
        new Exchange
        {
            ExchangeCode = "VI",
            ExchangeName = "Vienna Stock Exchange",
            MicCode = "XWBO",
            TimeZone = "Europe/Vienna",
            PreMarketHours = null,
            TradingHours = "09:04-17:30",
            PostMarketHours = null,
            CloseDate = "7,0",
            CountryCode = "AT",
            CountryName = "Austria",
            Url = "https://www.tradinghours.com/exchanges/vse"
        },
        new Exchange
        {
            ExchangeCode = "VN",
            ExchangeName = "Vietnam exchanges including HOSE, HNX and UPCOM",
            MicCode = "HSTC, XSTC,XHNX",
            TimeZone = "Asia/Bangkok",
            PreMarketHours = null,
            TradingHours = "09:00-15:00",
            PostMarketHours = null,
            CloseDate = "7,0",
            CountryCode = "VN",
            CountryName = "Vietnam",
            Url = "https://www.tradinghours.com/exchanges/hose"
        },
        new Exchange
        {
            ExchangeCode = "VS",
            ExchangeName = "NASDAQ OMX VILNIUS",
            MicCode = "XLIT",
            TimeZone = "Europe/Vilnius",
            PreMarketHours = null,
            TradingHours = "09:00-16:30",
            PostMarketHours = null,
            CloseDate = "7,0",
            CountryCode = "LT",
            CountryName = "Lithuania",
            Url = "https://www.tradinghours.com/exchanges/omxv-vilnius"
        },
        new Exchange
        {
            ExchangeCode = "WA",
            ExchangeName = "WARSAW STOCK EXCHANGE/EQUITIES/MAIN MARKET",
            MicCode = "XWAR",
            TimeZone = "Europe/Warsaw",
            PreMarketHours = null,
            TradingHours = "08:30-17:05",
            PostMarketHours = null,
            CloseDate = "7,0",
            CountryCode = "PL",
            CountryName = "Poland",
            Url = "https://www.tradinghours.com/exchanges/gpw"
        },
        new Exchange
        {
            ExchangeCode = "PK",
            ExchangeName = "Pakistan Stock Exchange",
            MicCode = "XKAR",
            TimeZone = "Asia/Karachi",
            PreMarketHours = "09:15-09:30",
            TradingHours = "09:30-15:30",
            PostMarketHours = "15:30-15:50",
            CloseDate = "7,0",
            CountryCode = "PK",
            CountryName = "Pakistan",
            Url = "https://www.tradinghours.com/markets/psx"
        },
        new Exchange
        {
            ExchangeCode = "HA",
            ExchangeName = "Hanover Stock Exchange",
            MicCode = "XHAN",
            TimeZone = "Europe/Berlin",
            PreMarketHours = null,
            TradingHours = "08:00-20:00",
            PostMarketHours = null,
            CloseDate = "7,0",
            CountryCode = "DE",
            CountryName = "Germany",
            Url = null
        },
        new Exchange
        {
            ExchangeCode = "SX",
            ExchangeName = "DEUTSCHE BOERSE Stoxx",
            MicCode = null,
            TimeZone = "Europe/Berlin",
            PreMarketHours = null,
            TradingHours = "08:00-20:00",
            PostMarketHours = null,
            CloseDate = "7,0",
            CountryCode = "DE",
            CountryName = "Germany",
            Url = null
        },
        new Exchange
        {
            ExchangeCode = "TG",
            ExchangeName = "DEUTSCHE BOERSE TradeGate",
            MicCode = "XGAT",
            TimeZone = "Europe/Berlin",
            PreMarketHours = null,
            TradingHours = "08:00-22:00",
            PostMarketHours = null,
            CloseDate = "7,0",
            CountryCode = "DE",
            CountryName = "Germany",
            Url = null
        },
        new Exchange
        {
            ExchangeCode = "SC",
            ExchangeName = "BOERSE_FRANKFURT_ZERTIFIKATE",
            MicCode = null,
            TimeZone = "Europe/Berlin",
            PreMarketHours = null,
            TradingHours = "08:00-20:00",
            PostMarketHours = null,
            CloseDate = "7,0",
            CountryCode = "DE",
            CountryName = "Germany",
            Url = null
        },
    ];
}
