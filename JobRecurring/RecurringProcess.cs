using HtmlAgilityPack;
using JobRecurring.Enum;
using JobRecurring.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace JobRecurring
{
   public class RecurringProcess
    {
        private readonly Timer _timer;
        public RecurringProcess()
        {
            _timer = new Timer(3600000) { AutoReset = true };
            _timer.Elapsed += TimerElapsed;
        }
        private void TimerElapsed(object sender,ElapsedEventArgs e)
        {
            var DayOfWeek = DateTime.UtcNow.DayOfWeek.ToString();
            var ListMovieRecurring = new List<Movie>();
            using (var db = new DbMeChillEntities())
            {

                ListMovieRecurring = db.Movies.Where(x =>
                x.RecurringTime != null
                && x.Status != 5
                && ((x.RecurringTime.Value.Date == DateTime.UtcNow.Date && x.RecurringTime.Value.Hour == DateTime.UtcNow.Hour && x.RecurringRepeat == RecurringEnum.NoRepeart.ToString()) ||
                (x.RecurringTime.Value.Date <= DateTime.UtcNow.Date && x.RecurringTime.Value.Hour == DateTime.UtcNow.Hour && x.RecurringRepeat == RecurringEnum.Daily.ToString())
                ||
                (x.RecurringTime.Value.Date <= DateTime.UtcNow.Date && x.RecurringTime.Value.Hour == DateTime.UtcNow.Hour && x.RecurringRepeat == RecurringEnum.Weekly.ToString() && x.DaysOfWeek.Contains(DayOfWeek)))).ToList();
            }

            foreach (var movie in ListMovieRecurring)
            {
                if (!string.IsNullOrEmpty(movie.RecurringURL))
                {
                    this.ProcessMovie(movie);
                }

            }
        }

        private static string GetNumbers(string input)
        {
            return new string(input.Where(c => char.IsDigit(c)).ToArray());
        }
        private string ConvertKeywordEpisode(string MovieName, string OtherName, int Episode, int? Type)
        {
            var keyword = new List<string>();
            string[] webphim = { "motchill", "motphim", "phimmoi", "tvhay", "hayghe", "subnhanh", "bichill", "vieon", "fptplay", "netflix", "youtube", "bilutv", "phim1080", "fullphim", "dongphim", "tvzing", "luotphim", "wetv" };

            if (Type == 2)
            {
                keyword.Add("xem phim " + MovieName.ToLower() + " tập " + Episode + " vietsub");
                keyword.Add("xem phim " + MovieName.ToLower() + " tập " + Episode + " thuyết minh");

                keyword.Add("xem phim " + MovieName.ToLower() + " tập " + Episode + " hd");
                if (!string.IsNullOrEmpty(OtherName))
                {
                    keyword.Add(MovieName.ToLower() + " tập " + Episode + " vietsub");
                    keyword.Add(MovieName.ToLower() + " tập " + Episode + " thuyết minh");
                }
                keyword.AddRange(webphim);

            }
            else
            {
                keyword.Add("xem phim " + MovieName.ToLower() + " full vietsub");
                keyword.Add("xem phim " + MovieName.ToLower() + " full thuyết minh");
                keyword.Add("xem phim " + MovieName.ToLower() + " full lồng tiếng");
                keyword.Add("xem phim " + MovieName.ToLower() + " full hd");
                if (!string.IsNullOrEmpty(OtherName))
                {
                    keyword.Add(MovieName.ToLower() + " full vietsub");
                    keyword.Add(MovieName.ToLower() + " full thuyết minh");
                }
                keyword.AddRange(webphim);

            }



            return string.Join(", ", keyword);
        }

   
        public void ProcessMovie(Movie mMovie)
        {
            try
            {
                var db = new DbMeChillEntities();
                int TypeId = 2;
                var movie = db.Movies.Where(x => x.Id == mMovie.Id).FirstOrDefault();
                string content;
                using (WebClient wc = new WebClient())
                {
                    content = wc.DownloadString(mMovie.RecurringURL);
                }
                var doc = new HtmlDocument();

                doc.LoadHtml(content);
                var anchor = doc.DocumentNode.SelectNodes("//a");
                var ListEpisode = new List<EpisodeModel>();
                foreach (var item in anchor.Where(x => x.Attributes["href"]?.Value.Contains("xem-phim") == true))
                {
                    var episode = new EpisodeModel();
                    string link = item.Attributes["href"].Value;
                    episode.Name = item.InnerText;
                    string text = item.InnerText;
                    try
                    {
                        episode.EpisodeNumber = int.Parse(GetNumbers(text.Split('-')[0]));
                    }
                    catch
                    {
                        continue;
                    
                    }

                    string ContentEpisode = "";
                    using (WebClient wc = new WebClient())
                    {
                        ContentEpisode = wc.DownloadString(link);
                    }
                    var docEpisode = new HtmlDocument();

                    docEpisode.LoadHtml(ContentEpisode);

                    if (movie.RecurringURL.Contains("animehay"))
                    {
                        TypeId = 1;
                        var anchorEpisode = docEpisode.DocumentNode.SelectNodes("//*[contains(., 'https://playhydrax.com/')]")[0];

                        if (anchorEpisode != null)
                        {
                            episode.SupplierId = 4;
                            var html = anchorEpisode.InnerHtml.ToString();
                            var indexof = html.IndexOf("https://playhydrax.com/");
                            var videoId = html.Substring(indexof + 23, 20);
                            videoId = videoId.Substring(0, videoId.IndexOf('"'));
                            episode.VideoId = videoId;
                            ListEpisode.Add(episode);
                        }
                    }
                    else
                    {
                        var anchorEpisode = docEpisode.DocumentNode.SelectSingleNode("//iframe");
                        if (anchorEpisode != null)
                        {
                            string src = anchorEpisode.Attributes["src"].Value;
                            if (src.Contains("ok.ru/videoembed/"))
                            {
                                episode.SupplierId = 5;
                                var last = src.IndexOf("ok.ru/videoembed/") + 17;
                                episode.VideoId = src.Substring(last, src.Length - last);
                                ListEpisode.Add(episode);
                            }
                            else if (src.Contains("https://short.ink/"))
                            {
                                episode.SupplierId = 4;
                                var last = src.IndexOf("https://short.ink/") + 18;
                                episode.VideoId = src.Substring(last, src.Length - last);
                                ListEpisode.Add(episode);
                            }
                            else if (src.Contains("https://www.fembed.com/v/"))
                            {
                                episode.SupplierId = 9;
                                var last = src.IndexOf("https://www.fembed.com/v/") + 25;
                                episode.VideoId = src.Substring(last, src.Length - last);
                                ListEpisode.Add(episode);
                            }
                        }
                    }
                }
                foreach (var item in ListEpisode.GroupBy(x => x.EpisodeNumber))
                {
                    var checkExist = db.Episodes.FirstOrDefault(x => x.EpisodeNumber == item.Key.Value && x.ProductId == movie.Id && x.Type == TypeId);
                    if (checkExist == null)
                    {
                        var episode = new Episode();
                        episode.ProductId = movie.Id;
                        episode.Status = true;
                        episode.EpisodeNumber = item.Key.Value;
                        if (movie.RecurringURL.Contains("animehay"))
                        {
                            episode.Name = "Tập " + item.FirstOrDefault()?.Name;
                        }
                        else
                        {
                            episode.Name = "Tập " + episode.EpisodeNumber;
                        }

                        episode.Type = TypeId;
                        if (episode.Type == 1)
                        {
                            episode.Link = "tap-" + episode.EpisodeNumber;
                        }
                        else
                        {
                            episode.Link = "tap-" + episode.EpisodeNumber + "-thuyet-minh";
                        }

                        episode.FullLink = movie.Link + "-" + episode.Link;
                        episode.CreateOn = DateTime.UtcNow;
                        episode.ViewNumber = 0;
                        episode.Keyword = this.ConvertKeywordEpisode(movie.Name, movie.OtherName, episode.EpisodeNumber, movie.TypeId);
                        var source = new List<EpisodeSource>();
                        source = item.Select(x => new EpisodeSource
                        {
                            SupplierId = x.SupplierId,
                            CreateOn = DateTime.UtcNow,
                            IsIframe = true,
                            Link = x.VideoId,
                        }).ToList();
                        episode.EpisodeSources = source;
                        db.Episodes.Add(episode);
                    }
                    else
                    {
                        var source = checkExist.EpisodeSources;
                        foreach (var s in item)
                        {
                            var checkSource = source.Where(x => x.SupplierId == s.SupplierId).FirstOrDefault();
                            if (checkSource == null)
                            {
                                source.Add(new EpisodeSource
                                {
                                    SupplierId = s.SupplierId,
                                    CreateOn = DateTime.UtcNow,
                                    IsIframe = true,
                                    Link = s.VideoId,
                                });
                            }
                        }

                        checkExist.EpisodeSources = source;
                    }

                }

                if (movie.EpisodesTotal > 0)
                {
                    if(movie.EpisodesTotal== ListEpisode.GroupBy(x => x.EpisodeNumber).OrderByDescending(x => x.Key).First().Key)
                    {
                        movie.Status = 5;
                    }
                }
                db.SaveChanges();
               
            }
            catch (Exception ex)
            {
             
            }

        }
        public void Start()
        {
            _timer.Start();
        }
        public void Stop()
        {
            _timer.Stop();
        }
    }
}
