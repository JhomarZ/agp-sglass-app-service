using AGP.Gordon.DataAccessLayer.SAPEXPANSION;
using System.Globalization;

namespace AGP.Gordon.ServiceLayer;

public record ToleranciaValor(string? Formula, string Tipo, decimal? Nominal,
                                 decimal? TolMas, decimal? Min, decimal? Max)
{
    public static ToleranciaValor From(ParametroInspeccionTolerancia e) =>
    new(e.Formula, e.TipoTolerancia, e.Nominal, e.TolMas, e.Minimo, e.Maximo);

            // Lo que se imprime: Formula manda; si está vacía se arma desde las columnas
    public string Display => !string.IsNullOrWhiteSpace(Formula) ? Formula.Trim() : Calculada;

            public string Calculada => Tipo switch
            {
                "SIMETRICA" => $"{F(Nominal)}±{F(TolMas)}",
                "NOMINAL" => F(Nominal),
                "MAXIMO" => $"<{F(Max)}",
                "MINIMO" => $">{F(Min)}",
                "RANGO" => $"{F(Min)}-{F(Max)}",
                _ => "-"
            };

    static string F(decimal? v) =>
                v?.ToString("0.##", CultureInfo.InvariantCulture).Replace('.', ',') ?? "";
}




