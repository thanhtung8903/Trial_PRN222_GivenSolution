# Codebase Documentation

## Directory Structure

```plaintext
- Project2/
    - appsettings.Development.json
    - appsettings.json
    - libman.json
    - Program.cs
    - Data/
        - SeedData.cs
    - Hubs/
        - MovieHub.cs
    - Models/
        - Director.cs
        - Genre.cs
        - Movie.cs
        - Producer.cs
        - Star.cs
        - TrialContext.cs
    - Pages/
        - Movies/
            - Director_Movie.cshtml.cs
    - Properties/
        - launchSettings.json
    - wwwroot/
        - js/
            - signalr/
                - dist/
                    - browser/
                        - signalr.js
                        - signalr.min.js

```

## Code Snippets

### appsettings.Development.json

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}

```

### appsettings.json

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "Trial": "Server=(local);uid=sa;pwd=123;database=PE_PRN222_Trial;TrustServerCertificate=True"
  }
}

```

### libman.json

```json
{
  "version": "1.0",
  "defaultProvider": "unpkg",
  "libraries": [
    {
      "library": "@microsoft/signalr@latest",
      "destination": "wwwroot/js/signalr/",
      "files": [
        "dist/browser/signalr.js",
        "dist/browser/signalr.min.js"
      ]
    }
  ]
}
```

### Program.cs

```csharp
﻿using Microsoft.EntityFrameworkCore;
using Project2.Data;
using Project2.Hubs;
using Project2.Models;

var builder = WebApplication.CreateBuilder(args);

// Sử dụng InMemoryDatabase để test
builder.Services.AddDbContext<TrialContext>(options =>
    // options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    options.UseInMemoryDatabase("TrialDb"));

builder.Services.AddRazorPages();
builder.Services.AddSignalR();

var app = builder.Build();

// Seed dữ liệu vào InMemoryDatabase
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    SeedData.Initialize(services);
}

app.UseStaticFiles();
app.MapRazorPages();
app.MapHub<MovieHub>("/movieHub");

// Mapping: redirect từ "/" đến "/Movies/Director_Movie"
app.MapGet("/", context =>
{
    context.Response.Redirect("/Movies/Director_Movie");
    return Task.CompletedTask;
});

app.Run();

```

### Data\SeedData.cs

```csharp
﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Project2.Models;
using System;
using System.Linq;

namespace Project2.Data;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using var context = new TrialContext(serviceProvider.GetRequiredService<DbContextOptions<TrialContext>>());
        // Nếu đã có dữ liệu thì không seed
        if (context.Movies.Any())
        {
            return;
        }

        // Seed Directors
        context.Directors.AddRange(
            new Director { Id = 1, FullName = "David Gordon Green", Male = true, Dob = new DateOnly(1975, 4, 9), Nationality = "USA", Description = "David Gordon Green was born on April 9, 1975 in Little Rock, Arkansas, USA. He is a producer and director, known for Sát Nhân Halloween (2018), Halloween Kills (2021) and George Washington (2000)." },
            new Director { Id = 2, FullName = "Aaron Horvath", Male = true, Dob = new DateOnly(1980, 8, 19), Nationality = "USA", Description = "Aaron Horvath was born on August 19, 1980 in California, USA. He is a producer and writer, known for Biet Doi Sieu Anh Hung Teen Titans (2018), Biet Doi Thieu Nien Titan Xuat Kich! (2013) and Naruto." },
            new Director { Id = 4, FullName = "David Bruckner", Male = true, Dob = new DateOnly(1981, 8, 19), Nationality = "England", Description = "David Bruckner is known for The Night House (2020), Nghi Lễ Tế Thần (2017) and Southbound (2015)." },
            new Director { Id = 5, FullName = "Mike Barker", Male = true, Dob = new DateOnly(1965, 11, 29), Nationality = "England", Description = "Mike Barker was born on November 29, 1965 in England, UK. He is a director and producer, known for Fargo (2014) and Broadchurch (2013)." },
            new Director { Id = 6, FullName = "Joseph Kosinski", Male = true, Dob = new DateOnly(1974, 5, 3), Nationality = "USA", Description = "Joseph Kosinski is a director whose uncompromising style has quickly made a mark in the filmmaking zeitgeist. His feature film debut, \"Tron: Legacy\" for Walt Disney Studios, grossed over $400 million worldwide and was nominated for several awards." }
        );

        // Seed Genres
        context.Genres.AddRange(
            new Genre { Id = 1, Title = "Action" },
            new Genre { Id = 2, Title = "Drama" },
            new Genre { Id = 6, Title = "Adventure" },
            new Genre { Id = 7, Title = "Comedy" },
            new Genre { Id = 8, Title = "Animation" },
            new Genre { Id = 9, Title = "Fantasy" },
            new Genre { Id = 10, Title = "Sci-fi" },
            new Genre { Id = 11, Title = "Crime" },
            new Genre { Id = 12, Title = "Thriller" },
            new Genre { Id = 13, Title = "Family" },
            new Genre { Id = 14, Title = "Horror" }
        );

        // Seed Producers
        context.Producers.AddRange(
            new Producer { Id = 1, Name = "Paramount Pictures" },
            new Producer { Id = 2, Name = "Disney Channel" },
            new Producer { Id = 3, Name = "Blumhouse Productions" },
            new Producer { Id = 4, Name = "Universal Pictures" },
            new Producer { Id = 5, Name = "Cinemundo" },
            new Producer { Id = 6, Name = "Illumination Entertainment" },
            new Producer { Id = 7, Name = "20th Century Studios" },
            new Producer { Id = 8, Name = "Made Up Stories" }
        );

        // Seed Stars
        context.Stars.AddRange(
            new Star { Id = 1, FullName = "Jamie Lee Curtis", Male = false, Dob = new DateOnly(1958, 11, 22), Description = "Jamie Lee Curtis was born on November 22, 1958 in Los Angeles...", Nationality = "USA" },
            new Star { Id = 2, FullName = "Andi Matichak", Male = false, Dob = new DateOnly(1970, 9, 11), Description = "Andi Matichak is an American actress...", Nationality = "USA" },
            new Star { Id = 3, FullName = "James Jude Courtney", Male = true, Dob = new DateOnly(1957, 1, 31), Description = "James Jude Courtney was born on January 31, 1957 in Portland, Ohio...", Nationality = "USA" },
            new Star { Id = 4, FullName = "Chris Pratt", Male = true, Dob = new DateOnly(1979, 6, 21), Description = "Christopher Michael 'Chris' Pratt was born on June 21, 1979...", Nationality = "USA" },
            new Star { Id = 5, FullName = "Anya Taylor-Joy", Male = false, Dob = new DateOnly(1996, 4, 16), Description = "Anya-Josephine Marie Taylor-Joy (born 16 April 1996) is a British-American actress...", Nationality = "USA" },
            new Star { Id = 6, FullName = "Charlie Day", Male = true, Dob = new DateOnly(1976, 2, 9), Description = "Charles Peckham Day was born in New York City...", Nationality = "USA" },
            new Star { Id = 7, FullName = "Odessa A’zion", Male = false, Dob = new DateOnly(1984, 10, 1), Description = "Odessa A’zion is known for Hellraiser (2022)...", Nationality = "USA" },
            new Star { Id = 8, FullName = "Jamie Clayton", Male = false, Dob = new DateOnly(1978, 1, 15), Description = "Jamie Clayton's big break was playing Kyla on season three of Hung...", Nationality = "USA" },
            new Star { Id = 9, FullName = "Leonardo DiCaprio", Male = true, Dob = new DateOnly(1974, 11, 11), Description = "Leonardo DiCaprio was born November 11, 1974 in Los Angeles...", Nationality = "USA" },
            new Star { Id = 10, FullName = "Jonah Hill", Male = true, Dob = new DateOnly(1983, 12, 20), Description = "Jonah Hill was born and raised in Los Angeles...", Nationality = "USA" }
        );

        // Seed Movies
        context.Movies.AddRange(
            new Movie { Id = 2, Title = "Halloween Ends", ReleaseDate = new DateOnly(2022, 10, 14), Description = "The saga of Michael Myers and Laurie Strode comes to a spine-chilling climax...", Language = "English", ProducerId = 3, DirectorId = 1 },
            new Movie { Id = 3, Title = "The Super Mario Bros. Movie", ReleaseDate = new DateOnly(2023, 4, 7), Description = "A plumber named Mario travels through an underground labyrinth...", Language = "English", ProducerId = 6, DirectorId = 2 },
            new Movie { Id = 5, Title = "Hellraiser", ReleaseDate = new DateOnly(2022, 10, 7), Description = "A take on Clive Barker's 1987 horror classic...", Language = "English", ProducerId = 7, DirectorId = 4 },
            new Movie { Id = 6, Title = "Luckiest Girl Alive", ReleaseDate = new DateOnly(2022, 10, 7), Description = "A woman in New York, who seems to have things under control...", Language = "English", ProducerId = 8, DirectorId = 5 },
            new Movie { Id = 8, Title = "Broadchurch", ReleaseDate = new DateOnly(2013, 3, 4), Description = "The murder of a young boy in a small coastal town...", Language = "English", ProducerId = 7, DirectorId = 5 },
            new Movie { Id = 9, Title = "Top Gun: Maverick", ReleaseDate = new DateOnly(2022, 5, 27), Description = "After thirty years, Maverick is still pushing the envelope...", Language = "English", ProducerId = 1, DirectorId = 6 },
            new Movie { Id = 10, Title = "The Wolf of Wall Street", ReleaseDate = new DateOnly(2014, 1, 11), Description = "Based on the true story of Jordan Belfort...", Language = "English", ProducerId = 1, DirectorId = 5 }
        );

        context.SaveChanges();

        // Thiết lập các mối quan hệ nhiều-nhiều
        var movie2 = context.Movies.Find(2);
        var movie3 = context.Movies.Find(3);
        var movie5 = context.Movies.Find(5);
        var movie6 = context.Movies.Find(6);
        var movie8 = context.Movies.Find(8);
        var movie9 = context.Movies.Find(9);
        var movie10 = context.Movies.Find(10);

        if (movie2 != null)
        {
            movie2.Genres.Add(context.Genres.Find(12));
            movie2.Genres.Add(context.Genres.Find(14));
            movie2.Stars.Add(context.Stars.Find(1));
            movie2.Stars.Add(context.Stars.Find(2));
            movie2.Stars.Add(context.Stars.Find(3));
        }
        if (movie3 != null)
        {
            movie3.Genres.Add(context.Genres.Find(6));
            movie3.Genres.Add(context.Genres.Find(7));
            movie3.Genres.Add(context.Genres.Find(8));
            movie3.Genres.Add(context.Genres.Find(9));
            movie3.Genres.Add(context.Genres.Find(10));
            movie3.Stars.Add(context.Stars.Find(4));
            movie3.Stars.Add(context.Stars.Find(5));
            movie3.Stars.Add(context.Stars.Find(6));
        }
        if (movie5 != null)
        {
            movie5.Genres.Add(context.Genres.Find(12));
            movie5.Genres.Add(context.Genres.Find(14));
            movie5.Stars.Add(context.Stars.Find(7));
            movie5.Stars.Add(context.Stars.Find(8));
        }
        if (movie6 != null)
        {
            movie6.Genres.Add(context.Genres.Find(2));
            movie6.Genres.Add(context.Genres.Find(12));
        }
        if (movie8 != null)
        {
            movie8.Genres.Add(context.Genres.Find(2));
            movie8.Genres.Add(context.Genres.Find(11));
        }
        if (movie9 != null)
        {
            movie9.Genres.Add(context.Genres.Find(1));
            movie9.Genres.Add(context.Genres.Find(2));
        }
        if (movie10 != null)
        {
            movie10.Genres.Add(context.Genres.Find(2));
            movie10.Genres.Add(context.Genres.Find(7));
            movie10.Genres.Add(context.Genres.Find(11));
        }

        context.SaveChanges();
    }
}
```

### Hubs\MovieHub.cs

```csharp
﻿using Microsoft.AspNetCore.SignalR;

namespace Project2.Hubs
{
    public class MovieHub : Hub
    {
        // Có thể thêm các phương thức nếu cần truyền dữ liệu qua hub
    }
}
```

### Models\Director.cs

```csharp
﻿using System;
using System.Collections.Generic;

namespace Project2.Models;

public partial class Director
{
    public int Id { get; set; }

    public string FullName { get; set; } = null!;

    public bool Male { get; set; }

    public DateOnly Dob { get; set; }

    public string Nationality { get; set; } = null!;

    public string Description { get; set; } = null!;

    public virtual ICollection<Movie> Movies { get; set; } = new List<Movie>();
}

```

### Models\Genre.cs

```csharp
﻿using System;
using System.Collections.Generic;

namespace Project2.Models;

public partial class Genre
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<Movie> Movies { get; set; } = new List<Movie>();
}

```

### Models\Movie.cs

```csharp
﻿using System;
using System.Collections.Generic;

namespace Project2.Models;

public partial class Movie
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public DateOnly? ReleaseDate { get; set; }

    public string? Description { get; set; }

    public string Language { get; set; } = null!;

    public int? ProducerId { get; set; }

    public int? DirectorId { get; set; }

    public virtual Director? Director { get; set; }

    public virtual Producer? Producer { get; set; }

    public virtual ICollection<Genre> Genres { get; set; } = new List<Genre>();

    public virtual ICollection<Star> Stars { get; set; } = new List<Star>();
}

```

### Models\Producer.cs

```csharp
﻿using System;
using System.Collections.Generic;

namespace Project2.Models;

public partial class Producer
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Movie> Movies { get; set; } = new List<Movie>();
}

```

### Models\Star.cs

```csharp
﻿using System;
using System.Collections.Generic;

namespace Project2.Models;

public partial class Star
{
    public int Id { get; set; }

    public string FullName { get; set; } = null!;

    public bool? Male { get; set; }

    public DateOnly? Dob { get; set; }

    public string? Description { get; set; }

    public string? Nationality { get; set; }

    public virtual ICollection<Movie> Movies { get; set; } = new List<Movie>();
}

```

### Models\TrialContext.cs

```csharp
﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Project2.Models;

public partial class TrialContext : DbContext
{
    public TrialContext()
    {
    }

    public TrialContext(DbContextOptions<TrialContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Director> Directors { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<Movie> Movies { get; set; }

    public virtual DbSet<Producer> Producers { get; set; }

    public virtual DbSet<Star> Stars { get; set; }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //    => optionsBuilder.UseSqlServer("Name=ConnectionStrings:Trial");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Director>(entity =>
        {
            entity.Property(e => e.Description).HasColumnType("ntext");
            entity.Property(e => e.FullName)
                .HasMaxLength(40)
                .IsUnicode(false);
            entity.Property(e => e.Nationality)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.Property(e => e.Title)
                .HasMaxLength(10)
                .IsFixedLength();
        });

        modelBuilder.Entity<Movie>(entity =>
        {
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.Language)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Title)
                .HasMaxLength(200)
                .IsUnicode(false);

            entity.HasOne(d => d.Director).WithMany(p => p.Movies)
                .HasForeignKey(d => d.DirectorId)
                .HasConstraintName("FK_Movies_Directors");

            entity.HasOne(d => d.Producer).WithMany(p => p.Movies)
                .HasForeignKey(d => d.ProducerId)
                .HasConstraintName("FK_Movies_Producers");

            entity.HasMany(d => d.Genres).WithMany(p => p.Movies)
                .UsingEntity<Dictionary<string, object>>(
                    "MovieGenre",
                    r => r.HasOne<Genre>().WithMany()
                        .HasForeignKey("GenreId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Movie_Genre_Genres"),
                    l => l.HasOne<Movie>().WithMany()
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Movie_Genre_Movies"),
                    j =>
                    {
                        j.HasKey("MovieId", "GenreId");
                        j.ToTable("Movie_Genre");
                    });

            entity.HasMany(d => d.Stars).WithMany(p => p.Movies)
                .UsingEntity<Dictionary<string, object>>(
                    "MovieStar",
                    r => r.HasOne<Star>().WithMany()
                        .HasForeignKey("StarId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Movie_Star_Stars"),
                    l => l.HasOne<Movie>().WithMany()
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Movie_Star_Movies"),
                    j =>
                    {
                        j.HasKey("MovieId", "StarId");
                        j.ToTable("Movie_Star");
                    });
        });

        modelBuilder.Entity<Producer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Productions");

            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Star>(entity =>
        {
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.FullName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Nationality)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

```

### Pages\Movies\Director_Movie.cshtml.cs

```csharp
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Project2.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using Project2.Hubs;

namespace Project2.Pages.Movies
{
    public class DirectorMovieModel : PageModel
    {
        private readonly TrialContext _context;
        private readonly IHubContext<MovieHub> _hubContext;

        public DirectorMovieModel(TrialContext context, IHubContext<MovieHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        public List<Director> Directors { get; set; }
        public List<Movie> Movies { get; set; }
        public int? SelectedDirectorId { get; set; }

        public void OnGet(int? directorId)
        {
            // Load all directors
            Directors = _context.Directors.ToList();
            SelectedDirectorId = directorId;

            // Load movies, either all or filtered by director
            if (directorId.HasValue)
            {
                Movies = _context.Movies
                    .Include(m => m.Director)
                    .Include(m => m.Producer)
                    .Include(m => m.Stars)
                    .Where(m => m.DirectorId == directorId)
                    .ToList();
            }
            else
            {
                Movies = _context.Movies
                    .Include(m => m.Director)
                    .Include(m => m.Producer)
                    .Include(m => m.Stars)
                    .ToList();
            }
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var movie = _context.Movies.Find(id);
            if (movie != null)
            {
                _context.Movies.Remove(movie);
                await _context.SaveChangesAsync();

                // Notify all clients via SignalR
                await _hubContext.Clients.All.SendAsync("MovieDeleted", id);
            }
            return RedirectToPage(new { directorId = SelectedDirectorId });
        }
    }
}
```

### Properties\launchSettings.json

```json
﻿{
  "$schema": "http://json.schemastore.org/launchsettings.json",
  "iisSettings": {
    "windowsAuthentication": false,
    "anonymousAuthentication": true,
    "iisExpress": {
      "applicationUrl": "http://localhost:6454",
      "sslPort": 0
    }
  },
  "profiles": {
    "http": {
      "commandName": "Project",
      "dotnetRunMessages": true,
      "launchBrowser": true,
      "applicationUrl": "http://localhost:5186",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    },
    "IIS Express": {
      "commandName": "IISExpress",
      "launchBrowser": true,
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    }
  }
}

```

### wwwroot\js\signalr\dist\browser\signalr.js

```javascript
(function webpackUniversalModuleDefinition(root, factory) {
	if(typeof exports === 'object' && typeof module === 'object')
		module.exports = factory();
	else if(typeof define === 'function' && define.amd)
		define([], factory);
	else if(typeof exports === 'object')
		exports["signalR"] = factory();
	else
		root["signalR"] = factory();
})(self, () => {
return /******/ (() => { // webpackBootstrap
/******/ 	"use strict";
/******/ 	// The require scope
/******/ 	var __webpack_require__ = {};
/******/ 	
/************************************************************************/
/******/ 	/* webpack/runtime/define property getters */
/******/ 	(() => {
/******/ 		// define getter functions for harmony exports
/******/ 		__webpack_require__.d = (exports, definition) => {
/******/ 			for(var key in definition) {
/******/ 				if(__webpack_require__.o(definition, key) && !__webpack_require__.o(exports, key)) {
/******/ 					Object.defineProperty(exports, key, { enumerable: true, get: definition[key] });
/******/ 				}
/******/ 			}
/******/ 		};
/******/ 	})();
/******/ 	
/******/ 	/* webpack/runtime/global */
/******/ 	(() => {
/******/ 		__webpack_require__.g = (function() {
/******/ 			if (typeof globalThis === 'object') return globalThis;
/******/ 			try {
/******/ 				return this || new Function('return this')();
/******/ 			} catch (e) {
/******/ 				if (typeof window === 'object') return window;
/******/ 			}
/******/ 		})();
/******/ 	})();
/******/ 	
/******/ 	/* webpack/runtime/hasOwnProperty shorthand */
/******/ 	(() => {
/******/ 		__webpack_require__.o = (obj, prop) => (Object.prototype.hasOwnProperty.call(obj, prop))
/******/ 	})();
/******/ 	
/******/ 	/* webpack/runtime/make namespace object */
/******/ 	(() => {
/******/ 		// define __esModule on exports
/******/ 		__webpack_require__.r = (exports) => {
/******/ 			if(typeof Symbol !== 'undefined' && Symbol.toStringTag) {
/******/ 				Object.defineProperty(exports, Symbol.toStringTag, { value: 'Module' });
/******/ 			}
/******/ 			Object.defineProperty(exports, '__esModule', { value: true });
/******/ 		};
/******/ 	})();
/******/ 	
/************************************************************************/
var __webpack_exports__ = {};
// ESM COMPAT FLAG
__webpack_require__.r(__webpack_exports__);

// EXPORTS
__webpack_require__.d(__webpack_exports__, {
  "AbortError": () => (/* reexport */ AbortError),
  "DefaultHttpClient": () => (/* reexport */ DefaultHttpClient),
  "HttpClient": () => (/* reexport */ HttpClient),
  "HttpError": () => (/* reexport */ HttpError),
  "HttpResponse": () => (/* reexport */ HttpResponse),
  "HttpTransportType": () => (/* reexport */ HttpTransportType),
  "HubConnection": () => (/* reexport */ HubConnection),
  "HubConnectionBuilder": () => (/* reexport */ HubConnectionBuilder),
  "HubConnectionState": () => (/* reexport */ HubConnectionState),
  "JsonHubProtocol": () => (/* reexport */ JsonHubProtocol),
  "LogLevel": () => (/* reexport */ LogLevel),
  "MessageType": () => (/* reexport */ MessageType),
  "NullLogger": () => (/* reexport */ NullLogger),
  "Subject": () => (/* reexport */ Subject),
  "TimeoutError": () => (/* reexport */ TimeoutError),
  "TransferFormat": () => (/* reexport */ TransferFormat),
  "VERSION": () => (/* reexport */ VERSION)
});

;// CONCATENATED MODULE: ./src/Errors.ts
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
/** Error thrown when an HTTP request fails. */
class HttpError extends Error {
    /** Constructs a new instance of {@link @microsoft/signalr.HttpError}.
     *
     * @param {string} errorMessage A descriptive error message.
     * @param {number} statusCode The HTTP status code represented by this error.
     */
    constructor(errorMessage, statusCode) {
        const trueProto = new.target.prototype;
        super(`${errorMessage}: Status code '${statusCode}'`);
        this.statusCode = statusCode;
        // Workaround issue in Typescript compiler
        // https://github.com/Microsoft/TypeScript/issues/13965#issuecomment-278570200
        this.__proto__ = trueProto;
    }
}
/** Error thrown when a timeout elapses. */
class TimeoutError extends Error {
    /** Constructs a new instance of {@link @microsoft/signalr.TimeoutError}.
     *
     * @param {string} errorMessage A descriptive error message.
     */
    constructor(errorMessage = "A timeout occurred.") {
        const trueProto = new.target.prototype;
        super(errorMessage);
        // Workaround issue in Typescript compiler
        // https://github.com/Microsoft/TypeScript/issues/13965#issuecomment-278570200
        this.__proto__ = trueProto;
    }
}
/** Error thrown when an action is aborted. */
class AbortError extends Error {
    /** Constructs a new instance of {@link AbortError}.
     *
     * @param {string} errorMessage A descriptive error message.
     */
    constructor(errorMessage = "An abort occurred.") {
        const trueProto = new.target.prototype;
        super(errorMessage);
        // Workaround issue in Typescript compiler
        // https://github.com/Microsoft/TypeScript/issues/13965#issuecomment-278570200
        this.__proto__ = trueProto;
    }
}
/** Error thrown when the selected transport is unsupported by the browser. */
/** @private */
class UnsupportedTransportError extends Error {
    /** Constructs a new instance of {@link @microsoft/signalr.UnsupportedTransportError}.
     *
     * @param {string} message A descriptive error message.
     * @param {HttpTransportType} transport The {@link @microsoft/signalr.HttpTransportType} this error occurred on.
     */
    constructor(message, transport) {
        const trueProto = new.target.prototype;
        super(message);
        this.transport = transport;
        this.errorType = 'UnsupportedTransportError';
        // Workaround issue in Typescript compiler
        // https://github.com/Microsoft/TypeScript/issues/13965#issuecomment-278570200
        this.__proto__ = trueProto;
    }
}
/** Error thrown when the selected transport is disabled by the browser. */
/** @private */
class DisabledTransportError extends Error {
    /** Constructs a new instance of {@link @microsoft/signalr.DisabledTransportError}.
     *
     * @param {string} message A descriptive error message.
     * @param {HttpTransportType} transport The {@link @microsoft/signalr.HttpTransportType} this error occurred on.
     */
    constructor(message, transport) {
        const trueProto = new.target.prototype;
        super(message);
        this.transport = transport;
        this.errorType = 'DisabledTransportError';
        // Workaround issue in Typescript compiler
        // https://github.com/Microsoft/TypeScript/issues/13965#issuecomment-278570200
        this.__proto__ = trueProto;
    }
}
/** Error thrown when the selected transport cannot be started. */
/** @private */
class FailedToStartTransportError extends Error {
    /** Constructs a new instance of {@link @microsoft/signalr.FailedToStartTransportError}.
     *
     * @param {string} message A descriptive error message.
     * @param {HttpTransportType} transport The {@link @microsoft/signalr.HttpTransportType} this error occurred on.
     */
    constructor(message, transport) {
        const trueProto = new.target.prototype;
        super(message);
        this.transport = transport;
        this.errorType = 'FailedToStartTransportError';
        // Workaround issue in Typescript compiler
        // https://github.com/Microsoft/TypeScript/issues/13965#issuecomment-278570200
        this.__proto__ = trueProto;
    }
}
/** Error thrown when the negotiation with the server failed to complete. */
/** @private */
class FailedToNegotiateWithServerError extends Error {
    /** Constructs a new instance of {@link @microsoft/signalr.FailedToNegotiateWithServerError}.
     *
     * @param {string} message A descriptive error message.
     */
    constructor(message) {
        const trueProto = new.target.prototype;
        super(message);
        this.errorType = 'FailedToNegotiateWithServerError';
        // Workaround issue in Typescript compiler
        // https://github.com/Microsoft/TypeScript/issues/13965#issuecomment-278570200
        this.__proto__ = trueProto;
    }
}
/** Error thrown when multiple errors have occurred. */
/** @private */
class AggregateErrors extends Error {
    /** Constructs a new instance of {@link @microsoft/signalr.AggregateErrors}.
     *
     * @param {string} message A descriptive error message.
     * @param {Error[]} innerErrors The collection of errors this error is aggregating.
     */
    constructor(message, innerErrors) {
        const trueProto = new.target.prototype;
        super(message);
        this.innerErrors = innerErrors;
        // Workaround issue in Typescript compiler
        // https://github.com/Microsoft/TypeScript/issues/13965#issuecomment-278570200
        this.__proto__ = trueProto;
    }
}

;// CONCATENATED MODULE: ./src/HttpClient.ts
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
/** Represents an HTTP response. */
class HttpResponse {
    constructor(statusCode, statusText, content) {
        this.statusCode = statusCode;
        this.statusText = statusText;
        this.content = content;
    }
}
/** Abstraction over an HTTP client.
 *
 * This class provides an abstraction over an HTTP client so that a different implementation can be provided on different platforms.
 */
class HttpClient {
    get(url, options) {
        return this.send({
            ...options,
            method: "GET",
            url,
        });
    }
    post(url, options) {
        return this.send({
            ...options,
            method: "POST",
            url,
        });
    }
    delete(url, options) {
        return this.send({
            ...options,
            method: "DELETE",
            url,
        });
    }
    /** Gets all cookies that apply to the specified URL.
     *
     * @param url The URL that the cookies are valid for.
     * @returns {string} A string containing all the key-value cookie pairs for the specified URL.
     */
    // @ts-ignore
    getCookieString(url) {
        return "";
    }
}

;// CONCATENATED MODULE: ./src/ILogger.ts
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// These values are designed to match the ASP.NET Log Levels since that's the pattern we're emulating here.
/** Indicates the severity of a log message.
 *
 * Log Levels are ordered in increasing severity. So `Debug` is more severe than `Trace`, etc.
 */
var LogLevel;
(function (LogLevel) {
    /** Log level for very low severity diagnostic messages. */
    LogLevel[LogLevel["Trace"] = 0] = "Trace";
    /** Log level for low severity diagnostic messages. */
    LogLevel[LogLevel["Debug"] = 1] = "Debug";
    /** Log level for informational diagnostic messages. */
    LogLevel[LogLevel["Information"] = 2] = "Information";
    /** Log level for diagnostic messages that indicate a non-fatal problem. */
    LogLevel[LogLevel["Warning"] = 3] = "Warning";
    /** Log level for diagnostic messages that indicate a failure in the current operation. */
    LogLevel[LogLevel["Error"] = 4] = "Error";
    /** Log level for diagnostic messages that indicate a failure that will terminate the entire application. */
    LogLevel[LogLevel["Critical"] = 5] = "Critical";
    /** The highest possible log level. Used when configuring logging to indicate that no log messages should be emitted. */
    LogLevel[LogLevel["None"] = 6] = "None";
})(LogLevel || (LogLevel = {}));

;// CONCATENATED MODULE: ./src/Loggers.ts
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
/** A logger that does nothing when log messages are sent to it. */
class NullLogger {
    constructor() { }
    /** @inheritDoc */
    // eslint-disable-next-line
    log(_logLevel, _message) {
    }
}
/** The singleton instance of the {@link @microsoft/signalr.NullLogger}. */
NullLogger.instance = new NullLogger();

;// CONCATENATED MODULE: ./src/Utils.ts
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


// Version token that will be replaced by the prepack command
/** The version of the SignalR client. */
const VERSION = "8.0.7";
/** @private */
class Arg {
    static isRequired(val, name) {
        if (val === null || val === undefined) {
            throw new Error(`The '${name}' argument is required.`);
        }
    }
    static isNotEmpty(val, name) {
        if (!val || val.match(/^\s*$/)) {
            throw new Error(`The '${name}' argument should not be empty.`);
        }
    }
    static isIn(val, values, name) {
        // TypeScript enums have keys for **both** the name and the value of each enum member on the type itself.
        if (!(val in values)) {
            throw new Error(`Unknown ${name} value: ${val}.`);
        }
    }
}
/** @private */
class Platform {
    // react-native has a window but no document so we should check both
    static get isBrowser() {
        return !Platform.isNode && typeof window === "object" && typeof window.document === "object";
    }
    // WebWorkers don't have a window object so the isBrowser check would fail
    static get isWebWorker() {
        return !Platform.isNode && typeof self === "object" && "importScripts" in self;
    }
    // react-native has a window but no document
    static get isReactNative() {
        return !Platform.isNode && typeof window === "object" && typeof window.document === "undefined";
    }
    // Node apps shouldn't have a window object, but WebWorkers don't either
    // so we need to check for both WebWorker and window
    static get isNode() {
        return typeof process !== "undefined" && process.release && process.release.name === "node";
    }
}
/** @private */
function getDataDetail(data, includeContent) {
    let detail = "";
    if (isArrayBuffer(data)) {
        detail = `Binary data of length ${data.byteLength}`;
        if (includeContent) {
            detail += `. Content: '${formatArrayBuffer(data)}'`;
        }
    }
    else if (typeof data === "string") {
        detail = `String data of length ${data.length}`;
        if (includeContent) {
            detail += `. Content: '${data}'`;
        }
    }
    return detail;
}
/** @private */
function formatArrayBuffer(data) {
    const view = new Uint8Array(data);
    // Uint8Array.map only supports returning another Uint8Array?
    let str = "";
    view.forEach((num) => {
        const pad = num < 16 ? "0" : "";
        str += `0x${pad}${num.toString(16)} `;
    });
    // Trim of trailing space.
    return str.substr(0, str.length - 1);
}
// Also in signalr-protocol-msgpack/Utils.ts
/** @private */
function isArrayBuffer(val) {
    return val && typeof ArrayBuffer !== "undefined" &&
        (val instanceof ArrayBuffer ||
            // Sometimes we get an ArrayBuffer that doesn't satisfy instanceof
            (val.constructor && val.constructor.name === "ArrayBuffer"));
}
/** @private */
async function sendMessage(logger, transportName, httpClient, url, content, options) {
    const headers = {};
    const [name, value] = getUserAgentHeader();
    headers[name] = value;
    logger.log(LogLevel.Trace, `(${transportName} transport) sending data. ${getDataDetail(content, options.logMessageContent)}.`);
    const responseType = isArrayBuffer(content) ? "arraybuffer" : "text";
    const response = await httpClient.post(url, {
        content,
        headers: { ...headers, ...options.headers },
        responseType,
        timeout: options.timeout,
        withCredentials: options.withCredentials,
    });
    logger.log(LogLevel.Trace, `(${transportName} transport) request complete. Response status: ${response.statusCode}.`);
}
/** @private */
function createLogger(logger) {
    if (logger === undefined) {
        return new ConsoleLogger(LogLevel.Information);
    }
    if (logger === null) {
        return NullLogger.instance;
    }
    if (logger.log !== undefined) {
        return logger;
    }
    return new ConsoleLogger(logger);
}
/** @private */
class SubjectSubscription {
    constructor(subject, observer) {
        this._subject = subject;
        this._observer = observer;
    }
    dispose() {
        const index = this._subject.observers.indexOf(this._observer);
        if (index > -1) {
            this._subject.observers.splice(index, 1);
        }
        if (this._subject.observers.length === 0 && this._subject.cancelCallback) {
            this._subject.cancelCallback().catch((_) => { });
        }
    }
}
/** @private */
class ConsoleLogger {
    constructor(minimumLogLevel) {
        this._minLevel = minimumLogLevel;
        this.out = console;
    }
    log(logLevel, message) {
        if (logLevel >= this._minLevel) {
            const msg = `[${new Date().toISOString()}] ${LogLevel[logLevel]}: ${message}`;
            switch (logLevel) {
                case LogLevel.Critical:
                case LogLevel.Error:
                    this.out.error(msg);
                    break;
                case LogLevel.Warning:
                    this.out.warn(msg);
                    break;
                case LogLevel.Information:
                    this.out.info(msg);
                    break;
                default:
                    // console.debug only goes to attached debuggers in Node, so we use console.log for Trace and Debug
                    this.out.log(msg);
                    break;
            }
        }
    }
}
/** @private */
function getUserAgentHeader() {
    let userAgentHeaderName = "X-SignalR-User-Agent";
    if (Platform.isNode) {
        userAgentHeaderName = "User-Agent";
    }
    return [userAgentHeaderName, constructUserAgent(VERSION, getOsName(), getRuntime(), getRuntimeVersion())];
}
/** @private */
function constructUserAgent(version, os, runtime, runtimeVersion) {
    // Microsoft SignalR/[Version] ([Detailed Version]; [Operating System]; [Runtime]; [Runtime Version])
    let userAgent = "Microsoft SignalR/";
    const majorAndMinor = version.split(".");
    userAgent += `${majorAndMinor[0]}.${majorAndMinor[1]}`;
    userAgent += ` (${version}; `;
    if (os && os !== "") {
        userAgent += `${os}; `;
    }
    else {
        userAgent += "Unknown OS; ";
    }
    userAgent += `${runtime}`;
    if (runtimeVersion) {
        userAgent += `; ${runtimeVersion}`;
    }
    else {
        userAgent += "; Unknown Runtime Version";
    }
    userAgent += ")";
    return userAgent;
}
// eslint-disable-next-line spaced-comment
/*#__PURE__*/ function getOsName() {
    if (Platform.isNode) {
        switch (process.platform) {
            case "win32":
                return "Windows NT";
            case "darwin":
                return "macOS";
            case "linux":
                return "Linux";
            default:
                return process.platform;
        }
    }
    else {
        return "";
    }
}
// eslint-disable-next-line spaced-comment
/*#__PURE__*/ function getRuntimeVersion() {
    if (Platform.isNode) {
        return process.versions.node;
    }
    return undefined;
}
function getRuntime() {
    if (Platform.isNode) {
        return "NodeJS";
    }
    else {
        return "Browser";
    }
}
/** @private */
function getErrorString(e) {
    if (e.stack) {
        return e.stack;
    }
    else if (e.message) {
        return e.message;
    }
    return `${e}`;
}
/** @private */
function getGlobalThis() {
    // globalThis is semi-new and not available in Node until v12
    if (typeof globalThis !== "undefined") {
        return globalThis;
    }
    if (typeof self !== "undefined") {
        return self;
    }
    if (typeof window !== "undefined") {
        return window;
    }
    if (typeof __webpack_require__.g !== "undefined") {
        return __webpack_require__.g;
    }
    throw new Error("could not find global");
}

;// CONCATENATED MODULE: ./src/FetchHttpClient.ts
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.




class FetchHttpClient extends HttpClient {
    constructor(logger) {
        super();
        this._logger = logger;
        // Node added a fetch implementation to the global scope starting in v18.
        // We need to add a cookie jar in node to be able to share cookies with WebSocket
        if (typeof fetch === "undefined" || Platform.isNode) {
            // In order to ignore the dynamic require in webpack builds we need to do this magic
            // @ts-ignore: TS doesn't know about these names
            const requireFunc =  true ? require : 0;
            // Cookies aren't automatically handled in Node so we need to add a CookieJar to preserve cookies across requests
            this._jar = new (requireFunc("tough-cookie")).CookieJar();
            if (typeof fetch === "undefined") {
                this._fetchType = requireFunc("node-fetch");
            }
            else {
                // Use fetch from Node if available
                this._fetchType = fetch;
            }
            // node-fetch doesn't have a nice API for getting and setting cookies
            // fetch-cookie will wrap a fetch implementation with a default CookieJar or a provided one
            this._fetchType = requireFunc("fetch-cookie")(this._fetchType, this._jar);
        }
        else {
            this._fetchType = fetch.bind(getGlobalThis());
        }
        if (typeof AbortController === "undefined") {
            // In order to ignore the dynamic require in webpack builds we need to do this magic
            // @ts-ignore: TS doesn't know about these names
            const requireFunc =  true ? require : 0;
            // Node needs EventListener methods on AbortController which our custom polyfill doesn't provide
            this._abortControllerType = requireFunc("abort-controller");
        }
        else {
            this._abortControllerType = AbortController;
        }
    }
    /** @inheritDoc */
    async send(request) {
        // Check that abort was not signaled before calling send
        if (request.abortSignal && request.abortSignal.aborted) {
            throw new AbortError();
        }
        if (!request.method) {
            throw new Error("No method defined.");
        }
        if (!request.url) {
            throw new Error("No url defined.");
        }
        const abortController = new this._abortControllerType();
        let error;
        // Hook our abortSignal into the abort controller
        if (request.abortSignal) {
            request.abortSignal.onabort = () => {
                abortController.abort();
                error = new AbortError();
            };
        }
        // If a timeout has been passed in, setup a timeout to call abort
        // Type needs to be any to fit window.setTimeout and NodeJS.setTimeout
        let timeoutId = null;
        if (request.timeout) {
            const msTimeout = request.timeout;
            timeoutId = setTimeout(() => {
                abortController.abort();
                this._logger.log(LogLevel.Warning, `Timeout from HTTP request.`);
                error = new TimeoutError();
            }, msTimeout);
        }
        if (request.content === "") {
            request.content = undefined;
        }
        if (request.content) {
            // Explicitly setting the Content-Type header for React Native on Android platform.
            request.headers = request.headers || {};
            if (isArrayBuffer(request.content)) {
                request.headers["Content-Type"] = "application/octet-stream";
            }
            else {
                request.headers["Content-Type"] = "text/plain;charset=UTF-8";
            }
        }
        let response;
        try {
            response = await this._fetchType(request.url, {
                body: request.content,
                cache: "no-cache",
                credentials: request.withCredentials === true ? "include" : "same-origin",
                headers: {
                    "X-Requested-With": "XMLHttpRequest",
                    ...request.headers,
                },
                method: request.method,
                mode: "cors",
                redirect: "follow",
                signal: abortController.signal,
            });
        }
        catch (e) {
            if (error) {
                throw error;
            }
            this._logger.log(LogLevel.Warning, `Error from HTTP request. ${e}.`);
            throw e;
        }
        finally {
            if (timeoutId) {
                clearTimeout(timeoutId);
            }
            if (request.abortSignal) {
                request.abortSignal.onabort = null;
            }
        }
        if (!response.ok) {
            const errorMessage = await deserializeContent(response, "text");
            throw new HttpError(errorMessage || response.statusText, response.status);
        }
        const content = deserializeContent(response, request.responseType);
        const payload = await content;
        return new HttpResponse(response.status, response.statusText, payload);
    }
    getCookieString(url) {
        let cookies = "";
        if (Platform.isNode && this._jar) {
            // @ts-ignore: unused variable
            this._jar.getCookies(url, (e, c) => cookies = c.join("; "));
        }
        return cookies;
    }
}
function deserializeContent(response, responseType) {
    let content;
    switch (responseType) {
        case "arraybuffer":
            content = response.arrayBuffer();
            break;
        case "text":
            content = response.text();
            break;
        case "blob":
        case "document":
        case "json":
            throw new Error(`${responseType} is not supported.`);
        default:
            content = response.text();
            break;
    }
    return content;
}

;// CONCATENATED MODULE: ./src/XhrHttpClient.ts
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.




class XhrHttpClient extends HttpClient {
    constructor(logger) {
        super();
        this._logger = logger;
    }
    /** @inheritDoc */
    send(request) {
        // Check that abort was not signaled before calling send
        if (request.abortSignal && request.abortSignal.aborted) {
            return Promise.reject(new AbortError());
        }
        if (!request.method) {
            return Promise.reject(new Error("No method defined."));
        }
        if (!request.url) {
            return Promise.reject(new Error("No url defined."));
        }
        return new Promise((resolve, reject) => {
            const xhr = new XMLHttpRequest();
            xhr.open(request.method, request.url, true);
            xhr.withCredentials = request.withCredentials === undefined ? true : request.withCredentials;
            xhr.setRequestHeader("X-Requested-With", "XMLHttpRequest");
            if (request.content === "") {
                request.content = undefined;
            }
            if (request.content) {
                // Explicitly setting the Content-Type header for React Native on Android platform.
                if (isArrayBuffer(request.content)) {
                    xhr.setRequestHeader("Content-Type", "application/octet-stream");
                }
                else {
                    xhr.setRequestHeader("Content-Type", "text/plain;charset=UTF-8");
                }
            }
            const headers = request.headers;
            if (headers) {
                Object.keys(headers)
                    .forEach((header) => {
                    xhr.setRequestHeader(header, headers[header]);
                });
            }
            if (request.responseType) {
                xhr.responseType = request.responseType;
            }
            if (request.abortSignal) {
                request.abortSignal.onabort = () => {
                    xhr.abort();
                    reject(new AbortError());
                };
            }
            if (request.timeout) {
                xhr.timeout = request.timeout;
            }
            xhr.onload = () => {
                if (request.abortSignal) {
                    request.abortSignal.onabort = null;
                }
                if (xhr.status >= 200 && xhr.status < 300) {
                    resolve(new HttpResponse(xhr.status, xhr.statusText, xhr.response || xhr.responseText));
                }
                else {
                    reject(new HttpError(xhr.response || xhr.responseText || xhr.statusText, xhr.status));
                }
            };
            xhr.onerror = () => {
                this._logger.log(LogLevel.Warning, `Error from HTTP request. ${xhr.status}: ${xhr.statusText}.`);
                reject(new HttpError(xhr.statusText, xhr.status));
            };
            xhr.ontimeout = () => {
                this._logger.log(LogLevel.Warning, `Timeout from HTTP request.`);
                reject(new TimeoutError());
            };
            xhr.send(request.content);
        });
    }
}

;// CONCATENATED MODULE: ./src/DefaultHttpClient.ts
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.





/** Default implementation of {@link @microsoft/signalr.HttpClient}. */
class DefaultHttpClient extends HttpClient {
    /** Creates a new instance of the {@link @microsoft/signalr.DefaultHttpClient}, using the provided {@link @microsoft/signalr.ILogger} to log messages. */
    constructor(logger) {
        super();
        if (typeof fetch !== "undefined" || Platform.isNode) {
            this._httpClient = new FetchHttpClient(logger);
        }
        else if (typeof XMLHttpRequest !== "undefined") {
            this._httpClient = new XhrHttpClient(logger);
        }
        else {
            throw new Error("No usable HttpClient found.");
        }
    }
    /** @inheritDoc */
    send(request) {
        // Check that abort was not signaled before calling send
        if (request.abortSignal && request.abortSignal.aborted) {
            return Promise.reject(new AbortError());
        }
        if (!request.method) {
            return Promise.reject(new Error("No method defined."));
        }
        if (!request.url) {
            return Promise.reject(new Error("No url defined."));
        }
        return this._httpClient.send(request);
    }
    getCookieString(url) {
        return this._httpClient.getCookieString(url);
    }
}

;// CONCATENATED MODULE: ./src/TextMessageFormat.ts
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// Not exported from index
/** @private */
class TextMessageFormat {
    static write(output) {
        return `${output}${TextMessageFormat.RecordSeparator}`;
    }
    static parse(input) {
        if (input[input.length - 1] !== TextMessageFormat.RecordSeparator) {
            throw new Error("Message is incomplete.");
        }
        const messages = input.split(TextMessageFormat.RecordSeparator);
        messages.pop();
        return messages;
    }
}
TextMessageFormat.RecordSeparatorCode = 0x1e;
TextMessageFormat.RecordSeparator = String.fromCharCode(TextMessageFormat.RecordSeparatorCode);

;// CONCATENATED MODULE: ./src/HandshakeProtocol.ts
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


/** @private */
class HandshakeProtocol {
    // Handshake request is always JSON
    writeHandshakeRequest(handshakeRequest) {
        return TextMessageFormat.write(JSON.stringify(handshakeRequest));
    }
    parseHandshakeResponse(data) {
        let messageData;
        let remainingData;
        if (isArrayBuffer(data)) {
            // Format is binary but still need to read JSON text from handshake response
            const binaryData = new Uint8Array(data);
            const separatorIndex = binaryData.indexOf(TextMessageFormat.RecordSeparatorCode);
            if (separatorIndex === -1) {
                throw new Error("Message is incomplete.");
            }
            // content before separator is handshake response
            // optional content after is additional messages
            const responseLength = separatorIndex + 1;
            messageData = String.fromCharCode.apply(null, Array.prototype.slice.call(binaryData.slice(0, responseLength)));
            remainingData = (binaryData.byteLength > responseLength) ? binaryData.slice(responseLength).buffer : null;
        }
        else {
            const textData = data;
            const separatorIndex = textData.indexOf(TextMessageFormat.RecordSeparator);
            if (separatorIndex === -1) {
                throw new Error("Message is incomplete.");
            }
            // content before separator is handshake response
            // optional content after is additional messages
            const responseLength = separatorIndex + 1;
            messageData = textData.substring(0, responseLength);
            remainingData = (textData.length > responseLength) ? textData.substring(responseLength) : null;
        }
        // At this point we should have just the single handshake message
        const messages = TextMessageFormat.parse(messageData);
        const response = JSON.parse(messages[0]);
        if (response.type) {
            throw new Error("Expected a handshake response from the server.");
        }
        const responseMessage = response;
        // multiple messages could have arrived with handshake
        // return additional data to be parsed as usual, or null if all parsed
        return [remainingData, responseMessage];
    }
}

;// CONCATENATED MODULE: ./src/IHubProtocol.ts
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
/** Defines the type of a Hub Message. */
var MessageType;
(function (MessageType) {
    /** Indicates the message is an Invocation message and implements the {@link @microsoft/signalr.InvocationMessage} interface. */
    MessageType[MessageType["Invocation"] = 1] = "Invocation";
    /** Indicates the message is a StreamItem message and implements the {@link @microsoft/signalr.StreamItemMessage} interface. */
    MessageType[MessageType["StreamItem"] = 2] = "StreamItem";
    /** Indicates the message is a Completion message and implements the {@link @microsoft/signalr.CompletionMessage} interface. */
    MessageType[MessageType["Completion"] = 3] = "Completion";
    /** Indicates the message is a Stream Invocation message and implements the {@link @microsoft/signalr.StreamInvocationMessage} interface. */
    MessageType[MessageType["StreamInvocation"] = 4] = "StreamInvocation";
    /** Indicates the message is a Cancel Invocation message and implements the {@link @microsoft/signalr.CancelInvocationMessage} interface. */
    MessageType[MessageType["CancelInvocation"] = 5] = "CancelInvocation";
    /** Indicates the message is a Ping message and implements the {@link @microsoft/signalr.PingMessage} interface. */
    MessageType[MessageType["Ping"] = 6] = "Ping";
    /** Indicates the message is a Close message and implements the {@link @microsoft/signalr.CloseMessage} interface. */
    MessageType[MessageType["Close"] = 7] = "Close";
    MessageType[MessageType["Ack"] = 8] = "Ack";
    MessageType[MessageType["Sequence"] = 9] = "Sequence";
})(MessageType || (MessageType = {}));

;// CONCATENATED MODULE: ./src/Subject.ts
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

/** Stream implementation to stream items to the server. */
class Subject {
    constructor() {
        this.observers = [];
    }
    next(item) {
        for (const observer of this.observers) {
            observer.next(item);
        }
    }
    error(err) {
        for (const observer of this.observers) {
            if (observer.error) {
                observer.error(err);
            }
        }
    }
    complete() {
        for (const observer of this.observers) {
            if (observer.complete) {
                observer.complete();
            }
        }
    }
    subscribe(observer) {
        this.observers.push(observer);
        return new SubjectSubscription(this, observer);
    }
}

;// CONCATENATED MODULE: ./src/MessageBuffer.ts
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


/** @private */
class MessageBuffer {
    constructor(protocol, connection, bufferSize) {
        this._bufferSize = 100000;
        this._messages = [];
        this._totalMessageCount = 0;
        this._waitForSequenceMessage = false;
        // Message IDs start at 1 and always increment by 1
        this._nextReceivingSequenceId = 1;
        this._latestReceivedSequenceId = 0;
        this._bufferedByteCount = 0;
        this._reconnectInProgress = false;
        this._protocol = protocol;
        this._connection = connection;
        this._bufferSize = bufferSize;
    }
    async _send(message) {
        const serializedMessage = this._protocol.writeMessage(message);
        let backpressurePromise = Promise.resolve();
        // Only count invocation messages. Acks, pings, etc. don't need to be resent on reconnect
        if (this._isInvocationMessage(message)) {
            this._totalMessageCount++;
            let backpressurePromiseResolver = () => { };
            let backpressurePromiseRejector = () => { };
            if (isArrayBuffer(serializedMessage)) {
                this._bufferedByteCount += serializedMessage.byteLength;
            }
            else {
                this._bufferedByteCount += serializedMessage.length;
            }
            if (this._bufferedByteCount >= this._bufferSize) {
                backpressurePromise = new Promise((resolve, reject) => {
                    backpressurePromiseResolver = resolve;
                    backpressurePromiseRejector = reject;
                });
            }
            this._messages.push(new BufferedItem(serializedMessage, this._totalMessageCount, backpressurePromiseResolver, backpressurePromiseRejector));
        }
        try {
            // If this is set it means we are reconnecting or resending
            // We don't want to send on a disconnected connection
            // And we don't want to send if resend is running since that would mean sending
            // this message twice
            if (!this._reconnectInProgress) {
                await this._connection.send(serializedMessage);
            }
        }
        catch {
            this._disconnected();
        }
        await backpressurePromise;
    }
    _ack(ackMessage) {
        let newestAckedMessage = -1;
        // Find index of newest message being acked
        for (let index = 0; index < this._messages.length; index++) {
            const element = this._messages[index];
            if (element._id <= ackMessage.sequenceId) {
                newestAckedMessage = index;
                if (isArrayBuffer(element._message)) {
                    this._bufferedByteCount -= element._message.byteLength;
                }
                else {
                    this._bufferedByteCount -= element._message.length;
                }
                // resolve items that have already been sent and acked
                element._resolver();
            }
            else if (this._bufferedByteCount < this._bufferSize) {
                // resolve items that now fall under the buffer limit but haven't been acked
                element._resolver();
            }
            else {
                break;
            }
        }
        if (newestAckedMessage !== -1) {
            // We're removing everything including the message pointed to, so add 1
            this._messages = this._messages.slice(newestAckedMessage + 1);
        }
    }
    _shouldProcessMessage(message) {
        if (this._waitForSequenceMessage) {
            if (message.type !== MessageType.Sequence) {
                return false;
            }
            else {
                this._waitForSequenceMessage = false;
                return true;
            }
        }
        // No special processing for acks, pings, etc.
        if (!this._isInvocationMessage(message)) {
            return true;
        }
        const currentId = this._nextReceivingSequenceId;
        this._nextReceivingSequenceId++;
        if (currentId <= this._latestReceivedSequenceId) {
            if (currentId === this._latestReceivedSequenceId) {
                // Should only hit this if we just reconnected and the server is sending
                // Messages it has buffered, which would mean it hasn't seen an Ack for these messages
                this._ackTimer();
            }
            // Ignore, this is a duplicate message
            return false;
        }
        this._latestReceivedSequenceId = currentId;
        // Only start the timer for sending an Ack message when we have a message to ack. This also conveniently solves
        // timer throttling by not having a recursive timer, and by starting the timer via a network call (recv)
        this._ackTimer();
        return true;
    }
    _resetSequence(message) {
        if (message.sequenceId > this._nextReceivingSequenceId) {
            // eslint-disable-next-line @typescript-eslint/no-floating-promises
            this._connection.stop(new Error("Sequence ID greater than amount of messages we've received."));
            return;
        }
        this._nextReceivingSequenceId = message.sequenceId;
    }
    _disconnected() {
        this._reconnectInProgress = true;
        this._waitForSequenceMessage = true;
    }
    async _resend() {
        const sequenceId = this._messages.length !== 0
            ? this._messages[0]._id
            : this._totalMessageCount + 1;
        await this._connection.send(this._protocol.writeMessage({ type: MessageType.Sequence, sequenceId }));
        // Get a local variable to the _messages, just in case messages are acked while resending
        // Which would slice the _messages array (which creates a new copy)
        const messages = this._messages;
        for (const element of messages) {
            await this._connection.send(element._message);
        }
        this._reconnectInProgress = false;
    }
    _dispose(error) {
        error !== null && error !== void 0 ? error : (error = new Error("Unable to reconnect to server."));
        // Unblock backpressure if any
        for (const element of this._messages) {
            element._rejector(error);
        }
    }
    _isInvocationMessage(message) {
        // There is no way to check if something implements an interface.
        // So we individually check the messages in a switch statement.
        // To make sure we don't miss any message types we rely on the compiler
        // seeing the function returns a value and it will do the
        // exhaustive check for us on the switch statement, since we don't use 'case default'
        switch (message.type) {
            case MessageType.Invocation:
            case MessageType.StreamItem:
            case MessageType.Completion:
            case MessageType.StreamInvocation:
            case MessageType.CancelInvocation:
                return true;
            case MessageType.Close:
            case MessageType.Sequence:
            case MessageType.Ping:
            case MessageType.Ack:
                return false;
        }
    }
    _ackTimer() {
        if (this._ackTimerHandle === undefined) {
            this._ackTimerHandle = setTimeout(async () => {
                try {
                    if (!this._reconnectInProgress) {
                        await this._connection.send(this._protocol.writeMessage({ type: MessageType.Ack, sequenceId: this._latestReceivedSequenceId }));
                    }
                    // Ignore errors, that means the connection is closed and we don't care about the Ack message anymore.
                }
                catch { }
                clearTimeout(this._ackTimerHandle);
                this._ackTimerHandle = undefined;
                // 1 second delay so we don't spam Ack messages if there are many messages being received at once.
            }, 1000);
        }
    }
}
class BufferedItem {
    constructor(message, id, resolver, rejector) {
        this._message = message;
        this._id = id;
        this._resolver = resolver;
        this._rejector = rejector;
    }
}

;// CONCATENATED MODULE: ./src/HubConnection.ts
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.







const DEFAULT_TIMEOUT_IN_MS = 30 * 1000;
const DEFAULT_PING_INTERVAL_IN_MS = 15 * 1000;
const DEFAULT_STATEFUL_RECONNECT_BUFFER_SIZE = 100000;
/** Describes the current state of the {@link HubConnection} to the server. */
var HubConnectionState;
(function (HubConnectionState) {
    /** The hub connection is disconnected. */
    HubConnectionState["Disconnected"] = "Disconnected";
    /** The hub connection is connecting. */
    HubConnectionState["Connecting"] = "Connecting";
    /** The hub connection is connected. */
    HubConnectionState["Connected"] = "Connected";
    /** The hub connection is disconnecting. */
    HubConnectionState["Disconnecting"] = "Disconnecting";
    /** The hub connection is reconnecting. */
    HubConnectionState["Reconnecting"] = "Reconnecting";
})(HubConnectionState || (HubConnectionState = {}));
/** Represents a connection to a SignalR Hub. */
class HubConnection {
    /** @internal */
    // Using a public static factory method means we can have a private constructor and an _internal_
    // create method that can be used by HubConnectionBuilder. An "internal" constructor would just
    // be stripped away and the '.d.ts' file would have no constructor, which is interpreted as a
    // public parameter-less constructor.
    static create(connection, logger, protocol, reconnectPolicy, serverTimeoutInMilliseconds, keepAliveIntervalInMilliseconds, statefulReconnectBufferSize) {
        return new HubConnection(connection, logger, protocol, reconnectPolicy, serverTimeoutInMilliseconds, keepAliveIntervalInMilliseconds, statefulReconnectBufferSize);
    }
    constructor(connection, logger, protocol, reconnectPolicy, serverTimeoutInMilliseconds, keepAliveIntervalInMilliseconds, statefulReconnectBufferSize) {
        this._nextKeepAlive = 0;
        this._freezeEventListener = () => {
            this._logger.log(LogLevel.Warning, "The page is being frozen, this will likely lead to the connection being closed and messages being lost. For more information see the docs at https://learn.microsoft.com/aspnet/core/signalr/javascript-client#bsleep");
        };
        Arg.isRequired(connection, "connection");
        Arg.isRequired(logger, "logger");
        Arg.isRequired(protocol, "protocol");
        this.serverTimeoutInMilliseconds = serverTimeoutInMilliseconds !== null && serverTimeoutInMilliseconds !== void 0 ? serverTimeoutInMilliseconds : DEFAULT_TIMEOUT_IN_MS;
        this.keepAliveIntervalInMilliseconds = keepAliveIntervalInMilliseconds !== null && keepAliveIntervalInMilliseconds !== void 0 ? keepAliveIntervalInMilliseconds : DEFAULT_PING_INTERVAL_IN_MS;
        this._statefulReconnectBufferSize = statefulReconnectBufferSize !== null && statefulReconnectBufferSize !== void 0 ? statefulReconnectBufferSize : DEFAULT_STATEFUL_RECONNECT_BUFFER_SIZE;
        this._logger = logger;
        this._protocol = protocol;
        this.connection = connection;
        this._reconnectPolicy = reconnectPolicy;
        this._handshakeProtocol = new HandshakeProtocol();
        this.connection.onreceive = (data) => this._processIncomingData(data);
        this.connection.onclose = (error) => this._connectionClosed(error);
        this._callbacks = {};
        this._methods = {};
        this._closedCallbacks = [];
        this._reconnectingCallbacks = [];
        this._reconnectedCallbacks = [];
        this._invocationId = 0;
        this._receivedHandshakeResponse = false;
        this._connectionState = HubConnectionState.Disconnected;
        this._connectionStarted = false;
        this._cachedPingMessage = this._protocol.writeMessage({ type: MessageType.Ping });
    }
    /** Indicates the state of the {@link HubConnection} to the server. */
    get state() {
        return this._connectionState;
    }
    /** Represents the connection id of the {@link HubConnection} on the server. The connection id will be null when the connection is either
     *  in the disconnected state or if the negotiation step was skipped.
     */
    get connectionId() {
        return this.connection ? (this.connection.connectionId || null) : null;
    }
    /** Indicates the url of the {@link HubConnection} to the server. */
    get baseUrl() {
        return this.connection.baseUrl || "";
    }
    /**
     * Sets a new url for the HubConnection. Note that the url can only be changed when the connection is in either the Disconnected or
     * Reconnecting states.
     * @param {string} url The url to connect to.
     */
    set baseUrl(url) {
        if (this._connectionState !== HubConnectionState.Disconnected && this._connectionState !== HubConnectionState.Reconnecting) {
            throw new Error("The HubConnection must be in the Disconnected or Reconnecting state to change the url.");
        }
        if (!url) {
            throw new Error("The HubConnection url must be a valid url.");
        }
        this.connection.baseUrl = url;
    }
    /** Starts the connection.
     *
     * @returns {Promise<void>} A Promise that resolves when the connection has been successfully established, or rejects with an error.
     */
    start() {
        this._startPromise = this._startWithStateTransitions();
        return this._startPromise;
    }
    async _startWithStateTransitions() {
        if (this._connectionState !== HubConnectionState.Disconnected) {
            return Promise.reject(new Error("Cannot start a HubConnection that is not in the 'Disconnected' state."));
        }
        this._connectionState = HubConnectionState.Connecting;
        this._logger.log(LogLevel.Debug, "Starting HubConnection.");
        try {
            await this._startInternal();
            if (Platform.isBrowser) {
                // Log when the browser freezes the tab so users know why their connection unexpectedly stopped working
                window.document.addEventListener("freeze", this._freezeEventListener);
            }
            this._connectionState = HubConnectionState.Connected;
            this._connectionStarted = true;
            this._logger.log(LogLevel.Debug, "HubConnection connected successfully.");
        }
        catch (e) {
            this._connectionState = HubConnectionState.Disconnected;
            this._logger.log(LogLevel.Debug, `HubConnection failed to start successfully because of error '${e}'.`);
            return Promise.reject(e);
        }
    }
    async _startInternal() {
        this._stopDuringStartError = undefined;
        this._receivedHandshakeResponse = false;
        // Set up the promise before any connection is (re)started otherwise it could race with received messages
        const handshakePromise = new Promise((resolve, reject) => {
            this._handshakeResolver = resolve;
            this._handshakeRejecter = reject;
        });
        await this.connection.start(this._protocol.transferFormat);
        try {
            let version = this._protocol.version;
            if (!this.connection.features.reconnect) {
                // Stateful Reconnect starts with HubProtocol version 2, newer clients connecting to older servers will fail to connect due to
                // the handshake only supporting version 1, so we will try to send version 1 during the handshake to keep old servers working.
                version = 1;
            }
            const handshakeRequest = {
                protocol: this._protocol.name,
                version,
            };
            this._logger.log(LogLevel.Debug, "Sending handshake request.");
            await this._sendMessage(this._handshakeProtocol.writeHandshakeRequest(handshakeRequest));
            this._logger.log(LogLevel.Information, `Using HubProtocol '${this._protocol.name}'.`);
            // defensively cleanup timeout in case we receive a message from the server before we finish start
            this._cleanupTimeout();
            this._resetTimeoutPeriod();
            this._resetKeepAliveInterval();
            await handshakePromise;
            // It's important to check the stopDuringStartError instead of just relying on the handshakePromise
            // being rejected on close, because this continuation can run after both the handshake completed successfully
            // and the connection was closed.
            if (this._stopDuringStartError) {
                // It's important to throw instead of returning a rejected promise, because we don't want to allow any state
                // transitions to occur between now and the calling code observing the exceptions. Returning a rejected promise
                // will cause the calling continuation to get scheduled to run later.
                // eslint-disable-next-line @typescript-eslint/no-throw-literal
                throw this._stopDuringStartError;
            }
            const useStatefulReconnect = this.connection.features.reconnect || false;
            if (useStatefulReconnect) {
                this._messageBuffer = new MessageBuffer(this._protocol, this.connection, this._statefulReconnectBufferSize);
                this.connection.features.disconnected = this._messageBuffer._disconnected.bind(this._messageBuffer);
                this.connection.features.resend = () => {
                    if (this._messageBuffer) {
                        return this._messageBuffer._resend();
                    }
                };
            }
            if (!this.connection.features.inherentKeepAlive) {
                await this._sendMessage(this._cachedPingMessage);
            }
        }
        catch (e) {
            this._logger.log(LogLevel.Debug, `Hub handshake failed with error '${e}' during start(). Stopping HubConnection.`);
            this._cleanupTimeout();
            this._cleanupPingTimer();
            // HttpConnection.stop() should not complete until after the onclose callback is invoked.
            // This will transition the HubConnection to the disconnected state before HttpConnection.stop() completes.
            await this.connection.stop(e);
            throw e;
        }
    }
    /** Stops the connection.
     *
     * @returns {Promise<void>} A Promise that resolves when the connection has been successfully terminated, or rejects with an error.
     */
    async stop() {
        // Capture the start promise before the connection might be restarted in an onclose callback.
        const startPromise = this._startPromise;
        this.connection.features.reconnect = false;
        this._stopPromise = this._stopInternal();
        await this._stopPromise;
        try {
            // Awaiting undefined continues immediately
            await startPromise;
        }
        catch (e) {
            // This exception is returned to the user as a rejected Promise from the start method.
        }
    }
    _stopInternal(error) {
        if (this._connectionState === HubConnectionState.Disconnected) {
            this._logger.log(LogLevel.Debug, `Call to HubConnection.stop(${error}) ignored because it is already in the disconnected state.`);
            return Promise.resolve();
        }
        if (this._connectionState === HubConnectionState.Disconnecting) {
            this._logger.log(LogLevel.Debug, `Call to HttpConnection.stop(${error}) ignored because the connection is already in the disconnecting state.`);
            return this._stopPromise;
        }
        const state = this._connectionState;
        this._connectionState = HubConnectionState.Disconnecting;
        this._logger.log(LogLevel.Debug, "Stopping HubConnection.");
        if (this._reconnectDelayHandle) {
            // We're in a reconnect delay which means the underlying connection is currently already stopped.
            // Just clear the handle to stop the reconnect loop (which no one is waiting on thankfully) and
            // fire the onclose callbacks.
            this._logger.log(LogLevel.Debug, "Connection stopped during reconnect delay. Done reconnecting.");
            clearTimeout(this._reconnectDelayHandle);
            this._reconnectDelayHandle = undefined;
            this._completeClose();
            return Promise.resolve();
        }
        if (state === HubConnectionState.Connected) {
            // eslint-disable-next-line @typescript-eslint/no-floating-promises
            this._sendCloseMessage();
        }
        this._cleanupTimeout();
        this._cleanupPingTimer();
        this._stopDuringStartError = error || new AbortError("The connection was stopped before the hub handshake could complete.");
        // HttpConnection.stop() should not complete until after either HttpConnection.start() fails
        // or the onclose callback is invoked. The onclose callback will transition the HubConnection
        // to the disconnected state if need be before HttpConnection.stop() completes.
        return this.connection.stop(error);
    }
    async _sendCloseMessage() {
        try {
            await this._sendWithProtocol(this._createCloseMessage());
        }
        catch {
            // Ignore, this is a best effort attempt to let the server know the client closed gracefully.
        }
    }
    /** Invokes a streaming hub method on the server using the specified name and arguments.
     *
     * @typeparam T The type of the items returned by the server.
     * @param {string} methodName The name of the server method to invoke.
     * @param {any[]} args The arguments used to invoke the server method.
     * @returns {IStreamResult<T>} An object that yields results from the server as they are received.
     */
    stream(methodName, ...args) {
        const [streams, streamIds] = this._replaceStreamingParams(args);
        const invocationDescriptor = this._createStreamInvocation(methodName, args, streamIds);
        // eslint-disable-next-line prefer-const
        let promiseQueue;
        const subject = new Subject();
        subject.cancelCallback = () => {
            const cancelInvocation = this._createCancelInvocation(invocationDescriptor.invocationId);
            delete this._callbacks[invocationDescriptor.invocationId];
            return promiseQueue.then(() => {
                return this._sendWithProtocol(cancelInvocation);
            });
        };
        this._callbacks[invocationDescriptor.invocationId] = (invocationEvent, error) => {
            if (error) {
                subject.error(error);
                return;
            }
            else if (invocationEvent) {
                // invocationEvent will not be null when an error is not passed to the callback
                if (invocationEvent.type === MessageType.Completion) {
                    if (invocationEvent.error) {
                        subject.error(new Error(invocationEvent.error));
                    }
                    else {
                        subject.complete();
                    }
                }
                else {
                    subject.next((invocationEvent.item));
                }
            }
        };
        promiseQueue = this._sendWithProtocol(invocationDescriptor)
            .catch((e) => {
            subject.error(e);
            delete this._callbacks[invocationDescriptor.invocationId];
        });
        this._launchStreams(streams, promiseQueue);
        return subject;
    }
    _sendMessage(message) {
        this._resetKeepAliveInterval();
        return this.connection.send(message);
    }
    /**
     * Sends a js object to the server.
     * @param message The js object to serialize and send.
     */
    _sendWithProtocol(message) {
        if (this._messageBuffer) {
            return this._messageBuffer._send(message);
        }
        else {
            return this._sendMessage(this._protocol.writeMessage(message));
        }
    }
    /** Invokes a hub method on the server using the specified name and arguments. Does not wait for a response from the receiver.
     *
     * The Promise returned by this method resolves when the client has sent the invocation to the server. The server may still
     * be processing the invocation.
     *
     * @param {string} methodName The name of the server method to invoke.
     * @param {any[]} args The arguments used to invoke the server method.
     * @returns {Promise<void>} A Promise that resolves when the invocation has been successfully sent, or rejects with an error.
     */
    send(methodName, ...args) {
        const [streams, streamIds] = this._replaceStreamingParams(args);
        const sendPromise = this._sendWithProtocol(this._createInvocation(methodName, args, true, streamIds));
        this._launchStreams(streams, sendPromise);
        return sendPromise;
    }
    /** Invokes a hub method on the server using the specified name and arguments.
     *
     * The Promise returned by this method resolves when the server indicates it has finished invoking the method. When the promise
     * resolves, the server has finished invoking the method. If the server method returns a result, it is produced as the result of
     * resolving the Promise.
     *
     * @typeparam T The expected return type.
     * @param {string} methodName The name of the server method to invoke.
     * @param {any[]} args The arguments used to invoke the server method.
     * @returns {Promise<T>} A Promise that resolves with the result of the server method (if any), or rejects with an error.
     */
    invoke(methodName, ...args) {
        const [streams, streamIds] = this._replaceStreamingParams(args);
        const invocationDescriptor = this._createInvocation(methodName, args, false, streamIds);
        const p = new Promise((resolve, reject) => {
            // invocationId will always have a value for a non-blocking invocation
            this._callbacks[invocationDescriptor.invocationId] = (invocationEvent, error) => {
                if (error) {
                    reject(error);
                    return;
                }
                else if (invocationEvent) {
                    // invocationEvent will not be null when an error is not passed to the callback
                    if (invocationEvent.type === MessageType.Completion) {
                        if (invocationEvent.error) {
                            reject(new Error(invocationEvent.error));
                        }
                        else {
                            resolve(invocationEvent.result);
                        }
                    }
                    else {
                        reject(new Error(`Unexpected message type: ${invocationEvent.type}`));
                    }
                }
            };
            const promiseQueue = this._sendWithProtocol(invocationDescriptor)
                .catch((e) => {
                reject(e);
                // invocationId will always have a value for a non-blocking invocation
                delete this._callbacks[invocationDescriptor.invocationId];
            });
            this._launchStreams(streams, promiseQueue);
        });
        return p;
    }
    on(methodName, newMethod) {
        if (!methodName || !newMethod) {
            return;
        }
        methodName = methodName.toLowerCase();
        if (!this._methods[methodName]) {
            this._methods[methodName] = [];
        }
        // Preventing adding the same handler multiple times.
        if (this._methods[methodName].indexOf(newMethod) !== -1) {
            return;
        }
        this._methods[methodName].push(newMethod);
    }
    off(methodName, method) {
        if (!methodName) {
            return;
        }
        methodName = methodName.toLowerCase();
        const handlers = this._methods[methodName];
        if (!handlers) {
            return;
        }
        if (method) {
            const removeIdx = handlers.indexOf(method);
            if (removeIdx !== -1) {
                handlers.splice(removeIdx, 1);
                if (handlers.length === 0) {
                    delete this._methods[methodName];
                }
            }
        }
        else {
            delete this._methods[methodName];
        }
    }
    /** Registers a handler that will be invoked when the connection is closed.
     *
     * @param {Function} callback The handler that will be invoked when the connection is closed. Optionally receives a single argument containing the error that caused the connection to close (if any).
     */
    onclose(callback) {
        if (callback) {
            this._closedCallbacks.push(callback);
        }
    }
    /** Registers a handler that will be invoked when the connection starts reconnecting.
     *
     * @param {Function} callback The handler that will be invoked when the connection starts reconnecting. Optionally receives a single argument containing the error that caused the connection to start reconnecting (if any).
     */
    onreconnecting(callback) {
        if (callback) {
            this._reconnectingCallbacks.push(callback);
        }
    }
    /** Registers a handler that will be invoked when the connection successfully reconnects.
     *
     * @param {Function} callback The handler that will be invoked when the connection successfully reconnects.
     */
    onreconnected(callback) {
        if (callback) {
            this._reconnectedCallbacks.push(callback);
        }
    }
    _processIncomingData(data) {
        this._cleanupTimeout();
        if (!this._receivedHandshakeResponse) {
            data = this._processHandshakeResponse(data);
            this._receivedHandshakeResponse = true;
        }
        // Data may have all been read when processing handshake response
        if (data) {
            // Parse the messages
            const messages = this._protocol.parseMessages(data, this._logger);
            for (const message of messages) {
                if (this._messageBuffer && !this._messageBuffer._shouldProcessMessage(message)) {
                    // Don't process the message, we are either waiting for a SequenceMessage or received a duplicate message
                    continue;
                }
                switch (message.type) {
                    case MessageType.Invocation:
                        this._invokeClientMethod(message)
                            .catch((e) => {
                            this._logger.log(LogLevel.Error, `Invoke client method threw error: ${getErrorString(e)}`);
                        });
                        break;
                    case MessageType.StreamItem:
                    case MessageType.Completion: {
                        const callback = this._callbacks[message.invocationId];
                        if (callback) {
                            if (message.type === MessageType.Completion) {
                                delete this._callbacks[message.invocationId];
                            }
                            try {
                                callback(message);
                            }
                            catch (e) {
                                this._logger.log(LogLevel.Error, `Stream callback threw error: ${getErrorString(e)}`);
                            }
                        }
                        break;
                    }
                    case MessageType.Ping:
                        // Don't care about pings
                        break;
                    case MessageType.Close: {
                        this._logger.log(LogLevel.Information, "Close message received from server.");
                        const error = message.error ? new Error("Server returned an error on close: " + message.error) : undefined;
                        if (message.allowReconnect === true) {
                            // It feels wrong not to await connection.stop() here, but processIncomingData is called as part of an onreceive callback which is not async,
                            // this is already the behavior for serverTimeout(), and HttpConnection.Stop() should catch and log all possible exceptions.
                            // eslint-disable-next-line @typescript-eslint/no-floating-promises
                            this.connection.stop(error);
                        }
                        else {
                            // We cannot await stopInternal() here, but subsequent calls to stop() will await this if stopInternal() is still ongoing.
                            this._stopPromise = this._stopInternal(error);
                        }
                        break;
                    }
                    case MessageType.Ack:
                        if (this._messageBuffer) {
                            this._messageBuffer._ack(message);
                        }
                        break;
                    case MessageType.Sequence:
                        if (this._messageBuffer) {
                            this._messageBuffer._resetSequence(message);
                        }
                        break;
                    default:
                        this._logger.log(LogLevel.Warning, `Invalid message type: ${message.type}.`);
                        break;
                }
            }
        }
        this._resetTimeoutPeriod();
    }
    _processHandshakeResponse(data) {
        let responseMessage;
        let remainingData;
        try {
            [remainingData, responseMessage] = this._handshakeProtocol.parseHandshakeResponse(data);
        }
        catch (e) {
            const message = "Error parsing handshake response: " + e;
            this._logger.log(LogLevel.Error, message);
            const error = new Error(message);
            this._handshakeRejecter(error);
            throw error;
        }
        if (responseMessage.error) {
            const message = "Server returned handshake error: " + responseMessage.error;
            this._logger.log(LogLevel.Error, message);
            const error = new Error(message);
            this._handshakeRejecter(error);
            throw error;
        }
        else {
            this._logger.log(LogLevel.Debug, "Server handshake complete.");
        }
        this._handshakeResolver();
        return remainingData;
    }
    _resetKeepAliveInterval() {
        if (this.connection.features.inherentKeepAlive) {
            return;
        }
        // Set the time we want the next keep alive to be sent
        // Timer will be setup on next message receive
        this._nextKeepAlive = new Date().getTime() + this.keepAliveIntervalInMilliseconds;
        this._cleanupPingTimer();
    }
    _resetTimeoutPeriod() {
        if (!this.connection.features || !this.connection.features.inherentKeepAlive) {
            // Set the timeout timer
            this._timeoutHandle = setTimeout(() => this.serverTimeout(), this.serverTimeoutInMilliseconds);
            // Set keepAlive timer if there isn't one
            if (this._pingServerHandle === undefined) {
                let nextPing = this._nextKeepAlive - new Date().getTime();
                if (nextPing < 0) {
                    nextPing = 0;
                }
                // The timer needs to be set from a networking callback to avoid Chrome timer throttling from causing timers to run once a minute
                this._pingServerHandle = setTimeout(async () => {
                    if (this._connectionState === HubConnectionState.Connected) {
                        try {
                            await this._sendMessage(this._cachedPingMessage);
                        }
                        catch {
                            // We don't care about the error. It should be seen elsewhere in the client.
                            // The connection is probably in a bad or closed state now, cleanup the timer so it stops triggering
                            this._cleanupPingTimer();
                        }
                    }
                }, nextPing);
            }
        }
    }
    // eslint-disable-next-line @typescript-eslint/naming-convention
    serverTimeout() {
        // The server hasn't talked to us in a while. It doesn't like us anymore ... :(
        // Terminate the connection, but we don't need to wait on the promise. This could trigger reconnecting.
        // eslint-disable-next-line @typescript-eslint/no-floating-promises
        this.connection.stop(new Error("Server timeout elapsed without receiving a message from the server."));
    }
    async _invokeClientMethod(invocationMessage) {
        const methodName = invocationMessage.target.toLowerCase();
        const methods = this._methods[methodName];
        if (!methods) {
            this._logger.log(LogLevel.Warning, `No client method with the name '${methodName}' found.`);
            // No handlers provided by client but the server is expecting a response still, so we send an error
            if (invocationMessage.invocationId) {
                this._logger.log(LogLevel.Warning, `No result given for '${methodName}' method and invocation ID '${invocationMessage.invocationId}'.`);
                await this._sendWithProtocol(this._createCompletionMessage(invocationMessage.invocationId, "Client didn't provide a result.", null));
            }
            return;
        }
        // Avoid issues with handlers removing themselves thus modifying the list while iterating through it
        const methodsCopy = methods.slice();
        // Server expects a response
        const expectsResponse = invocationMessage.invocationId ? true : false;
        // We preserve the last result or exception but still call all handlers
        let res;
        let exception;
        let completionMessage;
        for (const m of methodsCopy) {
            try {
                const prevRes = res;
                res = await m.apply(this, invocationMessage.arguments);
                if (expectsResponse && res && prevRes) {
                    this._logger.log(LogLevel.Error, `Multiple results provided for '${methodName}'. Sending error to server.`);
                    completionMessage = this._createCompletionMessage(invocationMessage.invocationId, `Client provided multiple results.`, null);
                }
                // Ignore exception if we got a result after, the exception will be logged
                exception = undefined;
            }
            catch (e) {
                exception = e;
                this._logger.log(LogLevel.Error, `A callback for the method '${methodName}' threw error '${e}'.`);
            }
        }
        if (completionMessage) {
            await this._sendWithProtocol(completionMessage);
        }
        else if (expectsResponse) {
            // If there is an exception that means either no result was given or a handler after a result threw
            if (exception) {
                completionMessage = this._createCompletionMessage(invocationMessage.invocationId, `${exception}`, null);
            }
            else if (res !== undefined) {
                completionMessage = this._createCompletionMessage(invocationMessage.invocationId, null, res);
            }
            else {
                this._logger.log(LogLevel.Warning, `No result given for '${methodName}' method and invocation ID '${invocationMessage.invocationId}'.`);
                // Client didn't provide a result or throw from a handler, server expects a response so we send an error
                completionMessage = this._createCompletionMessage(invocationMessage.invocationId, "Client didn't provide a result.", null);
            }
            await this._sendWithProtocol(completionMessage);
        }
        else {
            if (res) {
                this._logger.log(LogLevel.Error, `Result given for '${methodName}' method but server is not expecting a result.`);
            }
        }
    }
    _connectionClosed(error) {
        this._logger.log(LogLevel.Debug, `HubConnection.connectionClosed(${error}) called while in state ${this._connectionState}.`);
        // Triggering this.handshakeRejecter is insufficient because it could already be resolved without the continuation having run yet.
        this._stopDuringStartError = this._stopDuringStartError || error || new AbortError("The underlying connection was closed before the hub handshake could complete.");
        // If the handshake is in progress, start will be waiting for the handshake promise, so we complete it.
        // If it has already completed, this should just noop.
        if (this._handshakeResolver) {
            this._handshakeResolver();
        }
        this._cancelCallbacksWithError(error || new Error("Invocation canceled due to the underlying connection being closed."));
        this._cleanupTimeout();
        this._cleanupPingTimer();
        if (this._connectionState === HubConnectionState.Disconnecting) {
            this._completeClose(error);
        }
        else if (this._connectionState === HubConnectionState.Connected && this._reconnectPolicy) {
            // eslint-disable-next-line @typescript-eslint/no-floating-promises
            this._reconnect(error);
        }
        else if (this._connectionState === HubConnectionState.Connected) {
            this._completeClose(error);
        }
        // If none of the above if conditions were true were called the HubConnection must be in either:
        // 1. The Connecting state in which case the handshakeResolver will complete it and stopDuringStartError will fail it.
        // 2. The Reconnecting state in which case the handshakeResolver will complete it and stopDuringStartError will fail the current reconnect attempt
        //    and potentially continue the reconnect() loop.
        // 3. The Disconnected state in which case we're already done.
    }
    _completeClose(error) {
        if (this._connectionStarted) {
            this._connectionState = HubConnectionState.Disconnected;
            this._connectionStarted = false;
            if (this._messageBuffer) {
                this._messageBuffer._dispose(error !== null && error !== void 0 ? error : new Error("Connection closed."));
                this._messageBuffer = undefined;
            }
            if (Platform.isBrowser) {
                window.document.removeEventListener("freeze", this._freezeEventListener);
            }
            try {
                this._closedCallbacks.forEach((c) => c.apply(this, [error]));
            }
            catch (e) {
                this._logger.log(LogLevel.Error, `An onclose callback called with error '${error}' threw error '${e}'.`);
            }
        }
    }
    async _reconnect(error) {
        const reconnectStartTime = Date.now();
        let previousReconnectAttempts = 0;
        let retryError = error !== undefined ? error : new Error("Attempting to reconnect due to a unknown error.");
        let nextRetryDelay = this._getNextRetryDelay(previousReconnectAttempts++, 0, retryError);
        if (nextRetryDelay === null) {
            this._logger.log(LogLevel.Debug, "Connection not reconnecting because the IRetryPolicy returned null on the first reconnect attempt.");
            this._completeClose(error);
            return;
        }
        this._connectionState = HubConnectionState.Reconnecting;
        if (error) {
            this._logger.log(LogLevel.Information, `Connection reconnecting because of error '${error}'.`);
        }
        else {
            this._logger.log(LogLevel.Information, "Connection reconnecting.");
        }
        if (this._reconnectingCallbacks.length !== 0) {
            try {
                this._reconnectingCallbacks.forEach((c) => c.apply(this, [error]));
            }
            catch (e) {
                this._logger.log(LogLevel.Error, `An onreconnecting callback called with error '${error}' threw error '${e}'.`);
            }
            // Exit early if an onreconnecting callback called connection.stop().
            if (this._connectionState !== HubConnectionState.Reconnecting) {
                this._logger.log(LogLevel.Debug, "Connection left the reconnecting state in onreconnecting callback. Done reconnecting.");
                return;
            }
        }
        while (nextRetryDelay !== null) {
            this._logger.log(LogLevel.Information, `Reconnect attempt number ${previousReconnectAttempts} will start in ${nextRetryDelay} ms.`);
            await new Promise((resolve) => {
                this._reconnectDelayHandle = setTimeout(resolve, nextRetryDelay);
            });
            this._reconnectDelayHandle = undefined;
            if (this._connectionState !== HubConnectionState.Reconnecting) {
                this._logger.log(LogLevel.Debug, "Connection left the reconnecting state during reconnect delay. Done reconnecting.");
                return;
            }
            try {
                await this._startInternal();
                this._connectionState = HubConnectionState.Connected;
                this._logger.log(LogLevel.Information, "HubConnection reconnected successfully.");
                if (this._reconnectedCallbacks.length !== 0) {
                    try {
                        this._reconnectedCallbacks.forEach((c) => c.apply(this, [this.connection.connectionId]));
                    }
                    catch (e) {
                        this._logger.log(LogLevel.Error, `An onreconnected callback called with connectionId '${this.connection.connectionId}; threw error '${e}'.`);
                    }
                }
                return;
            }
            catch (e) {
                this._logger.log(LogLevel.Information, `Reconnect attempt failed because of error '${e}'.`);
                if (this._connectionState !== HubConnectionState.Reconnecting) {
                    this._logger.log(LogLevel.Debug, `Connection moved to the '${this._connectionState}' from the reconnecting state during reconnect attempt. Done reconnecting.`);
                    // The TypeScript compiler thinks that connectionState must be Connected here. The TypeScript compiler is wrong.
                    if (this._connectionState === HubConnectionState.Disconnecting) {
                        this._completeClose();
                    }
                    return;
                }
                retryError = e instanceof Error ? e : new Error(e.toString());
                nextRetryDelay = this._getNextRetryDelay(previousReconnectAttempts++, Date.now() - reconnectStartTime, retryError);
            }
        }
        this._logger.log(LogLevel.Information, `Reconnect retries have been exhausted after ${Date.now() - reconnectStartTime} ms and ${previousReconnectAttempts} failed attempts. Connection disconnecting.`);
        this._completeClose();
    }
    _getNextRetryDelay(previousRetryCount, elapsedMilliseconds, retryReason) {
        try {
            return this._reconnectPolicy.nextRetryDelayInMilliseconds({
                elapsedMilliseconds,
                previousRetryCount,
                retryReason,
            });
        }
        catch (e) {
            this._logger.log(LogLevel.Error, `IRetryPolicy.nextRetryDelayInMilliseconds(${previousRetryCount}, ${elapsedMilliseconds}) threw error '${e}'.`);
            return null;
        }
    }
    _cancelCallbacksWithError(error) {
        const callbacks = this._callbacks;
        this._callbacks = {};
        Object.keys(callbacks)
            .forEach((key) => {
            const callback = callbacks[key];
            try {
                callback(null, error);
            }
            catch (e) {
                this._logger.log(LogLevel.Error, `Stream 'error' callback called with '${error}' threw error: ${getErrorString(e)}`);
            }
        });
    }
    _cleanupPingTimer() {
        if (this._pingServerHandle) {
            clearTimeout(this._pingServerHandle);
            this._pingServerHandle = undefined;
        }
    }
    _cleanupTimeout() {
        if (this._timeoutHandle) {
            clearTimeout(this._timeoutHandle);
        }
    }
    _createInvocation(methodName, args, nonblocking, streamIds) {
        if (nonblocking) {
            if (streamIds.length !== 0) {
                return {
                    arguments: args,
                    streamIds,
                    target: methodName,
                    type: MessageType.Invocation,
                };
            }
            else {
                return {
                    arguments: args,
                    target: methodName,
                    type: MessageType.Invocation,
                };
            }
        }
        else {
            const invocationId = this._invocationId;
            this._invocationId++;
            if (streamIds.length !== 0) {
                return {
                    arguments: args,
                    invocationId: invocationId.toString(),
                    streamIds,
                    target: methodName,
                    type: MessageType.Invocation,
                };
            }
            else {
                return {
                    arguments: args,
                    invocationId: invocationId.toString(),
                    target: methodName,
                    type: MessageType.Invocation,
                };
            }
        }
    }
    _launchStreams(streams, promiseQueue) {
        if (streams.length === 0) {
            return;
        }
        // Synchronize stream data so they arrive in-order on the server
        if (!promiseQueue) {
            promiseQueue = Promise.resolve();
        }
        // We want to iterate over the keys, since the keys are the stream ids
        // eslint-disable-next-line guard-for-in
        for (const streamId in streams) {
            streams[streamId].subscribe({
                complete: () => {
                    promiseQueue = promiseQueue.then(() => this._sendWithProtocol(this._createCompletionMessage(streamId)));
                },
                error: (err) => {
                    let message;
                    if (err instanceof Error) {
                        message = err.message;
                    }
                    else if (err && err.toString) {
                        message = err.toString();
                    }
                    else {
                        message = "Unknown error";
                    }
                    promiseQueue = promiseQueue.then(() => this._sendWithProtocol(this._createCompletionMessage(streamId, message)));
                },
                next: (item) => {
                    promiseQueue = promiseQueue.then(() => this._sendWithProtocol(this._createStreamItemMessage(streamId, item)));
                },
            });
        }
    }
    _replaceStreamingParams(args) {
        const streams = [];
        const streamIds = [];
        for (let i = 0; i < args.length; i++) {
            const argument = args[i];
            if (this._isObservable(argument)) {
                const streamId = this._invocationId;
                this._invocationId++;
                // Store the stream for later use
                streams[streamId] = argument;
                streamIds.push(streamId.toString());
                // remove stream from args
                args.splice(i, 1);
            }
        }
        return [streams, streamIds];
    }
    _isObservable(arg) {
        // This allows other stream implementations to just work (like rxjs)
        return arg && arg.subscribe && typeof arg.subscribe === "function";
    }
    _createStreamInvocation(methodName, args, streamIds) {
        const invocationId = this._invocationId;
        this._invocationId++;
        if (streamIds.length !== 0) {
            return {
                arguments: args,
                invocationId: invocationId.toString(),
                streamIds,
                target: methodName,
                type: MessageType.StreamInvocation,
            };
        }
        else {
            return {
                arguments: args,
                invocationId: invocationId.toString(),
                target: methodName,
                type: MessageType.StreamInvocation,
            };
        }
    }
    _createCancelInvocation(id) {
        return {
            invocationId: id,
            type: MessageType.CancelInvocation,
        };
    }
    _createStreamItemMessage(id, item) {
        return {
            invocationId: id,
            item,
            type: MessageType.StreamItem,
        };
    }
    _createCompletionMessage(id, error, result) {
        if (error) {
            return {
                error,
                invocationId: id,
                type: MessageType.Completion,
            };
        }
        return {
            invocationId: id,
            result,
            type: MessageType.Completion,
        };
    }
    _createCloseMessage() {
        return { type: MessageType.Close };
    }
}

;// CONCATENATED MODULE: ./src/DefaultReconnectPolicy.ts
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// 0, 2, 10, 30 second delays before reconnect attempts.
const DEFAULT_RETRY_DELAYS_IN_MILLISECONDS = [0, 2000, 10000, 30000, null];
/** @private */
class DefaultReconnectPolicy {
    constructor(retryDelays) {
        this._retryDelays = retryDelays !== undefined ? [...retryDelays, null] : DEFAULT_RETRY_DELAYS_IN_MILLISECONDS;
    }
    nextRetryDelayInMilliseconds(retryContext) {
        return this._retryDelays[retryContext.previousRetryCount];
    }
}

;// CONCATENATED MODULE: ./src/HeaderNames.ts
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
class HeaderNames {
}
HeaderNames.Authorization = "Authorization";
HeaderNames.Cookie = "Cookie";

;// CONCATENATED MODULE: ./src/AccessTokenHttpClient.ts
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


/** @private */
class AccessTokenHttpClient extends HttpClient {
    constructor(innerClient, accessTokenFactory) {
        super();
        this._innerClient = innerClient;
        this._accessTokenFactory = accessTokenFactory;
    }
    async send(request) {
        let allowRetry = true;
        if (this._accessTokenFactory && (!this._accessToken || (request.url && request.url.indexOf("/negotiate?") > 0))) {
            // don't retry if the request is a negotiate or if we just got a potentially new token from the access token factory
            allowRetry = false;
            this._accessToken = await this._accessTokenFactory();
        }
        this._setAuthorizationHeader(request);
        const response = await this._innerClient.send(request);
        if (allowRetry && response.statusCode === 401 && this._accessTokenFactory) {
            this._accessToken = await this._accessTokenFactory();
            this._setAuthorizationHeader(request);
            return await this._innerClient.send(request);
        }
        return response;
    }
    _setAuthorizationHeader(request) {
        if (!request.headers) {
            request.headers = {};
        }
        if (this._accessToken) {
            request.headers[HeaderNames.Authorization] = `Bearer ${this._accessToken}`;
        }
        // don't remove the header if there isn't an access token factory, the user manually added the header in this case
        else if (this._accessTokenFactory) {
            if (request.headers[HeaderNames.Authorization]) {
                delete request.headers[HeaderNames.Authorization];
            }
        }
    }
    getCookieString(url) {
        return this._innerClient.getCookieString(url);
    }
}

;// CONCATENATED MODULE: ./src/ITransport.ts
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// This will be treated as a bit flag in the future, so we keep it using power-of-two values.
/** Specifies a specific HTTP transport type. */
var HttpTransportType;
(function (HttpTransportType) {
    /** Specifies no transport preference. */
    HttpTransportType[HttpTransportType["None"] = 0] = "None";
    /** Specifies the WebSockets transport. */
    HttpTransportType[HttpTransportType["WebSockets"] = 1] = "WebSockets";
    /** Specifies the Server-Sent Events transport. */
    HttpTransportType[HttpTransportType["ServerSentEvents"] = 2] = "ServerSentEvents";
    /** Specifies the Long Polling transport. */
    HttpTransportType[HttpTransportType["LongPolling"] = 4] = "LongPolling";
})(HttpTransportType || (HttpTransportType = {}));
/** Specifies the transfer format for a connection. */
var TransferFormat;
(function (TransferFormat) {
    /** Specifies that only text data will be transmitted over the connection. */
    TransferFormat[TransferFormat["Text"] = 1] = "Text";
    /** Specifies that binary data will be transmitted over the connection. */
    TransferFormat[TransferFormat["Binary"] = 2] = "Binary";
})(TransferFormat || (TransferFormat = {}));

;// CONCATENATED MODULE: ./src/AbortController.ts
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// Rough polyfill of https://developer.mozilla.org/en-US/docs/Web/API/AbortController
// We don't actually ever use the API being polyfilled, we always use the polyfill because
// it's a very new API right now.
// Not exported from index.
/** @private */
class AbortController_AbortController {
    constructor() {
        this._isAborted = false;
        this.onabort = null;
    }
    abort() {
        if (!this._isAborted) {
            this._isAborted = true;
            if (this.onabort) {
                this.onabort();
            }
        }
    }
    get signal() {
        return this;
    }
    get aborted() {
        return this._isAborted;
    }
}

;// CONCATENATED MODULE: ./src/LongPollingTransport.ts
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.





// Not exported from 'index', this type is internal.
/** @private */
class LongPollingTransport {
    // This is an internal type, not exported from 'index' so this is really just internal.
    get pollAborted() {
        return this._pollAbort.aborted;
    }
    constructor(httpClient, logger, options) {
        this._httpClient = httpClient;
        this._logger = logger;
        this._pollAbort = new AbortController_AbortController();
        this._options = options;
        this._running = false;
        this.onreceive = null;
        this.onclose = null;
    }
    async connect(url, transferFormat) {
        Arg.isRequired(url, "url");
        Arg.isRequired(transferFormat, "transferFormat");
        Arg.isIn(transferFormat, TransferFormat, "transferFormat");
        this._url = url;
        this._logger.log(LogLevel.Trace, "(LongPolling transport) Connecting.");
        // Allow binary format on Node and Browsers that support binary content (indicated by the presence of responseType property)
        if (transferFormat === TransferFormat.Binary &&
            (typeof XMLHttpRequest !== "undefined" && typeof new XMLHttpRequest().responseType !== "string")) {
            throw new Error("Binary protocols over XmlHttpRequest not implementing advanced features are not supported.");
        }
        const [name, value] = getUserAgentHeader();
        const headers = { [name]: value, ...this._options.headers };
        const pollOptions = {
            abortSignal: this._pollAbort.signal,
            headers,
            timeout: 100000,
            withCredentials: this._options.withCredentials,
        };
        if (transferFormat === TransferFormat.Binary) {
            pollOptions.responseType = "arraybuffer";
        }
        // Make initial long polling request
        // Server uses first long polling request to finish initializing connection and it returns without data
        const pollUrl = `${url}&_=${Date.now()}`;
        this._logger.log(LogLevel.Trace, `(LongPolling transport) polling: ${pollUrl}.`);
        const response = await this._httpClient.get(pollUrl, pollOptions);
        if (response.statusCode !== 200) {
            this._logger.log(LogLevel.Error, `(LongPolling transport) Unexpected response code: ${response.statusCode}.`);
            // Mark running as false so that the poll immediately ends and runs the close logic
            this._closeError = new HttpError(response.statusText || "", response.statusCode);
            this._running = false;
        }
        else {
            this._running = true;
        }
        this._receiving = this._poll(this._url, pollOptions);
    }
    async _poll(url, pollOptions) {
        try {
            while (this._running) {
                try {
                    const pollUrl = `${url}&_=${Date.now()}`;
                    this._logger.log(LogLevel.Trace, `(LongPolling transport) polling: ${pollUrl}.`);
                    const response = await this._httpClient.get(pollUrl, pollOptions);
                    if (response.statusCode === 204) {
                        this._logger.log(LogLevel.Information, "(LongPolling transport) Poll terminated by server.");
                        this._running = false;
                    }
                    else if (response.statusCode !== 200) {
                        this._logger.log(LogLevel.Error, `(LongPolling transport) Unexpected response code: ${response.statusCode}.`);
                        // Unexpected status code
                        this._closeError = new HttpError(response.statusText || "", response.statusCode);
                        this._running = false;
                    }
                    else {
                        // Process the response
                        if (response.content) {
                            this._logger.log(LogLevel.Trace, `(LongPolling transport) data received. ${getDataDetail(response.content, this._options.logMessageContent)}.`);
                            if (this.onreceive) {
                                this.onreceive(response.content);
                            }
                        }
                        else {
                            // This is another way timeout manifest.
                            this._logger.log(LogLevel.Trace, "(LongPolling transport) Poll timed out, reissuing.");
                        }
                    }
                }
                catch (e) {
                    if (!this._running) {
                        // Log but disregard errors that occur after stopping
                        this._logger.log(LogLevel.Trace, `(LongPolling transport) Poll errored after shutdown: ${e.message}`);
                    }
                    else {
                        if (e instanceof TimeoutError) {
                            // Ignore timeouts and reissue the poll.
                            this._logger.log(LogLevel.Trace, "(LongPolling transport) Poll timed out, reissuing.");
                        }
                        else {
                            // Close the connection with the error as the result.
                            this._closeError = e;
                            this._running = false;
                        }
                    }
                }
            }
        }
        finally {
            this._logger.log(LogLevel.Trace, "(LongPolling transport) Polling complete.");
            // We will reach here with pollAborted==false when the server returned a response causing the transport to stop.
            // If pollAborted==true then client initiated the stop and the stop method will raise the close event after DELETE is sent.
            if (!this.pollAborted) {
                this._raiseOnClose();
            }
        }
    }
    async send(data) {
        if (!this._running) {
            return Promise.reject(new Error("Cannot send until the transport is connected"));
        }
        return sendMessage(this._logger, "LongPolling", this._httpClient, this._url, data, this._options);
    }
    async stop() {
        this._logger.log(LogLevel.Trace, "(LongPolling transport) Stopping polling.");
        // Tell receiving loop to stop, abort any current request, and then wait for it to finish
        this._running = false;
        this._pollAbort.abort();
        try {
            await this._receiving;
            // Send DELETE to clean up long polling on the server
            this._logger.log(LogLevel.Trace, `(LongPolling transport) sending DELETE request to ${this._url}.`);
            const headers = {};
            const [name, value] = getUserAgentHeader();
            headers[name] = value;
            const deleteOptions = {
                headers: { ...headers, ...this._options.headers },
                timeout: this._options.timeout,
                withCredentials: this._options.withCredentials,
            };
            let error;
            try {
                await this._httpClient.delete(this._url, deleteOptions);
            }
            catch (err) {
                error = err;
            }
            if (error) {
                if (error instanceof HttpError) {
                    if (error.statusCode === 404) {
                        this._logger.log(LogLevel.Trace, "(LongPolling transport) A 404 response was returned from sending a DELETE request.");
                    }
                    else {
                        this._logger.log(LogLevel.Trace, `(LongPolling transport) Error sending a DELETE request: ${error}`);
                    }
                }
            }
            else {
                this._logger.log(LogLevel.Trace, "(LongPolling transport) DELETE request accepted.");
            }
        }
        finally {
            this._logger.log(LogLevel.Trace, "(LongPolling transport) Stop finished.");
            // Raise close event here instead of in polling
            // It needs to happen after the DELETE request is sent
            this._raiseOnClose();
        }
    }
    _raiseOnClose() {
        if (this.onclose) {
            let logMessage = "(LongPolling transport) Firing onclose event.";
            if (this._closeError) {
                logMessage += " Error: " + this._closeError;
            }
            this._logger.log(LogLevel.Trace, logMessage);
            this.onclose(this._closeError);
        }
    }
}

;// CONCATENATED MODULE: ./src/ServerSentEventsTransport.ts
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.



/** @private */
class ServerSentEventsTransport {
    constructor(httpClient, accessToken, logger, options) {
        this._httpClient = httpClient;
        this._accessToken = accessToken;
        this._logger = logger;
        this._options = options;
        this.onreceive = null;
        this.onclose = null;
    }
    async connect(url, transferFormat) {
        Arg.isRequired(url, "url");
        Arg.isRequired(transferFormat, "transferFormat");
        Arg.isIn(transferFormat, TransferFormat, "transferFormat");
        this._logger.log(LogLevel.Trace, "(SSE transport) Connecting.");
        // set url before accessTokenFactory because this._url is only for send and we set the auth header instead of the query string for send
        this._url = url;
        if (this._accessToken) {
            url += (url.indexOf("?") < 0 ? "?" : "&") + `access_token=${encodeURIComponent(this._accessToken)}`;
        }
        return new Promise((resolve, reject) => {
            let opened = false;
            if (transferFormat !== TransferFormat.Text) {
                reject(new Error("The Server-Sent Events transport only supports the 'Text' transfer format"));
                return;
            }
            let eventSource;
            if (Platform.isBrowser || Platform.isWebWorker) {
                eventSource = new this._options.EventSource(url, { withCredentials: this._options.withCredentials });
            }
            else {
                // Non-browser passes cookies via the dictionary
                const cookies = this._httpClient.getCookieString(url);
                const headers = {};
                headers.Cookie = cookies;
                const [name, value] = getUserAgentHeader();
                headers[name] = value;
                eventSource = new this._options.EventSource(url, { withCredentials: this._options.withCredentials, headers: { ...headers, ...this._options.headers } });
            }
            try {
                eventSource.onmessage = (e) => {
                    if (this.onreceive) {
                        try {
                            this._logger.log(LogLevel.Trace, `(SSE transport) data received. ${getDataDetail(e.data, this._options.logMessageContent)}.`);
                            this.onreceive(e.data);
                        }
                        catch (error) {
                            this._close(error);
                            return;
                        }
                    }
                };
                // @ts-ignore: not using event on purpose
                eventSource.onerror = (e) => {
                    // EventSource doesn't give any useful information about server side closes.
                    if (opened) {
                        this._close();
                    }
                    else {
                        reject(new Error("EventSource failed to connect. The connection could not be found on the server,"
                            + " either the connection ID is not present on the server, or a proxy is refusing/buffering the connection."
                            + " If you have multiple servers check that sticky sessions are enabled."));
                    }
                };
                eventSource.onopen = () => {
                    this._logger.log(LogLevel.Information, `SSE connected to ${this._url}`);
                    this._eventSource = eventSource;
                    opened = true;
                    resolve();
                };
            }
            catch (e) {
                reject(e);
                return;
            }
        });
    }
    async send(data) {
        if (!this._eventSource) {
            return Promise.reject(new Error("Cannot send until the transport is connected"));
        }
        return sendMessage(this._logger, "SSE", this._httpClient, this._url, data, this._options);
    }
    stop() {
        this._close();
        return Promise.resolve();
    }
    _close(e) {
        if (this._eventSource) {
            this._eventSource.close();
            this._eventSource = undefined;
            if (this.onclose) {
                this.onclose(e);
            }
        }
    }
}

;// CONCATENATED MODULE: ./src/WebSocketTransport.ts
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.




/** @private */
class WebSocketTransport {
    constructor(httpClient, accessTokenFactory, logger, logMessageContent, webSocketConstructor, headers) {
        this._logger = logger;
        this._accessTokenFactory = accessTokenFactory;
        this._logMessageContent = logMessageContent;
        this._webSocketConstructor = webSocketConstructor;
        this._httpClient = httpClient;
        this.onreceive = null;
        this.onclose = null;
        this._headers = headers;
    }
    async connect(url, transferFormat) {
        Arg.isRequired(url, "url");
        Arg.isRequired(transferFormat, "transferFormat");
        Arg.isIn(transferFormat, TransferFormat, "transferFormat");
        this._logger.log(LogLevel.Trace, "(WebSockets transport) Connecting.");
        let token;
        if (this._accessTokenFactory) {
            token = await this._accessTokenFactory();
        }
        return new Promise((resolve, reject) => {
            url = url.replace(/^http/, "ws");
            let webSocket;
            const cookies = this._httpClient.getCookieString(url);
            let opened = false;
            if (Platform.isNode || Platform.isReactNative) {
                const headers = {};
                const [name, value] = getUserAgentHeader();
                headers[name] = value;
                if (token) {
                    headers[HeaderNames.Authorization] = `Bearer ${token}`;
                }
                if (cookies) {
                    headers[HeaderNames.Cookie] = cookies;
                }
                // Only pass headers when in non-browser environments
                webSocket = new this._webSocketConstructor(url, undefined, {
                    headers: { ...headers, ...this._headers },
                });
            }
            else {
                if (token) {
                    url += (url.indexOf("?") < 0 ? "?" : "&") + `access_token=${encodeURIComponent(token)}`;
                }
            }
            if (!webSocket) {
                // Chrome is not happy with passing 'undefined' as protocol
                webSocket = new this._webSocketConstructor(url);
            }
            if (transferFormat === TransferFormat.Binary) {
                webSocket.binaryType = "arraybuffer";
            }
            webSocket.onopen = (_event) => {
                this._logger.log(LogLevel.Information, `WebSocket connected to ${url}.`);
                this._webSocket = webSocket;
                opened = true;
                resolve();
            };
            webSocket.onerror = (event) => {
                let error = null;
                // ErrorEvent is a browser only type we need to check if the type exists before using it
                if (typeof ErrorEvent !== "undefined" && event instanceof ErrorEvent) {
                    error = event.error;
                }
                else {
                    error = "There was an error with the transport";
                }
                this._logger.log(LogLevel.Information, `(WebSockets transport) ${error}.`);
            };
            webSocket.onmessage = (message) => {
                this._logger.log(LogLevel.Trace, `(WebSockets transport) data received. ${getDataDetail(message.data, this._logMessageContent)}.`);
                if (this.onreceive) {
                    try {
                        this.onreceive(message.data);
                    }
                    catch (error) {
                        this._close(error);
                        return;
                    }
                }
            };
            webSocket.onclose = (event) => {
                // Don't call close handler if connection was never established
                // We'll reject the connect call instead
                if (opened) {
                    this._close(event);
                }
                else {
                    let error = null;
                    // ErrorEvent is a browser only type we need to check if the type exists before using it
                    if (typeof ErrorEvent !== "undefined" && event instanceof ErrorEvent) {
                        error = event.error;
                    }
                    else {
                        error = "WebSocket failed to connect. The connection could not be found on the server,"
                            + " either the endpoint may not be a SignalR endpoint,"
                            + " the connection ID is not present on the server, or there is a proxy blocking WebSockets."
                            + " If you have multiple servers check that sticky sessions are enabled.";
                    }
                    reject(new Error(error));
                }
            };
        });
    }
    send(data) {
        if (this._webSocket && this._webSocket.readyState === this._webSocketConstructor.OPEN) {
            this._logger.log(LogLevel.Trace, `(WebSockets transport) sending data. ${getDataDetail(data, this._logMessageContent)}.`);
            this._webSocket.send(data);
            return Promise.resolve();
        }
        return Promise.reject("WebSocket is not in the OPEN state");
    }
    stop() {
        if (this._webSocket) {
            // Manually invoke onclose callback inline so we know the HttpConnection was closed properly before returning
            // This also solves an issue where websocket.onclose could take 18+ seconds to trigger during network disconnects
            this._close(undefined);
        }
        return Promise.resolve();
    }
    _close(event) {
        // webSocket will be null if the transport did not start successfully
        if (this._webSocket) {
            // Clear websocket handlers because we are considering the socket closed now
            this._webSocket.onclose = () => { };
            this._webSocket.onmessage = () => { };
            this._webSocket.onerror = () => { };
            this._webSocket.close();
            this._webSocket = undefined;
        }
        this._logger.log(LogLevel.Trace, "(WebSockets transport) socket closed.");
        if (this.onclose) {
            if (this._isCloseEvent(event) && (event.wasClean === false || event.code !== 1000)) {
                this.onclose(new Error(`WebSocket closed with status code: ${event.code} (${event.reason || "no reason given"}).`));
            }
            else if (event instanceof Error) {
                this.onclose(event);
            }
            else {
                this.onclose();
            }
        }
    }
    _isCloseEvent(event) {
        return event && typeof event.wasClean === "boolean" && typeof event.code === "number";
    }
}

;// CONCATENATED MODULE: ./src/HttpConnection.ts
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.









const MAX_REDIRECTS = 100;
/** @private */
class HttpConnection {
    constructor(url, options = {}) {
        this._stopPromiseResolver = () => { };
        this.features = {};
        this._negotiateVersion = 1;
        Arg.isRequired(url, "url");
        this._logger = createLogger(options.logger);
        this.baseUrl = this._resolveUrl(url);
        options = options || {};
        options.logMessageContent = options.logMessageContent === undefined ? false : options.logMessageContent;
        if (typeof options.withCredentials === "boolean" || options.withCredentials === undefined) {
            options.withCredentials = options.withCredentials === undefined ? true : options.withCredentials;
        }
        else {
            throw new Error("withCredentials option was not a 'boolean' or 'undefined' value");
        }
        options.timeout = options.timeout === undefined ? 100 * 1000 : options.timeout;
        let webSocketModule = null;
        let eventSourceModule = null;
        if (Platform.isNode && "function" !== "undefined") {
            // In order to ignore the dynamic require in webpack builds we need to do this magic
            // @ts-ignore: TS doesn't know about these names
            const requireFunc =  true ? require : 0;
            webSocketModule = requireFunc("ws");
            eventSourceModule = requireFunc("eventsource");
        }
        if (!Platform.isNode && typeof WebSocket !== "undefined" && !options.WebSocket) {
            options.WebSocket = WebSocket;
        }
        else if (Platform.isNode && !options.WebSocket) {
            if (webSocketModule) {
                options.WebSocket = webSocketModule;
            }
        }
        if (!Platform.isNode && typeof EventSource !== "undefined" && !options.EventSource) {
            options.EventSource = EventSource;
        }
        else if (Platform.isNode && !options.EventSource) {
            if (typeof eventSourceModule !== "undefined") {
                options.EventSource = eventSourceModule;
            }
        }
        this._httpClient = new AccessTokenHttpClient(options.httpClient || new DefaultHttpClient(this._logger), options.accessTokenFactory);
        this._connectionState = "Disconnected" /* ConnectionState.Disconnected */;
        this._connectionStarted = false;
        this._options = options;
        this.onreceive = null;
        this.onclose = null;
    }
    async start(transferFormat) {
        transferFormat = transferFormat || TransferFormat.Binary;
        Arg.isIn(transferFormat, TransferFormat, "transferFormat");
        this._logger.log(LogLevel.Debug, `Starting connection with transfer format '${TransferFormat[transferFormat]}'.`);
        if (this._connectionState !== "Disconnected" /* ConnectionState.Disconnected */) {
            return Promise.reject(new Error("Cannot start an HttpConnection that is not in the 'Disconnected' state."));
        }
        this._connectionState = "Connecting" /* ConnectionState.Connecting */;
        this._startInternalPromise = this._startInternal(transferFormat);
        await this._startInternalPromise;
        // The TypeScript compiler thinks that connectionState must be Connecting here. The TypeScript compiler is wrong.
        if (this._connectionState === "Disconnecting" /* ConnectionState.Disconnecting */) {
            // stop() was called and transitioned the client into the Disconnecting state.
            const message = "Failed to start the HttpConnection before stop() was called.";
            this._logger.log(LogLevel.Error, message);
            // We cannot await stopPromise inside startInternal since stopInternal awaits the startInternalPromise.
            await this._stopPromise;
            return Promise.reject(new AbortError(message));
        }
        else if (this._connectionState !== "Connected" /* ConnectionState.Connected */) {
            // stop() was called and transitioned the client into the Disconnecting state.
            const message = "HttpConnection.startInternal completed gracefully but didn't enter the connection into the connected state!";
            this._logger.log(LogLevel.Error, message);
            return Promise.reject(new AbortError(message));
        }
        this._connectionStarted = true;
    }
    send(data) {
        if (this._connectionState !== "Connected" /* ConnectionState.Connected */) {
            return Promise.reject(new Error("Cannot send data if the connection is not in the 'Connected' State."));
        }
        if (!this._sendQueue) {
            this._sendQueue = new TransportSendQueue(this.transport);
        }
        // Transport will not be null if state is connected
        return this._sendQueue.send(data);
    }
    async stop(error) {
        if (this._connectionState === "Disconnected" /* ConnectionState.Disconnected */) {
            this._logger.log(LogLevel.Debug, `Call to HttpConnection.stop(${error}) ignored because the connection is already in the disconnected state.`);
            return Promise.resolve();
        }
        if (this._connectionState === "Disconnecting" /* ConnectionState.Disconnecting */) {
            this._logger.log(LogLevel.Debug, `Call to HttpConnection.stop(${error}) ignored because the connection is already in the disconnecting state.`);
            return this._stopPromise;
        }
        this._connectionState = "Disconnecting" /* ConnectionState.Disconnecting */;
        this._stopPromise = new Promise((resolve) => {
            // Don't complete stop() until stopConnection() completes.
            this._stopPromiseResolver = resolve;
        });
        // stopInternal should never throw so just observe it.
        await this._stopInternal(error);
        await this._stopPromise;
    }
    async _stopInternal(error) {
        // Set error as soon as possible otherwise there is a race between
        // the transport closing and providing an error and the error from a close message
        // We would prefer the close message error.
        this._stopError = error;
        try {
            await this._startInternalPromise;
        }
        catch (e) {
            // This exception is returned to the user as a rejected Promise from the start method.
        }
        // The transport's onclose will trigger stopConnection which will run our onclose event.
        // The transport should always be set if currently connected. If it wasn't set, it's likely because
        // stop was called during start() and start() failed.
        if (this.transport) {
            try {
                await this.transport.stop();
            }
            catch (e) {
                this._logger.log(LogLevel.Error, `HttpConnection.transport.stop() threw error '${e}'.`);
                this._stopConnection();
            }
            this.transport = undefined;
        }
        else {
            this._logger.log(LogLevel.Debug, "HttpConnection.transport is undefined in HttpConnection.stop() because start() failed.");
        }
    }
    async _startInternal(transferFormat) {
        // Store the original base url and the access token factory since they may change
        // as part of negotiating
        let url = this.baseUrl;
        this._accessTokenFactory = this._options.accessTokenFactory;
        this._httpClient._accessTokenFactory = this._accessTokenFactory;
        try {
            if (this._options.skipNegotiation) {
                if (this._options.transport === HttpTransportType.WebSockets) {
                    // No need to add a connection ID in this case
                    this.transport = this._constructTransport(HttpTransportType.WebSockets);
                    // We should just call connect directly in this case.
                    // No fallback or negotiate in this case.
                    await this._startTransport(url, transferFormat);
                }
                else {
                    throw new Error("Negotiation can only be skipped when using the WebSocket transport directly.");
                }
            }
            else {
                let negotiateResponse = null;
                let redirects = 0;
                do {
                    negotiateResponse = await this._getNegotiationResponse(url);
                    // the user tries to stop the connection when it is being started
                    if (this._connectionState === "Disconnecting" /* ConnectionState.Disconnecting */ || this._connectionState === "Disconnected" /* ConnectionState.Disconnected */) {
                        throw new AbortError("The connection was stopped during negotiation.");
                    }
                    if (negotiateResponse.error) {
                        throw new Error(negotiateResponse.error);
                    }
                    if (negotiateResponse.ProtocolVersion) {
                        throw new Error("Detected a connection attempt to an ASP.NET SignalR Server. This client only supports connecting to an ASP.NET Core SignalR Server. See https://aka.ms/signalr-core-differences for details.");
                    }
                    if (negotiateResponse.url) {
                        url = negotiateResponse.url;
                    }
                    if (negotiateResponse.accessToken) {
                        // Replace the current access token factory with one that uses
                        // the returned access token
                        const accessToken = negotiateResponse.accessToken;
                        this._accessTokenFactory = () => accessToken;
                        // set the factory to undefined so the AccessTokenHttpClient won't retry with the same token, since we know it won't change until a connection restart
                        this._httpClient._accessToken = accessToken;
                        this._httpClient._accessTokenFactory = undefined;
                    }
                    redirects++;
                } while (negotiateResponse.url && redirects < MAX_REDIRECTS);
                if (redirects === MAX_REDIRECTS && negotiateResponse.url) {
                    throw new Error("Negotiate redirection limit exceeded.");
                }
                await this._createTransport(url, this._options.transport, negotiateResponse, transferFormat);
            }
            if (this.transport instanceof LongPollingTransport) {
                this.features.inherentKeepAlive = true;
            }
            if (this._connectionState === "Connecting" /* ConnectionState.Connecting */) {
                // Ensure the connection transitions to the connected state prior to completing this.startInternalPromise.
                // start() will handle the case when stop was called and startInternal exits still in the disconnecting state.
                this._logger.log(LogLevel.Debug, "The HttpConnection connected successfully.");
                this._connectionState = "Connected" /* ConnectionState.Connected */;
            }
            // stop() is waiting on us via this.startInternalPromise so keep this.transport around so it can clean up.
            // This is the only case startInternal can exit in neither the connected nor disconnected state because stopConnection()
            // will transition to the disconnected state. start() will wait for the transition using the stopPromise.
        }
        catch (e) {
            this._logger.log(LogLevel.Error, "Failed to start the connection: " + e);
            this._connectionState = "Disconnected" /* ConnectionState.Disconnected */;
            this.transport = undefined;
            // if start fails, any active calls to stop assume that start will complete the stop promise
            this._stopPromiseResolver();
            return Promise.reject(e);
        }
    }
    async _getNegotiationResponse(url) {
        const headers = {};
        const [name, value] = getUserAgentHeader();
        headers[name] = value;
        const negotiateUrl = this._resolveNegotiateUrl(url);
        this._logger.log(LogLevel.Debug, `Sending negotiation request: ${negotiateUrl}.`);
        try {
            const response = await this._httpClient.post(negotiateUrl, {
                content: "",
                headers: { ...headers, ...this._options.headers },
                timeout: this._options.timeout,
                withCredentials: this._options.withCredentials,
            });
            if (response.statusCode !== 200) {
                return Promise.reject(new Error(`Unexpected status code returned from negotiate '${response.statusCode}'`));
            }
            const negotiateResponse = JSON.parse(response.content);
            if (!negotiateResponse.negotiateVersion || negotiateResponse.negotiateVersion < 1) {
                // Negotiate version 0 doesn't use connectionToken
                // So we set it equal to connectionId so all our logic can use connectionToken without being aware of the negotiate version
                negotiateResponse.connectionToken = negotiateResponse.connectionId;
            }
            if (negotiateResponse.useStatefulReconnect && this._options._useStatefulReconnect !== true) {
                return Promise.reject(new FailedToNegotiateWithServerError("Client didn't negotiate Stateful Reconnect but the server did."));
            }
            return negotiateResponse;
        }
        catch (e) {
            let errorMessage = "Failed to complete negotiation with the server: " + e;
            if (e instanceof HttpError) {
                if (e.statusCode === 404) {
                    errorMessage = errorMessage + " Either this is not a SignalR endpoint or there is a proxy blocking the connection.";
                }
            }
            this._logger.log(LogLevel.Error, errorMessage);
            return Promise.reject(new FailedToNegotiateWithServerError(errorMessage));
        }
    }
    _createConnectUrl(url, connectionToken) {
        if (!connectionToken) {
            return url;
        }
        return url + (url.indexOf("?") === -1 ? "?" : "&") + `id=${connectionToken}`;
    }
    async _createTransport(url, requestedTransport, negotiateResponse, requestedTransferFormat) {
        let connectUrl = this._createConnectUrl(url, negotiateResponse.connectionToken);
        if (this._isITransport(requestedTransport)) {
            this._logger.log(LogLevel.Debug, "Connection was provided an instance of ITransport, using that directly.");
            this.transport = requestedTransport;
            await this._startTransport(connectUrl, requestedTransferFormat);
            this.connectionId = negotiateResponse.connectionId;
            return;
        }
        const transportExceptions = [];
        const transports = negotiateResponse.availableTransports || [];
        let negotiate = negotiateResponse;
        for (const endpoint of transports) {
            const transportOrError = this._resolveTransportOrError(endpoint, requestedTransport, requestedTransferFormat, (negotiate === null || negotiate === void 0 ? void 0 : negotiate.useStatefulReconnect) === true);
            if (transportOrError instanceof Error) {
                // Store the error and continue, we don't want to cause a re-negotiate in these cases
                transportExceptions.push(`${endpoint.transport} failed:`);
                transportExceptions.push(transportOrError);
            }
            else if (this._isITransport(transportOrError)) {
                this.transport = transportOrError;
                if (!negotiate) {
                    try {
                        negotiate = await this._getNegotiationResponse(url);
                    }
                    catch (ex) {
                        return Promise.reject(ex);
                    }
                    connectUrl = this._createConnectUrl(url, negotiate.connectionToken);
                }
                try {
                    await this._startTransport(connectUrl, requestedTransferFormat);
                    this.connectionId = negotiate.connectionId;
                    return;
                }
                catch (ex) {
                    this._logger.log(LogLevel.Error, `Failed to start the transport '${endpoint.transport}': ${ex}`);
                    negotiate = undefined;
                    transportExceptions.push(new FailedToStartTransportError(`${endpoint.transport} failed: ${ex}`, HttpTransportType[endpoint.transport]));
                    if (this._connectionState !== "Connecting" /* ConnectionState.Connecting */) {
                        const message = "Failed to select transport before stop() was called.";
                        this._logger.log(LogLevel.Debug, message);
                        return Promise.reject(new AbortError(message));
                    }
                }
            }
        }
        if (transportExceptions.length > 0) {
            return Promise.reject(new AggregateErrors(`Unable to connect to the server with any of the available transports. ${transportExceptions.join(" ")}`, transportExceptions));
        }
        return Promise.reject(new Error("None of the transports supported by the client are supported by the server."));
    }
    _constructTransport(transport) {
        switch (transport) {
            case HttpTransportType.WebSockets:
                if (!this._options.WebSocket) {
                    throw new Error("'WebSocket' is not supported in your environment.");
                }
                return new WebSocketTransport(this._httpClient, this._accessTokenFactory, this._logger, this._options.logMessageContent, this._options.WebSocket, this._options.headers || {});
            case HttpTransportType.ServerSentEvents:
                if (!this._options.EventSource) {
                    throw new Error("'EventSource' is not supported in your environment.");
                }
                return new ServerSentEventsTransport(this._httpClient, this._httpClient._accessToken, this._logger, this._options);
            case HttpTransportType.LongPolling:
                return new LongPollingTransport(this._httpClient, this._logger, this._options);
            default:
                throw new Error(`Unknown transport: ${transport}.`);
        }
    }
    _startTransport(url, transferFormat) {
        this.transport.onreceive = this.onreceive;
        if (this.features.reconnect) {
            this.transport.onclose = async (e) => {
                let callStop = false;
                if (this.features.reconnect) {
                    try {
                        this.features.disconnected();
                        await this.transport.connect(url, transferFormat);
                        await this.features.resend();
                    }
                    catch {
                        callStop = true;
                    }
                }
                else {
                    this._stopConnection(e);
                    return;
                }
                if (callStop) {
                    this._stopConnection(e);
                }
            };
        }
        else {
            this.transport.onclose = (e) => this._stopConnection(e);
        }
        return this.transport.connect(url, transferFormat);
    }
    _resolveTransportOrError(endpoint, requestedTransport, requestedTransferFormat, useStatefulReconnect) {
        const transport = HttpTransportType[endpoint.transport];
        if (transport === null || transport === undefined) {
            this._logger.log(LogLevel.Debug, `Skipping transport '${endpoint.transport}' because it is not supported by this client.`);
            return new Error(`Skipping transport '${endpoint.transport}' because it is not supported by this client.`);
        }
        else {
            if (transportMatches(requestedTransport, transport)) {
                const transferFormats = endpoint.transferFormats.map((s) => TransferFormat[s]);
                if (transferFormats.indexOf(requestedTransferFormat) >= 0) {
                    if ((transport === HttpTransportType.WebSockets && !this._options.WebSocket) ||
                        (transport === HttpTransportType.ServerSentEvents && !this._options.EventSource)) {
                        this._logger.log(LogLevel.Debug, `Skipping transport '${HttpTransportType[transport]}' because it is not supported in your environment.'`);
                        return new UnsupportedTransportError(`'${HttpTransportType[transport]}' is not supported in your environment.`, transport);
                    }
                    else {
                        this._logger.log(LogLevel.Debug, `Selecting transport '${HttpTransportType[transport]}'.`);
                        try {
                            this.features.reconnect = transport === HttpTransportType.WebSockets ? useStatefulReconnect : undefined;
                            return this._constructTransport(transport);
                        }
                        catch (ex) {
                            return ex;
                        }
                    }
                }
                else {
                    this._logger.log(LogLevel.Debug, `Skipping transport '${HttpTransportType[transport]}' because it does not support the requested transfer format '${TransferFormat[requestedTransferFormat]}'.`);
                    return new Error(`'${HttpTransportType[transport]}' does not support ${TransferFormat[requestedTransferFormat]}.`);
                }
            }
            else {
                this._logger.log(LogLevel.Debug, `Skipping transport '${HttpTransportType[transport]}' because it was disabled by the client.`);
                return new DisabledTransportError(`'${HttpTransportType[transport]}' is disabled by the client.`, transport);
            }
        }
    }
    _isITransport(transport) {
        return transport && typeof (transport) === "object" && "connect" in transport;
    }
    _stopConnection(error) {
        this._logger.log(LogLevel.Debug, `HttpConnection.stopConnection(${error}) called while in state ${this._connectionState}.`);
        this.transport = undefined;
        // If we have a stopError, it takes precedence over the error from the transport
        error = this._stopError || error;
        this._stopError = undefined;
        if (this._connectionState === "Disconnected" /* ConnectionState.Disconnected */) {
            this._logger.log(LogLevel.Debug, `Call to HttpConnection.stopConnection(${error}) was ignored because the connection is already in the disconnected state.`);
            return;
        }
        if (this._connectionState === "Connecting" /* ConnectionState.Connecting */) {
            this._logger.log(LogLevel.Warning, `Call to HttpConnection.stopConnection(${error}) was ignored because the connection is still in the connecting state.`);
            throw new Error(`HttpConnection.stopConnection(${error}) was called while the connection is still in the connecting state.`);
        }
        if (this._connectionState === "Disconnecting" /* ConnectionState.Disconnecting */) {
            // A call to stop() induced this call to stopConnection and needs to be completed.
            // Any stop() awaiters will be scheduled to continue after the onclose callback fires.
            this._stopPromiseResolver();
        }
        if (error) {
            this._logger.log(LogLevel.Error, `Connection disconnected with error '${error}'.`);
        }
        else {
            this._logger.log(LogLevel.Information, "Connection disconnected.");
        }
        if (this._sendQueue) {
            this._sendQueue.stop().catch((e) => {
                this._logger.log(LogLevel.Error, `TransportSendQueue.stop() threw error '${e}'.`);
            });
            this._sendQueue = undefined;
        }
        this.connectionId = undefined;
        this._connectionState = "Disconnected" /* ConnectionState.Disconnected */;
        if (this._connectionStarted) {
            this._connectionStarted = false;
            try {
                if (this.onclose) {
                    this.onclose(error);
                }
            }
            catch (e) {
                this._logger.log(LogLevel.Error, `HttpConnection.onclose(${error}) threw error '${e}'.`);
            }
        }
    }
    _resolveUrl(url) {
        // startsWith is not supported in IE
        if (url.lastIndexOf("https://", 0) === 0 || url.lastIndexOf("http://", 0) === 0) {
            return url;
        }
        if (!Platform.isBrowser) {
            throw new Error(`Cannot resolve '${url}'.`);
        }
        // Setting the url to the href propery of an anchor tag handles normalization
        // for us. There are 3 main cases.
        // 1. Relative path normalization e.g "b" -> "http://localhost:5000/a/b"
        // 2. Absolute path normalization e.g "/a/b" -> "http://localhost:5000/a/b"
        // 3. Networkpath reference normalization e.g "//localhost:5000/a/b" -> "http://localhost:5000/a/b"
        const aTag = window.document.createElement("a");
        aTag.href = url;
        this._logger.log(LogLevel.Information, `Normalizing '${url}' to '${aTag.href}'.`);
        return aTag.href;
    }
    _resolveNegotiateUrl(url) {
        const negotiateUrl = new URL(url);
        if (negotiateUrl.pathname.endsWith('/')) {
            negotiateUrl.pathname += "negotiate";
        }
        else {
            negotiateUrl.pathname += "/negotiate";
        }
        const searchParams = new URLSearchParams(negotiateUrl.searchParams);
        if (!searchParams.has("negotiateVersion")) {
            searchParams.append("negotiateVersion", this._negotiateVersion.toString());
        }
        if (searchParams.has("useStatefulReconnect")) {
            if (searchParams.get("useStatefulReconnect") === "true") {
                this._options._useStatefulReconnect = true;
            }
        }
        else if (this._options._useStatefulReconnect === true) {
            searchParams.append("useStatefulReconnect", "true");
        }
        negotiateUrl.search = searchParams.toString();
        return negotiateUrl.toString();
    }
}
function transportMatches(requestedTransport, actualTransport) {
    return !requestedTransport || ((actualTransport & requestedTransport) !== 0);
}
/** @private */
class TransportSendQueue {
    constructor(_transport) {
        this._transport = _transport;
        this._buffer = [];
        this._executing = true;
        this._sendBufferedData = new PromiseSource();
        this._transportResult = new PromiseSource();
        this._sendLoopPromise = this._sendLoop();
    }
    send(data) {
        this._bufferData(data);
        if (!this._transportResult) {
            this._transportResult = new PromiseSource();
        }
        return this._transportResult.promise;
    }
    stop() {
        this._executing = false;
        this._sendBufferedData.resolve();
        return this._sendLoopPromise;
    }
    _bufferData(data) {
        if (this._buffer.length && typeof (this._buffer[0]) !== typeof (data)) {
            throw new Error(`Expected data to be of type ${typeof (this._buffer)} but was of type ${typeof (data)}`);
        }
        this._buffer.push(data);
        this._sendBufferedData.resolve();
    }
    async _sendLoop() {
        while (true) {
            await this._sendBufferedData.promise;
            if (!this._executing) {
                if (this._transportResult) {
                    this._transportResult.reject("Connection stopped.");
                }
                break;
            }
            this._sendBufferedData = new PromiseSource();
            const transportResult = this._transportResult;
            this._transportResult = undefined;
            const data = typeof (this._buffer[0]) === "string" ?
                this._buffer.join("") :
                TransportSendQueue._concatBuffers(this._buffer);
            this._buffer.length = 0;
            try {
                await this._transport.send(data);
                transportResult.resolve();
            }
            catch (error) {
                transportResult.reject(error);
            }
        }
    }
    static _concatBuffers(arrayBuffers) {
        const totalLength = arrayBuffers.map((b) => b.byteLength).reduce((a, b) => a + b);
        const result = new Uint8Array(totalLength);
        let offset = 0;
        for (const item of arrayBuffers) {
            result.set(new Uint8Array(item), offset);
            offset += item.byteLength;
        }
        return result.buffer;
    }
}
class PromiseSource {
    constructor() {
        this.promise = new Promise((resolve, reject) => [this._resolver, this._rejecter] = [resolve, reject]);
    }
    resolve() {
        this._resolver();
    }
    reject(reason) {
        this._rejecter(reason);
    }
}

;// CONCATENATED MODULE: ./src/JsonHubProtocol.ts
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.





const JSON_HUB_PROTOCOL_NAME = "json";
/** Implements the JSON Hub Protocol. */
class JsonHubProtocol {
    constructor() {
        /** @inheritDoc */
        this.name = JSON_HUB_PROTOCOL_NAME;
        /** @inheritDoc */
        this.version = 2;
        /** @inheritDoc */
        this.transferFormat = TransferFormat.Text;
    }
    /** Creates an array of {@link @microsoft/signalr.HubMessage} objects from the specified serialized representation.
     *
     * @param {string} input A string containing the serialized representation.
     * @param {ILogger} logger A logger that will be used to log messages that occur during parsing.
     */
    parseMessages(input, logger) {
        // The interface does allow "ArrayBuffer" to be passed in, but this implementation does not. So let's throw a useful error.
        if (typeof input !== "string") {
            throw new Error("Invalid input for JSON hub protocol. Expected a string.");
        }
        if (!input) {
            return [];
        }
        if (logger === null) {
            logger = NullLogger.instance;
        }
        // Parse the messages
        const messages = TextMessageFormat.parse(input);
        const hubMessages = [];
        for (const message of messages) {
            const parsedMessage = JSON.parse(message);
            if (typeof parsedMessage.type !== "number") {
                throw new Error("Invalid payload.");
            }
            switch (parsedMessage.type) {
                case MessageType.Invocation:
                    this._isInvocationMessage(parsedMessage);
                    break;
                case MessageType.StreamItem:
                    this._isStreamItemMessage(parsedMessage);
                    break;
                case MessageType.Completion:
                    this._isCompletionMessage(parsedMessage);
                    break;
                case MessageType.Ping:
                    // Single value, no need to validate
                    break;
                case MessageType.Close:
                    // All optional values, no need to validate
                    break;
                case MessageType.Ack:
                    this._isAckMessage(parsedMessage);
                    break;
                case MessageType.Sequence:
                    this._isSequenceMessage(parsedMessage);
                    break;
                default:
                    // Future protocol changes can add message types, old clients can ignore them
                    logger.log(LogLevel.Information, "Unknown message type '" + parsedMessage.type + "' ignored.");
                    continue;
            }
            hubMessages.push(parsedMessage);
        }
        return hubMessages;
    }
    /** Writes the specified {@link @microsoft/signalr.HubMessage} to a string and returns it.
     *
     * @param {HubMessage} message The message to write.
     * @returns {string} A string containing the serialized representation of the message.
     */
    writeMessage(message) {
        return TextMessageFormat.write(JSON.stringify(message));
    }
    _isInvocationMessage(message) {
        this._assertNotEmptyString(message.target, "Invalid payload for Invocation message.");
        if (message.invocationId !== undefined) {
            this._assertNotEmptyString(message.invocationId, "Invalid payload for Invocation message.");
        }
    }
    _isStreamItemMessage(message) {
        this._assertNotEmptyString(message.invocationId, "Invalid payload for StreamItem message.");
        if (message.item === undefined) {
            throw new Error("Invalid payload for StreamItem message.");
        }
    }
    _isCompletionMessage(message) {
        if (message.result && message.error) {
            throw new Error("Invalid payload for Completion message.");
        }
        if (!message.result && message.error) {
            this._assertNotEmptyString(message.error, "Invalid payload for Completion message.");
        }
        this._assertNotEmptyString(message.invocationId, "Invalid payload for Completion message.");
    }
    _isAckMessage(message) {
        if (typeof message.sequenceId !== 'number') {
            throw new Error("Invalid SequenceId for Ack message.");
        }
    }
    _isSequenceMessage(message) {
        if (typeof message.sequenceId !== 'number') {
            throw new Error("Invalid SequenceId for Sequence message.");
        }
    }
    _assertNotEmptyString(value, errorMessage) {
        if (typeof value !== "string" || value === "") {
            throw new Error(errorMessage);
        }
    }
}

;// CONCATENATED MODULE: ./src/HubConnectionBuilder.ts
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.







const LogLevelNameMapping = {
    trace: LogLevel.Trace,
    debug: LogLevel.Debug,
    info: LogLevel.Information,
    information: LogLevel.Information,
    warn: LogLevel.Warning,
    warning: LogLevel.Warning,
    error: LogLevel.Error,
    critical: LogLevel.Critical,
    none: LogLevel.None,
};
function parseLogLevel(name) {
    // Case-insensitive matching via lower-casing
    // Yes, I know case-folding is a complicated problem in Unicode, but we only support
    // the ASCII strings defined in LogLevelNameMapping anyway, so it's fine -anurse.
    const mapping = LogLevelNameMapping[name.toLowerCase()];
    if (typeof mapping !== "undefined") {
        return mapping;
    }
    else {
        throw new Error(`Unknown log level: ${name}`);
    }
}
/** A builder for configuring {@link @microsoft/signalr.HubConnection} instances. */
class HubConnectionBuilder {
    configureLogging(logging) {
        Arg.isRequired(logging, "logging");
        if (isLogger(logging)) {
            this.logger = logging;
        }
        else if (typeof logging === "string") {
            const logLevel = parseLogLevel(logging);
            this.logger = new ConsoleLogger(logLevel);
        }
        else {
            this.logger = new ConsoleLogger(logging);
        }
        return this;
    }
    withUrl(url, transportTypeOrOptions) {
        Arg.isRequired(url, "url");
        Arg.isNotEmpty(url, "url");
        this.url = url;
        // Flow-typing knows where it's at. Since HttpTransportType is a number and IHttpConnectionOptions is guaranteed
        // to be an object, we know (as does TypeScript) this comparison is all we need to figure out which overload was called.
        if (typeof transportTypeOrOptions === "object") {
            this.httpConnectionOptions = { ...this.httpConnectionOptions, ...transportTypeOrOptions };
        }
        else {
            this.httpConnectionOptions = {
                ...this.httpConnectionOptions,
                transport: transportTypeOrOptions,
            };
        }
        return this;
    }
    /** Configures the {@link @microsoft/signalr.HubConnection} to use the specified Hub Protocol.
     *
     * @param {IHubProtocol} protocol The {@link @microsoft/signalr.IHubProtocol} implementation to use.
     */
    withHubProtocol(protocol) {
        Arg.isRequired(protocol, "protocol");
        this.protocol = protocol;
        return this;
    }
    withAutomaticReconnect(retryDelaysOrReconnectPolicy) {
        if (this.reconnectPolicy) {
            throw new Error("A reconnectPolicy has already been set.");
        }
        if (!retryDelaysOrReconnectPolicy) {
            this.reconnectPolicy = new DefaultReconnectPolicy();
        }
        else if (Array.isArray(retryDelaysOrReconnectPolicy)) {
            this.reconnectPolicy = new DefaultReconnectPolicy(retryDelaysOrReconnectPolicy);
        }
        else {
            this.reconnectPolicy = retryDelaysOrReconnectPolicy;
        }
        return this;
    }
    /** Configures {@link @microsoft/signalr.HubConnection.serverTimeoutInMilliseconds} for the {@link @microsoft/signalr.HubConnection}.
     *
     * @returns The {@link @microsoft/signalr.HubConnectionBuilder} instance, for chaining.
     */
    withServerTimeout(milliseconds) {
        Arg.isRequired(milliseconds, "milliseconds");
        this._serverTimeoutInMilliseconds = milliseconds;
        return this;
    }
    /** Configures {@link @microsoft/signalr.HubConnection.keepAliveIntervalInMilliseconds} for the {@link @microsoft/signalr.HubConnection}.
     *
     * @returns The {@link @microsoft/signalr.HubConnectionBuilder} instance, for chaining.
     */
    withKeepAliveInterval(milliseconds) {
        Arg.isRequired(milliseconds, "milliseconds");
        this._keepAliveIntervalInMilliseconds = milliseconds;
        return this;
    }
    /** Enables and configures options for the Stateful Reconnect feature.
     *
     * @returns The {@link @microsoft/signalr.HubConnectionBuilder} instance, for chaining.
     */
    withStatefulReconnect(options) {
        if (this.httpConnectionOptions === undefined) {
            this.httpConnectionOptions = {};
        }
        this.httpConnectionOptions._useStatefulReconnect = true;
        this._statefulReconnectBufferSize = options === null || options === void 0 ? void 0 : options.bufferSize;
        return this;
    }
    /** Creates a {@link @microsoft/signalr.HubConnection} from the configuration options specified in this builder.
     *
     * @returns {HubConnection} The configured {@link @microsoft/signalr.HubConnection}.
     */
    build() {
        // If httpConnectionOptions has a logger, use it. Otherwise, override it with the one
        // provided to configureLogger
        const httpConnectionOptions = this.httpConnectionOptions || {};
        // If it's 'null', the user **explicitly** asked for null, don't mess with it.
        if (httpConnectionOptions.logger === undefined) {
            // If our logger is undefined or null, that's OK, the HttpConnection constructor will handle it.
            httpConnectionOptions.logger = this.logger;
        }
        // Now create the connection
        if (!this.url) {
            throw new Error("The 'HubConnectionBuilder.withUrl' method must be called before building the connection.");
        }
        const connection = new HttpConnection(this.url, httpConnectionOptions);
        return HubConnection.create(connection, this.logger || NullLogger.instance, this.protocol || new JsonHubProtocol(), this.reconnectPolicy, this._serverTimeoutInMilliseconds, this._keepAliveIntervalInMilliseconds, this._statefulReconnectBufferSize);
    }
}
function isLogger(logger) {
    return logger.log !== undefined;
}

;// CONCATENATED MODULE: ./src/index.ts
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.













;// CONCATENATED MODULE: ./src/browser-index.ts
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// This is where we add any polyfills we'll need for the browser. It is the entry module for browser-specific builds.
// Copy from Array.prototype into Uint8Array to polyfill on IE. It's OK because the implementations of indexOf and slice use properties
// that exist on Uint8Array with the same name, and JavaScript is magic.
// We make them 'writable' because the Buffer polyfill messes with it as well.
if (!Uint8Array.prototype.indexOf) {
    Object.defineProperty(Uint8Array.prototype, "indexOf", {
        value: Array.prototype.indexOf,
        writable: true,
    });
}
if (!Uint8Array.prototype.slice) {
    Object.defineProperty(Uint8Array.prototype, "slice", {
        // wrap the slice in Uint8Array so it looks like a Uint8Array.slice call
        // eslint-disable-next-line object-shorthand
        value: function (start, end) { return new Uint8Array(Array.prototype.slice.call(this, start, end)); },
        writable: true,
    });
}
if (!Uint8Array.prototype.forEach) {
    Object.defineProperty(Uint8Array.prototype, "forEach", {
        value: Array.prototype.forEach,
        writable: true,
    });
}


/******/ 	return __webpack_exports__;
/******/ })()
;
});
//# sourceMappingURL=signalr.js.map
```

### wwwroot\js\signalr\dist\browser\signalr.min.js

```javascript
var t,e;t=self,e=()=>(()=>{var t={d:(e,s)=>{for(var i in s)t.o(s,i)&&!t.o(e,i)&&Object.defineProperty(e,i,{enumerable:!0,get:s[i]})}};t.g=function(){if("object"==typeof globalThis)return globalThis;try{return this||new Function("return this")()}catch(t){if("object"==typeof window)return window}}(),t.o=(t,e)=>Object.prototype.hasOwnProperty.call(t,e),t.r=t=>{"undefined"!=typeof Symbol&&Symbol.toStringTag&&Object.defineProperty(t,Symbol.toStringTag,{value:"Module"}),Object.defineProperty(t,"t",{value:!0})};var e,s={};t.r(s),t.d(s,{AbortError:()=>r,DefaultHttpClient:()=>H,HttpClient:()=>d,HttpError:()=>i,HttpResponse:()=>u,HttpTransportType:()=>F,HubConnection:()=>q,HubConnectionBuilder:()=>tt,HubConnectionState:()=>A,JsonHubProtocol:()=>Y,LogLevel:()=>e,MessageType:()=>x,NullLogger:()=>f,Subject:()=>U,TimeoutError:()=>n,TransferFormat:()=>B,VERSION:()=>p});class i extends Error{constructor(t,e){const s=new.target.prototype;super(`${t}: Status code '${e}'`),this.statusCode=e,this.__proto__=s}}class n extends Error{constructor(t="A timeout occurred."){const e=new.target.prototype;super(t),this.__proto__=e}}class r extends Error{constructor(t="An abort occurred."){const e=new.target.prototype;super(t),this.__proto__=e}}class o extends Error{constructor(t,e){const s=new.target.prototype;super(t),this.transport=e,this.errorType="UnsupportedTransportError",this.__proto__=s}}class h extends Error{constructor(t,e){const s=new.target.prototype;super(t),this.transport=e,this.errorType="DisabledTransportError",this.__proto__=s}}class c extends Error{constructor(t,e){const s=new.target.prototype;super(t),this.transport=e,this.errorType="FailedToStartTransportError",this.__proto__=s}}class a extends Error{constructor(t){const e=new.target.prototype;super(t),this.errorType="FailedToNegotiateWithServerError",this.__proto__=e}}class l extends Error{constructor(t,e){const s=new.target.prototype;super(t),this.innerErrors=e,this.__proto__=s}}class u{constructor(t,e,s){this.statusCode=t,this.statusText=e,this.content=s}}class d{get(t,e){return this.send({...e,method:"GET",url:t})}post(t,e){return this.send({...e,method:"POST",url:t})}delete(t,e){return this.send({...e,method:"DELETE",url:t})}getCookieString(t){return""}}!function(t){t[t.Trace=0]="Trace",t[t.Debug=1]="Debug",t[t.Information=2]="Information",t[t.Warning=3]="Warning",t[t.Error=4]="Error",t[t.Critical=5]="Critical",t[t.None=6]="None"}(e||(e={}));class f{constructor(){}log(t,e){}}f.instance=new f;const p="8.0.7";class w{static isRequired(t,e){if(null==t)throw new Error(`The '${e}' argument is required.`)}static isNotEmpty(t,e){if(!t||t.match(/^\s*$/))throw new Error(`The '${e}' argument should not be empty.`)}static isIn(t,e,s){if(!(t in e))throw new Error(`Unknown ${s} value: ${t}.`)}}class g{static get isBrowser(){return!g.isNode&&"object"==typeof window&&"object"==typeof window.document}static get isWebWorker(){return!g.isNode&&"object"==typeof self&&"importScripts"in self}static get isReactNative(){return!g.isNode&&"object"==typeof window&&void 0===window.document}static get isNode(){return"undefined"!=typeof process&&process.release&&"node"===process.release.name}}function m(t,e){let s="";return y(t)?(s=`Binary data of length ${t.byteLength}`,e&&(s+=`. Content: '${function(t){const e=new Uint8Array(t);let s="";return e.forEach((t=>{s+=`0x${t<16?"0":""}${t.toString(16)} `})),s.substr(0,s.length-1)}(t)}'`)):"string"==typeof t&&(s=`String data of length ${t.length}`,e&&(s+=`. Content: '${t}'`)),s}function y(t){return t&&"undefined"!=typeof ArrayBuffer&&(t instanceof ArrayBuffer||t.constructor&&"ArrayBuffer"===t.constructor.name)}async function b(t,s,i,n,r,o){const h={},[c,a]=$();h[c]=a,t.log(e.Trace,`(${s} transport) sending data. ${m(r,o.logMessageContent)}.`);const l=y(r)?"arraybuffer":"text",u=await i.post(n,{content:r,headers:{...h,...o.headers},responseType:l,timeout:o.timeout,withCredentials:o.withCredentials});t.log(e.Trace,`(${s} transport) request complete. Response status: ${u.statusCode}.`)}class v{constructor(t,e){this.i=t,this.h=e}dispose(){const t=this.i.observers.indexOf(this.h);t>-1&&this.i.observers.splice(t,1),0===this.i.observers.length&&this.i.cancelCallback&&this.i.cancelCallback().catch((t=>{}))}}class E{constructor(t){this.l=t,this.out=console}log(t,s){if(t>=this.l){const i=`[${(new Date).toISOString()}] ${e[t]}: ${s}`;switch(t){case e.Critical:case e.Error:this.out.error(i);break;case e.Warning:this.out.warn(i);break;case e.Information:this.out.info(i);break;default:this.out.log(i)}}}}function $(){let t="X-SignalR-User-Agent";return g.isNode&&(t="User-Agent"),[t,C(p,S(),g.isNode?"NodeJS":"Browser",k())]}function C(t,e,s,i){let n="Microsoft SignalR/";const r=t.split(".");return n+=`${r[0]}.${r[1]}`,n+=` (${t}; `,n+=e&&""!==e?`${e}; `:"Unknown OS; ",n+=`${s}`,n+=i?`; ${i}`:"; Unknown Runtime Version",n+=")",n}function S(){if(!g.isNode)return"";switch(process.platform){case"win32":return"Windows NT";case"darwin":return"macOS";case"linux":return"Linux";default:return process.platform}}function k(){if(g.isNode)return process.versions.node}function P(t){return t.stack?t.stack:t.message?t.message:`${t}`}class T extends d{constructor(e){if(super(),this.u=e,"undefined"==typeof fetch||g.isNode){const t=require;this.p=new(t("tough-cookie").CookieJar),"undefined"==typeof fetch?this.m=t("node-fetch"):this.m=fetch,this.m=t("fetch-cookie")(this.m,this.p)}else this.m=fetch.bind(function(){if("undefined"!=typeof globalThis)return globalThis;if("undefined"!=typeof self)return self;if("undefined"!=typeof window)return window;if(void 0!==t.g)return t.g;throw new Error("could not find global")}());if("undefined"==typeof AbortController){const t=require;this.v=t("abort-controller")}else this.v=AbortController}async send(t){if(t.abortSignal&&t.abortSignal.aborted)throw new r;if(!t.method)throw new Error("No method defined.");if(!t.url)throw new Error("No url defined.");const s=new this.v;let o;t.abortSignal&&(t.abortSignal.onabort=()=>{s.abort(),o=new r});let h,c=null;if(t.timeout){const i=t.timeout;c=setTimeout((()=>{s.abort(),this.u.log(e.Warning,"Timeout from HTTP request."),o=new n}),i)}""===t.content&&(t.content=void 0),t.content&&(t.headers=t.headers||{},y(t.content)?t.headers["Content-Type"]="application/octet-stream":t.headers["Content-Type"]="text/plain;charset=UTF-8");try{h=await this.m(t.url,{body:t.content,cache:"no-cache",credentials:!0===t.withCredentials?"include":"same-origin",headers:{"X-Requested-With":"XMLHttpRequest",...t.headers},method:t.method,mode:"cors",redirect:"follow",signal:s.signal})}catch(t){if(o)throw o;throw this.u.log(e.Warning,`Error from HTTP request. ${t}.`),t}finally{c&&clearTimeout(c),t.abortSignal&&(t.abortSignal.onabort=null)}if(!h.ok){const t=await I(h,"text");throw new i(t||h.statusText,h.status)}const a=I(h,t.responseType),l=await a;return new u(h.status,h.statusText,l)}getCookieString(t){let e="";return g.isNode&&this.p&&this.p.getCookies(t,((t,s)=>e=s.join("; "))),e}}function I(t,e){let s;switch(e){case"arraybuffer":s=t.arrayBuffer();break;case"text":default:s=t.text();break;case"blob":case"document":case"json":throw new Error(`${e} is not supported.`)}return s}class _ extends d{constructor(t){super(),this.u=t}send(t){return t.abortSignal&&t.abortSignal.aborted?Promise.reject(new r):t.method?t.url?new Promise(((s,o)=>{const h=new XMLHttpRequest;h.open(t.method,t.url,!0),h.withCredentials=void 0===t.withCredentials||t.withCredentials,h.setRequestHeader("X-Requested-With","XMLHttpRequest"),""===t.content&&(t.content=void 0),t.content&&(y(t.content)?h.setRequestHeader("Content-Type","application/octet-stream"):h.setRequestHeader("Content-Type","text/plain;charset=UTF-8"));const c=t.headers;c&&Object.keys(c).forEach((t=>{h.setRequestHeader(t,c[t])})),t.responseType&&(h.responseType=t.responseType),t.abortSignal&&(t.abortSignal.onabort=()=>{h.abort(),o(new r)}),t.timeout&&(h.timeout=t.timeout),h.onload=()=>{t.abortSignal&&(t.abortSignal.onabort=null),h.status>=200&&h.status<300?s(new u(h.status,h.statusText,h.response||h.responseText)):o(new i(h.response||h.responseText||h.statusText,h.status))},h.onerror=()=>{this.u.log(e.Warning,`Error from HTTP request. ${h.status}: ${h.statusText}.`),o(new i(h.statusText,h.status))},h.ontimeout=()=>{this.u.log(e.Warning,"Timeout from HTTP request."),o(new n)},h.send(t.content)})):Promise.reject(new Error("No url defined.")):Promise.reject(new Error("No method defined."))}}class H extends d{constructor(t){if(super(),"undefined"!=typeof fetch||g.isNode)this.$=new T(t);else{if("undefined"==typeof XMLHttpRequest)throw new Error("No usable HttpClient found.");this.$=new _(t)}}send(t){return t.abortSignal&&t.abortSignal.aborted?Promise.reject(new r):t.method?t.url?this.$.send(t):Promise.reject(new Error("No url defined.")):Promise.reject(new Error("No method defined."))}getCookieString(t){return this.$.getCookieString(t)}}class D{static write(t){return`${t}${D.RecordSeparator}`}static parse(t){if(t[t.length-1]!==D.RecordSeparator)throw new Error("Message is incomplete.");const e=t.split(D.RecordSeparator);return e.pop(),e}}D.RecordSeparatorCode=30,D.RecordSeparator=String.fromCharCode(D.RecordSeparatorCode);class R{writeHandshakeRequest(t){return D.write(JSON.stringify(t))}parseHandshakeResponse(t){let e,s;if(y(t)){const i=new Uint8Array(t),n=i.indexOf(D.RecordSeparatorCode);if(-1===n)throw new Error("Message is incomplete.");const r=n+1;e=String.fromCharCode.apply(null,Array.prototype.slice.call(i.slice(0,r))),s=i.byteLength>r?i.slice(r).buffer:null}else{const i=t,n=i.indexOf(D.RecordSeparator);if(-1===n)throw new Error("Message is incomplete.");const r=n+1;e=i.substring(0,r),s=i.length>r?i.substring(r):null}const i=D.parse(e),n=JSON.parse(i[0]);if(n.type)throw new Error("Expected a handshake response from the server.");return[s,n]}}var x,A;!function(t){t[t.Invocation=1]="Invocation",t[t.StreamItem=2]="StreamItem",t[t.Completion=3]="Completion",t[t.StreamInvocation=4]="StreamInvocation",t[t.CancelInvocation=5]="CancelInvocation",t[t.Ping=6]="Ping",t[t.Close=7]="Close",t[t.Ack=8]="Ack",t[t.Sequence=9]="Sequence"}(x||(x={}));class U{constructor(){this.observers=[]}next(t){for(const e of this.observers)e.next(t)}error(t){for(const e of this.observers)e.error&&e.error(t)}complete(){for(const t of this.observers)t.complete&&t.complete()}subscribe(t){return this.observers.push(t),new v(this,t)}}class L{constructor(t,e,s){this.C=1e5,this.S=[],this.k=0,this.P=!1,this.T=1,this.I=0,this._=0,this.H=!1,this.D=t,this.R=e,this.C=s}async A(t){const e=this.D.writeMessage(t);let s=Promise.resolve();if(this.U(t)){this.k++;let t=()=>{},i=()=>{};y(e)?this._+=e.byteLength:this._+=e.length,this._>=this.C&&(s=new Promise(((e,s)=>{t=e,i=s}))),this.S.push(new N(e,this.k,t,i))}try{this.H||await this.R.send(e)}catch{this.L()}await s}N(t){let e=-1;for(let s=0;s<this.S.length;s++){const i=this.S[s];if(i.q<=t.sequenceId)e=s,y(i.M)?this._-=i.M.byteLength:this._-=i.M.length,i.j();else{if(!(this._<this.C))break;i.j()}}-1!==e&&(this.S=this.S.slice(e+1))}W(t){if(this.P)return t.type===x.Sequence&&(this.P=!1,!0);if(!this.U(t))return!0;const e=this.T;return this.T++,e<=this.I?(e===this.I&&this.O(),!1):(this.I=e,this.O(),!0)}F(t){t.sequenceId>this.T?this.R.stop(new Error("Sequence ID greater than amount of messages we've received.")):this.T=t.sequenceId}L(){this.H=!0,this.P=!0}async B(){const t=0!==this.S.length?this.S[0].q:this.k+1;await this.R.send(this.D.writeMessage({type:x.Sequence,sequenceId:t}));const e=this.S;for(const t of e)await this.R.send(t.M);this.H=!1}X(t){null!=t||(t=new Error("Unable to reconnect to server."));for(const e of this.S)e.J(t)}U(t){switch(t.type){case x.Invocation:case x.StreamItem:case x.Completion:case x.StreamInvocation:case x.CancelInvocation:return!0;case x.Close:case x.Sequence:case x.Ping:case x.Ack:return!1}}O(){void 0===this.V&&(this.V=setTimeout((async()=>{try{this.H||await this.R.send(this.D.writeMessage({type:x.Ack,sequenceId:this.I}))}catch{}clearTimeout(this.V),this.V=void 0}),1e3))}}class N{constructor(t,e,s,i){this.M=t,this.q=e,this.j=s,this.J=i}}!function(t){t.Disconnected="Disconnected",t.Connecting="Connecting",t.Connected="Connected",t.Disconnecting="Disconnecting",t.Reconnecting="Reconnecting"}(A||(A={}));class q{static create(t,e,s,i,n,r,o){return new q(t,e,s,i,n,r,o)}constructor(t,s,i,n,r,o,h){this.K=0,this.G=()=>{this.u.log(e.Warning,"The page is being frozen, this will likely lead to the connection being closed and messages being lost. For more information see the docs at https://learn.microsoft.com/aspnet/core/signalr/javascript-client#bsleep")},w.isRequired(t,"connection"),w.isRequired(s,"logger"),w.isRequired(i,"protocol"),this.serverTimeoutInMilliseconds=null!=r?r:3e4,this.keepAliveIntervalInMilliseconds=null!=o?o:15e3,this.Y=null!=h?h:1e5,this.u=s,this.D=i,this.connection=t,this.Z=n,this.tt=new R,this.connection.onreceive=t=>this.et(t),this.connection.onclose=t=>this.st(t),this.it={},this.nt={},this.rt=[],this.ot=[],this.ht=[],this.ct=0,this.lt=!1,this.ut=A.Disconnected,this.dt=!1,this.ft=this.D.writeMessage({type:x.Ping})}get state(){return this.ut}get connectionId(){return this.connection&&this.connection.connectionId||null}get baseUrl(){return this.connection.baseUrl||""}set baseUrl(t){if(this.ut!==A.Disconnected&&this.ut!==A.Reconnecting)throw new Error("The HubConnection must be in the Disconnected or Reconnecting state to change the url.");if(!t)throw new Error("The HubConnection url must be a valid url.");this.connection.baseUrl=t}start(){return this.wt=this.gt(),this.wt}async gt(){if(this.ut!==A.Disconnected)return Promise.reject(new Error("Cannot start a HubConnection that is not in the 'Disconnected' state."));this.ut=A.Connecting,this.u.log(e.Debug,"Starting HubConnection.");try{await this.yt(),g.isBrowser&&window.document.addEventListener("freeze",this.G),this.ut=A.Connected,this.dt=!0,this.u.log(e.Debug,"HubConnection connected successfully.")}catch(t){return this.ut=A.Disconnected,this.u.log(e.Debug,`HubConnection failed to start successfully because of error '${t}'.`),Promise.reject(t)}}async yt(){this.bt=void 0,this.lt=!1;const t=new Promise(((t,e)=>{this.vt=t,this.Et=e}));await this.connection.start(this.D.transferFormat);try{let s=this.D.version;this.connection.features.reconnect||(s=1);const i={protocol:this.D.name,version:s};if(this.u.log(e.Debug,"Sending handshake request."),await this.$t(this.tt.writeHandshakeRequest(i)),this.u.log(e.Information,`Using HubProtocol '${this.D.name}'.`),this.Ct(),this.St(),this.kt(),await t,this.bt)throw this.bt;!!this.connection.features.reconnect&&(this.Pt=new L(this.D,this.connection,this.Y),this.connection.features.disconnected=this.Pt.L.bind(this.Pt),this.connection.features.resend=()=>{if(this.Pt)return this.Pt.B()}),this.connection.features.inherentKeepAlive||await this.$t(this.ft)}catch(t){throw this.u.log(e.Debug,`Hub handshake failed with error '${t}' during start(). Stopping HubConnection.`),this.Ct(),this.Tt(),await this.connection.stop(t),t}}async stop(){const t=this.wt;this.connection.features.reconnect=!1,this.It=this._t(),await this.It;try{await t}catch(t){}}_t(t){if(this.ut===A.Disconnected)return this.u.log(e.Debug,`Call to HubConnection.stop(${t}) ignored because it is already in the disconnected state.`),Promise.resolve();if(this.ut===A.Disconnecting)return this.u.log(e.Debug,`Call to HttpConnection.stop(${t}) ignored because the connection is already in the disconnecting state.`),this.It;const s=this.ut;return this.ut=A.Disconnecting,this.u.log(e.Debug,"Stopping HubConnection."),this.Ht?(this.u.log(e.Debug,"Connection stopped during reconnect delay. Done reconnecting."),clearTimeout(this.Ht),this.Ht=void 0,this.Dt(),Promise.resolve()):(s===A.Connected&&this.Rt(),this.Ct(),this.Tt(),this.bt=t||new r("The connection was stopped before the hub handshake could complete."),this.connection.stop(t))}async Rt(){try{await this.xt(this.At())}catch{}}stream(t,...e){const[s,i]=this.Ut(e),n=this.Lt(t,e,i);let r;const o=new U;return o.cancelCallback=()=>{const t=this.Nt(n.invocationId);return delete this.it[n.invocationId],r.then((()=>this.xt(t)))},this.it[n.invocationId]=(t,e)=>{e?o.error(e):t&&(t.type===x.Completion?t.error?o.error(new Error(t.error)):o.complete():o.next(t.item))},r=this.xt(n).catch((t=>{o.error(t),delete this.it[n.invocationId]})),this.qt(s,r),o}$t(t){return this.kt(),this.connection.send(t)}xt(t){return this.Pt?this.Pt.A(t):this.$t(this.D.writeMessage(t))}send(t,...e){const[s,i]=this.Ut(e),n=this.xt(this.Mt(t,e,!0,i));return this.qt(s,n),n}invoke(t,...e){const[s,i]=this.Ut(e),n=this.Mt(t,e,!1,i);return new Promise(((t,e)=>{this.it[n.invocationId]=(s,i)=>{i?e(i):s&&(s.type===x.Completion?s.error?e(new Error(s.error)):t(s.result):e(new Error(`Unexpected message type: ${s.type}`)))};const i=this.xt(n).catch((t=>{e(t),delete this.it[n.invocationId]}));this.qt(s,i)}))}on(t,e){t&&e&&(t=t.toLowerCase(),this.nt[t]||(this.nt[t]=[]),-1===this.nt[t].indexOf(e)&&this.nt[t].push(e))}off(t,e){if(!t)return;t=t.toLowerCase();const s=this.nt[t];if(s)if(e){const i=s.indexOf(e);-1!==i&&(s.splice(i,1),0===s.length&&delete this.nt[t])}else delete this.nt[t]}onclose(t){t&&this.rt.push(t)}onreconnecting(t){t&&this.ot.push(t)}onreconnected(t){t&&this.ht.push(t)}et(t){if(this.Ct(),this.lt||(t=this.jt(t),this.lt=!0),t){const s=this.D.parseMessages(t,this.u);for(const t of s)if(!this.Pt||this.Pt.W(t))switch(t.type){case x.Invocation:this.Wt(t).catch((t=>{this.u.log(e.Error,`Invoke client method threw error: ${P(t)}`)}));break;case x.StreamItem:case x.Completion:{const s=this.it[t.invocationId];if(s){t.type===x.Completion&&delete this.it[t.invocationId];try{s(t)}catch(t){this.u.log(e.Error,`Stream callback threw error: ${P(t)}`)}}break}case x.Ping:break;case x.Close:{this.u.log(e.Information,"Close message received from server.");const s=t.error?new Error("Server returned an error on close: "+t.error):void 0;!0===t.allowReconnect?this.connection.stop(s):this.It=this._t(s);break}case x.Ack:this.Pt&&this.Pt.N(t);break;case x.Sequence:this.Pt&&this.Pt.F(t);break;default:this.u.log(e.Warning,`Invalid message type: ${t.type}.`)}}this.St()}jt(t){let s,i;try{[i,s]=this.tt.parseHandshakeResponse(t)}catch(t){const s="Error parsing handshake response: "+t;this.u.log(e.Error,s);const i=new Error(s);throw this.Et(i),i}if(s.error){const t="Server returned handshake error: "+s.error;this.u.log(e.Error,t);const i=new Error(t);throw this.Et(i),i}return this.u.log(e.Debug,"Server handshake complete."),this.vt(),i}kt(){this.connection.features.inherentKeepAlive||(this.K=(new Date).getTime()+this.keepAliveIntervalInMilliseconds,this.Tt())}St(){if(!(this.connection.features&&this.connection.features.inherentKeepAlive||(this.Ot=setTimeout((()=>this.serverTimeout()),this.serverTimeoutInMilliseconds),void 0!==this.Ft))){let t=this.K-(new Date).getTime();t<0&&(t=0),this.Ft=setTimeout((async()=>{if(this.ut===A.Connected)try{await this.$t(this.ft)}catch{this.Tt()}}),t)}}serverTimeout(){this.connection.stop(new Error("Server timeout elapsed without receiving a message from the server."))}async Wt(t){const s=t.target.toLowerCase(),i=this.nt[s];if(!i)return this.u.log(e.Warning,`No client method with the name '${s}' found.`),void(t.invocationId&&(this.u.log(e.Warning,`No result given for '${s}' method and invocation ID '${t.invocationId}'.`),await this.xt(this.Bt(t.invocationId,"Client didn't provide a result.",null))));const n=i.slice(),r=!!t.invocationId;let o,h,c;for(const i of n)try{const n=o;o=await i.apply(this,t.arguments),r&&o&&n&&(this.u.log(e.Error,`Multiple results provided for '${s}'. Sending error to server.`),c=this.Bt(t.invocationId,"Client provided multiple results.",null)),h=void 0}catch(t){h=t,this.u.log(e.Error,`A callback for the method '${s}' threw error '${t}'.`)}c?await this.xt(c):r?(h?c=this.Bt(t.invocationId,`${h}`,null):void 0!==o?c=this.Bt(t.invocationId,null,o):(this.u.log(e.Warning,`No result given for '${s}' method and invocation ID '${t.invocationId}'.`),c=this.Bt(t.invocationId,"Client didn't provide a result.",null)),await this.xt(c)):o&&this.u.log(e.Error,`Result given for '${s}' method but server is not expecting a result.`)}st(t){this.u.log(e.Debug,`HubConnection.connectionClosed(${t}) called while in state ${this.ut}.`),this.bt=this.bt||t||new r("The underlying connection was closed before the hub handshake could complete."),this.vt&&this.vt(),this.Xt(t||new Error("Invocation canceled due to the underlying connection being closed.")),this.Ct(),this.Tt(),this.ut===A.Disconnecting?this.Dt(t):this.ut===A.Connected&&this.Z?this.Jt(t):this.ut===A.Connected&&this.Dt(t)}Dt(t){if(this.dt){this.ut=A.Disconnected,this.dt=!1,this.Pt&&(this.Pt.X(null!=t?t:new Error("Connection closed.")),this.Pt=void 0),g.isBrowser&&window.document.removeEventListener("freeze",this.G);try{this.rt.forEach((e=>e.apply(this,[t])))}catch(s){this.u.log(e.Error,`An onclose callback called with error '${t}' threw error '${s}'.`)}}}async Jt(t){const s=Date.now();let i=0,n=void 0!==t?t:new Error("Attempting to reconnect due to a unknown error."),r=this.zt(i++,0,n);if(null===r)return this.u.log(e.Debug,"Connection not reconnecting because the IRetryPolicy returned null on the first reconnect attempt."),void this.Dt(t);if(this.ut=A.Reconnecting,t?this.u.log(e.Information,`Connection reconnecting because of error '${t}'.`):this.u.log(e.Information,"Connection reconnecting."),0!==this.ot.length){try{this.ot.forEach((e=>e.apply(this,[t])))}catch(s){this.u.log(e.Error,`An onreconnecting callback called with error '${t}' threw error '${s}'.`)}if(this.ut!==A.Reconnecting)return void this.u.log(e.Debug,"Connection left the reconnecting state in onreconnecting callback. Done reconnecting.")}for(;null!==r;){if(this.u.log(e.Information,`Reconnect attempt number ${i} will start in ${r} ms.`),await new Promise((t=>{this.Ht=setTimeout(t,r)})),this.Ht=void 0,this.ut!==A.Reconnecting)return void this.u.log(e.Debug,"Connection left the reconnecting state during reconnect delay. Done reconnecting.");try{if(await this.yt(),this.ut=A.Connected,this.u.log(e.Information,"HubConnection reconnected successfully."),0!==this.ht.length)try{this.ht.forEach((t=>t.apply(this,[this.connection.connectionId])))}catch(t){this.u.log(e.Error,`An onreconnected callback called with connectionId '${this.connection.connectionId}; threw error '${t}'.`)}return}catch(t){if(this.u.log(e.Information,`Reconnect attempt failed because of error '${t}'.`),this.ut!==A.Reconnecting)return this.u.log(e.Debug,`Connection moved to the '${this.ut}' from the reconnecting state during reconnect attempt. Done reconnecting.`),void(this.ut===A.Disconnecting&&this.Dt());n=t instanceof Error?t:new Error(t.toString()),r=this.zt(i++,Date.now()-s,n)}}this.u.log(e.Information,`Reconnect retries have been exhausted after ${Date.now()-s} ms and ${i} failed attempts. Connection disconnecting.`),this.Dt()}zt(t,s,i){try{return this.Z.nextRetryDelayInMilliseconds({elapsedMilliseconds:s,previousRetryCount:t,retryReason:i})}catch(i){return this.u.log(e.Error,`IRetryPolicy.nextRetryDelayInMilliseconds(${t}, ${s}) threw error '${i}'.`),null}}Xt(t){const s=this.it;this.it={},Object.keys(s).forEach((i=>{const n=s[i];try{n(null,t)}catch(s){this.u.log(e.Error,`Stream 'error' callback called with '${t}' threw error: ${P(s)}`)}}))}Tt(){this.Ft&&(clearTimeout(this.Ft),this.Ft=void 0)}Ct(){this.Ot&&clearTimeout(this.Ot)}Mt(t,e,s,i){if(s)return 0!==i.length?{arguments:e,streamIds:i,target:t,type:x.Invocation}:{arguments:e,target:t,type:x.Invocation};{const s=this.ct;return this.ct++,0!==i.length?{arguments:e,invocationId:s.toString(),streamIds:i,target:t,type:x.Invocation}:{arguments:e,invocationId:s.toString(),target:t,type:x.Invocation}}}qt(t,e){if(0!==t.length){e||(e=Promise.resolve());for(const s in t)t[s].subscribe({complete:()=>{e=e.then((()=>this.xt(this.Bt(s))))},error:t=>{let i;i=t instanceof Error?t.message:t&&t.toString?t.toString():"Unknown error",e=e.then((()=>this.xt(this.Bt(s,i))))},next:t=>{e=e.then((()=>this.xt(this.Vt(s,t))))}})}}Ut(t){const e=[],s=[];for(let i=0;i<t.length;i++){const n=t[i];if(this.Kt(n)){const r=this.ct;this.ct++,e[r]=n,s.push(r.toString()),t.splice(i,1)}}return[e,s]}Kt(t){return t&&t.subscribe&&"function"==typeof t.subscribe}Lt(t,e,s){const i=this.ct;return this.ct++,0!==s.length?{arguments:e,invocationId:i.toString(),streamIds:s,target:t,type:x.StreamInvocation}:{arguments:e,invocationId:i.toString(),target:t,type:x.StreamInvocation}}Nt(t){return{invocationId:t,type:x.CancelInvocation}}Vt(t,e){return{invocationId:t,item:e,type:x.StreamItem}}Bt(t,e,s){return e?{error:e,invocationId:t,type:x.Completion}:{invocationId:t,result:s,type:x.Completion}}At(){return{type:x.Close}}}const M=[0,2e3,1e4,3e4,null];class j{constructor(t){this.Gt=void 0!==t?[...t,null]:M}nextRetryDelayInMilliseconds(t){return this.Gt[t.previousRetryCount]}}class W{}W.Authorization="Authorization",W.Cookie="Cookie";class O extends d{constructor(t,e){super(),this.Qt=t,this.Yt=e}async send(t){let e=!0;this.Yt&&(!this.Zt||t.url&&t.url.indexOf("/negotiate?")>0)&&(e=!1,this.Zt=await this.Yt()),this.te(t);const s=await this.Qt.send(t);return e&&401===s.statusCode&&this.Yt?(this.Zt=await this.Yt(),this.te(t),await this.Qt.send(t)):s}te(t){t.headers||(t.headers={}),this.Zt?t.headers[W.Authorization]=`Bearer ${this.Zt}`:this.Yt&&t.headers[W.Authorization]&&delete t.headers[W.Authorization]}getCookieString(t){return this.Qt.getCookieString(t)}}var F,B;!function(t){t[t.None=0]="None",t[t.WebSockets=1]="WebSockets",t[t.ServerSentEvents=2]="ServerSentEvents",t[t.LongPolling=4]="LongPolling"}(F||(F={})),function(t){t[t.Text=1]="Text",t[t.Binary=2]="Binary"}(B||(B={}));class X{constructor(){this.ee=!1,this.onabort=null}abort(){this.ee||(this.ee=!0,this.onabort&&this.onabort())}get signal(){return this}get aborted(){return this.ee}}class J{get pollAborted(){return this.se.aborted}constructor(t,e,s){this.$=t,this.u=e,this.se=new X,this.ie=s,this.ne=!1,this.onreceive=null,this.onclose=null}async connect(t,s){if(w.isRequired(t,"url"),w.isRequired(s,"transferFormat"),w.isIn(s,B,"transferFormat"),this.re=t,this.u.log(e.Trace,"(LongPolling transport) Connecting."),s===B.Binary&&"undefined"!=typeof XMLHttpRequest&&"string"!=typeof(new XMLHttpRequest).responseType)throw new Error("Binary protocols over XmlHttpRequest not implementing advanced features are not supported.");const[n,r]=$(),o={[n]:r,...this.ie.headers},h={abortSignal:this.se.signal,headers:o,timeout:1e5,withCredentials:this.ie.withCredentials};s===B.Binary&&(h.responseType="arraybuffer");const c=`${t}&_=${Date.now()}`;this.u.log(e.Trace,`(LongPolling transport) polling: ${c}.`);const a=await this.$.get(c,h);200!==a.statusCode?(this.u.log(e.Error,`(LongPolling transport) Unexpected response code: ${a.statusCode}.`),this.oe=new i(a.statusText||"",a.statusCode),this.ne=!1):this.ne=!0,this.he=this.ce(this.re,h)}async ce(t,s){try{for(;this.ne;)try{const n=`${t}&_=${Date.now()}`;this.u.log(e.Trace,`(LongPolling transport) polling: ${n}.`);const r=await this.$.get(n,s);204===r.statusCode?(this.u.log(e.Information,"(LongPolling transport) Poll terminated by server."),this.ne=!1):200!==r.statusCode?(this.u.log(e.Error,`(LongPolling transport) Unexpected response code: ${r.statusCode}.`),this.oe=new i(r.statusText||"",r.statusCode),this.ne=!1):r.content?(this.u.log(e.Trace,`(LongPolling transport) data received. ${m(r.content,this.ie.logMessageContent)}.`),this.onreceive&&this.onreceive(r.content)):this.u.log(e.Trace,"(LongPolling transport) Poll timed out, reissuing.")}catch(t){this.ne?t instanceof n?this.u.log(e.Trace,"(LongPolling transport) Poll timed out, reissuing."):(this.oe=t,this.ne=!1):this.u.log(e.Trace,`(LongPolling transport) Poll errored after shutdown: ${t.message}`)}}finally{this.u.log(e.Trace,"(LongPolling transport) Polling complete."),this.pollAborted||this.ae()}}async send(t){return this.ne?b(this.u,"LongPolling",this.$,this.re,t,this.ie):Promise.reject(new Error("Cannot send until the transport is connected"))}async stop(){this.u.log(e.Trace,"(LongPolling transport) Stopping polling."),this.ne=!1,this.se.abort();try{await this.he,this.u.log(e.Trace,`(LongPolling transport) sending DELETE request to ${this.re}.`);const t={},[s,n]=$();t[s]=n;const r={headers:{...t,...this.ie.headers},timeout:this.ie.timeout,withCredentials:this.ie.withCredentials};let o;try{await this.$.delete(this.re,r)}catch(t){o=t}o?o instanceof i&&(404===o.statusCode?this.u.log(e.Trace,"(LongPolling transport) A 404 response was returned from sending a DELETE request."):this.u.log(e.Trace,`(LongPolling transport) Error sending a DELETE request: ${o}`)):this.u.log(e.Trace,"(LongPolling transport) DELETE request accepted.")}finally{this.u.log(e.Trace,"(LongPolling transport) Stop finished."),this.ae()}}ae(){if(this.onclose){let t="(LongPolling transport) Firing onclose event.";this.oe&&(t+=" Error: "+this.oe),this.u.log(e.Trace,t),this.onclose(this.oe)}}}class z{constructor(t,e,s,i){this.$=t,this.Zt=e,this.u=s,this.ie=i,this.onreceive=null,this.onclose=null}async connect(t,s){return w.isRequired(t,"url"),w.isRequired(s,"transferFormat"),w.isIn(s,B,"transferFormat"),this.u.log(e.Trace,"(SSE transport) Connecting."),this.re=t,this.Zt&&(t+=(t.indexOf("?")<0?"?":"&")+`access_token=${encodeURIComponent(this.Zt)}`),new Promise(((i,n)=>{let r,o=!1;if(s===B.Text){if(g.isBrowser||g.isWebWorker)r=new this.ie.EventSource(t,{withCredentials:this.ie.withCredentials});else{const e=this.$.getCookieString(t),s={};s.Cookie=e;const[i,n]=$();s[i]=n,r=new this.ie.EventSource(t,{withCredentials:this.ie.withCredentials,headers:{...s,...this.ie.headers}})}try{r.onmessage=t=>{if(this.onreceive)try{this.u.log(e.Trace,`(SSE transport) data received. ${m(t.data,this.ie.logMessageContent)}.`),this.onreceive(t.data)}catch(t){return void this.le(t)}},r.onerror=t=>{o?this.le():n(new Error("EventSource failed to connect. The connection could not be found on the server, either the connection ID is not present on the server, or a proxy is refusing/buffering the connection. If you have multiple servers check that sticky sessions are enabled."))},r.onopen=()=>{this.u.log(e.Information,`SSE connected to ${this.re}`),this.ue=r,o=!0,i()}}catch(t){return void n(t)}}else n(new Error("The Server-Sent Events transport only supports the 'Text' transfer format"))}))}async send(t){return this.ue?b(this.u,"SSE",this.$,this.re,t,this.ie):Promise.reject(new Error("Cannot send until the transport is connected"))}stop(){return this.le(),Promise.resolve()}le(t){this.ue&&(this.ue.close(),this.ue=void 0,this.onclose&&this.onclose(t))}}class V{constructor(t,e,s,i,n,r){this.u=s,this.Yt=e,this.de=i,this.fe=n,this.$=t,this.onreceive=null,this.onclose=null,this.pe=r}async connect(t,s){let i;return w.isRequired(t,"url"),w.isRequired(s,"transferFormat"),w.isIn(s,B,"transferFormat"),this.u.log(e.Trace,"(WebSockets transport) Connecting."),this.Yt&&(i=await this.Yt()),new Promise(((n,r)=>{let o;t=t.replace(/^http/,"ws");const h=this.$.getCookieString(t);let c=!1;if(g.isNode||g.isReactNative){const e={},[s,n]=$();e[s]=n,i&&(e[W.Authorization]=`Bearer ${i}`),h&&(e[W.Cookie]=h),o=new this.fe(t,void 0,{headers:{...e,...this.pe}})}else i&&(t+=(t.indexOf("?")<0?"?":"&")+`access_token=${encodeURIComponent(i)}`);o||(o=new this.fe(t)),s===B.Binary&&(o.binaryType="arraybuffer"),o.onopen=s=>{this.u.log(e.Information,`WebSocket connected to ${t}.`),this.we=o,c=!0,n()},o.onerror=t=>{let s=null;s="undefined"!=typeof ErrorEvent&&t instanceof ErrorEvent?t.error:"There was an error with the transport",this.u.log(e.Information,`(WebSockets transport) ${s}.`)},o.onmessage=t=>{if(this.u.log(e.Trace,`(WebSockets transport) data received. ${m(t.data,this.de)}.`),this.onreceive)try{this.onreceive(t.data)}catch(t){return void this.le(t)}},o.onclose=t=>{if(c)this.le(t);else{let e=null;e="undefined"!=typeof ErrorEvent&&t instanceof ErrorEvent?t.error:"WebSocket failed to connect. The connection could not be found on the server, either the endpoint may not be a SignalR endpoint, the connection ID is not present on the server, or there is a proxy blocking WebSockets. If you have multiple servers check that sticky sessions are enabled.",r(new Error(e))}}}))}send(t){return this.we&&this.we.readyState===this.fe.OPEN?(this.u.log(e.Trace,`(WebSockets transport) sending data. ${m(t,this.de)}.`),this.we.send(t),Promise.resolve()):Promise.reject("WebSocket is not in the OPEN state")}stop(){return this.we&&this.le(void 0),Promise.resolve()}le(t){this.we&&(this.we.onclose=()=>{},this.we.onmessage=()=>{},this.we.onerror=()=>{},this.we.close(),this.we=void 0),this.u.log(e.Trace,"(WebSockets transport) socket closed."),this.onclose&&(!this.ge(t)||!1!==t.wasClean&&1e3===t.code?t instanceof Error?this.onclose(t):this.onclose():this.onclose(new Error(`WebSocket closed with status code: ${t.code} (${t.reason||"no reason given"}).`)))}ge(t){return t&&"boolean"==typeof t.wasClean&&"number"==typeof t.code}}class K{constructor(t,s={}){var i;if(this.me=()=>{},this.features={},this.ye=1,w.isRequired(t,"url"),this.u=void 0===(i=s.logger)?new E(e.Information):null===i?f.instance:void 0!==i.log?i:new E(i),this.baseUrl=this.be(t),(s=s||{}).logMessageContent=void 0!==s.logMessageContent&&s.logMessageContent,"boolean"!=typeof s.withCredentials&&void 0!==s.withCredentials)throw new Error("withCredentials option was not a 'boolean' or 'undefined' value");s.withCredentials=void 0===s.withCredentials||s.withCredentials,s.timeout=void 0===s.timeout?1e5:s.timeout;let n=null,r=null;if(g.isNode){const t=require;n=t("ws"),r=t("eventsource")}g.isNode||"undefined"==typeof WebSocket||s.WebSocket?g.isNode&&!s.WebSocket&&n&&(s.WebSocket=n):s.WebSocket=WebSocket,g.isNode||"undefined"==typeof EventSource||s.EventSource?g.isNode&&!s.EventSource&&void 0!==r&&(s.EventSource=r):s.EventSource=EventSource,this.$=new O(s.httpClient||new H(this.u),s.accessTokenFactory),this.ut="Disconnected",this.dt=!1,this.ie=s,this.onreceive=null,this.onclose=null}async start(t){if(t=t||B.Binary,w.isIn(t,B,"transferFormat"),this.u.log(e.Debug,`Starting connection with transfer format '${B[t]}'.`),"Disconnected"!==this.ut)return Promise.reject(new Error("Cannot start an HttpConnection that is not in the 'Disconnected' state."));if(this.ut="Connecting",this.ve=this.yt(t),await this.ve,"Disconnecting"===this.ut){const t="Failed to start the HttpConnection before stop() was called.";return this.u.log(e.Error,t),await this.It,Promise.reject(new r(t))}if("Connected"!==this.ut){const t="HttpConnection.startInternal completed gracefully but didn't enter the connection into the connected state!";return this.u.log(e.Error,t),Promise.reject(new r(t))}this.dt=!0}send(t){return"Connected"!==this.ut?Promise.reject(new Error("Cannot send data if the connection is not in the 'Connected' State.")):(this.Ee||(this.Ee=new G(this.transport)),this.Ee.send(t))}async stop(t){return"Disconnected"===this.ut?(this.u.log(e.Debug,`Call to HttpConnection.stop(${t}) ignored because the connection is already in the disconnected state.`),Promise.resolve()):"Disconnecting"===this.ut?(this.u.log(e.Debug,`Call to HttpConnection.stop(${t}) ignored because the connection is already in the disconnecting state.`),this.It):(this.ut="Disconnecting",this.It=new Promise((t=>{this.me=t})),await this._t(t),void await this.It)}async _t(t){this.$e=t;try{await this.ve}catch(t){}if(this.transport){try{await this.transport.stop()}catch(t){this.u.log(e.Error,`HttpConnection.transport.stop() threw error '${t}'.`),this.Ce()}this.transport=void 0}else this.u.log(e.Debug,"HttpConnection.transport is undefined in HttpConnection.stop() because start() failed.")}async yt(t){let s=this.baseUrl;this.Yt=this.ie.accessTokenFactory,this.$.Yt=this.Yt;try{if(this.ie.skipNegotiation){if(this.ie.transport!==F.WebSockets)throw new Error("Negotiation can only be skipped when using the WebSocket transport directly.");this.transport=this.Se(F.WebSockets),await this.ke(s,t)}else{let e=null,i=0;do{if(e=await this.Pe(s),"Disconnecting"===this.ut||"Disconnected"===this.ut)throw new r("The connection was stopped during negotiation.");if(e.error)throw new Error(e.error);if(e.ProtocolVersion)throw new Error("Detected a connection attempt to an ASP.NET SignalR Server. This client only supports connecting to an ASP.NET Core SignalR Server. See https://aka.ms/signalr-core-differences for details.");if(e.url&&(s=e.url),e.accessToken){const t=e.accessToken;this.Yt=()=>t,this.$.Zt=t,this.$.Yt=void 0}i++}while(e.url&&i<100);if(100===i&&e.url)throw new Error("Negotiate redirection limit exceeded.");await this.Te(s,this.ie.transport,e,t)}this.transport instanceof J&&(this.features.inherentKeepAlive=!0),"Connecting"===this.ut&&(this.u.log(e.Debug,"The HttpConnection connected successfully."),this.ut="Connected")}catch(t){return this.u.log(e.Error,"Failed to start the connection: "+t),this.ut="Disconnected",this.transport=void 0,this.me(),Promise.reject(t)}}async Pe(t){const s={},[n,r]=$();s[n]=r;const o=this.Ie(t);this.u.log(e.Debug,`Sending negotiation request: ${o}.`);try{const t=await this.$.post(o,{content:"",headers:{...s,...this.ie.headers},timeout:this.ie.timeout,withCredentials:this.ie.withCredentials});if(200!==t.statusCode)return Promise.reject(new Error(`Unexpected status code returned from negotiate '${t.statusCode}'`));const e=JSON.parse(t.content);return(!e.negotiateVersion||e.negotiateVersion<1)&&(e.connectionToken=e.connectionId),e.useStatefulReconnect&&!0!==this.ie._e?Promise.reject(new a("Client didn't negotiate Stateful Reconnect but the server did.")):e}catch(t){let s="Failed to complete negotiation with the server: "+t;return t instanceof i&&404===t.statusCode&&(s+=" Either this is not a SignalR endpoint or there is a proxy blocking the connection."),this.u.log(e.Error,s),Promise.reject(new a(s))}}He(t,e){return e?t+(-1===t.indexOf("?")?"?":"&")+`id=${e}`:t}async Te(t,s,i,n){let o=this.He(t,i.connectionToken);if(this.De(s))return this.u.log(e.Debug,"Connection was provided an instance of ITransport, using that directly."),this.transport=s,await this.ke(o,n),void(this.connectionId=i.connectionId);const h=[],a=i.availableTransports||[];let u=i;for(const i of a){const a=this.Re(i,s,n,!0===(null==u?void 0:u.useStatefulReconnect));if(a instanceof Error)h.push(`${i.transport} failed:`),h.push(a);else if(this.De(a)){if(this.transport=a,!u){try{u=await this.Pe(t)}catch(t){return Promise.reject(t)}o=this.He(t,u.connectionToken)}try{return await this.ke(o,n),void(this.connectionId=u.connectionId)}catch(t){if(this.u.log(e.Error,`Failed to start the transport '${i.transport}': ${t}`),u=void 0,h.push(new c(`${i.transport} failed: ${t}`,F[i.transport])),"Connecting"!==this.ut){const t="Failed to select transport before stop() was called.";return this.u.log(e.Debug,t),Promise.reject(new r(t))}}}}return h.length>0?Promise.reject(new l(`Unable to connect to the server with any of the available transports. ${h.join(" ")}`,h)):Promise.reject(new Error("None of the transports supported by the client are supported by the server."))}Se(t){switch(t){case F.WebSockets:if(!this.ie.WebSocket)throw new Error("'WebSocket' is not supported in your environment.");return new V(this.$,this.Yt,this.u,this.ie.logMessageContent,this.ie.WebSocket,this.ie.headers||{});case F.ServerSentEvents:if(!this.ie.EventSource)throw new Error("'EventSource' is not supported in your environment.");return new z(this.$,this.$.Zt,this.u,this.ie);case F.LongPolling:return new J(this.$,this.u,this.ie);default:throw new Error(`Unknown transport: ${t}.`)}}ke(t,e){return this.transport.onreceive=this.onreceive,this.features.reconnect?this.transport.onclose=async s=>{let i=!1;if(this.features.reconnect){try{this.features.disconnected(),await this.transport.connect(t,e),await this.features.resend()}catch{i=!0}i&&this.Ce(s)}else this.Ce(s)}:this.transport.onclose=t=>this.Ce(t),this.transport.connect(t,e)}Re(t,s,i,n){const r=F[t.transport];if(null==r)return this.u.log(e.Debug,`Skipping transport '${t.transport}' because it is not supported by this client.`),new Error(`Skipping transport '${t.transport}' because it is not supported by this client.`);if(!function(t,e){return!t||0!=(e&t)}(s,r))return this.u.log(e.Debug,`Skipping transport '${F[r]}' because it was disabled by the client.`),new h(`'${F[r]}' is disabled by the client.`,r);if(!(t.transferFormats.map((t=>B[t])).indexOf(i)>=0))return this.u.log(e.Debug,`Skipping transport '${F[r]}' because it does not support the requested transfer format '${B[i]}'.`),new Error(`'${F[r]}' does not support ${B[i]}.`);if(r===F.WebSockets&&!this.ie.WebSocket||r===F.ServerSentEvents&&!this.ie.EventSource)return this.u.log(e.Debug,`Skipping transport '${F[r]}' because it is not supported in your environment.'`),new o(`'${F[r]}' is not supported in your environment.`,r);this.u.log(e.Debug,`Selecting transport '${F[r]}'.`);try{return this.features.reconnect=r===F.WebSockets?n:void 0,this.Se(r)}catch(t){return t}}De(t){return t&&"object"==typeof t&&"connect"in t}Ce(t){if(this.u.log(e.Debug,`HttpConnection.stopConnection(${t}) called while in state ${this.ut}.`),this.transport=void 0,t=this.$e||t,this.$e=void 0,"Disconnected"!==this.ut){if("Connecting"===this.ut)throw this.u.log(e.Warning,`Call to HttpConnection.stopConnection(${t}) was ignored because the connection is still in the connecting state.`),new Error(`HttpConnection.stopConnection(${t}) was called while the connection is still in the connecting state.`);if("Disconnecting"===this.ut&&this.me(),t?this.u.log(e.Error,`Connection disconnected with error '${t}'.`):this.u.log(e.Information,"Connection disconnected."),this.Ee&&(this.Ee.stop().catch((t=>{this.u.log(e.Error,`TransportSendQueue.stop() threw error '${t}'.`)})),this.Ee=void 0),this.connectionId=void 0,this.ut="Disconnected",this.dt){this.dt=!1;try{this.onclose&&this.onclose(t)}catch(s){this.u.log(e.Error,`HttpConnection.onclose(${t}) threw error '${s}'.`)}}}else this.u.log(e.Debug,`Call to HttpConnection.stopConnection(${t}) was ignored because the connection is already in the disconnected state.`)}be(t){if(0===t.lastIndexOf("https://",0)||0===t.lastIndexOf("http://",0))return t;if(!g.isBrowser)throw new Error(`Cannot resolve '${t}'.`);const s=window.document.createElement("a");return s.href=t,this.u.log(e.Information,`Normalizing '${t}' to '${s.href}'.`),s.href}Ie(t){const e=new URL(t);e.pathname.endsWith("/")?e.pathname+="negotiate":e.pathname+="/negotiate";const s=new URLSearchParams(e.searchParams);return s.has("negotiateVersion")||s.append("negotiateVersion",this.ye.toString()),s.has("useStatefulReconnect")?"true"===s.get("useStatefulReconnect")&&(this.ie._e=!0):!0===this.ie._e&&s.append("useStatefulReconnect","true"),e.search=s.toString(),e.toString()}}class G{constructor(t){this.xe=t,this.Ae=[],this.Ue=!0,this.Le=new Q,this.Ne=new Q,this.qe=this.Me()}send(t){return this.je(t),this.Ne||(this.Ne=new Q),this.Ne.promise}stop(){return this.Ue=!1,this.Le.resolve(),this.qe}je(t){if(this.Ae.length&&typeof this.Ae[0]!=typeof t)throw new Error(`Expected data to be of type ${typeof this.Ae} but was of type ${typeof t}`);this.Ae.push(t),this.Le.resolve()}async Me(){for(;;){if(await this.Le.promise,!this.Ue){this.Ne&&this.Ne.reject("Connection stopped.");break}this.Le=new Q;const t=this.Ne;this.Ne=void 0;const e="string"==typeof this.Ae[0]?this.Ae.join(""):G.We(this.Ae);this.Ae.length=0;try{await this.xe.send(e),t.resolve()}catch(e){t.reject(e)}}}static We(t){const e=t.map((t=>t.byteLength)).reduce(((t,e)=>t+e)),s=new Uint8Array(e);let i=0;for(const e of t)s.set(new Uint8Array(e),i),i+=e.byteLength;return s.buffer}}class Q{constructor(){this.promise=new Promise(((t,e)=>[this.j,this.Oe]=[t,e]))}resolve(){this.j()}reject(t){this.Oe(t)}}class Y{constructor(){this.name="json",this.version=2,this.transferFormat=B.Text}parseMessages(t,s){if("string"!=typeof t)throw new Error("Invalid input for JSON hub protocol. Expected a string.");if(!t)return[];null===s&&(s=f.instance);const i=D.parse(t),n=[];for(const t of i){const i=JSON.parse(t);if("number"!=typeof i.type)throw new Error("Invalid payload.");switch(i.type){case x.Invocation:this.U(i);break;case x.StreamItem:this.Fe(i);break;case x.Completion:this.Be(i);break;case x.Ping:case x.Close:break;case x.Ack:this.Xe(i);break;case x.Sequence:this.Je(i);break;default:s.log(e.Information,"Unknown message type '"+i.type+"' ignored.");continue}n.push(i)}return n}writeMessage(t){return D.write(JSON.stringify(t))}U(t){this.ze(t.target,"Invalid payload for Invocation message."),void 0!==t.invocationId&&this.ze(t.invocationId,"Invalid payload for Invocation message.")}Fe(t){if(this.ze(t.invocationId,"Invalid payload for StreamItem message."),void 0===t.item)throw new Error("Invalid payload for StreamItem message.")}Be(t){if(t.result&&t.error)throw new Error("Invalid payload for Completion message.");!t.result&&t.error&&this.ze(t.error,"Invalid payload for Completion message."),this.ze(t.invocationId,"Invalid payload for Completion message.")}Xe(t){if("number"!=typeof t.sequenceId)throw new Error("Invalid SequenceId for Ack message.")}Je(t){if("number"!=typeof t.sequenceId)throw new Error("Invalid SequenceId for Sequence message.")}ze(t,e){if("string"!=typeof t||""===t)throw new Error(e)}}const Z={trace:e.Trace,debug:e.Debug,info:e.Information,information:e.Information,warn:e.Warning,warning:e.Warning,error:e.Error,critical:e.Critical,none:e.None};class tt{configureLogging(t){if(w.isRequired(t,"logging"),void 0!==t.log)this.logger=t;else if("string"==typeof t){const e=function(t){const e=Z[t.toLowerCase()];if(void 0!==e)return e;throw new Error(`Unknown log level: ${t}`)}(t);this.logger=new E(e)}else this.logger=new E(t);return this}withUrl(t,e){return w.isRequired(t,"url"),w.isNotEmpty(t,"url"),this.url=t,this.httpConnectionOptions="object"==typeof e?{...this.httpConnectionOptions,...e}:{...this.httpConnectionOptions,transport:e},this}withHubProtocol(t){return w.isRequired(t,"protocol"),this.protocol=t,this}withAutomaticReconnect(t){if(this.reconnectPolicy)throw new Error("A reconnectPolicy has already been set.");return t?Array.isArray(t)?this.reconnectPolicy=new j(t):this.reconnectPolicy=t:this.reconnectPolicy=new j,this}withServerTimeout(t){return w.isRequired(t,"milliseconds"),this.Ve=t,this}withKeepAliveInterval(t){return w.isRequired(t,"milliseconds"),this.Ke=t,this}withStatefulReconnect(t){return void 0===this.httpConnectionOptions&&(this.httpConnectionOptions={}),this.httpConnectionOptions._e=!0,this.Y=null==t?void 0:t.bufferSize,this}build(){const t=this.httpConnectionOptions||{};if(void 0===t.logger&&(t.logger=this.logger),!this.url)throw new Error("The 'HubConnectionBuilder.withUrl' method must be called before building the connection.");const e=new K(this.url,t);return q.create(e,this.logger||f.instance,this.protocol||new Y,this.reconnectPolicy,this.Ve,this.Ke,this.Y)}}return Uint8Array.prototype.indexOf||Object.defineProperty(Uint8Array.prototype,"indexOf",{value:Array.prototype.indexOf,writable:!0}),Uint8Array.prototype.slice||Object.defineProperty(Uint8Array.prototype,"slice",{value:function(t,e){return new Uint8Array(Array.prototype.slice.call(this,t,e))},writable:!0}),Uint8Array.prototype.forEach||Object.defineProperty(Uint8Array.prototype,"forEach",{value:Array.prototype.forEach,writable:!0}),s})(),"object"==typeof exports&&"object"==typeof module?module.exports=e():"function"==typeof define&&define.amd?define([],e):"object"==typeof exports?exports.signalR=e():t.signalR=e();
//# sourceMappingURL=signalr.min.js.map
```

