using System;
using System.Collections.Generic;

namespace AGP.Gordon.DataAccessLayer.SAPEXPANSION;

public partial class BallisticTest
{
    public int Id { get; set; }

    public string? PreparedFor { get; set; }

    public string? Formulas { get; set; }

    public string? GlassTransparency { get; set; }

    public string? TestSpecification { get; set; }

    public string? ProjectileCaliber { get; set; }

    public string? AmmunitionWheight { get; set; }

    public string? RequiredBulletVelocity { get; set; }

    public int? GunId { get; set; }

    public string? ShootingPattern { get; set; }

    public string? TestDistance { get; set; }

    public string? Conditioning { get; set; }

    public string? AmbientTemperature { get; set; }

    public string? RelativeHumidity { get; set; }

    public string? AverageThickness { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? UpdatedBy { get; set; }

    public string? Gun { get; set; }

    public int? IdCompania { get; set; }

    public byte? ImagenFs { get; set; }

    public byte? ImagenBs { get; set; }

    public byte? ImagenWi { get; set; }

    public byte? Activo { get; set; }

    public string? Probeta { get; set; }

    public decimal? VelocidadA { get; set; }

    public decimal? VelocidadB { get; set; }

    public decimal? VelocidadC { get; set; }

    public decimal? VelocidadD { get; set; }

    public decimal? VelocidadE { get; set; }

    public string? EfectoA { get; set; }

    public string? EfectoB { get; set; }

    public string? EfectoC { get; set; }

    public string? EfectoD { get; set; }

    public string? EfectoE { get; set; }

    public byte? ImagenProf { get; set; }

    public byte? ImagenWbi { get; set; }

    public byte? ImagenWai { get; set; }
}
