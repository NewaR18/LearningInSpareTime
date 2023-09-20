﻿using LearnJWT.Model;
using Microsoft.EntityFrameworkCore;

namespace LearnJWT.AppDbContext
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Users> Users { get; set; }
        public DbSet<JwtUser> JwtUsers { get; set; }
    }
}
