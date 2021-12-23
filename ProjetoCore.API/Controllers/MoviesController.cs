using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjetoCore.API.Data;
using ProjetoCore.API.Models;
using ProjetoCore.API.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace ProjetoCore.API.Controllers
{
    public class MoviesController : Controller
    {
        private readonly ProjetoCoreAPIContext movieDb;
        private readonly IMapper _mapper;

        /*private readonly ProjetoCoreAPIContext movieDb;
        private readonly IWebHostEnvironment webHostEnvironment;*/

        //public MoviesController(ProjetoCoreAPIContext context, IWebHostEnvironment hostEnvironment)
        public MoviesController(ProjetoCoreAPIContext context, IMapper mapper)

        {
            movieDb = context;
            _mapper = mapper;
            /*movieDb = context;
            webHostEnvironment = hostEnvironment;*/
        }
        // GET: Movie
        public ViewResult Index(string searchString, int? SelectedGenre, string sortOrder)
        {
            var genres = movieDb.Genre.OrderBy(g => g.Name).ToList();
            ViewBag.SelectedGenre = new SelectList(genres, "GenreID", "Name", SelectedGenre);
            int genreID = SelectedGenre.GetValueOrDefault();

            var movies = movieDb.Movie
                .Where(c => !SelectedGenre.HasValue || c.GenreID == genreID);

            if (!String.IsNullOrEmpty(searchString))
            {
                movies = movies.Where(s => s.Title.Contains(searchString) || s.Director.Contains(searchString));
            }
            ViewBag.RatingSortParm = sortOrder == "Rating" ? "rating_asc" : "Rating";
            switch (sortOrder)
            {
                case "Rating":
                    movies = movies.OrderByDescending(s => s.Rating);
                    break;
                case "rating_asc":
                    movies = movies.OrderBy(s => s.Rating);
                    break;
            }
            return View(movies);
        }
        // GET: Movie/Create
        //[Authorize]
        /*public ActionResult Create()
        {
            ViewBag.GenreID = new SelectList(movieDb.Genre, "GenreID", "Name");
            return View();
        }

        // POST: Movie/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize]
        //[DateActionFilter]
        public ActionResult Create([Bind("ID,Title,Director,ReleaseDate,Gross,Rating, GenreID")] Movie movie, IFormFile Image)
        {
            if (ModelState.IsValid)
            {
                using (var ms = new MemoryStream())
                {
                    Image.CopyTo(ms);
                    movie.ImageFile = ms.ToArray();
                }

                movieDb.Add(movie);
                movieDb.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.GenreID = new SelectList(movieDb.Genre, "GenreID", "Name", movie.GenreID);
            return View(movie);
        }/*
            /*public ActionResult Create([Bind("ID,Title,Director,ReleaseDate,Gross,Rating, GenreID")] Movie movie)
            {
                if (ModelState.IsValid)
                {
                    movieDb.Movie.Add(movie);
                    movieDb.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.GenreID = new SelectList(movieDb.Genre, "GenreID", "Name", movie.GenreID);
                return View(movie);
            }*/
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Movie movie = movieDb.Movie.Find(id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }

        //public ActionResult Edit(long? id)
        //[Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Movie movie = movieDb.Movie.Find(id);
            if(movie == null)
            {
                return NotFound();
            }
            MovieViewModel movieVM = new MovieViewModel();
            movieDb.Add(_mapper.Map<Movie>(movieVM));
            ViewBag.GenreID = new SelectList(movieDb.Genre, "GenreID", "Name");
            return View(movieVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MovieViewModel movieMV)
        {
            if (ModelState.IsValid)
            {
                if (movieMV.ImageUpload != null)
                {
                    movieMV.ImageFile = ConvertToByte(movieMV.ImageUpload);
                }
                movieDb.Update(_mapper.Map<Movie>(movieMV));
                movieDb.SaveChanges();

                /*if (movieMV.ImageUpload != null)
                {
                    movieMV.ImageFile = ConvertToByte(movieMV.ImageUpload);
                }
                try
                {
                    movieDb.Add(_mapper.Map<Movie>(movieVM));
                    movieDb.Update(Movie);
                    movieDb.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movieMV.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));*/
            }
            return View(movieMV);
        }
        private bool MovieExists(long id)
        {
            return movieDb.Movie.Any(e => e.ID == id);
        }
        public ActionResult Create()
        {
            var movie = new Movie();
            ViewBag.GenreID = new SelectList(movieDb.Genre, "GenreID", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MovieViewModel movieMV)
        {
            if (ModelState.IsValid)
            {

                if (movieMV.ImageUpload != null)
                {
                    movieMV.ImageFile = ConvertToByte(movieMV.ImageUpload);
                }
                movieDb.Add(_mapper.Map<Movie>(movieMV));
                await movieDb.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            return View(movieMV);
        }

        private byte[] ConvertToByte(IFormFile imagem)
        {
            if (imagem.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    imagem.CopyTo(ms);
                    return ms.ToArray();
                }

            }
            return null;
        }
    }
}
        /* if (ModelState.IsValid)
         {
             try
             {

             }
             if (movieMV.ImageUpload != null)
             {
                 movieMV.imagembyte = ConvertToByte(movieMV.ImageUpload);
             }
             movieDb.Add(_mapper.Map<Movie>(movieMV));
             movieDb.SaveChanges();
             return RedirectToAction(nameof(Index));
         }
         return View(movieMV);*/
        // POST: Testecs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /*public ActionResult Edit(int? id)
        {
            Movie movie = movieDb.Movie.Find(id);
            if(id == null)
            {
                return NotFound();
            }
            var model = new MovieViewModel();
                model.Title = movie.Title;
                model.Director = movie.Director;
                model.ReleaseDate = movie.ReleaseDate;
                model.Gross = movie.Gross;
                model.Rating = movie.Rating;
                model.GenreID = movie.GenreID;
                model.imagembyte = movie.ImageFile;
                ViewBag.GenreID = new SelectList(movieDb.Genre, "GenreID", "Name", movie.GenreID);
            return View(model);
        }*/



        

       

            // GET: Testecs/Edit/5

            /*if (ModelState.IsValid)
            {
                movieDb.Add();
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GenreID"] = new SelectList(_context.Set<Genre>(), "GenreID", "Name", movie.GenreID);
            return View(movie);*/

        /*public ActionResult Edit(long id, [Bind("Id,Nome")] MovieViewModel movieVM)
        {
            if (id != movieVM.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(testecs);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TestecsExists(testecs.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(testecs);
        }*/
        // GET: Movie/Edit/5
        //[Authorize]

        /*[HttpPost]
        public ActionResult Edit(int id, string title, string director,
        DateTime releaseDate, decimal gross, double rating, string imageUrl, int genreID)*/

        /*if (ModelState.IsValid)
        {
            if (movieMV.ImageUpload != null)
            {
                movieMV.imagembyte = ConvertToByte(movieMV.ImageUpload);
            }
            var model = new MovieViewModel
            {
                Title = movieMV.Title,
                Director = movieMV.Director,
                ReleaseDate = movieMV.ReleaseDate,
                Gross = movieMV.Gross,
                Rating = movieMV.Rating,
                GenreID = movieMV.GenreID,
                ImageUpload = movieMV.ImageUpload,
            };
            movieDb.Add(model);
            movieDb.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        return View(model);*/
        //}
        /*var model = new MovieViewModel
        {
            Title = movieMV.Title,
            Director = movieMV.Director,
            ReleaseDate = movieMV.ReleaseDate,
            Gross = movieMV.Gross,
            Rating = movieMV.Rating,
            GenreID = movieMV.GenreID,
            ImageUpload = movieMV.ImageUpload,
        };*/
        //if (ModelState.IsValid)
        //{

        /*   var movie = movieDb.Movie.Find(movieMV.ID);
           movie.Title = movieMV.Title;
           movie.Director = movieMV.Director;
           movie.ReleaseDate = movieMV.ReleaseDate;
           movie.Gross = movieMV.Gross;
           movie.Rating = movieMV.Rating;
           movie.GenreID = movieMV.GenreID;

           movie.ImageFile = ConvertToByte(movieMV.ImageUpload);
           movieDb.Entry(movie).State = EntityState.Modified;
           movieDb.SaveChanges();
           //return View(model);

           return RedirectToAction(nameof(Index));
       //}
       //ViewBag.GenreId = new SelectList(movieDb.Genre, "GenreId", "Name", movie.GenreID);
       //return View();
       return View(model);
       }*/

        // GET: Movie/Delete/5
        // [Authorize(Users = "admin@mvc.br")]
        /*public ActionResult Delete(int? id)
        {
            if (id == null)
            {
               return NotFound();
            }
           var moviedeleted = movieDb.Movie.Find(id);
            movieDb.Movie.Remove(moviedeleted);
            if (moviedeleted == null)
            {
                return NotFound();
            }
            return View(moviedeleted);
        }
       // POST: Testecs/Delete/5
       [HttpPost, ActionName("Delete")]
       [ValidateAntiForgeryToken]
       public ActionResult DeleteConfirmed(long id)
       {
           var moviedeleted = movieDb.Movie.Find(id);
           movieDb.Movie.Remove(moviedeleted);
           movieDb.SaveChanges();
           return RedirectToAction(nameof(Index));
       }

       private bool TestecsExists(long id)
       {
           return movieDb.Movie.Any(e => e.ID == id);
       }

       protected override void Dispose(bool disposing)
      {
         if (disposing)
         {
            movieDb.Dispose();
         }
         base.Dispose(disposing);
      }

      public ActionResult Catalogo(string titulo)
      {
         string filePath = Server.MapPath("~/Catalogo/") + titulo.ToLower() + ".pdf";
         if (System.IO.File.Exists(filePath))
            return new FilePathResult(filePath, "application/pdf");
         else return HttpNotFound();
      }
      public JsonResult Filmes()
      {
         // atenção: este código é apenas um exemplo; 
         // ver possíveis vulnerabilidades em:
         // http://msdn.microsoft.com/query/dev11.query?appId=Dev11IDEF1&l=EN-US&k=k(System.Web.Mvc.JsonRequestBehavior);k(TargetFrameworkMoniker-.NETFramework,Version%3Dv4.5);k(DevLang-csharp)&rd=true 
         // http://haacked.com/archive/2008/11/20/anatomy-of-a-subtle-json-vulnerability.aspx
         // http://msdn.microsoft.com/en-us/library/hh404095.aspx

         var model = from movie in movieDb.Movies
                     select new
                     {
                        Titulo = movie.Title,
                        Diretor = movie.Director,
                        Ano = movie.ReleaseDate.Year,
                        Genero = movie.Genre.Name
                     };

         return Json(model.OrderBy(m => m.Ano), JsonRequestBehavior.AllowGet);
      }

      public ActionResult Browse(string genre = "Action")
      {
         var genreModel = movieDb.Genres.Include("Movies").Single(g => g.Name == genre);

         return View(genreModel);
      }
        public ActionResult GetImage(int id)
        {
            Movie movie = movieDb.Movies.Find(id);
            if(movie != null && movie.ImageFile != null)
            {
                return File(movie.ImageFile, movie.ImageMimeType);
            }
            else
            {   
                return new FilePathResult("~/Images/nao-disponivel.jpg", "image/jpeg");
            }
        }
        // GET: /Movie/GenreMenu
        [ChildActionOnly]
        public ActionResult GenreMenu(int num = 5)
        {
            var genres = movieDb.Genres
            .OrderByDescending(g => g.Movies.Count)
            .Take(num)
            .ToList();
            return this.PartialView(genres);
        }
        public ActionResult UserData()
        {
            if (!Request.IsAuthenticated)
            {
                return Content("Not Authenticated");
            }
            var store = new UserStore<ApplicationUser>(new
            ApplicationDbContext());
            var userManager = new UserManager<ApplicationUser>(store);
            ApplicationUser user =
            userManager.FindByNameAsync(User.Identity.Name).Result;
            return Content("Id: " + User.Identity.Name + " Name: " +
            user.FullName);
        }*/
        /*// GET: Movies
        public async Task<IActionResult> Index()
        {
            var projetoCoreAPIContext = _context.Movie.Include(m => m.Genre);
            return View(await projetoCoreAPIContext.ToListAsync());
        }

        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie
                .Include(m => m.Genre)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }
        */
        // GET: Movies/Create


        // POST: Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.


        // GET: Movies/Edit/5
        /*public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            ViewData["GenreID"] = new SelectList(_context.Set<Genre>(), "GenreID", "Name", movie.GenreID);
            return View(movie);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Title,ReleaseDate,Director,Gross,Rating,GenreID,ImageFile,ImageMimeType,ImageUrl")] Movie movie)
        {
            if (id != movie.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["GenreID"] = new SelectList(_context.Set<Genre>(), "GenreID", "Name", movie.GenreID);
            return View(movie);
        }

        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie
                .Include(m => m.Genre)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movie = await _context.Movie.FindAsync(id);
            _context.Movie.Remove(movie);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(int id)
        {
            return _context.Movie.Any(e => e.ID == id);
        }*/

