using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Repozytorium.Models
{
    public class Kategoria
    {
        public Kategoria()
        {
            this.Ogloszenie_Kategoria = new HashSet<Ogloszenie_Kategoria>();
        }

        [Key]
        [Display(Name = "Id kategorii: ")]
        public int Id { get; set; }

        [Display(Name= "Nazwa kategorii: ")]
        [Required]
        public string Nazwa { get; set; }

        [Display(Name = "Id rodzica: ")]
        [Required]
        public int ParentId { get; set; }


        #region SEO

        [Display(Name = "Tytuł strony: ")]
        [MaxLength(72)]
        public string MetaTytul { get; set; }

        [Display(Name ="Opis strony: ")]
        [MaxLength(160)]
        public string Metaopis { get; set; }

        [Display(Name ="Słowa kluczowe: ")]
        [MaxLength(160)]
        public string MetaSlowa { get; set; }

        [Display(Name ="Treść strony: ")]
        public string Tresc { get; set; }

        #endregion

        public ICollection<Ogloszenie_Kategoria> Ogloszenie_Kategoria { get; set; }
    }
}