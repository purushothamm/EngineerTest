using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using EngineerTest.Class;
using System.Text;
using EngineerTest.Models;
using Microsoft.AspNetCore.Mvc;
//using demo.MVC.Models;

namespace EngineerTest.Controllers
{
    public class TmdbApiController : Controller
    {
        // GET: TmdbApi
        public ActionResult Index(string MovieName, int? page)
        {
            if (page != null)
                CallAPI(MovieName, Convert.ToInt32(page));

            Models.TheMovieDb theMovieDb = new Models.TheMovieDb();
            theMovieDb.searchText = MovieName;
            return View(theMovieDb);
        }

        [HttpPost]
        public ActionResult Index(Models.TheMovieDb theMovieDb, string searchText)
        {
            if (ModelState.IsValid)
            {
                CallAPI(searchText, 0);
            }
            return View(theMovieDb);
        }
        public void CallAPI(string searchText, int page)
        {
            int pageNo = Convert.ToInt32(page) == 0 ? 1 : Convert.ToInt32(page);

            /*Calling API https://developers.themoviedb.org/3/search/Movie */
            string apiKey = "1660ca5fe5b0e26bdcd9c838a9b5c7c7";
            HttpWebRequest apiRequest = WebRequest.Create("https://api.themoviedb.org/3/search/Movie?api_key=" + apiKey + "&language=en-US&query=" + searchText + "&page=" + pageNo + "&include_adult=false") as HttpWebRequest;

            string apiResponse = "";
            using (HttpWebResponse response = apiRequest.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());
                apiResponse = reader.ReadToEnd();
            }
            /*End*/

            /*http://json2csharp.com*/
            //ResponseSearchPeople rootObject = JsonConvert.DeserializeObject<ResponseSearchPeople>(apiResponse);
            ResponseSearchMovie rootObject = JsonConvert.DeserializeObject<ResponseSearchMovie>(apiResponse);

            StringBuilder sb = new StringBuilder();
            sb.Append("<div class=\"resultDiv\"><p>Names</p>");
            foreach (Result result in rootObject.results)
            {
                string image = result.poster_path == null ? Url.Content("~/Content/Image/no-image.png") : "https://image.tmdb.org/t/p/w500/" + result.poster_path;
                string link = Url.Action("GetMovie", "TmdbApi", new { id = result.id });
                sb.Append("<div class=\"result\" resourceId=\"" + result.id + "\">" + "<a href=\"" + link + "\"><img src=\"" + image + "\" />" + "<p>" + result.title + "</a></p></div>");
            }
            ViewBag.Result = sb.ToString();

            int pageSize = 20;
            PagingInfo pagingInfo = new PagingInfo();
            pagingInfo.CurrentPage = pageNo;
            pagingInfo.TotalItems = rootObject.total_results;
            pagingInfo.ItemsPerPage = pageSize;
            ViewBag.Paging = pagingInfo;
        }

        public ActionResult GetMovie(int id)
        {
            /*Calling API https://developers.themoviedb.org/3/people */
            string apiKey = "1660ca5fe5b0e26bdcd9c838a9b5c7c7";
            HttpWebRequest apiRequest = WebRequest.Create("https://api.themoviedb.org/3/Movie/" + id + "?api_key=" + apiKey + "&language=en-US") as HttpWebRequest;

            string apiResponse = "";
            using (HttpWebResponse response = apiRequest.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());
                apiResponse = reader.ReadToEnd();
            }
            /*End*/

            /*http://json2csharp.com*/
            ResponseMovie rootObject = JsonConvert.DeserializeObject<ResponseMovie>(apiResponse);
            
            TheMovieDb theMovieDb = new TheMovieDb();
            theMovieDb.adult = rootObject.adult;
            theMovieDb.backdrop_path = rootObject.backdrop_path;
            theMovieDb.belongs_to_collection = rootObject.belongs_to_collection;
            theMovieDb.homepage = rootObject.homepage;
            theMovieDb.id = rootObject.id;
            theMovieDb.imdb_id = rootObject.imdb_id;
            theMovieDb.original_language = rootObject.original_language;
            theMovieDb.original_title = rootObject.original_title;
            theMovieDb.overview = rootObject.overview;
            theMovieDb.popularity = rootObject.popularity;
            theMovieDb.poster_path = rootObject.poster_path;
            theMovieDb.release_date = rootObject.release_date;
            theMovieDb.revenue = rootObject.revenue;
            theMovieDb.runtime = rootObject.runtime;
            theMovieDb.status = rootObject.status;
            theMovieDb.tagline = rootObject.tagline;
            theMovieDb.title = rootObject.title;
            theMovieDb.video = rootObject.video;
            theMovieDb.vote_average = rootObject.vote_average;
            theMovieDb.vote_count = rootObject.vote_count;

            return View(theMovieDb);
        }
    }
}