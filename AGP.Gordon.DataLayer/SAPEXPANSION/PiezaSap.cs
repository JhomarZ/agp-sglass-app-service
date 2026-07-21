using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AGP.Gordon.DataAccessLayer.SAPEXPANSION;

public partial class PiezaSap
{
    public int Id { get; set; }

    public int? CertificadoId { get; set; }

    public string? OrdProceso { get; set; }

    public string? Cliente { get; set; }

    public string? Vehiculo { get; set; }

    public string? Pieza { get; set; }

    public string? BombaA { get; set; }

    public string? TolBombaA { get; set; }

    public string? BombaB { get; set; }

    public string? TolBombaB { get; set; }

    public string? BombaC { get; set; }

    public string? TolBombaC { get; set; }

    public string? BombaD { get; set; }

    public string? TolBombaD { get; set; }

    public string? Formula { get; set; }

    public string? Ruta { get; set; }

    public string? Modelo { get; set; }

    public string? Vidrio { get; set; }

    public string? Espesor { get; set; }

    public string? Ocn { get; set; }

    public string? Resistencia { get; set; }

    public string? ResponsableFt { get; set; }

    public string? Perimetro { get; set; }

    public string? Area { get; set; }

    public string? Nivel { get; set; }

    public string? Color { get; set; }

    public string? Logo { get; set; }

    public string? MedCurvatura { get; set; }

    public DateTime FechaCrea { get; set; }

    public DateTime? FechaEdita { get; set; }

    public string? RutaImagen { get; set; }

    public string? TipoPedido { get; set; }

    public string? LoteLogistico { get; set; }

    public string? ImagenFt { get; set; }

    public string? Modulo { get; set; }

    public string? Zfer { get; set; }

    public int? IdCompania { get; set; }

    public byte? TerminadoDimensional { get; set; }

    public byte? TerminadoApariencia { get; set; }

    public byte? TerminadoOptico { get; set; }

    public string? Laeng { get; set; }

    public string? Breit { get; set; }

    public string? Matnr01 { get; set; }

    public string? Matnr02 { get; set; }

    public string? NroPedidoUro { get; set; }

    public string? CodigoImagenTecnica { get; set; }

    public string? Altura { get; set; }

    public string? PartNumber { get; set; }

    public string? Documento { get; set; }

    public string? CodigoImagenStandar { get; set; }

    public int? UsuarioCompania { get; set; }

    public string? UsuarioCrea { get; set; }

    [NotMapped]
    public string? DefectoImagen { get; set; }

    [NotMapped]
    public byte[] DefectoImagenByte { get; set; }

    [NotMapped]
    public string? IMAGEN_PLANO_STANDAR { get; set; }

    public string GetSuplier()
    {
        string suplier = "";
        switch (this.UsuarioCompania)
        {
            case 1001:
                suplier = "AGP PERU S.A.C."; break;
            case 1002:
                suplier = "AGP COLOMBIA"; break;
            case 1003:
                suplier = "AGP BRAZIL"; break;
        }
        return suplier;
    }

    public string GetSuplierAddress()
    {
        string suplier = "";
        switch (this.UsuarioCompania)
        {
            case 1001:
                suplier = "AV. GUILLERMO DANSEY #2016, CERCADO DE LIMA. LIMA - PERÚ"; break;
            case 1002:
                suplier = "Cl. 15 ##35-59, Bogotá, Colombia"; break;
            case 1003:
                suplier = "Alameda Arpo, 2751 - São José dos Pinhais/PR - Brazil"; break;
        }
        return suplier;
    }
    public string GetQualityEngineer()
    {
        string suplier = "";
        switch (this.UsuarioCompania)
        {
            case 1001:
                suplier = "Elena Benjarano"; break;
            case 1002:
                suplier = "Lina Tovar"; break;
            case 1003:
                suplier = "Jeovane Rocha"; break;
        }
        return suplier;
    }
    public string GetQualityManager()
    {
        string suplier = "";
        switch (this.UsuarioCompania)
        {
            case 1001:
                suplier = "Adriano dos Santos"; break;
            case 1002:
                suplier = "Elena Bejarano"; break;
            case 1003:
                suplier = "Lucas Kracker"; break; 
        } 
        return suplier;
    }

    public string GetPlantNameOrigen()
    {
        string plantName = "";
        switch (this.UsuarioCompania)
        {
            case 1001:
                plantName = "PERU"; break;
            case 1002:
                plantName = "COLOMBIA"; break;
            case 1003:
                plantName = "BRASIL"; break;
        }
        return plantName;
    }

  

}
