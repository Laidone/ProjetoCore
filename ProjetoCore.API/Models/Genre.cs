using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjetoCore.API.Models
{
    public class Genre
    {
        public int GenreID { get; set; }

        [Required]
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public virtual ICollection<Movie> Movies { get; set; }

        //public int GenreID { get; set; }
        //public string Name { get; set; }
        //public string Description { get; set; }

        //public virtual ICollection<Movie> Movies { get; set; }
    }
}