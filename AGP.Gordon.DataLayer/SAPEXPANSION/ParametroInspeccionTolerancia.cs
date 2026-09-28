using System;
using System.Collections.Generic;


namespace AGP.Gordon.DataAccessLayer.SAPEXPANSION
{
    public class ParametroInspeccionTolerancia
    {
        public int Id { get; set; }
        public string? Zfer { get; set; }
        public int ParametroInspeccionId { get; set; }
        public byte NroValor { get; set; }
        public string? Formula { get; set; }
        public string TipoTolerancia { get; set; } = "NA";
        public decimal? Nominal { get; set; }
        public decimal? TolMas { get; set; }
        public decimal? TolMenos { get; set; }
        public decimal? Minimo { get; set; }
        public decimal? Maximo { get; set; }
        public bool Activo { get; set; } = true;
    }
}
