﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Lab_A.Abstraction.IModels;
using Microsoft.EntityFrameworkCore;

namespace Lab_A.DAL.Models;

[Table("OrderAnalysis")]
public partial class OrderAnalysis : IOrderAnalysis
{
    [Key]
    [Column("order_analysis_id")]
    public int OrderAnalysisId { get; set; }

    [Column("analysis_id")]
    public int? AnalysisId { get; set; }

    [Column("client_order_id")]
    public int? ClientOrderId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdateDatetime { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreateDatetime { get; set; }

    [ForeignKey("AnalysisId")]
    [InverseProperty("OrderAnalyses")]
    public virtual Analysis Analysis { get; set; }

    [InverseProperty("OrderAnalysis")]
    public virtual ICollection<AnalysisResult> AnalysisResults { get; set; } = new List<AnalysisResult>();

    [ForeignKey("ClientOrderId")]
    [InverseProperty("OrderAnalyses")]
    public virtual ClientOrder ClientOrder { get; set; }

    IAnalysis IOrderAnalysis.Analysis
    {
        get => Analysis;
        set => Analysis = (Analysis)value;
    }

    ICollection<IAnalysisResult> IOrderAnalysis.AnalysisResults
    {
        get => AnalysisResults.Cast<IAnalysisResult>().ToList();
        set => AnalysisResults = value.Cast<AnalysisResult>().ToList();
    }

    IClientOrder IOrderAnalysis.ClientOrder
    {
        get => ClientOrder;
        set => ClientOrder = (ClientOrder)value;
    }
}