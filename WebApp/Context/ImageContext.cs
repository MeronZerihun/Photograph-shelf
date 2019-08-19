using System;
using Microsoft.EntityFrameworkCore;

namespace WebApp
{
    public class ImageContext:DbContext
    {
        public ImageContext(DbContextOptions<ImageContext> options) : base(options)
        {
           
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
        public virtual DbSet<Image> Images { get; set; }
    }
}
