using System.Globalization;
using System.Net;
using System.Reflection.Metadata;
using HtmlAgilityPack;
using Newtonsoft.Json.Linq;
using PaperHelper.Entities.Entities;

namespace PaperHelper.Utils;

/// <summary>
/// Paper抓取信息
/// </summary>
public class PaperInfo
{
    public IFormFile File { get; set; }
    public Paper Paper { get; set; }
}

/// <summary>
/// 论文抓取类
/// </summary>
public class PaperSpider
{
    private readonly Uri _sciHubUrl;
    private readonly Uri _doiUrl;
    private readonly Uri _abstractUrl;
    private readonly HttpClient _client;

    public PaperSpider(IConfiguration configuration)
    {
        _sciHubUrl = new Uri(configuration.GetValue<string>("SciHubUrl"));
        _doiUrl = new Uri(configuration.GetValue<string>("DoiUrl"));
        _abstractUrl = new Uri(configuration.GetValue<string>("AbstractUrl"));
        _client = new HttpClient();
    }
    
    /// <summary>
    /// 获取scihub信息
    /// </summary>
    private async Task<string> FetchSciHub(string url)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, _sciHubUrl);
        request.Headers.Add("authority", "sci-hub.wf");
        request.Headers.Add("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
        request.Headers.Add("accept-language", "zh-CN,zh;q=0.9,en;q=0.8,zh-TW;q=0.7");
        request.Headers.Add("cache-control", "no-cache");
        //request.Headers.Add("content-type", "application/x-www-form-urlencoded");
        request.Headers.Add("origin", "https://sci-hub.ren");
        request.Headers.Add("pragma", "no-cache");
        request.Headers.Add("referer", "https://sci-hub.ren/");
        request.Headers.Add("sec-ch-ua", "\" Not A;Brand\";v=\"99\", \"Chromium\";v=\"102\", \"Google Chrome\";v=\"102\"");
        request.Headers.Add("sec-ch-ua-mobile", "?0");
        request.Headers.Add("sec-ch-ua-platform", "\"Windows\"");
        request.Headers.Add("sec-fetch-dest", "document");
        request.Headers.Add("sec-fetch-mode", "navigate");
        request.Headers.Add("sec-fetch-site", "cross-site");
        request.Headers.Add("sec-fetch-user", "?1");
        request.Headers.Add("upgrade-insecure-requests", "1");
        request.Headers.Add("user-agent","Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/102.0.0.0 Safari/537.36");
        
        request.Content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
        {
            new ("request", url),
        });
        var response = await _client.SendAsync(request);
        return await response.Content.ReadAsStringAsync();
    }
    
    /// <summary>
    /// 获取详细信息
    /// </summary>
    private async Task<string> FetchDoiInfo(string doi)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, new Uri(_doiUrl, doi));
        request.Headers.Add("Accept", "text/bibliography; style=bibtex");
        
        var response = await _client.SendAsync(request);
        return await response.Content.ReadAsStringAsync();
    }
    
    /// <summary>
    /// 获取摘要信息
    /// </summary>
    private async Task<string> FetchAbstract(string doi)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, new Uri(_abstractUrl, doi));
        
        request.Headers.Add("Accept", "application/json");
        request.Headers.Add("Accept-Language", "zh-CN,zh;q=0.9,en;q=0.8,zh-TW;q=0.7");
        request.Headers.Add("Cache-Control", "no-cache");
        request.Headers.Add("Connection", "keep-alive");
        request.Headers.Add("Origin", "https://www.scholarcy.com");
        request.Headers.Add("Pragma", "no-cache");
        request.Headers.Add("Referer", "https://www.scholarcy.com/");
        request.Headers.Add("Sec-Fetch-Dest", "empty");
        request.Headers.Add("Sec-Fetch-Mode", "cors");
        request.Headers.Add("Sec-Fetch-Site", "same-site");
        request.Headers.Add("user-agent","Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/102.0.0.0 Safari/537.36");
        request.Headers.Add("sec-ch-ua", "\" Not A;Brand\";v=\"99\", \"Chromium\";v=\"102\", \"Google Chrome\";v=\"102\"");
        request.Headers.Add("sec-ch-ua-mobile", "?0");
        request.Headers.Add("sec-ch-ua-platform", "\"Windows\"");
        
        var response = await _client.SendAsync(request);
        return await response.Content.ReadAsStringAsync();
    }
    
    /// <summary>
    /// 下载pdf
    /// </summary>
    private async Task<IFormFile> FetchFile(Uri url)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
        request.Headers.Add("Accept-Language", "zh-CN,zh;q=0.9,en;q=0.8,zh-TW;q=0.7");
        request.Headers.Add("Cache-Control", "no-cache");
        request.Headers.Add("Connection", "keep-alive");
        request.Headers.Add("Host", "sci-hub.wf");
        request.Headers.Add("Pragma", "no-cache");
        request.Headers.Add("Sec-Fetch-Dest", "document");
        request.Headers.Add("Sec-Fetch-Mode", "navigate");
        request.Headers.Add("Sec-Fetch-Site", "cross-site");
        request.Headers.Add("Sec-Fetch-User", "?1");
        request.Headers.Add("Upgrade-Insecure-Requests", "1");
        request.Headers.Add("user-agent","Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/102.0.0.0 Safari/537.36");
    
        var response = await _client.SendAsync(request);
        var stream = await response.Content.ReadAsStreamAsync();
        var file = new FormFile(stream, 0, stream.Length, Path.GetFileNameWithoutExtension(url.LocalPath), Path.GetFileName(url.LocalPath));
        return file;
    }
    
    /// <summary>
    /// 获取DOI及下载地址
    /// </summary>
    /// <param name="url">url</param>
    /// <returns></returns>
    public async Task<(string?, string?)> GetDoi(string url)
    {
        var sciHubHtml = await FetchSciHub(url);
        var sciHubDoc = new HtmlDocument();
        sciHubDoc.LoadHtml(sciHubHtml);

        var fileNode = sciHubDoc.DocumentNode.SelectSingleNode("//*[@id='pdf']");
        
        if (fileNode == null)
        {
            return (null, null);
        }

        var basicInfo = sciHubDoc.DocumentNode.SelectSingleNode("//head/title").InnerText.Split(" | ");
        var paperUrl = fileNode.Attributes["src"].Value;
        return (basicInfo[2], paperUrl);
    }
    
    /// <summary>
    /// 获取论文
    /// </summary>
    /// <param name="doi">doi</param>
    /// <param name="url">文件url</param>
    /// <returns>论文信息及文件</returns>
    public async Task<PaperInfo?> GetPaper(string doi, string url)
    {
        var paperUrl = new Uri(url);
        var file = await FetchFile(paperUrl);
        var doiInfo = await FetchDoiInfo(doi);
        var abstractJson = await FetchAbstract(doi);
        
        
        var paperInfo = new PaperInfo
        {
            Paper = new Paper
            {
                Doi = doi
            }
        };
        
        // info from doi
        var bib = BibtexLibrary.BibtexImporter.FromString(doiInfo);
        var bibTags = bib.Entries.First().Tags;
        
        var authors = bibTags.GetValueOrDefault("author", "").Split(", ");
        
        paperInfo.Paper.Title = bibTags["title"];
        paperInfo.Paper.Authors = JArray.FromObject(authors).ToString();

        var urlStr = bibTags.GetValueOrDefault("url", "");
        if (urlStr != "")
        {
            paperInfo.Paper.Url = new Uri(urlStr);
        }

        paperInfo.Paper.Publication = bibTags.GetValueOrDefault("journal", null);
        
        var yearStr = bibTags.GetValueOrDefault("year", "");
        if (yearStr != "")
        {
            paperInfo.Paper.Year = int.Parse(yearStr);
        }
        
        var monthStr = bibTags.GetValueOrDefault("month", "");
        if (monthStr != "")
        {
            paperInfo.Paper.Month = DateTime.ParseExact(bibTags["month"], "MMM", CultureInfo.InvariantCulture).Month;
        }
        
        paperInfo.Paper.Volume = bibTags.GetValueOrDefault("volume", null);
        
        // abstract
        var abstractJsonObj = JObject.Parse(abstractJson);
        
        var abstractNode = abstractJsonObj.GetValue("abstract");
        if (abstractNode != null)
        {
            paperInfo.Paper.Abstract = abstractNode.ToString();
        }
        
        var keywordsNode = abstractJsonObj.GetValue("keywords");
        if (keywordsNode != null)
        {
            paperInfo.Paper.Keywords = keywordsNode.ToString();
        }
        
        // file
        paperInfo.File = file;

        return paperInfo;
    }
}