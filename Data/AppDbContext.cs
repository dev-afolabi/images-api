﻿using FletcherProj.Models;
using Microsoft.EntityFrameworkCore;

namespace FletcherProj.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Images> Images { get; set; }
    }
}
