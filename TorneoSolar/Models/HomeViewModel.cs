using System.Collections.Generic;

namespace TorneoSolar.Models
{
    public class HomeViewModel
    {
        public IEnumerable<Noticias> Noticias { get; set; }
        public IEnumerable<Partido> UltimosResultados { get; set; }
    }
}
