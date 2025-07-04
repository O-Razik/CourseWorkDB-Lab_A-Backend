﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Lab_A.Abstraction.IModels;
using Microsoft.EntityFrameworkCore;

namespace Lab_A.DAL.Models;

[Table("City")]
public partial class City : ICity
{
    [Key]
    [Column("city_id")]
    public int CityId { get; set; }

    [Column("city", TypeName = "nvarchar(255)")]
    [StringLength(255)]
    public string CityName { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdateDatetime { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreateDatetime { get; set; }

    [InverseProperty("City")]
    public virtual ICollection<AnalysisCenter> AnalysisCenters { get; set; } = new List<AnalysisCenter>();
    
    [InverseProperty("City")]
    public virtual ICollection<Laboratory> Laboratories { get; set; } = new List<Laboratory>();

    ICollection<IAnalysisCenter> ICity.AnalysisCenters
    {
        get => AnalysisCenters.Cast<IAnalysisCenter>().ToList();
        set => AnalysisCenters = value.Cast<AnalysisCenter>().ToList();
    }

    ICollection<ILaboratory> ICity.Laboratories
    {
        get => Laboratories.Cast<ILaboratory>().ToList();
        set => Laboratories = value.Cast<Laboratory>().ToList();
    }
}